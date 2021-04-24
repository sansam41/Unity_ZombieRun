using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//씬 변경시 사용하는 포탈 스크립트
public class SceneManagement : MonoBehaviour
{
    PlayerInput playerInput;
    bool Ok = false;

    private void OnTriggerStay(Collider other)
    {
        //콜리더에 사용자가 접근했을 경우
        if (other.tag == "Player")
        {
            UIManager.instance.SetActivePotalInteractionUI(true);//안내 UI true
            playerInput = other.GetComponent<PlayerInput>();
            if (playerInput.KeyDown_F)//사용자가 콜리더에 접근한 상태에서 F키를 눌렀을 경우
            {
                //사용자의 움직임을 멈추고 다른 씬으로 넘어가는 메소드 실행
                Ok = true;
                PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
                playerMovement.playerStop();
            }
        }
        if (Ok)
            ChangeScene();
    }
    private void OnTriggerExit(Collider other) {
        UIManager.instance.SetActivePotalInteractionUI(false);
    }

    //던전 씬으로 이동
    private void ChangeScene() {
        UIManager.instance.SetActivePanel(true);
        for (int i = 0; i < 100; i++)
            if (UIManager.instance.GetAlpha() >= 1)
                if (SceneManager.GetActiveScene().name == "HomeTown")
                {
                    SceneManager.LoadScene("Main");
                    break;
                }
    }
}
