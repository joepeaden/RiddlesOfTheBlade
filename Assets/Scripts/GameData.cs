using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "MyScriptables/GameData")]
public class GameData : ScriptableObject
{
    public float baseTimer;
    public float spawnInterval;
}
