/// <summary>
/// 
/// </summary>
public enum ConditionType
{
    Poison,           // 移動のたびにダメージ
    Hide_Symbols,     // すべてのシンボルが見えない
    View,             // 視界の増減 2.5 ～ 6.0f
    Dont_Take,        // アイテムやオーブのシンボルが取れない。エネミーのみ有効
    Walk_through,     // コライダーのある地形を通過できる
}
