using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBullet : MonoBehaviour
{

    public Transform target;

    public float speed = 10f;
    public float damage = 5f;

    void Update()
    {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;

        if (obj.tag == "Enemy")
        {
            obj.SendMessage("Hurt", damage);
            Destroy(gameObject);
        }
        else if (obj.tag == "Ground")
        {
            Destroy(gameObject);
        }
    }
}
