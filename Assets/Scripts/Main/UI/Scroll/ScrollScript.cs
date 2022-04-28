using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace Main.Scroll
{
    public class ScrollScript : ScrollRect
    {
        //인스펙터 변수
        [SerializeField]
        private bool isHorizontalMove = false; //이 스크롤뷰타 수직으로 이동할 때, 그냥 인스펙터에서 안 보임 Debug로 열어서 수정

        //참조 변수
        private AgentScroll agentScroll = null;
        private ScrollRect parentScrollRect = null;

        //변수
        private bool isParentMove = false;

        protected override void Start()
        {
            //캐싱
            agentScroll = GetComponentInParent<AgentScroll>();
            parentScrollRect = agentScroll.GetComponent<ScrollRect>();
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            if (isHorizontalMove)
            {
                // 드래그 시작하는 순간 수직이동이 크면 부모가 드래그 시작한 것, 수직이동이 크면 자식이 드래그 시작한 것
                isParentMove = Mathf.Abs(eventData.delta.y) > Mathf.Abs(eventData.delta.x);
            }
            else
            {// 드래그 시작하는 순간 수평이동이 크면 부모가 드래그 시작한 것, 수직이동이 크면 자식이 드래그 시작한 것
                isParentMove = Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y);
            }

            if (isParentMove)
            {
                //부모 스크롤뷰 이동
                agentScroll.OnBeginDrag(eventData);
                parentScrollRect.OnBeginDrag(eventData);
            }
            else
            {
                //하위 스크롤뷰 이동
                base.OnBeginDrag(eventData);
            }
        }

        public override void OnDrag(PointerEventData eventData)
        {
            if (isParentMove)
            {
                //부모 스크롤뷰 이동
                agentScroll.OnDrag(eventData);
                parentScrollRect.OnDrag(eventData);
            }
            else
            {
                //하위 스크롤뷰 이동
                base.OnDrag(eventData);
            }

        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            if (isParentMove)
            {
                //부모 스크롤뷰 이동
                agentScroll.OnEndDrag(eventData);
                parentScrollRect.OnEndDrag(eventData);
            }
            else
            {
                //하위 스크롤뷰 이동
                base.OnEndDrag(eventData);
            }
        }
    }
}