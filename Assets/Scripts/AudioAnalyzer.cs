using System;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent (typeof (AudioSource))]
public class AudioAnalyzer : MonoBehaviour
{
    AudioSource _audioSource;
    public float[] samples = new float[512];
    public float[] _freqBand = new float[8];
    public int N;
    public float fs;
    public float NFreq;
    public float BDuration;
    public float FreqRes;
    

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        GetSpectrumSource();
        GenerateFrequencyBands();
    }

    void GetSpectrumSource()
    {

        //_audioSource.GetSpectrumData(samples, 0, FFTWindow.Hanning);
        FFT(_audioSource, false);
    }

    void FFT(AudioSource _audioSource, bool invert)
    {
        float[] _samples = new float[1024];
        _audioSource.GetOutputData(_samples, 0);
        N = _samples.Length;
        fs = _audioSource.clip.frequency;
        NFreq = fs / 2;
        BDuration = N / fs;
        FreqRes = fs / N;

        float[] win = Hanning(N);
       

        List<Complex> samplesList = new List<Complex>();
        for(int i = 0; i < N; i++)
        {
            samplesList.Add(new Complex(_samples[i] * win[i], 0));
        }

        //sort
        for(int i = 1, j=0; i < N; i++)
        {
            int bit = N >> 1;
            for ( ; Convert.ToBoolean(j & bit); bit >>= 1) 
            {
                j ^= bit;
            }
            j ^= bit;

            if (i < j)
            {
                Complex tempi = samplesList[i];
                Complex tempj = samplesList[j];

                samplesList[i] = tempj;
                samplesList[j] = tempi;
            }
        }

        for (int len = 2; len <= N; len <<= 1)
        {
            double ang = (2 * Math.PI / len) * (invert ? -1 : 1);
            Complex wlen = new Complex(Math.Cos(ang), Math.Sin(ang));
            for (int i = 0; i < N; i += len)
            {
                Complex w = new Complex(1, 0);
                for (int j = 0; j < len / 2; j++)
                {
                    Complex u = samplesList[i + j];
                    Complex v = samplesList[i + j + len / 2] * w;

                    samplesList[i + j] = u + v;
                    samplesList[i + j + len / 2] = u - v;
                    w *= wlen;
                }
            }
        }

        for (int i = 0; i < N/2; i++)
        {
            if (i == 0)
            {
                samples[i] = 2 * ((float)samplesList[i].Magnitude + (float)samplesList[i + 1].Magnitude) / 2 / N;
                
            }else if (i == N - 1)
            {
                samples[i] = 2 * ((float)samplesList[i].Magnitude + (float)samplesList[i - 1].Magnitude) / 2 / N;
            }
            else
            {
                samples[i] = 2 * ((float)samplesList[i - 1].Magnitude + (float)samplesList[i].Magnitude + (float)samplesList[i + 1].Magnitude) / 3 / N;
            }
        }
    }

    float[] Hanning(int N)
    {
        
        float[] temp = new float[N];
        for (int i = 0; i < N; i++) 
        {
            temp[i] = (float)(0.5f + 0.5f * Math.Cos(2 * Math.PI * (i + N / 2) / (N - 1)));
        }
        return temp;
    }

    void GenerateFrequencyBands()
    {
        int count = 0;

        for(int i = 0; i < 8; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i + 1);
            //
            //if (i == 7)
            //{
            //    sampleCount += 2;
            //}
            for(int j = 0; j < sampleCount; j++)
            {
                average += samples[count] * (count + 1);
                count++;
            }
            average /= count;
            _freqBand[i] = average;
        }
    }
}
