using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThingToProtect : MonoBehaviour
{

    public float health = 100f;
    float currentHealth;

    public GameObject healthBarPrefab;
    GameObject healthBar;

    // Use this for initialization
    void Awake()
    {
        currentHealth = health;
        healthBar = Instantiate(healthBarPrefab, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity, transform);
    }

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

    void CheckHealth()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
