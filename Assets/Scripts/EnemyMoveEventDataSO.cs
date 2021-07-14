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
    /// �A�[�`���[�^�C�v�̈ړ�
    /// </summary>
    /// <param name="tran"></param>
    /// <param name="duration"></param>
    public void MoveTypeArcher(Transform tran, float duration) {
        Debug.Log("�A�[�`���[�^�C�v�̈ړ�");

        tran.DOMoveX(tran.position.x + 15.0f, duration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }
}
