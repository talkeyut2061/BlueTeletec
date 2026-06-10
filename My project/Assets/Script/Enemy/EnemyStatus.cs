using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// enemyの管理を行う
/// hitpointが0になったら(0以下)、
/// enemyのanimation compornentをnullにする
/// また、playerとの距離が一定数まで下がった場合、戦闘態勢を取る(battle animationを再生)
/// </summary>
public class EnemyStatus : MonoBehaviour
{

    [Header("main system")]
    private int hitpoint;
    [SerializeField] public int maxhitpoint = 0;
    [SerializeField] public int attack = 0;
    [SerializeField] public int defence = 0;

    [Header("sub system")]
    [SerializeField] public bool _isattack = false;
    bool _ispatroll = false;

    [Header("distance")]
    Transform distance;
 



    [SerializeField] private EnemyStatus enemystatus;
    //other
    Animator _animator;
    Animation _animation;
    private int value = UnityEngine.Random.Range(1, 100);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animator = GetComponent<Animator>();
        _animation = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {

        _isattack = !_isattack;
        StartCoroutine(IsBattle());
    }


    /// <summary>
    /// 戦闘中、2f(2秒)毎に攻撃する
    /// </summary>
    /// <returns></returns>
    IEnumerator IsBattle()
    {
        while(_isattack == true)
        {
            yield return new WaitForSeconds(2f);
            
        }
    }
    
}

