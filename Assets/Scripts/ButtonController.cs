using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
   public void OnStrat()
    {
        //Game��ʂɈڍs����
        SceneManager.LoadScene("GameScene");
    }

    public void OnTitle()
    {
        //Title�Ɉڍs����
        SceneManager.LoadScene("StartScene");
    }
}
