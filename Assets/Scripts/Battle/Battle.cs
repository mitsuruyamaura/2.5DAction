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

    [SerializeField]  // Debug用
    private int totalComboCount;

    [SerializeField]
    private int currentBattleTotalExp;


    IEnumerator Start()
    {
        SceneStateManager.instance.battle = this;

        if (GameData.instance.isDebugOn) {
            yield return new WaitForSeconds(1.0f);

            // Debug用
            SceneStateManager.instance.PreparateStageScene();
        } else {
            // バトル開始時の処理
            StartCoroutine(OnEnter());
        }
    }

    /// <summary>
    /// Hp表示更新(UniRX への変更可能)
    /// </summary>
    public void UpdateDisplayHp() {
        txtHp.text = GameData.instance.hp + "/ " + GameData.instance.maxHp;

        sliderHp.DOValue((float)GameData.instance.hp / GameData.instance.maxHp, sliderAnimeDuration).SetEase(Ease.Linear);

        Debug.Log("Battle Hp 表示更新");
    }

    /// <summary>
    /// バトル開始時の処理
    /// </summary>
    /// <returns></returns>
    public IEnumerator OnEnter() {

        Debug.Log("バトル開始時の処理");

        // リザルト表示を隠す
        normalResultCancas.gameObject.SetActive(false);

        yield return new WaitUntil(() => SceneStateManager.instance.GetScene(SceneName.Battle).isLoaded);

        currentBattleState = BattleState.Wait;

        // Hp表示更新
        UpdateDisplayHp();

        // ボスバトルかノーマルバトルかの判定
        if (GameData.instance.isBossBattled) {
            // ボスの生成と List への登録
            enemyGenerator.GenerateBoss(this);

        } else {
            // 敵の生成と List への登録
            yield return StartCoroutine(enemyGenerator.GenerateEnemies(this));
        }

        // 敵の総数をクリア目標として設定
        maxEnemyCount = enemiesList.Count;

        // 倒した敵の数の監視
        StartCoroutine(ObservateBattleState());

        playerController.SetUpPlayerController(this);

        // タイミングゲージの設定と移動開始
        timingGaugeController.SetUpTimingGaugeController(this);

        currentBattleState = BattleState.Play;

        Debug.Log("バトル開始");
    }

    /// <summary>
    /// 倒した敵の数の監視
    /// </summary>
    /// <returns></returns>
    private IEnumerator ObservateBattleState() {

        Debug.Log("倒した敵の数の監視 : 開始");

        // 倒した敵の数が maxEnemyCount の値と同じになるまでループ
        while (destroyEnemyCount < maxEnemyCount) {
            yield return null;
        }

        Debug.Log("倒した敵の数の監視 : 終了");

        currentBattleState = BattleState.GameUp;

        // バトル終了時の処理
        StartCoroutine(OnExit());
    }

    /// <summary>
    /// バトル終了時の処理
    /// </summary>
    /// <returns></returns>
    public IEnumerator OnExit() {

        Debug.Log("バトル終了時の処理");

        // TODO 終了時の処理
        // リザルト表示  表示内で new WaitUntil や UniRX で監視して画面のタップを待つ
        normalResultCancas.DisplayResult(currentBattleTotalExp, totalComboCount);

        // Battle 終了の余韻
        yield return new WaitForSeconds(3.5f);


        // ノーダメージボーナスの判定


        // クリティカルの回数やコンボした数の判定

        // 今回のバトルで獲得した EXP を総 EXP に加算
        GameData.instance.totalExp += currentBattleTotalExp;

        if (GameData.instance.isBossBattled) {

            // クリア演出
            yield return StartCoroutine(PlayClearEffect());

            Debug.Log("ボスバトル終了 : ワールドへ戻る");

            SceneStateManager.instance.PrepareteNextScene(SceneName.World);

        } else {

            // スタミナ獲得
            GameData.instance.staminaPoint.Value += bonusStaminaPoint;

            Debug.Log("バトル終了 : ステージへ戻る");

            // Stage へ戻る
            SceneStateManager.instance.PreparateStageScene();
        }
    }

    /// <summary>
    /// 倒した敵を List から削除し、倒した敵の数の加算
    /// </summary>
    public void RemoveEnemyFromEnemiesList(EnemyController enemyController) {

        enemiesList.Remove(enemyController);

        destroyEnemyCount++;
    }

    /// <summary>
    /// エネミーの情報を List に追加
    /// </summary>
    /// <param name="enemyController"></param>
    public void AddEnemyFromEnemiesList(EnemyController enemyController) {
        enemiesList.Add(enemyController);
    }

    /// <summary>
    /// ゲームクリア時の演出
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayClearEffect() {

        // クリアのロゴ表示
        clearLogoEffect.transform.parent.gameObject.SetActive(true);

        yield return null;

        // ロゴが光るエフェクト再生
        clearLogoEffect.Play();

        // クリアのエフェクトを生成
        for (int i = 0; i < Random.Range(3, 6); i++) {

            GameObject clearEffect = Instantiate(EffectManager.instance.clearEffectPrefab, clearEffectPos);

            // 位置をランダム化
            clearEffect.transform.localPosition = new Vector3(
                clearEffect.transform.localPosition.x + Random.Range(-10.0f, 10.0f),
                clearEffect.transform.localPosition.y + Random.Range(-5.0f, 5.0f),
                clearEffect.transform.localPosition.z);

            Destroy(clearEffect, 1.25f);
            yield return new WaitForSeconds(1.0f);
        }
    }

    /// <summary>
    /// クリティカル(コンボした)総数のカウントアップ
    /// </summary>
    public void AddTotalBattleCount() {
        totalComboCount++;
    }

    /// <summary>
    /// バトル内で獲得した EXP の総計
    /// </summary>
    /// <param name="exp"></param>
    public void AddCurrentBattleTotalExp(int exp) {
        currentBattleTotalExp += exp;
    }
}
