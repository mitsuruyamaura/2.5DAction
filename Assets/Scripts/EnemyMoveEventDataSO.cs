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
            EnemyMoveType.Assasin => MoveTypeAssasin,
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

        tran.DOMoveX(tran.position.x + Random.Range(-15.0f, 15.0f), duration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }

    /// <summary>
    /// �f�B�t�F���_�[�^�C�v�̈ړ�
    /// </summary>
    /// <param name="tran"></param>
    /// <param name="duration"></param>
    public void MoveTypeDefender(Transform tran, float duration) {
        Debug.Log("�f�B�t�F���_�[�^�C�v�̈ړ�");

        tran.DOMoveZ(tran.position.z + Random.Range(-5.0f, 5.0f), duration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }

    /// <summary>
    /// �A�T�V���^�C�v�̈ړ�
    /// </summary>
    /// <param name="tran"></param>
    /// <param name="duration"></param>
    public void MoveTypeAssasin(Transform tran, float duration) {
        tran.DOMove(tran.position, duration).SetEase(Ease.InOutQuart);
    }
}
