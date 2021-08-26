using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using DG.Tweening;

public class MapMoveController : MonoBehaviour
{
    private Vector3 movePos;
    private float moveDuration = 0.35f;

    public float MoveDuration { get => moveDuration; }

    //private float moveTilePanel = 8.0f;
    //private Rigidbody2D rb;
    //Vector2 velocity;

    //[SerializeField]
    //private Tilemap tilemapWalk;

    [SerializeField]
    private Tilemap tilemapCollider;

    [SerializeField]  // Debug�p
    private bool isMoving;

    public bool IsMoving { get => isMoving; }

    [SerializeField]
    private List<PlayerConditionBase> conditionsList = new List<PlayerConditionBase>();

    private Stage stage;

    private int steppingRecoveryPoint = 3;


    //void Start() {
    //    transform.GetChild(0).TryGetComponent(out rb);    
    //}

    public void SetUpMapMoveController(Stage stage) {
        this.stage = stage;
        tilemapCollider = DataBaseManager.instance.tilemapCollider;
    }

    /// <summary>
    /// �ړ��̓��͔���
    /// </summary>
    /// <param name="context"></param>
    public void OnInputMove(InputAction.CallbackContext context) {

        // TODO �ړ��֎~�Ȃ珈�����Ȃ�

        // �����̃^�[���ȊO�͏������Ȃ�
        if (stage.CurrentTurnState != Stage.TurnState.Player) {
            return;
        }

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
        //Debug.Log(movePos);
        isMoving = true;


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

        //Debug.Log(tilePos);
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
    /// �ړ��ł���^�C��������
    /// </summary>
    public void CheckMoveTile(Vector2 nextPos) {

        // �v���C���[�̔ԂłȂ���Ώ������Ȃ�
        if (stage.CurrentTurnState != Stage.TurnState.Player) {
            return;
        }

        isMoving = true;

        movePos = nextPos;

        // �^�C���}�b�v�̍��W�ɕϊ�
        Vector3Int tilePos = tilemapCollider.WorldToCell(transform.position + movePos);

        //Debug.Log(tilePos);
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

        // �R���f�B�V�������t�^����Ă���ꍇ�A�������Ԃ��X�V
        if (conditionsList.Count > 0) {
            // ���݂̃R���f�B�V�����̏�Ԃ̎c�莞�Ԃ��X�V
            UpdateConditionsDuration();
        }

        // �ړ�
        transform.DOMove(destination, moveDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() => {
                isMoving = false;

                // �G�l�~�[�̔ԂɂȂ�A�G�l�~�[�̈ړ��������s��
                stage.CurrentTurnState = Stage.TurnState.Enemy;
            });
    }

    private void OnTriggerEnter2D(Collider2D collision) {

        if (collision.TryGetComponent(out SymbolBase symbolBase)) {
            //switch (symbolBase.symbolType) {
            //case SymbolType.Enemy:
            //    Debug.Log("�ړ���œG�ɐڐG");
            //    StartCoroutine(PreparateBattle(symbolBase));

            //    break;

            //case SymbolType.Stamina:
            //case SymbolType.Life:
            //case SymbolType.Orb:

            // �G�l�~�[�̃V���{���ɐڐG�����ہA�v���C���[�� Walk_Through �̃R���f�B�V�������t�^����Ă���ꍇ
            // symbolBase.symbolType == SymbolType.Enemy && conditionsList.Exists(x => x.GetConditionType() == ConditionType.Walk_through)
            if (symbolBase.symbolType == SymbolType.Enemy && TryGetComponent(out PlayerCondition_WalkThrough walkThrough)) {
                // �G�l�~�[�ɐڐG���Ă��퓬���J�n���Ȃ�
                return;
            }

            if (!symbolBase.isSymbolTriggerd) {
                Debug.Log("�ړ���ŃV���{���ɐڐG : " + symbolBase.symbolType.ToString());
                symbolBase.TriggerAppearEffect(this);
            }

                    //break;
            //}
        }
    }

    /// <summary>
    /// ���݂̃R���f�B�V�����̏�Ԃ̎c�莞�Ԃ��X�V
    /// </summary>
    private void UpdateConditionsDuration() {
        for (int i = 0; i< conditionsList.Count; i++) {
            conditionsList[i].CalcDuration();
        }
    }

    /// <summary>
    /// �R���f�B�V������ǉ�
    /// </summary>
    /// <param name="playerCondition"></param>
    public void AddConditionsList(PlayerConditionBase playerCondition) {
        conditionsList.Add(playerCondition);
    }

    /// <summary>
    /// �R���f�B�V�������폜
    /// </summary>
    public void RemoveConditionsList(PlayerConditionBase playerCondition) {
        conditionsList.Remove(playerCondition);
        Destroy(playerCondition);
    }

    /// <summary>
    /// �R���f�B�V������ List ���擾
    /// </summary>
    /// <returns></returns>
    public List<PlayerConditionBase> GetConditionsList() {
        return conditionsList;
    }

    /// <summary>
    /// ������
    /// </summary>
    public void Stepping() {
        // �v���C���[�̔ԂłȂ���Ώ������Ȃ�
        if (stage.CurrentTurnState != Stage.TurnState.Player) {
            return;
        }

        GameData.instance.staminaPoint.Value--;

        // �R���f�B�V�������t�^����Ă���ꍇ�A�������Ԃ��X�V
        if (conditionsList.Count > 0) {
            // ���݂̃R���f�B�V�����̏�Ԃ̎c�莞�Ԃ��X�V
            UpdateConditionsDuration();
        }

        // �����݂���HP��
        GameData.instance.hp = Mathf.Clamp(GameData.instance.hp += steppingRecoveryPoint, 0, GameData.instance.maxHp);

        StartCoroutine(stage.UpdateDisplayHp(1.0f));

        // �G�l�~�[�̔ԂɂȂ�A�G�l�~�[�̈ړ��������s��
        stage.CurrentTurnState = Stage.TurnState.Enemy;
    }
}
