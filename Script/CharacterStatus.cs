using UnityEngine;

public class CharacterStatus : MonoBehaviour
{
    public int maxHealth; // 最大体力
    public int currentHealth; // 現在の体力
    public int attack = 10; // 攻撃力
    public int defense = 5; // 防御力

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth; // 初期体力を最大体力に設定
    }

    // Update is called once per frame


   
}
