using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    Rigidbody NPCrigidbody;
    Coroutine smoothMove = null;


    protected PlayerInput playerInput;
    protected PlayerMovement playerMovement;
    protected PlayerShooter playerShooter;
    private Animator playerAnimator;


    protected Vector3 nomalT;
    // Start is called before the first frame update
    protected GameObject NpcObject;
    protected string NpcName ="nobody";
    private void Awake()
    {
        NPCrigidbody = GetComponent<Rigidbody>();
        NpcObject = GameObject.Find("NPC2").gameObject;
        NpcName = "Zombie";
        nomalT.Set(63f, 22f, 30f); //NPC 기본 방향
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            LookPlayer(other);
            UIManager.instance.SetActiveNpcInteractionUI(true);
            playerInput = other.GetComponent<PlayerInput>();
            playerMovement = other.GetComponent<PlayerMovement>();
            playerAnimator = other.GetComponent<Animator>();

            if (playerInput.KeyDown_F)
            {
                talk(NpcObject, NpcName);
                playerAnimator.SetFloat("Move", 0);
                UIManager.instance.SetActiveNpcTalkBox(true);
                UIManager.instance.UpdateTalkBox(NpcName);
                UIManager.instance.SetActiveUpgradeButton(false);
                playerMovement.enabled = false;
            }
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            LookNomal(nomalT);
            UIManager.instance.SetActiveNpcInteractionUI(false);
            UIManager.instance.SetActiveNpcTalkBox(false);
            UIManager.instance.SetActiveUpgradeButton(false);
        }
    }

    protected void LookPlayer(Collider other) {
        float time = 0.3f;
        Vector3 lookAt = other.transform.position;
        lookAt.y = transform.position.y;

        if (smoothMove == null)
        {
            smoothMove = StartCoroutine(LookAtSmoothly(transform,lookAt,time));
        }
        else {
            StopCoroutine(smoothMove);
            smoothMove = StartCoroutine(LookAtSmoothly(transform, lookAt, time));
        }
    }
    protected void LookNomal(Vector3 transnomal) {
        Vector3 lookAt = nomalT;
        float time = 1f;
        if (smoothMove == null)
        {
            smoothMove = StartCoroutine(LookAtSmoothly(transform, lookAt, time));
        }
        else
        {
            StopCoroutine(smoothMove);
            smoothMove = StartCoroutine(LookAtSmoothly(transform, lookAt, time));
        }
    }

    IEnumerator LookAtSmoothly(Transform Npc,Vector3 worldPostion,float duration) {
        Quaternion currentRot = Npc.rotation;
        Quaternion newRot = Quaternion.LookRotation(worldPostion - Npc.position, Npc.TransformDirection(Vector3.up));
        float counter = 0;
        while (counter < duration) {
            counter += Time.deltaTime;
            Npc.rotation = Quaternion.Lerp(currentRot, newRot, counter / duration);
            yield return null;
        }
    }

    protected void talk(GameObject newNpcObject,string newNpcName) {
        NpcObject=newNpcObject;
        NpcName = newNpcName;
    }

}
