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
        //// 引数の token を使ってキャンセル可能にするため、CancellationTokenSource を生成
        //CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(token);

        //// btnStop.OnClickAsObservable を購読し、ボタン押下時に cts を利用して UniTask をキャンセル
        //stopButtonSubscription = btnStop.OnClickAsObservable()
        //    .ThrottleFirst(System.TimeSpan.FromSeconds(1.0f))
        //    .Subscribe(_ => cts.Cancel());

        // デバッグ用
        currentCoolTime = UnityEngine.Random.Range(1.0f, 2.0f);
        currentAccuracy = UnityEngine.Random.Range(50.0f, 100.0f);
        currentMinDamage = GetRandomValue(1, 4);
        currentMaxDamage = GetRandomValue(1, 4);
        currentMinAttackCount = 1;
        currentMaxAttackCount = GetRandomValue(1, 4);
        //Debug.Log(currentCoolTime);

        // 今回の行動回数
        int count = 0;

        try {
            // バトルがキャンセルされていない間、あるいは制限時間が来たら
            while (!token.IsCancellationRequested) {

                // 例えば、特定の行動回数のときだけ処理変わる、など (count == 0 なら fillAmount = 0.5f スタートなど。)

                imgIconGauge.fillAmount = 1.0f;

                // バフ、デバフを適用して現在値を算出

                // 
                tweener = null;
                tweener = imgIconGauge.DOFillAmount(0f, currentCoolTime).SetEase(Ease.Linear).SetLink(gameObject);
                
                // キャンセル時の処理を登録(このスコープ内でキャンセルされた場合に、この処理が実行される)
                using (token.Register(() => {
                    tweener?.Kill();
                })) {
                    // 攻撃までの待機時間
                    // 小数点以下も含めた精度を保つために、適切なスケーリングを行う
                    int attackInterval = Mathf.CeilToInt(currentCoolTime * 1000); // 1000 でスケールして切り上げ
                    //Debug.Log(attackInterval);
                    await UniTask.Delay(attackInterval, cancellationToken: token);
                }

                // 攻撃回数分だけ、行動を処理
                int currentAttackCount = GetRandomValue(currentMinAttackCount, currentMaxAttackCount);
                for (int i = 0; i < currentAttackCount; i++) {
                    AutoAttack(myEntityType);
                    // for 文なのでフレーム跨がせる。そうしないと、ここですべて処理しようとして処理が一時止まる
                    await UniTask.Yield(cancellationToken: token);
                }
                count++;
            }
        }
        catch (OperationCanceledException) {
            // while 文を抜けてキャンセルされた場合の処理
            //tweener?.Kill();
            //tweener = null;

            // キャンセルされたことを通知
            onCancel.OnNext(Unit.Default);
        }
        imgIconGauge.fillAmount = 1.0f;
    }

    private void AutoAttack(EntityType myEntityType) {
        float randomValue = UnityEngine.Random.Range(0, 100.0f);

        // 命中判定
        if (randomValue <= currentAccuracy) {
            // 命中した場合の処理
            Debug.Log("Hit!");

            // 攻撃力設定
            int damage = GetRandomValue(currentMinDamage, currentMaxDamage);

            // ダメージ処理
            if(myEntityType == EntityType.Player) {
                BattleManager.instance.UpdateEnemyHp(damage);
            } else {
                BattleManager.instance.UpdatePlayerHp(damage);
            }
        } else {
            // 失敗した場合の処理
            Debug.Log("Miss!");
        }
    }

    private int GetRandomValue(int min, int max) {
        // maxが2以上の場合にランダムな値を返す
        if (max >= 2) {
            return UnityEngine.Random.Range(min, max);
        }
        // maxが2未満の場合、minをそのまま返す
        else {
            return min;
        }
    }
}