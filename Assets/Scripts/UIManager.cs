using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance { get { return instance; }  }

    [SerializeField] Material mat;
    [SerializeField] GameObject speedSliderObject;
    [SerializeField] GameObject radiusSliderObject;
    [SerializeField] GameObject maxItSliderObject;
    [SerializeField] GameObject colorSliderObject;
    [SerializeField] TMP_Text positionText;
    [SerializeField] TMP_Text zoomText;
    [SerializeField] FractalManager fractalManager;




    Slider speedSlider;
    TMP_Text speedText;

    Slider radiusSlider;
    TMP_Text radiusText;

    Slider maxItSlider;
    TMP_Text maxItText;

    Slider colorSlider;
    TMP_Text colorText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }    
    }
    // Start is called before the first frame update
    void Start()
    {

        speedSlider = speedSliderObject.transform.GetChild(1).GetComponent<Slider>();
        radiusSlider = radiusSliderObject.transform.GetChild(1).GetComponent<Slider>();
        maxItSlider = maxItSliderObject.transform.GetChild(1).GetComponent<Slider>();
        colorSlider = colorSliderObject.transform.GetChild(1).GetComponent<Slider>();

        speedText = speedSliderObject.transform.GetChild(0).GetComponent<TMP_Text>();
        radiusText = radiusSliderObject.transform.GetChild(0).GetComponent<TMP_Text>();
        maxItText = maxItSliderObject.transform.GetChild(0).GetComponent<TMP_Text>();
        colorText = colorSliderObject.transform.GetChild(0).GetComponent<TMP_Text>();

        colorSlider.value = mat.GetFloat("_Color");
        maxItSlider.value = mat.GetFloat("_MaxIt");
        radiusSlider.value = mat.GetFloat("_Radius");
        speedSlider.value = mat.GetFloat("_Speed");

        //slider text
        colorText.text = $"Color: {colorSlider.value}";
        maxItText.text = $"MaxIt: {maxItSlider.value}";
        radiusText.text = $"Radius: {radiusSlider.value}";
        speedText.text = $"Speed: {speedSlider.value}";

    }

    // Update is called once per frame
    void Update()
    {
        positionText.text = $"Position: \n({fractalManager.pos.x}, {fractalManager.pos.y})";
        zoomText.text = $"Zoom: \n{fractalManager.scale}";

    }

    public void ChangeColor()
    {
        if (colorSlider != null)
        {
            mat.SetFloat("_Color", colorSlider.value);
            colorText.text = $"Color: {colorSlider.value}";

        }
    }

    public void ChangeMaxIt()
    {
        if (maxItSlider != null)
        {
            mat.SetFloat("_MaxIt", maxItSlider.value);
            maxItText.text = $"MaxIt: {maxItSlider.value}";

        }
    }

    public void ChangeRadius()
    {
        if (radiusSlider != null)
        {
            mat.SetFloat("_Radius", radiusSlider.value);
            radiusText.text = $"Radius: {radiusSlider.value}";

        }
    }

    public void ChangeSpeed()
    {
        if (speedSlider != null)
        {
            mat.SetFloat("_Speed", speedSlider.value);
            speedText.text = $"Speed: {speedSlider.value}";
        }
    }
}
