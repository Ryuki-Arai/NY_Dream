using System;

public class GameManager
{
    static GameManager _instance = new GameManager();

    static SoundManager _instanceSM = new SoundManager();

    static UIManager _instanceUI;

    static SceneManager _instanceScene = null;

    GameState _gameState = GameState.PlayGame;

    int _touchCigarettes;

    float _sumScore;

    float _fanValue;

    public GameState State => _gameState;
    public static GameManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new GameManager();
            }
            return _instance;
        }
    }

    public static SoundManager InstanceSM => _instanceSM;

    public void SetUI(UIManager ui) => _instanceUI = ui;
    public static SceneManager InstanceScene { get => _instanceScene; set => _instanceScene = value; }

    public int TouchCigarettes => _touchCigarettes;

    public float SumScore => _sumScore;

    public float FanValue => _fanValue;

    /// <summary>
    /// 煙草の接触を検知
    /// intで加算されたら検知判定
    /// </summary>
    public void AddCigarettes(int touchCigarettes)
    {
        _touchCigarettes += touchCigarettes;
        _instanceUI.IndicateSmoke();
    }

    /// <summary>引数をスコアに加算する</summary>
    public void AddScore(float scoreValue)
    {
        _sumScore += scoreValue;
        _instanceUI.ScoreInterpolation(_sumScore);
    }

    /// <summary>引数を扇のカウントに加算</summary>
    public void AddFanValue(float fanValue)
    {
        _fanValue += fanValue;
        _instanceUI.FanGaugeInterpolation(_fanValue);
    }

    /// <summary>引数をフィーバー</summary>
    public void AddFevarValue(int fevarValue)
    {
        _instanceUI.FevarGaugeInterpolation(fevarValue);
    }

    public void ChangeState(GameState gameState)
    {
        _gameState = gameState;
        switch (gameState)
        {
            case GameState.WaitGame:
                break;

            case GameState.PlayGame:
                _instanceSM.CallSound(SoundType.BGM,0);
                break;

            case GameState.Fevar:
                _instanceSM.CallSound(SoundType.BGM,3);
                break;

            case GameState.Finish:
                break;
        }
    }
}

public enum GameState 
{
    WaitGame,
    PlayGame,
    Fevar,
    Finish
}
