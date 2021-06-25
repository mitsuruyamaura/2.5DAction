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
        Wait,      // �Q�[�W�`���[�W
        Ready,     // �Q�[�WMax �U���\
        Attack,    // �U����
        Avoid,     // ���

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
    /// �ړ��p�̃L�[����
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
    /// �ړ�
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
    /// �_�b�V�����
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

        // �G�ɍU��

    }
}
