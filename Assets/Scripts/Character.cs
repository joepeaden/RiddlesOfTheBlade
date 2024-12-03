using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [HideInInspector]
    public UnityEvent OnDeath = new();
    /// <summary>
    /// (int, int) corresponds to (current hp, max hp)
    /// </summary>
    [HideInInspector]
    public UnityEvent<int, int> OnGetHit = new();

    public bool IsPlayer;

    public Character currentTarget { get; set; }
    public Vector2 LookDirection { get; set; }
    public int DamageBuff { get; set; }
    public int PushbackBuff { get; set; }
    public int HPRegenPerSec { get; set; }
    public int MaxSpeed { get; private set; }

    public CharacterData Data => data;
    [SerializeField] private CharacterData data;

    [SerializeField] private Transform attackColliderT;
    [SerializeField] private Transform _charSpriteTransform;
    [SerializeField] private Transform _atkSpriteTransform;

    private HashSet<Character> _targets = new();
    private bool _isDead = false;
    private Rigidbody2D _rb;
    private int currentHP;
    private Vector3 originalAttackColliderScale;
    private float regenTimer;
    private float lastXPos;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        originalAttackColliderScale = attackColliderT.localScale;
    }

    private void OnEnable()
    {
        if (IsPlayer)
        {
            MaxSpeed = data.playerMoveVelocity;
        }
        else
        {
            MaxSpeed = data.enemyMoveVelocity;
        }

        currentHP = GetMaxHP();
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
        else
        {
            // handle health regen
            regenTimer -= Time.deltaTime;

            if (regenTimer <= 0 && (IsPlayer && currentHP < data.playerHP || !IsPlayer && currentHP < data.enemyHP))
            {
                currentHP += HPRegenPerSec;
                regenTimer = 1f;
            }

            UpdateSpriteRotation(_charSpriteTransform);
            UpdateSpriteRotation(_atkSpriteTransform);

            lastXPos = transform.position.x;
        }
    }

    public void UpdateSpriteRotation(Transform spriteTransform)
    {
        if (transform.position.x > lastXPos)
        {
            if (spriteTransform.rotation.eulerAngles.y != 0)
            {
                spriteTransform.eulerAngles = new Vector3(spriteTransform.eulerAngles.x, 0, spriteTransform.eulerAngles.z);
            }
        }
        else if (transform.position.x < lastXPos)
        {
            if (spriteTransform.rotation.eulerAngles.y != 180)
            {
                spriteTransform.eulerAngles = new Vector3(spriteTransform.eulerAngles.x, 180f, spriteTransform.eulerAngles.z);
            }
        }

    }

    public int GetMaxHP()
    {
        return IsPlayer ? data.playerHP : data.enemyHP;
    }

    public void MultiplyAttackAOE(float aoeBuff)
    {
        attackColliderT.localScale *= aoeBuff;
    }

    public void AddSpeedBuff(int buff)
    {
        MaxSpeed += buff;
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

        OnGetHit.Invoke(currentHP, GetMaxHP());

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
