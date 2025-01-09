using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerManager : MonoBehaviour
{
    // 移動速度
    [SerializeField] float moveSpeed = 4;
    [SerializeField] float jumpSpeed = 200;

    // カメラ回転用の変数
    [SerializeField] float x_rotate = 5f;   // カメラのX軸回転スピード
    [SerializeField] float y_rotate = 5f;   // カメラのY軸回転スピード
    [SerializeField] float maxup = 60f;     // カメラが向く最大上方向
    [SerializeField] float mindown = -60f;  // カメラが向く最大下方向

    private float x;    // 横方向の移動
    private float z;    // 奥方向の移動
    private bool isJump = false;

    private Vector3 lastMousePosition;  // マウス位置の追跡用
    private Vector3 newAngle;           // カメラの回転用の角度

    private Rigidbody rb;
    private Animator animator;



    // Start is called before the first frame update
    void Start()
    {
        // リジッドボディコンポーネントを取得
        rb = GetComponent<Rigidbody>();
        // Animatorコンポーネントを取得
        animator = GetComponent<Animator>();

        // 初期マウス位置を現在のマウス位置で初期化
        lastMousePosition = Input.mousePosition;
        // 初期角度は現在のゲームオブジェクトのローカル角度
        newAngle = this.gameObject.transform.localEulerAngles;

        // カーソルを最初はロックしておく
        Cursor.lockState = CursorLockMode.Locked;
        //カーソル見えるようにしておく
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        // キーボード入力を取得
        x = Input.GetAxisRaw("Horizontal");   // 横方向の取得
        z = Input.GetAxisRaw("Vertical");     // 奥方向の取得

        // ジャンプの入力を取得（スペースキー）+ 設置しているか
        if (Input.GetKeyDown(KeyCode.Space) && animator.GetBool("IsGround"))
        {
            isJump = true; // ジャンプフラグを立てる
        }

        // マウスによるカメラ回転処理
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            // カメラのY軸回転（水平）
            newAngle.y += ((Input.GetAxis("Mouse X")) * y_rotate);
            // カメラのX軸回転（垂直）
            newAngle.x -= ((Input.GetAxis("Mouse Y")) * x_rotate);

            // カメラが上向きすぎず、下向きすぎないように制限
            newAngle.x = Mathf.Clamp(newAngle.x, mindown, maxup);

            // カメラの角度を更新
            this.gameObject.transform.localEulerAngles = newAngle;
        }

        // マウスのロック解除が必要な場合
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else if (Input.GetMouseButtonDown(0))  // クリックでロック再開
        {
            Cursor.lockState = CursorLockMode.Locked;
            //カーソル見えるようにしておく
            Cursor.visible = true;
        }
    }

    private void FixedUpdate()
    {
        // カメラの前方向に基づく移動方向ベクトルを作成（Y軸成分はゼロにする）
        Vector3 moveDirection = transform.forward * z + transform.right * x;
        moveDirection.y = 0;  // Y軸方向の移動を防ぐ

        // 横方向の速度設定
        rb.velocity = new Vector3(moveDirection.x * moveSpeed, rb.velocity.y, moveDirection.z * moveSpeed);

        // Animator Controllerからパラメーターを取得する
        animator.SetFloat("Speed", rb.velocity.magnitude);

        // キャラクターの向きを進行方向に合わせる
        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }
    }

    // コライダーに接触している間ここに入ってくる
    private void OnCollisionStay(Collision other)
    {
        if (isJump)
        {
            if (animator.GetBool("IsGround"))
            {
                // ジャンプする（上方向に力を加える）
                rb.AddForce(transform.up * jumpSpeed, ForceMode.Impulse);
                animator.SetTrigger("jumping");
                animator.SetFloat("JumpPower", 1);
                isJump = false;
            }
        }
    }

    // コライダーに入った時
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            animator.SetBool("IsGround", true);
            animator.SetFloat("JumpPower", 0);
        }
    }

    // コライダーが離れた時
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            animator.SetBool("IsGround", false);
        }
    }
}