using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{

    private GameObject[] roomTemplates;
    private const int width = 11;
    private const int height = 7;
    Dictionary<int, Vector2[,]> rooms = new Dictionary<int, Vector2[,]>();
    List<Vector2[,]> allGridPositions = new List<Vector2[,]>();

    List<int[,]> allGridNumbers = new List<int[,]>();

    public List<GizmoData> gizmoData = new List<GizmoData>();
    Vector2 gizmoLocations;
    Color gizmoColor = Color.black;
    int cellIndex = 0;

    // List<List<Vector2>> allCells = new List<List<Vector2>>();
    List<GameObject> rocks = new List<GameObject>();
    int rockIndex;
    int roomIndex = 0;

    public GameObject rock;

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
            Vector2 roomPos = r.transform.position;
            int[,] gridNumbers = {
            { 0, 0, 0, 1, 1, 0, 1, 0, 0, 0, 1 },
            { 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1 },
            { 1, 1, 0, 0, 0, 0, 1, 1, 1, 0, 0 },        
            { 0, 0, 1, 1, 1, 0, 1, 0, 0, 1, 0 },
            { 0, 0, 0, 1, 0, 0, 0, 0, 1, 1, 1 },
            { 1, 0, 0, 1, 0, 0, 0, 0, 1, 0, 1 },
            { 1, 1, 0, 1, 1, 0, 0, 0, 0, 0, 1 }
           
        }; ;
            Vector2[,] gridPositions = new Vector2[100, width * height];
            roomPos += initPos - roomOffset;

           // Debug.Log(roomPos);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0, columnIndex = 0; x < width; x++)
                {
                    gridPositions[rowIndex, columnIndex] = new Vector2(x + gridOffset, -y - gridOffset) + roomPos;
                   // gridNumbers[rowIndex, columnIndex] = 0;

                    gizmoData.Add(new GizmoData());
                    gizmoData[cellIndex].gizmoLocation = gridPositions[rowIndex, columnIndex];
                    gizmoData[cellIndex].gizmoColor = gizmoColor;

                    if (gridNumbers[rowIndex, columnIndex] == 1)
                    {
                        gizmoData[cellIndex].gizmoColor = Color.yellow;
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
        }
    }

    // Function to print next generation 
    void nextGeneration(int[,] grid,
                               int M, int N)
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

        for (int i = 0, x = 1; i < M; i++)
        {
            for (int j = 0; j < N; j++)
            {
                if (future[i, j] == 0)
                {
                    Debug.Log("isnul");
                    gizmoData[x * j].gizmoColor = Color.black;
                    grid[i, j] = future[i, j];
                }
                else
                {
                    Debug.Log("iseen");
                    gizmoData[x * j].gizmoColor = Color.yellow;
                    grid[i, j] = future[i, j];
                }
            }
            x++;
        }
    }

        void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            nextGeneration(allGridNumbers[0], height, width);
        }
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        foreach(GizmoData g in gizmoData)
        {
            Gizmos.color = g.gizmoColor;
            Gizmos.DrawSphere(g.gizmoLocation, 0.5f);
        }


        //invisible gizmo's for 0, visible for 1?
    }
}
