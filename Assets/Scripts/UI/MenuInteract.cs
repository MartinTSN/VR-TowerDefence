using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.UI;

public class MenuInteract : MonoBehaviour
{
    /// <summary>
    /// The steam-controller action for clicking.
    /// </summary>
    public SteamVR_Action_Boolean canvasClickAction;
    Hand hand;

    void OnEnable()
    {
        if (hand == null)
            hand = this.GetComponent<Hand>();
    }

    void Update()
    {
        Ray raycast = new Ray(hand.transform.position, hand.transform.forward);
        Debug.DrawRay(raycast.origin, raycast.direction * 100);
        RaycastHit hit;

        if (canvasClickAction.GetStateDown(hand.handType))
        {
            if (Physics.Raycast(raycast, out hit))
            {
                if (hit.collider.gameObject.tag == "Button")
                {
                    GameObject button;
                    button = hit.collider.gameObject.GetComponentInChildren<Button>().gameObject;
                    button.GetComponent<Button>().onClick.Invoke();
                }
            }
        }
    }
}
