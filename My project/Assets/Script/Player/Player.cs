using Cinemachine;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// playerの管理を行う
/// playerはweaponオブジェクトに触れたら、hitpointを減らして、damage animationを再生する
/// もし、hitpointが0になったら、animatorやplayerinput コンポーネントをfalseにする
/// </summary>
public class Player : MonoBehaviour
{
    [Header("Main Status")]
    [SerializeField] private int _hitpoint = 0;
    [SerializeField] private int _maxhitpoint = 10;
    [SerializeField] public int _playerattack = 10;
    [SerializeField] private int _defense = 10;

    [Header("Inventri")]
    [SerializeField] private List<GameObject> ItemSlot = new List<GameObject>();

    [Header("Other")]
    [SerializeField] private GameObject[] ragdoll;
    [SerializeField] private int ragdolltime = 5;
    [SerializeField] private PlayerInput _playerinput;
    [SerializeField] private ThirdPersonController _thirdPersonController;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform muzzle;
    [SerializeField] private GameObject bullet;
    [SerializeField] private bool _isDead = false;

    [Header("refer")]
    [SerializeField] private EnemyStatus _enemyStatus;

    void Start()
    {
        _thirdPersonController = GetComponent<ThirdPersonController>();
        _playerinput = GetComponent<PlayerInput>();
        _animator = GetComponent<Animator>();
        _hitpoint = _maxhitpoint;

        foreach (var item in ragdoll)
            item.SetActive(false);
    }

    void Update()
    {
        if (_isDead) return;

        // -------------------------------
        // 旧 Input System（エラー原因）
        // if (Input.GetMouseButtonDown(0))
        //     Attack();
        // -------------------------------

        // 新 Input System（正しい書き方）
        if (Mouse.current.leftButton.wasPressedThisFrame)
            Attack();

        // こちらは新 Input System なので OK
        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            Debug.Log("playerは10ダメージを受けた");
            _animator.SetTrigger("Damage");
            _hitpoint -= 10;
        }

        Death();
    }

    /// <summary>
    /// 素手 or 武器攻撃
    /// </summary>
    private void Attack()
    {
        if (ItemSlot.Count == 0)
        {
            _animator.SetTrigger("Attack");
            ragdoll[0].SetActive(true);
        }
        else
        {
            _animator.SetTrigger("Attack");
            Instantiate(bullet, muzzle.position, muzzle.rotation);
        }
    }

    /// <summary>
    /// weaponオブジェクトとplayerが接した際に拾う
    /// </summary>
    private void OnControllerColliderHit(ControllerColliderHit collision)
    {
        if (collision.gameObject.CompareTag("Weapon"))
        {
            Debug.Log("weaponを拾った");
            ItemSlot.Add(collision.gameObject);
            collision.gameObject.SetActive(false);
        }

        else if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.LogWarning("enemyに触れたので、体力が減った");
            _enemyStatus = collision.gameObject.GetComponent<EnemyStatus>();
            _hitpoint -= _enemyStatus._enemyattack;
        }
    }

    /// <summary>
    /// playerのhitpointが0を下回ったら死亡処理
    /// </summary>
    private void Death()
    {
        if (_hitpoint > 0) return;

        _isDead = true;

        _animator.enabled = false;
        _thirdPersonController.enabled = false;
        _playerinput.enabled = false;

        foreach (var item in ragdoll)
            item.SetActive(true);

        StartCoroutine(AfterDeath());
    }

    IEnumerator AfterDeath()
    {
        yield return new WaitForSeconds(ragdolltime);

        foreach (var item in ragdoll)
            item.SetActive(false);

        _hitpoint = _maxhitpoint;
        _animator.enabled = true;
        _thirdPersonController.enabled = true;
        _playerinput.enabled = true;

        _isDead = false;
    }
}
