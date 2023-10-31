using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public static class Coroutine_Caching
{
    public static readonly WaitForFixedUpdate WaitForFixedUpdate 
        = new WaitForFixedUpdate();
    public static readonly WaitForSeconds m_waitForSeconds = 
        new WaitForSeconds(5.0f);  
}

public class Card : MonoBehaviour {
    public float x, y;  //원래 카드의 좌표

    public Animator anim;

    public Text CountDownText;
 
    //private void Start() {
    //        GameManager.CardDestroy += DestroyCard;
    //        GameManager.CardClose += CloseCard;
    //    
    //}
    private float CountDown = 5.0f; 

    private IEnumerator CountDown_Coroutine;
    private void Start()
    {
        CountDown_Coroutine = CountDownRoutine();

        //x값이 0보다 작거나 같으면 x-5의 위치에서 크면 x+5의 위치에서 시작
        if (x <= 0) {
            transform.position = new Vector3(x - 5.0f, y, 0);
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180f));
            anim.SetTrigger("leftAppear");
        } else {
            transform.position = new Vector3(x + 5.0f, y, 0);
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, -180f));
            anim.SetTrigger("rightAppear");
        }
        StartCoroutine(MoveOnPlace());
    }

    public void OpenCard()
    {
        anim.SetBool("IsOpen", true);
        transform.Find("front").gameObject.SetActive(true);
        transform.Find("back").gameObject.SetActive(false);                  

        if (GameManager.instance.FirstCard == null)
        {
            GameManager.instance.FirstCard = gameObject;
            CountDown = 5.0f;
            CountDown_Coroutine = CountDownRoutine();
            StartCoroutine(CountDown_Coroutine); 
        }
        else
        {
            GameManager.instance.SecondCard = gameObject;
            GameManager.instance.IsMatched();
        }
    }

    IEnumerator CountDownRoutine()
    {        
        while(CountDown > 0.0f)
        {
            transform.Find("Canvas(CountDown)").gameObject.SetActive(true);
            CountDown -= Time.deltaTime;
            CountDownText.text = CountDown.ToString("N2");
            yield return null;
        }        
        CountDown = 5.0f;
        GameManager.instance.FirstCard = null;
        CloseCard();               
    }
    IEnumerator MoveOnPlace() { //원래 자리로 이동
        yield return new WaitForSeconds(0.5f);
        while (abs(x - transform.position.x) > 0.01f) { //원래 있어야할 위치와 현재 위치의 차이가 0.01f를 넘으면 계속 이동
            transform.position = Vector3.Lerp(transform.position, new Vector3(x, y, 0), Time.deltaTime * 5);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
    float abs(float x) { //절대값
        return x < 0 ? x * -1 : x;
    }
    public void DestroyCard()
    {
        transform.Find("Canvas(CountDown)").gameObject.SetActive(false);       
        StopCoroutine(CountDown_Coroutine);
        Invoke("DestroyCardInvoke", 0.7f);
    }

    void DestroyCardInvoke()
    {
        Destroy(gameObject);
    }

    public void CloseCard()
    {
        transform.Find("Canvas(CountDown)").gameObject.SetActive(false);
        StopCoroutine(CountDown_Coroutine);
        Invoke("CloseCardInvoke", 0.7f);
    }

    void CloseCardInvoke()
    {
        anim.SetBool("IsOpen", false);
        transform.Find("back").gameObject.SetActive(true);
        transform.Find("front").gameObject.SetActive(false);
    }
}
