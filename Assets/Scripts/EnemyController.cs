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

    public int attackPower;

    [SerializeField]
    private Transform floatingMessageTran;

    private bool isDamaged;

    public bool IsDamaged {
        set { isDamaged = value; }
        get { return isDamaged; }
    }

    private PlayerController playerController;
    private bool isAttack;

    [SerializeField]
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
    private System.Func<Transform, float> moveFunc;

    [SerializeField]
    private int exp;

    private Tween tween;


    public enum EnemyState{
        Attack,
        Move,
        Wait
    }
    public EnemyState currentEnemyState;

    [SerializeField]
    private Transform effectPos;

    /// <summary>
    /// �_���[�W�v�Z
    /// </summary>
    /// <param name="damage"></param>
    public IEnumerator CalcDamage(int damage, int attackCount) {
        
        isDamaged = true;

        //anim.SetTrigger("Hit");

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

            if (effectPos != null) {
                destroyEffect.transform.position = effectPos.position;
            }
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

                if (enemyMoveType == EnemyMoveType.Assasin) {
                    while (currentEnemyState == EnemyState.Wait) {
                        // 70% �U��
                        if (Random.Range(0, 100) <= 70) {
                            Vector3 targetPos = new Vector3(playerController.transform.position.x, transform.position.y, playerController.transform.position.z);
                            currentEnemyState = EnemyState.Attack;

                            // �ړ��U��
                            tween = transform.DOMove(targetPos, Random.Range(1.0f, 2.0f)).SetEase(Ease.InOutQuart).OnComplete(() => { Attack(); });
                        } else {
                            Vector3 direction = new Vector3(transform.position.x + Random.Range(3, 5), transform.position.y, transform.position.z + Random.Range(3, 5));
                            currentEnemyState = EnemyState.Move;

                            // �ړ�����
                            tween = transform.DOMove(direction, Random.Range(2.0f, 3.0f)).SetEase(Ease.Linear).OnComplete(() => { currentEnemyState = EnemyState.Wait; });
                        }
                    }
                } else {

                    Attack();
                }
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

        currentEnemyState = EnemyState.Wait;
        tween = null;

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

        // �A�[�`���[�̏ꍇ�A�e�𔭎�
        if (enemyMoveType == EnemyMoveType.Archer) {
            BulletController bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            bullet.Shoot(GetBulletDirection(), bulletSpeed, attackPower, temp.x);
        }


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
