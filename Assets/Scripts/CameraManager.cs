using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject FenceOpenCamera;
    public Transform[] FencePos=new Transform[2];
    public GameObject MainCamera;
    public GameObject Player;
    private PlayerMovement playerMovement;
    private static CameraManager c_instance;


    public static CameraManager instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (c_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                c_instance = FindObjectOfType<CameraManager>();
            }

            // 싱글톤 오브젝트를 반환
            return c_instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerMovement=Player.GetComponent<PlayerMovement>();

        MainCamera.SetActive(true);
        FenceOpenCamera.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MainCameraOn() {
        playerMovement.playerOn();
        MainCamera.SetActive(true);
        FenceOpenCamera.SetActive(false);
    }

    public void FenceCameraOn(int i) {
        playerMovement.playerStop();
        FenceOpenCamera.transform.position = FencePos[i].position;
        FenceOpenCamera.transform.rotation = FencePos[i].rotation;
        FenceOpenCamera.SetActive(true);
        MainCamera.SetActive(false);
    }
}
