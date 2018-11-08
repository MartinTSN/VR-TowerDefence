/*

            Handles the TowerBullet logic.

*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script that is put on the Tower Bullet.
/// </summary>
public class TowerBullet : MonoBehaviour
{
    /// <summary>
    /// The enemy that the bullet goes to.
    /// </summary>
    public Transform target;
    /// <summary>
    /// The speed of the bullet.
    /// </summary>
    public float speed = 10f;
    /// <summary>
    /// How much the bullet damages.
    /// </summary>
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

    /// <summary>
    /// Checks if the bullet hit an enemy.
    /// </summary>
    /// <param name="other">A gameobjects collider.</param>
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
