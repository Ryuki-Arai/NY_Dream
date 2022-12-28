using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HAgauge : MonoBehaviour
{
    [SerializeField]
    Sprite[] _gaugeImage;

    Image _image;

    int value;
    public int Value
    {
        get { return value; }
        set 
        { 
            this.value = value;
            StateChange();
        }
    }

    int maxValue;

    private void Start()
    {
        _image = GetComponent<Image>();
    }

    public void SetValue(int val)
    {
        maxValue = val;
        value = 0;
    }

    void StateChange()
    {
        if (value >= maxValue) _image.sprite = _gaugeImage[3];
        else if (value >= (maxValue / 3) * 2) _image.sprite = _gaugeImage[2];
        else if (value >= maxValue / 3) _image.sprite = _gaugeImage[1];
        else _image.sprite = _gaugeImage[0];
        Debug.Log(value);
    }
}
