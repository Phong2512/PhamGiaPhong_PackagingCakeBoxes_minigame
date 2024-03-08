using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Tile : MonoBehaviour
{
    public TileState state { get; private set; }
    public TileCell cell { get; private set; }
    public int id { get; private set; }
    private Image image;
    private RectTransform rectTransform;
    private void Awake()
    {
        image = this.GetComponent<Image>();
        rectTransform = this.GetComponent<RectTransform>();
    }
    // Tu?y chi?nh scptbObj cho tile
    public void SetState(TileState state)
    {
        this.state = state;
        id = state.tile_ID;
        image.sprite = state.tile_SPT;
        rectTransform.sizeDelta = new Vector2(state.width, state.height);

    }
    //Set ô trong cô?t lu?c spawn
    public void Spawn(TileCell cell)
    {
        if (this.cell != null)
        {
            this.cell.tile = null;
        }
        this.cell = cell;
        this.cell.tile = this;
        transform.position = cell.transform.position;
    }
    //Ha?m di chuyê?n
    public void MoveTo(TileCell cell)
    {
        if (id <= 0) return;
        if (this.cell != null)
        {
            this.cell.tile = null;
        }
        this.cell = cell;
        this.cell.tile = this;
        this.transform.DOMove(cell.transform.position, 0.1f).SetEase(Ease.OutSine);
    }
    public void Merge(TileCell cell)
    {
        if (this.cell != null)
        {
            this.cell.tile = null;
        }
        this.cell = null;
        this.id = -1;
        this.transform.DOMove(cell.transform.position, 0.1f).SetEase(Ease.OutSine).OnComplete(() =>
        {
            this.gameObject.SetActive(false);
        });
    }
    public void RestartBlock()
    {
        if (this.cell != null)
        {
            this.cell.tile = null;
        }
        this.cell = null;

    }
}
