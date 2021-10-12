using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TreasurePopUp : PopUpBase
{
    [SerializeField]
    private Button btnFilter;

    [SerializeField]
    private Transform raritySetTran;

    [SerializeField]
    private Image imgTreasureIcon;

    [SerializeField]
    private Text txtTreasureName;

    [SerializeField]
    private RarityDetail rarityDetailPrefab;

    /// <summary>
    /// �ݒ肵�ă|�b�v�A�b�v���J��
    /// </summary>
    /// <param name="itemData"></param>
    public void DisplayPopUp(AbilityItemDataSO.AbilityItemData itemData) {

        canvasGroup.alpha = 0;

        btnFilter.onClick.AddListener(OnExitPopUp);
        btnFilter.interactable = false;

        imgTreasureIcon.sprite = itemData.abilitySprite;
        txtTreasureName.text = itemData.abilityName;

        canvasGroup.DOFade(1.0f, 0.5f).SetEase(Ease.Linear).OnComplete(() => { StartCoroutine(CreateRarityDetails(itemData.rarity)); });
    }

    /// <summary>
    /// ���A���e�B�̃A�C�R�����쐬
    /// </summary>
    /// <param name="rarity"></param>
    /// <returns></returns>
    private IEnumerator CreateRarityDetails(int rarity) {

        for (int i = 0; i < rarity + 1; i++) {

            RarityDetail rarityDetail = Instantiate(rarityDetailPrefab, raritySetTran);
            rarityDetail.PlayAnim();

            yield return new WaitForSeconds(0.55f * (i + 1));
        }
        btnFilter.interactable = true;
    }

    public override void OnExitPopUp() {
        base.OnExitPopUp();

        ClosePopUp();
    }

    /// <summary>
    /// �|�b�v�A�b�v�����
    /// </summary>
    private void ClosePopUp() {

        canvasGroup.DOFade(0, 0.5f).SetEase(Ease.Linear).OnComplete(() => { Destroy(gameObject); });

    }
}
