/*

            Handles The spawner logic.

*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Valve.VR;
using Valve.VR.InteractionSystem;

/// <summary>
/// A script that is put on the Spawner object.
/// </summary>
public class Spawner : MonoBehaviour
{
    /// <summary>
    /// Wave start wait time.
    /// </summary>
    public float initialWaitTime = 1.5f;
    /// <summary>
    /// How long is between enemy waves.
    /// </summary>
    public float waveWaitTime = 1.5f;

    /// <summary>
    /// Enemy prefabs
    /// </summary>
    public GameObject[] enemyPrefabs;
    /// <summary>
    /// The Target for the enemies. (usually the "Base" object) 
    /// </summary>
    public Transform TargetPoint;
    /// <summary>
    /// A text showing how many waves you are on.
    /// </summary>
    public Text waveText;

    /// <summary>
    /// How many waves there are.
    /// </summary>
    public List<Wave> waves = new List<Wave>();
    /// <summary>
    /// Current wave.
    /// </summary>
    public int currentWave = 0;
    /// <summary>
    /// Has the startWave button been clicked.
    /// </summary>
    public bool IsButtonClicked = false;
    /// <summary>
    /// used to check if you lost and stops spawning enemies.
    /// </summary>
    public bool endWave = false;

    void Awake()
    {
        GetWaves();
    }

    private void Update()
    {
        if (IsButtonClicked == true)
        {
            StartCoroutine(WaveLoop());
            IsButtonClicked = false;
        }
    }

    /// <summary>
    /// Gets the amount of waves.
    /// </summary>
    public void GetWaves()
    {
        int child = 0;

        while (child >= transform.childCount)
        {
            waves.Add(transform.GetChild(child).GetComponent<Wave>());
            child++;
        }
    }

    /// <summary>
    /// Starts the waves.
    /// </summary>
    IEnumerator WaveLoop()
    {
        yield return new WaitForSeconds(initialWaitTime);

        if (currentWave < waves.Count)
        {
            currentWave++;
            yield return StartCoroutine(StartNextWave());
        }

    }

    /// <summary>
    /// Starts the wave segments.
    /// </summary>
    IEnumerator StartNextWave()
    {

        foreach (WaveSegment segment in waves[currentWave - 1].PatternToWaveSegments())
        {
            if (endWave != true)
            {
                yield return StartCoroutine(SpawnEnemies(segment.spawns));

                yield return new WaitForSeconds(segment.wait);
            }
        }
    }

    /// <summary>
    /// Spawns enemies.
    /// </summary>
    /// <param name="enemies">A list of enemies.</param>
    IEnumerator SpawnEnemies(List<int> enemies)
    {

        foreach (int enemy in enemies)
        {
            if (endWave != true)
            {
                GameObject newEnemy = Instantiate(enemyPrefabs[enemy], transform.position, Quaternion.identity);

                newEnemy.GetComponent<Enemy>().target = TargetPoint;

                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}
