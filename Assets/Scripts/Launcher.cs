using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    new Rigidbody rigidbody;
    // Start is called before the first frame update
    float damage = 100f;



    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

    }
    void Update()
    {
        rigidbody.AddForce(transform.forward, ForceMode.Impulse);
    }

    private void OnTriggerStay(Collider other)
    {
        /*
        IDamageable target = other.GetComponent<IDamageable>();

        if (target != null) {
            target.OnDamage(damage, other.transform.position, other.transform.position);
            Destroy(gameObject);
        }*/
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1.2f);
        foreach (Collider col in colliders)
        {
            /*
            if (col.rigidbody)
            {
                if (col.rigidbody != gameObject.rigidbody)
                {
                    col.rigidbody.AddExplosionForce(power, transform.position, radius);
                }
            }*/
            if (col.tag == "Player") {
                continue;

            }
            IDamageable target = col.GetComponent<IDamageable>();
            if (target != null)
            {
                target.OnDamage(damage, other.transform.position, other.transform.position);
                Debug.Log(damage);
            }
        }
        Destroy(gameObject);

    }
}
