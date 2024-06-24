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
    public float battleDuration = 5.0f; // �o�g���̐������ԁi�b�j

    private CancellationTokenSource cts;

    // �v���C���[�p�̃A�C�e�����X�g
    public List<BackPackInItem> playerBackPackItemList = new List<BackPackInItem>();

    // �G�p�̃A�C�e�����X�g
    public List<BackPackInItem> enemyBackPackItemList = new List<BackPackInItem>();

    [Header("�f�o�b�O"), Space(2)]
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
            // UI �X�V

            CheckEndCondition();
        });

        EnemyHP.Subscribe(hp => CheckEndCondition());
    }

    void Start() {
        SetUp();
    }

    public void SetUp() {
        cts = new CancellationTokenSource();

        // �f�o�b�O�p
        if (playerItemNoList.Count > 0) {
            for (int i = 0; i < playerItemNoList.Count; i++) {
                ItemData itemData = DataBaseManager.instance.GetItemData(playerItemNoList[i]);
                playerBackPackItemList[i].itemData = itemData;
            }
        }

        // �o�g���̂��тɃZ�b�g�ł͂Ȃ��A�A�C�e���擾���ɃZ�b�g���AOnNext �ŃX�^�[�g������
        // �v���C���[�p�̃A�C�e��������
        playerBackPackItemList.ForEach(item => OnBattleStart.Subscribe(_ => item.Hoge(item.itemData, cts.Token, EntityType.Player).Forget()));

        // �G�p�̃A�C�e��������
        //enemyBackPackItemList.ForEach(item => item.Hoge(item.itemData, cts.Token, EntityType.Enemy).Forget());
    }

    public async UniTask<bool> StartBattle() {
        // �V�����g�[�N���\�[�X�𐶐�
        // ���������Ȃ��� Cancel ��Ԃ̂܂܂̃g�[�N���𗘗p���Ă��܂����߁ABackPackItem �̏������X�L�b�v����Ă��܂�
        cts = new CancellationTokenSource();

        OnBattleStart.OnNext(Unit.Default);
        //Debug.Log("StartBattle");

        // �o�g���G�t�F�N�g�Đ�
        battleEffectSetObj.SetActive(true);

        StartBattleShake(amplitude, frequency);

        // �o�g���������Ԃ��Ǘ�����^�X�N���J�n
        ManageBattleDuration(cts.Token).Forget();

        channel = Channel.CreateSingleConsumerUnbounded<bool>();
        bool isBattleResult = await channel.Reader.ReadAsync();

        channel.Writer.TryComplete();
        return isBattleResult;
    }

    public void StopBattle() {
        if (cts != null) {
            cts.Cancel(); // ���ׂĂ� Hoge ���\�b�h�̎��s���L�����Z������
            cts.Dispose(); // �Â��g�[�N���\�[�X��j��
            cts = null; // �Q�Ƃ��N���A
        }

        // �J�����ƃ}�X�N�̐���@���ɖ߂�
        StopBattleShake();

        // �o�g���G�t�F�N�g��\��
        battleEffectSetObj.SetActive(false);

        channel.Writer.TryWrite(true);
    }

    private async UniTask ManageBattleDuration(CancellationToken token) {
        try {
            await UniTask.Delay((int)(battleDuration * 1000), cancellationToken: token);
            StopBattle();
        }
        catch (OperationCanceledException) {
            // �o�g�����蓮�ŃL�����Z�����ꂽ���������ɗ���
        }
    }


    public void UpdatePlayerHp(int damage) {
        // �Ɖ��o(�t���[�g�\�����ӂ��߁A�����͑��肪�s��)
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
    /// �_���[�W��^����^�C�~���O�ŗh�炷���A�����Ɨh�炷����������
    /// �J�G���ׂ̈́A�_���[�W�̃^�C�~���O�̂�
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