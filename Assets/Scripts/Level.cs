using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LevelState;

public class Level : MonoBehaviour
{
    public LevelState state { get; private set; }
    public int level { get; private set; }
    public int amout { get; private set; }
    public TileInfomation[] tileInfomation;
    public bool isClearLv;
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
    }
    public void StartLvGame()
    {
        if (this.transform.GetChild(1).gameObject.activeSelf) return;
        GameController.Instance.EnableGameScreen(level);
        GameController.Instance.GetBoard().SetAmout(amout);
        for (int i = 0; i < tileInfomation.Length; i++)
        {
            GameController.Instance.GetBoard().Create(tileInfomation[i].id, tileInfomation[i].x_Pos, tileInfomation[i].y_Pos);
        }

    }
}
