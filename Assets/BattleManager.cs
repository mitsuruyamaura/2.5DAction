using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using UniRx;
using System;
using Cinemachine;

public enum EntityType {
    Player,
    Enemy
}

public class BattleManager : MonoBehaviour {

    public static BattleManager instance;

    public ReactiveProperty<int> PlayerHP = new ReactiveProperty<int>(100);
    public ReactiveProperty<int> EnemyHP = new ReactiveProperty<int>(100);
    public float battleDuration = 5.0f; // バトルの制限時間（秒）

    private CancellationTokenSource cts;

    // プレイヤー用のアイテムリスト
    public List<BackPackInItem> playerBackPackItemList = new List<BackPackInItem>();

    // 敵用のアイテムリスト
    public List<BackPackInItem> enemyBackPackItemList = new List<BackPackInItem>();

    [Header("デバッグ"), Space(2)]
    [SerializeField] private List<int> playerItemNoList = new();
    [SerializeField] private List<int> enemyItemNoList = new();

    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private SpriteMask mask;
    //[SerializeField] 
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;
    //[SerializeField]
    private GameObject battleEffectSetObj;

    [SerializeField] private float amplitude = 1.0f, frequency = 1.0f;

    public Subject<Unit> OnBattleStart = new();
    private Channel<bool> channel;


    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);            
        } else {
            Destroy(gameObject);
        }

        if (virtualCamera != null) {
            virtualCameraNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            battleEffectSetObj = virtualCamera.transform.GetChild(2).gameObject;
        }
    }

    private void OnEnable() {
        PlayerHP.Subscribe(hp => 
        {
            // UI 更新

            CheckEndCondition();
        });

        EnemyHP.Subscribe(hp => CheckEndCondition());
    }

    void Start() {
        SetUp();
    }

    public void SetUp() {
        cts = new CancellationTokenSource();

        // デバッグ用
        if (playerItemNoList.Count > 0) {
            for (int i = 0; i < playerItemNoList.Count; i++) {
                ItemData itemData = DataBaseManager.instance.GetItemData(playerItemNoList[i]);
                playerBackPackItemList[i].itemData = itemData;
            }
        }

        // バトルのたびにセットではなく、アイテム取得時にセットし、OnNext でスタートさせる
        // プレイヤー用のアイテムを処理
        playerBackPackItemList.ForEach(item => OnBattleStart.Subscribe(_ => item.Hoge(item.itemData, cts.Token, EntityType.Player).Forget()));

        // 敵用のアイテムを処理
        //enemyBackPackItemList.ForEach(item => item.Hoge(item.itemData, cts.Token, EntityType.Enemy).Forget());
    }

    public async UniTask<bool> StartBattle() {
        // 新しいトークンソースを生成
        // 初期化しないと Cancel 状態のままのトークンを利用してしまうため、BackPackItem の処理がスキップされてしまう
        cts = new CancellationTokenSource();

        OnBattleStart.OnNext(Unit.Default);
        //Debug.Log("StartBattle");

        // バトルエフェクト再生
        battleEffectSetObj.SetActive(true);

        StartBattleShake(amplitude, frequency);

        // バトル制限時間を管理するタスクを開始
        ManageBattleDuration(cts.Token).Forget();

        channel = Channel.CreateSingleConsumerUnbounded<bool>();
        bool isBattleResult = await channel.Reader.ReadAsync();

        channel.Writer.TryComplete();
        return isBattleResult;
    }

    public void StopBattle() {
        if (cts != null) {
            cts.Cancel(); // すべての Hoge メソッドの実行をキャンセルする
            cts.Dispose(); // 古いトークンソースを破棄
            cts = null; // 参照をクリア
        }

        // カメラとマスクの制御　元に戻す
        StopBattleShake();

        // バトルエフェクト非表示
        battleEffectSetObj.SetActive(false);

        channel.Writer.TryWrite(true);
    }

    private async UniTask ManageBattleDuration(CancellationToken token) {
        try {
            await UniTask.Delay((int)(battleDuration * 1000), cancellationToken: token);
            StopBattle();
        }
        catch (OperationCanceledException) {
            // バトルが手動でキャンセルされた時もここに来る
        }
    }


    public void UpdatePlayerHp(int damage) {
        // と演出(フロート表示もふくめ、ここは相手が行う)
        PlayerHP.Value = Mathf.Clamp(PlayerHP.Value - damage, 0, GameData.instance.maxHp);
    }

    public void UpdateEnemyHp(int damage) {
        EnemyHP.Value = Mathf.Clamp(EnemyHP.Value - damage, 0, 1000);
    }

    public bool CheckEndCondition() {
        if (PlayerHP.Value <= 0 || EnemyHP.Value <= 0) {
            StopBattle();
            return true;
        }
        return false;
    }

    /// <summary>
    /// ダメージを与えるタイミングで揺らすか、ずっと揺らすか検討する
    /// カエルの為は、ダメージのタイミングのみ
    /// </summary>
    /// <param name="amplitude"></param>
    /// <param name="frequency"></param>
    public void StartBattleShake(float amplitude, float frequency) {
        if (virtualCameraNoise != null) {
            virtualCameraNoise.m_AmplitudeGain = amplitude;
            virtualCameraNoise.m_FrequencyGain = frequency;
        }
    }

    public void StopBattleShake() {
        if (virtualCameraNoise != null) {
            virtualCameraNoise.m_AmplitudeGain = 0f;
            virtualCameraNoise.m_FrequencyGain = 0f;
        }
    }
}