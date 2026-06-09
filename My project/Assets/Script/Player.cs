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

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Weapon") && Keyboard.current.fKey.wasPressedThisFrame)
        {
            ItemSlot.Add(gameObject);
            gameObject.SetActive(false);
        }
    }
}
