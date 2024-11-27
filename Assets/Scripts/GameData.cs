using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GameData", menuName = "MyScriptables/GameData")]
public class GameData : ScriptableObject
{
    public float baseTimer;
    public float spawnInterval;

    public List<PowerData> powers = new();
}
