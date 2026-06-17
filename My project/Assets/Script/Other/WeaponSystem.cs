using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// 武器の管理を行う
/// 武器の種類ごとに要素は異なる
/// 近距離ならパリィが出来る
/// 遠距離は弾数制限がある
/// 
/// 
/// </summary>
public class WeaponSystem : MonoBehaviour
{
    public int fullcount = 0;
    public int currentcount;
    public int basicattack = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentcount = fullcount;

    }

    // Update is called once per frame
    void Update()
    {

    }
}

