using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
namespace Battle
{
    [System.Serializable]
    public class EffectCommand
    {
        [SerializeField]
        private Transform _effectPoolManager;
        [SerializeField]
        private List<GameObject> _effectObjectList;
 
        
        public async Task AwitLoadAssetAsync(string s)
        {
            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(s);
            await handle.Task;
            _effectObjectList.Add(handle.Result);
        }
        /// <summary>
        /// �ʱ�ȭ
        /// </summary>
        /// <param name="battleManager"></param>
        /// <param name="effect_PoolManager"></param>
        public void SetInitialization()
        {
            AllLoadAssetAsync();
        }
        public async void AllLoadAssetAsync()
        {
            for (int i = 0; i < System.Enum.GetValues(typeof(EffectType)).Length; i++)
            {
                string s = System.Enum.GetName(typeof(EffectType), i);
                await AwitLoadAssetAsync(s);
            }
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

            //������ ���� ����
            effect_Object = PoolManager.CreateObject(_effectObjectList[(int)effectType].gameObject, effData.pos, Quaternion.identity).GetComponent<EffectObject>();
            effect_Object.transform.SetParent(effect_Parent);
            effect_Object.SetEffect(effData);
            return effect_Object._effectState;
        }
    }

}