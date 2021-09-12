using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Tilemaps;

public enum MoveDirectionTpye {
    Up,
    Down,
    Left,
    Right,
    Count
}

public class EnemySymbol : SymbolBase
{
    private Tilemap tilemapCollider;
    private BoxCollider2D boxCol;
    private float moveDuration = 0.05f;

    public override void OnEnterSymbol(SymbolManager symbolManager) {
        base.OnEnterSymbol(symbolManager);

        tilemapCollider = symbolManager.tilemapCollider;
        TryGetComponent(out boxCol);
    }

    public override void TriggerAppearEffect(MapMoveController mapMoveController) {

        base.TriggerAppearEffect(mapMoveController);

        Debug.Log("�ړ���œG�ɐڐG");

        tween = transform.DOShakeScale(0.75f, 1.0f)
            .SetEase(Ease.OutQuart)
            .OnComplete(() => { OnExitSymbol(); } );
    }

    protected override void OnExitSymbol() {

        // �G�l�~�[�̃V���{���p�� List ����폜
        symbolManager.RemoveEnemySymbol(this);

        base.OnExitSymbol();

        // �o�g���̏���
        PreparateBattle();
    }

    /// <summary>
    /// �o�g���̏���
    /// </summary>
    private void PreparateBattle() {

        // �V�[���J�ڂ̏���
        SceneStateManager.instance.PreparateBattleScene();
    }

    /// <summary>
    /// �G�l�~�[�������_���ȕ����ɂP�}�X�ړ����邩�A���̏�őҋ@
    /// </summary>
    public void EnemyMove() {

        // �ړ���������������_���ɂP�ݒ�
        MoveDirectionTpye randomDirType = (MoveDirectionTpye)Random.Range(0, (int)MoveDirectionTpye.Count);

        Vector3 nextPos = GetMoveDirection(randomDirType);

        // �����̃R���C�_�[���I�t�ɂ��� Ray �������̃R���C�_�[�ɓ������Ă��܂��딻���h��
        SwtichCollider(false);

        // �ړ���������� Ray �𓊎˂��đ��̃V���{�������݂��Ă��Ȃ������m�F
        RaycastHit2D hit = Physics2D.Raycast(transform.position, nextPos, 0.8f, LayerMask.GetMask("Symbol"));�@�@�@//

        // Scene �r���[�ɂ� Ray �̉���
        Debug.DrawRay(transform.position, nextPos, Color.blue, 0.8f);

        SwtichCollider(true);

        // Ray �̓��ː�ɕʂ̃V���{��������ꍇ�ɂ� => �G�l�~�[�݂̂Ƃ肠�������O�B�A�C�e���̏�ɃG�l�~�[�����悤�ɂȂ�̂� 
        if (hit.collider != null) {
            // �I��
            return;
        }

        if (hit.collider != null &&  hit.collider.TryGetComponent(out EnemySymbol enemySymbol)) {

            return;
        }

        // �ړ��ł���^�C�����^�C���}�b�v�̍��W�ɕϊ����Ċm�F
        Vector3Int tilePos = tilemapCollider.WorldToCell(transform.position + nextPos);

        //Debug.Log(tilePos);
        //Debug.Log(tilemapCollider.GetColliderType(tilePos));

        // Grid �̃R���C�_�[�łȂ����
        if (tilemapCollider.GetColliderType(tilePos) != Tile.ColliderType.Grid) {

            // �ړ�
            transform.DOMove(transform.position + nextPos, moveDuration).SetEase(Ease.Linear);
        }
    }

    /// <summary>
    /// �ړ���������̏������W�ɕϊ�
    /// </summary>
    /// <param name="nextDirection"></param>
    /// <returns></returns>
    private Vector3 GetMoveDirection(MoveDirectionTpye nextDirection) {
        return nextDirection switch {
            MoveDirectionTpye.Up => new Vector2(0, 1),
            MoveDirectionTpye.Down => new Vector2(0, -1),
            MoveDirectionTpye.Left => new Vector2(-1, 0),
            MoveDirectionTpye.Right => new Vector2(1, 0),
            _ => Vector2.zero
        };
    }

    /// <summary>
    /// �R���C�_�[�̃I���I�t�؂�ւ�
    /// </summary>
    /// <param name="isSwicth"></param>
    public void SwtichCollider(bool isSwicth) {
        boxCol.enabled = isSwicth;
    }
}
