using System.Collections.Generic;
using UnityEngine;

// 적 게임 오브젝트를 주기적으로 생성
public class EnemySpawner : MonoBehaviour {
    public Enemy enemyPrefab; // 생성할 적 AI
    public Enemy BossPrefab; // 생성할 적 AI

    public Transform[] spawnPoints; // 적 AI를 소환할 위치들

    public float damageMax = 40f; // 최대 공격력
    public float damageMin = 20f; // 최소 공격력

    public float healthMax = 200f; // 최대 체력
    public float healthMin = 100f; // 최소 체력

    public float speedMax = 3f; // 최대 속도
    public float speedMin = 1f; // 최소 속도

    private bool isWave = false; //웨이브 스폰 여부
    private bool isenemy=false; //적 생존 여부
    private bool isClear = false;   //모든 웨이브 클리어 여부
    private bool save = false;  //모든 방 클리어시 스코어에 대한 공유를 한번만 실행하기 위한 확인 변수
    public Color strongEnemyColor = Color.red; // 강한 적 AI가 가지게 될 피부색

    private List<Enemy> enemies = new List<Enemy>(); // 생성된 적들을 담는 리스트
    private int wave; // 현재 웨이브
    private int Room=-1;
    private int Open;

    public int Maxwave = 3;
    public int LastRoom = 2;

    public GameObject SpawnPoint;

    private void Start()
    {
        Open = 2;
    }
    private void Update() {
        // 게임 오버 상태일때는 생성하지 않음
        if (GameManager.instance != null && GameManager.instance.isGameover)
        {
            return;
        }
        if (!isClear)
        {
            //몬스터 스폰
            if (!isWave && wave < Maxwave && Open == 2)
            {
                SetActiveUI(true);
                Invoke("SpawnWave", 3f);
                isWave = true; //웨이브당 한번만 실행
            }

            //웨이브 클리어시 다음 웨이브를 위해 조건들 재조정
            if (isenemy && enemies.Count == 0)  //해당 웨이브 적을 모두 죽였을 경우.
            {
                isenemy = false;
                isWave = false;
                if (wave == Maxwave)    //웨이브가 해당 방의 마지막 웨이브 였을 경우
                {
                    Room++; //다음 방 Open
                    if (Room >= LastRoom)  //모든 방을 클리어 한 경우
                    {
                        isClear = true;
                        return;
                    }
                    Open = 1;
                }
            }

            if (Room >= 0)  
                if (Open == 1)
                {
                    NextRoom();
                    MoveSpawnPosition();
                }
                else if (Open == 0)
                {
                    NextRoom();
                    isWave = false;
                    wave = 0;
                }
            // UI 갱신
            UpdateUI();
        }
        else
        {
            if (save == false)
            {
                GameManager.instance.ClearGame();
                save = true;
            }
        }
    }

    // 웨이브 정보를 UI로 표시
    private void UpdateUI() {
        // 현재 웨이브 표시
        UIManager.instance.UpdateWaveText(wave);
    }

    private void SetActiveUI(bool check) {
        UIManager.instance.waveText.enabled = check;
    }

    // 현재 웨이브에 맞춰 적을 생성
    private void SpawnWave() {
        wave++;
        //현재 웨이브의 *1.5값을 반올림한 값만큼 적 스폰
        int spawnCount = Mathf.RoundToInt(wave * 1.5f);
        spawnCount += 2*(Room+1);

        for (int i = 0; i < spawnCount; i++) {
            //적의 세기를 0에서 100사이에서 결정
            float enemyIntensity = Random.Range(0f, 1f);
            CreateEnemy(enemyIntensity);
        }

        float BossIntensity = Random.Range(0f, 1f);
        if(Room==1&&wave== Maxwave)
            CreateBoss(BossIntensity);
        isenemy = true;
        SetActiveUI(false);
    }


    // 적을 생성하고 생성한 적에게 추적할 대상을 할당
    private void CreateEnemy(float intensity) {
        //Lerp는 선형보간의 줄임말으로 시작점과 도착점을 t기준으로 정하는 함수
        float health = Mathf.Lerp(healthMin, healthMax, intensity);
        float damage = Mathf.Lerp(damageMin, damageMax, intensity);
        float speed = Mathf.Lerp(speedMin, speedMax, intensity);

        Color skinColor = Color.Lerp(Color.white,strongEnemyColor, intensity);

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        //Instantiate(Prefab,위치,회전)
        Enemy enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        enemy.Setup(health, damage, speed, skinColor);

        enemies.Add(enemy);
         //람다식(익명함수)을 onDeath이벤트에 등록
        enemy.onDeath += () => enemies.Remove(enemy);
        enemy.onDeath += () => Destroy(enemy.gameObject, 10f);
        enemy.onDeath += () => GameManager.instance.AddScore(100);
    }

    private void CreateBoss(float intensity)
    {
        //Lerp는 선형보간의 줄임말으로 시작점과 도착점을 t기준으로 정하는 함수
        float health = Mathf.Lerp(1000, 1500, intensity);
        float damage = Mathf.Lerp(50, 80, intensity);
        float speed = Mathf.Lerp(speedMin, speedMax, intensity);

        Color skinColor = Color.Lerp(Color.white, strongEnemyColor, intensity);

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        //Instantiate(Prefab,위치,회전)
        Enemy enemy = Instantiate(BossPrefab, spawnPoint.position, spawnPoint.rotation);

        enemy.Setup(health, damage, speed, skinColor);

        enemies.Add(enemy);
        //람다식(익명함수)을 onDeath이벤트에 등록
        enemy.onDeath += () => enemies.Remove(enemy);
        enemy.onDeath += () => Destroy(enemy.gameObject, 10f);
        enemy.onDeath += () => GameManager.instance.AddScore(100);
    }

    public void NextRoom() {
        Open=GameManager.instance.ControlFence(Room, Open);
    }

    public void MoveSpawnPosition() {
        Transform SpawnTransform = SpawnPoint.GetComponent<Transform>();
        Vector3 newPosition = new Vector3(27 + 27 * Room, SpawnTransform.position.y, SpawnTransform.position.z);
        SpawnTransform.position = newPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!isenemy&& !isWave)
            if (other.tag == "Player")
                Open = 0;
    }
}