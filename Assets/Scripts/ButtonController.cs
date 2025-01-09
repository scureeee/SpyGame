using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
   public void OnStrat()
    {
        //Game‰æ–Ê‚ÉˆÚs‚·‚é
        SceneManager.LoadScene("GameScene");
    }

    public void OnTitle()
    {
        //Title‚ÉˆÚs‚·‚é
        SceneManager.LoadScene("StartScene");
    }
}
