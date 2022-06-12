using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Utill.Data;
using Utill.Tool;
using Main.Event;

namespace Main.Scroll
{
    public class MainScroll : AgentScroll
    {
        [SerializeField]
        private Slider accentSlider;
        [SerializeField]
        private RectTransform[] panelIcons;

        protected override void SettingAwake()
        {
            base.SettingAwake();
            EventManager.Instance.StartListening(EventsType.MoveMainPn,(x) => OnMoveMainPanel((int)x));
        }
        protected override void SettingStart()
        {
            base.SettingStart();
        }
        protected override void SettingUpdate()
        {
            base.SettingUpdate();
            accentSlider.value = Mathf.Lerp(accentSlider.value, _scrollbar.value, 0.2f);
        }

        /// <summary>
        /// �巡�װ� ������ �ߵ�
        /// </summary>
        /// <param name="eventData"></param>
        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            if (_curPos == _targetPos)
            {
                if(DeltaSlide(eventData.delta.y) == true)
                {
                    StressImage(); 
                }
                return; 
            }
            StressImage();
            EventManager.Instance.TriggerEvent(EventsType.SetOriginShopPn);
        }

        /// <summary>
        /// ���� �г��̵� �̹��� ����(�������� ������)
        /// </summary>
        private void StressImage()
        {
            for (int i = 0; i < panelIcons.Length; i++)
            {
                if (_targetIndex == i)
                {
                    panelIcons[i].anchoredPosition3D = new Vector3(0, 0);
                }
                else panelIcons[i].anchoredPosition3D = new Vector3(0, 0);
            }
        }

        /// <summary>
        /// ����,����,��������â �� �Ѱ����� �̵��ϴ� �� 
        /// </summary>
        /// <param name="n">0=���� 1=���� 2=��������</param>
        public override void OnMoveMainPanel(int index)
        {
            base.OnMoveMainPanel(index);
            StressImage();
        }
    }
}
