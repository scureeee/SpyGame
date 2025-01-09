using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject Prefab;
    public int Guncount;
    public int Bombcount;
    private float ShootForce = 600f;

    // Start is called before the first frame update
    void Start()
    {
        Guncount = 1;
        Bombcount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Input.GetMouseButtonDown(0))
        {
            //回数制限
            if(Guncount > 0)
            {
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.CompareTag("Enemy"))
                    {
                        Destroy(hit.collider.gameObject);
                        Guncount--;
                    }
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.V))
        {
            //回数制限
            if (Bombcount > 0)
            {
                //カーソル方向にprefabを飛ばすためのraycastを行う
                if(Physics.Raycast(ray, out hit))
                {
                    Debug.Log("v");
                    //prefabの生成
                    GameObject projectile = Instantiate(Prefab, Camera.main.transform.position, Quaternion.identity);
                
                    //Rigitbodyを取得して、力を加える
                    Rigidbody rb = projectile.GetComponent<Rigidbody>();

                    if (rb != null)
                    {
                        //カーソルのヒットした場所の方向にPrefabを飛ばす
                        Vector3 shootDirection = (hit.point - Camera.main.transform.position).normalized;

                        rb.AddForce(shootDirection * ShootForce);
                        Bombcount--;
                    }
                }
            }
        }
    }
}
