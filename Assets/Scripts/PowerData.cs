using UnityEngine;

[CreateAssetMenu(fileName = "PowerData", menuName = "MyScriptables/PowerData")]
public class PowerData : ScriptableObject
{
    public string riddle;
    public string answer;
    public int attackPushbackBuff;
    public int damageBuff;
    public float aoeMultiplier;
    public int speedBuff;
    public int hpRegenPerSec;
}
