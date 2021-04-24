﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject[] FenceOpenCamera = new GameObject[2];
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
        for (int i = 0; i < FenceOpenCamera.Length; i++)
            FenceOpenCamera[i].SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MainCameraOn() {
        playerMovement.playerOn();
        MainCamera.SetActive(true);
        for (int i = 0; i < FenceOpenCamera.Length; i++)
            FenceOpenCamera[i].SetActive(false);
    }

    public void FenceCameraOn(int i) {
        playerMovement.playerStop();
        FenceOpenCamera[i].SetActive(true);
        MainCamera.SetActive(false);
    }
}
