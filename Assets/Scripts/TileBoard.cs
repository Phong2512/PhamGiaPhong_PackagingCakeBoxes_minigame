using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBoard : MonoBehaviour
{
    [SerializeField] Tile tilePrefab;
    [SerializeField] TileState[] tileStates;
    [SerializeField] TileGrid grid;
    [SerializeField] List<Tile> tiles;
    private bool isWin = false;
    public int cout { get; private set; }
    public int tileAmout = 16;
    private bool waiting = false;
    private void Awake()
    {
        grid = GetComponentInChildren<TileGrid>();
        tiles = new List<Tile>(tileAmout);
    }
    private void Start()
    {

    }
    //Khoi Tạo tile
    public void CreateTile(int num, int x, int y)
    {
        Tile tile = Instantiate(tilePrefab, grid.transform);
        tile.SetState(tileStates[num]);
        tile.Spawn(grid.GetCell(x, y));
        tiles.Add(tile);
    }
    private void Update()
    {
        if (isWin) return;
        if (!waiting)
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
    }
    private void HandleMoveTiles(Vector2Int direction, int startX, int incrementX, int startY, int incrementY)
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
        tiles.Remove(a);
        a.Merge(b.cell);
        b.SetState(tileStates[2]);
    }
    IEnumerator WaitFowMove()
    {
        waiting = true;
        yield return new WaitForSeconds(0.1f);
        waiting = false;
    }
}
