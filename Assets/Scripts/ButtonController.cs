using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
   public void OnStrat()
    {
        //Game画面に移行する
        SceneManager.LoadScene("GameScene");
    }

    public void OnTitle()
    {
        //Titleに移行する
        SceneManager.LoadScene("StartScene");
    }
}
