using UnityEngine;
using UnityEngine.UI;

public class GameManeger : MonoBehaviour
{
    public GameObject Manual;
    public GameObject Player;
    public GameObject Enemy;
    public Transform[] Spawnpoint;
    public Transform[] EnemySpawnpoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Manual.SetActive(false);
        int point = Random.Range(0, Spawnpoint.Length);
        Transform spawn = Spawnpoint[point];
        Transform enemyspawn = EnemySpawnpoint[point];
        Instantiate (Player, spawn.position, spawn.rotation);
        Instantiate(Enemy,enemyspawn.position, enemyspawn.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnManual()
    {
        Manual.SetActive(true);
        
    }
}
