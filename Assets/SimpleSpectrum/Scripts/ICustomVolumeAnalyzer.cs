using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICustomVolumeAnalyzer
{
    void ProcessNewVolume(float newVal);
}