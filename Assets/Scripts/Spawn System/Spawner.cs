using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Spawner : MonoBehaviour
{
    public float initialWaitTime = 1.5f;
    public float waveWaitTime = 1.5f;


    public GameObject[] enemyPrefabs;
    public Transform TargetPoint;

    public Text waveText;

    public string nextLevel = "level #";

    public List<Wave> waves = new List<Wave>();
    public int currentWave = 0;

    public bool IsButtonClicked = false;

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

    void GetWaves()
    {
        int child = 0;

        while (child < transform.childCount)
        {
            waves.Add(transform.GetChild(child).GetComponent<Wave>());
            child++;
        }
    }

    IEnumerator WaveLoop()
    {
        yield return new WaitForSeconds(initialWaitTime);

        if (currentWave < waves.Count)
        {
            currentWave++;
            yield return StartCoroutine(StartNextWave());
        }

    }

    IEnumerator StartNextWave()
    {
        foreach (WaveSegment segment in waves[currentWave - 1].PatternToWaveSegments())
        {
            yield return StartCoroutine(SpawnEnemies(segment.spawns));

            yield return new WaitForSeconds(segment.wait);
        }
    }

    IEnumerator SpawnEnemies(List<int> enemies)
    {
        foreach (int enemy in enemies)
        {
            GameObject newEnemy = Instantiate(enemyPrefabs[enemy], transform.position, Quaternion.identity);

            newEnemy.GetComponent<Enemy>().target = TargetPoint;

            yield return new WaitForSeconds(0.5f);
        }
    }
}
