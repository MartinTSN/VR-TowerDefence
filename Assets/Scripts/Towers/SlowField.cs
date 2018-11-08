using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowField : MonoBehaviour
{
    public float SlowPercentage = 50f;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.gameObject.GetComponent<Enemy>().moveSpeed = other.gameObject.GetComponent<Enemy>().moveSpeed / 100 * SlowPercentage;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.gameObject.GetComponent<Enemy>().moveSpeed = other.gameObject.GetComponent<Enemy>().moveSpeed / (SlowPercentage / 100); 
        }
    }
}
