/*

        Used to handle all logic for the "Base" object.

*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script that is put on the "Base" object.
/// </summary>
public class ThingToProtect : MonoBehaviour
{
    /// <summary>
    /// The starting about of health.
    /// </summary>
    public float health = 100f;
    /// <summary>
    /// The amount of health that the "Base" currently has.
    /// </summary>
    float currentHealth;

    /// <summary>
    /// The prefab of the healthbar object.
    /// </summary>
    public GameObject healthBarPrefab;
    /// <summary>
    /// The Healthbar object.
    /// </summary>
    GameObject healthBar;

    void Awake()
    {
        currentHealth = health;
        healthBar = Instantiate(healthBarPrefab, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity, transform);
    }

    /// <summary>
    /// Checks if an enemy object has entered it and damages the "Base" object.
    /// Checks if any enemies are remaining after.
    /// </summary>
    /// <param name="other">The object that enters the trigger.</param>
    void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;

        if (obj.tag == "Enemy")
        {
            currentHealth -= obj.GetComponent<Enemy>().damage;

            Transform pivot = healthBar.transform.Find("HealthyPivot");
            Vector3 scale = pivot.localScale;
            scale.x = Mathf.Clamp(currentHealth / health, 0, 1);

            pivot.localScale = scale;

            Destroy(obj);
            CheckIfNoEnemy checkEnemy = new CheckIfNoEnemy();
            checkEnemy.NoEnemy();
            CheckHealth();
        }
    }

    /// <summary>
    /// Checks if the object should be dead.
    /// </summary>
    void CheckHealth()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);

            GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject Enemy in Enemies)
            {
                Destroy(Enemy);
            }
            GameObject.FindGameObjectWithTag("Spawner").GetComponent<Spawner>().endWave = true;
            RenderSettings.skybox = null;
            PurchaseSpace.currentstate = PurchaseSpace.MenuStates.Lose;
        }
    }
}
