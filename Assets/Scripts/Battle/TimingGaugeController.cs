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
    /// ポインターのループ移動を開始
    /// </summary>
    public void MoveGaugePointer() {
        tween =slider.DOValue(1.0f, pointerDuration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }

    /// <summary>
    /// ポインタ―のループ移動を停止
    /// </summary>
    public void StopPointer() {
        tween.Kill();
    }

    /// <summary>
    /// ポインターの移動を一時停止
    /// </summary>
    /// <returns></returns>
    public IEnumerator PausePointer() {
        tween.Pause();

        yield return new WaitForSeconds(0.25f);

        ResumePointer();
    }

    /// <summary>
    /// ポインターの移動を再開
    /// </summary>
    public void ResumePointer() {
        tween.Play();
    }

    /// <summary>
    /// クリティカル判定とクリティカル(コンボ)数のカウント
    /// </summary>
    /// <returns></returns>
    public bool CheckCritial() {
        //Debug.Log(slider.value);

        // クリティカルの判定
        bool isCritical = slider.value >= 0.475f && slider.value < 0.540f ? true : false;

        // クリティカルしていたら
        if (isCritical) {
            battle.AddTotalBattleCount();
        }

        return isCritical;
    }
}
