using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using Coffee.UIExtensions;
using DG.Tweening;
using UnityEngine.Tilemaps;

public class Stage : MonoBehaviour
{
    [SerializeField]
    private Text txtStaminaPoint;

    [SerializeField]
    private Image[] imgOrbs;

    [SerializeField]
    private Slider sliderHp;

    [SerializeField]
    private Text txtHp;

    [SerializeField]
    private Text txtExp;

    [SerializeField]
    private Text txtPlayerLevel;

    [SerializeField]
    private ShinyEffectForUGUI shinyEffectImgPlayerLevelFrame;

    [SerializeField]
    private Slider sliderExp;

    [SerializeField]
    private StageGenerator stageGenerator;

    [SerializeField]
    private SymbolManager symbolManager;

    [SerializeField]
    private InputButtonManager inputButtonManager;

    [SerializeField]
    private MapMoveController mapMoveController;

    private float sliderAnimeDuration = 0.5f;

    int levelupCount;

    public enum TurnState {
        None,
        Player,
        Enemy
    }

    private TurnState currentTurnState = TurnState.None;

    public TurnState CurrentTurnState
    {
        set => currentTurnState = value;
        get => currentTurnState;
    }


    void Start()
    {
        // �X�e�[�W�̃����_���쐬
        stageGenerator.GenerateStageFromRandomTiles();

        // �ʏ�̃V���{���̃����_���쐬���� List �ɒǉ�
        symbolManager.AllClearSymbolsList();
        symbolManager.SymbolsList =  stageGenerator.GenerateSymbols(-1);

        // ����V���{���̃����_���쐬���� List �ɒǉ�
        symbolManager.SymbolsList.AddRange(stageGenerator.GenerateSpecialSymbols());

        // �S�V���{���̐ݒ�
        symbolManager.SetUpAllSymbos();

        // �X�^�~�i�̒l�̍w�ǊJ�n
        GameData.instance.staminaPoint.Subscribe(_ => UpdateDisplayStaminaPoint());
        
        // �I�[�u�̏��쐬
        for (int i = 0; i < imgOrbs.Length; i++) {
            GameData.instance.orbs.Add(i, false);
        }
        // �I�[�u�̍w�ǊJ�n
        GameData.instance.orbs.ObserveReplace().Subscribe((DictionaryReplaceEvent<int, bool> x) => UpdateDisplayOrbs(x.Key, x.NewValue));

        //GameData.instance.maxHp = GameData.instance.hp;

        // Hp�\���X�V
        //StartCoroutine(UpdateDisplayHp());

        // �v���C���[���x���ƌo���l�̕\���X�V
        UpdateDisplayPlayerLevel();
        UpdateDisplayExp(true);

        // �v���C���[�̐ݒ�
        mapMoveController.SetUpMapMoveController(this);
        inputButtonManager.SetUpInputButtonManager(mapMoveController);

        CurrentTurnState = TurnState.Player;

        // �v���C���[�̈ړ��̊Ď�
        StartCoroutine(ObserveEnemyTurnState());

        symbolManager.SwitchEnemyCollider(true);
    }

    private IEnumerator ObserveEnemyTurnState() {
        while (CurrentTurnState != TurnState.None) {    // ���Ƃ� GameState �ɕς���

            if (CurrentTurnState == TurnState.Enemy) {

                Debug.Log("�G�̈ړ��@�J�n");
                yield return StartCoroutine(symbolManager.EnemisMove());

                Debug.Log("����");
                CurrentTurnState = TurnState.Player;              
            }

            yield return null;
        }
    }

    /// <summary>
    /// �X�^�~�i�|�C���g�̕\���X�V
    /// </summary>
    private void UpdateDisplayStaminaPoint() {
        txtStaminaPoint.text = GameData.instance.staminaPoint.ToString();

        if (GameData.instance.staminaPoint.Value <= 0) {
            Debug.Log("�{�X��");

            // �w�ǒ�~
            GameData.instance.staminaPoint.Dispose();

            GameData.instance.orbs.Dispose();


            // �ړ��֎~


            // TODO �{�X�Ƃ̃o�g���V�[���֑J��
        }
    }

