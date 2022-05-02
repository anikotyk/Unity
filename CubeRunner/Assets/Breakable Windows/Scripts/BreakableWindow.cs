using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Breakable Windows/Breakable Window")]
public class BreakableWindow : MonoBehaviour {
    
    [Tooltip("Layer should be TransparentFX or your own layer for breakable windows.")]
    [SerializeField] private LayerMask _layer;
    [SerializeField] [Range(2,25)] private int _partsX = 8;
    [SerializeField] [Range(2, 25)] private int _partsY = 8;

    [Space]
    [SerializeField] private bool _preCalculate = true;
    [SerializeField] private bool _addTorques = true;
    [SerializeField] private bool _hideSplintersInHierarchy = true;
    [SerializeField] private bool _useCollision = true;
    
    [Space]
    [Tooltip("Seconds after window is broken that splinters have to be destroyed.")]
    [SerializeField] private float _destroySplintersTime = 0;

    [Space]
    [SerializeField] private AudioClip _breakingSound;

    [SerializeField] private GameObject _parentToDestroy;

    [HideInInspector]
    private bool _isBroken = false;
    private List<GameObject> _splinters;
    private Vector3[] _vertices;
    private Vector3[] _normals;
    
    private bool _allreadyCalculated = false;
    private GameObject _splinterParent;
    private int[] _tris;
    
    private void Start()
    {
        if (_preCalculate == true && _allreadyCalculated == false)
        {
            BakeVertices();
            BakeSplinters();
            _allreadyCalculated = true;
        }

        if (transform.rotation.eulerAngles.x != 0 || transform.rotation.eulerAngles.z != 0)
        {
            //Debug.LogWarning("Warning: Window must not be rotated around x and z!");
        }

    }

    private void BakeVertices(bool trianglesToo = false)
    {
        _vertices = new Vector3[(_partsX + 1) * (_partsY + 1)];
        _normals = new Vector3[(_partsX + 1) * (_partsY + 1)];
        

        for (int y = 0; y < _partsY + 1; y++)
        {
            for (int x = 0; x < _partsX + 1; x++)
            {
                float randomX = Random.value > 0.5f ? Random.value / _partsX : -Random.value / _partsX;
                float randomY = Random.value > 0.5f ? Random.value / _partsY : -Random.value / _partsY;
                _vertices[y * (_partsX + 1) + x] = new Vector3((float)x / (float)_partsX - 0.5f + randomX, (float)y / (float)_partsY - 0.5f + randomY, 0);
                _normals[y * (_partsX + 1) + x] = -Vector3.forward;
            }
        }

        if (trianglesToo == true)
        {
            _tris = new int[_partsX * _partsY * 6];
            int pos = 0;
            for (int y = 0; y < _partsY; y++)
            {
                for (int x = 0; x < _partsX; x++)
                {
                    _tris[pos + 0] = y * (_partsX + 1) + x;
                    _tris[pos + 1] = y * (_partsX + 1) + x + 1;
                    _tris[pos + 2] = (y + 1) * (_partsX + 1) + x;

                    pos += 3;

                    _tris[pos + 0] = (y + 1) * (_partsX + 1) + x;
                    _tris[pos + 1] = y * (_partsX + 1) + x + 1;
                    _tris[pos + 2] = (y + 1) * (_partsX + 1) + x + 1;

                    pos += 3;
                }
            }
        }
    }

