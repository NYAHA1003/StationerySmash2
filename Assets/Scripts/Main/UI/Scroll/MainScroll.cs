using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Utill.Data;
using Utill.Tool;
using Main.Event;
using Main.Setting;

namespace Main.Scroll
{
    public class MainScroll : AgentScroll
    {
        private static RectTransform[] _staticPanelIcons;
        private static GameObject[] _staticTexts;

        [SerializeField]
        private Slider accentSlider;
        [SerializeField]
        private RectTransform[] panelIcons;
        [SerializeField]
        private GameObject[] texts;

        //������ bool�� ����µ� ���� �ٲܰ�..
        private static bool isChangeStage=false;
        protected override void SettingAwake()
        {
            base.SettingAwake();
            _staticPanelIcons = panelIcons;
            _staticTexts = texts;
            EventManager.Instance.StartListening(EventsType.MoveMainPn, (x) => OnMoveMainPanel((int)x));
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
                if (DeltaSlide(eventData.delta.y) == true)
                {
                    StressImage();
                }
            }
            StressImage();
            EventManager.Instance.TriggerEvent(EventsType.SetOriginShopPn);
        }

        /// <summary>
        /// ���� �г��̵� �̹��� ����(�������� ������)
        /// </summary>
        private static void StressImage()
        {
            if (_targetIndex == 0 && !isChangeStage)
            {
                Sound.StopBgm(1);
                Sound.PlayBgm(2);
                isChangeStage = true;
            }
            else if(isChangeStage)
            {
                Sound.StopBgm(2);
                Sound.PlayBgm(1);
                isChangeStage = false;
            }
            for (int i = 0; i < _staticPanelIcons.Length; i++)
            {
                if (_targetIndex == i)
                {
                    _staticTexts[i].SetActive(true);
                    _staticPanelIcons[i].anchoredPosition3D = new Vector3(0, 0);
                }
                else
                {
                    _staticPanelIcons[i].anchoredPosition3D = new Vector3(0, 0);
                    _staticTexts[i].SetActive(false);
                }
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
