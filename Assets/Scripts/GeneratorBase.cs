using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// �����p���N���X
/// �G�A�t���[�g�\���A�A�C�e���Ȃǂɔėp�I�ɗ��p����݌v
/// </summary>
public abstract class GeneratorBase : MonoBehaviour {

    protected IObjectPool<PoolBase> objectPool;                 // �I�u�W�F�N�g�v�[��

#pragma warning disable 0649
    [SerializeField] protected int initialPoolSize = 5;         // �I�u�W�F�N�g�v�[���̏����T�C�Y
    [SerializeField] protected PoolBase objectPoolPrefab;       // �v�[��������v���n�u
#pragma warning restore 0649


    /// <summary>
    /// �����ݒ� 
    /// </summary>
    /// <param name="entityObject"></param>
    public virtual void InitObjectPool() {

        // ObjectPool �̏�����
        objectPool = new ObjectPool<PoolBase>(
            createFunc: () => Create(),
            actionOnGet: OnGetFromPool,
            actionOnRelease: target => target.gameObject.SetActive(false),
            actionOnDestroy: target => Destroy(target.gameObject),
            collectionCheck: true,
            defaultCapacity: 10,
            maxSize: 1000);
    }

    /// <summary>
    /// Get() ���\�b�h�ɂ��AcreateFunc �Ƃ��Ď��s�����
    /// </summary>
    /// <returns></returns>
    protected virtual PoolBase Create() {
        PoolBase objectPoolInstance = Instantiate(objectPoolPrefab);

        // �Q�Ƃ�^���Ă���(�ˑ�����������)���ƂŁA�t���[�g�\������ Release �ł���
        objectPoolInstance.ObjectPool = objectPool;

        //Debug.Log("����");
        return objectPoolInstance;
    }

    /// <summary>
    /// bulletPool.Get() ���\�b�h�ɂ��AactionOnGet �Ƃ��Ď��s�����
    /// </summary>
    /// <param name="target"></param>
    protected virtual void OnGetFromPool(PoolBase target) => target.gameObject.SetActive(true);

    /// <summary>
    /// �O���N���X�����s����
    /// Get() ���\�b�h�ɂ��A�I�u�W�F�N�g�v�[������e�����o���Ė߂� �� OnGetFromPool ���\�b�h�����s�����
    /// �v�[�����ɒe���Ȃ��ꍇ�ɂ͐V�����������Ė߂� �� Create ���\�b�h�����s�����
    /// </summary>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <returns></returns>
    public virtual PoolBase GetObjectFromPool(Vector3 position, Quaternion rotation) {
        PoolBase pooledObject = objectPool.Get();
        pooledObject.transform.position = position;
        pooledObject.transform.rotation = rotation;

        //Debug.Log("�擾");
        return pooledObject;
    }

    /// <summary>
    /// ��L���\�b�h�� UI �I�u�W�F�N�g�����p
    /// </summary>
    /// <param name="parentTran"></param>
    /// <returns></returns>
    public virtual PoolBase GetObjectFromPool(Transform parentTran) {
        PoolBase pooledObject = objectPool.Get();
        pooledObject.transform.SetParent(parentTran, false);
        pooledObject.transform.localScale = Vector3.one;

        //Debug.Log("�擾");
        return pooledObject;
    }
}