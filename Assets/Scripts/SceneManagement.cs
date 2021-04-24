using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    PlayerInput playerInput;
    bool Ok = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            UIManager.instance.SetActivePotalInteractionUI(true);
            playerInput = other.GetComponent<PlayerInput>();
            if (playerInput.KeyDown_F)
            {
                Ok = true;
                PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
                playerMovement.playerStop();
            }
        }
        if (Ok)
            End();
    }
    private void OnTriggerExit(Collider other) {
        UIManager.instance.SetActivePotalInteractionUI(false);
    }

    private void End() {
        UIManager.instance.SetActivePanel(true);
        for (int i = 0; i < 100; i++)
            if (UIManager.instance.GetAlpha() >= 1)
                if (SceneManager.GetActiveScene().name == "Main")
                {
                    SceneManager.LoadScene("HomeTown");
                    break;
                }
                else if (SceneManager.GetActiveScene().name == "HomeTown")
                {
                    SceneManager.LoadScene("Main");
                    break;
                }
    }
}
