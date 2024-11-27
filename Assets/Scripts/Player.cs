using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance => _instance;
    private static Player _instance;

    private Character _character;
    private Rigidbody2D _rb;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Debug.Log("Too many players! Deleting one.");
            Destroy(gameObject);
        }

        _character = GetComponent<Character>();
        _rb = GetComponent<Rigidbody2D>();

        _character.IsPlayer = true;
        _character.OnDeath.AddListener(HandleDeath);
    }

    private void Start()
    {
        GameManager.Instance.OnWaveBegin.AddListener(Reset);
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnWaveBegin.RemoveListener(Reset);
        _character.OnDeath.RemoveListener(HandleDeath);
    }

    private void Update()
    {
        // avoiding needless processing
        if (Input.anyKey)
        {
            Vector2 inputDirection = Vector2.zero;

            if (Input.GetKey(KeyCode.W) && transform.position.y < 0)
            {
                inputDirection += Vector2.up;
            }
            if (Input.GetKey(KeyCode.S) && transform.position.y > -10)
            {
                inputDirection -= Vector2.up;
            }
            if (Input.GetKey(KeyCode.D) && transform.position.x < 10)
            {
                inputDirection += Vector2.right;
            }
            if (Input.GetKey(KeyCode.A) && transform.position.x > -10)
            {
                inputDirection -= Vector2.right;
            }

            // normalize in case two buttons were pressed at once
            inputDirection.Normalize();

            _character.LookDirection = inputDirection;

            _rb.velocity = inputDirection * _character.Data.playerMoveVelocity;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _character.Attack();
            }
        }
    }

    public void AddPower(PowerData newPower)
    {
        _character.DamageBuff += newPower.damageBuff;
        _character.MultiplyAttackAOE(newPower.aoeMultiplier);
        _character.PushbackBuff += newPower.attackPushbackBuff;
    }

    private void HandleDeath()
    {
        GameManager.Instance.GameOver(false);
    }

    private void Reset()
    {
        transform.position = new Vector3(0, 0, 0);
        gameObject.SetActive(true);
    }
}
