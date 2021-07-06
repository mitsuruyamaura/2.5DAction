using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using DG.Tweening;

public class MapMoveController : MonoBehaviour
{
    private Vector3 movePos;
    private float moveDuration = 0.5f;
    //private float moveTilePanel = 8.0f;
    //private Rigidbody2D rb;
    //Vector2 velocity;

    //[SerializeField]
    //private Tilemap tilemapWalk;

    [SerializeField]
    private Tilemap tilemapCollider;

    [SerializeField, HideInInspector]  // Debug用
    private bool isMoving;


    //void Start() {
    //    transform.GetChild(0).TryGetComponent(out rb);    
    //}

    /// <summary>
    /// 移動の入力判定
    /// </summary>
    /// <param name="context"></param>
    public void OnInputMove(InputAction.CallbackContext context) {

        // TODO 移動禁止なら処理しない


        // 移動中には処理しない
        if (isMoving) {
            return;
        }

        // キー入力値の受け取り
        movePos = context.ReadValue<Vector2>().normalized;

        // 取得タイミングによって不用意な数値が入るので、その場合には処理しない
        if (movePos == Vector3.zero) {
            return;
        }

        isMoving = true;

        Debug.Log(movePos);
        //move = (transform.position + move).normalized;

        //Vector3Int playerPos = new Vector3Int((int)transform.position.x, (int)transform.position.y, 0);
        //Debug.Log(playerPos);

        //Vector3Int gridPos = grid.WorldToCell(playerPos + move);
        //Debug.Log(gridPos);

        //Ray2D ray = new Ray2D(Vector2.zero, move);
        //Debug.Log(ray.origin);
        //Debug.Log(ray.direction);

        //RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction, 0.75f);  // , LayerMask.GetMask("Collider"), , LayerMask.GetMask("Confiner")
        //Debug.DrawRay(ray.origin, ray.direction, Color.red, 1.5f);

        //Debug.Log(hits.Length);

        //for (int i = 0; i < hits.Length; i++) {
        //    //Debug.Log(hits[i].collider.name);

        //    if (hits[i].collider != null) {

        // 斜め移動はなしにする
        if (Mathf.Abs(movePos.x) != 0) {
            movePos.y = 0;
        }

        // タイルマップの座標に変換
        Vector3Int tilePos = tilemapCollider.WorldToCell(transform.position + movePos);

        //Debug.Log(tilemapWalk.GetColliderType(tilePos));
        //Debug.Log(tilemapCollider.GetColliderType(tilePos));

        // Grid のコライダーの場合
        if (tilemapCollider.GetColliderType(tilePos) == Tile.ColliderType.Grid) {

           // 移動しないで終了
            isMoving = false;
        //    //break;

        //// Grid 以外の場合
        } else { 
        //if (tilemapWalk.GetColliderType(tilePos) == Tile.ColliderType.None) {   // tilemapCollider.GetColliderType(tilePos) != Tile.ColliderType.Grid) {
            
            // 移動させる
            Move(transform.position + movePos);
            //break;
        } 


        //}
        //}

        //isMoving = false;

        //if (tilemap.GetColliderType(gridPos) == Tile.ColliderType.Sprite) {
        //    Move(move);
        //}
    }

    void FixedUpdate()
    {
        //InputMove();
        //rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }

    public void OnJump(InputAction.CallbackContext context) {

    }

    /// <summary>
    /// 移動
    /// </summary>
    /// <param name="destination"></param>
    private void Move(Vector2 destination) {
        //transform.Translate(move * 0.5f);
        //rb.velocity = move * moveTilePanel;

        //velocity = move * moveTilePanel;

        //rb.MovePosition(rb.position + velocity );

        GameData.instance.staminaPoint.Value--;

        transform.DOMove(destination, moveDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                isMoving = false;                
            });        
    }

    private void OnTriggerEnter2D(Collider2D collision) {

        if (collision.TryGetComponent(out SymbolBase symbolBase)) {
            switch (symbolBase.symbolType) {
                case SymbolType.Enemy:
                    Debug.Log("移動先で敵に接触");
                    StartCoroutine(PreparateBattle(symbolBase));
                    
                    break;

                case SymbolType.Stamina:
                case SymbolType.Life:
                    Debug.Log("移動先で回復アイテムに接触 : " + symbolBase.symbolType.ToString());
                    symbolBase.TriggerAppearEffect();

                    break;
            }
        }
    }

    /// <summary>
    /// バトルの準備
    /// </summary>
    /// <param name="symbolBase"></param>
    /// <returns></returns>
    private IEnumerator PreparateBattle(SymbolBase symbolBase) {


        yield return new WaitForSeconds(moveDuration);

        Debug.Log("Appear Enemy");

        symbolBase.TriggerAppearEffect();

        // TODO バトル前に座標情報を GameData に保持


        // TODO エフェクトや SE


        // TODO 敵の情報を取得


        // TODO シーン遷移


    }
}
