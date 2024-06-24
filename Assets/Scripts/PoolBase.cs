using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// オブジェクトプール化するための汎用の抽象クラス
/// </summary>
public abstract class PoolBase : MonoBehaviour {

    protected IObjectPool<PoolBase> objectPool;

    // ObjectPool への参照を与えるプロパティ
    public IObjectPool<PoolBase> ObjectPool { get => objectPool; set => objectPool = value; }
}