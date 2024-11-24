using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    [SerializeField] private Character _character;

    private Vector2 currentLookDirection;

    private void Update()
    {
        // no need to update position if haven't moved much
        if (_character.LookDirection != currentLookDirection)
        {
            // move attack collider to direction of look
            if (Mathf.Abs(_character.LookDirection.y) > Mathf.Abs(_character.LookDirection.x))
            {
                if (_character.LookDirection.y > 0)
                {
                    transform.localPosition = Vector2.up;
                }
                else
                {
                    transform.localPosition = -Vector2.up;
                }
            }
            else
            {
                if (_character.LookDirection.x > 0)
                {
                    transform.localPosition = Vector2.right;
                }
                else
                {
                    transform.localPosition = -Vector2.right;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Character"))
        {
            Character newTarget = other.GetComponent<Character>();
            // if they're on the opposing team, set target
            if (newTarget != null && newTarget.IsPlayer != _character.IsPlayer)
            {
                _character.AddTarget(newTarget);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Character"))
        {
            Character newTarget = other.GetComponent<Character>();
            // if they're on the opposing team, set target
            if (newTarget != null && newTarget.IsPlayer != _character.IsPlayer)
            {
                _character.RemoveTarget(newTarget);
            }
        }
    }
}
