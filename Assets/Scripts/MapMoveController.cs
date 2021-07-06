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

    [SerializeField, HideInInspector]  // Debug�p
    private bool isMoving;


    //void Start() {
    //    transform.GetChild(0).TryGetComponent(out rb);    
    //}

    /// <summary>
    /// �ړ��̓��͔���
    /// </summary>
    /// <param name="context"></param>
    public void OnInputMove(InputAction.CallbackContext context) {

        // TODO �ړ��֎~�Ȃ珈�����Ȃ�


        // �ړ����ɂ͏������Ȃ�
        if (isMoving) {
            return;
        }

        // �L�[���͒l�̎󂯎��
        movePos = context.ReadValue<Vector2>().normalized;

        // �擾�^�C�~���O�ɂ���ĕs�p�ӂȐ��l������̂ŁA���̏ꍇ�ɂ͏������Ȃ�
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

        // �΂߈ړ��͂Ȃ��ɂ���
        if (Mathf.Abs(movePos.x) != 0) {
            movePos.y = 0;
        }

        // �^�C���}�b�v�̍��W�ɕϊ�
        Vector3Int tilePos = tilemapCollider.WorldToCell(transform.position + movePos);

        //Debug.Log(tilemapWalk.GetColliderType(tilePos));
        //Debug.Log(tilemapCollider.GetColliderType(tilePos));

        // Grid �̃R���C�_�[�̏ꍇ
        if (tilemapCollider.GetColliderType(tilePos) == Tile.ColliderType.Grid) {

           // �ړ����Ȃ��ŏI��
            isMoving = false;
        //    //break;

        //// Grid �ȊO�̏ꍇ
        } else { 
        //if (tilemapWalk.GetColliderType(tilePos) == Tile.ColliderType.None) {   // tilemapCollider.GetColliderType(tilePos) != Tile.ColliderType.Grid) {
            
            // �ړ�������
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
    /// �ړ�
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
                    Debug.Log("�ړ���œG�ɐڐG");
                    StartCoroutine(PreparateBattle(symbolBase));
                    
                    break;

                case SymbolType.Stamina:
                case SymbolType.Life:
                    Debug.Log("�ړ���ŉ񕜃A�C�e���ɐڐG : " + symbolBase.symbolType.ToString());
                    symbolBase.TriggerAppearEffect();

                    break;
            }
        }
    }

    /// <summary>
    /// �o�g���̏���
    /// </summary>
    /// <param name="symbolBase"></param>
    /// <returns></returns>
    private IEnumerator PreparateBattle(SymbolBase symbolBase) {


        yield return new WaitForSeconds(moveDuration);

        Debug.Log("Appear Enemy");

        symbolBase.TriggerAppearEffect();

        // TODO �o�g���O�ɍ��W���� GameData �ɕێ�


        // TODO �G�t�F�N�g�� SE


        // TODO �G�̏����擾


        // TODO �V�[���J��


    }
}
