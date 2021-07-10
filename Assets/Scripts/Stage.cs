using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using Coffee.UIExtensions;

public class Stage : MonoBehaviour
{
    [SerializeField]
    private Text txtStaminaPoint;

    [SerializeField]
    private Image[] imgOrbs;

    void Start()
    {
        // �X�^�~�i�̒l�̍w�ǊJ�n
        GameData.instance.staminaPoint.Subscribe(_ => UpdateDisplayStaminaPoint());
        
        // �I�[�u�̏��쐬
        for (int i = 0; i < imgOrbs.Length; i++) {
            GameData.instance.orbs.Add(i, false);
        }
        // �I�[�u�̍w�ǊJ�n
        GameData.instance.orbs.ObserveReplace().Subscribe((DictionaryReplaceEvent<int, bool> x) => UpdateDisplayOrbs(x.Key, x.NewValue));
    }

    /// <summary>
    /// �X�^�~�i�|�C���g�̕\���X�V
    /// </summary>
    private void UpdateDisplayStaminaPoint() {
        txtStaminaPoint.text = GameData.instance.staminaPoint.ToString();

        if (GameData.instance.staminaPoint.Value <= 0) {
            Debug.Log("�{�X��");

            // �w�ǒ�~
            GameData.instance.staminaPoint.Dispose();

            GameData.instance.orbs.Dispose();


            // �ړ��֎~


            // TODO �{�X�Ƃ̃o�g���V�[���֑J��
        }
    }

    /// <summary>
    /// �擾���Ă���I�[�u�̕\���X�V
    /// </summary>
    /// <param name="index"></param>
    /// <param name="isSwich"></param>
    public void UpdateDisplayOrbs(int index, bool isSwich) {

        Debug.Log(index);
        Debug.Log(isSwich);

        imgOrbs[index].color = isSwich ? Color.white : new Color(1, 1, 1, 0.5f);

        // �l�������ꍇ
        if (isSwich) {
            // ���鉉�o���Đ�
            imgOrbs[index].gameObject.GetComponent<ShinyEffectForUGUI>().Play();
        }
    }
}
