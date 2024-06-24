using UnityEngine;
using UnityEngine.Tilemaps;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class PlayerMovement : MonoBehaviour {

    [SerializeField] private Tilemap walkableTilemap;  // 移動可能なタイルマップ
    [SerializeField] private Tilemap colliderTilemap;  // コライダー用タイルマップ
    [SerializeField] private Stage stage;

    private Vector3Int playerPosition;                 // プレイヤーのタイル位置
    private Vector3Int prevPosition;                   // プレイヤーの1つ前のタイル位置
    private float offsetPos = 0.5f;
    private Tweener tweener;


    // デバッグ用 GameData に持たせる。Stage から変える
    public enum TurnState {
        None,
        Player_Wait,
        Player_Move,
        Enemy,
        Boss
    }

    // デバッグ用
    private TurnState currentTurnState = TurnState.None;


    void Start() {
        // デバッグ用
        SetUp();
    }

    public void SetUp() {
        // プレイヤーの初期位置をタイルマップのグリッド座標に変換
        playerPosition = walkableTilemap.WorldToCell(transform.position);

        // マウスクリックイベントを監視
        this.UpdateAsObservable()
            .Where(_ => Input.GetMouseButtonDown(0) && currentTurnState == TurnState.Player_Wait)
            .Subscribe(_ => OnClickMovePlayer());

        currentTurnState = TurnState.Player_Wait;
    }

    private void OnClickMovePlayer() {
        // マウスのワールド座標を取得
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0; // Z軸は無視

        // マウスのワールド座標をタイルマップのグリッド座標に変換
        Vector3Int mouseCellPos = walkableTilemap.WorldToCell(mouseWorldPos);

        //Debug.Log("Mouse Cell Position: " + mouseCellPos);
        //Debug.Log("Player Position: " + playerPosition);

        // プレイヤーのタイル位置と同じなら移動しない
        if (mouseCellPos == playerPosition) {
            return;
        }

        currentTurnState = TurnState.Player_Move;

        // 方向ベクトルを計算
        Vector3Int direction = mouseCellPos - playerPosition;

        // 角度を計算
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 角度に基づいて移動方向を決定
        if (angle >= -45 && angle <= 45) {
            direction = Vector3Int.right;    // 右
        } else if (angle > 45 && angle <= 135) {
            direction = Vector3Int.up;       // 上
        } else if (angle > 135 || angle <= -135) {
            direction = Vector3Int.left;     // 左
        } else if (angle > -135 && angle < -45) {
            direction = Vector3Int.down;     // 下
        }

        // 移動先のタイル位置を計算
        Vector3Int targetPosition = playerPosition + direction;

        // ターゲット位置が移動可能であるかチェック
        if (colliderTilemap.GetColliderType(targetPosition) == Tile.ColliderType.None && walkableTilemap.HasTile(targetPosition)) {
            // プレイヤーの位置を更新
            prevPosition = playerPosition;
            playerPosition = targetPosition;

            // 新しいタイル位置をワールド座標に変換
            Vector3 newWorldPosition = walkableTilemap.CellToWorld(playerPosition);
            //transform.position = new Vector3(newWorldPosition.x + offsetPos, newWorldPosition.y + offsetPos, transform.position.z);

            Vector3 destination = new (newWorldPosition.x + offsetPos, newWorldPosition.y + offsetPos, transform.position.z);

            // 移動
            tweener = transform.DOMove(destination, 0.5f)
                .SetEase(Ease.Linear).SetLink(gameObject)
                .OnComplete(() => {
                    //isMoving = false;

                    // エネミーの番になり、エネミーの移動処理を行う
                    //stage.CurrentTurnState = Stage.TurnState.Enemy;

                    // 敵のターン開始
                    stage.ExecuteEnemyTurnAsync().Forget();
                    tweener.Kill();
                    RestorePlayerWaitState();
                });
        }
        else {
            RestorePlayerWaitState();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void RestorePlayerWaitState() {
        // 状態をPlayer_Waitに戻す共通処理
        currentTurnState = TurnState.Player_Wait;
    }
}