using UnityEngine;

// 주어진 Gun 오브젝트를 쏘거나 재장전
// 알맞은 애니메이션을 재생하고 IK를 사용해 캐릭터 양손이 총에 위치하도록 조정
public class PlayerShooter : MonoBehaviour {
    //public Gun gun;
    public Gun_Uzi Uzi; // 사용할 총
    public Gun_RocketLauncher RocketLauncher; //사용할 로켓런처


    public Transform gunPivot; // 총 배치의 기준점
    public Transform leftHandMount; // 총의 왼쪽 손잡이, 왼손이 위치할 지점
    public Transform rightHandMount; // 총의 오른쪽 손잡이, 오른손이 위치할 지점

    private PlayerInput playerInput; // 플레이어의 입력
    private Animator playerAnimator; // 애니메이터 컴포넌트
    private PlayerMovement playerMovement;
    public int gunType;

    private void Start() {
        // 사용할 컴포넌트들을 가져오기
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        gunType=PlayerPrefs.GetInt("GunType");  //GunType 
                                                //1:Uzi, 2:RocketLauncher
    }

    private void OnEnable() {
        // 슈터가 활성화될 때 총도 함께 활성화
        if(gunType==1)
            Uzi.gameObject.SetActive(true);
        else
            RocketLauncher.gameObject.SetActive(true);
    }
    
    private void OnDisable() {
        // 슈터가 비활성화될 때 총도 함께 비활성화
        if (gunType == 1)
            Uzi.gameObject.SetActive(false);
        else
            RocketLauncher.gameObject.SetActive(false);
    }

    private void Update()
    {
        gunType = PlayerPrefs.GetInt("GunType");

        gunActive();

        // 입력을 감지하고 총 발사하거나 재장전
        if (playerInput.fire)
        {
            if (playerMovement.enabled)
            {
                playerMovement.LookAtTarget();
                Invoke("delayfuntion", 0.1f);
        }
    }
       else if (playerInput.reload)
        {
            //RocketLauncher일 경우 자동 재장전 이므로 Uzi일 경우에만 실행
            if (gunType == 1)
                if (Uzi.Reload())
                {
                    playerAnimator.SetTrigger("Reload");
                }

        }
        //남은 탄알 UI갱신
        UpdateUI();
    }

    // 탄약 UI 갱신
    private void UpdateUI() {
        if (gunType == 1)
            if (Uzi != null && UIManager.instance != null)
            {
                // UI 매니저의 탄약 텍스트에 탄창의 탄약과 남은 전체 탄약을 표시
                UIManager.instance.UpdateAmmoText(Uzi.magAmmo, Uzi.ammoRemain);
            }
        else
            if (RocketLauncher != null && UIManager.instance != null)
            {
                // UI 매니저의 탄약 텍스트에 탄창의 탄약과 남은 전체 탄약을 표시
                //UIManager.instance.UpdateAmmoText(gun2.magAmmo, gun2.ammoRemain);
            }
    }

    // 애니메이터의 IK 갱신
    private void OnAnimatorIK(int layerIndex) {
        //총의 기준점 gunPivot을 3D AVATA모델의 오른쪽 팔꿈치 위치로 이동
        gunPivot.position = playerAnimator.GetIKHintPosition(AvatarIKHint.RightElbow);

        //IK를 사용하여 왼손의 위치와 회전을 총의 왼쪽 손잡이에 맞춤
        playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f); //위치에대한 가중치는 100%
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f); //회전에대한 가중치는 100%

        playerAnimator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandMount.position);
        playerAnimator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandMount.rotation);

        //IK를 사용하여 왼손의 위치와 회전을 총의 오른쪽 손잡이에 맞춤
        playerAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f); //위치에대한 가중치는 100%
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f); //회전에대한 가중치는 100%


        playerAnimator.SetIKPosition(AvatarIKGoal.RightHand, rightHandMount.position);
        playerAnimator.SetIKRotation(AvatarIKGoal.RightHand, rightHandMount.rotation);
    }

    //gunType에 알맞는 총을 액티브
    private void gunActive() {
        if (gunType == 1)
        {
            Uzi.gameObject.SetActive(true);
            RocketLauncher.gameObject.SetActive(false);
        }
        else
        {
            RocketLauncher.gameObject.SetActive(true);
            Uzi.gameObject.SetActive(false);
        }
    }

    //gunType에 알맞는 Fire 실행
    private void delayfuntion() {
        if(playerMovement.enabled)
        if (gunType == 1)
        {
                Uzi.Fire();
        }
        else
        {
                RocketLauncher.Fire();
        }
    }
}