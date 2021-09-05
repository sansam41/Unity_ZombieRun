using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public enum TutorialType
    {
        Control,
        WEAPON,
        POTAL,
        NPC,
        END

    }
    public GameObject       TutorialUI;
    public GameObject       MainCam;
    public GameObject       TutoCam;

    public Transform        WeaponCamPos;
    public Transform        PotalCamPos;
    public Transform        NPCCamPos;

    public Text             Title;
    public Text             Tuto;
    public Text             ButtonText;

    public GameObject       OkButton;


    public PlayerMovement   playerMovement;

    private int cnt=-1;


    // Start is called before the first frame update
    void Start()
    {
        MainCam.SetActive(true);
        TutoCam.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextTutorial() {
        playerMovement.playerStop();
        TutorialUI.SetActive(true);
        ButtonText.text = "다음";
        cnt++;
        if (cnt == (int)TutorialType.WEAPON)
        {
            TutoCam.SetActive(true);
            MainCam.SetActive(false);
            TutoCam.transform.position = WeaponCamPos.position;
            TutoCam.transform.rotation = WeaponCamPos.rotation;
            Title.text = "무기교체";
            Tuto.text = "무기 타일에 올라가 [F] 키를 누르면 해당 무기로 변경 됩니다.";
        }
        else if (cnt == (int)TutorialType.POTAL)
        {

            TutoCam.transform.position = PotalCamPos.position;
            TutoCam.transform.rotation = PotalCamPos.rotation;
            Title.text = "던전입장";
            Tuto.text = "포탈 타일에 올라가 [F] 키를 누르면 던전으로 입장합니다.";
        }
        else if (cnt == (int)TutorialType.NPC) {
            TutoCam.transform.position = NPCCamPos.position;
            TutoCam.transform.rotation = NPCCamPos.rotation;
            Title.text = "NPC대화";
            Tuto.text = "NPC에게 다가가 [F]키를 누르면 NPC와 대화를 할 수 있습니다.";
            OkButton.SetActive(true);
            ButtonText.text = "처음으로";
        }
        else if (cnt == (int)TutorialType.Control || cnt== (int)TutorialType.END)
        {
            MainCam.SetActive(true);
            TutoCam.SetActive(false);

            playerMovement.playerOn();
            Title.text = "조작법";
            Tuto.text = "WASD 키로 플레이어를 움직이며 마우스 좌클릭을 통해 공격을 합니다.";
            cnt = 0;
        }
    }

    public void EndTutorial() {
        cnt = -1;
        MainCam.SetActive(true);
        TutoCam.SetActive(false);
        TutorialUI.SetActive(false);
        playerMovement.playerOn();
    }
}
