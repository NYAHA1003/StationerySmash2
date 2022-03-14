using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utill
{
    public enum eState  // 가질 수 있는 상태 나열
    {
        IDLE, MOVE, ATTACK, WAIT, DAMAGED, DIE, PULL, THROW, NONE,
    };

    public enum eEvent  // 이벤트 나열
    {
        ENTER, UPDATE, EXIT, NOTHING
    };
}
