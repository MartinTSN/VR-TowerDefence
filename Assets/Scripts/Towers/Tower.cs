using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{

    public float range = 3f;
    public float fireRate = 1f;
    public GameObject bulletPrefab;
    public Transform barrelExit;

    Transform target;
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

    void AimAtTarget()
    {
        Vector3 lookPos = target.position - transform.position;
        lookPos.y = 0;

        Quaternion rotation = Quaternion.LookRotation(lookPos);

        transform.rotation = rotation;
    }

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
