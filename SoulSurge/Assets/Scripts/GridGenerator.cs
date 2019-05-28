using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{

    private GameObject[] roomTemplates;
    private const int width = 11;
    private const int height = 7;

    private List<Vector2[,]> allGridPositions = new List<Vector2[,]>();
    private List<int[,]> allGridNumbers = new List<int[,]>();

   List<GizmoData> gizmoData = new List<GizmoData>();
    private Vector2 gizmoLocations;
    private Color gizmoColor = Color.black;
    private int cellIndex = 0;

    private int roomI = 0;
    private int roomAmount;
    List<Vector2> roomPos = new List<Vector2>();

    public Camera camera;

    public GameObject stone;

    void Start()
    {

    }

    public void GenerateGrid()
    {
        roomTemplates = GameObject.FindGameObjectsWithTag("R");
        Vector2 roomOffset = new Vector2(0.5f, 0.5f);
        float gridOffset = 0.5f;
        Vector2 initPos = new Vector2(-width / 2, height / 2);

        foreach (GameObject r in roomTemplates)
        {
            int rowIndex = 0;
            roomPos.Add(r.transform.position);
            int[,] gridNumbers = {
            { 0, 0, 0, 1, 1, 0, 1, 0, 0, 0, 1 },
            { 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1 },
            { 1, 1, 0, 0, 0, 0, 1, 1, 1, 0, 0 },        
            { 0, 0, 1, 1, 1, 0, 1, 0, 0, 1, 0 },
            { 0, 0, 0, 1, 0, 0, 0, 0, 1, 1, 1 },
            { 1, 0, 0, 1, 0, 0, 0, 0, 1, 0, 1 },
            { 1, 1, 0, 1, 1, 0, 0, 0, 0, 0, 1 }
           
        };
            Vector2[,] gridPositions = new Vector2[100, width * height];
            roomPos[roomAmount] += initPos - roomOffset;
            gizmoData.Add(new GizmoData());

            // Debug.Log(roomPos);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0, columnIndex = 0; x < width; x++)
                {
                    gridPositions[rowIndex, columnIndex] = new Vector2(x + gridOffset, -y - gridOffset) + roomPos[roomAmount];

                    gizmoData[roomAmount].gizmoLocation[rowIndex, columnIndex] = gridPositions[rowIndex, columnIndex];
                    gizmoData[roomAmount].gizmoColor[roomI, columnIndex] = gizmoColor;

                    if (gridNumbers[rowIndex, columnIndex] == 1)
                    {
                        gizmoData[roomAmount].gizmoColor[roomI, columnIndex] = Color.yellow;
                    }

                    cellIndex++;
                    columnIndex++;
                }
                rowIndex++;
            }

            allGridNumbers.Add(gridNumbers);
            allGridPositions.Add(gridPositions);
       
            int randomRow = Random.Range(2, height - 2);
            int randomColumn = Random.Range(2, width - 2);

            roomAmount++;
        }
    }

    // Function to print next generation 
    void nextGeneration(int[,] grid,
                               int M, int N, int roomIndex)
    {
        int[,] future = new int[M, N];

        // Loop through every cell 
        for (int l = 1; l < M - 1; l++)
        {
            for (int m = 1; m < N - 1; m++)
            {

                // finding no Of Neighbours 
                // that are alive 
                int aliveNeighbours = 0;
                for (int i = -1; i <= 1; i++)
                    for (int j = -1; j <= 1; j++)
                        aliveNeighbours +=
                                grid[l + i, m + j];

                // The cell needs to be subtracted 
                // from its neighbours as it was  
                // counted before 
                aliveNeighbours -= grid[l, m];

                // Implementing the Rules of Life 

                // Cell is lonely and dies 
                if ((grid[l, m] == 1) &&
                            (aliveNeighbours < 2))
                    future[l, m] = 0;

                // Cell dies due to over population 
                else if ((grid[l, m] == 1) &&
                             (aliveNeighbours > 3))
                    future[l, m] = 0;

                // A new cell is born 
                else if ((grid[l, m] == 0) &&
                            (aliveNeighbours == 3))
                    future[l, m] = 1;

                // Remains the same 
                else
                    future[l, m] = grid[l, m];
            }
        }

        int gridSize = width * height;

        for (int i = 0, x = 1; i < M; i++)
        {
            for (int j = 0; j < N; j++)
            {
                if (future[i, j] == 0)
                {
                    Debug.Log("isnul"+ ((x * j)));
                    // gizmoData[(x * j) + gridSize * roomIndex].gizmoColor = Color.black;
                    gizmoData[roomIndex].gizmoColor[i, j] = Color.black;
                    grid[i, j] = future[i, j];
                }
                else
                {
                    Debug.Log("iseen");
                    gizmoData[roomIndex].gizmoColor[i, j] = Color.yellow;
                    grid[i, j] = future[i, j];
                }
            }
            x++;
        }
    }

    void PlaceObjects()
    {
        int roomNum = 0;
        foreach(int[,] n in allGridNumbers)
        {
            Vector2[,] positions = allGridPositions[roomNum];
            for(int y = 0; y < height; y++)
            {
                for(int x = 0; x < width; x++)
                {
                    if(n[y, x] == 1)
                    {
                        Instantiate(stone, positions[y, x], Quaternion.identity);
                    }
                }
            }
            roomNum++;
        }
    }

        void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            roomI++;
            Debug.Log("Roomindex: " + roomI);
          //  Vector3 newCameraPos = new Vector3(roomPos[roomIndex].x, roomPos[roomIndex].y, -10);
          //  camera.transform.position = newCameraPos;
        }

        else if (Input.GetKeyDown(KeyCode.P))
        {
            roomI--;
            Debug.Log("Roomindex: " + roomI);
          //  Vector3 newCameraPos = new Vector3(roomPos[roomIndex].x, roomPos[roomIndex].y, -10);
           // camera.transform.position = newCameraPos;
        }

        else if (Input.GetKeyDown(KeyCode.G))
        {
            nextGeneration(allGridNumbers[roomI], height, width, roomI);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            PlaceObjects();
        }
    }

    void OnDrawGizmos()
    {      
        // Draw a yellow sphere at the transform's position
        foreach(GizmoData g in gizmoData)
        
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Gizmos.color = g.gizmoColor[i, j];
                    Gizmos.DrawSphere(g.gizmoLocation[i, j], 0.5f);
                }
            }
        //invisible gizmo's for 0, visible for 1?
    }
}
