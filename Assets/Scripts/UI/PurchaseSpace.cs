/*

            Handles the purchaseSpace logic.

*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;

/// <summary>
/// A script that is put on the PurchaseSpace canvas.
/// </summary>
public class PurchaseSpace : MonoBehaviour
{
    /// <summary>
    /// A text showing how much money you have.
    /// </summary>
    public Text moneyText;
    /// <summary>
    /// A text showing which wave you're at.
    /// </summary>
    public Text waveText;
    /// <summary>
    /// A basicTower prefab.
    /// </summary>
    public GameObject basicTowerPrefab;
    /// <summary>
    /// A teleportPoint prefab.
    /// </summary>
    public GameObject teleportPointPrefab;
    /// <summary>
    /// A wall prefab.
    /// </summary>
    public GameObject WallPrefab;
    /// <summary>
    /// A slowField prefab.
    /// </summary>
    public GameObject slowFieldPrefab;
    /// <summary>
    /// The tower object.
    /// </summary>
    public static GameObject boughtTower;
    /// <summary>
    /// The teleportPoint object.
    /// </summary>
    public static GameObject teleportPoint;
    /// <summary>
    /// The wall object.
    /// </summary>
    public static GameObject Wall;
    /// <summary>
    /// The slowField object.
    /// </summary>
    public static GameObject slowField;
    /// <summary>
    /// The spawner object.
    /// </summary>
    public GameObject spawner;
    public enum MenuStates { Main, Lose, Won };
    public static MenuStates currentstate;

    public GameObject mainMenu;
    public GameObject howToMenu;
    public GameObject wonMenu;

    void Awake()
    {
        currentstate = MenuStates.Main;
    }

    void Update()
    {
        moneyText.text = "Money " + Money.amount + " Units";
        waveText.text = "Wave " + spawner.GetComponent<Spawner>().currentWave + "/" + spawner.GetComponent<Spawner>().waves.Count;
        switch (currentstate)
        {
            case MenuStates.Main:
                mainMenu.SetActive(true);
                howToMenu.SetActive(false);
                wonMenu.SetActive(false);
                break;
            case MenuStates.Lose:
                howToMenu.SetActive(true);
                mainMenu.SetActive(false);
                wonMenu.SetActive(false);
                break;
            case MenuStates.Won:
                wonMenu.SetActive(true);
                mainMenu.SetActive(false);
                howToMenu.SetActive(false);
                break;
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void StartAgain()
    {
        SceneManager.LoadScene("Level 1");
    }

    /// <summary>
    /// Creates the Tower object. Activated when the button is pressed.
    /// </summary>
    public void BuyBasicTower()
    {
        if (Money.amount < 30 || boughtTower != null)
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

        Money.amount -= 30;
    }

    /// <summary>
    /// Creates the teleportPoint object. Activated when the button is pressed.
    /// </summary>
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

    /// <summary>
    /// Creates the wall object. Activated when the button is pressed.
    /// </summary>
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

    /// <summary>
    /// Creates the slowField object. Activated when the button is pressed.
    /// </summary>
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

    /// <summary>
    /// Starts the waves. Activated when the button is pressed.
    /// </summary>
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
