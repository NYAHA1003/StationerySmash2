using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Main.Event;
using Utill.Data; 
[Serializable]
public class MeshInfo
{
    public int x;
    public int y;
    [Range(0.0f, 1.0f)] public float sepXPoint; // 나누는 점의 비율 
    [Range(0.0f, 1.0f)] public float sepYPoint; // 나누는 점의 비율 

}
public class CardMesh : MonoBehaviour
{

    [SerializeField]
    private MeshInfo _meshInfo;
    [SerializeField]
    private Transform _slicedParent;
    [SerializeField]
    private GameObject _slicedMeshObj;

    private Mesh _mesh;
    private Mesh _slicedMesh;
    private MeshFilter _meshFilter;
    private MeshFilter _slicedMeshFilter;
    private MeshRenderer meshRenderer;

    private RectTransform _slicedRect;
    private RectTransform __slicedParentRect;
    private Vector3[] vertices;
    private Vector2[] uvs;
    private int[] triangles;

    //private float sepPoint = 1.5f;

    [SerializeField]
    private float x, y, sepXpoint, sepYpoint;

    private void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _slicedMeshFilter = _slicedMeshObj.GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        _slicedRect = _slicedMeshObj.GetComponent<RectTransform>();
        __slicedParentRect = _slicedParent.GetComponent<RectTransform>();
    }
    [ContextMenu("MeshTest")]
    public void StartMesh()
    {
        StartCoroutine(OpenCard());
    }
    /// <summary>
    /// 카드 포지션 세팅  
    /// </summary>
    private void Initialize()
    {
        x = _meshInfo.x;
        y = _meshInfo.y;
        sepXpoint = x * _meshInfo.sepXPoint;
        sepYpoint = y * _meshInfo.sepYPoint;
        __slicedParentRect.anchoredPosition = new Vector3(sepXpoint, 0, 0);
        _slicedRect.anchoredPosition = new Vector3(-sepXpoint, 0, 0);

        _mesh?.Clear();
        _slicedMesh?.Clear();
        _slicedParent.rotation = Quaternion.identity;

    }

    /// <summary>
    /// 카드팩 만들기
    /// </summary>
    private void CreateMesh()
    {
        Initialize(); 

        _mesh = new Mesh();

        vertices = new Vector3[4];
        uvs = new Vector2[4];
        triangles = new int[6];
        vertices[0] = Vector3.zero;
        vertices[1] = new Vector3(0, y, 0);
        vertices[2] = new Vector3(x, y, 0);
        vertices[3] = new Vector3(x, 0, 0);

        uvs[0] = Vector2.zero;
        uvs[1] = new Vector2(0, 1);
        uvs[2] = Vector2.one;
        uvs[3] = new Vector2(1, 0);

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 3;
        triangles[3] = 1;
        triangles[4] = 2;
        triangles[5] = 3;

        _mesh.vertices = vertices;
        _mesh.uv = uvs;
        _mesh.triangles = triangles;

        _meshFilter.mesh = _mesh;
    }

    public IEnumerator OpenCard()
    {
        CreateMesh();
        vertices = new Vector3[4];
        uvs = new Vector2[4];
        triangles = new int[6];

        vertices[0] = new Vector3(0, 0, 0);
        vertices[1] = new Vector3(0, sepYpoint, 0);
        vertices[2] = new Vector3(sepXpoint, sepYpoint, 0);
        vertices[3] = new Vector3(sepXpoint, 0, 0);

        uvs[0] = Vector2.zero;
        uvs[1] = new Vector2(0, _meshInfo.sepYPoint);
        uvs[2] = new Vector2(_meshInfo.sepXPoint, _meshInfo.sepYPoint);
        uvs[3] = new Vector2(_meshInfo.sepXPoint, 0);

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 3;
        triangles[3] = 1;
        triangles[4] = 2;
        triangles[5] = 3;

        _mesh.vertices = vertices;
        _mesh.uv = uvs;
        _mesh.triangles = triangles;

        _meshFilter.mesh = _mesh;


        _slicedMesh = new Mesh();

        vertices[0] = new Vector3(sepXpoint, 0, 0);
        vertices[1] = new Vector3(sepXpoint, y, 0);
        vertices[2] = new Vector3(x, y, 0);
        vertices[3] = new Vector3(x, 0, 0);

        uvs[0] = new Vector2(_meshInfo.sepXPoint, 0);
        uvs[1] = new Vector2(_meshInfo.sepXPoint, 1);
        uvs[2] = new Vector2(1, 1);
        uvs[3] = new Vector2(1, 0);

        _slicedMesh.vertices = vertices;
        _slicedMesh.triangles = triangles;
        _slicedMesh.uv = uvs;
        _slicedMeshFilter.mesh = _slicedMesh;
        //        GameObject g = new GameObject("SlicedMesh", typeof(MeshFilter), typeof(MeshRenderer), typeof(RectTransform));


        //    g.transform.SetParent(_slicedParent.transform);
        //      g.GetComponent<RectTransform>().anchoredPosition = new Vector3(-1.5f, 0, 0);
        //  g.GetComponent<MeshFilter>().mesh = _slicedMesh;
        //g.GetComponent<MeshRenderer>().material = _mat;

        yield return new WaitForSeconds(0.3f);

        MoveTest(_slicedMeshObj);
    }

    public void MoveTest(GameObject g)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_slicedParent.DORotate(new Vector3(0, 0, -35), 0.5f));
        sequence.Append(_slicedParent.DORotate(new Vector3(0, 0, -80), 0.4f));
        sequence.AppendCallback(() =>
        {
            // 카드 사이 빛나는 이펙트 추가 
        });
        sequence.AppendCallback(() =>
        {
            EventManager.Instance.TriggerEvent(EventsType.ActiveAndAnimateCard);
        });
        //sequence.Append(g.GetComponent<RectTransform>().DOAnchorPosX(-2f, 0.6f));
        //        sequence.Join(g.GetComponent<RectTransform>().DORotate(new Vector3(0, 360, -70), 0.6f,RotateMode.FastBeyond360));
        //sequence.Join(g.GetComponent<RectTransform>().DOScale(new Vector3(0.3f, 0.3f, 0.3f), 0.6f));

    }



}
