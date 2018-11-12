/*

        Handles end of round stuff

*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script that is put on the "Base" object. Checks if there are any remaining enemies and ends the round.
/// </summary>
public class CheckIfNoEnemy
{
    Spawner spawner;

    public void NoEnemy()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length <= 1)
        {
            spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<Spawner>();
            if (spawner.waves.Count == spawner.currentWave)
            {
                PurchaseSpace.currentstate = PurchaseSpace.MenuStates.Won;
            }
            GameObject Rhand = GameObject.FindGameObjectWithTag("RightHand");
            Rhand.GetComponent<LineRenderer>().enabled = true;
            Rhand.GetComponent<CanvasInteract>().enabled = true;
            GameObject Lhand = GameObject.FindGameObjectWithTag("LeftHand");
            Lhand.GetComponent<LineRenderer>().enabled = true;
            Lhand.GetComponent<CanvasInteract>().enabled = true;
            Rhand.GetComponentInChildren<Attatch>().UnSet();
            foreach (var Text in GameObject.FindGameObjectsWithTag("Text"))
            {
                Text.SetActive(true);
            }

            foreach (GameObject Tower in GameObject.FindGameObjectsWithTag("Tower"))
            {
                Tower.GetComponent<Tower>().enabled = false;
            }

        }
    }
}
