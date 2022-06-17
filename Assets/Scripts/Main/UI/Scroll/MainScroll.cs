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

        //지금은 bool로 때우는데 추후 바꿀것..
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
        /// 드래그가 끝날시 발동
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
        /// 우측 패널이동 이미지 강조(왼쪽으로 움직임)
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
        /// 상점,메인,스테이지창 중 한곳으로 이동하는 것 
        /// </summary>
        /// <param name="n">0=상점 1=메인 2=스테이지</param>
        public override void OnMoveMainPanel(int index)
        {
            base.OnMoveMainPanel(index);
            StressImage();
        }


    }
}
