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
        /// 초기화
        /// </summary>
        /// <param name="battleManager"></param>
        /// <param name="effect_PoolManager"></param>
        public void SetInitialization()
        {
            LoadParent();
            AllLoadAssetAsync();
        }
       
        /// <summary>
        /// 이펙트 설정
        /// </summary>
        /// <param name="effectType">이펙트 타입</param>
        /// <param name="position">이펙트 위치</param>
        /// <param name="startLifeTime">이펙트의 유지시간</param>
        /// <param name="isSetLifeTime">이펙트 설정을 할 것인지</param>
        public IEffect SetEffect(EffectType effectType, EffData effData)
        {
            Transform effect_Parent = _effectPoolManager.GetChild((int)effectType);
            EffectObject effect_Object = null;

            //재사용할 수 있을 이펙트 찾기
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
                    //제일 짧은 lifeTime 찾기
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
            //이펙트 없으면 새로 만듦
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
        /// 에셋 가져오기
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
        /// 부모 생성
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
        /// 모든 에셋 가져오기
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