using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// �I�u�W�F�N�g�v�[�������邽�߂̔ėp�̒��ۃN���X
/// </summary>
public abstract class PoolBase : MonoBehaviour {

    protected IObjectPool<PoolBase> objectPool;

    // ObjectPool �ւ̎Q�Ƃ�^����v���p�e�B
    public IObjectPool<PoolBase> ObjectPool { get => objectPool; set => objectPool = value; }
}