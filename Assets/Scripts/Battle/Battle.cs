using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Coffee.UIExtensions;

public enum BattleState {
    Wait,
    Play,
    GameUp
}


public class Battle : MonoBehaviour
{
    [SerializeField]
    private Slider sliderHp;

    [SerializeField]
    private Text txtHp;

    public BattleState currentBattleState;

    public int maxEnemyCount;

    public int destroyEnemyCount;

    public int bonusStaminaPoint;

    private float sliderAnimeDuration = 0.5f;

    [SerializeField]
    private EnemyGenerator enemyGenerator;

    [SerializeField]
    private List<EnemyController> enemiesList = new List<EnemyController>();

    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private Transform clearEffectPos;

    [SerializeField]
    private ShinyEffectForUGUI clearLogoEffect;

    [SerializeField]
    private NormalResultCancas normalResultCancas;

    [SerializeField]
    private TimingGaugeController timingGaugeController;

    [SerializeField]  // Debug�p
    private int totalComboCount;

    [SerializeField]
    private int currentBattleTotalExp;


    IEnumerator Start()
    {
        SceneStateManager.instance.battle = this;

        if (GameData.instance.isDebugOn) {
            yield return new WaitForSeconds(1.0f);

            // Debug�p
            SceneStateManager.instance.PreparateStageScene();
        } else {
            // �o�g���J�n���̏���
            StartCoroutine(OnEnter());
        }
    }

    /// <summary>
    /// Hp�\���X�V(UniRX �ւ̕ύX�\)
    /// </summary>
    public void UpdateDisplayHp() {
        txtHp.text = GameData.instance.hp + "/ " + GameData.instance.maxHp;

        sliderHp.DOValue((float)GameData.instance.hp / GameData.instance.maxHp, sliderAnimeDuration).SetEase(Ease.Linear);

        Debug.Log("Battle Hp �\���X�V");
    }

    /// <summary>
    /// �o�g���J�n���̏���
    /// </summary>
    /// <returns></returns>
    public IEnumerator OnEnter() {

        Debug.Log("�o�g���J�n���̏���");

        // ���U���g�\�����B��
        normalResultCancas.gameObject.SetActive(false);

        yield return new WaitUntil(() => SceneStateManager.instance.GetScene(SceneName.Battle).isLoaded);

        currentBattleState = BattleState.Wait;

        // Hp�\���X�V
        UpdateDisplayHp();

        // �{�X�o�g�����m�[�}���o�g�����̔���
        if (GameData.instance.isBossBattled) {
            // �{�X�̐����� List �ւ̓o�^
            enemyGenerator.GenerateBoss(this);

        } else {
            // �G�̐����� List �ւ̓o�^
            yield return StartCoroutine(enemyGenerator.GenerateEnemies(this));
        }

        // �G�̑������N���A�ڕW�Ƃ��Đݒ�
        maxEnemyCount = enemiesList.Count;

        // �|�����G�̐��̊Ď�
        StartCoroutine(ObservateBattleState());

        playerController.SetUpPlayerController(this);

        // �^�C�~���O�Q�[�W�̐ݒ�ƈړ��J�n
        timingGaugeController.SetUpTimingGaugeController(this);

        currentBattleState = BattleState.Play;

        Debug.Log("�o�g���J�n");
    }

    /// <summary>
    /// �|�����G�̐��̊Ď�
    /// </summary>
    /// <returns></returns>
    private IEnumerator ObservateBattleState() {

        Debug.Log("�|�����G�̐��̊Ď� : �J�n");

        // �|�����G�̐��� maxEnemyCount �̒l�Ɠ����ɂȂ�܂Ń��[�v
        while (destroyEnemyCount < maxEnemyCount) {
            yield return null;
        }

        Debug.Log("�|�����G�̐��̊Ď� : �I��");

        currentBattleState = BattleState.GameUp;

        // �o�g���I�����̏���
        StartCoroutine(OnExit());
    }

    /// <summary>
    /// �o�g���I�����̏���
    /// </summary>
    /// <returns></returns>
    public IEnumerator OnExit() {

        Debug.Log("�o�g���I�����̏���");

        // TODO �I�����̏���
        // ���U���g�\��  �\������ new WaitUntil �� UniRX �ŊĎ����ĉ�ʂ̃^�b�v��҂�
        normalResultCancas.DisplayResult(currentBattleTotalExp, totalComboCount);

        // Battle �I���̗]�C
        yield return new WaitForSeconds(3.5f);


        // �m�[�_���[�W�{�[�i�X�̔���


        // �N���e�B�J���̉񐔂�R���{�������̔���

        // ����̃o�g���Ŋl������ EXP �� EXP �ɉ��Z
        GameData.instance.totalExp += currentBattleTotalExp;

        if (GameData.instance.isBossBattled) {

            // �N���A���o
            yield return StartCoroutine(PlayClearEffect());

            Debug.Log("�{�X�o�g���I�� : ���[���h�֖߂�");

            SceneStateManager.instance.PrepareteNextScene(SceneName.World);

        } else {

            // �X�^�~�i�l��
            GameData.instance.staminaPoint.Value += bonusStaminaPoint;

            Debug.Log("�o�g���I�� : �X�e�[�W�֖߂�");

            // Stage �֖߂�
            SceneStateManager.instance.PreparateStageScene();
        }
    }

    /// <summary>
    /// �|�����G�� List ����폜���A�|�����G�̐��̉��Z
    /// </summary>
    public void RemoveEnemyFromEnemiesList(EnemyController enemyController) {

        enemiesList.Remove(enemyController);

        destroyEnemyCount++;
    }

    /// <summary>
    /// �G�l�~�[�̏��� List �ɒǉ�
    /// </summary>
    /// <param name="enemyController"></param>
    public void AddEnemyFromEnemiesList(EnemyController enemyController) {
        enemiesList.Add(enemyController);
    }

    /// <summary>
    /// �Q�[���N���A���̉��o
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayClearEffect() {

        // �N���A�̃��S�\��
        clearLogoEffect.transform.parent.gameObject.SetActive(true);

        yield return null;

        // ���S������G�t�F�N�g�Đ�
        clearLogoEffect.Play();

        // �N���A�̃G�t�F�N�g�𐶐�
        for (int i = 0; i < Random.Range(3, 6); i++) {

            GameObject clearEffect = Instantiate(EffectManager.instance.clearEffectPrefab, clearEffectPos);

            // �ʒu�������_����
            clearEffect.transform.localPosition = new Vector3(
                clearEffect.transform.localPosition.x + Random.Range(-10.0f, 10.0f),
                clearEffect.transform.localPosition.y + Random.Range(-5.0f, 5.0f),
                clearEffect.transform.localPosition.z);

            Destroy(clearEffect, 1.25f);
            yield return new WaitForSeconds(1.0f);
        }
    }

    /// <summary>
    /// �N���e�B�J��(�R���{����)�����̃J�E���g�A�b�v
    /// </summary>
    public void AddTotalBattleCount() {
        totalComboCount++;
    }

    /// <summary>
    /// �o�g�����Ŋl������ EXP �̑��v
    /// </summary>
    /// <param name="exp"></param>
    public void AddCurrentBattleTotalExp(int exp) {
        currentBattleTotalExp += exp;
    }
}
