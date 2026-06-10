using TMPro;
using UnityEngine;



/// <summary>
/// Objectの干渉の管理を行う
/// trueになっているcompornent flagに応じて、以下の操作を行う
/// breaking flag・・・playerかenemyの干渉をある程度受けたらdestroy(exp 攻撃)
/// transforming flag・・・キーの0bjectが干渉したら、transformを行う
/// 両方・・・breakingでの干渉→destroy transformingでの干渉→ collision.enabled
/// 
/// </summary>
public class ObjectInterference : MonoBehaviour
{
    [Header("breaking flag")]
    [SerializeField] public int unbreaking = 10;

    [Header("Moving flag")]
    [SerializeField] public float move = 2f;
    [SerializeField] Transform targetTransform;

    [Header("Component flag")]
    public bool breaking = false;
    public bool transforming = false;

    Collider _collision;
    Player _player;
    EnemyStatus _enemyStatus;


    void Start()
    {
        _collision = GetComponent<Collider>();
        _player = GetComponent<Player>();
        _enemyStatus = GetComponent<EnemyStatus>();
    }

    void Update()
    {
        if (breaking && unbreaking <= 0)
        {
            Destroy(gameObject);
        }

        if (transforming)
        {
            targetTransform.position += Vector3.up * move * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (breaking && collision.gameObject.CompareTag("Player"))
            unbreaking--;

        else if (breaking && collision.gameObject.CompareTag("Enemy"))
            unbreaking--;



        if (transforming)
        {
            // 例：キーとなる object の tag が "Key"
            if (collision.gameObject.CompareTag("Key"))
            {
                transforming = true;
            }
        }
    }
}
