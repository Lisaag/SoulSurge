using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GridGenerator : MonoBehaviour
{
    private GameObject[] roomTemplates;
    private const int width = 11;
    private const int height = 7;
    public GameObject designerUI;
    public Rigidbody2D playerRigidbody;

    private List<Vector2[,]> allGridPositions = new List<Vector2[,]>();
    private List<int[,]> allGridNumbers = new List<int[,]>();

    List<GizmoData> gizmoData = new List<GizmoData>();
    private Vector2 gizmoLocations;
    private Color gizmoColor = new Color(0, 0, 0, 0);
    private int cellIndex = 0;

    private int roomI = 0;
    private int roomAmount = 0;
    List<Vector2> roomPos = new List<Vector2>();
    private int cellValue = 1;
    public int amountOfIterations = 3;

    public GameObject[] objects;
    public Button generateButton;

    public void GenerateGrid()
    {
        roomTemplates = GameObject.FindGameObjectsWithTag("R");
        Vector2 roomOffset = new Vector2(0.5f, 0.5f);
        float gridOffset = 0.5f;
        Vector2 initPos = new Vector2(-width / 2, height / 2);

        foreach (GameObject r in roomTemplates)
        {
            if (r == roomTemplates[0])
            {
                continue;
            }

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
                        gizmoData[roomAmount].gizmoColor[y, x] = gizmoColor;
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
        generateButton.interactable = false;
    }

    // Function to print next generation 
    public void nextGeneration(int[,] grid,
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
                    gizmoData[roomIndex].gizmoColor[i, j] = gizmoColor;
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
                else if (future[i, j] == 3)
                {
                    gizmoData[roomIndex].gizmoColor[i, j] = Color.green;
                    grid[i, j] = future[i, j];
                }
                else if (future[i, j] == 10)
                {
                    gizmoData[roomIndex].gizmoColor[i, j] = Color.magenta;
                    grid[i, j] = future[i, j];
                }
            }
            x++;
        }
    }

    public void PlaceObjects()
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
                    else if (n[y, x] == 3)
                    {
                        int objectIndex = Random.Range(0, 101);

                        if (objectIndex <= 80)
                        {
                            Instantiate(objects[6], positions[y, x], Quaternion.identity);
                        }
                        else
                        {
                            Instantiate(objects[1], positions[y, x], Quaternion.identity);
                        }
                    }
                    else if (n[y, x] == 10)
                    {
                        Instantiate(objects[objects.Length - 1], positions[y, x], Quaternion.identity);
                    }

                }
            }
            roomNum++;
        }
        playerRigidbody.isKinematic = false;
        designerUI.SetActive(false);
        Time.timeScale = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Space)) { SpawnPentagram(); }
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

        if (roomI > -1 && roomI <= roomPos.Count && roomPos.Count > 0)
        {
            Gizmos.color = new Color(0, 0, 90, 0.3f);
            Gizmos.DrawCube(roomPos[roomI] + new Vector2(5.5f, -3.5f), new Vector2(12, 8));
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

    public void GoToNextRoom()
    {
        if (roomTemplates[roomI + 1].CompareTag("EndingRoom"))
        {
            if (roomTemplates[roomI + 2]) // Room spawned later than end boss room
            {
                roomI += 2;
                Debug.Log("Roomindex: " + roomI);
            }
            else
            {
                return;
            }
        }
        else
        {
            roomI++;
        }
    }

    public void GoToPreviousRoom()
    {
        if (roomI - 1 < 0)
        {
            return;
        }
        roomI--;
        Debug.Log("Roomindex: " + roomI);
    }

    public void ExecuteGameOfLife()
    {
        nextGeneration(allGridNumbers[roomI], height, width, roomI);
    }

    public void NextIteration()
    {
        if (cellValue + 1 > amountOfIterations)
        {
            return;
        }
        cellValue++;
        Debug.Log("cellvalue: " + cellValue);
    }

    public void PreviousIteration()
    {
        if (cellValue - 1 < 0)
        {
            return;
        }
        cellValue--;
        Debug.Log("cellvalue: " + cellValue);
    }

    public void GenerateDungeon()
    {
        SceneManager.LoadScene("Base");
    }

    //creates a new pattern in case room gets empty/designer is not happy with the result
    public void ResetPattern()
    {
        allGridNumbers[roomI] = GeneratePattern(allGridNumbers[roomI]);


        for (int i = 0, x = 1; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (allGridNumbers[roomI][i, j] == 0)
                {
                    // gizmoData[(x * j) + gridSize * roomIndex].gizmoColor = Color.black;
                    gizmoData[roomI].gizmoColor[i, j] = gizmoColor;
                }
                else if (allGridNumbers[roomI][i, j] == 1)
                {
                    gizmoData[roomI].gizmoColor[i, j] = Color.yellow;
                }
                else if (allGridNumbers[roomI][i, j] == 2)
                {
                    gizmoData[roomI].gizmoColor[i, j] = Color.red;
                }
                else if (allGridNumbers[roomI][i, j] == 3)
                {
                    gizmoData[roomI].gizmoColor[i, j] = Color.green;
                }
            }
            x++;
        }
    }

    void SpawnPentagram()
    {
        int[,] roomNumbers = allGridNumbers[roomI];
        for (int y = 1; y < height - 1; y++)
        {
            for (int x = 1; x < width - 1; x++)
            {
                if (roomNumbers[y, x] > 0)
                {
                    continue;
                }
                else
                {
                    if (!CheckNeighbours(roomNumbers, y, x))
                    {
                        continue;
                    }
                    else
                    {
                        //Instantiate(objects[objects.Length - 1], roomPositions[y, x], Quaternion.identity);
                        roomNumbers[y, x] = 10;
                        return;
                    }
                }
            }
        }
    }

    bool CheckNeighbours(int[,] roomNumbers, int y, int x)
    {
        if (roomNumbers[y, x - 1] != 0) { return false; }
        else if (roomNumbers[y, x + 1] != 0) { return false; }
        else if (roomNumbers[y - 1, x - 1] != 0) { return false; }
        else if (roomNumbers[y - 1, x + 1] != 0) { return false; }
        else if (roomNumbers[y - 1, x] != 0) { return false; }
        else if (roomNumbers[y + 1, x - 1] != 0) { return false; }
        else if (roomNumbers[y + 1, x + 1] != 0) { return false; }
        else if (roomNumbers[y + 1, x] != 0) { return false; }

        return true;
    }
}
