using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "LevelData")]
public class LevelData : ScriptableObject
{
    public int RealLevelIndex;
    public int FakeLevelIndex;
    public int PerLevelReward;
}
