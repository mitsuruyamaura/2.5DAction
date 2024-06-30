using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SymbolGenerator : GeneratorBase, ISetup {

    // �G�t�F�N�g�f�[�^�̃��X�g
    public SymbolDataSO symbolDataSO;

    // �G�t�F�N�g�̃I�u�W�F�N�g�v�[�����Ǘ�����f�B�N�V���i���[�BKey �� SymbolType �^�BValue �� IObjectPool �^
    private Dictionary<SymbolType, IObjectPool<PoolBase>> symbolPools = new();


    public void SetUp(GameObject entityObject = null) {
        InitObjectPool();
    }

    public override void InitObjectPool() {
        foreach (SymbolData symbolData in symbolDataSO.symbolDataList) {
            if (symbolData == null) {
                continue;
            }

            // Dictionary �p�̒l�Ƃ��� �I�u�W�F�N�g�v�[����ݒ�
            IObjectPool<PoolBase> pool = CreateObjectPool(symbolData);

            // Dictionary �ɒǉ�
            symbolPools.Add(symbolData.symbolType, pool);
        }
    }

    /// <summary>
    /// �e�V���{���p�̃I�u�W�F�N�g�v�[�������������č쐬
    /// </summary>
    /// <param name="effectPrefab"></param>
    /// <returns></returns>
    private IObjectPool<PoolBase> CreateObjectPool(SymbolData symbolData) {
        // �I�u�W�F�N�g�v�[���̏�����
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
    /// Get ���\�b�h���s���A�v�[�����ꂽ�I�u�W�F�N�g���Ȃ��ꍇ�Ɏ��s���A�G�t�F�N�g�𐶐����ĕԂ�
    /// </summary>
    /// <param name="effectPrefab"></param>
    /// <returns></returns>
    private SymbolBase CreateSymbol(SymbolData symbolData) {
        // eff_index�ɑΉ�����I�u�W�F�N�g�v�[����Dictionary����擾
        IObjectPool<PoolBase> effectPool = symbolPools[symbolData.symbolType];

        // �G�t�F�N�g�𐶐����ăI�u�W�F�N�g�v�[���ɏ���������
        SymbolBase symbolBase = Instantiate(symbolData.sumbolPrefab);
        symbolBase.transform.SetParent(transform);

        // �߂��̃v�[����ݒ肷��
        symbolBase.ObjectPool = effectPool;
        return symbolBase;
    }


    /// <summary>
    /// �O������Ăяo�����\�b�h
    /// �I�u�W�F�N�g�v�[������V���{�����擾���ĕԂ�
    /// </summary>
    /// <param name="effIndex"></param>
    /// <returns></returns>
    public SymbolBase GetSymbol(SymbolType symbolType) {

        // �G�t�F�N�g����������I�u�W�F�N�g�v�[����T��
        if (symbolPools.ContainsKey(symbolType)) {

            // ���������I�u�W�F�N�g�v�[�����w��
            IObjectPool<PoolBase> effectPool = symbolPools[symbolType];

            // �w�肳�ꂽ�I�u�W�F�N�g�v�[��������A�w�肳�ꂽ�V���{�����擾�B�Ȃ���ΐ���
            SymbolBase pooledSymbol = (SymbolBase)effectPool.Get();

            return pooledSymbol;
        } else {
            Debug.Log($"�w�肳�ꂽ SymbolType �͓o�^������܂���B{symbolType}");
            return null;
        }
    }
}