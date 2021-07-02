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

    /// <summary>
    /// �_���[�W�v�Z
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
    /// ��]���o
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
    /// ��]���o
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
    /// �G�̔j��m�F
    /// </summary>
    private void CheckDestroy() {
        if (hp <= 0) {
            Destroy(gameObject);

            GameObject destroyEffect = Instantiate(EffectManager.instance.destroyEffectPrefab, transform.position, EffectManager.instance.destroyEffectPrefab.transform.rotation);
            Destroy(destroyEffect, 1.0f);
        } 
    }

    /// <summary>
    /// �t���[�g�\���쐬
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
    /// �U������
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
    /// �U��
    /// </summary>
    private void Attack() {
        // TODO �A�j���[�V����
        anim.SetTrigger("Attack");

        // �G�t�F�N�g


        playerController.CalcHp(-attackPower);
    }
}
