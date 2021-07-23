/// <summary>
/// 
/// </summary>
public enum ConditionType
{
    Poison,           // 移動のたびにダメージ
    Hide_Symbols,     // すべてのシンボルが見えないが、通過すれば取れる。エネミーも戦闘になる
    View,             // 視界の増減 2.5 ～ 6.0f
    Untouchable,      // アイテムやオーブのシンボルが見えない上に取れない。エネミーのみ見える
    Walk_through,     // エネミーのシンボルを戦闘なしで通過できる
}
