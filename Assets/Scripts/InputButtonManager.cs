using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

public class InputButtonManager : MonoBehaviour
{
    [SerializeField]
    private Button btnUp;

    [SerializeField]
    private Button btnDown;

    [SerializeField]
    private Button btnLeft;

    [SerializeField]
    private Button btnRight;

    [SerializeField]
    private Button btnStepping;

    //[SerializeField]
    private MapMoveController mapMoveController;

    /// <summary>
    /// �C���v�b�g�p�̃{�^���̐ݒ�
    /// </summary>
    /// <param name="mapMoveController"></param>
    public void SetUpInputButtonManager(MapMoveController mapMoveController) {
        this.mapMoveController = mapMoveController;

        // �e�{�^���Ƀ��\�b�h�o�^�B�ړ����͏d���^�b�v�h�~
        btnUp?
            .OnClickAsObservable()
            .Where(_ => !mapMoveController.IsMoving)
            // .TakeUntilDestroy(this) ��ǉ�����ƈ����Ɏw�肵���Q�[���I�u�W�F�N�g���N���X�̔j���ƈꏏ�ɍw�ǂ��I��
            .ThrottleFirst(TimeSpan.FromSeconds(mapMoveController.MoveDuration))
            .Subscribe(_ => {
                InputMoveButton(new Vector2(0, 1));
            });

        btnDown?
            .OnClickAsObservable()
            .Where(_ => !mapMoveController.IsMoving)
            .ThrottleFirst(TimeSpan.FromSeconds(mapMoveController.MoveDuration))
            .Subscribe(_ => {
                InputMoveButton(new Vector2(0, -1));
            });

        btnLeft?
            .OnClickAsObservable()
            .Where(_ => !mapMoveController.IsMoving)
            .ThrottleFirst(TimeSpan.FromSeconds(mapMoveController.MoveDuration))
            .Subscribe(_ => {
                 InputMoveButton(new Vector2(-1, 0));
            });

        btnRight?
            .OnClickAsObservable()
            .Where(_ => !mapMoveController.IsMoving)
            .ThrottleFirst(TimeSpan.FromSeconds(mapMoveController.MoveDuration))
            .Subscribe(_ => {
                InputMoveButton(new Vector2(1, 0));
            });

        //btnUp.onClick.AddListener(() => InputMoveButton(new Vector2(0, 1)));
        //btnDown.onClick.AddListener(() => InputMoveButton(new Vector2(0, -1)));
        //btnLeft.onClick.AddListener(() => InputMoveButton(new Vector2(-1, 0)));
        //btnRight.onClick.AddListener(() => InputMoveButton(new Vector2(1, 0)));

        btnStepping.onClick.AddListener(InputSteppingButton);
    }

    /// <summary>
    /// ���͂��ꂽ�L�[�̕������擾
    /// </summary>
    /// <param name="pos"></param>
    private void InputMoveButton(Vector2 pos) {
        SwitchActivateAllButtons(false);
        mapMoveController.CheckMoveTile(pos);
    }

    /// <summary>
    /// �����݃{�^���̏���
    /// </summary>
    private void InputSteppingButton() {
        SwitchActivateAllButtons(false);
        mapMoveController.Stepping();

        //Debug.Log("Input Stepping");
    }

    /// <summary>
    /// �{�^���̊�����/�񊈐����̐؂�ւ�
    /// </summary>
    /// <param name="isSwitch"></param>
    public void SwitchActivateAllButtons(bool isSwitch) {
        //Debug.Log(isSwitch);
        btnDown.interactable = isSwitch;
        btnLeft.interactable = isSwitch;
        btnRight.interactable = isSwitch;
        btnUp.interactable = isSwitch;
        btnStepping.interactable = isSwitch;
    }
}
