using StarterAssets;
using System;
using System.Collections;
using System.Runtime.Serialization;
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
    [SerializeField] private int _normacount;

    [Header("Text Component")]
    [SerializeField] private GameObject _title;
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private TMP_Text _normaText;
    [SerializeField] private TMP_Text _killCountText;
    [SerializeField] private TMP_Text _killLog;
    [SerializeField] private TMP_Text _complete;  
    [SerializeField] private TMP_Text _failed;    

    [Header("Canvas component")]
    [SerializeField] private RawImage _creditsImage;

    [Header("SpawnPoint")]
    [SerializeField] private GameObject[] _enemyspawnPoints;
    [SerializeField] private GameObject[] _playerspawnPoints;

    [Header("Other Component")]
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _enemy;

    [Header("Background music Audio Component")]
    [SerializeField] private AudioSource _audioSource;     //共通のaudiosource
    [SerializeField] private AudioClip _titlemusicclip;    //タイトル画面で流れるsound
    [SerializeField] private AudioClip _battlemusicclip;   //ゲーム中(戦闘中)に流れるbgm
    [SerializeField] private AudioClip _losemusicclip;     //ノルマ未達成に流れる
    [SerializeField] private AudioClip _gamewinsmusicclip; //ノルマ達成時に流れる

    [Header("Effect sound Audio Component")]
    [SerializeField] private AudioClip _clicksoundclip;    //クリック音
    [SerializeField] private AudioClip _damagevoiceclip;   //playerやnpcのダメージボイス
    [SerializeField] private AudioClip _deathvoiceclip;    //playerやnpc(enemy)の死亡ボイス

    void Awake()
    {
        _complete.gameObject.SetActive(false);
        _failed.gameObject.SetActive(false);
        _normaText.gameObject.SetActive(false);
    }

    void Start()
    {
        // UI 初期化

        _timeText.enabled = false;
        _killCountText.enabled = false;
        _killLog.enabled = false;
        _title.SetActive(true);
        _creditsImage.gameObject.SetActive(false);
        _enemy.SetActive(false);
        _player.SetActive(false);
       
        // ★FIX: タイトルBGMを正しくループ再生する
        _audioSource.clip = _titlemusicclip;
        _audioSource.loop = true;
        _audioSource.Play();
        // _audioSource.PlayOneShot(_titlemusicclip);
        // _audioSource.loop = true;

       
    }

    void Update()
    {
        // ★FIX: Credits 表示中だけ ESC キーでタイトルに戻る
        if (_creditsImage.gameObject.activeSelf)
        {
            if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                BackTitle();
            }
        }
    }

    /// <summary>
    /// buttonが押されたら、このメソッドを呼び出し、gameをstartする
    /// gametimerで時間を変更出来る
    /// </summary>
    public void GameStart()
    {
        _normacount = (_gameTimer / 6) - 5;

        // ★ここで normaText を更新する
        _normaText.text = $"{_gameTimer}秒以内に敵を{_normacount}体、倒せ！";

        _audioSource.PlayOneShot(_clicksoundclip);
        StartCoroutine(GameFlow());
    }

    /// <summary>
    /// クレジット出す用
    /// escキーで戻る
    /// </summary>
    public void Credit()
    {
        _audioSource.PlayOneShot(_clicksoundclip);
        _creditsImage.gameObject.SetActive(true);
        _title.SetActive(false);
    }

    public void BackTitle()
    {
        // ★FIX: Start() を呼び直さず、必要なオブジェクトだけ状態を戻す
        _creditsImage.gameObject.SetActive(false);
        _title.SetActive(true);
    }

    /// <summary>
    /// ゲーム全体の流れを管理するコルーチン
    /// </summary>
    IEnumerator GameFlow()
    {
        // カウントダウン表示 
        _audioSource.Stop();
        _title.SetActive(false);
        _normaText.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);
        _normaText.gameObject.SetActive(false);
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

        // ★FIX: バトルBGMを1回だけ再生してループ
        _audioSource.clip = _battlemusicclip;
        _audioSource.loop = true;
        _audioSource.Play();
        // ゲームタイマー
        int timer = _gameTimer;

        while (timer >= 0)
        {
            Cursor.visible = false;
            // _audioSource.PlayOneShot(_battlemusicclip); // ← 毎秒鳴っていたバグ

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
    public void AddKill() // ★FIX: 外部から呼ぶので public に
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
        if (_killCount >= _normacount) // 例：_normacount体倒せばクリア
        {
            _player.SetActive(false);
            _complete.gameObject.SetActive(true);
            _timeText.enabled = false;
            _complete.text = "GAME CLEAR!";
        }
        else
        {
            _player.SetActive(false);
            _failed.gameObject.SetActive(true);
            _timeText.enabled = false;
            _failed.text = "FAILED...";
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
