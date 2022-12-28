using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ResultChange : MonoBehaviour
{
    [SerializeField]
    Sprite _badImage = null;
    [SerializeField]
    Sprite _normalImage = null;
    [SerializeField]
    Sprite _happyImage = null;
    Image _panelImage = null;
    [SerializeField]
    GameObject _panel = null;
    [SerializeField]
    TextMeshProUGUI _scoreText = null;
    [SerializeField]
    int _badScore = 0;
    [SerializeField]
    int _normalScore = 0;
    [SerializeField]
    int _happyScore = 0;

    private void Start()
    {
        _panelImage = _panel.GetComponent<Image>();
    }
    // Update is called once per frame
    public void Result(int score)
    {
        if(_happyScore <= score)
        {
            _panelImage.sprite = _happyImage;
            GameManager.InstanceSM.CallSound(SoundType.BGM, 4);
        }
        else if(_normalScore <= score)
        {
            _panelImage.sprite = _normalImage;
            GameManager.InstanceSM.CallSound(SoundType.BGM, 5);
        }
        else 
        {
            _panelImage.sprite = _badImage;
            GameManager.InstanceSM.CallSound(SoundType.BGM, 6);
        }
        _scoreText.text = "‚Æ‚­‚Ä‚ñ " + score.ToString();
    }
}
