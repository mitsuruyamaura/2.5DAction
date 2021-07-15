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

    [SerializeField]
    private Text txtExp;

    [SerializeField]
    private Text txtPlayerLevel;

    [SerializeField]
    private ShinyEffectForUGUI shinyEffectImgPlayerLevelFrame;

    [SerializeField]
    private Slider sliderExp;

    private float sliderAnimeDuration = 0.5f;

    int levelupCount;

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

        //GameData.instance.maxHp = GameData.instance.hp;

        // Hp�\���X�V
        StartCoroutine(UpdateDisplayHp());

        // �v���C���[���x���ƌo���l�̕\���X�V
        UpdateDisplayPlayerLevel();
        UpdateDisplayExp(true);
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

    /// <summary>
    /// Hp�\���X�V
    /// </summary>
    public IEnumerator UpdateDisplayHp(float waitTime = 0.0f) {
        txtHp.text = GameData.instance.hp + "/ " + GameData.instance.maxHp;

        yield return new WaitForSeconds(waitTime);

        sliderHp.DOValue((float)GameData.instance.hp / GameData.instance.maxHp, sliderAnimeDuration).SetEase(Ease.Linear);

        Debug.Log("Hp �\���X�V");
    }

    private void OnEnable() {

        // �o�g���O�� Hp ����A�j�����ĕ\�����邽�߂ɑҋ@���Ԃ����
        StartCoroutine(UpdateDisplayHp(1.0f));

        // �o�g����Ƀ��x���A�b�v�������̃J�E���g�̏�����
        levelupCount = 0;
     
        // ���x���A�b�v���邩�m�F
        CheckExpNextLevel();

        // ���x���A�b�v���Ă�����
        if (levelupCount > 0) {

            Debug.Log("���x���A�b�v�̃{�[�i�X����");

            // ���x���A�b�v�̃{�[�i�X

        }
    }

    /// <summary>
    /// ���x���A�b�v���邩�m�F
    /// </summary>
    public void CheckExpNextLevel() {

        // ���݂̌o���l�Ǝ��̃��x���ɕK�v�Ȍo���l���ׂāA���x�����オ�邩�m�F
        if (GameData.instance.totalExp < DataBaseManager.instance.CalcNextLevelExp(GameData.instance.playerLevel -1)) {
            // �B���Ă��Ȃ��ꍇ�ɂ͌o���l�ƃQ�[�W�X�V
            UpdateDisplayExp(true);

            // �����I��
            return;
        } else {
            // �B���Ă���ꍇ�ɂ̓��x���A�b�v
            GameData.instance.playerLevel++;
            levelupCount++;

            Debug.Log("���x���A�b�v�I ���݂̃��x�� : " + GameData.instance.playerLevel);

            // ���x���A�b�v���o
            shinyEffectImgPlayerLevelFrame.Play();

            // �v���C���[���x���ƌo���l�̕\���X�V
            UpdateDisplayPlayerLevel();
            UpdateDisplayExp(false);

            // ����Ƀ��x�����オ�邩�ċA�������s���Ċm�F
            CheckExpNextLevel();
        }
    }

    /// <summary>
    /// �v���C���[���x���̕\���X�V
    /// </summary>
    private void UpdateDisplayPlayerLevel() {
        // �v���C���[���x���̕\���X�V
        txtPlayerLevel.text = GameData.instance.playerLevel.ToString();
    }

    /// <summary>
    /// �o���l�̕\���X�V
    /// </summary>
    /// <param name="isSliderOn"></param>
    private void UpdateDisplayExp(bool isSliderOn) {
        // ����/�ڕW�o���l�̕\���X�V
        txtExp.text = GameData.instance.totalExp + " / " + DataBaseManager.instance.CalcNextLevelExp(GameData.instance.playerLevel - 1);

        if (isSliderOn) {
            // �Q�[�W�X�V
            sliderExp.DOValue((float)GameData.instance.totalExp / DataBaseManager.instance.CalcNextLevelExp(GameData.instance.playerLevel - 1), 1.0f).SetEase(Ease.Linear);
        }
    }
}
