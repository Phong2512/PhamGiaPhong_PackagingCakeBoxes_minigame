using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static LevelState;

public class Level : MonoBehaviour
{
    public LevelState state { get; private set; }
    public int level { get; private set; }
    public int amout { get; private set; }
    public TileInfomation[] tileInfomation;
    public bool isClearLv;
    public bool upRow;
    private void Awake()
    {
        this.GetComponent<Button>().onClick.AddListener(StartLvGame);
    }
    private void OnEnable()
    {
        if (isClearLv)
        {
            this.transform.GetChild(0).gameObject.SetActive(true);
        }


    }
    public void Unlock()
    {
        this.transform.GetChild(1).gameObject.SetActive(false);
    }
    public void SetState(LevelState state)
    {
        this.state = state;
        level = state.level;
        amout = state.amout;
        tileInfomation = state.tileInfomation;
        upRow = state.upRow;
    }
    public void StartLvGame()
    {
        if (this.transform.GetChild(1).gameObject.activeSelf) return;
        GameController.Instance.EnableGameScreen(level);
        GameController.Instance.GetBoard().SetAmout(amout);
        GameController.Instance.GetBoard().SetUpRow(upRow);
        for (int i = 0; i < tileInfomation.Length; i++)
        {
            GameController.Instance.GetBoard().Create(tileInfomation[i].id, tileInfomation[i].x_Pos, tileInfomation[i].y_Pos);
        }

    }
}
