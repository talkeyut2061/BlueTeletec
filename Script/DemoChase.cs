using UnityEngine;

public class DemoChase : MonoBehaviour
{

    public float trunspeed = 4f; // 回転速度  
    public Transform target;     //照準する対象

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = target.position - transform.position; // ターゲットへの方向ベクトルを計算
        Quaternion targetRotation = Quaternion.LookRotation(direction); // ターゲットの方向を向く回転を計算
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, trunspeed * Time.deltaTime); // 回転を適用
    }
}
