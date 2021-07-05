using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public static int coinMultiplier;
    public PlayerData _playerData;
    public LevelData _levelData;

    public AIMotorController[] LevelAI;

    [SerializeField] private Transform StartPoint, FinishPoint;
    [SerializeField] private Transform[] Competitors;
    [SerializeField] private float[] CompetitorsX;
    //[SerializeField] private 
    private void Start()
    {
        ObserverManager.GameWin.AddListener(GameWin);
    }
    private void Update()
    {
        for (int i = 0; i < Competitors.Length; i++)
        {
            CompetitorsX[i] = Competitors[i].transform.position.x;
        }
        Array.Sort(CompetitorsX);
    }
    public void StartGame()
    {
        ObserverManager.GamePlay?.Invoke();

        foreach (AIMotorController item in LevelAI)
            item.isControllable = true;
    }
    private void GameWin()
    {
        _playerData.PlayerCurrentCoin += (_levelData.FakeLevelIndex * 50) * GameManager.coinMultiplier;
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OpenNextLevel()
    {
        _levelData.FakeLevelIndex++;

        if (_levelData.FakeLevelIndex % 5 == 0)
            _levelData.RealLevelIndex = 5;
        else
            _levelData.RealLevelIndex = _levelData.FakeLevelIndex % 5;

        SceneManager.LoadScene("Level" + _levelData.RealLevelIndex, LoadSceneMode.Single);
    }
}
