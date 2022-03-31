using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

namespace Battle
{
    public class EffectCommand : BattleCommand
    {

        private Transform _effectPoolManager;

        /// <summary>
        /// 초기화
        /// </summary>
        /// <param name="battleManager"></param>
        /// <param name="effect_PoolManager"></param>
        public void SetInitialization(BattleManager battleManager, Transform effect_PoolManager)
        {
            this._battleManager = battleManager;
            this._effectPoolManager = effect_PoolManager;
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

            //없으면 새로 만듦
            effect_Object = _battleManager.CreateObject(_battleManager._effectObjectList[(int)effectType].gameObject, effData.pos, Quaternion.identity).GetComponent<EffectObject>();
            effect_Object.transform.SetParent(effect_Parent);
            effect_Object.SetEffect(effData);
            return effect_Object._effectState;
        }
    }

}