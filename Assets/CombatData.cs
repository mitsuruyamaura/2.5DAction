using UniRx;

/// <summary>
/// �v���C���[�A�G����
/// </summary>
[System.Serializable]
public class CombatData {
    public ReactiveProperty<int> Hp = new();

    public int inventorySize;


    // �C���x���g��

    // �o�t�E�f�o�t


    public CombatData(int hp, int defaultInventorySize) {
        Hp = new(hp);
        inventorySize = defaultInventorySize;
    }
}