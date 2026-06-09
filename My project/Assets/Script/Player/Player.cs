using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// playerの管理を行う
/// </summary>
public class Player : MonoBehaviour
{

    public List<GameObject> ItemSlot = new List<GameObject>();
    [SerializeField] private Player player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }



    private void Attack()
    {
        if (ItemSlot != null && Input.GetMouseButton(0))
        {
            
        }
    }


    /// <summary>
    /// weaponオブジェクトとplayerが接した際に、
    /// weaponオブジェクトはitemslot listにaddする
    /// </summary>
    /// <param name="collision"></param>
    private void OnControllerColliderHit(ControllerColliderHit collision)
    {
        if(collision.gameObject.CompareTag("Weapon"))
        {
            Debug.Log("weaponを拾った");
            ItemSlot.Add(collision.gameObject);
            collision.gameObject.SetActive(false);
        }
    }
}
