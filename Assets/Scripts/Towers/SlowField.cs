/*

            Handles the SlowField logic.

*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script that is put on the slowfield.
/// </summary>
public class SlowField : MonoBehaviour
{
    /// <summary>
    /// How much enemies are slowed. In percentage.
    /// </summary>
    public float SlowPercentage = 50f;

    /// <summary>
    /// Checks if an enemy has entered it and slows it.
    /// </summary>
    /// <param name="other">The enemy object.</param>
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.gameObject.GetComponent<Enemy>().moveSpeed = other.gameObject.GetComponent<Enemy>().moveSpeed / 100 * SlowPercentage;
        }
    }
    /// <summary>
    /// Checks when the enemy has left it. Returns it to its normal speed.
    /// </summary>
    /// <param name="other">The enemy object.</param>
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.gameObject.GetComponent<Enemy>().moveSpeed = other.gameObject.GetComponent<Enemy>().moveSpeed / (SlowPercentage / 100); 
        }
    }
}
