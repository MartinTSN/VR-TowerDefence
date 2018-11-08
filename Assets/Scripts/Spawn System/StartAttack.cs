using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAttack : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            foreach (GameObject Tower in GameObject.FindGameObjectsWithTag("Tower"))
            {
                Tower.GetComponent<Tower>().enabled = true;
            }

        }
    }
}
