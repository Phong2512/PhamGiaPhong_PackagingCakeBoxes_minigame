using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject[] GameScreen;
    [SerializeField] LevelState[] levelStates;
    [SerializeField] Level[] levels;
    [SerializeField] GameObject GamePopup;
    [SerializeField] TileBoard Board;
    private bool isCreateLv = false;
    private int inGameLv = 0;

    #region Instance
    private static GameController instance;
    public static GameController Instance
    {
        get
        {

            if (instance == null)
            {
                instance = FindObjectOfType<GameController>();
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(GameController).Name);
                    instance = singletonObject.AddComponent<GameController>();
                }
            }
            return instance;
        }
    }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
    private void Start()
    {
        EnableHomeScreen();
    }
    #endregion
    public TileBoard GetBoard()
    {
        return Board;
    }
    private void CreateLevel()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            if (!isCreateLv)
            {
                levels[i].SetState(levelStates[i]);

            }

        }
        isCreateLv = true;
    }
    private void EnableScreen(int num)
    {
        foreach (GameObject gameObject in GameScreen)
        {
            gameObject.SetActive(false);
        }
        GameScreen[num].SetActive(true);
    }
    public void EnableGameScreen(int lv)
    {
        EnableScreen(2);
        inGameLv = lv;
    }
    public void EnableLevelSelection()
    {
        EnableScreen(1);
        CreateLevel();

    }
    public void EnableHomeScreen()
    {
        EnableScreen(0);
    }
    public void RestartGame()
    {
        Board.RestartGame();
        levels[inGameLv - 1].StartLvGame();
    }
    public void NextLv()
    {
        Board.RestartGame();
        levels[inGameLv - 1].isClearLv = true;
        levels[inGameLv].Unlock();
        levels[inGameLv].StartLvGame();
    }
    public void ClearGame(bool isWin)
    {
        GamePopup.SetActive(true);
        if (isWin)
        {
            GamePopup.transform.GetChild(0).gameObject.SetActive(true);
            GamePopup.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            GamePopup.transform.GetChild(0).gameObject.SetActive(false);
            GamePopup.transform.GetChild(1).gameObject.SetActive(true);
        }
    }
}
