/*

            Handles all Bullet logic.

*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script that is put on the bullet.
/// </summary>
public class GunBullet : MonoBehaviour
{
    /// <summary>
    /// How fast the bullet is.
    /// </summary>
    public float speed = 3f;
    /// <summary>
    /// How much damage the bullet does.
    /// </summary>
    public float damage = 1f;
    /// <summary>
    /// How long it takes for the bullet to despawn. 1f = 1 sec.
    /// </summary>
    public float bulletDespawnRate = 4f;
    /// <summary>
    /// A tempoary rigidbody.
    /// </summary>
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

    /// <summary>
    /// Checks if the bullet hit an enemy.
    /// </summary>
    /// <param name="other">A gameobjects collider.</param>
    private void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;
        if (obj.tag == "Enemy")
        {
            obj.SendMessage("Hurt", damage);
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Checks if the bullet hit something other than an enemy.
    /// </summary>
    /// <param name="collision">The gameobject that is hit</param>
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
