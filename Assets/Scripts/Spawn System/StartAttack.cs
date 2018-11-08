/*

            Used to activate turrets.

*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script that is put on a gameobject.
/// </summary>
public class StartAttack : MonoBehaviour
{
    /// <summary>
    /// Checks if an enemy has entered it.
    /// </summary>
    /// <param name="other">The collider of a gameobject.</param>
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
