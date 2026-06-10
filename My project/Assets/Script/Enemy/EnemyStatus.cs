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
    [SerializeField] public int maxhitpoint = 0;
    [SerializeField] private int hitpoint;
    [SerializeField] public int attack = 0;
    [SerializeField] public int defence = 0;

    [Header("sub system")]
    [SerializeField] private bool _isattack = false;
    [SerializeField] private bool _ispatroll = false;

    [Header("distance")]
    Transform distance;
 



    [SerializeField] private EnemyStatus enemystatus;
    //other
    Animator _animator;
    private int random = UnityEngine.Random.Range(1, 100);
    int rangeA = 50;
    Player _player;
    bool damage;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hitpoint = maxhitpoint;
        _animator = GetComponent<Animator>();
        _player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {

        _isattack = !_isattack;
        StartCoroutine(IsBattle());
    }


    /// <summary>
    /// 戦闘中、2f(2秒)毎に乱数の生成を行う。
    /// 生成される乱数は1から100とする
    /// そして、その乱数が値aと値bの範囲内なら攻撃を行う
    /// </summary>
    /// <returns></returns>
    IEnumerator IsBattle()
    {
        while(_isattack == true)
        {
            yield return new WaitForSeconds(2f);
            if (random >= rangeA)
            {
                
            }
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Weapon"))
        {
            hitpoint -= _player.attack;
            _animator.SetTrigger("damage");
        }


    }

}

