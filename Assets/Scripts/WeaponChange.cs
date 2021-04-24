using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChange : MonoBehaviour
{
    PlayerInput playerInput;
    public int gunType;
    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            UIManager.instance.SetActiveWeaponsChangeUI(true);
            playerInput = other.GetComponent<PlayerInput>();
            if (playerInput.KeyDown_F)
            {
                PlayerPrefs.SetInt("GunType", gunType);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        UIManager.instance.SetActiveWeaponsChangeUI(false);
    }
}
