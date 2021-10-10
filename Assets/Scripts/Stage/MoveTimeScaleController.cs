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
        btnStaminaFrame.onClick.AddListener(OnClickSwitchMoveTimeScale);
    }

    /// <summary>
    /// �c��̃X�^�~�i���\���{�^�����������ۂ̏���
    /// </summary>
    private void OnClickSwitchMoveTimeScale() {

        // MoveTimeScale ���P�i�߂�BNormal => OneHalf => Double => Normal �ŃT�C�N����
        (Sprite nextSprite, MoveTimeScale nextMoveTimeScaleType) nextTimeScaleValue = currentMoveTimeScale switch
        {
            MoveTimeScale.Normal => (oneHalfSpeedIcon, MoveTimeScale.One_Half),
            MoveTimeScale.One_Half => (doubleSpeedIcon, MoveTimeScale.Double),
            MoveTimeScale.Double => (normalSpeedIcon, MoveTimeScale.Normal),
            _ => (normalSpeedIcon, MoveTimeScale.Normal)
        };

        //    // enum �Ǘ�
        //    currentMoveTimeScale = currentMoveTimeScale switch {
        //    MoveTimeScaleType.Normal => MoveTimeScaleType.One_Half,
        //    MoveTimeScaleType.One_Half => MoveTimeScaleType.Double,
        //    MoveTimeScaleType.Double => MoveTimeScaleType.Normal,
        //    _ => MoveTimeScaleType.Normal
        //};

        // int �Ǘ�
        currentTimeScaleNo++;
        currentTimeScaleNo = currentTimeScaleNo % (int)MoveTimeScale.Count == 0 ? 0 : currentTimeScaleNo;

        //// �A�C�R���摜�̐ݒ�
        //imgSpeedIcon.sprite = currentMoveTimeScale switch {
        //    MoveTimeScaleType.Normal => normalSpeedIcon,
        //    MoveTimeScaleType.One_Half => oneHalfSpeedIcon,
        //    MoveTimeScaleType.Double => doubleSpeedIcon,
        //    _ => normalSpeedIcon
        //};

        // MoveTimeScale ��ύX
        currentMoveTimeScale = nextTimeScaleValue.nextMoveTimeScaleType;

        // �A�C�R���摜�̕ύX���A���݂� MoveTimeScale �ɍ��킹��
        imgSpeedIcon.sprite = nextTimeScaleValue.nextSprite;

        // �v���C���[�ƃG�l�~�[�V���{���̈ړ����x�̐ݒ�l�����݂� MoveTimeScale �̓��e�ɍX�V
        GameData.instance.moveTimeScale = (float)currentMoveTimeScale / 100;
    }
}
