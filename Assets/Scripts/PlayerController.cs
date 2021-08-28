using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Animator anim;

    private float x;
    private float z;
    private float scale;


    private CinemachineImpulseSource cinemachineImpulseSource;

    //[SerializeField]
    //private float moveSpeed;

    //[SerializeField]
    //private float dashPower;

    //[SerializeField]
    //private int chargePower;

    //[SerializeField]
    private int chargePoint;

    //[SerializeField]
    //private int maxChargePoint;

    //[SerializeField]
    //private int attackPower;

    //[SerializeField]
    //private float attackIntervalTime;

    //[SerializeField]
    //private float dashAvoidIntervalTime;

    [SerializeField]
    private Transform dashEffectTran;

    [SerializeField]
    private TimingGaugeController timingGaugeController;

    [SerializeField]
    private int attackCount = 1;

    private int maxAttackCount = 4;

    public enum PlayerState {
        Wait,      // �Q�[�W�`���[�W
        Ready,     // �Q�[�WMax �U���\
        Attack,    // �U����
        DashAvoid,     // ���

    }

    public PlayerState currentPlayerState;

    //[SerializeField]
    //private int hp;

    //private int maxHp;

    private Battle battle;

    [SerializeField]
    private Joystick joystick;

    [SerializeField]
    private Button btnAction;

    [SerializeField]
    private Image imgCutin;

    private IEnumerator actionCoroutine;


    void Start()  // TODO SetUp ���\�b�h�Ɉڂ�
    {
        TryGetComponent(out rb);
        anim = transform.GetComponentInChildren<Animator>();

        TryGetComponent(out cinemachineImpulseSource);

        scale = transform.localScale.x;

        StartCoroutine(ChargeAttackGauge());

        //maxHp = hp;

        btnAction.onClick.AddListener(Action);

        imgCutin.gameObject.SetActive(false);

        //DamageEffect();
    }


    void Update() {
        if (battle != null && battle.currentBattleState != BattleState.Play) {
            return;
        }

        InputMove();

        if (Input.GetButtonDown("Jump")) {
            Action();
        }

        if (Input.GetKeyDown(KeyCode.K) && actionCoroutine == null) {

            // �J�b�g�C���Đ�
            actionCoroutine = SpecialAttackEffect();
            StartCoroutine(actionCoroutine);

            // TODO ����U��

        }
    }

    /// <summary>
    /// �ړ��p�̃L�[����
    /// </summary>
    private void InputMove() {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
        
        x = joystick.Horizontal;
        z = joystick.Vertical;
    }

    private void FixedUpdate() {
        
        if (battle != null && battle.currentBattleState != BattleState.Play) {
            return;
        }

        Move();     
    }

    /// <summary>
    /// �ړ�
    /// </summary>
    private void Move() {
        if (x != 0 || z != 0) {
            Vector3 dir = new Vector3(x, rb.velocity.y, z).normalized;

            rb.velocity = dir * GameData.instance.currentCharaData.moveSpeed;
            
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
    /// �X�e�[�g�ɉ����āA�_�b�V��������U�����s��
    /// </summary>
    private void Action() {

        Vector3 dashX = transform.right * (x * GameData.instance.currentCharaData.dashPower);
        Vector3 dashZ = transform.forward * (z * GameData.instance.currentCharaData.dashPower);

        rb.AddForce(dashX + dashZ, ForceMode.Impulse);
        //Debug.Log(dashX + dashZ);

        // �X�e�[�g�m�F
        if (currentPlayerState == PlayerState.Ready) {
            currentPlayerState = PlayerState.Attack;
            anim.SetTrigger("Attack");

            //Debug.Log(timingGaugeController.CheckCritial());

            StartCoroutine(timingGaugeController.PausePointer());

            StartCoroutine(ActionInterval(GameData.instance.currentCharaData.attackIntervalTime));
        } else if (currentPlayerState == PlayerState.Wait) {
            currentPlayerState = PlayerState.DashAvoid;
            anim.SetTrigger("Dash");
            // �e�� Scale ���Q�Ƃ���̂ŁA�����������ŕς��
            GameObject dashEffect = Instantiate(EffectManager.instance.dashWindPrefab, dashEffectTran);
            Destroy(dashEffect, 0.5f);
            StartCoroutine(ActionInterval(GameData.instance.currentCharaData.dashAvoidIntervalTime));
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

            // TODO ���݂͓G�ɂ��������Ƃ������N���e�B�J������B�ɂ��ꍇ�ɂ͋�U�莞�Ƀ��Z�b�g����
            //if (timingGaugeController.CheckCritial()) {
            //    attackCount++;
            //} else {
            //    attackCount = 1;
            //}

            attackCount = timingGaugeController.CheckCritial() ? attackCount += 1 : 1;
            Debug.Log(attackCount);

            // �_���[�W�v�Z
            StartCoroutine(enemyController.CalcDamage(GameData.instance.currentCharaData.attackPower, attackCount));

            if (attackCount >= maxAttackCount) {
                attackCount = 1;
            }
        }        
    }

    /// <summary>
    /// HP�v�Z
    /// </summary>
    public void CalcHp(int damage) {�@�@�@// �G�̏������炤

        GameData.instance.hp = Mathf.Clamp(GameData.instance.hp += damage, 0, GameData.instance.maxHp);

        // TODO �G�t�F�N�g


        Debug.Log("amount : " + damage);

        if (GameData.instance.hp <= 0) {
            Debug.Log("Game Over");
        }
    }

    /// <summary>
    /// �U���Q�[�W�̃`���[�W
    /// </summary>
    /// <returns></returns>
    private IEnumerator ChargeAttackGauge() {

        while (currentPlayerState == PlayerState.Wait) {

            chargePoint += GameData.instance.currentCharaData.chargeSpeed;

            // TODO UI �A��

            if (chargePoint >= GameData.instance.currentCharaData.maxChargePoint) {
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
    /// ��ʂ�h�炷���o
    /// </summary>
    public void DamageEffect() {
        cinemachineImpulseSource.GenerateImpulse();
    }

    /// <summary>
    /// PlayerController �̏����ݒ�
    /// </summary>
    /// <param name="battle"></param>
    public void SetUpPlayerController(Battle battle) {
        this.battle = battle;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent(out BulletController bullet)) {
            Debug.Log("��e");

            GameData.instance.hp -= bullet.bulletPower;

            battle.UpdateDisplayHp();

            // Hp ���c���Ă��邩����
            if (GameData.instance.hp <= 0) {
                Debug.Log("Game Over");

                battle.currentBattleState = BattleState.GameUp;
            }

            // TODO �_���[�W�A�j���Đ�

            // TODO ���o

            Destroy(other.gameObject);
        }
    }

    /// <summary>
    /// �J�b�g�C��
    /// </summary>
    public IEnumerator SpecialAttackEffect() {
        imgCutin.gameObject.SetActive(true);

        yield return new WaitForSeconds(2.0f);

        actionCoroutine = null;
        imgCutin.gameObject.SetActive(false);
    }
}
