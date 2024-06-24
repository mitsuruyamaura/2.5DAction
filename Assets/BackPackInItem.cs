using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using System;
using UniRx;

public class BackPackInItem : MonoBehaviour
{
    [SerializeField]
    private Image imgIconGauge;

    public ItemData itemData;

    public float currentCoolTime;
    public float currentAccuracy;
    public int currentMinDamage;
    public int currentMaxDamage;
    public int currentMinAttackCount;
    public int currentMaxAttackCount;

    private Tweener tweener;
    private Subject<Unit> onCancel = new Subject<Unit>();
    public IObservable<Unit> OnCancel => onCancel;

    [SerializeField] private Button btnStop;
    private IDisposable stopButtonSubscription;


    public async UniTask Hoge(ItemData itemData, CancellationToken token, EntityType myEntityType) {
        //Debug.Log("Hoge");
        //// ������ token ���g���ăL�����Z���\�ɂ��邽�߁ACancellationTokenSource �𐶐�
        //CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(token);

        //// btnStop.OnClickAsObservable ���w�ǂ��A�{�^���������� cts �𗘗p���� UniTask ���L�����Z��
        //stopButtonSubscription = btnStop.OnClickAsObservable()
        //    .ThrottleFirst(System.TimeSpan.FromSeconds(1.0f))
        //    .Subscribe(_ => cts.Cancel());

        // �f�o�b�O�p
        currentCoolTime = UnityEngine.Random.Range(1.0f, 2.0f);
        currentAccuracy = UnityEngine.Random.Range(50.0f, 100.0f);
        currentMinDamage = GetRandomValue(1, 4);
        currentMaxDamage = GetRandomValue(1, 4);
        currentMinAttackCount = 1;
        currentMaxAttackCount = GetRandomValue(1, 4);
        //Debug.Log(currentCoolTime);

        // ����̍s����
        int count = 0;

        try {
            // �o�g�����L�����Z������Ă��Ȃ��ԁA���邢�͐������Ԃ�������
            while (!token.IsCancellationRequested) {

                // �Ⴆ�΁A����̍s���񐔂̂Ƃ����������ς��A�Ȃ� (count == 0 �Ȃ� fillAmount = 0.5f �X�^�[�g�ȂǁB)

                imgIconGauge.fillAmount = 1.0f;

                // �o�t�A�f�o�t��K�p���Č��ݒl���Z�o

                // 
                tweener = null;
                tweener = imgIconGauge.DOFillAmount(0f, currentCoolTime).SetEase(Ease.Linear).SetLink(gameObject);
                
                // �L�����Z�����̏�����o�^(���̃X�R�[�v���ŃL�����Z�����ꂽ�ꍇ�ɁA���̏��������s�����)
                using (token.Register(() => {
                    tweener?.Kill();
                })) {
                    // �U���܂ł̑ҋ@����
                    // �����_�ȉ����܂߂����x��ۂ��߂ɁA�K�؂ȃX�P�[�����O���s��
                    int attackInterval = Mathf.CeilToInt(currentCoolTime * 1000); // 1000 �ŃX�P�[�����Đ؂�グ
                    //Debug.Log(attackInterval);
                    await UniTask.Delay(attackInterval, cancellationToken: token);
                }

                // �U���񐔕������A�s��������
                int currentAttackCount = GetRandomValue(currentMinAttackCount, currentMaxAttackCount);
                for (int i = 0; i < currentAttackCount; i++) {
                    AutoAttack(myEntityType);
                    // for ���Ȃ̂Ńt���[���ׂ�����B�������Ȃ��ƁA�����ł��ׂď������悤�Ƃ��ď������ꎞ�~�܂�
                    await UniTask.Yield(cancellationToken: token);
                }
                count++;
            }
        }
        catch (OperationCanceledException) {
            // while ���𔲂��ăL�����Z�����ꂽ�ꍇ�̏���
            //tweener?.Kill();
            //tweener = null;

            // �L�����Z�����ꂽ���Ƃ�ʒm
            onCancel.OnNext(Unit.Default);
        }
        imgIconGauge.fillAmount = 1.0f;
    }

    private void AutoAttack(EntityType myEntityType) {
        float randomValue = UnityEngine.Random.Range(0, 100.0f);

        // ��������
        if (randomValue <= currentAccuracy) {
            // ���������ꍇ�̏���
            Debug.Log("Hit!");

            // �U���͐ݒ�
            int damage = GetRandomValue(currentMinDamage, currentMaxDamage);

            // �_���[�W����
            if(myEntityType == EntityType.Player) {
                BattleManager.instance.UpdateEnemyHp(damage);
            } else {
                BattleManager.instance.UpdatePlayerHp(damage);
            }
        } else {
            // ���s�����ꍇ�̏���
            Debug.Log("Miss!");
        }
    }

    private int GetRandomValue(int min, int max) {
        // max��2�ȏ�̏ꍇ�Ƀ����_���Ȓl��Ԃ�
        if (max >= 2) {
            return UnityEngine.Random.Range(min, max);
        }
        // max��2�����̏ꍇ�Amin�����̂܂ܕԂ�
        else {
            return min;
        }
    }
}