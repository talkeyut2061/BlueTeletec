using Cinemachine;
using NUnit.Framework;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// playerの管理を行う
/// </summary>
public class Player : MonoBehaviour
{
    [Header("Main Status")]
    int hitpoint;
    public int maxhitpoint = 0;
    public int attack = 0;
    public int defeace = 0;

    [Header("Inventri")]
    public List<GameObject> ItemSlot = new List<GameObject>();

    [Header("Other")]
    [SerializeField] private Player player;
    [SerializeField] GameObject[] ragdoll;

    PlayerInput _playerinput;
    ThirdPersonController _thirdPersonController;
    CharacterController _characterController;
    ICinemachineCamera _Cinemachine;
    WeaponSystem _weaponSystem;
    Animator _animator;
    GameObject muzzle;

    [Header("refer")]
    EnemyStatus _enemyStatus;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    /// <summary>
    /// 最初のみragdoll オブジェクトをfalseにする
    /// </summary>
    void Start()
    {
        _thirdPersonController = GetComponent<ThirdPersonController>();
        _playerinput = GetComponent<PlayerInput>();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

        hitpoint = maxhitpoint;
        foreach (var item in ragdoll)
        {
            item.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(Attack());
        Death();
    }


    /// <summary>
    /// ItemSlot listがnullではないかつ左クリックした際、遠距離武器なら発射、
    /// 近接武器は振る
    /// もし。nullであった場合、
    /// handattack animationを再生かつ一部のcolliderオブジェクトをtrueにする
    /// また遠距離武器は弾数制限があり、0の時に、Rキーでaddしない限り、遠距離は攻撃不可かGキーで
    /// weaponオブジェクトをremoveするしかない
    /// 
    /// </summary>
    IEnumerator Attack()
    {
        yield return new WaitForSeconds(2f);

        if (ItemSlot == null && Input.GetMouseButton(0))
        {
            _animator.SetTrigger("Attack");
            ragdoll[0].SetActive(true);
        }

        yield return null;
    }


    /// <summary>
    /// weaponオブジェクトとplayerが接した際に、
    /// weaponオブジェクトはitemslot listにaddする
    /// また、enemyオブジェクトに触れた時は、enemystatusのattack変数を参照して、
    /// hitpointを減少する
    /// </summary>
    /// <param name="collision"></param>
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
            Debug.LogWarning("enemyに触れた");
            hitpoint -= _enemyStatus.attack;

            //if (hitpoint < 0)
            //    Death();
        }
    }


    /// <summary>
    /// playerのhitpointが0を下回ったら、GetCompornentしたCompornentをnullにして、
    /// ragdoll listにあるcollider objectをforeachでtrueにする
    /// </summary>
    private void Death()
    {
        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            _animator.enabled = false;
            _characterController.enabled = false;
            _thirdPersonController.enabled = false;
            _playerinput.enabled = false;

            foreach (var item in ragdoll)
            {
                item.SetActive(true);
            }

            StartCoroutine(AfterDeath());

            
        }
    }

    IEnumerator AfterDeath()
    {
        yield return new WaitForSeconds(3f);

        foreach (var item in ragdoll)
        {
            item.SetActive(false);
        }
        _animator.enabled = true;
        _characterController.enabled =true;
        _thirdPersonController.enabled = true;
        _playerinput.enabled = true;

       
    }
}
