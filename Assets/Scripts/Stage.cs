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
    }

    private IEnumerator ObserveEnemyTurnState() {
        while (CurrentTurnState != TurnState.None) {    // あとで GameState に変える

            if (CurrentTurnState == TurnState.Enemy) {

                Debug.Log("敵の移動　開始");
                yield return StartCoroutine(symbolManager.EnemisMove());

                Debug.Log("完了");
                CurrentTurnState = TurnState.Player;              
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
            GameData.instance.staminaPoint.Dispose();

            GameData.instance.orbs.Dispose();


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

        // プレイヤーの移動の監視再開
        StartCoroutine(ObserveEnemyTurnState());
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
}
