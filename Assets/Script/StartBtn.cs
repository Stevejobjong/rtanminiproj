using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartBtn : MonoBehaviour
{
    public AudioClip clip;

    // Start is called before the first frame update
    public void OnBtnStart()
    {
        SoundManager.instance.SFXPlay("StartBtn", clip);
        //버튼 클릭 시 buttonpress로 이미지 변경
        GetComponent<Image>().sprite = Resources.Load<Sprite>("buttonpress");
        transform.position = new Vector3(transform.position.x, transform.position.y - 27.0f, 0);//버튼 눌릴때 살짝 아래로
        //씬이 곧바로 로드되어 효과음이 재생되지 않는 문제 해결(1초 딜레이)
        Invoke("LoadGameScene", 1f);
    }
    private void LoadGameScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}
