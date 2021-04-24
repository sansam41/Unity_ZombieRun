using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Elf : NPC
{
    // Start is called before the first frame update
    void Awake()
    {
        NpcObject = GameObject.Find("NPC1").gameObject;
        NpcName = "Elf";
        nomalT.Set(72f, 22f, 35f); //NPC 기본 방향
    }

    // Update is called once per frame
    void Update()
    {
        UIManager.instance.UpdateDamageText();
    }

    private new void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            base.OnTriggerStay(other);
            if (playerInput.KeyDown_F)
            {
                talk(NpcObject, NpcName);
                UIManager.instance.SetActiveNpcTalkBox(true);
                UIManager.instance.UpdateTalkBox(NpcName);
                UIManager.instance.SetActiveUpgradeButton(true);
                playerMovement.enabled = false;
            }
        }
    }
    private new void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            LookNomal(nomalT);
            UIManager.instance.SetActiveNpcInteractionUI(false);
            UIManager.instance.SetActiveNpcTalkBox(false);
            UIManager.instance.SetActiveUpgradeButton(false);
        }
    }

    public void upgrade()
    {
        int newCoin = PlayerPrefs.GetInt("Coin");
        newCoin -= 100;
        Debug.Log("클릭");
        PlayerPrefs.SetInt("Coin", newCoin);
        PlayerPrefs.Save();
        UIManager.instance.UpdateCoinText();
        int upgrade=PlayerPrefs.GetInt("Upgrade");
        PlayerPrefs.SetInt("Upgrade", upgrade+1);

        Debug.Log(PlayerPrefs.GetInt("Upgrade"));
    }


}
