/*

            Handles Canvas Interaction logic.

*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;

/// <summary>
/// A script that is put on the hands.
/// </summary>
public class CanvasInteract : MonoBehaviour
{
    /// <summary>
    /// The steam-controller action for clicking.
    /// </summary>
    public SteamVR_Action_Boolean canvasClickAction;
    /// <summary>
    /// The steam-controller action for Cancel-building.
    /// </summary>
    public SteamVR_Action_Boolean cancelBuildingAction;
    /// <summary>
    /// The grid enemies follow.
    /// </summary>
    private Grid grid;
    /// <summary>
    /// A SteamVR hand.
    /// </summary>
    Hand hand;
    /// <summary>
    /// A object Tower.
    /// </summary>
    GameObject boughtTower = PurchaseSpace.boughtTower;
    /// <summary>
    /// A object TeleportPoint.
    /// </summary>
    GameObject teleportPoint = PurchaseSpace.teleportPoint;
    /// <summary>
    /// A object Wall.
    /// </summary>
    GameObject wall = PurchaseSpace.Wall;
    /// <summary>
    /// A object SlowField.
    /// </summary>
    GameObject slowField = PurchaseSpace.slowField;

    void Awake()
    {
        grid = GameObject.Find("GameobjectManager").GetComponent<Grid>();
    }

    void OnEnable()
    {
        if (hand == null)
            hand = this.GetComponent<Hand>();
    }

    /// <summary>
    /// Moves the puurchased item infront of the hand that bought it.
    /// </summary>
    void MovePurchasedItem()
    {
        if (boughtTower != null)
        {
            boughtTower.transform.position = hand.transform.position + hand.transform.forward * 1;
        }
        else if (teleportPoint != null)
        {
            teleportPoint.transform.position = hand.transform.position + hand.transform.forward * 1;
        }
        else if (slowField != null)
        {
            slowField.transform.position = hand.transform.position + hand.transform.forward * 1;
        }
        else if (wall != null)
        {
            wall.transform.position = hand.transform.position + hand.transform.forward * 1;
        }
    }

    /// <summary>
    /// Checks if the aimed space is a wall, sets the bought item on it if clicked.
    /// </summary>
    void CheckForWall()
    {
        Ray raycast = new Ray(hand.transform.position, hand.transform.forward);
        RaycastHit hit;

        Debug.DrawRay(raycast.origin, raycast.direction * 100);

        if (Physics.Raycast(raycast, out hit))
        {
            if (hit.collider.gameObject.tag == "Wall")
            {
                if (boughtTower != null)
                {
                    boughtTower.transform.position = hit.collider.gameObject.transform.position + new Vector3(0, 0.5f, 0);
                }
                else if (teleportPoint != null)
                {
                    teleportPoint.transform.position = hit.collider.gameObject.transform.position + new Vector3(0, 0.5f, 0);
                }
                if (canvasClickAction.GetStateDown(hand.handType))
                {
                    hit.collider.gameObject.tag = "UsedWall";

                    if (boughtTower != null)
                    {
                        BoughtTower();
                    }
                    else if (teleportPoint != null)
                    {
                        TeleportPoint();
                    }
                }
            }
        }
    }

    /// <summary>
    /// Checks if the aimed space is the ground, sets the bought item on it if clicked.
    /// </summary>
    void CheckForGround()
    {
        Ray raycast = new Ray(hand.transform.position, hand.transform.forward);
        RaycastHit hit;

        Debug.DrawRay(raycast.origin, raycast.direction * 100);

        if (Physics.Raycast(raycast, out hit))
        {
            if (hit.collider.gameObject.tag == "WallPlacement")
            {
                if (wall != null)
                {
                    wall.transform.position = grid.GetNearestPoint(hit.point);
                }
                else if (slowField != null)
                {
                    slowField.transform.position = grid.GetNearestPoint(hit.point);
                }
                if (canvasClickAction.GetStateDown(hand.handType))
                {
                    if (hit.collider.gameObject.tag == "WallPlacement")
                    {
                        if (wall != null)
                        {
                            var finalpos = grid.GetNearestPoint(hit.point);
                            GameObject testWall = Instantiate(wall, finalpos, Quaternion.identity);
                            testWall.GetComponent<BoxCollider>().enabled = true;
                            grid.CreateGrid();
                            GameObject.Find("GameobjectManager").GetComponent<Grid>().CreateGrid();
                            Destroy(testWall);
                            testWall = null;

                            PathRequestManager.RequestPath(GameObject.FindGameObjectWithTag("Spawner").transform.position, GameObject.FindGameObjectWithTag("Protect").transform.position, WallStuff);
                        }
                        else if (slowField != null)
                        {
                            SlowField();
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Activated when a wall is placed.
    /// </summary>
    /// <param name="waypoints">A set of waypoints.</param>
    /// <param name="pathSuccessful">If the path succeded.</param>
    void WallStuff(Vector3[] waypoints, bool pathSuccessful)
    {
        Ray raycast = new Ray(hand.transform.position, hand.transform.forward);
        Debug.DrawRay(raycast.origin, raycast.direction * 100);

        RaycastHit hit;

        if (pathSuccessful == true)
        {
            if (Physics.Raycast(raycast, out hit))
            {
                var finalpos = grid.GetNearestPoint(hit.point);
                wall = Instantiate(wall, finalpos, Quaternion.identity);
                Wall();
            }
        }
    }

    /// <summary>
    /// Sets the properties of the tower when placed.
    /// </summary>
    void BoughtTower()
    {
        Color color = boughtTower.GetComponent<Renderer>().material.color;
        color.a = 1f;
        boughtTower.GetComponent<Renderer>().material.color = color;

        boughtTower.GetComponent<MeshCollider>().enabled = true;
        boughtTower.GetComponentInChildren<MeshCollider>().enabled = true;
        boughtTower.GetComponent<Tower>().enabled = true;

        boughtTower = null;
        PurchaseSpace.boughtTower = null;
    }

    /// <summary>
    /// Sets the properties of the teleportpoint when placed.
    /// </summary>
    void TeleportPoint()
    {
        teleportPoint.GetComponentInChildren<SphereCollider>().enabled = true;

        teleportPoint = null;
        PurchaseSpace.teleportPoint = null;
    }

    /// <summary>
    /// Sets the properties of the wall when placed.
    /// </summary>
    void Wall()
    {
        Color color = wall.GetComponent<Renderer>().material.color;
        color.a = 1f;
        wall.GetComponent<Renderer>().material.color = color;

        wall.GetComponent<BoxCollider>().enabled = true;

        wall = null;
        PurchaseSpace.Wall = null;
    }

    /// <summary>
    /// Sets the properties of the slowfield when placed.
    /// </summary>
    void SlowField()
    {
        slowField.GetComponent<BoxCollider>().enabled = true;

        slowField = null;
        PurchaseSpace.slowField = null;
    }

    void Update()
    {
        Ray raycast = new Ray(hand.transform.position, hand.transform.forward);
        Debug.DrawRay(raycast.origin, raycast.direction * 100);
        RaycastHit hit;

        //                                                              Refunds the bought item.
        if (cancelBuildingAction.GetStateDown(hand.handType))
        {
            if (teleportPoint != null)
            {
                Destroy(teleportPoint);
                teleportPoint = null;
                PurchaseSpace.teleportPoint = null;
                Money.amount += 5f;
            }
            if (boughtTower != null)
            {
                Destroy(boughtTower);
                boughtTower = null;
                PurchaseSpace.boughtTower = null;
                Money.amount += 40f;
            }
            if (wall != null)
            {
                Destroy(wall);
                wall = null;
                PurchaseSpace.Wall = null;
                Money.amount += 5f;
            }
            if (slowField != null)
            {
                Destroy(slowField);
                slowField = null;
                PurchaseSpace.slowField = null;
                Money.amount += 20;
            }
        }

        //                                                              Clicks the button aimed at.
        if (canvasClickAction.GetStateDown(hand.handType))
        {
            if (Physics.Raycast(raycast, out hit))
            {
                if (hit.collider.gameObject.tag == "Button")
                {
                    GameObject button;
                    button = hit.collider.gameObject.GetComponentInChildren<Button>().gameObject;
                    button.GetComponent<Button>().onClick.Invoke();
                    if (PurchaseSpace.boughtTower != null)
                    {
                        boughtTower = PurchaseSpace.boughtTower;
                    }
                    if (PurchaseSpace.teleportPoint != null)
                    {
                        teleportPoint = PurchaseSpace.teleportPoint;
                    }
                    if (PurchaseSpace.Wall != null)
                    {
                        wall = PurchaseSpace.Wall;
                    }
                    if (PurchaseSpace.slowField != null)
                    {
                        slowField = PurchaseSpace.slowField;
                    }
                }
            }
        }

        //                                                              Checks which item was bought and activated the right script.
        if (boughtTower != null || teleportPoint != null)
        {
            MovePurchasedItem();
            CheckForWall();
        }
        if (slowField != null || wall != null)
        {
            MovePurchasedItem();
            CheckForGround();
        }
    }
}
