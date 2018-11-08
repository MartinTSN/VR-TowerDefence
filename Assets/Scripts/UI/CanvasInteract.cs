using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class CanvasInteract : MonoBehaviour
{

    public SteamVR_Action_Boolean canvasClickAction;
    public SteamVR_Action_Boolean cancelBuildingAction;
    private Grid grid;
    Hand hand;
    GameObject boughtTower = PurchaseSpace.boughtTower;
    GameObject teleportPoint = PurchaseSpace.teleportPoint;
    GameObject wall = PurchaseSpace.Wall;
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
                else if (slowField != null)
                {
                    slowField.transform.position = hit.collider.gameObject.transform.position;
                    hit.collider.gameObject.GetComponent<Renderer>().enabled = false;
                }

                foreach (var item in GameObject.FindGameObjectsWithTag("Wall"))
                {
                    item.GetComponent<Renderer>().enabled = true;
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
                    else if (slowField != null)
                    {
                        SlowField();
                    }
                }
            }
            if (wall != null)
            {
                if (hit.collider.gameObject.tag == "WallPlacement")
                {
                    wall.transform.position = grid.GetNearestPoint(hit.point);
                }
                if (canvasClickAction.GetStateDown(hand.handType))
                {
                    if (hit.collider.gameObject.tag == "WallPlacement")
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
                }
            }

        }
    }

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

    void TeleportPoint()
    {
        teleportPoint.GetComponentInChildren<SphereCollider>().enabled = true;

        teleportPoint = null;
        PurchaseSpace.teleportPoint = null;
    }

    void Wall()
    {
        print("At Wall Script");
        Color color = wall.GetComponent<Renderer>().material.color;
        color.a = 1f;
        wall.GetComponent<Renderer>().material.color = color;

        wall.GetComponent<BoxCollider>().enabled = true;

        wall = null;
        PurchaseSpace.Wall = null;
    }

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
        if (boughtTower != null || teleportPoint != null || slowField != null || wall != null)
        {
            MovePurchasedItem();
            CheckForWall();
        }
    }
}
