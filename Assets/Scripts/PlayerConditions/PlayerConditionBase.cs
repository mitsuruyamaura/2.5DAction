using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// コンディションのベースクラス
/// </summary>
public class PlayerConditionBase : MonoBehaviour
{
    [SerializeField]  // Debug
    protected float conditionDuration;　　//　持続時間

    [SerializeField]  // Debug
    protected float conditionValue;       //  効果  =>  攻撃力を増減する値、マップの見える範囲を増減する値

    protected ConditionEffect conditionEffect;

    protected MapMoveController mapMoveController;
    protected SymbolManager symbolManager;

    protected ConditionType conditionType;   //  この情報が、コンディションの適用値になる

    /// <summary>
    /// コンディションをセットする際に呼び出す
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="value"></param>
    public void AddCondition(ConditionType conditionType, float duration, float value, MapMoveController mapMoveController, SymbolManager symbolManager) {
        this.conditionType = conditionType;
        conditionDuration = duration;
        conditionValue = value;
        this.mapMoveController = mapMoveController;
        this.symbolManager = symbolManager;

        OnEnterCondition();
    }

    /// <summary>
    /// コンディションの効果を適用
    /// </summary>
    /// <returns></returns>
    protected virtual void OnEnterCondition() {

        // 生成するエフェクトのプレファブを取得
        ConditionEffect conditionEffectPrefab = ConditionEffectManager.instance.GetConditionEffect(conditionType);
        Debug.Log(conditionEffectPrefab);

        // プレファブが取得できたら
        if (conditionEffectPrefab != null) {
            // エフェクト生成
            conditionEffect = Instantiate(conditionEffectPrefab, mapMoveController.GetConditionEffectTran());

            Debug.Log("エフェクト生成 : " + conditionType.ToString());
        }

        Debug.Log("コンディション付与");
    }

    /// <summary>
    /// コンディションが終了するときに呼び出す
    /// </summary>
    public void RemoveCondition() {
        OnExitCondition();
    }

    protected virtual void OnExitCondition() {

        if (conditionEffect != null) {
            // エフェクト破棄
            Destroy(conditionEffect.gameObject);
        }

        Debug.Log("コンディション削除");

        // コンディションの List から削除
        mapMoveController.RemoveConditionsList(this);
    }

    /// <summary>
    /// コンディションの残り時間の更新
    /// </summary>
    public virtual void CalcDuration() {

        // 持続時間を減少
        conditionDuration--;

        // コンディションの残り時間がなくなったら
        if (conditionDuration <= 0) {

            // コンディションを削除して終了する
            RemoveCondition();
        }
    }

    /// <summary>
    /// 設定されているコンディションの種類を取得
    /// </summary>
    /// <returns></returns>
    public ConditionType GetConditionType() {
        return conditionType;
    }

    /// <summary>
    /// 持続時間の延長と効果の上書き
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="value"></param>
    public void ExtentionCondition(float duration, float value) {
        conditionDuration += duration;
        conditionValue = value;

        // コンディションの効果を適用
        OnEnterCondition();
    }

    /// <summary>
    /// コンディションの効果値を取得
    /// </summary>
    /// <returns></returns>
    public float GetConditionValue() {
        return conditionValue;
    }

    /// <summary>
    /// コンディションの効果を適用
    /// </summary>
    public virtual void ApplyEffect() {

        // 毒のダメージ、攻撃力半減、移動速度半減などを適用する

        // 値を変化させる効果の場合は、持続時間経過後に OnExitCondition() を上書きして元の値に戻すこと

    }
}
