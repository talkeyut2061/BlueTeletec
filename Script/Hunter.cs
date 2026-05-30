using UnityEngine;
using UnityEngine.AI;



/// <summary>
/// Hunter(CPU)の制御を行う
/// playerとの距離が一定以下になった時、
/// 戦闘モードに入り、一定確率で攻撃を行う
/// </summary>
public class Hunter : MonoBehaviour
{

    public float Player_distance;
    public bool IsAttack;
    public Collider SearchArea;
    public bool Attack = false;
    private NavMeshAgent _navi;
    private Transform here;
    private Transform player;
    private float Distance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _navi = GetComponent<NavMeshAgent>();
        

    }

    // Update is called once per frame
    void Update()
    {
        Distance = Vector3.Distance(here.position, player.position);
    }



    private class IsAttacking
    { 
    public float Distance { get; }
    }

    //void IsAttack()
    //{
    //    if(Distance < 5f && Attack)
    //    {
    //        Animation.Setbool("Attack");
    //    }
    //}
}


