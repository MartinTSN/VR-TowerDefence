using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Attatch : MonoBehaviour
{
    public Hand hand;
    public GameObject gameobject;
    private GameObject Gun;

    private void Update()
    {
        Set();
    }

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

    public void UnSet()
    {
        hand.DetachObject(gameobject);
        gameObject.SetActive(false);
    }
}
