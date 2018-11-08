using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class PurchaseSpace : MonoBehaviour
{
    public Text moneyText;
    public Text waveText;
    public GameObject basicTowerPrefab;
    public GameObject teleportPointPrefab;
    public GameObject WallPrefab;
    public GameObject slowFieldPrefab;
    public static GameObject boughtTower;
    public static GameObject teleportPoint;
    public static GameObject Wall;
    public static GameObject slowField;
    public GameObject spawner;

    void Update()
    {
        moneyText.text = "Money " + Money.amount + " Units";
        waveText.text = "Wave " + spawner.GetComponent<Spawner>().currentWave + "/" + spawner.GetComponent<Spawner>().waves.Count;
    }

    public void BuyBasicTower()
    {
        if (Money.amount < 40 || boughtTower != null)
        {
            return;
        }
        if (teleportPoint != null || Wall != null || slowField != null)
        {
            return;
        }

        boughtTower = Instantiate(basicTowerPrefab, transform.position, Quaternion.identity);

        Color color = boughtTower.GetComponent<Renderer>().material.color;
        color.a = 0.5f;
        boughtTower.GetComponent<Renderer>().material.color = color;

        boughtTower.GetComponent<MeshCollider>().enabled = false;
        boughtTower.GetComponentInChildren<MeshCollider>().enabled = false;

        Money.amount -= 40;
    }

    public void BuyTeleportPoint()
    {
        if (Money.amount < 5 || teleportPoint != null)
        {
            return;
        }
        if (boughtTower != null || Wall != null || slowField != null)
        {
            return;
        }

        teleportPoint = Instantiate(teleportPointPrefab, transform.position, Quaternion.identity);

        teleportPoint.GetComponentInChildren<SphereCollider>().enabled = false;

        Money.amount -= 5;
    }

    public void BuyWall()
    {
        if (Money.amount < 5 || Wall != null)
        {
            return;
        }
        if (teleportPoint != null || boughtTower != null || slowField != null)
        {
            return;
        }

        Wall = Instantiate(WallPrefab, transform.position, Quaternion.identity);
        Wall.GetComponent<BoxCollider>().enabled = false;
        Color color = Wall.GetComponent<Renderer>().material.color;
        color.a = 0.5f;
        Wall.GetComponent<Renderer>().material.color = color;

        

        Money.amount -= 5;
    }

    public void BuySlowField()
    {
        if (Money.amount < 20 || slowField != null)
        {
            return;
        }
        if (teleportPoint != null || boughtTower != null || Wall != null)
        {
            return;
        }

        slowField = Instantiate(slowFieldPrefab, transform.position, Quaternion.identity);

        Color color = slowField.GetComponent<Renderer>().material.color;
        color.a = 0.5f;
        slowField.GetComponent<Renderer>().material.color = color;

        slowField.GetComponent<BoxCollider>().enabled = false;

        Money.amount -= 20;
    }

    public void StartWave()
    {
        spawner.GetComponent<Spawner>().IsButtonClicked = true;
        spawner.GetComponent<Spawner>().GetComponentInParent<Grid>().CreateGrid();
        Destroy(teleportPoint);
        teleportPoint = null;
        Destroy(boughtTower);
        boughtTower = null;
        Destroy(Wall);
        Wall = null;
        Destroy(slowField);
        slowField = null;

    }
}
