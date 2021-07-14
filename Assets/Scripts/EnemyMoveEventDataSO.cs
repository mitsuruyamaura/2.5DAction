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

        tran.DOMoveX(tran.position.x + 15.0f, duration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }
}
