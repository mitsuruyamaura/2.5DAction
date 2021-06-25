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
        Wait,      // Q[W`[W
        Ready,     // Q[WMax UÂ\
        Attack,    // U
        Avoid,     // ñð

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
    /// Ú®pÌL[üÍ
    /// </summary>
    private void InputMove() {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
    }

    private void FixedUpdate() {
        Move();
    }

    /// <summary>
    /// Ú®
    /// </summary>
    private void Move() {
        if (x != 0 || z != 0) {
            rb.velocity = new Vector3(x * moveSpeed, rb.velocity.y, z * moveSpeed);
        } else {
            rb.velocity = Vector3.zero;
        }
    }

    /// <summary>
    /// ñð
    /// </summary>
    private void Avoid() {

    }

    private void OnCollisionEnter(Collision collision) {
        if (currentPlayerState != PlayerState.Attack) {
            return;
        }

        // GÉU

    }
}
