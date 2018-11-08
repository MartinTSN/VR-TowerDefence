using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmosTest : MonoBehaviour
{
    private Grid grid;
    public GameObject wall;
    private GameObject Wall;
    private bool placeholder = false;

    private void Awake()
    {
        grid = GameObject.Find("GameobjectManager").GetComponent<Grid>();
    }

    void Update()
    {
        Ray raycast = new Ray(gameObject.transform.position, gameObject.transform.forward);
        Debug.DrawRay(raycast.origin, raycast.direction * 100);

        RaycastHit hit;

        if (Physics.Raycast(raycast, out hit))
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (hit.collider.gameObject.tag == "WallPlacement")
                {
                    PlaceCube(hit.point);
                    GameObject.Find("GameobjectManager").GetComponent<Grid>().CreateGrid();
                    Destroy(Wall);
                    Wall = null;
                    PathRequestManager.RequestPath(GameObject.FindGameObjectWithTag("Spawner").transform.position, GameObject.FindGameObjectWithTag("Protect").transform.position, OnPathFound);
                    

                }
            }
        }
    }

    public void OnPathFound(Vector3[] waypoints, bool pathSuccessful)
    {
        Ray raycast = new Ray(gameObject.transform.position, gameObject.transform.forward);
        Debug.DrawRay(raycast.origin, raycast.direction * 100);

        RaycastHit hit;

        if (pathSuccessful == true)
        {
            if (Physics.Raycast(raycast, out hit))
            {
                PlaceCube(hit.point);
            }
        }
    }

    private void PlaceCube(Vector3 clickPoint)
    {
        var finalpos = grid.GetNearestPoint(clickPoint);
        Wall = Instantiate(wall, finalpos, Quaternion.identity);
    }
}
