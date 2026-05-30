using NUnit.Framework.Internal.Execution;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// カメラ関連のscript
/// ゲームシーン実行時、TitleCameraをmaincameraとし、
/// buttonが押されたらcoroutineによる処理を経て、PlayerCameraを
/// main cameraとしてゲームを始める
/// </summary>
public class CameraManager : MonoBehaviour
{
    public Camera TitleCamera;
    public CinemachineCamera PlayingCamera;
    public Image TitleBackground;
    public GameManeger manager;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TitleCamera = GetComponent<Camera>();
        manager = GetComponent<GameManeger>();
        TitleCamera.enabled = true;
        PlayingCamera.enabled = false;
        TitleBackground.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Game()
    {
        StartCoroutine(Ongame());
    }
    IEnumerator Ongame()
    {
        TitleCamera.enabled = false;

        yield return new WaitForSeconds(300);
        PlayingCamera.enabled = true;
    }
    
}
