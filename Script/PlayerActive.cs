using StarterAssets;
using System.Collections;
using System.Threading;
using UnityEngine;

public class PlayerActive : MonoBehaviour
{
    public bool IsOkay;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        IsOkay = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ButtonCoroutine()
    {
        StartCoroutine(OnMoveActive());
    }
    IEnumerator OnMoveActive()
    {

        yield return new WaitForSeconds(3f);
       
    }
   
}
