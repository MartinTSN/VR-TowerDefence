using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBullet : MonoBehaviour
{
    public float speed = 3f;
    public float damage = 1f;
    public float bulletDespawnRate = 4f;
    Rigidbody tempBody;

    void Start()
    {
        tempBody = gameObject.GetComponent<Rigidbody>();
        tempBody.AddForce(GunShoot.raycast.direction * speed);
    }

    void Update()
    {
        Destroy(gameObject, bulletDespawnRate);
    }
    private void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;
        if (obj.tag == "Enemy")
        {
            obj.SendMessage("Hurt", damage);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        GameObject obj = collision.gameObject;

        if (obj.tag == "Wall")
        {
            Destroy(gameObject);
        }
        if (obj.tag == "Tower")
        {
            Destroy(gameObject);
        }
        if (obj.tag == "Ground")
        {
            Destroy(gameObject);
        }
        if (obj.tag == "TowerBarrel")
        {
            Destroy(gameObject);
        }
        if (obj.tag == "UsedWall")
        {
            Destroy(gameObject);
        }
    }
}
