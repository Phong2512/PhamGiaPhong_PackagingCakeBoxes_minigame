using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class TileBoard : MonoBehaviour
{
    [SerializeField] Tile tilePrefab;
    [SerializeField] TileState[] tileStates;
    [SerializeField] TileGrid grid;
    [SerializeField] List<Tile> tiles;
    [SerializeField] List<Tile> blocks;
    [SerializeField] Text timerText;
    private float timeLeft;
    public static TileBoard instance;
    private bool isDone = false;
    public int amout { get; private set; }
    private int cout;
    private int tileAmout = 16;
    private int blockAmout = 16;
    private bool waiting = false;
    private int index;
    private bool stopTime;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        blocks = new List<Tile>(blockAmout);
        tiles = new List<Tile>(tileAmout);
    }
    private void Start()
    {
        index = 0;
        cout = 0;
        timeLeft = 45f;
        stopTime = false;

    }
    public void SetAmout(int amout)
    {
        this.amout = amout;
    }

    //Khoi Tạo tile
    public void Create(int num, int x, int y)
    {
        index++;
        if (num == 0)
        {
            if (blocks.Count >= index)
            {
                GetOldBlocks(index - 1, num, x, y);
            }
            else
            {
                CreateBlocks(num, x, y);
            }
        }
        else
        {
            if (tiles.Count >= index)
            {
                GetOldTile(index - 1, num, x, y);
            }
            else
            {
                CreateTile(num, x, y);
            }
        }
    }

    private void CreateTile(int num, int x, int y)
    {
        Tile tile = Instantiate(tilePrefab, grid.transform);
        tile.SetState(tileStates[num]);
        tile.Spawn(grid.GetCell(x, y));
        tiles.Add(tile);


    }
    private void GetOldTile(int index, int num, int x, int y)
    {

        if (!tiles[index].gameObject.activeInHierarchy)
        {
            Tile tile = TileBoard.instance.tiles[index];
            tile.SetState(tileStates[num]);
            tile.Spawn(grid.GetCell(x, y));
            tile.gameObject.SetActive(true);
        }

    }
    private void CreateBlocks(int num, int x, int y)
    {

        Tile block = Instantiate(tilePrefab, grid.transform);
        block.SetState(tileStates[num]);
        block.Spawn(grid.GetCell(x, y));
        blocks.Add(block);

    }
    private void GetOldBlocks(int index, int num, int x, int y)
    {

        if (!blocks[index].gameObject.activeInHierarchy)
        {
            Tile block = TileBoard.instance.blocks[index];
            block.gameObject.SetActive(true);
            block.Spawn(grid.GetCell(x, y));
        }

    }
    private void Update()
    {
        if (isDone) return;
        Countdown();
        if (!waiting)
        {
            MoveWithKey();
        }
    }

    private void MoveWithKey()
    {

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            HandleMoveTiles(Vector2Int.up, 0, 1, 1, 1);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            HandleMoveTiles(Vector2Int.down, 0, 1, grid.height - 2, -1);
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            HandleMoveTiles(Vector2Int.left, 1, 1, 0, 1);
        }

        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            HandleMoveTiles(Vector2Int.right, grid.width - 2, -1, 0, 1);
        }
    }
    public void LeanSwipeMove(int num)
    {
        if (!waiting)
        {
            switch (num)
            {
                case 0:
                    {
                        HandleMoveTiles(Vector2Int.up, 0, 1, 1, 1);
                        break;
                    }
                case 1:
                    {
                        HandleMoveTiles(Vector2Int.right, grid.width - 2, -1, 0, 1);
                        break;
                    }
                case 2:
                    {
                        HandleMoveTiles(Vector2Int.down, 0, 1, grid.height - 2, -1);
                        break;
                    }
                case 3:
                    {
                        HandleMoveTiles(Vector2Int.left, 1, 1, 0, 1);
                        break;
                    }
            }
        }
    }
    public void HandleMoveTiles(Vector2Int direction, int startX, int incrementX, int startY, int incrementY)
    {
        bool changed = false;
        for (int x = startX; x >= 0 && x < grid.width; x += incrementX)
        {
            for (int y = startY; y >= 0 && y < grid.height; y += incrementY)
            {
                TileCell cell = grid.GetCell(x, y);
                if (cell.occupied)
                {
                    changed = MoveTile(cell.tile, direction);
                }
            }
        }
        if (changed)
        {
            StartCoroutine(WaitFowMove());
        }
    }
    private bool MoveTile(Tile tile, Vector2Int direction)
    {
        TileCell newCell = null;
        TileCell adjacent = grid.GetAdjacentCell(tile.cell, direction);

        while (adjacent != null)
        {
            if (adjacent.occupied)
            {
                if (CanMerge(tile, adjacent.tile))
                {
                    Merge(tile, adjacent.tile);
                }
                break;
            }
            newCell = adjacent;
            adjacent = grid.GetAdjacentCell(adjacent, direction);
        }
        if (newCell != null)
        {
            tile.MoveTo(newCell);
            return true;
        }
        return false;
    }
    //Kiem tra id > 0
    private bool CanMerge(Tile a, Tile b)
    {
        if (a.id > 0 && b.id > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void Merge(Tile a, Tile b)
    {
        a.Merge(b.cell, tileStates[2]);
        cout++;
    }
    IEnumerator WaitFowMove()
    {
        waiting = true;
        yield return new WaitForSeconds(0.1f);
        if (CheckWin())
        {
            isDone = true;
            GameController.Instance.ClearGame(CheckWin());
        }
        else
        {
            waiting = false;
        }
    }
    private bool CheckWin()
    {
        return cout == amout;
    }
    public void RestartGame()
    {
        index = 0;
        waiting = false;
        cout = 0;
        timeLeft = 45f;
        isDone = false;
        foreach (Tile tile in tiles)
        {
            if (tile.gameObject.activeInHierarchy)
            {
                tile.gameObject.SetActive(false);
            }
        }
        foreach (Tile block in blocks)
        {
            if (block.gameObject.activeInHierarchy)
            {
                block.RestartBlock();
                block.gameObject.SetActive(false);
            }
        }
    }
    private void Countdown()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(timeLeft / 60f);
            int seconds = Mathf.FloorToInt(timeLeft % 60f);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        }
        else
        {
            isDone = true;
            timerText.text = "00:00";
            GameController.Instance.ClearGame(CheckWin());
        }


    }
}
