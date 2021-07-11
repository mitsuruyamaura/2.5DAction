using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using Coffee.UIExtensions;
using DG.Tweening;

public class Stage : MonoBehaviour
{
    [SerializeField]
    private Text txtStaminaPoint;

    [SerializeField]
    private Image[] imgOrbs;

    [SerializeField]
    private Slider sliderHp;

    [SerializeField]
    private Text txtHp;

    void Start()
    {
        // スタミナの値の購読開始
        GameData.instance.staminaPoint.Subscribe(_ => UpdateDisplayStaminaPoint());
        
        // オーブの情報作成
        for (int i = 0; i < imgOrbs.Length; i++) {
            GameData.instance.orbs.Add(i, false);
        }
        // オーブの購読開始
        GameData.instance.orbs.ObserveReplace().Subscribe((DictionaryReplaceEvent<int, bool> x) => UpdateDisplayOrbs(x.Key, x.NewValue));

        GameData.instance.maxHp = GameData.instance.hp;

        UpdateDisplayHp();

        
    }

    /// <summary>
    /// スタミナポイントの表示更新
    /// </summary>
    private void UpdateDisplayStaminaPoint() {
        txtStaminaPoint.text = GameData.instance.staminaPoint.ToString();

        if (GameData.instance.staminaPoint.Value <= 0) {
            Debug.Log("ボス戦");

            // 購読停止
            GameData.instance.staminaPoint.Dispose();

            GameData.instance.orbs.Dispose();


            // 移動禁止


            // TODO ボスとのバトルシーンへ遷移
        }
    }

    /// <summary>
    /// 取得しているオーブの表示更新
    /// </summary>
    /// <param name="index"></param>
    /// <param name="isSwich"></param>
    public void UpdateDisplayOrbs(int index, bool isSwich) {

        Debug.Log(index);
        Debug.Log(isSwich);

        imgOrbs[index].color = isSwich ? Color.white : new Color(1, 1, 1, 0.5f);

        // 獲得した場合
        if (isSwich) {
            // 光る演出を再生
            imgOrbs[index].gameObject.GetComponent<ShinyEffectForUGUI>().Play();
        }
    }

    /// <summary>
    /// Hp表示更新
    /// </summary>
    public void UpdateDisplayHp() {
        txtHp.text = GameData.instance.hp + "/ " + GameData.instance.maxHp;

        sliderHp.DOValue(GameData.instance.hp / GameData.instance.maxHp, 0.5f).SetEase(Ease.Linear);

        Debug.Log("Hp 表示更新");
    }
}
