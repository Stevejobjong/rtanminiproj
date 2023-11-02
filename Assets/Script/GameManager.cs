using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //public delegate void DetroyFunction();
    //public delegate void CloseFunction();
    //public static event DetroyFunction CardDestroy;
    //public static event CloseFunction CardClose;

    public GameObject bg;
    public Text TimeText;
    public Text BestSText;
    public Text endText;
    public Text MatchCountText;

    public GameObject endTxt;
    public Card Card;
    public Transform cards;

    [HideInInspector]
    public GameObject FirstCard;
    [HideInInspector]
    public GameObject SecondCard;

    public Text StartCountText;

    float time = 0.0f;
    float Fail = 3.0f;
    //float Success = 1.0f;
    public float TimeLimit; // 인스펙터창에서 남은시간 조절할수있게
    float RemainTime; // 남은시간
    int MatchCount = 0; // 매칭 시도횟수

    [HideInInspector]
    public bool isStartCnt = false;
    [HideInInspector]
    public bool isStart = false;

    [HideInInspector]
    public float startCountDown = 3.99f;
    public GameObject panelCanvas;

    [HideInInspector]
    public bool twoselect = false;
    public int bombcount = 6;

    public enum names { 임종운, 변정민, 조성민, 권오태, 김윤진, 폭탄a, 폭탄b, 폭탄c }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
         SoundManager.instance.bgSound.pitch = 1f; // 속도는 다시 원상태로 복구
         RemainTime = TimeLimit;
         int[] rtans = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7 };

        rtans = rtans.OrderBy(item => Random.Range(-1.0f, 1.0f)).ToArray();

        for (int i = 0; i < 16; i++)
        {
            Card newCard = Instantiate(Card);
            newCard.transform.parent = GameObject.Find("Cards").transform;

            newCard.x = (i / 4) * 1.4f - 2.1f;
            newCard.y = (i % 4) * 1.4f - 3.0f;

            string rtanName = System.Enum.GetName(typeof(names), rtans[i])+2; //뒤에 숫자 부분은 아직 카드가 늘어남(난이도 증가)에 따라 명명규칙을 모르는 상태
            newCard.name = System.Enum.GetName(typeof(names), rtans[i]); //판넬을 불러오기 위해 해당 카드의 이름만 지정
            newCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(rtanName);
        }

        Time.timeScale = 1.0f;
    }

    private void Update()
    {
        TimeText.text = RemainTime.ToString("N2");
        if (isStart) {
            time += Time.deltaTime;
            RemainTime = TimeLimit - time;
        }

        if(RemainTime <= 10.0f) // 10초 남았을때
        {
            SoundManager.instance.bgSound.pitch = 1.2f; // 브금 속도 바르게 변경
            bg.GetComponent<Animator>().SetBool("warning", true); // 배경 빨갛게 깜빡이며 경고느낌 나게하기
            //TimeText.color = new Color32(255,0,0,255);
            if (RemainTime <= 0.0f)
            {
                endText.text = "Game Over!!";  // 시간 초과인 경우 게임 오버
                GameOver();
            }
        }
        MatchCountText.text = "Try : " + MatchCount;
    }
    public void StartCountFunc() {
        if (!isStartCnt) {
            isStartCnt = true;
            StartCoroutine(StartCount());
        }
    }
    public IEnumerator StartCount() {
        StartCountText.gameObject.SetActive(true);
        while (startCountDown > 1.0f) {
            startCountDown -= Time.deltaTime;
            float n = Mathf.Floor(startCountDown);
            StartCountText.text = n.ToString("N0");

            yield return null;
        }
        StartCountText.gameObject.SetActive(false);
    }

    public void IsMatched()
    {
        MatchCount++;

        string firstCardImage = FirstCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;
        string secondCardImage = SecondCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;

        if (firstCardImage == secondCardImage)
        {
            SoundManager.instance.bgSound.PlayOneShot(SoundManager.instance.soundEffect[1]); //매칭성공효과음
            int membernum = (int)System.Enum.Parse(typeof(names), FirstCard.name);
            //CardDestroy();
            FirstCard.GetComponent<Card>().DestroyCard();
            SecondCard.GetComponent<Card>().DestroyCard();
            int cardsLeft = GameObject.Find("Cards").transform.childCount;
            int remainingCards = cards.childCount;

            panelCanvas.GetComponent<PanelManager>().Openpanel(membernum);

            if (remainingCards == 2+bombcount)
            {
                Time.timeScale = 0.0f;
                endText.text = "Clear!!";  // 남은 카드가 0인 경우 클리어
                GameOver();
            }
        }
        else
        {
            SoundManager.instance.bgSound.PlayOneShot(SoundManager.instance.soundEffect[2]); //매칭실패효과음
            TimeText.GetComponent<Animator>().SetTrigger("isFail"); //틀렸을때 색깔 빨갛게 하는 애니메이션 트리거
            FirstCard.GetComponent<Card>().CloseCard();
            SecondCard.GetComponent<Card>().CloseCard();            
            if(RemainTime > 3.0f)
            {
                time += Fail;
            }
            else
            {
                RemainTime = 0.01f;
                endText.text = "Game Over!!";  // 시간 초과인 경우 게임 오버
                GameOver();
            }
        }

        FirstCard = null;
        SecondCard = null;
    }

    void GameOver()
    {
        
        

        if (endText.text == "Clear!!")
        {
            SoundManager.instance.bgSound.PlayOneShot(SoundManager.instance.soundEffect[3]); //클리어 효과음
            if (PlayerPrefs.HasKey("BestSText") == false)
            {
                PlayerPrefs.SetFloat("BestSText", RemainTime);
            }
            else
            {
                if (RemainTime < PlayerPrefs.GetFloat("BestSText"))
                {
                    PlayerPrefs.SetFloat("BestSText", RemainTime);
                }
            }
        }
        else
        {
            SoundManager.instance.bgSound.PlayOneShot(SoundManager.instance.soundEffect[4]); //실패 효과음
        }

        float BestScore = PlayerPrefs.GetFloat("BestSText");

        endTxt.SetActive(true);
        BestSText.text = BestScore.ToString("N2");
        isStart = false;
        Invoke("Stopbgsound", 0.5f);
        SoundManager.instance.bgSound.pitch = 1f; // 속도는 다시 원상태로 복구
        Time.timeScale = 0.0f;
    }
    void Stopbgsound()
    {
        SoundManager.instance.bgSound.Stop(); // 게임오버될때 브금멈추기
    }
}
