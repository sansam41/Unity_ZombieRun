﻿using System.Collections;
using UnityEngine;

// 총을 구현한다
public class Gun_Uzi : MonoBehaviour {
    // 총의 상태를 표현하는데 사용할 타입을 선언한다
    public enum State {
        Ready, // 발사 준비됨
        Empty, // 탄창이 빔
        Reloading, // 재장전 중
        Wait //기다리기
    }

    public State state { get; protected set; } // 현재 총의 상태

    public Transform fireTransform; // 총알이 발사될 위치

    public ParticleSystem muzzleFlashEffect; // 총구 화염 효과
    public ParticleSystem shellEjectEffect; // 탄피 배출 효과

    private LineRenderer bulletLineRenderer; // 총알 궤적을 그리기 위한 렌더러

    private AudioSource gunAudioPlayer; // 총 소리 재생기
    public AudioClip shotClip; // 발사 소리
    public AudioClip reloadClip; // 재장전 소리

    public float damage = 25; // 공격력
    public float upgradedDamage = 10;
    protected float fireDistance = 50f; // 사정거리

    public int ammoRemain = 10000; // 남은 전체 탄약
    public int magCapacity = 25; // 탄창 용량
    public int magAmmo; // 현재 탄창에 남아있는 탄약


    public float timeBetFire = 0.12f; // 총알 발사 간격
    public float reloadTime = 1.8f; // 재장전 소요 시간
    protected float lastFireTime; // 총을 마지막으로 발사한 시점

    public int gunType;//총의 종류

    private void Awake() {
        // 사용할 컴포넌트들의 참조를 가져오기
        gunAudioPlayer = GetComponent<AudioSource>();
        bulletLineRenderer = GetComponent<LineRenderer>();

        //사용할 점을 두 개로 변경(Position - Size)
        bulletLineRenderer.positionCount = 2;
        //라인 렌더러 비활성화
        bulletLineRenderer.enabled = false;


    }

    private void Update()
    {
        gunType = PlayerPrefs.GetInt("GunType"); //총 종류 확인
    }

    private void OnEnable() {
        // 총 상태 초기화
        magAmmo = magCapacity;
        //총의 현재 상태를 총을 쏠 준비가 된 상태로 변경
        state = State.Ready;
        //마지막으로 총을 쏜 시점을 초기화
        lastFireTime = 0;
    }

    // 발사 시도
    public void Fire() {
        upgradedDamage = damage + 10 * PlayerPrefs.GetInt("Upgrade");
        if (state == State.Ready && Time.time >= lastFireTime + timeBetFire) {
            lastFireTime = Time.time;
            Shot();
        }
    }

    // 실제 발사 처리
    private void Shot() {
        //레이캐스트에 의한 충돌 정보를 저장하는 컨테이너, FPS 총알 충돌 구현 방식
        RaycastHit hit;
        //탄알이 맞은 곳을 저장할 변수
        Vector3 hitPosition = Vector3.zero;
        Debug.Log(upgradedDamage);

        //레이캐스트(시작 지점, 방향, 충돌 정보 컨테이너_out, 사정거리)
        if (Physics.Raycast(fireTransform.position, fireTransform.forward, out hit, fireDistance))
        {
            //레이가 어떤 물체와 충돌한 경우

            //충돌한 상대방으로부터 IDamageable 오브젝트 가져오기 시도
            IDamageable target = hit.collider.GetComponent<IDamageable>();


            //상대방으로부터 IDamageable 오브젝트를 가져오는데 성공 했다면
            if (target != null)
            {
                //상대방의 OnDamage함수를 실행시켜 상대방에 대미지 주기
                target.OnDamage(upgradedDamage, hit.point, hit.normal);
            }
            hitPosition = hit.point;
        }
        else {
            //레이가 다른 물체와 충돌하지 않았다면
            //탄알이 최대 사정거리 까지 날아갔을 때의 위치를 충돌 위치로 사용
            hitPosition = fireTransform.position + fireTransform.forward * fireDistance;
        }
        StartCoroutine(ShotEffect(hitPosition));//발사 이펙트 코루틴 시작
        magAmmo--;
        if (magAmmo <= 0) {
            state = State.Empty;
        }

 
    }

    // 발사 이펙트와 소리를 재생하고 총알 궤적을 그린다, 코루틴 메소드
    protected IEnumerator ShotEffect(Vector3 hitPosition)
    {
        muzzleFlashEffect.Play();//총구화염 이펙트
        shellEjectEffect.Play();//탄창 배출 이펙트

        gunAudioPlayer.PlayOneShot(shotClip);

        if (gunType == 1)
        {
            // 라인 렌더러를 활성화하여 총알 궤적을 그린다
            bulletLineRenderer.SetPosition(0, fireTransform.position);
            bulletLineRenderer.SetPosition(1, hitPosition);

            bulletLineRenderer.enabled = true;
            // 0.03초 동안 잠시 처리를 대기, 코루팀 메소드 사용
            yield return new WaitForSeconds(0.03f);

            // 라인 렌더러를 비활성화하여 총알 궤적을 지운다
            bulletLineRenderer.enabled = false;
        }
    }

    // 재장전 시도
    public bool Reload()
    {
        if (state == State.Reloading || ammoRemain <= 0 || magAmmo >= magCapacity){
            //이미 재장전 중이거나 만음 탄알이 없음. 혹은 탄창에 탄알이 가득 찬 경우.
            return false;
        }

        StartCoroutine(ReloadRoutine());
        return true;
    }

    // 실제 재장전 처리를 진행
    private IEnumerator ReloadRoutine() {
        // 현재 상태를 재장전 중 상태로 전환
        state = State.Reloading;
        //재장전 소리 재생
        gunAudioPlayer.PlayOneShot(reloadClip);

        // 재장전 소요 시간 만큼 처리를 쉬기
        yield return new WaitForSeconds(reloadTime);

        int ammoToFill = magCapacity - magAmmo;//최대 탄약 수 - 현재 탄약 수


        if (ammoRemain < ammoToFill) {
            //만약 남은 탄알의 개수가 채워야할 탄알보다 적다면
            //채워야할 탄알수를 남은 탄알에 맞춰서 줄임 
            ammoToFill = ammoRemain;
        }
        magAmmo += ammoToFill;
        ammoRemain -= ammoToFill;

        state = State.Ready;
        // 총의 현재 상태를 발사 준비된 상태로 변경
        state = State.Ready;
    }

}