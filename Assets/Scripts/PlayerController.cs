using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Animator anim;

    private float x;
    private float z;

    [SerializeField]
    private float moveSpeed;

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
        TryGetComponent(out anim);
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
    }

    /// <summary>
    /// 回避
    /// </summary>
    private void Avoid() {

    }

    private void OnCollisionEnter(Collision collision) {
        if (currentPlayerState != PlayerState.Attack) {
            return;
        }

        // 敵に攻撃

    }
}
