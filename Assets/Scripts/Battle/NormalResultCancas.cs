using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class NormalResultCancas : MonoBehaviour
{
    [SerializeField]
    private Text txtExp;

    [SerializeField]
    private Text txtTotalComboCount;

    [SerializeField]
    private RectTransform imgBackFrameRect;

    [SerializeField]
    private CanvasGroup canvasGroupResult;

    [SerializeField]
    private CanvasGroup canvasGroupExpSet;

    [SerializeField]
    private CanvasGroup canvasGroupTotalComboCountSet;

    /// <summary>
    /// ���U���g�\��
    /// </summary>
    /// <param name="exp"></param>
    /// <param name="totalComboCount"></param>
    public void DisplayResult(int exp, int totalComboCount) {

        // ��U���ׂĔ�\���ɂ���
        canvasGroupResult.alpha = 0;
        canvasGroupExpSet.alpha = 0;
        canvasGroupTotalComboCountSet.alpha = 0;
        imgBackFrameRect.sizeDelta = new Vector2(0, 355);

        Sequence sequence = DOTween.Sequence();

        // Canvas �\���BResult �̕��������o��
        sequence.Append(canvasGroupResult.DOFade(1.0f, 0.5f).SetEase(Ease.Linear));

        // �t���[���\��
        sequence.Append(imgBackFrameRect.DOSizeDelta(new Vector2(2000, 355), 0.5f).SetEase(Ease.OutQuart));

        // EXP �\��
        sequence.Append(canvasGroupExpSet.DOFade(1.0f, 0.5f));

        // EXP ���Z�A�j��
        sequence.Append(txtExp.DOCounter(0, exp, 0.5f).SetEase(Ease.InQuart));

        // �R���{�� �\��
        sequence.Append(canvasGroupTotalComboCountSet.DOFade(1.0f, 0.5f));

        // �R���{�� ���Z�A�j��
        sequence.Append(txtTotalComboCount.DOCounter(0, totalComboCount, 0.5f).SetEase(Ease.InQuart));
    }
}
