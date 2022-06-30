using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utill.Data
{
    public enum EffectType
    {
        Attack,
        Stun,
        Ink,
        Slow,
        Throw,
    }
    public struct EffData
    {
        public Vector2 pos;
        public float lifeTime;
        public Transform trm;
        public Unit unit;
        public EffData(Unit unit,Vector2 pos, float lifeTime = 0.5f, Transform trm = null)
        {
            this.pos = pos;
            this.lifeTime = lifeTime;
            this.trm = trm;
            this.unit = unit;
        }

    }

}