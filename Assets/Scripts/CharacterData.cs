using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "MyScriptables/CharacterData")]
public class CharacterData : ScriptableObject
{
    [Header("General")]
    public int attackPushback;
    public int baseDamage;

    [Header("Player")]
    public int playerHP;
    public int playerMoveVelocity;

    [Header("Enemy")]
    public int enemyHP;
    public int enemyMoveVelocity;
    public float enemyDecisionInterval;
    public float enemyStopDistance;
    public float enemyDecisionVariance;
    public float enemyPathfindingVariance;
    public float enemyAttackTimer;
    public float enemyAttackVariance;
}
