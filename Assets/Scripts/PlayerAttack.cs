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
            //�񐔐���
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
            //�񐔐���
            if (Bombcount > 0)
            {
                //�J�[�\��������prefab���΂����߂�raycast���s��
                if(Physics.Raycast(ray, out hit))
                {
                    Debug.Log("v");
                    //prefab�̐���
                    GameObject projectile = Instantiate(Prefab, Camera.main.transform.position, Quaternion.identity);
                
                    //Rigitbody���擾���āA�͂�������
                    Rigidbody rb = projectile.GetComponent<Rigidbody>();

                    if (rb != null)
                    {
                        //�J�[�\���̃q�b�g�����ꏊ�̕�����Prefab���΂�
                        Vector3 shootDirection = (hit.point - Camera.main.transform.position).normalized;

                        rb.AddForce(shootDirection * ShootForce);
                        Bombcount--;
                    }
                }
            }
        }
    }
}
