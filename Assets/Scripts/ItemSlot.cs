using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : PoolBase {
    public Image bottomImage;     // �������̔w�i�摜
    public Image topImage;        // �Q�[�W�摜�i�t�F�[�h�C������摜�j
    public RectTransform icon;    // �A�C�R����Transform

    public float cooldownTime = 5f; // �N�[���_�E������
    public float scaleDuration = 0.2f; // �X�P�[���A�j���[�V�����̎�������
    public Vector3 scaleAmount = new Vector3(1.2f, 1.2f, 1.2f); // �X�P�[���{��

    private Tweener fadeTweener;  // Tween �I�u�W�F�N�g���Ǘ�����ϐ�

    void Start() {
        // ������Ԃŏ�̉摜�𓧖��ɐݒ�
        Color color = topImage.color;
        color.a = 0;
        topImage.color = color;

        StartCooldownAnimation();
    }

    void StartCooldownAnimation() {
        // �N�[���_�E���A�j���[�V�������J�n
        fadeTweener = topImage.DOFade(1, cooldownTime)
            .OnComplete(OnGaugeFull)
            .SetLoops(-1, LoopType.Restart)
            .SetLink(gameObject); // �I�u�W�F�N�g�̔j��ɕR�Â�
    }

    void OnGaugeFull() {
        icon.DOScale(scaleAmount, scaleDuration)
            .SetEase(Ease.OutBack)
            .OnComplete(() => icon.DOScale(Vector3.one, scaleDuration).SetEase(Ease.InBack));
    }

    public void PauseCooldownAnimation() {
        if (fadeTweener != null && fadeTweener.IsPlaying()) {
            fadeTweener.Pause();
        }
    }

    public void ResumeCooldownAnimation() {
        if (fadeTweener != null && !fadeTweener.IsPlaying()) {
            fadeTweener.Play();
        }
    }

    public void StopCooldownAnimation() {
        if (fadeTweener != null) {
            fadeTweener.Kill();
        }
    }

    private void OnDestroy() {
        // �I�u�W�F�N�g���j�󂳂ꂽ�Ƃ��� Tween ���~
        StopCooldownAnimation();
    }

    private void UseEffect() {
        Debug.Log("�A�C�e���̌��ʂ��������܂����I");
        // �����ŃA�C�e���̎��ۂ̌��ʂ��������܂�
    }
}
