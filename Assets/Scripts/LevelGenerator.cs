using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
  
    public GameObject tile0_Empty;
    public GameObject tile1_OutsideCorner;
    public GameObject tile2_OutsideWall;
    public GameObject tile3_InsideCorner;
    public GameObject tile4_InsideWall;
    public GameObject tile5_Pellet;
    public GameObject tile6_PowerPellet;
    public GameObject tile7_TJunction;
    public GameObject tile8_GhostExit;

  
    public float cellSize = 1f;
    public int pixelsPerUnit = 32;
    public string manualLevelRootName = "ManualLevelRoot";
    public bool adjustCamera = true;
    public float cameraPadding = 1.0f;
  
    public string generatedRootName = "GeneratedLevel";

    private int[,] levelMap =
    {
        {1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1},
        {2,6,5,5,5,5,4,5,5,5,5,5,5,5,5,5,5,6,5,0,2},
        {2,5,3,4,4,5,4,5,3,4,4,5,4,5,4,5,4,5,5,5,2},
        {2,5,4,5,5,5,4,5,4,5,5,5,4,5,4,5,4,5,4,5,2},
        {2,5,4,5,5,5,4,5,3,4,4,5,4,5,3,4,3,5,4,5,2},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,2},
        {2,5,3,4,4,3,5,3,4,3,5,3,4,4,3,5,3,4,3,5,2},
        {2,5,4,0,0,4,5,4,0,4,5,4,0,0,4,5,4,0,4,5,2},
        {2,5,3,4,4,3,5,3,4,3,5,3,4,4,3,5,3,4,3,5,2},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,2},
        {2,5,5,5,5,4,4,5,5,5,5,5,5,5,5,5,5,5,5,5,2},
        {2,5,5,5,5,5,5,5,5,5,5,3,4,8,8,8,4,3,5,5,2},
        {2,5,5,5,5,5,5,4,5,5,5,4,0,0,0,0,0,4,5,5,2},
        {2,5,5,5,5,6,5,5,5,5,5,3,4,8,8,8,4,3,5,5,2},
        {2,5,4,5,4,5,4,5,3,4,5,5,5,5,5,5,4,5,5,5,2},
        {2,5,3,4,3,5,4,5,4,5,5,5,4,5,4,5,3,4,5,6,2},
        {2,6,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,6,2},
        {2,5,5,5,5,5,5,5,5,5,5,5,4,5,5,5,5,5,5,5,2},
        {1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1}
    };

    private GameObject generatedRoot;
    private Dictionary<int, GameObject> prefabMap;

    void Start()
    {
        if (!string.IsNullOrEmpty(manualLevelRootName))
        {
            var old = GameObject.Find(manualLevelRootName);
            if (old != null) 
                DestroyImmediate(old);
        }

        var existed = GameObject.Find(generatedRootName);
        if (existed) 
            DestroyImmediate(existed);

        generatedRoot = new GameObject(generatedRootName);
        prefabMap = new Dictionary<int, GameObject>
        {
            [0] = tile0_Empty,
            [1] = tile1_OutsideCorner,
            [2] = tile2_OutsideWall,
            [3] = tile3_InsideCorner,
            [4] = tile4_InsideWall,
            [5] = tile5_Pellet,
            [6] = tile6_PowerPellet,
            [7] = tile7_TJunction,
            [8] = tile8_GhostExit
        };

        Generate();
        if (adjustCamera) 
            FitCameraToLevel();
    }

    void Generate()
    {
        int rows = levelMap.GetLength(0);
        int cols = levelMap.GetLength(1);

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                int code = levelMap[r, c];
                if (!prefabMap.ContainsKey(code) || prefabMap[code] == null) 
                    continue;

                Vector3 pos = IndexToWorld(r, c);
                Quaternion rot = GetRotationForTile(code, r, c);

                var go = Instantiate(prefabMap[code], pos, rot, generatedRoot.transform);
                go.name = $"T_{code}_{r}_{c}";
                go.transform.localScale = Vector3.one;
            }
        }
    }

    Vector3 IndexToWorld(int row, int col)
    {
        float x = col * cellSize;
        float y = row * cellSize;
        return new Vector3(x, y, 0f);
    }

    Quaternion GetRotationForTile(int code, int r, int c)
    {
        bool up = IsSameFamilyNeighbor(code, r + 1, c);
        bool right = IsSameFamilyNeighbor(code, r, c + 1);
        bool down = IsSameFamilyNeighbor(code, r - 1, c);
        bool left = IsSameFamilyNeighbor(code, r, c - 1);

        
        if (code == 2 || code == 4)
        {
            if (up && down && !left && !right)
                return Quaternion.Euler(0, 0, 90);
            if (left && right && !up && !down)
                return Quaternion.identity;
            if (left || right) 
                return Quaternion.identity;
            return Quaternion.Euler(0, 0, 90);
        }

       
        if (code == 1)
        {
            if (right && up) 
                return Quaternion.identity;           
            if (down && right) 
                return Quaternion.Euler(0, 0, -90); 
            if (left && down) 
                return Quaternion.Euler(0, 0, 180); 
            if (up && left) 
                return Quaternion.Euler(0, 0, 90);     
            return Quaternion.identity;
        }

       
        if (code == 3)
        {          
            if (left && down)
                return Quaternion.identity;           
            if (up && left) 
                return Quaternion.Euler(0, 0, -90);     
            if (right && up)
                return Quaternion.Euler(0, 0, 180);    
            if (down && right)
                return Quaternion.Euler(0, 0, 90);   
            return Quaternion.identity;
        }

      
        if (code == 7)
        {
            int open = 0;
            if (!up) 
                open = 0;
            if (!right)
                open = 1;
            if (!down) 
                open = 2;
            if (!left)
                open = 3;

            switch (open)
            {
                case 0: return Quaternion.identity;
                case 1: return Quaternion.Euler(0, 0, -90);
                case 2: return Quaternion.Euler(0, 0, 180);
                case 3: return Quaternion.Euler(0, 0, 90);
            }
        }

        if (code == 8) 
            return Quaternion.identity;
        return Quaternion.identity;
    }

    bool IsSameFamilyNeighbor(int code, int r, int c)
    {
        if (r < 0 || c < 0 || r >= levelMap.GetLength(0) || c >= levelMap.GetLength(1))
            return false;

        int n = levelMap[r, c];
        HashSet<int> outer = new HashSet<int> { 1, 2, 7 };
        HashSet<int> inner = new HashSet<int> { 3, 4, 7 };

        if (code == 1 || code == 2)
            return outer.Contains(n);
        if (code == 3 || code == 4) 
            return inner.Contains(n);
        if (code == 7) 
            return (outer.Contains(n) || inner.Contains(n));
        return false;
    }

    void FitCameraToLevel()
    {
        var cam = Camera.main;
        if (cam == null || !cam.orthographic) 
            return;

        int rows = levelMap.GetLength(0);
        int cols = levelMap.GetLength(1);

        float width = cols * cellSize;
        float height = rows * cellSize;

        cam.transform.position = new Vector3(width / 2f - cellSize / 2f, height / 2f - cellSize / 2f, -10f);

        float halfHeight = height / 2f + cameraPadding;
        float halfWidthWorld = (width / 2f) * (Screen.height / (float)Screen.width) + cameraPadding;
        cam.orthographicSize = Mathf.Max(halfHeight, halfWidthWorld);
    }
}
