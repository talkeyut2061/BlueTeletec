using JetBrains.Annotations;
using KevinIglesias;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyStatus : MonoBehaviour
{

    [Header("main status")]
    [SerializeField] int maxhitpoint = 100;
    int hitpoint;
    [SerializeField] public int attack = 0;
    [SerializeField] int defense = 0;


    [Header("Other")]
    float distance;
    Transform enemy;
    Transform player;
    HumanSoldierController controller;

    bool _isattack = false;
    bool _isBattleRun = false;

    Animator _animator;
    Player _player;

    void Start()
    {
        distance = Vector3.Distance(enemy.position, player.position);
        hitpoint = maxhitpoint;
        controller = GetComponent<HumanSoldierController>();
        _animator = GetComponent<Animator>();
        _player = GetComponent<Player>();
    }


    /// <summary>
    /// もし、enemyとplayerとの距離が5f(5m)以下の場合
    /// _isattack flagをtrueかつ_isbattleRun flagがfalseの時のみ
    /// enemyの視点をplayerに向けて、コルーチンを呼び出す。
    /// </summary>
    void Update()
    {
        if (distance <= 5f)
        {
            _isattack = !_isattack;

            if (_isattack && !_isBattleRun)
                StartCoroutine(IsBattle());
        }
    }

    IEnumerator IsBattle()
    {
        _isBattleRun = true;

        while (_isattack)
        {
            yield return new WaitForSeconds(2f);

            int random = Random.Range(0, 100);

            if (random >= 50)
            {
                Debug.Log($"生成した乱数: {random}");
                _animator.SetTrigger("Attack");
            }
            else
            {
                Debug.Log($"生成した乱数: {random}");
            }
        }

        _isBattleRun = false;
    }

    public void TakeDamage(int amount)
    {
        //hitpoint -= amount;

        //if (hitpoint <= 0)
        if (Keyboard.current.leftCtrlKey.wasPressedThisFrame)
            hitpoint -= _player.attack;


        else
            _animator.SetTrigger("Damage");

        if (hitpoint >= 0)
            StartCoroutine(Die());
    }

   IEnumerator Die()
    {
        _animator.enabled = false;
        yield return new WaitForSeconds(5f);

        _animator.enabled = true;

    }
}
