using UnityEngine;
using System.Collections.Generic;

public class MazeGenerator : MonoBehaviour
{
    [Header("Maze Settings")]
    public int rows = 10;
    public int columns = 10;
    public float CellWidth = 1f;
    public float CellHeight = 1f;
    public int RandomSeed = 12345;
    public float trapCellRate = 0.1f;
    public int rangeBack=5;

    [Header("Instances")]
    public Transform parentMaze;
    private MazeCell[,] maze;
    public GameObject exitPortal;
    public Transform trees;
    public Transform TrapFloorPrefab;
    public GameObject WallPrefab;
    public GameObject FloorPrefab;
    public GameObject PortalPrefab;
    public GameObject PillarPrefab;
    public Transform enterPortal;
    public Transform BigMapCamera;

    public void StartSpawn()
    {
        RandomSeed = Random.Range(0, int.MaxValue);
        Debug.Log("Maze Seed: " + RandomSeed);
        Random.InitState(RandomSeed);
        TreeSpawner ts = trees.GetComponent<TreeSpawner>();
        ts.ringOffset = rows + 60;
        trees.localPosition = new Vector3(((rows - 1) * 8) / 2,trees.position.y, ((rows - 1) * 8) / 2);
        ts.SpawnTreesAroundMaze();
        BigMapCamera.localPosition= new Vector3(((rows - 1) * 8) / 2, BigMapCamera.position.y, ((rows - 1) * 8) / 2);
        BigMapCamera.GetComponent<Camera>().orthographicSize = rows * 4;
        rangeBack = Mathf.RoundToInt(rows * 1.5f - 17.5f);
        GenerateMaze();
        SpawnMaze();
    }