    private void GenerateSingleSplinter(int[] tris, Transform parent)
    {
        Vector3[] v = new Vector3[3];
        Vector3[] n = new Vector3[3];
        int[] t = new int[6];

        v[0] = Vector3.zero;
        v[1] = _vertices[tris[1]] - _vertices[tris[0]];
        v[2] = _vertices[tris[2]] - _vertices[tris[0]];

        n[0] = _normals[t[0]];
        n[1] = _normals[t[1]];
        n[2] = _normals[t[2]];

        t[0] = 0;
        t[1] = 1;
        t[2] = 2;
        t[3] = 2;
        t[4] = 1;
        t[5] = 0;

        Mesh m = new Mesh();
        m.vertices = v;
        m.normals = n;
        m.triangles = t;

        GameObject obj = new GameObject();
        obj.transform.position = new Vector3(_vertices[tris[0]].x * transform.localScale.x + transform.position.x, _vertices[tris[0]].y * transform.localScale.y + transform.position.y, transform.position.z);
        obj.transform.RotateAround(transform.position, transform.up, transform.rotation.eulerAngles.y);
        obj.transform.localScale = transform.localScale;
        
        obj.transform.rotation = transform.rotation;
        obj.layer = _layer.value;
        obj.name = "Glass Splinter";
        if (_destroySplintersTime > 0)
            Destroy(obj, _destroySplintersTime);


        if (_preCalculate == true)
        {
            obj.transform.parent = parent;
        }

        if (_hideSplintersInHierarchy) obj.hideFlags = HideFlags.HideInHierarchy;
        _splinters.Add(obj);

        MeshFilter mf = obj.AddComponent<MeshFilter>();
        mf.mesh = m;
        
        MeshCollider col = obj.AddComponent<MeshCollider>();
        col.inflateMesh = true;
        col.convex = true;
        
        Rigidbody rigid = obj.AddComponent<Rigidbody>();
        rigid.centerOfMass = (v[0] + v[1] + v[2]) / 3f;
        if (_addTorques && _preCalculate == false) rigid.AddTorque(new Vector3(Random.value > 0.5f ? Random.value * 50 : -Random.value * 50, Random.value > 0.5f ? Random.value * 50 : -Random.value * 50, Random.value > 0.5f ? Random.value * 50 : -Random.value * 50));
        

        MeshRenderer mr = obj.AddComponent<MeshRenderer>();
        mr.materials = GetComponent<Renderer>().materials;
    }

    private void BakeSplinters()
    {
        int[] t = new int[3];
        _splinters = new List<GameObject>();
        _splinterParent = new GameObject("Splinters");
        _splinterParent.transform.SetParent(transform);

        if (_preCalculate) _splinterParent.SetActive(false);

        for (int y = 0; y < _partsY; y++)
        {
            for (int x = 0; x < _partsX; x++)
            {
                t[0] = y * (_partsX + 1) + x;
                t[1] = y * (_partsX + 1) + x + 1;
                t[2] = (y + 1) * (_partsX + 1) + x;

                GenerateSingleSplinter(t, _splinterParent.transform);

                t[0] = (y + 1) * (_partsX + 1) + x;
                t[1] = y * (_partsX + 1) + x + 1;
                t[2] = (y + 1) * (_partsX + 1) + x + 1;

                GenerateSingleSplinter(t, _splinterParent.transform);
            }
        }
    }

    /// <summary>
    /// Breaks the window and returns an array of all splinter gameobjects.
    /// </summary>
    /// <returns>Returns an array of all splinter gameobjects.</returns>
    private GameObject[] BreakWindow()
    {
        if (_isBroken == false)
        {
            if (_allreadyCalculated == true)
            {
                _splinterParent.SetActive(true);
                if (_addTorques)
                {
                    for (int i = 0; i < _splinters.Count; i++)
                    {
                        _splinters[i].GetComponent<Rigidbody>().AddTorque(new Vector3(Random.value > 0.5f ? Random.value * 50 : -Random.value * 50, Random.value > 0.5f ? Random.value * 50 : -Random.value * 50, Random.value > 0.5f ? Random.value * 50 : -Random.value * 50));
                    }
                }
            }
            else
            {
                BakeVertices();
                BakeSplinters();
            }

            Physics.IgnoreLayerCollision(_layer.value, _layer.value, true);
            Destroy(GetComponent<Collider>());
            Destroy(GetComponent<MeshRenderer>());
            Destroy(GetComponent<MeshFilter>());

            _isBroken = true;            
        }

        if (_breakingSound != null)
        {
            AudioController.Instance.PlayBreakingWindowSound();
        }

        return _splinters.ToArray();
    }
    
    private void OnCollisionEnter(Collision col)
    {
        if (_useCollision == true && col.gameObject.tag=="Player")
        {
            if (HealthController.Instance.CurrentHealth<=0)
            {
                return;
            }

            if (gameObject.tag != "Obstacle")
            {
                GameController.Instance.AddMoney();
            }
            else
            {
                PlayerController.Instance.OnObstacleCollid();
            }
            
            if (HealthController.Instance.CurrentHealth > 0)
            {
                BreakWindow();
            }
            else
            {
                GameController.Instance.ToDestroyIfContinueRunning = _parentToDestroy;
                Destroy(GetComponent<Collider>());
            }
        }        
    }
    
}
