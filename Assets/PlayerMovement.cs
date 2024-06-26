using UnityEngine;
using UnityEngine.Tilemaps;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class PlayerMovement : MonoBehaviour {

    [SerializeField] private Tilemap walkableTilemap;  // �ړ��\�ȃ^�C���}�b�v
    [SerializeField] private Tilemap colliderTilemap;  // �R���C�_�[�p�^�C���}�b�v
    [SerializeField] private Stage stage;

    private Vector3Int playerPosition;                 // �v���C���[�̃^�C���ʒu
    private Vector3Int prevPosition;                   // �v���C���[��1�O�̃^�C���ʒu
    private float offsetPos = 0.5f;
    private Tweener tweener;


    // �f�o�b�O�p GameData �Ɏ�������BStage ����ς���
    public enum TurnState {
        None,
        Player_Wait,
        Player_Move,
        Enemy,
        Boss
    }

    // �f�o�b�O�p
    private TurnState currentTurnState = TurnState.None;


    void Start() {
        // �f�o�b�O�p
        SetUp();
    }

    public void SetUp() {
        // �v���C���[�̏����ʒu���^�C���}�b�v�̃O���b�h���W�ɕϊ�
        playerPosition = walkableTilemap.WorldToCell(transform.position);

        // �}�E�X�N���b�N�C�x���g���Ď�
        this.UpdateAsObservable()
            .Where(_ => Input.GetMouseButtonDown(0) && currentTurnState == TurnState.Player_Wait)
            .Subscribe(_ => OnClickMovePlayer());

        currentTurnState = TurnState.Player_Wait;
    }

    private void OnClickMovePlayer() {
        // �}�E�X�̃��[���h���W���擾
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0; // Z���͖���

        // �}�E�X�̃��[���h���W���^�C���}�b�v�̃O���b�h���W�ɕϊ�
        Vector3Int mouseCellPos = walkableTilemap.WorldToCell(mouseWorldPos);

        //Debug.Log("Mouse Cell Position: " + mouseCellPos);
        //Debug.Log("Player Position: " + playerPosition);

        // �v���C���[�̃^�C���ʒu�Ɠ����Ȃ�ړ����Ȃ�
        if (mouseCellPos == playerPosition) {
            return;
        }

        currentTurnState = TurnState.Player_Move;

        // �����x�N�g�����v�Z
        Vector3Int direction = mouseCellPos - playerPosition;

        // �p�x���v�Z
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // �p�x�Ɋ�Â��Ĉړ�����������
        if (angle >= -45 && angle <= 45) {
            direction = Vector3Int.right;    // �E
        } else if (angle > 45 && angle <= 135) {
            direction = Vector3Int.up;       // ��
        } else if (angle > 135 || angle <= -135) {
            direction = Vector3Int.left;     // ��
        } else if (angle > -135 && angle < -45) {
            direction = Vector3Int.down;     // ��
        }

        // �ړ���̃^�C���ʒu���v�Z
        Vector3Int targetPosition = playerPosition + direction;

        // �^�[�Q�b�g�ʒu���ړ��\�ł��邩�`�F�b�N
        if (colliderTilemap.GetColliderType(targetPosition) == Tile.ColliderType.None && walkableTilemap.HasTile(targetPosition)) {
            // �v���C���[�̈ʒu���X�V
            prevPosition = playerPosition;
            playerPosition = targetPosition;

            // �V�����^�C���ʒu�����[���h���W�ɕϊ�
            Vector3 newWorldPosition = walkableTilemap.CellToWorld(playerPosition);
            //transform.position = new Vector3(newWorldPosition.x + offsetPos, newWorldPosition.y + offsetPos, transform.position.z);

            Vector3 destination = new (newWorldPosition.x + offsetPos, newWorldPosition.y + offsetPos, transform.position.z);

            // �ړ�
            tweener = transform.DOMove(destination, 0.5f)
                .SetEase(Ease.Linear).SetLink(gameObject)
                .OnComplete(() => {
                    //isMoving = false;

                    // �G�l�~�[�̔ԂɂȂ�A�G�l�~�[�̈ړ��������s��
                    //stage.CurrentTurnState = Stage.TurnState.Enemy;

                    // �G�̃^�[���J�n
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
        // ��Ԃ�Player_Wait�ɖ߂����ʏ���
        currentTurnState = TurnState.Player_Wait;
    }
}