/*

            Handles the waves.

*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The wave Segments.
/// </summary>
public class WaveSegment
{
    /// <summary>
    /// A list of enemies that are going to spawn.
    /// </summary>
    public List<int> spawns = new List<int>();
    /// <summary>
    /// The wait-Time between enemies.
    /// </summary>
    public int wait;
}

/// <summary>
/// Makes the text into Waves.
/// </summary>
public class Wave : MonoBehaviour
{
    /// <summary>
    /// A string Pattern for enemies.
    /// </summary>
    [TextArea(3, 10)]
    public string pattern;

    /// <summary>
    /// Makes the pattern into WaveSegments.
    /// </summary>
    /// <returns></returns>
    public WaveSegment[] PatternToWaveSegments()
    {
        string[] lines = pattern.Split('\n');

        List<WaveSegment> segments = new List<WaveSegment>();

        foreach (string line in lines)
        {
            WaveSegment segment = new WaveSegment();

            string[] spawns = line.Split(' ');

            for (int i = 0; i < spawns.Length - 1; i++)
            {
                segment.spawns.Add(int.Parse(spawns[i]));
            }

            segment.wait = int.Parse(spawns[spawns.Length - 1]);

            segments.Add(segment);
        }

        return segments.ToArray();
    }
}
