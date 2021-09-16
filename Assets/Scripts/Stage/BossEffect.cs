using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BossEffect : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup canvasGroupMain;

    [SerializeField]
    private Image imgLogo;

    /// <summary>
    /// �{�X�o���G�t�F�N�g�̍Đ�
    /// </summary>
    /// <returns></returns>
    public IEnumerator PlayEffect() {

        canvasGroupMain.alpha = 0;

        imgLogo.color = new Color(1.0f, 1.0f, 1.0f, 0);

        Sequence sequence = DOTween.Sequence();

        sequence.Append(canvasGroupMain.DOFade(1.0f, 0.75f));
        Tween tween = sequence.Append(imgLogo.DOFade(1.0f, 1.0f).SetEase(Ease.Linear).SetLoops(3, LoopType.Yoyo));
        sequence.Append(canvasGroupMain.DOFade(0f, 0.75f)).OnComplete(() => { tween.Kill(); });

        yield return new WaitForSeconds(5.0f);

        Destroy(gameObject);
    }
}
