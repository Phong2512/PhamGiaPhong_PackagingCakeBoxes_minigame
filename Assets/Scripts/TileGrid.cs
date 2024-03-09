using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGrid : MonoBehaviour
{
    public TileRow[] rows { get; set; }
    public TileCell[] cells { get; set; }
    public int size => cells.Length;
    public int height => rows.Length;
    public int width => size / height;
    private bool isUpRow = false;
    private void Awake()
    {
        rows = GetComponentsInChildren<TileRow>();
        cells = GetComponentsInChildren<TileCell>();
    }
    private void Start()
    {
        for (int y = 0; y < rows.Length; y++)
        {
            for (int x = 0; x < rows[y].cells.Length; x++)
            {
                rows[y].cells[x].coordinates = new Vector2Int(x, y);
            }
        }
    }

    public TileCell GetCell(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return rows[y].cells[x];
        }
        else
        {
            return null;
        }
    }
    //Lâ?y ô liê?n kê?
    public TileCell GetAdjacentCell(TileCell cell, Vector2Int direction)
    {
        Vector2Int coordinates = cell.coordinates;
        coordinates.x += direction.x;
        coordinates.y -= direction.y;
        return GetCell(coordinates.x, coordinates.y);
    }
    //Random cô?t co?n trô?ng
    //public TileCell GetRandomEmptyCell(int x, int y)
    //{
    //    //int index = Random.Range(0, cells.Length);
    //    //int startIndex = index;

    //    //while (cells[index].occupied)
    //    //{
    //    //    index++;
    //    //    if (index >= cells.Length)
    //    //    {
    //    //        index = 0;
    //    //    }
    //    //    if (index == startIndex)
    //    //    {
    //    //        Debug.LogError("Tile in full cell!!!!!");
    //    //        return null;
    //    //    }
    //    //}
    //    return GetCell(x, y);
    //}
    public void UpdateRow(bool isUp)
    {
        if (isUpRow != isUp)
        {
            this.transform.GetChild(3).gameObject.SetActive(isUp);
            rows = GetComponentsInChildren<TileRow>();
            cells = GetComponentsInChildren<TileCell>();
            LocateCell();
            isUpRow = isUp;
        }
    }
    private void LocateCell()
    {
        for (int y = 0; y < rows.Length; y++)
        {
            for (int x = 0; x < rows[y].cells.Length; x++)
            {
                rows[y].cells[x].coordinates = new Vector2Int(x, y);
            }
        }
    }
}
