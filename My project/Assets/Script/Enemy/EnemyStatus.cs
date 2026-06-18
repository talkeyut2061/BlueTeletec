using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;


/// <summary>
/// enemyの管理を行う
/// enemyがweaponオブジェクトに触れたら、10ダメージを入れて、
/// hitpointが0になったら、animation コンポーネントを無効にする(ラグドール化)
/// </summary>
public class EnemyStatus : MonoBehaviour
{
    [Header("Main Status")]
    [SerializeField] private int _maxhitpoint = 100;
    [SerializeField] private int _hitpoint;
    [SerializeField] public int _enemyattack = 5;
    [SerializeField] private int _defense = 5;
    [SerializeField] private float _movespeed = 3f;

    [Header("Transform")]
    [SerializeField] private float _distance;
    [SerializeField] private Transform _enemydistance;
    [SerializeField] private Transform _playerdistance;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float _noticedistance;


    [Header("Bool")]

    bool _isAttack = true;

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
        _agent = GetComponent<NavMeshAgent>();

        // 武器の攻撃力を加算
        _enemyattack += _weaponObject.BasicAttack / 5;
        _isAttack = false;
        _hitpoint = _maxhitpoint;

        _animator = GetComponent<Animator>();
        _animation = GetComponent<Animation>();
        _characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        _distance = Vector3.Distance(_enemydistance.position, _playerdistance.position);

        // 30m以上 → 攻撃解除
        // 20〜30m → 気付いて走る
        if (_distance > 20f)
        {
            Debug.Log("enemyに気付かれた！ 逃げるか戦え！");
            _agent.SetDestination(_playerdistance.position);
            _isAttack = true;
            _animator.Play("Run", 3);

            // プレイヤーの方向を向く
            Vector3 dir = (_playerdistance.position - transform.position).normalized;
            transform.forward = dir;

            // ★ 実際に移動する
            _characterController.Move(dir * _movespeed * Time.deltaTime);

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

            yield return new WaitForSeconds(5f);

            _animator.enabled = true;
        }
    }
}
