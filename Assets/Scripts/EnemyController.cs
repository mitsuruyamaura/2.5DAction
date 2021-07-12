using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private int hp;

    [SerializeField]
    private float rotateSpeed;

    [SerializeField]
    private int attackIntervalTime;

    [SerializeField]
    private int attackPower;

    [SerializeField]
    private Transform floatingMessageTran;

    private bool isDamaged;

    public bool IsDamaged {
        set { isDamaged = value; }
        get { return isDamaged; }  
    }

    private PlayerController playerController;
    private bool isAttack;

    private Animator anim;
    private Battle battle;

    /// <summary>
    /// ダメージ計算
    /// </summary>
    /// <param name="damage"></param>
    public IEnumerator CalcDamage(int damage, int attackCount) {
        
        isDamaged = true;

        anim.SetTrigger("Hit");

        for (int i = 0; i < attackCount; i++) {
            hp -= damage;

            DoRotate();

            GameObject hitEffect = Instantiate(EffectManager.instance.enemyHitEffectPrefab, transform);
            Destroy(hitEffect, 1.0f);

            GenerateFloatingMessage(damage);

            yield return new WaitForSeconds(0.15f);
        }

        Debug.Log(attackCount);
        //StartCoroutine(Rotate());
    }

    /// <summary>
    /// 回転演出
    /// </summary>
    /// <returns></returns>
    private IEnumerator Rotate() {
        int timer = 0;
        while (timer <= 150) {
            timer++;
            transform.Rotate(0, rotateSpeed, 0);
            yield return null;
        }
        transform.eulerAngles = Vector3.zero;

        isDamaged = false;
    }

    /// <summary>
    /// 回転演出
    /// </summary>
    private void DoRotate() {
        transform.DORotate(new Vector3(0, 720, 0), 1.5f, RotateMode.FastBeyond360)
            .SetEase(Ease.OutBack)
            .OnComplete(() => 
            { 
                isDamaged = false;
                CheckDestroy();
            });
    }

    /// <summary>
    /// 敵の破壊確認
    /// </summary>
    private void CheckDestroy() {
        if (hp <= 0) {
            battle.RemoveEnemyFromEnemiesList(this);

            GameObject destroyEffect = Instantiate(EffectManager.instance.destroyEffectPrefab, transform.position, EffectManager.instance.destroyEffectPrefab.transform.rotation);
            Destroy(destroyEffect, 1.0f);

            Destroy(gameObject);
        } 
    }

    /// <summary>
    /// フロート表示作成
    /// </summary>
    /// <param name="damage"></param>
    private void GenerateFloatingMessage(int damage) {
        FloatingMessageControler floatingMessage = Instantiate(EffectManager.instance.floatingMessagePrefab, floatingMessageTran);

        floatingMessage.SetUpFloatingMessage(damage);
    }

    private void OnTriggerStay(Collider other) {
        if (playerController == null && other.TryGetComponent(out playerController)) {
            isAttack = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        playerController = null;
        isAttack = false;
    }

    /// <summary>
    /// 攻撃準備
    /// </summary>
    /// <returns></returns>
    private IEnumerator PraparateAttack() {

        int timer = 0;

        while (true) {
            if (isAttack) {
                timer++;

                if (timer > attackIntervalTime) {
                    timer = 0;
                    Attack();
                }
            }
            yield return null;
        }
    }

    void Start() {
        transform.GetChild(0).TryGetComponent(out anim);
        StartCoroutine(PraparateAttack());    
    }

    /// <summary>
    /// 攻撃
    /// </summary>
    private void Attack() {
        // TODO アニメーション
        anim.SetTrigger("Attack");

        // エフェクト


        playerController.CalcHp(-attackPower);
    }

    /// <summary>
    /// 敵の初期設定
    /// </summary>
    /// <param name="battle"></param>
    public void SetUpEnemy(Battle battle) {
        this.battle = battle;
    }
}
