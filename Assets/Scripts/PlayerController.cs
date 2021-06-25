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
        Wait,      // ゲージチャージ
        Ready,     // ゲージMax 攻撃可能
        Attack,    // 攻撃中
        DashAvoid,     // 回避中

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
    /// 移動用のキー入力
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
    /// ステートに応じて、ダッシュ回避か攻撃を行う
    /// </summary>
    private void Action() {

        if (Input.GetButtonDown("Jump")) {

            Vector3 dashX = transform.right * (x * dashPower);
            Vector3 dashZ = transform.forward * (z * dashPower);

            rb.AddForce(dashX + dashZ, ForceMode.Impulse);
            //Debug.Log(dashX + dashZ);

            // ステート確認
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
    /// 行動後の待機時間
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

        // 敵に接触した場合
        if (collision.gameObject.TryGetComponent(out EnemyController enemyController)) {
           
            rb.velocity = Vector3.zero;

            if (enemyController.IsDamaged) {
                return;
            }

            // ダメージ計算
            enemyController.CalcDamage(attackPower);
        }        
    }

    /// <summary>
    /// ダメージ計算
    /// </summary>
    private void CalcDamage() {　　　// 敵の情報をもらう
        // 敵からダメージを受ける

    }

    /// <summary>
    /// 攻撃ゲージのチャージ
    /// </summary>
    /// <returns></returns>
    private IEnumerator ChargeAttackGauge() {

        while (currentPlayerState == PlayerState.Wait) {

            chargePoint += chargePower;

            // TODO UI 連動

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
