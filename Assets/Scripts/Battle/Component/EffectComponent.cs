using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using Battle.Effect;

namespace Battle
{
    [System.Serializable]
    public class EffectComponent : BattleComponent
    {
        [SerializeField]
        private Transform _effectPoolManager;
        [SerializeField]
        private List<GameObject> _effectObjectList;

        /// <summary>
        /// �ʱ�ȭ
        /// </summary>
        /// <param name="battleManager"></param>
        /// <param name="effect_PoolManager"></param>
        public void SetInitialization()
        {
            LoadParent();
            AllLoadAssetAsync();
        }
       
        /// <summary>
        /// ����Ʈ ����
        /// </summary>
        /// <param name="effectType">����Ʈ Ÿ��</param>
        /// <param name="position">����Ʈ ��ġ</param>
        /// <param name="startLifeTime">����Ʈ�� �����ð�</param>
        /// <param name="isSetLifeTime">����Ʈ ������ �� ������</param>
        public IEffect SetEffect(EffectType effectType, EffData effData)
        {
            Transform effect_Parent = _effectPoolManager.GetChild((int)effectType);
            EffectObject effect_Object = null;

            //������ �� ���� ����Ʈ ã��
            for (int i = 0; i < effect_Parent.childCount; i++)
            {
                effect_Object = effect_Parent.GetChild(i).GetComponent<EffectObject>();
                if (!effect_Object.gameObject.activeSelf)
                {
                    effect_Object.SetEffect(effData);
                    return effect_Object._effectState;
                }
            }

            if (IsLimitEffectType(effectType) && effect_Parent.childCount > 5)
            {
                float minLifeTime = 10;
                int minLifeIndex = 0;
                for (int i = 0; i < effect_Parent.childCount; i++)
                {
                    effect_Object = effect_Parent.GetChild(i).GetComponent<EffectObject>();
                    //���� ª�� lifeTime ã��
                    if (minLifeTime > effData.lifeTime)
                    {
                        minLifeTime = effData.lifeTime;
                        minLifeIndex = i;
                    }
                }

                effect_Object = effect_Parent.GetChild(minLifeIndex).GetComponent<EffectObject>();
                effect_Object.SetEffect(effData);
                return effect_Object._effectState;
            }
            //����Ʈ ������ ���� ����
            effect_Object = PoolManager.CreateObject(_effectObjectList[(int)effectType].gameObject, effData.pos, Quaternion.identity).GetComponent<EffectObject>();
            effect_Object.transform.SetParent(effect_Parent);
            effect_Object.SetEffect(effData);
            return effect_Object._effectState;
        }

        private bool IsLimitEffectType(EffectType effectType)
        {
            if (effectType == EffectType.Stun)
                return false;
            return true;
        }

        /// <summary>
        /// ���� ��������
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private async Task AwitLoadAssetAsync(string s)
        {
            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(s);
            await handle.Task;
            _effectObjectList.Add(handle.Result);
        }

        /// <summary>
        /// �θ� ����
        /// </summary>
        private void LoadParent()
        {
            for (int i = 0; i < System.Enum.GetValues(typeof(EffectType)).Length; i++)
            {
                GameObject childObj = new GameObject();
                string s = i + "_" + System.Enum.GetName(typeof(EffectType), i);
                childObj.name = s;
                childObj.transform.parent = _effectPoolManager.transform;
            }
        }

        /// <summary>
        /// ��� ���� ��������
        /// </summary>
        private async void AllLoadAssetAsync()
        {
            for (int i = 0; i < System.Enum.GetValues(typeof(EffectType)).Length; i++)
            {
                string s = System.Enum.GetName(typeof(EffectType), i);
                await AwitLoadAssetAsync(s);
            }
        }
    }

}