    /// <summary>
    /// �擾���Ă���I�[�u�̕\���X�V
    /// </summary>
    /// <param name="index"></param>
    /// <param name="isSwich"></param>
    public void UpdateDisplayOrbs(int index, bool isSwich) {

        Debug.Log(index);
        Debug.Log(isSwich);

        imgOrbs[index].color = isSwich ? Color.white : new Color(1, 1, 1, 0.5f);

        // �l�������ꍇ
        if (isSwich) {
            // ���鉉�o���Đ�
            imgOrbs[index].gameObject.GetComponent<ShinyEffectForUGUI>().Play();
        }
    }

    /// <summary>
    /// Hp�\���X�V
    /// </summary>
    public IEnumerator UpdateDisplayHp(float waitTime = 0.0f) {
        txtHp.text = GameData.instance.hp + "/ " + GameData.instance.maxHp;

        yield return new WaitForSeconds(waitTime);

        sliderHp.DOValue((float)GameData.instance.hp / GameData.instance.maxHp, sliderAnimeDuration).SetEase(Ease.Linear);

        Debug.Log("Hp �\���X�V");
    }

    private void OnEnable() {
        // TODO �g�����W�V��������


        // �o�g���O�� Hp ����A�j�����ĕ\�����邽�߂ɑҋ@���Ԃ����
        StartCoroutine(UpdateDisplayHp(1.0f));

        // �o�g����Ƀ��x���A�b�v�������̃J�E���g�̏�����
        levelupCount = 0;
     
        // ���x���A�b�v���邩�m�F
        CheckExpNextLevel();

        // ���x���A�b�v���Ă�����
        if (levelupCount > 0) {

            Debug.Log("���x���A�b�v�̃{�[�i�X����");

            // ���x���A�b�v�̃{�[�i�X

        }

        //// �o�g������߂����ꍇ
        //if (CurrentTurnState == TurnState.Enemy) {
        //    // �v���C���[�̔Ԃɂ���
        //    CurrentTurnState = TurnState.Player;
        //}

        // �v���C���[�̈ړ��̊Ď��ĊJ
        StartCoroutine(ObserveEnemyTurnState());
    }

    /// <summary>
    /// ���x���A�b�v���邩�m�F
    /// </summary>
    public void CheckExpNextLevel() {

        // ���݂̌o���l�Ǝ��̃��x���ɕK�v�Ȍo���l���ׂāA���x�����オ�邩�m�F
        if (GameData.instance.totalExp < DataBaseManager.instance.CalcNextLevelExp(GameData.instance.playerLevel -1)) {
            // �B���Ă��Ȃ��ꍇ�ɂ͌o���l�ƃQ�[�W�X�V
            UpdateDisplayExp(true);

            // �����I��
            return;
        } else {
            // �B���Ă���ꍇ�ɂ̓��x���A�b�v
            GameData.instance.playerLevel++;
            levelupCount++;

            Debug.Log("���x���A�b�v�I ���݂̃��x�� : " + GameData.instance.playerLevel);

            // ���x���A�b�v���o
            shinyEffectImgPlayerLevelFrame.Play();

            // �v���C���[���x���ƌo���l�̕\���X�V
            UpdateDisplayPlayerLevel();
            UpdateDisplayExp(false);

            // ����Ƀ��x�����オ�邩�ċA�������s���Ċm�F
            CheckExpNextLevel();
        }
    }

    /// <summary>
    /// �v���C���[���x���̕\���X�V
    /// </summary>
    private void UpdateDisplayPlayerLevel() {
        // �v���C���[���x���̕\���X�V
        txtPlayerLevel.text = GameData.instance.playerLevel.ToString();
    }

    /// <summary>
    /// �o���l�̕\���X�V
    /// </summary>
    /// <param name="isSliderOn"></param>
    private void UpdateDisplayExp(bool isSliderOn) {
        // ����/�ڕW�o���l�̕\���X�V
        txtExp.text = GameData.instance.totalExp + " / " + DataBaseManager.instance.CalcNextLevelExp(GameData.instance.playerLevel - 1);

        if (isSliderOn) {
            // �Q�[�W�X�V
            sliderExp.DOValue((float)GameData.instance.totalExp / DataBaseManager.instance.CalcNextLevelExp(GameData.instance.playerLevel - 1), 1.0f).SetEase(Ease.Linear);
        }
    }
}
