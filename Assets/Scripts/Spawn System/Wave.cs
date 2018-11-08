using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSegment
{
    public List<int> spawns = new List<int>();
    public int wait;
}

public class Wave : MonoBehaviour
{

    [TextArea(3, 10)]
    public string pattern;

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
