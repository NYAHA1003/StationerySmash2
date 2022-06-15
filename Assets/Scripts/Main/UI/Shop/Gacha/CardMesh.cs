using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CardMesh : MonoBehaviour
{
    [SerializeField]
    private Material mat;
    [SerializeField]
    private Canvas meshCanvas;
    [SerializeField]
    private Transform slicedParent;

    private Mesh mesh;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    private Vector3[] vertices;
    private Vector2[] uvs;
    private int[] triangles;

    private float sepPoint = 1.5f;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
    }
    void Start()
    {



    }

    private void CreateMesh()
    {
        mesh.Clear();
        mesh = new Mesh();
        meshFilter.mesh = null; 

        vertices = new Vector3[4];
        uvs = new Vector2[4];
        triangles = new int[6];
        vertices[0] = Vector3.zero;
        vertices[1] = new Vector3(0, 2, 0);
        vertices[2] = new Vector3(2, 2, 0);
        vertices[3] = new Vector3(2, 0, 0);

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

        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        meshFilter.mesh = mesh;
    }
    [ContextMenu("MeshTest")]
    public void ABC()
    {
        StartCoroutine(OpenCard());
    }
    public IEnumerator OpenCard()
    {
        CreateMesh(); 
        vertices = new Vector3[4];
        uvs = new Vector2[4];
        triangles = new int[6];

        vertices[0] = new Vector3(0, 0, 0);
        vertices[1] = new Vector3(0, 2, 0);
        vertices[2] = new Vector3(sepPoint, 2, 0);
        vertices[3] = new Vector3(sepPoint, 0, 0);

        uvs[0] = Vector2.zero;
        uvs[1] = new Vector2(0, 1);
        uvs[2] = new Vector2(0.75f, 1);
        uvs[3] = new Vector2(0.75f, 0);

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 3;
        triangles[3] = 1;
        triangles[4] = 2;
        triangles[5] = 3;

        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        meshFilter.mesh = mesh;


        Mesh slicedMesh = new Mesh();

        vertices[0] = new Vector3(sepPoint, 0, 0);
        vertices[1] = new Vector3(sepPoint, 2, 0);
        vertices[2] = new Vector3(2, 2, 0);
        vertices[3] = new Vector3(2, 0, 0);

        uvs[0] = new Vector2(0.75f, 0);
        uvs[1] = new Vector2(0.75f, 1);
        uvs[2] = new Vector2(1, 1);
        uvs[3] = new Vector2(1, 0);

        slicedMesh.vertices = vertices;
        slicedMesh.triangles = triangles;
        slicedMesh.uv = uvs;

        GameObject g = new GameObject("SlicedMesh", typeof(MeshFilter), typeof(MeshRenderer), typeof(RectTransform));


        g.transform.SetParent(slicedParent.transform);
        g.GetComponent<RectTransform>().anchoredPosition = new Vector3(-1.5f, 0, 0);
        g.GetComponent<MeshFilter>().mesh = slicedMesh;
        g.GetComponent<MeshRenderer>().material = mat;

        yield return new WaitForSeconds(0.3f);

        MoveTest(g);
    }

    public void MoveTest(GameObject g)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(slicedParent.DORotate(new Vector3(0, 0, -35), 0.5f));
        sequence.Append(slicedParent.DORotate(new Vector3(0, 0, -80), 0.5f));
        sequence.Append(g.GetComponent<RectTransform>().DOAnchorPosX(-2f, 0.6f));
        //        sequence.Join(g.GetComponent<RectTransform>().DORotate(new Vector3(0, 360, -70), 0.6f,RotateMode.FastBeyond360));
        sequence.Join(g.GetComponent<RectTransform>().DOScale(new Vector3(0.3f, 0.3f, 0.3f), 0.6f));

    }
   


}
