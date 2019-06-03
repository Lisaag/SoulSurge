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
    private int roomAmount = 0;
    List<Vector2> roomPos = new List<Vector2>();
    private int cellValue = 1;

    public GameObject[] objects;

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

            int[,] gridNumbers = new int[height, width];
            gridNumbers = GeneratePattern(gridNumbers);

            Vector2[,] gridPositions = new Vector2[100, width * height];
            roomPos[roomAmount] += initPos - roomOffset;
            gizmoData.Add(new GizmoData());

            // Debug.Log(roomPos);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0, columnIndex = 0; x < width; x++)
                {
                    gridPositions[rowIndex, columnIndex] = new Vector2(x + gridOffset, -y - gridOffset) + roomPos[roomAmount];

                    gizmoData[roomAmount].gizmoLocation[rowIndex, columnIndex] = gridPositions[y, x];

                    if (gridNumbers[rowIndex, columnIndex] == 1)
                    {
                        gizmoData[roomAmount].gizmoColor[y, x] = Color.yellow;
                    }
                    else
                    {
                        gizmoData[roomAmount].gizmoColor[y, x] = Color.black;
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
                if ((grid[l, m] == cellValue) &&
                            (aliveNeighbours < 2 * cellValue))
                    future[l, m] = 0;

                // Cell dies due to over population 
                else if ((grid[l, m] == cellValue) &&
                             (aliveNeighbours > 3 * cellValue))
                    future[l, m] = 0;

                // A new cell is born 
                else if ((grid[l, m] == 0) &&
                            (aliveNeighbours == 3 * cellValue))
                    future[l, m] = cellValue;

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
                    // gizmoData[(x * j) + gridSize * roomIndex].gizmoColor = Color.black;
                    gizmoData[roomIndex].gizmoColor[i, j] = Color.black;
                    grid[i, j] = future[i, j];
                }
                else if (future[i, j] == 1)
                {
                    gizmoData[roomIndex].gizmoColor[i, j] = Color.yellow;
                    grid[i, j] = future[i, j];
                }
                else if (future[i, j] == 2)
                {
                    gizmoData[roomIndex].gizmoColor[i, j] = Color.red;
                    grid[i, j] = future[i, j];
                }
            }
            x++;
        }
    }

    void PlaceObjects()
    {
        int roomNum = 0;
        foreach (int[,] n in allGridNumbers)
        {
            Debug.Log("roomNum: " + roomNum);
            Vector2[,] positions = allGridPositions[roomNum];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (n[y, x] == 1)
                    {
                        Instantiate(objects[0], positions[y, x], Quaternion.identity);
                    }
                    else if (n[y, x] == 2)
                    {
                        int objectIndex = Random.Range(0, 101);

                        if (objectIndex <= 50)
                        {
                            Instantiate(objects[2], positions[y, x], Quaternion.identity);
                        }
                        else if (objectIndex > 50 && objectIndex <= 100)
                        {
                            int i = Random.Range(3, 6);
                            Instantiate(objects[i], positions[y, x], Quaternion.identity);
                        }
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
            GoToNextRoom();
        }

        else if (Input.GetKeyDown(KeyCode.P))
        {
            GoToPreviousRoom();
        }

        else if (Input.GetKeyDown(KeyCode.G))
        {
            ExecuteGameOfLife();
        }

        else if (Input.GetKeyDown(KeyCode.Return))
        {
            PlaceObjects();
        }

        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            NextIteration();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            PreviousIteration();
        }

        //Reset -- generate new pattern
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ResetPattern();
        }
    }

    void OnDrawGizmos()
    {
        //Draw sphere's to represent on which position, what kind of object will spawn
        foreach (GizmoData g in gizmoData)

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Gizmos.color = g.gizmoColor[i, j];
                    Gizmos.DrawSphere(g.gizmoLocation[i, j], 0.5f);
                }
            }
    }


    //Generates initial pattern for each iteration, also used to reset room with a new pattern
    int[,] GeneratePattern(int[,] grid)
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                //don't overlap cells that are already filled
                if (grid[i, j] > 0 && grid[i, j] < cellValue)
                {
                    continue;
                }

                int gridValue = Random.Range(0, 2);
                if (gridValue == 1)
                {
                    grid[i, j] = cellValue;
                }
                else
                {
                    grid[i, j] = 0;
                }
            }
        }

        return grid;
    }

    void GoToNextRoom()
    {
        roomI++;
        Debug.Log("Roomindex: " + roomI);
    }

    void GoToPreviousRoom(){
        roomI--;
        Debug.Log("Roomindex: " + roomI);
    }

    void ExecuteGameOfLife()
    {
        nextGeneration(allGridNumbers[roomI], height, width, roomI);
    }

    void NextIteration()
    {
        cellValue++;
        Debug.Log("cellvalue: " + cellValue);
    }

    void PreviousIteration()
    {
        cellValue--;
        Debug.Log("cellvalue: " + cellValue);
    }

    //creates a new pattern in case room gets empty/designer is not happy with the result
    void ResetPattern()
    {
        allGridNumbers[roomI] = GeneratePattern(allGridNumbers[roomI]);


        for (int i = 0, x = 1; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (allGridNumbers[roomI][i, j] == 0)
                {
                    // gizmoData[(x * j) + gridSize * roomIndex].gizmoColor = Color.black;
                    gizmoData[roomI].gizmoColor[i, j] = Color.black;
                }
                else if (allGridNumbers[roomI][i, j] == 1)
                {
                    gizmoData[roomI].gizmoColor[i, j] = Color.yellow;
                }
                else if (allGridNumbers[roomI][i, j] == 2)
                {
                    gizmoData[roomI].gizmoColor[i, j] = Color.red;
                }
            }
            x++;
        }
    }
}
