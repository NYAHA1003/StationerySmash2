using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Skin
{
    public class ButtonScroll : MonoBehaviour
    {
        //인스펙터 변수
        [SerializeField]
        private Vector2 _centerSize = new Vector2(200f, 200f);  // 중앙 UI의 크기
        [SerializeField]
        private Vector2 _edgeSize = new Vector2(100f, 100f);    // 가장 외곽 UI의 크기
        [SerializeField, Range(0f, 100f)]
        private float _spaceInterval = 25f; // 이미지 사이 간격
        [SerializeField, Range(0.01f, 2f)]
        private float _transitionTime = 0.5f; // 전환에 걸리는 시간
        [SerializeField]
        private int _currentIndex = 0;

        //상수
        private const int LEFT = 1;
        private const int RIGHT = -1;

        //변수
        private List<RectTransform> _targetList = new List<RectTransform>();
        private List<RectTransform> _imposterList = new List<RectTransform>();
        private RectTransform _currentImposter;
        private int _targetCount = 0;
        private bool _isTransiting = false;
        private float _progress; // 진행도 : 0 ~ 1
        private int _direction;
        private int _lookupCount;
        private int _lookupCenterIndex;
        private float[] _xPosTable; // 인덱스 위치에 따른 X 좌표 기록
        private Vector2[] _sizeTable; // 인덱스 위치에 따른 크기 기록


        private void OnEnable()
        {
            //켜질 때 콘텐츠 리스트 초기화
            _targetList.Clear();
            int childCount = transform.childCount;

            //콘텐츠들을 리스트에 추가
            for (int i = 0; i < childCount; i++)
            {
                Transform child = transform.GetChild(i);
                _targetList.Add(child.GetComponent<RectTransform>());
            }

            //콘텐츠 카운트와 회전카운트, 중심 인덱스 설정
            _targetCount = _targetList.Count;
            _lookupCount = _targetCount + 2;
            _lookupCenterIndex = _lookupCount / 2;

            GenerateLookUpTables();

            //끝에서 끝으로 옮길 객체 설정
            InitRectTransforms();
            GenerateImposters();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                LeftTransition();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                RightTransition();
            }

            if (_isTransiting)
            {
                _progress += Time.deltaTime / _transitionTime;
                if (_progress > 1f)
                {
                    _progress = 1f;
                }

                //이동 시작
                OnTransition();

                if (_progress == 1f)
                {
                    _isTransiting = false;
                    _currentIndex = (_currentIndex - _direction) % _targetCount;
                    if (_currentIndex < 0) _currentIndex += _targetCount;
                    OnTransitionEnd();
                }
            }
        }

        /// <summary>
        /// 오른쪽 버튼으로 변경
        /// </summary>
        public void RightTransition()
        {
            if (!_isTransiting)
            {
                _direction = RIGHT;
                BeginTransition();
            }
        }


        /// <summary>
        /// 왼쪽 버튼으로 변경
        /// </summary>
        public void LeftTransition()
        {
            if (!_isTransiting)
            {
                _direction = LEFT;
                BeginTransition();
            }
        }

        /// <summary>
        /// 버튼 이동 입력받음
        /// </summary>
        private void BeginTransition()
        {
            _isTransiting = true;
            _progress = 0;
            InitImposter();
        }

        /// <summary>
        /// 객체들 이동 시작
        /// </summary>
        private void OnTransition()
        {
            MoveAll();
            MoveImposter();
        }


        /// <summary>
        /// 테이블 생성
        /// </summary>
        private void GenerateLookUpTables()
        {
            _xPosTable = new float[_lookupCount];
            _sizeTable = new Vector2[_lookupCount];

            for (int i = 0; i < _lookupCount; i++)
            {
                _xPosTable[i] = GetXPosition(i);
                _sizeTable[i] = GetSize(i);
            }
        }

        /// <summary>
        /// lookup center index와의 인덱스 차이 계산
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private int GetIndexDiffFromCenter(int index)
        {
            return Mathf.Abs(index - _lookupCenterIndex);
        }

        /// <summary>
        /// 가로세로 크기 구하기
        /// </summary>
        /// <param name="lookupIndex"></param>
        /// <returns></returns>
        private Vector2 GetSize(int lookupIndex)
        {
            if (lookupIndex == _lookupCenterIndex)
                return _centerSize;

            float absGap = GetIndexDiffFromCenter(lookupIndex);

            return Vector2.Lerp(_centerSize, _edgeSize, absGap / _lookupCenterIndex);
        }

        /// <summary>
        /// X좌표 구하기 
        /// </summary>
        /// <param name="lookupIndex"></param>
        /// <returns></returns>
        private float GetXPosition(int lookupIndex)
        {
            // 중앙 위치
            if (lookupIndex == _lookupCenterIndex)
                return 0f;

            float absGap = GetIndexDiffFromCenter(lookupIndex);

            // // 1. 빈공간 합
            float pos = absGap * _spaceInterval;

            // 2. 너비 합
            for (int i = 0; i <= absGap; i++)
            {
                float w = Vector2.Lerp(_centerSize, _edgeSize, i / (float)_lookupCenterIndex).x;

                if (0 < i && i < absGap)
                    pos += w;
                else
                    pos += w * 0.5f;
            }

            return (lookupIndex < _lookupCenterIndex) ? -pos : pos;
        }
        private void OnTransitionEnd()
        {
            for (int i = 0; i < _targetCount; i++)
            {
                int li = GetLookupIndex(i);

                // 양측의 이미지를 알맞은 위치로 이동
                if (li == 1 || li == _targetCount)
                {
                    _targetList[i].SetSizeAndXPosition(_sizeTable[li], _xPosTable[li]);
                }
            }

            _currentImposter.gameObject.SetActive(false);
        }

        /// <summary>
        /// index를 lookup index에 매핑하기
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private int GetLookupIndex(int index)
        {
            index = index - _currentIndex + _lookupCenterIndex;

            if (index > _targetCount)
                index -= _targetCount;

            else if (index <= 0)
                index += _targetCount;

            return index;
        }

        /// <summary>
        /// lookup index를 index에 매핑하기
        /// </summary>
        /// <param name="lookupIndex"></param>
        /// <returns></returns>
        private int GetIndexFromLookupIndex(int lookupIndex)
        {
            int index = lookupIndex - _lookupCenterIndex + _currentIndex;

            if (index < 0)
                index += _targetCount;

            return index % _targetCount;
        }

        /// <summary>
        /// 가장 좌측 이미지의 실제 인덱스 
        /// </summary>
        /// <returns></returns>
        private int GetLeftImageIndex()
        {
            return GetIndexFromLookupIndex(0);
        }
        /// <summary>
        ///  가장 우측 이미지의 실제 인덱스
        /// </summary>
        /// <returns></returns>
        private int GetRightImageIndex()
        {
            return GetIndexFromLookupIndex(_lookupCount - 1);
        }

        /// <summary>
        /// Rect Transform X Pos, Size 초기 설정
        /// </summary>
        private void InitRectTransforms()
        {
            for (int i = 0; i < _targetCount; i++)
            {
                int li = GetLookupIndex(i);
                float xPos = _xPosTable[li];
                Vector2 size = _sizeTable[li];

                _targetList[i].SetSizeAndXPosition(size, xPos);
            }
        }

        /// <summary>
        ///  모든 이미지의 Rect Transform 이동시키기 
        /// </summary>
        private void MoveAll()
        {
            for (int i = 0; i < _targetCount; i++)
            {
                int li = GetLookupIndex(i);

                float curXPos = _xPosTable[li];
                float nextXPos = _xPosTable[li + _direction];
                float xPos = Mathf.Lerp(curXPos, nextXPos, _progress);

                Vector2 curSize = _sizeTable[li];
                Vector2 nextSize = _sizeTable[li + _direction];
                Vector2 size = Vector2.Lerp(curSize, nextSize, _progress);
                _targetList[i].SetSizeAndXPosition(size, xPos);
            }
        }

        /// <summary>
        /// 끝에서 끝으로 옮길 객체 생성
        /// </summary>
        private void GenerateImposters()
        {
            _imposterList.Clear();

            for (int i = 0; i < _targetCount; i++)
            {
                GameObject go = Instantiate(_targetList[i].gameObject);
                go.transform.SetParent(transform);
                go.hideFlags = HideFlags.HideInHierarchy;

                RectTransform rt = go.GetComponent<RectTransform>();
                _imposterList.Add(rt);
                go.SetActive(false);
            }
        }

        /// <summary>
        /// 끝에서 끝으로 옮길 객체 초기화
        /// </summary>
        private void InitImposter()
        {
            int index = 0;
            int lookupIndex = 0;
            float xPos;
            Vector2 size;

            switch (_direction)
            {
                default:
                case LEFT:
                    index = GetLeftImageIndex();
                    lookupIndex = 0;
                    break;

                case RIGHT:
                    index = GetRightImageIndex();
                    lookupIndex = _lookupCount - 1;
                    break;
            }

            xPos = _xPosTable[lookupIndex];
            size = _sizeTable[lookupIndex];

            _currentImposter = _imposterList[index];
            _currentImposter.SetSizeAndXPosition(size, xPos);
            _currentImposter.gameObject.SetActive(true);
        }

        /// <summary>
        /// 끝에서 끝으로 옮길 객체 옮기기
        /// </summary>
        private void MoveImposter()
        {
            int lookupIndex, lookupIndexNext;

            switch (_direction)
            {
                default:
                case LEFT:
                    lookupIndex = 0;
                    lookupIndexNext = 1;
                    break;

                case RIGHT:
                    lookupIndex = _lookupCount - 1;
                    lookupIndexNext = _lookupCount - 2;
                    break;
            }

            float curXPos = _xPosTable[lookupIndex];
            float nextXPos = _xPosTable[lookupIndexNext];
            float xPos = Mathf.Lerp(curXPos, nextXPos, _progress);

            Vector2 curSize = _sizeTable[lookupIndex];
            Vector2 nextSize = _sizeTable[lookupIndexNext];
            Vector2 size = Vector2.Lerp(curSize, nextSize, _progress);

            _currentImposter.SetSizeAndXPosition(size, xPos);
        }
    }

    static class RectTransformExtensionHelper
    {
        public static void SetSizeAndXPosition(this RectTransform @this, in Vector2 size, float xPos)
        {
            @this.sizeDelta = size;
            @this.anchoredPosition = new Vector2(xPos, 0f);
        }
    }
}