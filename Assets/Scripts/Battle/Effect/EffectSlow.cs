using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using Battle;

namespace Battle.Effect
{
    public class EffectSlow : EffectStun
    {
        public override void Update_Effect(EffectObject effObj, EffData effData)
        {
            this.effObj.transform.position = new Vector3(effData.trm.position.x, effData.trm.position.y, 0);
        }
    }

}