using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerManager : MonoBehaviour
{
    // �ړ����x
    [SerializeField] float moveSpeed = 4;
    [SerializeField] float jumpSpeed = 200;

    // �J������]�p�̕ϐ�
    [SerializeField] float x_rotate = 5f;   // �J������X����]�X�s�[�h
    [SerializeField] float y_rotate = 5f;   // �J������Y����]�X�s�[�h
    [SerializeField] float maxup = 60f;     // �J�����������ő�����
    [SerializeField] float mindown = -60f;  // �J�����������ő剺����

    private float x;    // �������̈ړ�
    private float z;    // �������̈ړ�
    private bool isJump = false;

    private Vector3 lastMousePosition;  // �}�E�X�ʒu�̒ǐ՗p
    private Vector3 newAngle;           // �J�����̉�]�p�̊p�x

    private Rigidbody rb;
    private Animator animator;



    // Start is called before the first frame update
    void Start()
    {
        // ���W�b�h�{�f�B�R���|�[�l���g���擾
        rb = GetComponent<Rigidbody>();
        // Animator�R���|�[�l���g���擾
        animator = GetComponent<Animator>();

        // �����}�E�X�ʒu�����݂̃}�E�X�ʒu�ŏ�����
        lastMousePosition = Input.mousePosition;
        // �����p�x�͌��݂̃Q�[���I�u�W�F�N�g�̃��[�J���p�x
        newAngle = this.gameObject.transform.localEulerAngles;

        // �J�[�\�����ŏ��̓��b�N���Ă���
        Cursor.lockState = CursorLockMode.Locked;
        //�J�[�\��������悤�ɂ��Ă���
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        // �L�[�{�[�h���͂��擾
        x = Input.GetAxisRaw("Horizontal");   // �������̎擾
        z = Input.GetAxisRaw("Vertical");     // �������̎擾

        // �W�����v�̓��͂��擾�i�X�y�[�X�L�[�j+ �ݒu���Ă��邩
        if (Input.GetKeyDown(KeyCode.Space) && animator.GetBool("IsGround"))
        {
            isJump = true; // �W�����v�t���O�𗧂Ă�
        }

        // �}�E�X�ɂ��J������]����
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            // �J������Y����]�i�����j
            newAngle.y += ((Input.GetAxis("Mouse X")) * y_rotate);
            // �J������X����]�i�����j
            newAngle.x -= ((Input.GetAxis("Mouse Y")) * x_rotate);

            // �J������������������A�����������Ȃ��悤�ɐ���
            newAngle.x = Mathf.Clamp(newAngle.x, mindown, maxup);

            // �J�����̊p�x���X�V
            this.gameObject.transform.localEulerAngles = newAngle;
        }

        // �}�E�X�̃��b�N�������K�v�ȏꍇ
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else if (Input.GetMouseButtonDown(0))  // �N���b�N�Ń��b�N�ĊJ
        {
            Cursor.lockState = CursorLockMode.Locked;
            //�J�[�\��������悤�ɂ��Ă���
            Cursor.visible = true;
        }
    }

    private void FixedUpdate()
    {
        // �J�����̑O�����Ɋ�Â��ړ������x�N�g�����쐬�iY�������̓[���ɂ���j
        Vector3 moveDirection = transform.forward * z + transform.right * x;
        moveDirection.y = 0;  // Y�������̈ړ���h��

        // �������̑��x�ݒ�
        rb.velocity = new Vector3(moveDirection.x * moveSpeed, rb.velocity.y, moveDirection.z * moveSpeed);

        // Animator Controller����p�����[�^�[���擾����
        animator.SetFloat("Speed", rb.velocity.magnitude);

        // �L�����N�^�[�̌�����i�s�����ɍ��킹��
        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }
    }

    // �R���C�_�[�ɐڐG���Ă���Ԃ����ɓ����Ă���
    private void OnCollisionStay(Collision other)
    {
        if (isJump)
        {
            if (animator.GetBool("IsGround"))
            {
                // �W�����v����i������ɗ͂�������j
                rb.AddForce(transform.up * jumpSpeed, ForceMode.Impulse);
                animator.SetTrigger("jumping");
                animator.SetFloat("JumpPower", 1);
                isJump = false;
            }
        }
    }

    // �R���C�_�[�ɓ�������
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            animator.SetBool("IsGround", true);
            animator.SetFloat("JumpPower", 0);
        }
    }

    // �R���C�_�[�����ꂽ��
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            animator.SetBool("IsGround", false);
        }
    }
}