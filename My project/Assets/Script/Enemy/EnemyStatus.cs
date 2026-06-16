using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyStatus : MonoBehaviour
{
    [Header("main status")]
    [SerializeField] private int _maxhitpoint = 100;
    private int _hitpoint;
    [SerializeField] public int _enemyattack = 5;
    [SerializeField] private int _defense = 5;
    [SerializeField] private float _movespeed = 3f;

    [Header("Other")]
    [SerializeField] private float _distance;
    [SerializeField] private Transform _enemy;
    [SerializeField] private Transform player;

    bool _isAttack = true;
    bool _isBattleRun = true;

    Animator _animator;
    Animation _animation;
    CharacterController _characterController;
    WeaponSystem _weaponSystem;
    WeaponObject _weaponObject;

    void Start()
    {
        // 先に GetComponent しないと null になる
        _weaponSystem = GetComponent<WeaponSystem>();
        _weaponObject = GetComponent<WeaponObject>();

        // 武器の攻撃力を加算
        _enemyattack += _weaponObject.BasicAttack / 5;

        _isAttack = false;
        _isBattleRun = false;
        _hitpoint = _maxhitpoint;

        _animator = GetComponent<Animator>();
        _animation = GetComponent<Animation>();
        _characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        _distance = Vector3.Distance(_enemy.position, player.position);

        // 30m以上 → 攻撃解除
        if (_distance > 30f)
        {
            _isAttack = false;
            _isBattleRun = false;
            return;
        }

        // 20〜30m → 気付いて走る
        if (_distance > 20f)
        {
            Debug.Log("enemyに気付かれた！ 逃げるか戦え！");
            _isAttack = true;
            _isBattleRun = true;
            _animator.Play("Run", 3);

            // ★ enemy が player の方向を向く（正しい書き方）
            transform.forward = (player.position - transform.position).normalized * _movespeed;

            return;
        }

        // 20m以内 → 攻撃モード
        if (_distance <= 20f)
        {
            Debug.Log("enemyが攻撃する！ 上手く避けろ！");
            _animator.SetTrigger("Walk");
            StartCoroutine(IsBattle());
        }
    }

    IEnumerator IsBattle()
    {
        while (_isAttack)
        {
            yield return new WaitForSeconds(3f);

            int random = Random.Range(0, 100);

            if (random >= 50)
            {
                Debug.Log($"生成した乱数は {random}");
                Debug.Log("enemyが攻撃した");
                _animator.SetTrigger("Attack");
            }
            else
            {
                Debug.Log($"生成した乱数は {random}");
                Debug.Log("攻撃しなかった");
            }
        }

        _isBattleRun = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Weapon"))
        {
            //_hitpoint -= _player._attack - _defense;

            if (_hitpoint <= 0)
                StartCoroutine(Die());
            else
                _animator.SetTrigger("Damage");
        }

        IEnumerator Die()
        {
            _characterController.enabled = false;
            _animator.enabled = false;

            yield return new WaitForSeconds(5f);

            _isAttack = false;
            _isBattleRun = false;

            yield return new WaitForSeconds(5f);

            _animator.enabled = true;
        }
    }
}
