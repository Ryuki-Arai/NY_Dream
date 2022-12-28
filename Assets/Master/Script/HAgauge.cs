using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HAgauge : MonoBehaviour
{
    [SerializeField]
    Sprite[] _gaugeImage;

    float _time;
    [SerializeField] float _interval;

    Image _image;

    bool _isFever = true;

    int value;
    public int Value
    {
        get { return value; }
        set 
        {
            if (_isFever)
            {
                this.value = value;
                Debug.Log(this.value);
                StateChange();
            }
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
        SetZero();
    }

    public void SetZero()
    {
        value = 0;
    } 

    public void ResetValue()
    {
        StateChange();
        StartCoroutine(FeverInterval());
    }

    void StateChange()
    {
        if (value >= maxValue)
        {
            _image.sprite = _gaugeImage[3];
            _isFever = false;
        }
        else if (value >= (maxValue / 3) * 2) _image.sprite = _gaugeImage[2];
        else if (value >= maxValue / 3) _image.sprite = _gaugeImage[1];
        else _image.sprite = _gaugeImage[0];
    }
    public IEnumerator FeverInterval()
    {
        _time = 0;
        _image.enabled = false;
        while(_interval > _time)
        {
            yield return new WaitForEndOfFrame();
            _time += Time.deltaTime;
        }
        _image.enabled = true;
        _isFever = true;
    }
}
