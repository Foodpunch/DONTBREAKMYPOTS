using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class Tilemap : MonoBehaviour {
    //Generates Tile map from scratch.

    public int size_x = 100;
    public int size_z = 50;
    public float tileSize = 1.0f;

    public Texture2D tilesTexture;
    public int tileRes = 100;       


    void Start() {
        CreateMesh();
    }
    Color[][] SplitTiles()
    {
        int numTilesPerRow = tilesTexture.width / tileRes;
        int numTilesRow = tilesTexture.height / tileRes;
        Color[][] _tiles = new Color[numTilesPerRow * numTilesRow][];
        for(int y=0;y < numTilesRow;y++)
        {
            for(int x=0;x<numTilesPerRow;x++)
            {
                _tiles[y * numTilesPerRow + x] = tilesTexture.GetPixels(x * tileRes, y * tileRes, tileRes, tileRes);
            }
        }
        return _tiles;
    }
    void CreateTexture()
    {
   

        int textWidth = size_x * tileRes;
        int textHeight = size_z * tileRes;
        Texture2D _texture = new Texture2D(textWidth, textHeight);

        Color[][] tiles = SplitTiles();

        for(int y = 0; y < size_z; y++)
        {
            for (int x = 0; x < size_x; x++)
            {
               Color[] _colors =  tiles[Random.Range(0,4)];
                _texture.SetPixels(x * tileRes, y * tileRes, tileRes, tileRes, _colors);
            }
        }
        _texture.filterMode = FilterMode.Point; //bilinear smooths out things, point lets you really see the pixels
        _texture.wrapMode = TextureWrapMode.Clamp;
        _texture.Apply();

        MeshRenderer _meshRenderer = GetComponent<MeshRenderer>();
        _meshRenderer.sharedMaterials[0].mainTexture = _texture;
    }

    public void CreateMesh()
    {
        int numTiles = size_x * size_z;
        int numTris = numTiles * 2;
        int vsize_x = size_x + 1;
        int vsize_z = size_z + 1;
        int numVerts = vsize_x * vsize_z;
     
        //Create data for the mesh
        Vector3[] _verts = new Vector3[numVerts];
        Vector3[] _normals = new Vector3[numVerts];
        Vector2[] _uvs = new Vector2[numVerts];
        int[] _tris = new int[numTris * 3];

        int x, z;
        for (z = 0; z < vsize_z; z++)
        {
            for (x = 0; x < vsize_x; x++)
            {
                _verts[z * vsize_x + x] = new Vector3(x * tileSize, 0, z * tileSize);
                _normals[z * vsize_x + x] = Vector3.up;
                _uvs[z * vsize_x + x] = new Vector2((float)x / vsize_x, (float)z / vsize_z);
            }
        }
        for (z = 0; z < size_z; z++)
        {
            for (x = 0; x < size_x; x++)
            {
                int sqIndex = z * size_x + x;
                int trisOffset = sqIndex * 6;
                _tris[trisOffset + 0] = z * vsize_x + x +           0;
                _tris[trisOffset + 1] = z * vsize_x + x + vsize_x + 0;
                _tris[trisOffset + 2] = z * vsize_x + x + vsize_x + 1;

                _tris[trisOffset + 3] = z * vsize_x + x +           0;
                _tris[trisOffset + 4] = z * vsize_x + x + vsize_x + 1;
                _tris[trisOffset + 5] = z * vsize_x + x +           1;

            }
        }



        //Create mesh and populate it with the data (a mesh has _verts, tris and _normals)
        Mesh _mesh = new Mesh();       //creates new mesh
        _mesh.vertices = _verts;       //assigns verts to verts
        _mesh.triangles = _tris;       //assigns tris to tris
        _mesh.normals = _normals;      //assigns normals to normals
        _mesh.uv = _uvs;                //asigns uvs to uvs

        //Assign mesh to filter,collider and renderer
        MeshFilter _meshFilter = GetComponent<MeshFilter>();
        MeshRenderer _meshRenderer = GetComponent<MeshRenderer>();
        MeshCollider _meshCollider = GetComponent<MeshCollider>();

        _meshFilter.mesh = _mesh;

        CreateTexture();
    }
}
