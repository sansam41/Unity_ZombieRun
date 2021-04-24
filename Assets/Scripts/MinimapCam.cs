using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCam : MonoBehaviour
{
    public GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveMinimap();
    }

    void MoveMinimap() {
        Vector3 newPos = new Vector3(Player.transform.position.x, transform.position.y, transform.position.z);

        transform.position = newPos;
    }
}
