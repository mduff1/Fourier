using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractalManager : MonoBehaviour
{
    public Material mat;
    public Vector2 pos;
    public float scale, angle;
    public float freqBand0, freqBand1, freqBand2, freqBand3, freqBand4, freqBand5, freqBand6, freqBand7;
    float scaler = 1.0f;

    private Vector2 smoothPos;
    private float smoothScale, smoothAngle;

    


    
    private void UpdateShader()
    {
        freqBand0 = GetComponent<AudioAnalyzer>()._freqBand[0];
        freqBand1 = GetComponent<AudioAnalyzer>()._freqBand[1];
        freqBand2 = GetComponent<AudioAnalyzer>()._freqBand[2];
        freqBand3 = GetComponent<AudioAnalyzer>()._freqBand[3];
        freqBand4 = GetComponent<AudioAnalyzer>()._freqBand[4];
        freqBand5 = GetComponent<AudioAnalyzer>()._freqBand[5];
        freqBand6 = GetComponent<AudioAnalyzer>()._freqBand[6];
        freqBand7 = GetComponent<AudioAnalyzer>()._freqBand[7];



        smoothPos = Vector2.Lerp(smoothPos, pos, 0.03f);
        smoothScale = Mathf.Lerp(smoothScale, scale, 0.05f);
        smoothAngle = Mathf.Lerp(smoothAngle, angle, 0.03f);
        float aspect = (float)Screen.width / (float)Screen.height;
        float scaleX = smoothScale;
        float scaleY = smoothScale;
       
        if (aspect > 1.0f)
        {
            scaleY /= aspect;
        }
        else
        {
            scaleX *= aspect;
        }
        mat.SetVector("_Area", new Vector4(smoothPos.x, smoothPos.y, scaleX, scaleY));
        mat.SetFloat("_Angle", smoothAngle);
        mat.SetFloat("_freqBand0", freqBand0 * scaler);
        mat.SetFloat("_freqBand1", freqBand1 * scaler);
        mat.SetFloat("_freqBand2", freqBand2 * scaler);
        mat.SetFloat("_freqBand3", freqBand3 * scaler);
        mat.SetFloat("_freqBand4", freqBand4 * scaler);
        mat.SetFloat("_freqBand5", freqBand5 * scaler);
        mat.SetFloat("_freqBand6", freqBand6 * scaler);
        mat.SetFloat("_freqBand7", freqBand7 * scaler);
        //mat.SetVector("_Resolution", new Vector4(Screen.width, Screen.height, 0,0));


    }

    private void HandleInputs()
    {
        if (Input.mouseScrollDelta.y > 0)
            scale *= 0.5f;
        if (Input.mouseScrollDelta.y < 0)
            scale *= 1.5f;
        if (Input.GetKey(KeyCode.Q))
            angle += 0.01f;
        if (Input.GetKey(KeyCode.E))
            angle -= 0.01f;

        Vector2 dir = new Vector2(0.01f*scale, 0);
        float s = Mathf.Sin(angle);
        float c = Mathf.Cos(angle);
        dir = new Vector2(dir.x * c - dir.y * s, dir.x * s + dir.y * c);
        if (Input.GetKey(KeyCode.A))
            pos -= dir;
        if (Input.GetKey(KeyCode.D))
            pos += dir;

        dir = new Vector2(-dir.y, dir.x);
        if (Input.GetKey(KeyCode.W))
            pos += dir;
        if (Input.GetKey(KeyCode.S))
            pos -= dir;

    }

    public void FixedUpdate()
    {
        HandleInputs();
        UpdateShader();
    }
}
