/*

            Handles all logic for enemies.

*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script that is put on an enemy.
/// </summary>
public class Enemy : MonoBehaviour
{
    const float minPathUpdateTime = .2f;
    const float pathUpdateMoveThreshold = .5f;

    /// <summary>
    /// The prefab of the healthbar object.
    /// </summary>
    public GameObject healthBarPrefab;
    /// <summary>
    /// The Healthbar object.
    /// </summary>
    GameObject healthBar;

    /// <summary>
    /// The amount of Health that the enemy has.
    /// </summary>
    public float health = 20f;
    /// <summary>
    /// The amount of health that the enemy currently has.
    /// </summary>
    float currentHealth;
    /// <summary>
    /// How much money is gained from killing it.
    /// </summary>
    public float worth = 4f;
    /// <summary>
    /// How much damage the enemy does.
    /// </summary>
    public float damage = 5f;

    /// <summary>
    /// The Target for the enemy. (usually the "Base" object)
    /// </summary>
    public Transform target;
    /// <summary>
    /// How fast the enemy moves.
    /// </summary>
    public float moveSpeed = 1f;
    /// <summary>
    /// How fast the enemy turns conors.
    /// </summary>
    public float turnSpeed = 3;
    /// <summary>
    /// The turn distance.
    /// </summary>
    public float turnDst = 5;

    /// <summary>
    /// The path that the enemy takes.
    /// </summary>
    Paths path;

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

    /// <summary>
    /// Used when the enemy is hit.
    /// </summary>
    /// <param name="damage">How much damage the enemy takes.</param>
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

    /// <summary>
    /// Used when a path is found.
    /// </summary>
    /// <param name="waypoints">The waypoints for the enemy</param>
    /// <param name="pathSuccessful">If it found a path</param>
    public void OnPathFound(Vector3[] waypoints, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = new Paths(waypoints, transform.position, turnDst);
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    /// <summary>
    /// Used to make the enemy follow the found path.
    /// </summary>
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
}
