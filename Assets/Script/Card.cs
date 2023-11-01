using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public static class Coroutine_Caching   //�ڷ�ƾ ����ȭ(ĳ���Ͽ� �������� ���δ�)
{
    public static readonly WaitForFixedUpdate WaitForFixedUpdate 
        = new WaitForFixedUpdate();
    public static readonly WaitForSeconds m_waitForSeconds = 
        new WaitForSeconds(5.0f);  
}

public class Card : MonoBehaviour {
    public float x, y;  //���� ī���� ��ǥ

    public Animator anim;
    public Text CountDownText;
 
    private float CountDown = 5.0f;

    private IEnumerator CountDown_Coroutine;
    private void Start()
    {
        CountDown_Coroutine = CountDownRoutine();

        //x���� 0���� �۰ų� ������ x-5�� ��ġ���� ũ�� x+5�� ��ġ���� ����
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
            if (gameObject == GameManager.instance.FirstCard)   //���� ī�� ���ý� ����X
                return;

            GameManager.instance.SecondCard = gameObject;
            GameManager.instance.IsMatched();
        }
    }
    IEnumerator CountDownRoutine()  //ù ī�� ���½� 5�� ī��Ʈ �ٿ�
    {        
        while(CountDown > 0.0f)
        {
            transform.Find("Canvas(CountDown)").gameObject.SetActive(true);
            CountDown -= Time.deltaTime;
            CountDownText.text = CountDown.ToString("N2");
            yield return null;
        }        
        CountDown = 5.0f;
        GameManager.instance.FirstCard = null;  //5�ʰ� ������ ù ī�� ���� ��
        CloseCard();                            //ī�� �ٽ� ������
    }
    IEnumerator MoveOnPlace() { //���� �ڸ��� �̵�
        yield return new WaitForSeconds(0.5f);
        while (abs(x - transform.position.x) > 0.01f) { //���� �־���� ��ġ�� ���� ��ġ�� ���̰� 0.01f�� ������ ��� �̵�
            transform.position = Vector3.Lerp(transform.position, new Vector3(x, y, 0), Time.deltaTime * 5);
            yield return null;
        }
        anim.SetBool("IsOpen", true);
        transform.Find("back").gameObject.SetActive(false);
        transform.Find("front").gameObject.SetActive(true);
        GameManager.instance.StartCountFunc();
        yield return new WaitForSeconds(GameManager.instance.startCountDown-1.0f);
        anim.SetBool("IsOpen", false);
        transform.Find("back").gameObject.SetActive(true);
        transform.Find("front").gameObject.SetActive(false);
        GameManager.instance.isStart = true;
    }
    float abs(float x) { //���밪
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
