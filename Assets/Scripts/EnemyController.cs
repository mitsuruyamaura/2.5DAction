using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

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

    [SerializeField]
    private BulletController bulletPrefab;

    [SerializeField]
    private float bulletSpeed;

    [SerializeField]
    private EnemyMoveType enemyMoveType;

    [SerializeField]
    private float moveDuraiton;

    private UnityAction<Transform, float> moveEvent;

    [SerializeField]
    private int exp;

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
            battle.RemoveEnemyFromEnemiesList(this);

            GameObject destroyEffect = Instantiate(EffectManager.instance.destroyEffectPrefab, transform.position, EffectManager.instance.destroyEffectPrefab.transform.rotation);
            Destroy(destroyEffect, 1.0f);

            // Exp ���Z�@���Ƃŏ���l�̐�������
            GameData.instance.totalExp += exp;

            Destroy(gameObject);
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
        if (playerController != null) {
            return;
        }

        if (playerController == null && other.TryGetComponent(out playerController)) {

            if (!isAttack) {
                isAttack = true;

                StartCoroutine(PreparateAttack());
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (playerController != null && other.TryGetComponent(out playerController)) {
            playerController = null;
            isAttack = false;
            Debug.Log("�U���͈͊O");
        }
    }

    /// <summary>
    /// �U������
    /// </summary>
    /// <returns></returns>
    private IEnumerator PreparateAttack() {
        Debug.Log("����");

        int timer = 0;

        while (isAttack) {
            timer++;

            if (timer > attackIntervalTime) {
                timer = 0;
                Attack();
            }
            yield return null;
        }

        Debug.Log("�U���I��");
    }

    void Start() {
        transform.GetChild(0).TryGetComponent(out anim);
        //StartCoroutine(PreparateAttack());    
    }

    /// <summary>
    /// �U��
    /// </summary>
    private void Attack() {
        Debug.Log("�U��");

        // TODO �A�j���[�V����
        anim.SetTrigger("Attack");

        // �G�t�F�N�g


        // �����̐؂�ւ��m�F
        Vector3 temp = transform.localScale;

        // TODO �O�����Z�q�ɕς���
        if (playerController.transform.position.x > transform.position.x) {
            temp.x *= 1.0f;
        } else {
            temp.x *= -1.0f;
        }
        transform.localScale = temp;

        BulletController bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        bullet.Shoot(GetBulletDirection(), bulletSpeed, attackPower, temp.x);

        //playerController.CalcHp(-attackPower);
    }

    /// <summary>
    /// �o���b�g�𔭎˂����������s��
    /// </summary>
    /// <returns></returns>
    private Vector3 GetBulletDirection() {
        return (playerController.transform.position - transform.position).normalized;
    }

    /// <summary>
    /// �G�̏����ݒ�
    /// </summary>
    /// <param name="battle"></param>
    public void SetUpEnemy(Battle battle) {
        this.battle = battle;

        moveEvent = DataBaseManager.instance.enemyMoveEventDataSO.GetEnemyMove(enemyMoveType);

        moveEvent.Invoke(transform, moveDuraiton);
    }
}
