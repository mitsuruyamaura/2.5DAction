using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Animator anim;

    private float x;
    private float z;
    private float scale;

    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private float dashPower;

    public enum PlayerState {
        Wait,      // ゲージチャージ
        Ready,     // ゲージMax 攻撃可能
        Attack,    // 攻撃中
        Avoid,     // 回避中

    }

    public PlayerState currentPlayerState;

    void Start()
    {
        TryGetComponent(out rb);
        anim = transform.GetComponentInChildren<Animator>();

        scale = transform.localScale.x;
    }


    void Update()
    {
        InputMove();
    }

    /// <summary>
    /// 移動用のキー入力
    /// </summary>
    private void InputMove() {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
    }

    private void FixedUpdate() {
        Move();
        DashAvoid();
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Move() {
        if (x != 0 || z != 0) {
            rb.velocity = new Vector3(x * moveSpeed, rb.velocity.y, z * moveSpeed);
            
        } else {
            rb.velocity = Vector3.zero;
        }

        if (x != 0) {
            Vector3 temp = transform.localScale;

            if (x >= 0) {
                temp.x = scale;
            } else {
                temp.x = -scale;
            }
            transform.localScale = temp;
        }
        
    }

    /// <summary>
    /// ダッシュ回避
    /// </summary>
    private void DashAvoid() {
        if (Input.GetButtonDown("Jump")) {
            //if (x != 0 || z != 0) {
                Vector3 dashX = transform.right * (x * dashPower);
                Vector3 dashZ = transform.forward * (z * dashPower);


                rb.AddForce(dashX + dashZ, ForceMode.Impulse);

                Debug.Log(dashX + dashZ);

            //}
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (currentPlayerState != PlayerState.Attack) {
            return;
        }

        // 敵に攻撃

    }
}
