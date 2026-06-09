using TMPro;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// gameのsystemを管理する
/// </summary>
public class GameSystem : MonoBehaviour
{


    [SerializeField] public int timer = 0;


    [Header("Other")]
    public TMP_Text Timetext;
    public GameObject canvas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
