using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    const float minPathUpdateTime = .2f;
    const float pathUpdateMoveThreshold = .5f;


    public GameObject healthBarPrefab;
    GameObject healthBar;

    public float health = 20f;
    float currentHealth;

    public float worth = 4f;

    public Transform target;
    public float moveSpeed = 1f;
    public float turnSpeed = 3;
    public float turnDst = 5;

    public float damage = 5f;

    Paths path;
    //Vector3[] path;
    //int targetIndex;


    void Start()
    {
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    void Awake()
    {
        currentHealth = health;
        healthBar = Instantiate(healthBarPrefab, transform.position + new Vector3(0, 0.25f, 0), Quaternion.identity, transform);
        healthBar.SetActive(false);
    }

    public void Hurt(float damage)
    {
        healthBar.SetActive(true);
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Money.amount += worth;
            Destroy(gameObject);

            CheckIfNoEnemy checkEnemy = new CheckIfNoEnemy();
            checkEnemy.NoEnemy();
        }

        Transform pivot = healthBar.transform.Find("HealthyPivot");
        Vector3 scale = pivot.localScale;
        scale.x = Mathf.Clamp(currentHealth / health, 0, 1);

        pivot.localScale = scale;
    }

    public void OnPathFound(Vector3[] waypoints, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = new Paths(waypoints, transform.position, turnDst);
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        bool followingPath = true;
        int pathIndex = 0;
        transform.LookAt(path.lookPoints[0]);

        while (followingPath)
        {
            Vector2 pos2D = new Vector2(transform.position.x, transform.position.z);
            while (path.turnBoundaries[pathIndex].HasCrossedLine(pos2D))
            {
                if (pathIndex == path.finishLineIndex)
                {
                    followingPath = false;
                    break;
                }
                else
                {
                    pathIndex++;
                }
            }

            if (followingPath)
            {
                Quaternion targetRotation = Quaternion.LookRotation(path.lookPoints[pathIndex] - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
                transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed, Space.Self);
            }

            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            path.DrawWithGizmos();
        }
    }


    //public void OnDrawGizmos()
    //{
    //    if (path != null)
    //    {
    //        for (int i = targetIndex; i < path.Length; i++)
    //        {
    //            Gizmos.color = Color.red;
    //            Gizmos.DrawCube(path[i], Vector3.one);

    //            if (i == targetIndex)
    //            {
    //                Gizmos.DrawLine(transform.position, path[i]);
    //            }
    //            else
    //            {
    //                Gizmos.DrawLine(path[i - 1], path[i]);
    //            }
    //        }
    //    }
    //}

    //IEnumerator FollowPath()
    //{
    //    Vector3 currentWaypoint = path[0];
    //    while (true)
    //    {
    //        if (transform.position == currentWaypoint)
    //        {
    //            targetIndex++;
    //            if (targetIndex >= path.Length)
    //            {
    //                yield break;
    //            }
    //            currentWaypoint = path[targetIndex];
    //        }
    //        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, moveSpeed * Time.deltaTime);
    //        yield return null;
    //    }
    //}

    //public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    //{
    //    if (pathSuccessful)
    //    {
    //        path = newPath;
    //        targetIndex = 0;
    //        StopCoroutine("FollowPath");
    //        StartCoroutine("FollowPath");
    //    }
    //}
}
