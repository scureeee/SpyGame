using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;

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
            textMeshPro.text = "”š’e:" + playerAttack.Bombcount.ToString() + "‰ñ";
        }
    }
}
