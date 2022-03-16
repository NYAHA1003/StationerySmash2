using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utill
{
    public enum DieType
    {
        StarKo,
        ScreenKo,
        OutKo,
    }

    public class Die
    {
        /// <summary>
     /// 랜덤으로 죽는 유형을 반환
     /// </summary>
     /// <returns></returns>
        public static DieType Return_RandomDieType()
        {
            int random = Random.Range(0, 100);
            if (random < 10)
            {
                return DieType.StarKo;
            }
            if (random < 30)
            {
                return DieType.ScreenKo;
            }
            return DieType.OutKo;
        }
    }

}