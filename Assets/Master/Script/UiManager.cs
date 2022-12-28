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
    [SerializeField, Header("リザルトの結果を表示する")]
    ResultChange _resultChange;

    [SerializeField, Header("リザルトのキャンバス")]
    Canvas _resultCanvas;

    [SerializeField, Header("スコアを表示するテキスト")]
    TMP_Text _scoreText;

    [SerializeField, Header("ゲームの制限時間を表示するテキスト")]
    TMP_Text _timeText;

    [SerializeField, Header("タバコのイメージ画像")]
    Image _smongImage;

    [SerializeField, Header("扇ゲージのスライダー")]
    Slider _fanGaugeSlider;

    [SerializeField, Header("フィーバーゲージのスライダー")]
    HAgauge _fevarGaugeSlider;

    [SerializeField, Header("スコアの変化にかける時間")]
    float _changeValueInterval;

    [SerializeField, Header("扇ゲージの変化にかける時間")]
    float _changeFanGaugeInterval;

    [SerializeField, Header("フィーバーゲージの変化にかける時間")]
    float _changeFevarGaugeInterval;

    [SerializeField, Header("ゲームの制限時間")]
    float _gameTime;

    [SerializeField, Header("煙草を表示する時間")]
    float _smongTime;

    [SerializeField, Header("フィーバーを表示する時間")]
    float _fevarTime;

    [SerializeField, Header("TimeLineの再生機器")]
    PlayableDirector _timeLine;

    [SerializeField, Header("フィーバー時のTimeLine")]
    TimelineAsset _feverTime;
    [SerializeField, Header("フィーバー終了時のTimeLine")]
    TimelineAsset _disFeverTime;

    [SerializeField, Header("扇ゲージの最大")]
    int _fanSliderValueMax = 10;

    [SerializeField, Header("フィーバーゲージの最大")]
    int _fevarSliderValueMax = 10;

    //ゲーム時間の計測用
    float _timer;

    //画像を表示する時間
    float _eventInterval;

    //画像表示の計測用
    float _eventTimer = 0;

    //煙草の煙のアニメーション
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
    /// スコアをDotweenで動的に表示する
    /// </summary>
    public void ScoreInterpolation(float scoreValue)
    {
        float sliderValue = float.Parse(_scoreText.text);

        DOTween.To(() => sliderValue, // 連続的に変化させる対象の値
            x => sliderValue = x, // 変化させた値 x をどう処理するかを書く
            scoreValue, // x をどの値まで変化させるか指示する
            _changeValueInterval)
            .OnUpdate(() => _scoreText.text = sliderValue.ToString("000"));
    }

    /// <summary>
    /// 扇ゲージをDotweenで動的に表示する
    /// </summary>
    public void FanGaugeInterpolation(float gaugeValue)
    {
        if (gaugeValue > _fanSliderValueMax) { gaugeValue = _fanSliderValueMax; }
        else if (gaugeValue == _fanSliderValueMax) { return; }

        DOTween.To(() => _fanGaugeSlider.value, // 連続的に変化させる対象の値
            x => _fanGaugeSlider.value = x, // 変化させた値 x をどう処理するかを書く
            gaugeValue, // x をどの値まで変化させるか指示する
            _changeFanGaugeInterval);
    }

    /// <summary>
    /// フィーバーゲージをDotweenで動的に表示する
    /// </summary>
    public void FevarGaugeInterpolation(float gaugeValue)
    {
        if (GameManager.Instance.State != GameState.Fevar)
        {
            _fevarGaugeSlider.Value = (int)gaugeValue; //ToDo intとFloatごっちゃなの修正
            IndicateFevar();
            
            /*DOTween.To(() => _fevarGaugeSlider.value, // 連続的に変化させる対象の値
                x => _fevarGaugeSlider.value = x, // 変化させた値 x をどう処理するかを書く
                gaugeValue, // x をどの値まで変化させるか指示する
                _changeFevarGaugeInterval).OnComplete(() => IndicateFevar());//*/

        }
    }

    /// <summary>
    /// 煙草の演出を表示する
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
    /// フィーバーの演出を表示する
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
