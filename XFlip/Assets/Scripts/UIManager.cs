using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public GameObject[] GameUI;

    public Text currentCoinText;
    public Text currentSpeedLevel;
    public Text currentAccelarationLevel;
    public Text currentLeanSpeedLevel;
    public Text RewardMultiplierText;
    public Text TotalRewardText;
    public PlayerData _playerData;

    void Start()
    {
        ObserverManager.GamePrepare.AddListener(ShowPrepareUI);
        ObserverManager.GamePlay.AddListener(ShowGamePlayUI);
        ObserverManager.GameWin.AddListener(ShowWinUI);
        ObserverManager.GameLose.AddListener(ShowLoseUI);

        ObserverManager.RefreshTexts.AddListener(UpdateTexts);

        ObserverManager.RefreshTexts?.Invoke();
        ObserverManager.GamePrepare?.Invoke();
    }
    private void ShowPrepareUI()
    {
        GameUI[0].SetActive(true);
    }
    private void ShowGamePlayUI()
    {
        GameUI[0].SetActive(false);
        GameUI[1].SetActive(true);
    }
    private void ShowWinUI()
    {
        GameUI[1].SetActive(false);
        GameUI[2].SetActive(true);

        RewardMultiplierText.text = "x" + GameManager.coinMultiplier.ToString();
        TotalRewardText.text = "= " + (GameManager.coinMultiplier * 50).ToString();
        ObserverManager.RefreshTexts?.Invoke();
    }
    private void ShowLoseUI()
    {
        GameUI[1].SetActive(false);
        GameUI[3].SetActive(true);
    }
    private void UpdateTexts()
    {
        currentCoinText.text = _playerData.PlayerCurrentCoin.ToString();
        currentSpeedLevel.text = "Level " + _playerData.SpeedLevel.ToString();
        currentAccelarationLevel.text = "Level " + _playerData.AccelarationLevel.ToString();
        currentLeanSpeedLevel.text = "Level " + _playerData.LeanSpeedLevel.ToString();
    }

}
