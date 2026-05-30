using System.Collections;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public GameObject Failed;
    public GameObject Win;
    public TMP_Text timertext;
    public GameObject time;

    public float currenttimer = 30f;
    public bool start;

    void Start()
    {
        Win.SetActive(false);
        StartCoroutine(Ontimer());
    }

    private IEnumerator Ontimer()
    {
        while (currenttimer >= 0f)
        {
            currenttimer -= Time.deltaTime;
            timertext.text = currenttimer.ToString("F0");
            yield return null; // 1フレーム待つ
        }

        // タイマー終了
        time.SetActive(false);
        Win.SetActive(true);
    }
}
