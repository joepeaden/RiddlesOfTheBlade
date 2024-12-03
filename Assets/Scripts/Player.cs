using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance => _instance;
    private static Player _instance;

    public Character Character { get; private set; }
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

        Character = GetComponent<Character>();
        _rb = GetComponent<Rigidbody2D>();

        Character.OnDeath.AddListener(HandleDeath);
    }

    private void Start()
    {
        GameManager.Instance.OnWaveBegin.AddListener(Reset);
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnWaveBegin.RemoveListener(Reset);
        Character.OnDeath.RemoveListener(HandleDeath);
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

            Character.LookDirection = inputDirection;

            _rb.velocity = inputDirection * Character.MaxSpeed;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Character.Attack();
            }
        }
    }

    public void AddPower(PowerData newPower)
    {
        Character.DamageBuff += newPower.damageBuff;

        if (newPower.aoeMultiplier > 0)
        {
            Character.MultiplyAttackAOE(newPower.aoeMultiplier);
        }

        Character.PushbackBuff += newPower.attackPushbackBuff;
        Character.HPRegenPerSec += newPower.hpRegenPerSec;
        Character.AddSpeedBuff(newPower.speedBuff);
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
