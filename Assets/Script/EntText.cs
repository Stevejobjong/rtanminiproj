using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EntText : MonoBehaviour
{
   public void RetryGame()
   {
        SceneManager.LoadScene("MainScene");
    }
}
