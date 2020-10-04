using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Data/New Item", order = 1)]
public class ItemInfo : ScriptableObject
{
    public new string name;
    public Sprite icon;
    public Sprite capacityIcon;

    [Tooltip("Use 0 and 1")]
    [TextArea(4,4)]
    public string pattern;

    public List<int[]> GetPattern()
    {
        List<int[]> result = new List<int[]>();
        var lines = pattern.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i].Trim();
            if (line.Length > 0)
            {
                int[] lineAsInt = new int[line.Length];
                for (int pos = 0; pos < line.Length; pos++)
                {
                    lineAsInt[pos] = line[pos] == '0' ? 0 : 1;
                }
                result.Add(lineAsInt);
            }
        }
        return result;
    }

    internal float GetXSize()
    {
        return pattern.Split('\n')[0].Length;
    }

    internal float GetYSize()
    {
        return pattern.Split('\n').Length;
    }
}
