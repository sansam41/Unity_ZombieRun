using UnityEngine;


    public class Gun_RocketLauncher: MonoBehaviour
    {
        
        public AudioClip GunShotClip;
        public AudioClip ReloadClip;
        public AudioSource source;
        public AudioSource reloadSource;
        public Vector2 audioPitch = new Vector2(.9f, 1.1f);

        
        public GameObject muzzlePrefab;
        public GameObject muzzlePosition;

        
        public bool autoFire;
        public float shotDelay = .5f;
        public bool rotate = true;
        public float rotationSpeed = .25f;

        
        public GameObject scope;
        public bool scopeActive = true;
        private bool lastScopeState;




        public GameObject projectilePrefab;

        public GameObject projectileToDisableOnFire;

        [SerializeField] private float timeLastFired;   //발사 속도 조절을 위한 변수


        private void Start()
        {
            if(source != null) source.clip = GunShotClip;
            timeLastFired = 0;
            lastScopeState = scopeActive;
        }

        private void Update()
        {
        }

    public void Fire()
    {
        // 다음 탄환 발사 가능 까지의 딜레이 검사
        if ((timeLastFired + shotDelay) <= Time.time || timeLastFired == 0f)
        {
            timeLastFired = Time.time;
            // 포구 위치 생성
            Transform Mtransform = muzzlePosition.transform;
            Transform Ttransform = transform;
            var flash = Instantiate(muzzlePrefab, Mtransform);
            // 탄환 생성
            if (projectilePrefab != null)
            {
                GameObject newProjectile = Instantiate(projectilePrefab, Mtransform.position, Mtransform.rotation, Ttransform);
            }

            // 포구에 위치해 있던 탄환 제거
            if (projectileToDisableOnFire != null)
            {
                projectileToDisableOnFire.SetActive(false);
                Invoke("ReEnableDisabledProjectile", 3);
            }

            //오디오 관리
            if (source != null)
            {

                if (source.transform.IsChildOf(transform))
                {
                    source.Play();
                }
                else
                {
                    
                    AudioSource newAS = Instantiate(source);
                    if ((newAS = Instantiate(source)) != null && newAS.outputAudioMixerGroup != null && newAS.outputAudioMixerGroup.audioMixer != null)
                    {
                        
                        newAS.outputAudioMixerGroup.audioMixer.SetFloat("Pitch", Random.Range(audioPitch.x, audioPitch.y));
                        newAS.pitch = Random.Range(audioPitch.x, audioPitch.y);

                        
                        newAS.PlayOneShot(GunShotClip);

                        
                        Destroy(newAS.gameObject, 4);
                    }
                }
            }

            

        }
    }

        //포구 다시 생성
        private void ReEnableDisabledProjectile()
        {
            reloadSource.Play();
            projectileToDisableOnFire.SetActive(true);
        }
    }