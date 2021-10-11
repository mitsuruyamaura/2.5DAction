using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using Coffee.UIExtensions;
using DG.Tweening;
public class Stage : MonoBehaviour {

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

    [SerializeField]
    private Button btnPlayerLevel;

    [SerializeField]
    private GameObject maskFieldObj;

    [SerializeField]
    private SelectAbilityPopUp selectAbilityPopUpPrefab;

    [SerializeField]
    private Transform canvasTran;

    private SelectAbilityPopUp selectAbilityPopUp;


    private float sliderAnimeDuration = 0.5f;

    int levelupCount;

    public enum TurnState {
        None,
        Player,
        Enemy,
        Boss
    }

    private TurnState currentTurnState = TurnState.None;

    public TurnState CurrentTurnState
    {
        set => currentTurnState = value;
        get => currentTurnState;
    }

    [SerializeField]
    private GameObject imgLevelUpPrefab;

    [SerializeField]
    private Transform overlayCanvasTran;

    [SerializeField]
    private MoveTimeScaleController moveTimeScaleController;


    void Start() {
        // Stage �̏��ݒ�
        SceneStateManager.instance.stage = this;

        // �X�e�[�W�̃����_���쐬(StageData �쐬��� StageData �ɓo�^����Ă��� StageType ��n��)
        stageGenerator.GenerateStageFromRandomTiles(GameData.instance.currentStageData.stageType);

        // �ʏ�̃V���{���̃����_���쐬���� List �ɒǉ�
        symbolManager.AllClearSymbolsList();
        symbolManager.SymbolsList = stageGenerator.GenerateSymbols(-1);

        // ����V���{���̃����_���쐬���� List �ɒǉ�
        symbolManager.SymbolsList.AddRange(stageGenerator.GenerateSpecialSymbols(GameData.instance.currentStageData.orbTypes));

        // �S�V���{���̐ݒ�
        symbolManager.SetUpAllSymbols();

        // �X�^�~�i�̒l���X�e�[�W���Ƃ̏����l�ɐݒ�(StageData �쐬��)
        GameData.instance.staminaPoint.Value = GameData.instance.currentStageData.initStamina;

        // �X�^�~�i�̒l�̍w�ǊJ�n
        GameData.instance.staminaPoint.Subscribe(_ => UpdateDisplayStaminaPoint());

        // ��U�A�l�������I�[�u�̏����\��
        for (int i = 0; i < imgOrbs.Length; i++) {
            imgOrbs[i].enabled = false;           
        }

        // �I�[�u�̏��쐬
        for (int i = 0; i < GameData.instance.currentStageData.orbTypes.Length; i++) {
            imgOrbs[i].enabled = true;
            imgOrbs[i].sprite = DataBaseManager.instance.orbDataSO.orbDatasList.Find(x => x.orbType == symbolManager.specialSymbols[i].orbType).spriteOrb;
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

        // �v���C���[�̈ړ��̊Ď�(OnEnable �ł���Ă���)
        //StartCoroutine(ObserveEnemyTurnState());

        symbolManager.SwitchEnemyCollider(true);

        // �A�r���e�B�I��p�E�C���h�E�̐���
        CreateSelectAbilityPopUp();

        btnPlayerLevel.onClick.AddListener(OnClickPlayerLevel);
        moveTimeScaleController.SetUpMoveButtonController();

        // �h���b�v����g���W���[�̏�������
        DataBaseManager.instance.CreateDropItemDatasList(GameData.instance.currentStageData.dropTreasureLevel);
    }

    /// <summary>
    /// �G�l�~�[�̃^�[���o�ߊĎ�����
    /// </summary>
    /// <returns></returns>
    public IEnumerator ObserveEnemyTurnState() {
        //while (CurrentTurnState == TurnState.Enemy) {    // ���Ƃ� GameState �ɕς���

        //if (CurrentTurnState == TurnState.Enemy) {
        Debug.Log("�G�̈ړ��@�J�n");
        yield return StartCoroutine(symbolManager.EnemisMove());

        Debug.Log("���ׂĂ̓G�̈ړ� ����");

        // �V���{���̃C�x���g�𔭐�������
        bool isEnemyTriggerEvent = mapMoveController.CallBackEnemySymbolTriggerEvent();

        Debug.Log(isEnemyTriggerEvent);

        // �^�[���̏�Ԃ��m�F
        if (!isEnemyTriggerEvent) {
            CheckTurn();
            CheckTreasureBox();
        }

        if (CurrentTurnState == TurnState.Boss) {

            // �{�X�̏o��
            Debug.Log("Boss �o��");

            // TODO ���o
            PreparateBossEffect();
        }
        //}

        //yield return null;
        //}
    }

    /// <summary>
    /// �X�^�~�i�|�C���g�̕\���X�V
    /// </summary>
    private void UpdateDisplayStaminaPoint() {
        txtStaminaPoint.text = GameData.instance.staminaPoint.ToString();

        if (GameData.instance.staminaPoint.Value <= 0) {
            Debug.Log("�{�X��");

            // �w�ǒ�~
            //GameData.instance.staminaPoint.Dispose();

            //GameData.instance.orbs.Dispose();


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
    /// <param name="waitTime"></param>
    /// <returns></returns>
    public IEnumerator UpdateDisplayHp(float waitTime = 0.0f) {
        txtHp.text = GameData.instance.hp + "/ " + GameData.instance.maxHp;

        yield return new WaitForSeconds(waitTime);

        sliderHp.DOValue((float)GameData.instance.hp / GameData.instance.maxHp, sliderAnimeDuration).SetEase(Ease.Linear);

        Debug.Log("Hp �\���X�V");
    }

    private void OnEnable() {
        Debug.Log("OnEneble");

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


            // ���x���A�b�v���o
            StartCoroutine(GenerateLebelUpEffect());
        }

        // �f�o�b�O  ���x���A�b�v���o
        //GenerateLebelUpEffect();

        //// �o�g������߂����ꍇ
        //if (CurrentTurnState == TurnState.Enemy) {
        //    // �v���C���[�̔Ԃɂ���
        //    CurrentTurnState = TurnState.Player;
        //}

        // �o�g���ŕt�^���ꂽ�f�o�t�̊m�F�ƕt�^
        CheckDebuffConditions();

        // �^�[���̊m�F�ƃv���C���[�̃^�[���ɐ؂�ւ��B�R���f�B�V�����̍X�V
        CheckTurn();

        // �I�[�u���l�����Ă���ꍇ�͊l�����������s
        CheckOrb();

        // �g���W���[�{�b�N�X���G�l�~�[�̉��ɒu�����ꍇ�Ɏg��
        //CheckTreasureBox();

        //if (CurrentTurnState == TurnState.Player) {

        //    // �v���C���[�̈ړ��̊Ď��ĊJ
        //    StartCoroutine(ObserveEnemyTurnState());
        //} else
        if (CurrentTurnState == TurnState.Boss) {

            // �{�X�̏o��
            Debug.Log("Boss �o��");


            // TODO ���o
            PreparateBossEffect();
        }
    }

    /// <summary>
    /// ���x���A�b�v���邩�m�F
    /// </summary>
    public void CheckExpNextLevel() {

        // ���݂̌o���l�Ǝ��̃��x���ɕK�v�Ȍo���l���ׂāA���x�����オ�邩�m�F
        if (GameData.instance.totalExp < DataBaseManager.instance.CalcNextLevelExp(GameData.instance.playerLevel - 1)) {
            // �B���Ă��Ȃ��ꍇ�ɂ͌o���l�ƃQ�[�W�X�V
            UpdateDisplayExp(true);

            // �����I��
            return;
        } else {
            // �B���Ă���ꍇ�ɂ̓��x���A�b�v
            GameData.instance.playerLevel++;
            levelupCount++;

            // �A�r���e�B�|�C���g���Z
            GameData.instance.AddAbilityPoint();

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

    /// <summary>
    /// �^�[���̊m�F�B�v���C���[�̃^�[���ɐ؂�ւ��B�R���f�B�V�����̍X�V
    /// </summary>
    private void CheckTurn() {
        if (GameData.instance.staminaPoint.Value <= 0) {
            CurrentTurnState = TurnState.Boss;
        } else {
            CurrentTurnState = TurnState.Player;

            // �R���f�B�V�����̎c�莞�Ԃ̍X�V(���܂� MapController ���̂Q�ӏ��ł���Ă���̂ŁA�����ň�{������)
            mapMoveController.UpdateConditionsDuration();

            // �ړ��{�^���Ƒ����݃{�^�����������Ԃɂ���
            ActivateInputButtons();

            // �R���f�B�V�����̌��ʂ�K�p
            ApplyEffectConditions();

            mapMoveController.IsMoving = false;

            // �v���C���[�̈ړ��̊Ď��ĊJ
            //StartCoroutine(ObserveEnemyTurnState());
        }
        Debug.Log(CurrentTurnState);
    }

    /// <summary>
    /// �I�[�u�̃C�x���g���o�^����Ă��邩�m�F���āA�o�^����Ă���ꍇ�ɂ͎��s
    /// </summary>
    private void CheckOrb() {
        mapMoveController.CallBackOrbSymbolTriggerEvent();
    }

    /// <summary>
    /// �g���W���[�{�b�N�X�̃C�x���g���o�^����Ă��邩�m�F���āA�o�^����Ă���ꍇ�ɂ͎��s
    /// </summary>
    private void CheckTreasureBox() {
        mapMoveController.CallBackOrbSymbolTriggerEvent();
    }

    /// <summary>
    /// �v���C���[���x���̃{�^�������������ۂ̏���
    /// </summary>
    private void OnClickPlayerLevel() {
        //Debug.Log("Show SelectAbilityPopUp");

        // �t�B�[���h���B��
        SwitchMaskField(false);

        selectAbilityPopUp.ShowPopUp();
    }

    /// <summary>
    /// �}�X�N�Ő؂蔲���ĕ\�����Ă���t�B�[���h�̕\��/��\���̐؂�ւ�
    /// </summary>
    /// <param name="isSwitch"></param>
    public void SwitchMaskField(bool isSwitch) {
        maskFieldObj.SetActive(isSwitch);
    }

    /// <summary>
    /// �A�r���e�B�I��p�E�C���h�E�̐����Ə����ݒ�
    /// </summary>
    private void CreateSelectAbilityPopUp() {
        selectAbilityPopUp = Instantiate(selectAbilityPopUpPrefab, canvasTran);
        selectAbilityPopUp.SetUpSelectAbilityPopUp(this);
    }

    /// <summary>
    /// �A�r���e�B�������̃G�t�F�N�g���v���C���[��ōĐ�
    /// </summary>
    public IEnumerator PlayAbilityPowerUpEffect() {
        GameObject effect_1 = Instantiate(EffectManager.instance.abilityPowerUpPrefab_1, mapMoveController.transform.position, EffectManager.instance.abilityPowerUpPrefab_1.transform.rotation);
        effect_1.transform.position = new Vector3(effect_1.transform.position.x, effect_1.transform.position.y - 0.35f, effect_1.transform.position.z);
        Destroy(effect_1, 1.5f);

        yield return new WaitForSeconds(1.5f);

        GameObject effect_2 = Instantiate(EffectManager.instance.abilityPowerUpPrefab_2, mapMoveController.transform.position, EffectManager.instance.abilityPowerUpPrefab_2.transform.rotation);
        //effect_2.transform.position = new Vector3(effect_2.transform.position.x, effect_2.transform.position.y - 0.5f, effect_2.transform.position.z);
        Destroy(effect_2, 1.0f);
    }

    /// <summary>
    /// �ړ��̓��͂��\�ɂ���
    /// </summary>
    public void ActivateInputButtons() {
        inputButtonManager.SwitchActivateAllButtons(true);
    }

    /// <summary>
    /// �R���f�B�V�����̌��ʂ�K�p
    /// </summary>
    private void ApplyEffectConditions() {
        foreach (PlayerConditionBase condition in mapMoveController.GetConditionsList()) {

            condition.ApplyEffect();

            //// ����(�ړ��s��)�̏ꍇ
            //if (condition.GetConditionType() == ConditionType.Sleep) {
            //    // �����݂����o���Ȃ��悤�ɓ��͐������� 
            //    inputButtonManager.SwitchActivateMoveButtons(false);
            //}
            //// ����(��~�s��)�̏ꍇ
            //if (condition.GetConditionType() == ConditionType.Confusion) {
            //    // �����ݕs��
            //    inputButtonManager.SwitchActivateSteppingButton(false);

            //    // �����_���Ȉړ������o���Ȃ��悤�ɓ��͐������� => Map���Ő���

            //}
            //// �ł̏ꍇ
            //if (condition.GetConditionType() == ConditionType.Poison) {

            //    // �̗͂����炷
            //    condition.ApplyEffect();

            //    // �\���X�V
            //    StartCoroutine(UpdateDisplayHp(1.0f));
            //}
        }

        // ��J�̏ꍇ�͍U���͂�����(����̓R���f�B�V�����ł̌���)

        // �a�C�̏ꍇ�͈ړ����x������(����̓R���f�B�V�����ł̌���)

        // ��(�A�C�e���擾�s��)�̏ꍇ�̓G�l�~�[�̃V���{���݂̂̃G���J�E���g(����� MapController )

    }


    public InputButtonManager GetInputManager() {
        return inputButtonManager;
    }

    /// <summary>
    /// SymbolManager �̏����擾
    /// </summary>
    /// <returns></returns>
    public SymbolManager GetSymbolManager() {
        return symbolManager;
    }


    /// <summary>
    /// �o�g���ŕt�^���ꂽ�f�o�t�̊m�F
    /// </summary>
    private void CheckDebuffConditions() {

        if (GameData.instance.debuffConditionsList.Count == 0) {
            return;
        }

        for (int i = 0; i < GameData.instance.debuffConditionsList.Count; i++) {
            // �f�o�t�̕t�^
            AddDebuff(GameData.instance.debuffConditionsList[i]);
        }

        // �f�o�t���X�g���N���A
        GameData.instance.debuffConditionsList.Clear();
    }

    /// <summary>
    /// �f�o�t�̕t�^
    /// </summary>
    private void AddDebuff(ConditionType conditionType) {

        ConditionData conditionData = DataBaseManager.instance.conditionDataSO.conditionDatasList.Find(x => x.conditionType == conditionType);

        // ���łɓ����R���f�B�V�������t�^����Ă��邩�m�F
        if (mapMoveController.GetConditionsList().Exists(x => x.GetConditionType() == conditionType)) {
            // ���łɕt�^����Ă���ꍇ�́A�������Ԃ��X�V���A���ʂ͏㏑�����ď������I������
            mapMoveController.GetConditionsList().Find(x => x.GetConditionType() == conditionType).ExtentionCondition(conditionData.duration, conditionData.conditionValue);
            return;
        }

        // �t�^����R���f�B�V�������������A���łɍ����̃R���f�B�V�������t�^����Ă���Ƃ��ɂ́A�����̃R���f�B�V�����͖�������(����s�\�ɂȂ邽��)
        if (conditionType == ConditionType.Sleep && mapMoveController.GetConditionsList().Exists(x => x.GetConditionType() == ConditionType.Confusion)) {
            return;
        }

        // �t�^����Ă��Ȃ��R���f�B�V�����̏ꍇ�́A�t�^���鏀������
        PlayerConditionBase playerCondition;

        // Player �ɃR���f�B�V������t�^
        playerCondition = conditionType switch {

            ConditionType.View_Wide => mapMoveController.gameObject.AddComponent<PlayerCondition_View>(),
            ConditionType.View_Narrow => mapMoveController.gameObject.AddComponent<PlayerCondition_View>(),
            ConditionType.Hide_Symbols => mapMoveController.gameObject.AddComponent<PlayerCondition_HideSymbol>(),
            ConditionType.Untouchable => mapMoveController.gameObject.AddComponent<PlayerCondition_Untouchable>(),
            ConditionType.Walk_through => mapMoveController.gameObject.AddComponent<PlayerCondition_WalkThrough>(),
            ConditionType.Sleep => mapMoveController.gameObject.AddComponent<PlayerCondition_Sleep>(),
            ConditionType.Confusion => mapMoveController.gameObject.AddComponent<PlayerCondition_Confusion>(),
            ConditionType.Curse => mapMoveController.gameObject.AddComponent<PlayerCondition_Curse>(),
            ConditionType.Poison => mapMoveController.gameObject.AddComponent<PlayerCondition_Poison>(),
            ConditionType.Disease => mapMoveController.gameObject.AddComponent<PlayerCondition_Disease>(),
            ConditionType.Fatigue => mapMoveController.gameObject.AddComponent<PlayerCondition_Fatigue>(),
            _ => null
        };

        // �����ݒ�����s
        playerCondition.AddCondition(conditionType, conditionData.duration, conditionData.conditionValue, mapMoveController, symbolManager);

        // �R���f�B�V�����p�� List �ɒǉ�
        mapMoveController.AddConditionsList(playerCondition);
    }

    /// <summary>
    /// ���x���A�b�v���o
    /// </summary>
    /// <returns></returns>
    private IEnumerator GenerateLebelUpEffect() {

        Debug.Log("���x���A�b�v���o");

        yield return new WaitForSeconds(1.0f);

        GameObject levelUpLogo = Instantiate(EffectManager.instance.LevelUpLogoPrefab, overlayCanvasTran, false);
        GameObject levelUpEffect = Instantiate(EffectManager.instance.levelUpPrefab, transform.position, EffectManager.instance.levelUpPrefab.transform.rotation);
        Destroy(levelUpEffect, 2.5f);

        Sequence sequence = DOTween.Sequence();
        sequence.Append(levelUpLogo.transform.DOLocalMoveY(325.0f, 1.0f).SetEase(Ease.OutQuart));
        sequence.Append(levelUpLogo.transform.DOShakeScale(0.15f, 0.5f, 5).SetEase(Ease.InQuart));
        sequence.AppendInterval(0.5f).OnComplete(() => { Destroy(levelUpLogo); });
    }

    /// <summary>
    /// �{�X�o���̃G�t�F�N�g�����̏���
    /// </summary>
    /// <returns></returns>
    private void PreparateBossEffect() {

        StartCoroutine(PlayBossEffect());
    }

    /// <summary>
    /// �{�X�o���̃G�t�F�N�g����
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayBossEffect() {

        BossEffect bossEffect = Instantiate(EffectManager.instance.bossEffectPrefab, overlayCanvasTran, false);

        yield return StartCoroutine(bossEffect.PlayEffect());

        // �{�X�o�g���ł��邱�Ƃ��L�^
        GameData.instance.isBossBattled = true;

        // �V�[���J��
        SceneStateManager.instance.PreparateBattleScene();
    }   

    /// <summary>
    /// Overlay �ݒ�� Canvas �̏����擾
    /// </summary>
    /// <returns></returns>
    public Transform GetOverlayCanvasTran() {
        return overlayCanvasTran;
    }
}
