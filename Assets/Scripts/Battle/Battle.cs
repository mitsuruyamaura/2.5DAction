using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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

        yield return new WaitUntil(() => SceneStateManager.instance.GetScene(SceneName.Battle).isLoaded);

        currentBattleState = BattleState.Wait;

        // Hp�\���X�V
        UpdateDisplayHp();

        // �G�̐����� List �ւ̓o�^
        yield return StartCoroutine(enemyGenerator.GenerateEnemies(this));

        // �|�����G�̐��̊Ď�
        StartCoroutine(ObservateBattleState());

        playerController.SetUpPlayerController(this);

        currentBattleState = BattleState.Play;


        Debug.Log("�o�g���J�n");
    }

    /// <summary>
    /// �|�����G�̐��̊Ď�
    /// </summary>
    /// <returns></returns>
    private IEnumerator ObservateBattleState() {

        Debug.Log("�|�����G�̐��̊Ď� : �J�n");

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

        // Battle �I���̗]�C
        yield return new WaitForSeconds(1.0f);

        // �m�[�_���[�W�{�[�i�X�̔���


        // �N���e�B�J���̉񐔂�R���{�������̔���


        // �X�^�~�i�l��
        GameData.instance.staminaPoint.Value += bonusStaminaPoint;


        Debug.Log("�o�g���I��");


        // Stage �֖߂�
        SceneStateManager.instance.PreparateStageScene();
    }

    /// <summary>
    /// �|�����G�� List ����폜���A�|�����G�̐��̉��Z
    /// </summary>
    public void RemoveEnemyFromEnemiesList(EnemyController enemyController) {

        enemiesList.Remove(enemyController);

        destroyEnemyCount++;
    }


    public void AddEnemyFromEnemiesList(EnemyController enemyController) {
        enemiesList.Add(enemyController);
    }
}
