using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace Main.Scroll
{
    public class ScrollScript : ScrollRect
    {
        //�ν����� ����
        [SerializeField]
        private bool isHorizontalMove = false; //�� ��ũ�Ѻ�Ÿ �������� �̵��� ��, �׳� �ν����Ϳ��� �� ���� Debug�� ��� ����

        //���� ����
        private AgentScroll agentScroll = null;
        private ScrollRect parentScrollRect = null;

        //����
        private bool isParentMove = false;

        protected override void Start()
        {
            //ĳ��
            agentScroll = GetComponentInParent<AgentScroll>();
            parentScrollRect = agentScroll.GetComponent<ScrollRect>();
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            if (isHorizontalMove)
            {
                // �巡�� �����ϴ� ���� �����̵��� ũ�� �θ� �巡�� ������ ��, �����̵��� ũ�� �ڽ��� �巡�� ������ ��
                isParentMove = Mathf.Abs(eventData.delta.y) > Mathf.Abs(eventData.delta.x);
            }
            else
            {// �巡�� �����ϴ� ���� �����̵��� ũ�� �θ� �巡�� ������ ��, �����̵��� ũ�� �ڽ��� �巡�� ������ ��
                isParentMove = Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y);
            }

            if (isParentMove)
            {
                //�θ� ��ũ�Ѻ� �̵�
                agentScroll.OnBeginDrag(eventData);
                parentScrollRect.OnBeginDrag(eventData);
            }
            else
            {
                //���� ��ũ�Ѻ� �̵�
                base.OnBeginDrag(eventData);
            }
        }

        public override void OnDrag(PointerEventData eventData)
        {
            if (isParentMove)
            {
                //�θ� ��ũ�Ѻ� �̵�
                agentScroll.OnDrag(eventData);
                parentScrollRect.OnDrag(eventData);
            }
            else
            {
                //���� ��ũ�Ѻ� �̵�
                base.OnDrag(eventData);
            }

        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            if (isParentMove)
            {
                //�θ� ��ũ�Ѻ� �̵�
                agentScroll.OnEndDrag(eventData);
                parentScrollRect.OnEndDrag(eventData);
            }
            else
            {
                //���� ��ũ�Ѻ� �̵�
                base.OnEndDrag(eventData);
            }
        }
    }
}