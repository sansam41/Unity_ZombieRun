using UnityEngine;

// 게임 클리어, 게임 오버, 점수와 코인
// 던전 내 다음 방으로 이동 하기위해 문을 열고 닫는 게임 매니저
// 다른 클래스에서 접근하기 쉽도록 싱글톤 패턴 사용
public class GameManager : MonoBehaviour {
    // 싱글톤 접근용 프로퍼티
    public static GameManager instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<GameManager>();
            }

            // 싱글톤 오브젝트를 반환
            return m_instance;
        }
    }

    private static GameManager m_instance; // 싱글톤이 할당될 static 변수

    public GameObject[] fence=new GameObject[2]; //다음 방을 열기 위한 문
    public GameObject[] fenceIcon = new GameObject[2];  //미니맵에 문의 위치를 표시하기 위한 아이콘
    public PlayerMovement playerMovement;

    private int score = 0; // 현재 게임 점수
    public bool isGameover { get; private set; } // 게임 오버 상태

    private void Awake() {
        // 씬에 싱글톤 오브젝트가 된 다른 GameManager 오브젝트가 있다면
        if (instance != this)
        {
            // 자신을 파괴
            Destroy(gameObject);
        }

    }

    private void Start() {
        // 플레이어 캐릭터의 사망 이벤트 발생시 게임 오버
        FindObjectOfType<PlayerHealth>().onDeath += EndGame;

        // 플레이어 돈 불러오기
        if (PlayerPrefs.HasKey("Coin"))
            UIManager.instance.UpdateCoinText();
        else
        {
            PlayerPrefs.SetInt("Coin", 0);
            PlayerPrefs.Save();
            UIManager.instance.UpdateCoinText();
        }
    }

    private void Update()
    {
    }

    // 점수를 추가하고 UI 갱신
    public void AddScore(int newScore) {
        // 게임 오버가 아닌 상태에서만 점수 증가 가능
        if (!isGameover)
        {
            // 점수 추가
            score += newScore;
            // 점수 UI 텍스트 갱신
            UIManager.instance.UpdateScoreText(score);
        }
    }

    //스코어 리턴
    public int getScore() {
        return score;
    }


    //다음 방을 열거나 닫기위한 메소드
    public int ControlFence(int i,int Open)
    {
        //Open==0 : 문을 닫는다
        //Open==1 : 문을 연다
        //Open==2 : 현재 상태 유지
        if (Open==1)
        {
            Vector3 movePosition = Vector3.zero;
            Vector3 velo = Vector3.zero;
            movePosition.Set(fence[i].transform.position.x, -3f, fence[i].transform.position.z);
            fence[i].transform.position = Vector3.SmoothDamp(fence[i].transform.position, movePosition, ref velo, 0.4f);    //문을 천천히 연다
            fenceIcon[i].SetActive(true);   //다음방 문이 열린 상태이므로 미니맵에 아이콘 표시

            if (fence[i].transform.position.y <= -1&& -2 < fence[i].transform.position.y)   //문이 거의 다 열렸으므로 플레이어를 다시 비춘다
            {
                CameraManager.instance.MainCameraOn();
            }
            else if (fence[i].transform.position.y <= -2) {     //문이 다 열렸으므로 유지 상태 돌입
                return 2;
            }
            else
            {
                CameraManager.instance.FenceCameraOn(i);    //다음문이 열렸다는 것을 플레이어에게 컷신을 통해 알려주기 위해 카메라 변경
            }
        }
        else if(Open==0)
        {
            playerMovement.playerStop();    //문이 다 닫히기 전에 플레이어가 이전 방으로 돌아가는 것을 방지하기 위해 플레이어 컨트롤을 멈춤

            Vector3 movePosition = Vector3.zero;
            Vector3 velo = Vector3.zero;

            movePosition.Set(fence[i].transform.position.x, 3f, fence[i].transform.position.z); //문을 천천히 닫는다

            fence[i].transform.position = Vector3.SmoothDamp(fence[i].transform.position, movePosition, ref velo, 0.4f);

            if (fence[i].transform.position.y >= 0) //문이 다 닫혔으므로 유지 상태 돌입
            {
                fenceIcon[i].SetActive(false);  //아이콘을 끈다
                playerMovement.playerOn();  //플레이어 움직임을 활성화 한다.
                return 2;
            }
        }

        return Open;    //아직 현재 상태가 진행중 이므로 현재상태 Return
    }

    // 게임 오버 처리
    public void EndGame() {
        // 게임 오버 상태를 참으로 변경
        isGameover = true;
        // 게임 오버 UI를 활성화
        UIManager.instance.SetActiveGameoverUI(true);

        int newCoin = getScore() + PlayerPrefs.GetInt("Coin");
        PlayerPrefs.SetInt("Coin", newCoin);
        PlayerPrefs.Save();
    }

    public void ClearGame()
    {
        //게임 클리어 UI 활성화
        UIManager.instance.clearUI.SetActive(true);

        int ScoreToCoin = (int)(getScore() * 0.1f);

        //스코어를 돈으로 환산하여 저장
        int newCoin = ScoreToCoin + PlayerPrefs.GetInt("Coin");

        PlayerPrefs.SetInt("Coin", newCoin);
        PlayerPrefs.Save();
    }
}