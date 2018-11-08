/*

            Handles the logic for creating a lazer.

*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script that is put on the hands.
/// </summary>
public class Laser : MonoBehaviour
{
    /// <summary>
    /// A LineRenderer.
    /// </summary>
    private LineRenderer lr;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    void Update()
    {
        Ray raycast = new Ray(gameObject.transform.position, gameObject.transform.forward);
        Debug.DrawRay(raycast.origin, raycast.direction * 100);

        lr.SetPosition(0, new Vector3(transform.position.x, transform.position.y, transform.position.z));
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.position, out hit))
        {
            if (hit.collider)
            {
                lr.SetPosition(1, hit.point);
            }
        }
        else
        {
            lr.SetPosition(1, transform.forward * 5000);
        }
    }
}
