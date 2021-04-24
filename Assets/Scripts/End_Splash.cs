using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class End_Splash : MonoBehaviour
{
    GameObject SplashObj;
    Image image;
    private bool checkbool = false;
    // Start is called before the first frame update
    void Awake()
    {
        SplashObj = this.gameObject;
        image = SplashObj.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine("MainSplash");                        //코루틴    //판넬 투명도 조절
        if (checkbool)                                            //만약 checkbool 이 참이면
        {
            Destroy(this.gameObject);                        //판넬 파괴, 삭제
        }
    }

    IEnumerator MainSplash()
    {
        Color color = image.color;
        for (int i = 100; i >= 0; i--)
        {
            color.a += Time.deltaTime * 0.01f;
            image.color = color;
            if (image.color.a >= 255)
            {
                checkbool = true;
            }
        }
        yield return null;
    }
}
