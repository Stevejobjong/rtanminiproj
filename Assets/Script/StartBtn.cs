using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartBtn : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip startBtn;

    // Start is called before the first frame update
    public void OnBtnStart()
    {
        audioSource.PlayOneShot(startBtn);
        //씬이 곧바로 로드되어 효과음이 재생되지 않는 문제 해결(1초 딜레이)
        Invoke("LoadGameScene", 1f);
    }
    private void LoadGameScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}
