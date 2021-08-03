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

        tilemapCollider = DataBaseManager.instance.tilemapCollider;
        TryGetComponent(out boxCol);
    }

    public override void TriggerAppearEffect(MapMoveController mapMoveController) {

        base.TriggerAppearEffect(mapMoveController);

        Debug.Log("移動先で敵に接触");

        transform.DOShakeScale(0.75f, 1.0f)
            .SetEase(Ease.OutQuart)
            .OnComplete(() => { PreparateBattle(); } );
    }

    protected override void OnExitSymbol() {

        // エネミーのシンボル用の List から削除
        symbolManager.RemoveEnemySymbol(this);

        base.OnExitSymbol();
    }

    /// <summary>
    /// バトルの準備
    /// </summary>
    private void PreparateBattle() {

        //symbolManager.SwitchEnemyCollider(false);

        // TODO すべてのエネミーのシンボルの移動が終了しているか待機


        // TODO バトル前に座標情報を GameData に保持


        // TODO エフェクトや SE


        // TODO 敵の情報を取得


        // TODO シーン遷移

        SceneStateManager.instance.LoadBattleScene();
    }

    public void EnemyMove() {

        // 移動する方向をランダムに１つ設定
        MoveDirectionTpye randomDirType = (MoveDirectionTpye)Random.Range(0, (int)MoveDirectionTpye.Count);

        Vector3 nextPos = GetMoveDirection(randomDirType);

        // 自分のコライダーをオフにして Ray が自分のコライダーに当たってしまう誤判定を防ぐ
        SwtichCollider(false);

        // 移動する方向に Ray を投射して他のシンボルが存在していないかを確認
        RaycastHit2D hit = Physics2D.Raycast(transform.position, nextPos, 0.8f, LayerMask.GetMask("Symbol"));　　　//

        // Scene ビューにて Ray の可視化
        Debug.DrawRay(transform.position, nextPos, Color.blue, 0.8f);

        SwtichCollider(true);

        // Ray の投射先に別のシンボルがある場合には => エネミーのみとりあえず除外。アイテムの上にエネミーが乗るようになるので 
        if (hit.collider != null) {
            // 終了
            return;
        }

        if (hit.collider != null &&  hit.collider.TryGetComponent(out EnemySymbol enemySymbol)) {

            return;
        }

        // 移動できるタイルかタイルマップの座標に変換して確認
        Vector3Int tilePos = tilemapCollider.WorldToCell(transform.position + nextPos);

        //Debug.Log(tilePos);
        //Debug.Log(tilemapCollider.GetColliderType(tilePos));

        // Grid のコライダーでなければ
        if (tilemapCollider.GetColliderType(tilePos) != Tile.ColliderType.Grid) {

            // 移動
            transform.DOMove(transform.position + nextPos, moveDuration).SetEase(Ease.Linear);
        }
    }

    /// <summary>
    /// 移動する方向の情報を座標に変換
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

    public void SwtichCollider(bool isSwicth) {
        boxCol.enabled = isSwicth;
    }
}
