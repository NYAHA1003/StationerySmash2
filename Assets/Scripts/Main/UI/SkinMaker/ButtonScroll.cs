using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Skin
{
    public class ButtonScroll : MonoBehaviour
    {
        //�ν����� ����
        [SerializeField]
        private Vector2 _centerSize = new Vector2(200f, 200f);  // �߾� UI�� ũ��
        [SerializeField]
        private Vector2 _edgeSize = new Vector2(100f, 100f);    // ���� �ܰ� UI�� ũ��
        [SerializeField, Range(0f, 100f)]
        private float _spaceInterval = 25f; // �̹��� ���� ����
        [SerializeField, Range(0.01f, 2f)]
        private float _transitionTime = 0.5f; // ��ȯ�� �ɸ��� �ð�
        [SerializeField]
        private int _currentIndex = 0;

        //���
        private const int LEFT = 1;
        private const int RIGHT = -1;

        //����
        private List<RectTransform> _targetList = new List<RectTransform>();
        private List<RectTransform> _imposterList = new List<RectTransform>();
        private RectTransform _currentImposter;
        private int _targetCount = 0;
        private bool _isTransiting = false;
        private float _progress; // ���൵ : 0 ~ 1
        private int _direction;
        private int _lookupCount;
        private int _lookupCenterIndex;
        private float[] _xPosTable; // �ε��� ��ġ�� ���� X ��ǥ ���
        private Vector2[] _sizeTable; // �ε��� ��ġ�� ���� ũ�� ���


        private void OnEnable()
        {
            //���� �� ������ ����Ʈ �ʱ�ȭ
            _targetList.Clear();
            int childCount = transform.childCount;

            //���������� ����Ʈ�� �߰�
            for (int i = 0; i < childCount; i++)
            {
                Transform child = transform.GetChild(i);
                _targetList.Add(child.GetComponent<RectTransform>());
            }

            //������ ī��Ʈ�� ȸ��ī��Ʈ, �߽� �ε��� ����
            _targetCount = _targetList.Count;
            _lookupCount = _targetCount + 2;
            _lookupCenterIndex = _lookupCount / 2;

            GenerateLookUpTables();

            //������ ������ �ű� ��ü ����
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

                //�̵� ����
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
        /// ������ ��ư���� ����
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
        /// ���� ��ư���� ����
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
        /// ��ư �̵� �Է¹���
        /// </summary>
        private void BeginTransition()
        {
            _isTransiting = true;
            _progress = 0;
            InitImposter();
        }

        /// <summary>
        /// ��ü�� �̵� ����
        /// </summary>
        private void OnTransition()
        {
            MoveAll();
            MoveImposter();
        }


        /// <summary>
        /// ���̺� ����
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
        /// lookup center index���� �ε��� ���� ���
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private int GetIndexDiffFromCenter(int index)
        {
            return Mathf.Abs(index - _lookupCenterIndex);
        }

        /// <summary>
        /// ���μ��� ũ�� ���ϱ�
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
        /// X��ǥ ���ϱ� 
        /// </summary>
        /// <param name="lookupIndex"></param>
        /// <returns></returns>
        private float GetXPosition(int lookupIndex)
        {
            // �߾� ��ġ
            if (lookupIndex == _lookupCenterIndex)
                return 0f;

            float absGap = GetIndexDiffFromCenter(lookupIndex);

            // // 1. ����� ��
            float pos = absGap * _spaceInterval;

            // 2. �ʺ� ��
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

                // ������ �̹����� �˸��� ��ġ�� �̵�
                if (li == 1 || li == _targetCount)
                {
                    _targetList[i].SetSizeAndXPosition(_sizeTable[li], _xPosTable[li]);
                }
            }

            _currentImposter.gameObject.SetActive(false);
        }

        /// <summary>
        /// index�� lookup index�� �����ϱ�
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
        /// lookup index�� index�� �����ϱ�
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
        /// ���� ���� �̹����� ���� �ε��� 
        /// </summary>
        /// <returns></returns>
        private int GetLeftImageIndex()
        {
            return GetIndexFromLookupIndex(0);
        }
        /// <summary>
        ///  ���� ���� �̹����� ���� �ε���
        /// </summary>
        /// <returns></returns>
        private int GetRightImageIndex()
        {
            return GetIndexFromLookupIndex(_lookupCount - 1);
        }

        /// <summary>
        /// Rect Transform X Pos, Size �ʱ� ����
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
        ///  ��� �̹����� Rect Transform �̵���Ű�� 
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
        /// ������ ������ �ű� ��ü ����
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
        /// ������ ������ �ű� ��ü �ʱ�ȭ
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
        /// ������ ������ �ű� ��ü �ű��
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