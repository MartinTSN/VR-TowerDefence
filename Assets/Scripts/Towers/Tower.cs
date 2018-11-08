/*

            Handles the tower logic.

*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script that is put on a tower object.
/// </summary>
public class Tower : MonoBehaviour
{
    /// <summary>
    /// The tower range.
    /// </summary>
    public float range = 3f;
    /// <summary>
    /// How fast the tower shoots.
    /// </summary>
    public float fireRate = 1f;
    /// <summary>
    /// A prefab of the bullet that is shot.
    /// </summary>
    public GameObject bulletPrefab;
    /// <summary>
    /// The bullet-Exit.
    /// </summary>
    public Transform barrelExit;

    /// <summary>
    /// The target that is shot.
    /// </summary>
    Transform target;
    /// <summary>
    /// A counter used to shoot.
    /// </summary>
    float fireCounter = 0;

    void Update()
    {
        FindNextTarget();

        if (target != null)
        {
            AimAtTarget();
            Shoot();
        }
    }

    /// <summary>
    /// Finds the target that the tower shoots.
    /// </summary>
    void FindNextTarget()
    {
        int layerMask = 1 << 9;
        Collider[] enemies = Physics.OverlapSphere(transform.position, range, layerMask);

        if (enemies.Length > 0)
        {
            target = enemies[0].gameObject.transform;

            foreach (Collider enemy in enemies)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);

                if (distance < Vector3.Distance(transform.position, target.position))
                {
                    target = enemy.gameObject.transform;
                }
            }
        }
        else
        {
            target = null;
        }
    }

    /// <summary>
    /// Aims the tower at the target.
    /// </summary>
    void AimAtTarget()
    {
        Vector3 lookPos = target.position - transform.position;
        lookPos.y = 0;

        Quaternion rotation = Quaternion.LookRotation(lookPos);

        transform.rotation = rotation;
    }

    /// <summary>
    /// Shoots at the target.
    /// </summary>
    void Shoot()
    {
        if (fireCounter <= 0)
        {
            GameObject newbullet = Instantiate(bulletPrefab, barrelExit.position, Quaternion.identity);
            newbullet.GetComponent<TowerBullet>().target = target;
            fireCounter = fireRate;
        }
        else
        {
            fireCounter -= Time.deltaTime;
        }
    }
}
