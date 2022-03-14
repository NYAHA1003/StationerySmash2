using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utill
{
    public class Parabola : MonoBehaviour
    {
        /// <summary>
        /// 최고점 계산
        /// </summary>
        /// <param name="v">초기 벡터</param>
        /// <param name="sin">사인 함수의 라디안값</param>
        /// <param name="multiple">배수</param>
        /// <returns></returns>
        static public float Caculated_Height(float v, float sin, float multiple = 1)
        {
            float height = ((v * v) * (Mathf.Sin(sin) * Mathf.Sin(sin)) / Mathf.Abs((Physics2D.gravity.y * 2))) * multiple;
            return height;
        }

        /// <summary>
        /// 수평 도달 거리 계산
        /// </summary>
        /// <param name="v">초기 벡터</param>
        /// <param name="sin">사인 함수의 라디안 값</param>
        /// <returns></returns>
        static public float Caculated_Width(float v, float sin)
        {
            return (v * v) * (Mathf.Sin(sin * 2)) / Mathf.Abs(Physics2D.gravity.y);
        }

        /// <summary>
        /// 최고점 도달 시간
        /// </summary>
        /// <param name="v">초기 벡터</param>
        /// <param name="sin">사인 함수의 라디안값</param>
        /// <param name="multiple">시간 배수. 1이면 최고점 도달 시간, 2이면 수평 도달 시간</param>
        /// <returns></returns>
        static public float Caculated_Time(float v, float sin, float multiple = 1)
        {
            return Mathf.Abs((v * Mathf.Sin(sin) / Mathf.Abs(Physics2D.gravity.y)) * multiple);
        }

        /// <summary>
        /// t초 후의 위치
        /// </summary>
        /// <param name="v">초기 벡터</param>
        /// <param name="sin">사인 함수의 라디안값</param>
        /// <param name="time">시간</param>
        /// <returns></returns>
        static public float Caculated_TimeToPos(float v, float sin, float time)
        {
            return (v * time * Mathf.Sin(sin)) - (Mathf.Abs(Physics2D.gravity.y / 2) * (time * time));
        }

        

        public static AnimationCurve Return_ParabolaCurve()
        {
            AnimationCurve curve = new AnimationCurve();
            Keyframe key1 = new Keyframe();
            Keyframe key2 = new Keyframe();
            Keyframe key3 = new Keyframe();
            Keyframe key4 = new Keyframe();
            Keyframe key5 = new Keyframe();


            key1.time = 0.0f;
            key1.value = 0.0f;
            key1.inTangent = 1.614829182624817f;
            key1.outTangent = 1.614829182624817f;
            key1.weightedMode = 0;
            key1.inWeight = 0.0f;
            key1.outWeight = 0.07500000298023224f;

            key2.time = 0.1992366760969162f;
            key2.value = 0.2643127739429474f;
            key2.inTangent = 0.9678702354431152f;
            key2.outTangent = 0.9678702354431152f;
            key2.weightedMode = 0;
            key2.inWeight = 0.3851872384548187f;
            key2.outWeight = 0.5799757242202759f;

            key3.time = 0.41378992795944216f;
            key3.value = 0.4281325340270996f;
            key3.inTangent = 0.591562807559967f;
            key3.outTangent = 0.591562807559967f;
            key3.weightedMode = 0;
            key3.inWeight = 0.3333333432674408f;
            key3.outWeight = 0.27802589535713198f;

            key4.time = 0.7986378073692322f;
            key4.value = 0.6723865270614624f;
            key4.inTangent = 0.7356034517288208f;
            key4.outTangent = 0.7356034517288208f;
            key4.weightedMode = 0;
            key4.inWeight = 0.3333333432674408f;
            key4.outWeight = 0.3333333432674408f;

            key5.time = 1.13702392578125f;
            key5.value = 1.0023574829101563f;
            key5.inTangent = 1.7282278537750245f;
            key5.outTangent = 1.7282278537750245f;
            key5.weightedMode = 0;
            key5.inWeight = 0.22246798872947694f;
            key5.outWeight = 0.0f;


            curve.AddKey(key1);
            curve.AddKey(key2);
            curve.AddKey(key3);
            curve.AddKey(key4);
            curve.AddKey(key5);

            return curve;
        }

        public static AnimationCurve Return_ScreenKoCurve()
        {
            AnimationCurve curve = new AnimationCurve();
            Keyframe key1 = new Keyframe();
            Keyframe key2 = new Keyframe();
            Keyframe key3 = new Keyframe();


            key1.time = 0.0f;
            key1.value = 0.0f;
            key1.inTangent = -0.008813612163066864f;
            key1.outTangent = -0.008813612163066864f;
            key1.weightedMode = 0;
            key1.inWeight = 0.0f;
            key1.outWeight = 0.5556594729423523f;

            key2.time = 0.9358735680580139f;
            key2.value = 0.3258655071258545f;
            key2.inTangent = 1.0313912630081177f;
            key2.outTangent = 1.0313912630081177f;
            key2.weightedMode = 0;
            key2.inWeight = 0.3333333432674408f;
            key2.outWeight = 0.3333333432674408f;

            key3.time = 1.0f;
            key3.value = 1.0f;
            key3.inTangent = 2.0f;
            key3.outTangent = 2.0f;
            key3.weightedMode = 0;
            key3.inWeight = 0.0f;
            key3.outWeight = 0.0f;


            curve.AddKey(key1);
            curve.AddKey(key2);

            return curve;
        }

    }
}