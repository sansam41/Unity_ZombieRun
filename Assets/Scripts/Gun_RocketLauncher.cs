using UnityEngine;


    public class Gun_RocketLauncher: Weapon
{


        public Vector2 audioPitch = new Vector2(.9f, 1.1f);

        
        public GameObject muzzlePrefab;//포구 효과
        public GameObject muzzlePosition;//포구 위치
        public GameObject projectilePrefab; //탄환 프리펩

        public GameObject projectileToDisableOnFire;

        [SerializeField] private float timeLastFired;   //발사 속도 조절을 위한 변수

        private void Start()
        {
            timeBetFire = 1.0f;//딜레이 시간
            if (gunAudioPlayer != null) gunAudioPlayer.clip = shotClip;
                timeLastFired = 0;
        }

        private void Update()
        {
        }

    public override void Fire() 
    {
        // 다음 탄환 발사 가능 까지의 딜레이 검사
        if ((timeLastFired + timeBetFire) <= Time.time || timeLastFired == 0f)
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
            if (gunAudioPlayer != null)
            {

                if (gunAudioPlayer.transform.IsChildOf(transform))
                {
                    gunAudioPlayer.Play();
                }
                else
                {
                    
                    AudioSource newAS = Instantiate(gunAudioPlayer);
                    if ((newAS = Instantiate(gunAudioPlayer)) != null && newAS.outputAudioMixerGroup != null && newAS.outputAudioMixerGroup.audioMixer != null)
                    {
                        
                        newAS.outputAudioMixerGroup.audioMixer.SetFloat("Pitch", Random.Range(audioPitch.x, audioPitch.y));
                        newAS.pitch = Random.Range(audioPitch.x, audioPitch.y);

                        
                        newAS.PlayOneShot(shotClip);

                        
                        Destroy(newAS.gameObject, 4);
                    }
                }
            }

            

        }
    }

        //포구에 있는 탄환 다시 생성
        private void ReEnableDisabledProjectile()
        {
            reloadSource.Play();
            projectileToDisableOnFire.SetActive(true);
        }
    }