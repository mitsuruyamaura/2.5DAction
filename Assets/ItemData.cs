using System;
using System.Linq;

public enum Rarity {
    Common,
    Uncommon,
    Rare,
    Mystic,
    Epic,
    Legendary,
}

public enum ItemType {
    Sword,
    Axe,
    Bow,
    Ring,
    Spaer,
    Dagger,
    Armor,
    Shield,
    Postion,
}

public enum EffectType {
    Physical,  // �����n�̍U��
    Magic,     // ���@�n�̍U��
    Passive    // �������������œ�����p�b�V�u����
}

public enum StatusType {
    Strength,       // �́A�̗́A�ؗ�
    Intelligence,   // �m���A�m��
    Dexterity,      // ��p���A�f����
    Charm,          // ����
    Luck            // �^�̗ǂ�
}

[System.Serializable]
public class ItemData
{
    public int id;
    public string name;
    public Rarity rarity;
    public ItemType itemType;
    public string description;
    public int price;

    public float coolTime;
    public float accuracy;          // ������
    public int minValue;            // �ŏ��l�B�_���[�W�A�V�[���h�A��
    public int maxValue;            // �ő�l

    public int minAttackCount;      // 1�񓖂���̍U����
    public int maxAttackCount;

    public EffectType effectType;

    // �R�X�g�͔\�͒l�ƃZ�b�g�ɂ��� STR 5�A�̂悤�ɁBAP ����̊T�O�͂Ȃ���
    public StatusType[] statusTypes;
    public int[] requiredValues;

    public string effect;

    // �T�C�Y(�E�F�C�g)



    public ItemData(string[] datas) {
        id = int.Parse(datas[0]);
        name = datas[1];
        rarity = (Rarity)Enum.Parse(typeof(Rarity), datas[2]);
        itemType = (ItemType)Enum.Parse(typeof(ItemType), datas[3]);
        description = datas[4];
        price = int.Parse(datas[5]);

        coolTime = float.Parse(datas[6]);
        accuracy = float.Parse(datas[7]);
        minValue = int.Parse(datas[8]);
        maxValue = int.Parse(datas[9]);

        minAttackCount = int.Parse(datas[10]);
        maxAttackCount = int.Parse(datas[11]);
        effectType = (EffectType)Enum.Parse(typeof(EffectType), datas[12]);

        // ���p�X���b�V���ŋ�؂��Ĕz��ɕϊ�
        statusTypes = datas[13].Split('/').Select(type => (StatusType)Enum.Parse(typeof(StatusType), type)).ToArray();
        requiredValues = datas[14].Split('/').Select(int.Parse).ToArray();

        effect = datas.Length > 15 ? datas[15] : string.Empty;
    }
}