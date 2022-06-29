using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Utill.Data;
using Utill.Tool;
using Main.Event;
using TMPro; 

namespace Main.Scroll
{
    public class ShopScroll : AgentScroll
    {
        [SerializeField]
        private Scrollbar[] yScrollBars;
        [SerializeField]
        private Slider accentSlider;
        [SerializeField]
        private TextMeshProUGUI[] movePanelBtnText; 
        protected override void SettingAwake()
        {
            base.SettingAwake();

            EventManager.Instance.StartListening(EventsType.MoveShopPn, OnMoveShopPanel);
            EventManager.Instance.StartListening(EventsType.CloaseAllPn, SetOriginScroll);
            EventManager.Instance.StartListening(EventsType.SetOriginShopPn, SetOriginScroll);
        }

        protected override void Start()
        {
            base.Start();
            _targetIndex = Size -1;
            _targetPos = _pos[0]; 
        }
        protected override void SettingUpdate()
        {
            base.SettingUpdate();

            accentSlider.value = Mathf.Lerp(accentSlider.value, _scrollbar.value, 0.2f);
        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);

            if (_curPos == _targetPos)
            {
                DeltaSlide(eventData.delta.x);
            }
            SetOriginScroll(); 
            ChangeBtnSize();
        }
        /// <summary>
        /// ���ڰ���° ���� �г� �̵�
        /// </summary>
        /// <param name="n"></param>
        public void OnMoveShopPanel(object n)
        {
            _targetIndex = Size - (int)n - 1;
            _targetPos = _pos[(int)n];
            SetOriginScroll();
            ChangeBtnSize();
            Debug.Log(_pos[(int)n]);
        }

        /// <summary>
        /// ���� ��ũ�� value �ʱ�ȭ  
        /// </summary>
        void SetOriginScroll()
        {
            Debug.Log("����");
            for (int i = 0; i < Size; i++)
            {
                if (_contentTrm.GetChild(i).GetComponent<ScrollScript>())
                {
                    yScrollBars[i].value = 1;
                }
            }
        }

        /// <summary>
        ///  ��ܿ� �г� �ٲٴ� ��ư ũ�� ���� 
        /// </summary>
        void ChangeBtnSize()
        {
            for (int i = 0; i < Size; i++)
            {
                movePanelBtnText[i].fontSize = (_targetIndex == Size - i - 1) ? 60 : 36;  
            }
        }


    }
}