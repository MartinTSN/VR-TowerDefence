/*

        Handles end of round stuff

*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script that is put on the "Base" object. Checks if there are any remaining enemies and ends the round.
/// </summary>
public class CheckIfNoEnemy : MonoBehaviour
{
    public void NoEnemy()
    {

        if (GameObject.FindGameObjectsWithTag("Enemy").Length <= 1)
        {
            GameObject Rhand = GameObject.FindGameObjectWithTag("RightHand");
            Rhand.GetComponent<LineRenderer>().enabled = true;
            Rhand.GetComponent<CanvasInteract>().enabled = true;
            GameObject Lhand = GameObject.FindGameObjectWithTag("LeftHand");
            Lhand.GetComponent<LineRenderer>().enabled = true;
            Lhand.GetComponent<CanvasInteract>().enabled = true;
            Rhand.GetComponentInChildren<Attatch>().UnSet();

            foreach (GameObject Tower in GameObject.FindGameObjectsWithTag("Tower"))
            {
                Tower.GetComponent<Tower>().enabled = false;
            }
        }
    }

}
