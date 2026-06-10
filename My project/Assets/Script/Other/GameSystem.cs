using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// gameのsystemを管理する
/// playerがenemyをkillするたびにkillcountを+1する
/// enemyがkill(SetActive(false))されたら、3秒後にランダムなspawnpointに
/// spawn(SetActive(true))する
/// また、キルログを一定時間、表示する
/// playerが倒された時、ペナルティとしてkillcountを-1する
/// </summary>
public class GameSystem : MonoBehaviour
{

    [SerializeField] public int timer = 0;

    [Header("Main count")]
    private int killcount = 0;

    [Header("Main text")]
    public TMP_Text Timetext;
    public TMP_Text Killcounttext;
    public TMP_Text KillLog;

    [Header("Sub")]
    public GameObject canvas;

    [Header("referad script")]
    EnemyStatus _enemyStatus;

    [Header("Enemy Spawn Point")]
    public GameObject[] spawnpoint;
    public GameObject[] Player;
    public GameObject[] Enemy;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    IEnumerator Time()
    {
        yield return new WaitForSeconds(3f);
    }

    private void Kill()
    {
   
    }
}
