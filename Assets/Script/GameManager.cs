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
    float RemainTime;
    int MatchCount = 0;

    [HideInInspector]
    public bool isStartCnt = false;
    [HideInInspector]
    public bool isStart = false;

    public float startCountDown = 3.99f;
    public GameObject panelCanvas;

    [HideInInspector]
    public bool twoselect = false;

    public enum names { 임종운, 변정민, 조성민, 권오태, 김윤진 }

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
         RemainTime = TimeLimit;
         int[] rtans = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4 };

        rtans = rtans.OrderBy(item => Random.Range(-1.0f, 1.0f)).ToArray();

        for (int i = 0; i < 10; i++)
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

        if(RemainTime <= 10.0f)
        {
            bg.GetComponent<Animator>().SetBool("warning", true);
            TimeText.color = new Color32(255,0,0,255);
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
            int membernum = (int)System.Enum.Parse(typeof(names), FirstCard.name);
            //CardDestroy();
            FirstCard.GetComponent<Card>().DestroyCard();
            SecondCard.GetComponent<Card>().DestroyCard();
            int cardsLeft = GameObject.Find("Cards").transform.childCount;
            int remainingCards = cards.childCount;

            panelCanvas.GetComponent<PanelManager>().Openpanel(membernum);

            if (remainingCards == 2)
            {
                Time.timeScale = 0.0f;
                endText.text = "Clear!!";  // 남은 카드가 0인 경우 클리어
                GameOver();
            }

            /*if (cardsLeft <= 1) // 이부분은 남은카드가 2개일때 바로 종료하는 코드인가? 일단 삭제
            {
                endText.text = "Clear!!";  // 남은 카드가 0인 경우 클리어
                GameOver();
            }*/

            /* 성공했을때 시간추가하는부분 삭제
            if(time < 58.0f)
            {
                time += Success;
            }
            */

        }
        else
        {
            TimeText.GetComponent<Animator>().SetTrigger("isFail"); //틀렸을때 색깔 빨갛게 하는 애니메이션 트리거
            //CardClose();
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
        Time.timeScale = 0.0f;

        if (endText.text == "Clear!!")
        {
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

        float BestScore = PlayerPrefs.GetFloat("BestSText");

        endTxt.SetActive(true);
        BestSText.text = BestScore.ToString("N2");
        isStart = false;
    }
}
