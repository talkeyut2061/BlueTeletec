using UnityEngine;

public class ShipMoving : MonoBehaviour
{
    [Header("Speed Settings")]
    public float speed = 0f;          // 現在速度（ノット）
    public float accelPower = 5f;     // 加速力（ノット/秒）
    public float brakePower = 6f;     // 逆噴射（ブレーキ）力
    public float naturalDecel = 1f;   // 自然減速
    public float maxKnot = 30f;       // 最大前進速度
    public float backMaxKnot = 4f;    // 最大後退速度
    public float rotationSpeed = 10f; // 回転速度
    private float knotToMps = 0.514444f;

    float inputForward = 0f;
    float inputTurn = 0f;

    void Update()
    {
        ReadInput();
        UpdateSpeed();
        MoveShip();
        RotateShip();
    }

    void ReadInput()
    {
        // W = 前進, S = 後退
        inputForward = Input.GetAxisRaw("Vertical");  // -1 ～ 1
        inputTurn = Input.GetAxis("Horizontal");
    }

    void UpdateSpeed()
    {
        // 前進
        if (inputForward > 0)
            speed += accelPower * Time.deltaTime;

        // 後退
        else if (inputForward < 0)
            speed -= brakePower * Time.deltaTime;

        // 自然減速（前進・後退どちらも0に近づく）
        else
        {
            if (speed > 0)
                speed -= naturalDecel * Time.deltaTime;
            else if (speed < 0)
                speed += naturalDecel * Time.deltaTime;
        }

        // 速度制限
        speed = Mathf.Clamp(speed, -backMaxKnot, maxKnot);
    }

    void MoveShip()
    {
        float speedMps = speed * knotToMps * 0.9f;
        transform.position += transform.forward * speedMps * Time.deltaTime;
    }

    void RotateShip()
    {
        // 速度が低いと曲がりにくくする（船らしい挙動）
        float turnFactor = Mathf.Clamp01(Mathf.Abs(speed) / maxKnot);
        transform.Rotate(Vector3.up, inputTurn * rotationSpeed * turnFactor * Time.deltaTime);
    }
}
