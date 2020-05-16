using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterWavesController : MonoBehaviour
{
    public class Wave
    {
        public Wave(Transform wave)
        {
            waveTransform = wave;
            Height = wave.position.y;
        }

        public void SetWave (Transform wave)
        {
            waveTransform = wave;
            Height = wave.position.y;
        }

        public Transform waveTransform;
        public float Height;
        public float Angle;
    }
    [Range(0.0001f, 0.3f)]
    public float WaveStep;

    [Range(1.0f, 100.0f)]
    public float WavingCalmness;

    private List<Wave> WaterWaves;

    private bool isWaving;

    void Start()
    {
        WaterWaves = new List<Wave>(transform.childCount);

        for (int i = 0; i < transform.childCount; i++)
        {
            WaterWaves.Add(new Wave(transform.GetChild(i)));
            WaterWaves[i].Angle = WavingCalmness * i * WaveStep;
        }

        StartWaves();
    }

    public void StartWaves()
    {
        isWaving = true;
    }

    private void FixedUpdate()
    {
        if (isWaving)
        {
            for (int i = 0; i < WaterWaves.Count; i++)
            {
                WaterWaves[i].Angle += WaveStep;
                if (WaterWaves[i].Angle >= 360)
                    WaterWaves[i].Angle -= 360;

                WaterWaves[i].Height += Mathf.Sin(WaterWaves[i].Angle) / WavingCalmness;

                WaterWaves[i].waveTransform.position = new Vector3(WaterWaves[i].waveTransform.position.x,
                                                WaterWaves[i].Height, WaterWaves[i].waveTransform.position.z);
            }
        }
    }
}
