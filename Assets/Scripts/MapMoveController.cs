using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using DG.Tweening;

public class MapMoveController : MonoBehaviour
{
    private Vector3 move;
    private float moveTilePanel = 8.0f;
    private Rigidbody2D rb;
    Vector2 velocity;

    [SerializeField]
    private Grid grid;

    [SerializeField]
    private Tilemap tilemap;

    [SerializeField]
    private Tilemap tilemapCollider;

    public bool isMoving;

    void Start() {
        transform.GetChild(0).TryGetComponent(out rb);    
    }

    public void OnInputMove(InputAction.CallbackContext context) {

        if (isMoving) {
            return;
        }



        move = context.ReadValue<Vector2>().normalized;

        if (move == Vector3.zero) {
            return;
        }

        isMoving = true;

        //move.x = Input.GetAxisRaw("Horizontal");
        //move.y = Input.GetAxisRaw("Vertical");
        Debug.Log(move);
        //move = (transform.position + move).normalized;

        //Vector3Int playerPos = new Vector3Int((int)transform.position.x, (int)transform.position.y, 0);
        //Debug.Log(playerPos);

        //Vector3Int gridPos = grid.WorldToCell(playerPos + move);
        //Debug.Log(gridPos);

        Ray2D ray = new Ray2D(Vector2.zero, move);
        Debug.Log(ray.origin);
        Debug.Log(ray.direction);

        RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction, 0.75f);  // , LayerMask.GetMask("Collider"), , LayerMask.GetMask("Confiner")
        Debug.DrawRay(ray.origin, ray.direction, Color.red, 1.5f);

        Debug.Log(hits.Length);

        for (int i = 0; i < hits.Length; i++) {
            //Debug.Log(hits[i].collider.name);

            if (hits[i].collider != null) {

                var tilePos = tilemap.WorldToCell(transform.position + move);

                Debug.Log(tilemap.GetColliderType(tilePos));
                Debug.Log(tilemapCollider.GetColliderType(tilePos));

                if (tilemapCollider.GetColliderType(tilePos) == Tile.ColliderType.Grid) {
                    isMoving = false;
                    break;
                }

                if (tilemap.GetColliderType(tilePos) != Tile.ColliderType.Grid){   // tilemapCollider.GetColliderType(tilePos) != Tile.ColliderType.Grid) {
                    Move(transform.position +  move);
                    break;
                }


            }
        }

        //isMoving = false;

        


        //if (tilemap.GetColliderType(gridPos) == Tile.ColliderType.Sprite) {
        //    Move(move);
        //}


        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //InputMove();
        //rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }

    public void OnJump(InputAction.CallbackContext context) {

    }

    private void Move(Vector2 movePos) {
        //transform.Translate(move * 0.5f);
        //rb.velocity = move * moveTilePanel;

        //velocity = move * moveTilePanel;

        //rb.MovePosition(rb.position + velocity );

        transform.DOMove(movePos, 0.5f).SetEase(Ease.Linear).OnComplete(() => { isMoving = false; });
        
    }
}
