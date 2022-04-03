using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

namespace Battle
{
    [System.Serializable]
    public class EffectCommand : BattleCommand
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
        public void SetInitialization(BattleManager battleManager)
        {
            this._battleManager = battleManager;
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
            effect_Object = _battleManager.CreateObject(_effectObjectList[(int)effectType].gameObject, effData.pos, Quaternion.identity).GetComponent<EffectObject>();
            effect_Object.transform.SetParent(effect_Parent);
            effect_Object.SetEffect(effData);
            return effect_Object._effectState;
        }
    }

}