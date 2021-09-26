using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �V���{���ړ����̑��x�̎�ނƐݒ�l
/// </summary>
public enum MoveTimeScale {
    Normal = 100,
    One_Half = 75,
    Double = 50,
    Count = 3
}

public class MoveTimeScaleController : MonoBehaviour
{
    [SerializeField]
    private Sprite normalSpeedIcon;

    [SerializeField]
    private Sprite oneHalfSpeedIcon;

    [SerializeField]
    private Sprite doubleSpeedIcon;

    [SerializeField]
    private Image imgSpeedIcon;

    [SerializeField]
    private Button btnStaminaFrame;

    public MoveTimeScale currentMoveTimeScale;
    public int currentTimeScaleNo;

    /// <summary>
    /// �����ݒ�
    /// </summary>
    public void SetUpMoveButtonController() {
        currentMoveTimeScale = MoveTimeScale.Normal;
        imgSpeedIcon.sprite = normalSpeedIcon;
        btnStaminaFrame.onClick.AddListener(SwitchMoveTimeScale);
    }

    /// <summary>
    /// �c��̃X�^�~�i���\���{�^�����������ۂ̏���
    /// </summary>
    private void SwitchMoveTimeScale() {

        // enum �Ǘ�
        currentMoveTimeScale = currentMoveTimeScale switch {
            MoveTimeScale.Normal => MoveTimeScale.One_Half,
            MoveTimeScale.One_Half => MoveTimeScale.Double,
            MoveTimeScale.Double => MoveTimeScale.Normal,
            _ => MoveTimeScale.Normal
        };

        // int �Ǘ�
        currentTimeScaleNo++;
        currentTimeScaleNo = currentTimeScaleNo % (int)MoveTimeScale.Count == 0 ? 0 : currentTimeScaleNo;

        // �A�C�R���摜�̐ݒ�
        imgSpeedIcon.sprite = currentMoveTimeScale switch {
            MoveTimeScale.Normal => normalSpeedIcon,
            MoveTimeScale.One_Half => oneHalfSpeedIcon,
            MoveTimeScale.Double => doubleSpeedIcon,
            _ => normalSpeedIcon
        };

        // �v���C���[�ƃG�l�~�[�V���{���̈ړ����x�̐ݒ�l���X�V
        GameData.instance.moveTimeScale = (float)currentMoveTimeScale / 100;
    }
}
