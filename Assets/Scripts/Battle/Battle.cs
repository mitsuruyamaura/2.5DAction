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

        yield return new WaitUntil(() => SceneStateManager.instance.GetScene(SceneName.Battle).isLoaded);

        currentBattleState = BattleState.Wait;

        // Hp表示更新
        UpdateDisplayHp();

        // 敵の生成と List への登録
        yield return StartCoroutine(enemyGenerator.GenerateEnemies(this));

        // 倒した敵の数の監視
        StartCoroutine(ObservateBattleState());

        playerController.SetUpPlayerController(this);

        currentBattleState = BattleState.Play;


        Debug.Log("バトル開始");
    }

    /// <summary>
    /// 倒した敵の数の監視
    /// </summary>
    /// <returns></returns>
    private IEnumerator ObservateBattleState() {

        Debug.Log("倒した敵の数の監視 : 開始");

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

        // Battle 終了の余韻
        yield return new WaitForSeconds(1.0f);

        // ノーダメージボーナスの判定


        // クリティカルの回数やコンボした数の判定


        // スタミナ獲得
        GameData.instance.staminaPoint.Value += bonusStaminaPoint;


        Debug.Log("バトル終了");


        // Stage へ戻る
        SceneStateManager.instance.PreparateStageScene();
    }

    /// <summary>
    /// 倒した敵を List から削除し、倒した敵の数の加算
    /// </summary>
    public void RemoveEnemyFromEnemiesList(EnemyController enemyController) {

        enemiesList.Remove(enemyController);

        destroyEnemyCount++;
    }


    public void AddEnemyFromEnemiesList(EnemyController enemyController) {
        enemiesList.Add(enemyController);
    }
}
