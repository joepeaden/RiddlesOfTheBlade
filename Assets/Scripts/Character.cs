using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [HideInInspector]
    public UnityEvent OnDeath = new();

    public Character currentTarget { get; set; }
    public bool IsPlayer { get; set; }
    public Vector2 LookDirection { get; set; }
    public int DamageBuff { get; set; }
    public int PushbackBuff { get; set; }

    public CharacterData Data => data;
    [SerializeField] private CharacterData data;

    [SerializeField] private Transform attackColliderT;

    private HashSet<Character> _targets = new();
    private bool _isDead = false;
    private Rigidbody2D _rb;
    private int currentHP;
    private Vector3 originalAttackColliderScale;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        originalAttackColliderScale = attackColliderT.localScale;
    }

    private void OnEnable()
    {
        if (IsPlayer)
        {
            currentHP = data.playerHP;
        }
        else
        {
            currentHP = data.enemyHP;
        }

        _isDead = false;
        attackColliderT.localScale = originalAttackColliderScale;
        DamageBuff = 0;
        PushbackBuff = 0;
    }

    private void Update()
    {
        // needs to be done here instead of immediately in GetHit because we can't modify the targets collection during iteration
        if (_isDead)
        {
            gameObject.SetActive(false);
            OnDeath.Invoke();
        }
    }

    public void MultiplyAttackAOE(float aoeBuff)
    {
        attackColliderT.localScale *= aoeBuff;
    }

    public int GetKnockbackForce()
    {
        return data.attackPushback + PushbackBuff;
    }

    public int GetAttackDamage()
    {
        return data.baseDamage + DamageBuff;
    }

    public void GetHit(Character attackingCharacter)
    {
        // apply force away from attacking character
        Vector2 forceDirecton = transform.position - attackingCharacter.transform.position;
        _rb.AddForce(forceDirecton * attackingCharacter.GetKnockbackForce());

        currentHP -= attackingCharacter.GetAttackDamage();

        Debug.Log("Got Hit! Current HP: " + currentHP);

        if (currentHP <= 0)
        {
            _isDead = true;
        }
    }

    public void Attack()
    {
        foreach (Character target in _targets)
        {
            target.GetHit(this);
        }
    }

    public void AddTarget(Character newTarget)
    {
        _targets.Add(newTarget);
    }

    public void RemoveTarget(Character targetToRemove)
    {
        _targets.Remove(targetToRemove);
    }
}
