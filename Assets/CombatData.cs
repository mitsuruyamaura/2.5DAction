using UniRx;

/// <summary>
/// プレイヤー、敵共通
/// </summary>
[System.Serializable]
public class CombatData {
    public ReactiveProperty<int> Hp = new();

    public int inventorySize;


    // インベントリ

    // バフ・デバフ


    public CombatData(int hp, int defaultInventorySize) {
        Hp = new(hp);
        inventorySize = defaultInventorySize;
    }
}