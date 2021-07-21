using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// コンディションのベースクラス
/// </summary>
public class PlayerConditionBase : MonoBehaviour
{
    [SerializeField]  // Debug
    protected float conditionDuration;

    [SerializeField]  // Debug
    protected float conditionValue;

    protected MapMoveController mapMoveController;

    /// <summary>
    /// コンディションをセットする際に呼び出す
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="value"></param>
    public void AddCondition(float duration, float value, MapMoveController mapMoveController) {
        conditionDuration = duration;
        conditionValue = value;
        this.mapMoveController = mapMoveController;

        StartCoroutine(OnEnterCondition());
    }

    protected virtual IEnumerator OnEnterCondition() {
        yield return null;

        Debug.Log("コンディション付与");
    }

    /// <summary>
    /// コンディションが終了するときに呼び出す
    /// </summary>
    public void RemoveCondition() {
        StartCoroutine(OnExitCondition());
    }

    protected virtual IEnumerator OnExitCondition() {
        yield return null;

        Debug.Log("コンディション削除");

        // コンディションの List から削除
        mapMoveController.RemoveConditionsList(this);
    }

    /// <summary>
    /// コンディションの残り時間の更新
    /// </summary>
    public virtual void CalcDuration() {

        // コンディションの残り時間がなくなったら
        if (conditionDuration <= 0) {

            // コンディションを削除して終了する
            RemoveCondition();
        }
    }
}