    private void GenerateMaze()
    {
        rows = Mathf.Max(1, Mathf.Abs(rows));
        columns = Mathf.Max(1, Mathf.Abs(columns));

        maze = new MazeCell[rows, columns];
        for (int r = 0; r < rows; r++)
            for (int c = 0; c < columns; c++)
                maze[r, c] = new MazeCell();

        VisitCell(0, 0);

        List<Vector2Int> candidates = new();

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                if (maze[r, c].IsVisited && !maze[r, c].IsGoal) // goal noktasý tuzak olmasýn
                    candidates.Add(new Vector2Int(r, c));
            }
        }

        int trapCount = Mathf.RoundToInt(candidates.Count * trapCellRate);
        for (int i = 0; i < trapCount; i++)
        {
            int index = Random.Range(0, candidates.Count);
            Vector2Int cellPos = candidates[index];
            maze[cellPos.x, cellPos.y].IsTrap = true;
            candidates.RemoveAt(index);
        }

        SetFurthestGoalCell();
    }

    private void VisitCell(int row, int col)
    {
        MazeCell current = GetCell(row, col);
        current.IsVisited = true;

        List<(int r, int c, Direction dir)> neighbors = new();

        if (col + 1 < columns && !maze[row, col + 1].IsVisited)
            neighbors.Add((row, col + 1, Direction.Right));
        if (row + 1 < rows && !maze[row + 1, col].IsVisited)
            neighbors.Add((row + 1, col, Direction.Front));
        if (col - 1 >= 0 && !maze[row, col - 1].IsVisited)
            neighbors.Add((row, col - 1, Direction.Left));
        if (row - 1 >= 0 && !maze[row - 1, col].IsVisited)
            neighbors.Add((row - 1, col, Direction.Back));

        while (neighbors.Count > 0)
        {
            int index = Random.Range(0, neighbors.Count);
            var (newRow, newCol, dir) = neighbors[index];
            neighbors.RemoveAt(index);

            if (!maze[newRow, newCol].IsVisited)
            {
                RemoveWall(current, maze[newRow, newCol], dir);
                VisitCell(newRow, newCol);
            }
        }
    }

    private void RemoveWall(MazeCell a, MazeCell b, Direction dir)
    {
        switch (dir)
        {
            case Direction.Right:
                a.WallRight = false; b.WallLeft = false; break;
            case Direction.Front:
                a.WallFront = false; b.WallBack = false; break;
            case Direction.Left:
                a.WallLeft = false; b.WallRight = false; break;
            case Direction.Back:
                a.WallBack = false; b.WallFront = false; break;
        }
    }

    private void SetFurthestGoalCell()
    {
        bool[,] visited = new bool[rows, columns];
        int[,] distance = new int[rows, columns];

        Queue<Vector2Int> queue = new();
        queue.Enqueue(new Vector2Int(0, 0));
        visited[0, 0] = true;
        distance[0, 0] = 0;

        List<Vector2Int> allCells = new();
        int maxDepth = 0;

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            MazeCell cell = maze[current.x, current.y];

            foreach (Direction dir in System.Enum.GetValues(typeof(Direction)))
            {
                int newX = current.x;
                int newY = current.y;

                switch (dir)
                {
                    case Direction.Right:
                        if (!cell.WallRight) newY += 1;
                        break;
                    case Direction.Left:
                        if (!cell.WallLeft) newY -= 1;
                        break;
                    case Direction.Front:
                        if (!cell.WallFront) newX += 1;
                        break;
                    case Direction.Back:
                        if (!cell.WallBack) newX -= 1;
                        break;
                }

                if (newX >= 0 && newX < rows && newY >= 0 && newY < columns && !visited[newX, newY])
                {
                    visited[newX, newY] = true;
                    distance[newX, newY] = distance[current.x, current.y] + 1;
                    queue.Enqueue(new Vector2Int(newX, newY));

                    int d = distance[newX, newY];
                    maxDepth = Mathf.Max(maxDepth, d);

                    allCells.Add(new Vector2Int(newX, newY));
                }
            }
        }

        // Filtrele: sadece (maxDepth - rangeBack) ile maxDepth arasý hücreleri al
        int minDepth = Mathf.Max(1, maxDepth - rangeBack);
        List<Vector2Int> candidates = new();

        foreach (var pos in allCells)
        {
            int d = distance[pos.x, pos.y];
            if (d >= minDepth && d <= maxDepth)
            {
                candidates.Add(pos);
            }
        }

        // Önce tüm goal bayraklarýný sýfýrla
        for (int r = 0; r < rows; r++)
            for (int c = 0; c < columns; c++)
                maze[r, c].IsGoal = false;

        if (candidates.Count > 0)
        {
            Vector2Int selected = candidates[Random.Range(0, candidates.Count)];
            maze[selected.x, selected.y].IsGoal = true;
            Debug.Log($"[GOAL] Derinlik: {distance[selected.x, selected.y]} / Max: {maxDepth}");
        }
        else
        {
            Debug.LogWarning("Hedef derinliðe uygun hücre bulunamadý.");
        }
    }


    private MazeCell GetCell(int r, int c)
    {
        return maze[r, c];
    }

    private void SpawnMaze()
    {
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                Vector3 pos = new Vector3(c * CellWidth, 0, r * CellHeight) + transform.position;
                MazeCell cell = maze[r, c];
                GameObject floor;
                if (maze[r, c].IsTrap && TrapFloorPrefab != null && (r>=3 && c>=3) && !maze[r,c].IsGoal)
                {
                    floor = Instantiate(TrapFloorPrefab.gameObject, pos, Quaternion.identity, parentMaze);
                    floor.GetComponentInChildren<TrapFlame>().checkPoint = enterPortal;
                }
                else
                    floor=Instantiate(FloorPrefab, pos, Quaternion.identity, parentMaze);

                if (cell.WallRight)
                    SpawnWall(pos + new Vector3(CellWidth / 2, 0, 0), Quaternion.Euler(0, 90, 0));
                if (cell.WallFront)
                    SpawnWall(pos + new Vector3(0, 0, CellHeight / 2), Quaternion.identity);
                if (cell.WallLeft)
                    SpawnWall(pos + new Vector3(-CellWidth / 2, 0, 0), Quaternion.Euler(0, 270, 0));
                if (cell.WallBack)
                    SpawnWall(pos + new Vector3(0, 0, -CellHeight / 2), Quaternion.Euler(0, 180, 0));
            }
        }

        if (PillarPrefab != null)
        {
            for (int r = 0; r <= rows; r++)
            {
                for (int c = 0; c <= columns; c++)
                {
                    Vector3 pos = new Vector3(c * CellWidth - CellWidth / 2, 0, r * CellHeight - CellHeight / 2) + transform.position;
                    Instantiate(PillarPrefab, pos, Quaternion.identity, parentMaze);
                }
            }
        }

        PlaceExitPortal();

    }

    private void SpawnWall(Vector3 position, Quaternion rotation)
    {
        Instantiate(WallPrefab, position + WallPrefab.transform.position, rotation, parentMaze);
    }

    public class MazeCell
    {
        public bool WallLeft = true;
        public bool WallRight = true;
        public bool WallFront = true;
        public bool WallBack = true;
        public bool IsVisited = false;
        public bool IsGoal = false;

        public bool IsTrap = false;
    }


    public void ClearMaze()
    {
        foreach (Transform child in parentMaze)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in trees)
            Destroy(child.gameObject);

        maze = null;
    }

    private void PlaceExitPortal()
    {
        if (exitPortal == null)
        {
            Debug.LogWarning("Exit portal is not assigned.");
            return;
        }

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                if (maze[r, c].IsGoal)
                {
                    float x = c * CellWidth;
                    float z = r * CellHeight;
                    Vector3 worldPos = new Vector3(x, 0, z) + transform.position;

                    exitPortal.transform.position = worldPos + Vector3.up * 0.5f;
                    return;
                }
            }
        }

        Debug.LogWarning("Goal cell not found!");
    }


    private enum Direction { Right, Front, Left, Back }
}
