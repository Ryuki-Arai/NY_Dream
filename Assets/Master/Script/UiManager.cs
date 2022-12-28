using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField, Header("���U���g�̌��ʂ�\������")]
    ResultChange _resultChange;

    [SerializeField, Header("���U���g�̃L�����o�X")]
    Canvas _resultCanvas;

    [SerializeField, Header("�X�R�A��\������e�L�X�g")]
    TMP_Text _scoreText;

    [SerializeField, Header("�Q�[���̐������Ԃ�\������e�L�X�g")]
    TMP_Text _timeText;

    [SerializeField, Header("�^�o�R�̃C���[�W�摜")]
    Image _smongImage;

    [SerializeField, Header("��Q�[�W�̃X���C�_�[")]
    Slider _fanGaugeSlider;

    [SerializeField, Header("�t�B�[�o�[�Q�[�W�̃X���C�_�[")]
    HAgauge _fevarGaugeSlider;

    [SerializeField, Header("�X�R�A�̕ω��ɂ����鎞��")]
    float _changeValueInterval;

    [SerializeField, Header("��Q�[�W�̕ω��ɂ����鎞��")]
    float _changeFanGaugeInterval;

    [SerializeField, Header("�t�B�[�o�[�Q�[�W�̕ω��ɂ����鎞��")]
    float _changeFevarGaugeInterval;

    [SerializeField, Header("�Q�[���̐�������")]
    float _gameTime;

    [SerializeField, Header("������\�����鎞��")]
    float _smongTime;

    [SerializeField, Header("�t�B�[�o�[��\�����鎞��")]
    float _fevarTime;

    [SerializeField, Header("TimeLine�̍Đ��@��")]
    PlayableDirector _timeLine;

    [SerializeField, Header("�t�B�[�o�[����TimeLine")]
    TimelineAsset _feverTime;
    [SerializeField, Header("�t�B�[�o�[�I������TimeLine")]
    TimelineAsset _disFeverTime;

    [SerializeField, Header("��Q�[�W�̍ő�")]
    int _fanSliderValueMax = 10;

    [SerializeField, Header("�t�B�[�o�[�Q�[�W�̍ő�")]
    int _fevarSliderValueMax = 10;

    //�Q�[�����Ԃ̌v���p
    float _timer;

    //�摜��\�����鎞��
    float _eventInterval;

    //�摜�\���̌v���p
    float _eventTimer = 0;

    //�����̉��̃A�j���[�V����
    Animator _smongAni;

    private void Awake()
    {
        GameManager.Instance.SetUI(this);
        _fevarGaugeSlider.SetValue(_fevarSliderValueMax);
        _fanGaugeSlider.maxValue = _fanSliderValueMax;

        _timer = _gameTime;
        _timeText.text = _timer.ToString("00");

        _smongAni = _smongImage.gameObject.GetComponent<Animator>();

        _resultCanvas.enabled = false;
        _timeLine = GetComponent<PlayableDirector>();

        StartCoroutine(GameTime());
    }

    /// <summary>
    /// �X�R�A��Dotween�œ��I�ɕ\������
    /// </summary>
    public void ScoreInterpolation(float scoreValue)
    {
        float sliderValue = float.Parse(_scoreText.text);

        DOTween.To(() => sliderValue, // �A���I�ɕω�������Ώۂ̒l
            x => sliderValue = x, // �ω��������l x ���ǂ��������邩������
            scoreValue, // x ���ǂ̒l�܂ŕω������邩�w������
            _changeValueInterval)
            .OnUpdate(() => _scoreText.text = sliderValue.ToString("000"));
    }

    /// <summary>
    /// ��Q�[�W��Dotween�œ��I�ɕ\������
    /// </summary>
    public void FanGaugeInterpolation(float gaugeValue)
    {
        if (gaugeValue > _fanSliderValueMax) { gaugeValue = _fanSliderValueMax; }
        else if (gaugeValue == _fanSliderValueMax) { return; }

        DOTween.To(() => _fanGaugeSlider.value, // �A���I�ɕω�������Ώۂ̒l
            x => _fanGaugeSlider.value = x, // �ω��������l x ���ǂ��������邩������
            gaugeValue, // x ���ǂ̒l�܂ŕω������邩�w������
            _changeFanGaugeInterval);
    }

    /// <summary>
    /// �t�B�[�o�[�Q�[�W��Dotween�œ��I�ɕ\������
    /// </summary>
    public void FevarGaugeInterpolation(float gaugeValue)
    {
        if (GameManager.Instance.State != GameState.Fevar)
        {
            _fevarGaugeSlider.Value = (int)gaugeValue; //ToDo int��Float��������Ȃ̏C��
            IndicateFevar();
            
            /*DOTween.To(() => _fevarGaugeSlider.value, // �A���I�ɕω�������Ώۂ̒l
                x => _fevarGaugeSlider.value = x, // �ω��������l x ���ǂ��������邩������
                gaugeValue, // x ���ǂ̒l�܂ŕω������邩�w������
                _changeFevarGaugeInterval).OnComplete(() => IndicateFevar());//*/

        }
    }

    /// <summary>
    /// �����̉��o��\������
    /// </summary>
    public void IndicateSmoke()
    {
        if (GameManager.Instance.State != GameState.Fevar && GameManager.Instance.State != GameState.Finish)
        {
            if (_eventTimer == 0)
            {
                _eventInterval = _smongTime;
                StartCoroutine(EventTime());
            }

            _eventInterval = _smongTime;
            _eventTimer = 0;
            _smongAni.SetBool("isSmoke", true);
        }
    }

    /// <summary>
    /// �t�B�[�o�[�̉��o��\������
    /// </summary>
    public void IndicateFevar()
    {
        if (_fevarGaugeSlider.Value == _fevarSliderValueMax)
        {
            if (_eventTimer == 0)
            {
                _eventInterval = _fevarTime;
                _timeLine.playableAsset = _feverTime;
                _timeLine.Play();
                StartCoroutine(EventTime());
            }

            _eventInterval = _fevarTime;
            _eventTimer = 0;

            if (_smongImage.enabled)
            {
                _smongAni.SetBool("isSmoke", false);
            }

            GameManager.Instance.ChangeState(GameState.Fevar);
        }
    }

    public void Fan()
    {
        if (_smongImage.enabled)
        {
            FanGaugeInterpolation(0);

            _smongAni.Play("SmokeEnd");
        }
    }

    private IEnumerator GameTime()
    {
        while (GameManager.Instance.State != GameState.Finish && _timer > 0)
        {
            yield return new WaitForSeconds(1);

            _timer--;
            _timeText.text = _timer.ToString("00");
        }

        if (_timer <= 0)
        {
            GameManager.Instance.ChangeState(GameState.Finish);
            _timer = 0;
            _eventTimer = 0;
            _resultCanvas.enabled = true;
            _resultChange.Result(int.Parse(_scoreText.text));
        }
    }
    private IEnumerator EventTime()
    {
        while (GameManager.Instance.State != GameState.Finish && _eventInterval > _eventTimer)
        {
            yield return new WaitForEndOfFrame();
            
            _eventTimer += Time.deltaTime;
        }

        if (_eventInterval <= _eventTimer)
        {
            _fevarGaugeSlider.Value = 0;
            _timeLine.playableAsset = _disFeverTime;
            _timeLine.Play();
            GameManager.Instance.ChangeState(GameState.PlayGame);
        }
    }
}
