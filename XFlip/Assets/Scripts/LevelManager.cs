using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public LevelData _levelData;
    private void Awake()
    {
        if (_levelData.FakeLevelIndex % 5 == 0)
            _levelData.RealLevelIndex = 5;
        else
            _levelData.RealLevelIndex = _levelData.FakeLevelIndex % 5;

        SceneManager.LoadScene("Level" + _levelData.RealLevelIndex, LoadSceneMode.Single);
    }


}
