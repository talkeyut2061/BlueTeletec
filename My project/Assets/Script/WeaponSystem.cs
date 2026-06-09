using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// weaponの管理を行う
/// weaponの種類ごとに弾薬(count)数は異なる
/// </summary>
public class WeaponSystem : MonoBehaviour
{
    public int fullcount = 0;
    public int currentcount;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentcount = fullcount;

    }

    // Update is called once per frame
    void Update()
    {
        if (currentcount >= 0) ;

    }
}

