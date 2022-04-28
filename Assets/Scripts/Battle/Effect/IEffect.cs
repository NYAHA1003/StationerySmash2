using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
namespace Battle.Effect
{
    public interface IEffect
    {
        public void Update_Effect(EffectObject effObj, EffData effData);
        public void Set_Effect(EffectObject effObj, EffData effData);

        public void Delete_Effect();
    }

}