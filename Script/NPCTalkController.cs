using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPCTalkController : MonoBehaviour
{
    public Transform computer; // NPC
    public Transform player;   // Player
    public float talkRange = 1.0f;
    public float cooldownTime = 3f;

    private bool canTalk = true;
    private bool isTalking = false;

    void Update()
    {
        // NPCとプレイヤーの距離を毎フレーム計算
        float distance = Vector3.Distance(player.position, computer.position);

        // 会話中 or クールタイム中は何もしない
        if (!canTalk) return;

        // 一定距離以内なら会話可能
        if (distance < talkRange)
        {

            Debug.Log("NPCとの会話が出来ます");
            // Fキーで会話開始
            if (Keyboard.current.fKey.wasPressedThisFrame)
            {
                StartConversation();
            }
        }
    }

    void StartConversation()
    {
        isTalking = true;
        Debug.Log("会話開始");

        // 会話終了後にクールタイムへ
        StartCoroutine(TalkCooldown());
    }

    IEnumerator TalkCooldown()
    {
        isTalking = false;
        canTalk = false;

        Debug.Log("会話終了 → クールタイム開始");

        yield return new WaitForSeconds(cooldownTime);

        canTalk = true;
        Debug.Log("クールタイム終了 → 再び会話可能");
    }
}
