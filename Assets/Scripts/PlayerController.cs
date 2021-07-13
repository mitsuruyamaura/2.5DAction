using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Animator anim;

    private float x;
    private float z;
    private float scale;


    private CinemachineImpulseSource cinemachineImpulseSource;

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

    [SerializeField]
    private Transform dashEffectTran;

    [SerializeField]
    private TimingGaugeController timingGaugeController;

    [SerializeField]
    private int attackCount = 1;

    private int maxAttackCount = 4;

    public enum PlayerState {
        Wait,      // ゲージチャージ
        Ready,     // ゲージMax 攻撃可能
        Attack,    // 攻撃中
        DashAvoid,     // 回避中

    }

    public PlayerState currentPlayerState;

    [SerializeField]
    private int hp;

    private int maxHp;

    private Battle battle;


    void Start()
    {
        TryGetComponent(out rb);
        anim = transform.GetComponentInChildren<Animator>();

        TryGetComponent(out cinemachineImpulseSource);

        scale = transform.localScale.x;

        StartCoroutine(ChargeAttackGauge());

        maxHp = hp;

        //DamageEffect();
    }


    void Update()
    {
        if (battle != null && battle.currentBattleState != BattleState.Play) {
            return;
        }

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
        
        if (battle != null && battle.currentBattleState != BattleState.Play) {
            return;
        }

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

            temp.x = x >= 0 ? scale : -scale;

            //if (x >= 0) {
            //    temp.x = scale;
            //} else {
            //    temp.x = -scale;
            //}
            transform.localScale = temp;
        }

        anim.SetFloat("Speed", x != 0 ? Mathf.Abs(x) : Mathf.Abs(z));
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
                anim.SetTrigger("Attack");

                //Debug.Log(timingGaugeController.CheckCritial());

                StartCoroutine(timingGaugeController.PausePointer());

                StartCoroutine(ActionInterval(attackIntervalTime));
            } else if (currentPlayerState == PlayerState.Wait) {
                currentPlayerState = PlayerState.DashAvoid;
                anim.SetTrigger("Dash");
                // 親の Scale を参照するので、向きも自動で変わる
                GameObject dashEffect = Instantiate(EffectManager.instance.dashWindPrefab, dashEffectTran);
                Destroy(dashEffect, 0.5f);
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

            // TODO 現在は敵にあたったときだけクリティカル判定。緩い場合には空振り時にリセットする
            //if (timingGaugeController.CheckCritial()) {
            //    attackCount++;
            //} else {
            //    attackCount = 1;
            //}

            attackCount = timingGaugeController.CheckCritial() ? attackCount += 1 : 1;
            Debug.Log(attackCount);

            // ダメージ計算
            StartCoroutine(enemyController.CalcDamage(attackPower, attackCount));

            if (attackCount >= maxAttackCount) {
                attackCount = 1;
            }
        }        
    }

    /// <summary>
    /// HP計算
    /// </summary>
    public void CalcHp(int damage) {　　　// 敵の情報をもらう

        hp = Mathf.Clamp(hp += damage, 0, maxHp);

        // TODO エフェクト


        Debug.Log("amount : " + damage);

        if (hp <= 0) {
            Debug.Log("Game Over");
        }
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

    /// <summary>
    /// 画面を揺らす演出
    /// </summary>
    public void DamageEffect() {
        cinemachineImpulseSource.GenerateImpulse();
    }

    /// <summary>
    /// PlayerController の初期設定
    /// </summary>
    /// <param name="battle"></param>
    public void SetUpPlayerController(Battle battle) {
        this.battle = battle;
    }
}
