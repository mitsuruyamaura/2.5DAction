using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

[CreateAssetMenu(fileName = "EnemyMoveEventDataSO", menuName = "Create EnemyMoveEventDataSO")]
public class EnemyMoveEventDataSO : ScriptableObject
{
    public UnityAction<Transform, float> GetEnemyMove(EnemyMoveType enemyMoveType) {

        return enemyMoveType switch {
            EnemyMoveType.Archer => MoveTypeArcher,
            EnemyMoveType.Defender => MoveTypeDefender,

            _ => MoveTypeArcher
        };
    }

    /// <summary>
    /// アーチャータイプの移動
    /// </summary>
    /// <param name="tran"></param>
    /// <param name="duration"></param>
    public void MoveTypeArcher(Transform tran, float duration) {
        Debug.Log("アーチャータイプの移動");

        tran.DOMoveX(tran.position.x + Random.Range(-15.0f, 15.0f), duration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tran"></param>
    /// <param name="duration"></param>
    public void MoveTypeDefender(Transform tran, float duration) {
        Debug.Log("ディフェンダータイプの移動");

        tran.DOMoveX(tran.position.z + Random.Range(-15.0f, 15.0f), duration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }
}
