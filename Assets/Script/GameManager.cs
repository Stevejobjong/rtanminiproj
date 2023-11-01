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
    public Text BestScoreText;
    public Text StartCountText;
    public GameObject EndText;

    public Card Card;
    public GameObject FirstCard;
    public GameObject SecondCard;
    public bool isStart = false;
    public bool isStartCnt = false;

    float time = 0.0f;
    float Fail = 2.0f;
    float Success = 1.0f;
    public float startCountDown = 3.99f;

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
        int[] rtans = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7 };

        rtans = rtans.OrderBy(item => Random.Range(-1.0f, 1.0f)).ToArray();

        for (int i = 0; i < 16; i++)
        {
            Card newCard = Instantiate(Card);
            newCard.transform.parent = GameObject.Find("Cards").transform;

            newCard.x = (i / 4) * 1.4f - 2.1f;
            newCard.y = (i % 4) * 1.4f - 3.0f;

            string rtanName = "rtan" + rtans[i].ToString();
            newCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(rtanName);
        }

        Time.timeScale = 1.0f;
    }

    private void Update() {
        TimeText.text = time.ToString("N2");
        if (isStart) {
            time += Time.deltaTime;
        }

        if (time > 50.0f)
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
            //CardDestroy();
            FirstCard.GetComponent<Card>().DestroyCard();
            SecondCard.GetComponent<Card>().DestroyCard();

            int cardsLeft = GameObject.Find("Cards").transform.childCount;
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
    void GameOver()
    {
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
        EndText.SetActive(true);
        BestScoreText.text = BestScore.ToString("N2");
    }

}
