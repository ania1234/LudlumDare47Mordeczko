using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Implementing beat detection as per article: http://mziccard.me/2015/05/28/beats-detection-algorithms-1/
/// </summary>
public class SimpleColorChangeVolumeAnalyzer : MonoBehaviour, ICustomVolumeAnalyzer
{
    private OutputVolume ov;
    private SpriteRenderer sprite;
    private Image image;

    public Color startColor = Color.white;
    public Color endColor = Color.black;

    public int maxQueue = 42;
    public Queue<float> energyQueue = new Queue<float>();
    private float currentSum;
    private float currentVarianceSum;
    private float currentAverage;

    [Header("Cutoff Coefficients")]
    [Tooltip("How much the cutoff is affected by current variance in energies")]
    public float varianceCoefficient;

    [Tooltip("Constant cutoff coefficient")]
    public float constantCoefficient;

    private void Awake()
    {
        ov = this.GetComponent<OutputVolume>();
        sprite = this.GetComponent<SpriteRenderer>();
        image = this.GetComponent<Image>();
    }

    void ICustomVolumeAnalyzer.ProcessNewVolume(float newVal)
    {
        bool beat = false;
        if (energyQueue.Count == maxQueue)
        {
            var oldVal = energyQueue.Dequeue();
            currentSum -= oldVal;
            currentVarianceSum -= (currentAverage - oldVal) * (currentAverage - oldVal);
        }

        energyQueue.Enqueue(newVal);
        currentSum += newVal;
        currentAverage = currentSum / (float)energyQueue.Count;
        currentVarianceSum += (currentAverage - newVal) * (currentAverage - newVal);
        //TODO: variance

        var C = varianceCoefficient * (currentVarianceSum / (float)energyQueue.Count) + constantCoefficient;
        beat = newVal > C * currentAverage;

        if (beat)
        {

            if (sprite)
            {
                sprite.color = endColor;
            }
            if (image)
            {
                image.color = endColor;
            }
        }
        else
        {
            if (sprite)
            {
                sprite.color = Color.Lerp(sprite.color, startColor, 0.05f);
            }
            if (image)
            {
                image.color = Color.Lerp(image.color, startColor, 0.05f);
            }
        }
    }
}