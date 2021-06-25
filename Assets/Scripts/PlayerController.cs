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

    [SerializeField]
    private int chargePower;

    [SerializeField]
    private int chargePoint;

    [SerializeField]
    private int maxChargePoint;

    [SerializeField]
    private int attackPower;

    [SerializeField]
    private float attackIntervalTime;

    [SerializeField]
    private float dashAvoidIntervalTime;



    public enum PlayerState {
        Wait,      // �Q�[�W�`���[�W
        Ready,     // �Q�[�WMax �U���\
        Attack,    // �U����
        DashAvoid,     // ���

    }

    public PlayerState currentPlayerState;

    void Start()
    {
        TryGetComponent(out rb);
        anim = transform.GetComponentInChildren<Animator>();

        scale = transform.localScale.x;

        StartCoroutine(ChargeAttackGauge());
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
        Action();
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
    /// �X�e�[�g�ɉ����āA�_�b�V��������U�����s��
    /// </summary>
    private void Action() {

        if (Input.GetButtonDown("Jump")) {

            Vector3 dashX = transform.right * (x * dashPower);
            Vector3 dashZ = transform.forward * (z * dashPower);

            rb.AddForce(dashX + dashZ, ForceMode.Impulse);
            //Debug.Log(dashX + dashZ);

            // �X�e�[�g�m�F
            if (currentPlayerState == PlayerState.Ready) {
                currentPlayerState = PlayerState.Attack;
                StartCoroutine(ActionInterval(attackIntervalTime));
            } else if (currentPlayerState == PlayerState.Wait) {
                currentPlayerState = PlayerState.DashAvoid;
                StartCoroutine(ActionInterval(dashAvoidIntervalTime));
            }
        }
    }

    /// <summary>
    /// �s����̑ҋ@����
    /// </summary>
    /// <returns></returns>
    private IEnumerator ActionInterval(float intervalTime) {
        yield return new WaitForSeconds(intervalTime);

        currentPlayerState = PlayerState.Wait;

        StartCoroutine(ChargeAttackGauge());
    }

    private void OnCollisionEnter(Collision collision) {
        if (currentPlayerState != PlayerState.Attack) {
            return;
        }

        // �G�ɐڐG�����ꍇ
        if (collision.gameObject.TryGetComponent(out EnemyController enemyController)) {
           
            rb.velocity = Vector3.zero;

            if (enemyController.IsDamaged) {
                return;
            }

            // �_���[�W�v�Z
            enemyController.CalcDamage(attackPower);
        }        
    }

    /// <summary>
    /// �_���[�W�v�Z
    /// </summary>
    private void CalcDamage() {�@�@�@// �G�̏������炤
        // �G����_���[�W���󂯂�

    }

    /// <summary>
    /// �U���Q�[�W�̃`���[�W
    /// </summary>
    /// <returns></returns>
    private IEnumerator ChargeAttackGauge() {

        while (currentPlayerState == PlayerState.Wait) {

            chargePoint += chargePower;

            // TODO UI �A��

            if (chargePoint >= maxChargePoint) {
                chargePoint = 0;

                currentPlayerState = PlayerState.Ready;

                GameObject chargeEffect = Instantiate(EffectManager.instance.chargeUpEffectPrefab, transform.position, EffectManager.instance.chargeUpEffectPrefab.transform.rotation);
                chargeEffect.transform.SetParent(transform);
                chargeEffect.transform.localPosition = new Vector3(chargeEffect.transform.localPosition.x, chargeEffect.transform.localPosition.y + 0.5f, chargeEffect.transform.localPosition.z);
                Destroy(chargeEffect, 1.5f);
            }

            yield return null;
        }
    }
}
