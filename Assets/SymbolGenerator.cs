using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SymbolGenerator : GeneratorBase, ISetup {

    // エフェクトデータのリスト
    public SymbolDataSO symbolDataSO;

    // エフェクトのオブジェクトプールを管理するディクショナリー。Key は SymbolType 型。Value は IObjectPool 型
    private Dictionary<SymbolType, IObjectPool<PoolBase>> symbolPools = new();


    public void SetUp(GameObject entityObject = null) {
        InitObjectPool();
    }

    public override void InitObjectPool() {
        foreach (SymbolData symbolData in symbolDataSO.symbolDataList) {
            if (symbolData == null) {
                continue;
            }

            // Dictionary 用の値として オブジェクトプールを設定
            IObjectPool<PoolBase> pool = CreateObjectPool(symbolData);

            // Dictionary に追加
            symbolPools.Add(symbolData.symbolType, pool);
        }
    }

    /// <summary>
    /// 各シンボル用のオブジェクトプールを初期化して作成
    /// </summary>
    /// <param name="effectPrefab"></param>
    /// <returns></returns>
    private IObjectPool<PoolBase> CreateObjectPool(SymbolData symbolData) {
        // オブジェクトプールの初期化
        IObjectPool<PoolBase> pool = new ObjectPool<PoolBase>(
            createFunc: () => CreateSymbol(symbolData),
            actionOnGet: OnGetFromPool,
            actionOnRelease: target => target.gameObject.SetActive(false),
            actionOnDestroy: target => Destroy(target.gameObject),
            collectionCheck: true,
            defaultCapacity: 10,
            maxSize: 1000);

        return pool;
    }

    /// <summary>
    /// Get メソッド実行時、プールされたオブジェクトがない場合に実行し、エフェクトを生成して返す
    /// </summary>
    /// <param name="effectPrefab"></param>
    /// <returns></returns>
    private SymbolBase CreateSymbol(SymbolData symbolData) {
        // eff_indexに対応するオブジェクトプールをDictionaryから取得
        IObjectPool<PoolBase> effectPool = symbolPools[symbolData.symbolType];

        // エフェクトを生成してオブジェクトプールに所属させる
        SymbolBase symbolBase = Instantiate(symbolData.sumbolPrefab);
        symbolBase.transform.SetParent(transform);

        // 戻る先のプールを設定する
        symbolBase.ObjectPool = effectPool;
        return symbolBase;
    }


    /// <summary>
    /// 外部から呼び出すメソッド
    /// オブジェクトプールからシンボルを取得して返す
    /// </summary>
    /// <param name="effIndex"></param>
    /// <returns></returns>
    public SymbolBase GetSymbol(SymbolType symbolType) {

        // エフェクトが所属するオブジェクトプールを探す
        if (symbolPools.ContainsKey(symbolType)) {

            // 見つかったオブジェクトプールを指定
            IObjectPool<PoolBase> effectPool = symbolPools[symbolType];

            // 指定されたオブジェクトプール内から、指定されたシンボルを取得。なければ生成
            SymbolBase pooledSymbol = (SymbolBase)effectPool.Get();

            return pooledSymbol;
        } else {
            Debug.Log($"指定された SymbolType は登録がありません。{symbolType}");
            return null;
        }
    }
}