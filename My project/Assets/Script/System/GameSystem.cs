using StarterAssets;
using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

/// <summary>
/// ゲームのシステムを管理する
/// playerがenemyをkillするたびにkillcountを+1する
/// enemyがkill(SetActive(false))されたら、3秒後にランダムなspawnpointにspawn(SetActive(true))する
/// また、キルログを一定時間、表示する
/// playerが倒された時、ペナルティとしてkillcountを-1する
/// _gametimer内にkillcountが一定数以上達した場合、clearとする
/// </summary>
public class GameSystem : MonoBehaviour
{
    [Header("Count Component")]
    [SerializeField] private int _killCount = 0;      // 現在のキル数
    [SerializeField] private int _startTimer = 3;     // ゲーム開始前のカウントダウン
    [SerializeField] private int _gameTimer = 180;     // ゲーム時間

    [Header("Text Component")]
    [SerializeField] private TMP_Text _title;
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private TMP_Text _killCountText;
    [SerializeField] private TMP_Text _killLog;
    [SerializeField] private TMP_Text _stageClear;

    [Header("Button and Canvas Component")]
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _creditsButton;
    [SerializeField] private RawImage _creditsImage;

    [Header("SpawnPoint")]
    [SerializeField] private GameObject[] _enemyspawnPoints;
    [SerializeField] private GameObject[] _playerspawnPoints;

    [Header("Other Component")]
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _enemy;


    [Header("Audio Component")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _titlemusicclip;
    [SerializeField] private AudioClip _battlemusicclip;
    [SerializeField] private AudioClip _clicksoundclip;
    

    void Start()
    {
        // UI 初期化

        _timeText.enabled = false;
        _killCountText.enabled = false;
        _killLog.enabled = false;
        _stageClear.enabled = false;
        _title.enabled = true;
        _creditsImage.gameObject.SetActive(false);

        // ボタンにイベント登録
        _startButton.onClick.AddListener(GameStart);
        _enemy.SetActive(false);
        _player.SetActive(false);

        _audioSource.PlayOneShot(_titlemusicclip);
        _audioSource.loop = true;
    }

    /// <summary>
    /// buttonが押されたら、このメソッドを呼び出し、gameをstartする
    /// gametimerで時間を変更出来る
    /// </summary>
    private void GameStart()
    {
        _audioSource.PlayOneShot(_clicksoundclip);
        StartCoroutine(GameFlow());
    }


    /// <summary>
    /// クレジット出す用
    /// escキーで戻る
    /// </summary>
    private void Credit()
    {
        _audioSource.PlayOneShot(_clicksoundclip);
        _creditsImage.gameObject.SetActive(true);
        _startButton.gameObject.SetActive(false);
        _creditsButton.gameObject.SetActive(false);
        _title.enabled = false;
    }

  
        /// <summary>
        /// ゲーム全体の流れを管理するコルーチン
        /// </summary>
        IEnumerator GameFlow()
    {
        // カウントダウン表示 
        _audioSource.Stop();
        _startButton.gameObject.SetActive(false);
        _title.enabled = false;
        _timeText.enabled = true;
        for (int i = _startTimer; i > 0; i--)
        {
            _timeText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        // ゲーム開始
        _killCount = 0;
        _killCountText.enabled = true;
        _killCountText.text = "Kill : 0";

        // ゲームタイマー
        int timer = _gameTimer;
        

        while (timer > 0)
        {
            Cursor.visible = false;
            _audioSource.PlayOneShot(_battlemusicclip);
            _player.SetActive(true);
            _enemy.SetActive(true);
            _timeText.text = timer.ToString();
            timer--;
            yield return new WaitForSeconds(1f);
        }

        // タイムアップ → クリア判定
        CheckClear();
    }

    /// <summary>
    /// KillCount を +1 し、ログを表示する
    /// EnemyStatus から呼び出す想定
    /// </summary>
    private void AddKill()
    {
        _killCount++;
        _killCountText.text = $"Kill : {_killCount}";
        StartCoroutine(ShowKillLog());
    }

    /// <summary>
    /// KillLog を一定時間表示
    /// </summary>
    IEnumerator ShowKillLog()
    {
        _killLog.enabled = true;
        _killLog.text = "Enemy Killed!";
        yield return new WaitForSeconds(1.5f);
        _killLog.enabled = false;
    }

    /// <summary>
    /// クリア判定
    /// </summary>
    private void CheckClear()
    {
        if (_killCount >= 10) // 例：10体倒せばクリア
        {
            _stageClear.enabled = true;
            _stageClear.text = "GAME CLEAR!";
        }
        else
        {
            _stageClear.enabled = true;
            _stageClear.text = "FAILED...";
        }
    }

    /// <summary>
    /// 敵をランダムスポーン（EnemyStatus から呼び出す）
    /// </summary>
    public Transform GetRandomEnemySpawn()
    {
        int index = UnityEngine.Random.Range(0, _enemyspawnPoints.Length);
        return _enemyspawnPoints[index].transform;
    }

    /// <summary>
    /// プレイヤーをランダムスポーン (playerから呼び出す)
    /// </summary>
    /// <returns></returns>
    public Transform GetRandomPlayerSpawn()
    {
        int index = UnityEngine.Random.Range(0, _playerspawnPoints.Length);
        return _playerspawnPoints[index].transform;
    }
}
