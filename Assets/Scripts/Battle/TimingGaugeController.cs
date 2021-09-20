using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TimingGaugeController : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    [SerializeField]
    private float pointerDuration;

    private Tween tween;
    private Battle battle;

    public void SetUpTimingGaugeController(Battle battle)
    {
        this.battle = battle;
        MoveGaugePointer();
    }

    /// <summary>
    /// �|�C���^�[�̃��[�v�ړ����J�n
    /// </summary>
    public void MoveGaugePointer() {
        tween =slider.DOValue(1.0f, pointerDuration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }

    /// <summary>
    /// �|�C���^�\�̃��[�v�ړ����~
    /// </summary>
    public void StopPointer() {
        tween.Kill();
    }

    /// <summary>
    /// �|�C���^�[�̈ړ����ꎞ��~
    /// </summary>
    /// <returns></returns>
    public IEnumerator PausePointer() {
        tween.Pause();

        yield return new WaitForSeconds(0.25f);

        ResumePointer();
    }

    /// <summary>
    /// �|�C���^�[�̈ړ����ĊJ
    /// </summary>
    public void ResumePointer() {
        tween.Play();
    }

    /// <summary>
    /// �N���e�B�J������ƃN���e�B�J��(�R���{)���̃J�E���g
    /// </summary>
    /// <returns></returns>
    public bool CheckCritial() {
        //Debug.Log(slider.value);

        // �N���e�B�J���̔���
        bool isCritical = slider.value >= 0.475f && slider.value < 0.540f ? true : false;

        // �N���e�B�J�����Ă�����
        if (isCritical) {
            battle.AddTotalBattleCount();
        }

        return isCritical;
    }
}
