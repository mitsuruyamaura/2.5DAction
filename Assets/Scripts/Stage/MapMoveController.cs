using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using DG.Tweening;
using UnityEngine.Events;

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

    public bool IsMoving { set => isMoving = value;  get => isMoving; }

    [SerializeField]
    private List<PlayerConditionBase> conditionsList = new List<PlayerConditionBase>();

    private Stage stage;

    private int steppingRecoveryPoint = 3;

    //private UnityAction<MapMoveController> enemySymbolTriggerEvent;
    //private UnityAction<MapMoveController> orbSymbolTriggerEvent;
    private UnityEvent<MapMoveController> enemySymbolTriggerEvent;
    private UnityEvent<MapMoveController> orbSymbolTriggerEvent;

    [SerializeField]
    private Transform conditionEffectTran;


    void Start() {
        //transform.GetChild(0).TryGetComponent(out rb);

        //PlayerConditionBase condition = gameObject.AddComponent<PlayerCondition_Fatigue>();
        //condition.AddCondition(ConditionType.Fatigue, 5, 0.5f, this, stage.GetSymbolManager());
        //conditionsList.Add(condition);
    }

    /// <summary>
    /// �����ݒ�
    /// </summary>
    /// <param name="stage"></param>
    public void SetUpMapMoveController(Stage stage) {
        this.stage = stage;
        tilemapCollider = this.stage.GetSymbolManager().tilemapCollider;
    }

    /// <summary>
    /// �ړ��̓��͔���
    /// </summary>
    /// <param name="context"></param>
    public void OnInputMove(InputAction.CallbackContext context) {

        return;

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

        // ������Ԃ̃R���f�B�V�����̊m�F
        if (JudgeConditionType(ConditionType.Confusion)) {
            // ������Ԃ̏ꍇ�͈ړ���𗐐���(�΂߈ړ��͂����Ȃ�)
            int x = Random.Range(-1, 2);
            int y = Mathf.Abs(x) == 1 ? 0 : Random.Range(-1, 2);
            movePos = new Vector2(x, y);
        } else {
            // �L�[���͂����̂܂ܓK�p
            movePos = nextPos;
        }

        // �^�C���}�b�v�̍��W�ɕϊ�
        Vector3Int tilePos = tilemapCollider.WorldToCell(transform.position + movePos);

        //Debug.Log(tilePos);
        //Debug.Log(tilemapCollider.GetColliderType(tilePos));

        // Grid �̃R���C�_�[�̏ꍇ
        if (tilemapCollider.GetColliderType(tilePos) == Tile.ColliderType.Grid) {

            // �ړ����Ȃ��ŏI��
            isMoving = false;

            // �{�^����������悤�ɂ���
            stage.ActivateInputButtons();
            //    //break;

            //Debug.Log("Grid");
            
            // Grid �ȊO�̏ꍇ
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

        //// �R���f�B�V�������t�^����Ă���ꍇ�A�������Ԃ��X�V
        //if (conditionsList.Count > 0) {
        //    // ���݂̃R���f�B�V�����̏�Ԃ̎c�莞�Ԃ��X�V
        //    UpdateConditionsDuration();
        //}

        // �ړ�
        transform.DOMove(destination, moveDuration * GameData.instance.moveTimeScale)
            .SetEase(Ease.Linear)
            .OnComplete(() => {
                //isMoving = false;

                // �G�l�~�[�̔ԂɂȂ�A�G�l�~�[�̈ړ��������s��
                //stage.CurrentTurnState = Stage.TurnState.Enemy;

                // �G�̃^�[���J�n
                StartCoroutine(stage.ObserveEnemyTurnState());
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

            // �����V���{���ɐڐG�����ꍇ�͏������Ȃ�
            if (symbolBase.isSymbolTriggerd) {
                return;
            }

            Debug.Log("�ړ���ŃV���{���ɐڐG : " + symbolBase.symbolType.ToString());

            // �G�l�~�[�̃V���{���̏ꍇ
            if (symbolBase.symbolType == SymbolType.Enemy) {

                // �G�l�~�[�̃V���{���̃C�x���g�̏d���o�^�͂��Ȃ�
                if (enemySymbolTriggerEvent != null) {
                    return;
                }

                symbolBase.isSymbolTriggerd = true;

                // �V���{���̃C�x���g��o�^���ė\�񂵁A���ׂẴG�l�~�[�̈ړ����I�����Ă�����s
                //enemySymbolTriggerEvent = (x) =>  symbolBase.TriggerAppearEffect(this);
                enemySymbolTriggerEvent = new UnityEvent<MapMoveController>();
                enemySymbolTriggerEvent.AddListener(symbolBase.TriggerAppearEffect);

                Debug.Log("�o�^");
            }

            // �I�[�u���A�g���W���[�{�b�N�X�̏ꍇ
            if (symbolBase.symbolType == SymbolType.Orb || symbolBase.symbolType == SymbolType.TreasureBox) {
                // �V���{���̃C�x���g��o�^���ė\�񂵁A�o�g���� Stage �ɖ߂��Ă��Ă�����s
                //orbSymbolTriggerEvent = _ => symbolBase.TriggerAppearEffect(this);
                orbSymbolTriggerEvent = new UnityEvent<MapMoveController>();
                orbSymbolTriggerEvent.AddListener(symbolBase.TriggerAppearEffect);
            } else if (symbolBase.symbolType != SymbolType.Enemy) {

                // �􂢂̃R���f�B�V�����̊m�F
                if (JudgeConditionType(ConditionType.Curse)) {

                    // �􂢏�Ԃł���ꍇ�́A�V���{���̃C�x���g�𔭐������Ȃ�
                    return;
                }

                // ����ȊO�̃V���{���͂����Ɏ��s
                symbolBase.TriggerAppearEffect(this);
            }
            //break;
            //}
        }
    }

    /// <summary>
    /// ���݂̃R���f�B�V�����̏�Ԃ̎c�莞�Ԃ��X�V
    /// </summary>
    public void UpdateConditionsDuration() {
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

        //// �R���f�B�V�������t�^����Ă���ꍇ�A�������Ԃ��X�V
        //if (conditionsList.Count > 0) {
        //    // ���݂̃R���f�B�V�����̏�Ԃ̎c�莞�Ԃ��X�V
        //    UpdateConditionsDuration();
        //}

        // �����݂���HP��
        GameData.instance.hp = Mathf.Clamp(GameData.instance.hp += steppingRecoveryPoint, 0, GameData.instance.maxHp);

        StartCoroutine(stage.UpdateDisplayHp(1.0f));

        //// �G�l�~�[�̔ԂɂȂ�A�G�l�~�[�̈ړ��������s��
        //stage.CurrentTurnState = Stage.TurnState.Enemy;

        // �G�̃^�[���J�n
        StartCoroutine(stage.ObserveEnemyTurnState());
    }

    /// <summary>
    /// �o�^����Ă���G�l�~�[�V���{���̃C�x���g(�G�l�~�[�Ƃ̃o�g��)�����s
    /// </summary>
    public bool CallBackEnemySymbolTriggerEvent() {

        if (enemySymbolTriggerEvent != null) {
            // �C�x���g������Ƃ��������s����
            enemySymbolTriggerEvent?.Invoke(this);

            // �C�x���g���N���A
            enemySymbolTriggerEvent?.RemoveAllListeners();
            enemySymbolTriggerEvent = null;

            return true;
        }
        return false;
    }

    /// <summary>
    /// �o�^����Ă���I�[�u�V���{���̃C�x���g�����s
    /// </summary>
    public void CallBackOrbSymbolTriggerEvent() {

        if (orbSymbolTriggerEvent != null) {
            // �C�x���g������Ƃ��������s����
            orbSymbolTriggerEvent?.Invoke(this);

            // �C�x���g���N���A
            orbSymbolTriggerEvent?.RemoveAllListeners();
            orbSymbolTriggerEvent = null;
        }
    }

    /// <summary>
    /// Stage �̏����擾
    /// </summary>
    /// <returns></returns>
    public Stage GetStage() {
        return stage;
    }

    /// <summary>
    /// �����Ɏw�肳�ꂽ�R���f�B�V�������t�^����Ă��邩�m�F
    /// </summary>
    /// <param name="conditionType"></param>
    /// <returns></returns>
    public bool JudgeConditionType(ConditionType conditionType) {
        return conditionsList.Find(x => x.GetConditionType() == conditionType);
    }

    /// <summary>
    /// �R���f�B�V�����p�̃G�t�F�N�g�����ʒu�̎擾
    /// </summary>
    /// <returns></returns>
    public Transform GetConditionEffectTran() {
        return conditionEffectTran;
    }
}
