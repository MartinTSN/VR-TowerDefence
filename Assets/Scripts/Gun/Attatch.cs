/*

            Attatches the gun to the Hand gameobject.

*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

/// <summary>
/// A script that is put on the "StartWave" button. Attaches the gun to the hand object.
/// </summary>
public class Attatch : MonoBehaviour
{
    /// <summary>
    /// The hand that the gun is put on.
    /// </summary>
    public Hand hand;
    /// <summary>
    /// An object where the gun is placed in.
    /// </summary>
    public GameObject gameobject;
    //private GameObject Gun;

    private void Update()
    {
        Set();
    }

    /// <summary>
    /// Sets the gun to the hand. Spawns a new gun if no gun exists.
    /// </summary>
    public void Set()
    {
        if (GameObject.FindGameObjectsWithTag("HandGunHolder").Length == 0)
        {
            Instantiate(gameobject);
        }
        hand.AttachObject(gameobject, GrabTypes.Grip, Hand.AttachmentFlags.ParentToHand);
        gameobject.transform.position = hand.transform.position;
        gameobject.transform.rotation = hand.transform.rotation;
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Detaches the gun.
    /// </summary>
    public void UnSet()
    {
        hand.DetachObject(gameobject);
        gameObject.SetActive(false);
    }
}
