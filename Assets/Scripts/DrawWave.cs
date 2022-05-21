using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawWave : MonoBehaviour
{
    public int width = 500;
    public int height = 100;
    public Color waveformColor = Color.green;
    public Color bgColor = Color.black;
    public float sat = .5f;

    //[SerializeField] Image img;
    [SerializeField] AudioSource _audioSource;


    void Start()
    {
        Image img = GetComponent<Image>();

        Texture2D texture = DrawSpectrum(_audioSource, sat, width, height, waveformColor);
        img.overrideSprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        //DrawInput(_audioSource, sat, width, height, waveformColor);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public Texture2D DrawSpectrum(AudioSource _audioSource, float saturation, int width, int height, Color col)
    {
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        float[] samples = new float[1024];
        float[] waveform = new float[width];
        _audioSource.GetOutputData(samples, 0);
        //clip.GetData(samples, 0);
        int packSize = (samples.Length / width) + 1;
        int s = 0;
        for(int i = 0; i < samples.Length; i += packSize)
        {
            waveform[s] = Mathf.Abs(samples[i]);
            s++;
        }
       for(int i = 0; i < waveform.Length; i++)
       {
           waveform[i] = waveform[i] / Mathf.Max(waveform);
       }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                tex.SetPixel(x, y, bgColor);
            }
        }

        for (int x = 0; x < waveform.Length; x++)
        {
            for (int y = 0; y <= waveform[x] * ((float)height * .75f); y++)
            {
                tex.SetPixel(x, (height / 2) + y, col);
                tex.SetPixel(x, (height / 2) - y, col);
            }
        }
        tex.Apply();
        return tex;
    }

    public Texture2D DrawInput(AudioSource _audioSource, float saturation, int width, int height, Color col)
    {
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        float[] _samples = new float[_audioSource.clip.samples];
        float[] _waveform = new float[_audioSource.clip.samples];
        _audioSource.clip.GetData(_samples, 0);

        int s = 0;
        for (int i = 0; i < _samples.Length; i++)
        {
            _waveform[s] = Mathf.Abs(_samples[i]);
            s++;
        }

        return tex;
    }
}
