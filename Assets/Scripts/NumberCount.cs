using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NumberCount : MonoBehaviour
{

    public TextMeshProUGUI textDisplay;

    public PlayerAttack playerAttack;
    // Start is called before the first frame update
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        if (playerAttack != null)
        {
            textDisplay.text = "’e:" + playerAttack.Guncount.ToString() + "‰ñ";
        }
    }
}
