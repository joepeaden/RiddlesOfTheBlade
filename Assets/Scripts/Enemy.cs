using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Character _character;
    private Rigidbody2D _rb;

    private float _decisionTimer;
    private float _attackTimer;
    private Vector3 _targetPosition;

    private void Awake()
    {
        _character = GetComponent<Character>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        GameManager.Instance.OnWaveEnd.AddListener(HandleWaveEnd);
        GameManager.Instance.OnGameOver.AddListener(HandleGameOver);
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnWaveEnd.RemoveListener(HandleWaveEnd);
        GameManager.Instance.OnGameOver.RemoveListener(HandleGameOver);
    }

    private void OnEnable()
    {
        _attackTimer = GetNextAttackTime();
    }

    private void Update()
    {
        _decisionTimer -= Time.deltaTime;
        // don't update pathfinding etc. every single frame; not needed and expensive
        if (_decisionTimer <= 0)
        {
            if ((Player.Instance.transform.position - transform.position).magnitude > _character.Data.enemyStopDistance)
            {
                _targetPosition = new Vector2(Player.Instance.transform.position.x + Random.Range(-_character.Data.enemyPathfindingVariance, _character.Data.enemyPathfindingVariance),
                    Player.Instance.transform.position.y + Random.Range(-_character.Data.enemyPathfindingVariance, _character.Data.enemyPathfindingVariance));
            }

            // randomize the decision timer a little for variety and better performance
            // of course, it would be better to set an offset for each enemy so guaranteed they do not
            // stack up on the same frame by chance, but... this is just a game jam
            _decisionTimer = _character.Data.enemyDecisionInterval + Random.Range(-_character.Data.enemyDecisionVariance, _character.Data.enemyDecisionVariance);
        }

        Vector2 inputDirection = _targetPosition - transform.position;
        if (inputDirection.magnitude > _character.Data.enemyStopDistance)
        {
            inputDirection.Normalize();
            _rb.velocity = inputDirection * _character.Data.enemyMoveVelocity;
            _attackTimer = GetNextAttackTime();
        }
        else
        {
            _attackTimer -= Time.deltaTime;
            if (_attackTimer <= 0)
            {
                _character.Attack();
                _attackTimer = GetNextAttackTime();
            }
        }

        UpdateLookDirection();
    }

    private void HandleWaveEnd()
    {
        gameObject.SetActive(false);
    }

    private void HandleGameOver(bool playerWon)
    {
        gameObject.SetActive(false);
    }

    private void UpdateLookDirection()
    {
        Vector2 targetDirection = Player.Instance.transform.position - transform.position;
        targetDirection.Normalize();

        if (Mathf.Abs(targetDirection.y) > Mathf.Abs(targetDirection.x))
        {
            if (targetDirection.y > 0)
            {
                _character.LookDirection = Vector2.up;
            }
            else
            {
                _character.LookDirection = -Vector2.up;
            }
        }
        else
        {
            if (targetDirection.x > 0)
            {
                _character.LookDirection = Vector2.right;
            }
            else
            {
                _character.LookDirection = -Vector2.right;
            }
        }
    }

    private float GetNextAttackTime()
    {
        // want the minimum attack time to be the attack timer + 0
        return _character.Data.enemyAttackTimer + Random.Range(0, _character.Data.enemyAttackVariance);
    }
}
