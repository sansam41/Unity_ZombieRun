using UnityEngine;
using System;
using System.Collections;
// 플레이어 캐릭터의 움직임을 조종하는 스크립트
public class PlayerMovement : MonoBehaviour {
    public float moveSpeed = 180f; // 앞뒤 움직임의 속도
    public float rotateSpeed = 180f; // 좌우 회전 속도
    //마우스 움직임

    private PlayerInput playerInput; // 플레이어 입력을 알려주는 컴포넌트
    private Rigidbody playerRigidbody; // 플레이어 캐릭터의 리지드바디
    private Animator playerAnimator; // 플레이어 캐릭터의 애니메이터
    private PlayerMovement playerMovement;

    private void Start() {
        // 사용할 컴포넌트들의 참조를 가져오기
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
}

    // FixedUpdate는 물리 갱신 주기에 맞춰 실행됨
    private void FixedUpdate() {
        // 물리 갱신 주기마다 움직임, 회전, 애니메이션 처리 실행
        if (playerInput.fire)
        { 
            playerAnimator.SetFloat("Move", 0);//총 발사 중에는 애니메이션을 멈춘다.
        }
        else
        {
            SmoothMove();

            //플레이어가 다른 방향으로 이동 시에 애니메이션이 멈추는 경우를 방지(플레이어가 방향을 틀 경우 움직인은 상태이나 속도가 0으로 줄어 애니매이션이 정지)
            if (Math.Abs(playerInput.move) > Math.Abs(playerInput.rotate))
                playerAnimator.SetFloat("Move", playerInput.move);
            else
                playerAnimator.SetFloat("Move", playerInput.rotate);
        }

    }

    //WASD버튼 이동
    private void SmoothMove() {
        float xInput = playerInput.move;
        float zInput = playerInput.rotate * -1;

        if (xInput == 0 && zInput == 0)
            return;

        Vector3 movement=Vector3.zero;
        movement.Set(xInput, 0, zInput);
        movement = movement.normalized * moveSpeed * Time.deltaTime;//화면 프레임에 비례하여 플레이어의 속도 및 움직임 조절

        Quaternion newRotation = Quaternion.LookRotation(movement);//플레이어가 바라봐야할 방향을 설정

        playerRigidbody.rotation = Quaternion.Slerp(playerRigidbody.rotation, newRotation, moveSpeed * Time.deltaTime);//플레이어캐릭터가 비자연스럽게 회전하는 것을 방지
        playerRigidbody.MovePosition(transform.position + movement);//플레이어 캐릭터가 방향키의 방향으로 정해진 속도만큼 움직임
    }

    //클릭시 그 방향을 바라보도록
    public void LookAtTarget() {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))//카메라상에서의 마우스의 좌표
        {
            Vector3 mousePoint = hit.point;


            Vector3 dir = mousePoint - transform.position; //플레이어가 마우스포인터를 바라보는 방향을 구함 
            float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;//방향에 대한 절대각도를 구함
            Quaternion newRotation= Quaternion.Euler(0, angle, 0);//Vector3 절대각도를 Quaternion으로 변환
            playerRigidbody.rotation = Quaternion.Slerp(playerRigidbody.rotation, newRotation, rotateSpeed * Time.deltaTime);//캐릭터가 해당 방향을 바라보도록 설정
        }


    }


    public void playerOn() {
        UIManager.instance.SetActiveNpcTalkBox(false);
        Invoke("On", 0.2f);//버튼 클릭시 총이 발사되는 것을 방지하기 위해 on을 invoke로 호출해준다.
    }

    public void playerStop()
    {
        //npc와 대화를 하거나 던전 문이 열리거나 닫힐 때 플레이어의 움직임을 잠시 멈춘다.
        playerAnimator.SetFloat("Move", 0);
        playerMovement.enabled = false;
    }

    //invoke호출을 위해 플레이어의 움직임을 true로 해주는 메소드를 따로 선언
    public void On()
    {
        playerMovement.enabled = true;
    }
}