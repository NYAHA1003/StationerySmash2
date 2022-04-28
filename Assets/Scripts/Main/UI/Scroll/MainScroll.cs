using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Utill;
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
            EventManager.StartListening(EventsType.MoveMainPn, OnMoveMainPanel);
        }
        protected override void SettingStart()
        {
            base.SettingStart();
            _targetPos = _pos[1];
            _targetIndex = 1;
            StressImage();
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
                print(eventData.delta.x);
                DeltaSlide(eventData.delta.y);
            }
            StressImage();
            EventManager.TriggerEvent(EventsType.SetOriginShopPn);
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
                    panelIcons[i].anchoredPosition3D = new Vector3(-80, 0);
                    Debug.Log("���� Ÿ�� �ε��� : " + _targetIndex);
                }
                else panelIcons[i].anchoredPosition3D = new Vector3(0, 0);
            }
        }

        /// <summary>
        /// ����,����,��������â �� �Ѱ����� �̵��ϴ� �� 
        /// </summary>
        /// <param name="n">0=���� 1=���� 2=��������</param>
        public void OnMoveMainPanel(object n)
        {
            if ((int)n < 0 || (int)n > Size - 1)
            {
                Debug.LogError("���� �г� �����̴� ���� �Ѿ 0~SIZE-1 ���� ���� �ƴ�");
                return;
            }
            _targetIndex = Size - (int)n - 1;
            _targetPos = _pos[(int)n];
            StressImage();
        }
    }
}
