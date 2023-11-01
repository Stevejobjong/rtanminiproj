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

    public Text TimeText;
    public Text bestS;
    public Text bestSTitle;
    public Text endText;
    public GameObject Finish;

    public Card Card;
    public GameObject FirstCard;
    public GameObject SecondCard;

    float time = 0.0f;
    float Fail = 2.0f;
    float Success = 1.0f;

    public GameObject panelCanvas;

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
        time += Time.deltaTime;
        TimeText.text = time.ToString("N2");

        if(time > 50.0f)
        {
            TimeText.color = new Color32(255,0,0,255);
            if (time >= 60.0f)
            {
                GameOver();
            }
        }      
    }

    public void IsMatched()
    {
        string firstCardImage = FirstCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;
        string secondCardImage = SecondCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;

        if (firstCardImage == secondCardImage)
        {

            int membernum = (int)System.Enum.Parse(typeof(names), FirstCard.name);
            //CardDestroy();
            FirstCard.GetComponent<Card>().DestroyCard();
            SecondCard.GetComponent<Card>().DestroyCard();
            int cardsLeft = GameObject.Find("Cards").transform.childCount;

            panelCanvas.GetComponent<PanelManager>().Openpanel(membernum);

            if(time > 1.0f)
            {
                time -= Success;
            }           
            if (cardsLeft == 2)
            {
                //EndText.SetActive(true);
                //Time.timeScale = 0.0f;
                GameOver();
            }            
        }
        else
        {
            //CardClose();
            FirstCard.GetComponent<Card>().CloseCard();
            SecondCard.GetComponent<Card>().CloseCard();            
            if(time < 58.0f)
            {
                time += Fail;
            }
            else
            {
                time = 60.0f;
                Time.timeScale = 0.0f;
            }
        }

        FirstCard = null;
        SecondCard = null;
    }

    void GameOver()
    {
        if (time < 60.0f)
        {
            endText.text = "Clear!";
        }
        else
        {
            endText.text = "Game Over!";
        }

        Time.timeScale = 0.0f;
        if (PlayerPrefs.HasKey("BestScore") == false)
        {
            PlayerPrefs.SetFloat("BestScore" , time);
        }
        else
        {
            if(time < PlayerPrefs.GetFloat("BestScore"))
            {
                PlayerPrefs.SetFloat("BestScore", time);
            }
        }
        float BestScore = PlayerPrefs.GetFloat("BestScore");
        bestS.text = BestScore.ToString("N2");
        Finish.SetActive(true);
    }
}
