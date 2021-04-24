using UnityEngine;
using System;
using System.Collections;
// 플레이어 캐릭터를 사용자 입력에 따라 움직이는 스크립트
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

            //플레이어가 다른 방향으로 이동 시에 애니메이션이 멈추는 경우를 방지
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
        movement = movement.normalized * moveSpeed * Time.deltaTime;

        Quaternion newRotation = Quaternion.LookRotation(movement);

        playerRigidbody.rotation = Quaternion.Slerp(playerRigidbody.rotation, newRotation, moveSpeed * Time.deltaTime);
        playerRigidbody.MovePosition(transform.position + movement);
    }

    //
    public void LookAtTarget() {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))//카메라상에서의 마우스의 좌표
        {
            Vector3 mousePoint = hit.point;
            Vector3 playerLook = Vector3.zero;
            playerLook.Set(mousePoint.x, playerRigidbody.position.y, mousePoint.z);//플레이어가 보는 높이는 고정되어야 한다.


            Vector3 dir = mousePoint - transform.position;
            float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            Quaternion newRotation= Quaternion.Euler(0, angle, 0);
            playerRigidbody.rotation = Quaternion.Slerp(playerRigidbody.rotation, newRotation, rotateSpeed * Time.deltaTime);
        }


    }

    public void playerOn() {
        Invoke("On", 0.2f);
    }

    public void playerStop()
    {
        playerAnimator.SetFloat("Move", 0);
        playerMovement.enabled = false;
    }
    public void goodbye()
    {
        UIManager.instance.SetActiveNpcTalkBox(false);
        Invoke("On", 0.1f);

    }

    public void On()
    {
        playerMovement.enabled = true;
    }
}