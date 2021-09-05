using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BigRookGames.Weapons
{
    public class ProjectileController : Gun_RocketLauncher
    {
        public float            speed = 100;
        public LayerMask        collisionLayerMask;

        public GameObject       rocketExplosion;
        public MeshRenderer     projectileMesh;

        private bool            targetHit;
        public AudioSource      inFlightAudioSource;
        public ParticleSystem   disableOnHit;


        private void Start()
        {
            damage = 100f;
        }

        private void Update()
        {
            // --- Check to see if the target has been hit. We don't want to update the position if the target was hit ---
            if (targetHit) return;

            // --- moves the game object in the forward direction at the defined speed ---
            transform.position += transform.forward * (speed * Time.deltaTime);
        }



        /// <summary>
        /// Explodes on contact.
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter(Collision collision)
        {

            Collider[] colliders = Physics.OverlapSphere(transform.position, 1.5f);
            inFlightAudioSource.Stop();
            IDamageable temptarget = null;
            // --- Instantiate prefab for audio, delete after a few seconds ---
            foreach (Collider col in colliders)
            {
                if (col.tag == "Player")
                {
                    continue;

                }

                IDamageable target = col.GetComponent<IDamageable>();
                if (target != null&&temptarget!=target)
                {
                    temptarget = target;
                    target.OnDamage(GetDamage(), collision.transform.position, collision.transform.position);

                }
            }

            // --- return if not enabled because OnCollision is still called if compoenent is disabled ---
            if (!enabled) return;

            // --- Explode when hitting an object and disable the projectile mesh ---
            Explode();
            projectileMesh.enabled = false;
            targetHit = true;
            foreach (Collider col in GetComponents<Collider>())
            {
                col.enabled = false;
            }
            disableOnHit.Stop();


            // --- Destroy this object after 2 seconds. Using a delay because the particle system needs to finish ---
            Destroy(gameObject, 5f);
        }


        /// <summary>
        /// Instantiates an explode object.
        /// </summary>
        private void Explode()
        {
            // --- Instantiate new explosion option. I would recommend using an object pool ---
            GameObject newExplosion = Instantiate(rocketExplosion, transform.position, rocketExplosion.transform.rotation, null);


        }
    }
}