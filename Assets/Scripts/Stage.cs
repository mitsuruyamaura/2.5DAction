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


    void Start()
    {
        // ステージのランダム作成
        stageGenerator.GenerateStageFromRandomTiles();

        // 通常のシンボルのランダム作成して List に追加
        symbolManager.AllClearSymbolsList();
        symbolManager.SymbolsList =  stageGenerator.GenerateSymbols(-1);

        // 特殊シンボルのランダム作成して List に追加
        symbolManager.SymbolsList.AddRange(stageGenerator.GenerateSpecialSymbols());

        // 全シンボルの設定
        symbolManager.SetUpAllSymbos();

        // スタミナの値の購読開始
        GameData.instance.staminaPoint.Subscribe(_ => UpdateDisplayStaminaPoint());
        
        // オーブの情報作成
        for (int i = 0; i < imgOrbs.Length; i++) {
            GameData.instance.orbs.Add(i, false);
        }
        // オーブの購読開始
        GameData.instance.orbs.ObserveReplace().Subscribe((DictionaryReplaceEvent<int, bool> x) => UpdateDisplayOrbs(x.Key, x.NewValue));

        //GameData.instance.maxHp = GameData.instance.hp;

        // Hp表示更新
        //StartCoroutine(UpdateDisplayHp());

        // プレイヤーレベルと経験値の表示更新
        UpdateDisplayPlayerLevel();
        UpdateDisplayExp(true);

        // プレイヤーの設定
        mapMoveController.SetUpMapMoveController(this);
        inputButtonManager.SetUpInputButtonManager(mapMoveController);

        CurrentTurnState = TurnState.Player;

        // プレイヤーの移動の監視
        StartCoroutine(ObserveEnemyTurnState());

        symbolManager.SwitchEnemyCollider(true);

        // アビリティ選択用ウインドウの生成
        CreateSelectAbilityPopUp();

        btnPlayerLevel.onClick.AddListener(OnClickPlayerLevel);
    }

    /// <summary>
    /// エネミーのターン経過監視処理
    /// </summary>
    /// <returns></returns>
    private IEnumerator ObserveEnemyTurnState() {
        while (CurrentTurnState != TurnState.None) {    // あとで GameState に変える

            if (CurrentTurnState == TurnState.Enemy) {
                Debug.Log("敵の移動　開始");
                yield return StartCoroutine(symbolManager.EnemisMove());

                Debug.Log("すべての敵の移動 完了");

                // ターンの状態を確認
                CheckTurn();

                if (CurrentTurnState == TurnState.Boss) {

                    // ボスの出現
                    Debug.Log("Boss 出現");


                    // TODO 演出


                    // TODO シーン遷移

                }
            }

            yield return null;
        }
    }

    /// <summary>
    /// スタミナポイントの表示更新
    /// </summary>
    private void UpdateDisplayStaminaPoint() {
        txtStaminaPoint.text = GameData.instance.staminaPoint.ToString();

        if (GameData.instance.staminaPoint.Value <= 0) {
            Debug.Log("ボス戦");

            // 購読停止
            //GameData.instance.staminaPoint.Dispose();

            //GameData.instance.orbs.Dispose();


            // 移動禁止


            // TODO ボスとのバトルシーンへ遷移
        }
    }

    /// <summary>
    /// 取得しているオーブの表示更新
    /// </summary>
    /// <param name="index"></param>
    /// <param name="isSwich"></param>
    public void UpdateDisplayOrbs(int index, bool isSwich) {

        Debug.Log(index);
        Debug.Log(isSwich);

        imgOrbs[index].color = isSwich ? Color.white : new Color(1, 1, 1, 0.5f);

        // 獲得した場合
        if (isSwich) {
            // 光る演出を再生
            imgOrbs[index].gameObject.GetComponent<ShinyEffectForUGUI>().Play();
        }
    }

    /// <summary>
    /// Hp表示更新
    /// </summary>
    public IEnumerator UpdateDisplayHp(float waitTime = 0.0f) {
        txtHp.text = GameData.instance.hp + "/ " + GameData.instance.maxHp;

        yield return new WaitForSeconds(waitTime);

        sliderHp.DOValue((float)GameData.instance.hp / GameData.instance.maxHp, sliderAnimeDuration).SetEase(Ease.Linear);

        Debug.Log("Hp 表示更新");
    }

    private void OnEnable() {
        // TODO トランジション処理


        // バトル前の Hp からアニメして表示するために待機時間を作る
        StartCoroutine(UpdateDisplayHp(1.0f));

        // バトル後にレベルアップした時のカウントの初期化
        levelupCount = 0;
     
        // レベルアップするか確認
        CheckExpNextLevel();

        // レベルアップしていたら
        if (levelupCount > 0) {

            Debug.Log("レベルアップのボーナス発生");

            // レベルアップのボーナス

        }

        //// バトルから戻った場合
        //if (CurrentTurnState == TurnState.Enemy) {
        //    // プレイヤーの番にする
        //    CurrentTurnState = TurnState.Player;
        //}

        // ターンの確認
        CheckTurn();

        // オーブを獲得している場合は獲得処理を実行
        CheckOrb();

        if (CurrentTurnState == TurnState.Player) {

            // プレイヤーの移動の監視再開
            StartCoroutine(ObserveEnemyTurnState());
        } else if (CurrentTurnState == TurnState.Boss){

            // ボスの出現
            Debug.Log("Boss 出現");


            // TODO 演出


            // TODO シーン遷移

        }
    }

    /// <summary>
    /// レベルアップするか確認
    /// </summary>
    public void CheckExpNextLevel() {

        // 現在の経験値と次のレベルに必要な経験値を比べて、レベルが上がるか確認
        if (GameData.instance.totalExp < DataBaseManager.instance.CalcNextLevelExp(GameData.instance.playerLevel -1)) {
            // 達していない場合には経験値とゲージ更新
            UpdateDisplayExp(true);

            // 処理終了
            return;
        } else {
            // 達している場合にはレベルアップ
            GameData.instance.playerLevel++;
            levelupCount++;

            Debug.Log("レベルアップ！ 現在のレベル : " + GameData.instance.playerLevel);

            // レベルアップ演出
            shinyEffectImgPlayerLevelFrame.Play();

            // プレイヤーレベルと経験値の表示更新
            UpdateDisplayPlayerLevel();
            UpdateDisplayExp(false);

            // さらにレベルが上がるか再帰処理を行って確認
            CheckExpNextLevel();
        }
    }

    /// <summary>
    /// プレイヤーレベルの表示更新
    /// </summary>
    private void UpdateDisplayPlayerLevel() {
        // プレイヤーレベルの表示更新
        txtPlayerLevel.text = GameData.instance.playerLevel.ToString();
    }

    /// <summary>
    /// 経験値の表示更新
    /// </summary>
    /// <param name="isSliderOn"></param>
    private void UpdateDisplayExp(bool isSliderOn) {
        // 現在/目標経験値の表示更新
        txtExp.text = GameData.instance.totalExp + " / " + DataBaseManager.instance.CalcNextLevelExp(GameData.instance.playerLevel - 1);

        if (isSliderOn) {
            // ゲージ更新
            sliderExp.DOValue((float)GameData.instance.totalExp / DataBaseManager.instance.CalcNextLevelExp(GameData.instance.playerLevel - 1), 1.0f).SetEase(Ease.Linear);
        }
    }

    /// <summary>
    /// ターンの確認
    /// </summary>
    private void CheckTurn() {
        if (GameData.instance.staminaPoint.Value <= 0) {
            CurrentTurnState = TurnState.Boss;
        } else {
            // シンボルのイベントを発生させる
            mapMoveController.CallBackEnemySymbolTriggerEvent();

            CurrentTurnState = TurnState.Player;

            ActivateInputButtons();
        }
    }

    /// <summary>
    /// オーブのイベントが登録されているか確認して、登録されている場合には実行
    /// </summary>
    private void CheckOrb() {
        mapMoveController.CallBackOrbSymbolTriggerEvent();
    }

    /// <summary>
    /// プレイヤーレベルのボタンを押下した際の処理
    /// </summary>
    private void OnClickPlayerLevel() {
        //Debug.Log("Show SelectAbilityPopUp");

        // フィールドを隠す
        SwitchMaskField(false);

        selectAbilityPopUp.ShowPopUp();
    }

    /// <summary>
    /// マスクで切り抜いて表示しているフィールドの表示/非表示の切り替え
    /// </summary>
    /// <param name="isSwitch"></param>
    public void SwitchMaskField(bool isSwitch) {
        maskFieldObj.SetActive(isSwitch);
    }

    /// <summary>
    /// アビリティ選択用ウインドウの生成と初期設定
    /// </summary>
    private void CreateSelectAbilityPopUp() {
        selectAbilityPopUp = Instantiate(selectAbilityPopUpPrefab,canvasTran);
        selectAbilityPopUp.SetUpSelectAbilityPopUp(this);
    }

    /// <summary>
    /// アビリティ強化時のエフェクトをプレイヤー上で再生
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
    /// 移動の入力を可能にする
    /// </summary>
    public void ActivateInputButtons() {
        inputButtonManager.SwitchActivateAllButtons(true);
    }
}
