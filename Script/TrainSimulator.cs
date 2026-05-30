using UnityEngine;
using UnityEngine.InputSystem;

public class TrainSimulator : MonoBehaviour
{

    /// <summary>
    /// engineがtrueかつw,sキーで前進後退
    /// 押していない間、自然に減速する
    /// </summary>
    [Header("Control")]
    public float speed = 0f;
    public float maxspeed = 80f;
    public float acceleration = 2f;
    public float Friction = 0.15f;
    public bool engine = false;
    public bool brake = false;

    [Header("Other")]
    public AudioClip EmergencySound;
    public AudioSource MainSound;

    private void Start()
    {

    }

    void Update()
    {
        HandleEngineToggle();
        HandleAcceleration();
        HandleBrake();
    }


    /// <summary>
    /// leftshiftキーが押されたら、engineトリガーをtrueやfalseにする
    /// </summary>
    void HandleEngineToggle()
    {
        if (Keyboard.current.leftShiftKey.wasPressedThisFrame)
        {
            engine = !engine;
        }
    }

    void HandleAcceleration()
    {
        if (!engine) return;

        // 加速
        if (Input.GetKey(KeyCode.W))
        {
            speed += acceleration * Time.deltaTime;
        }
        // 減速（逆進）
        else if (Input.GetKey(KeyCode.S))
        {
            speed -= acceleration * Time.deltaTime;
        }
        // 自然減速
        else
        {
            speed = Mathf.MoveTowards(speed, 0, Friction * Time.deltaTime);
        }

        // 速度制限
        speed = Mathf.Clamp(speed, -maxspeed, maxspeed);

        // 実際の移動処理（ここが正しい）
        transform.position += transform.forward * speed * Time.deltaTime;
    }


    void HandleBrake()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            brake = !brake;

            if (Input.GetKey(KeyCode.LeftControl))
            {
                speed = Mathf.MoveTowards(speed, 0, acceleration * 5f * Time.deltaTime);

                if (!MainSound.isPlaying)
                {
                    MainSound.PlayOneShot(EmergencySound);
                }
                return;
            }

            // 通常ブレーキ
            if (Input.GetKey(KeyCode.Space))
            {
                speed = Mathf.MoveTowards(speed, 0, Friction * 5f * Time.deltaTime);
            }
        }
    }
}
