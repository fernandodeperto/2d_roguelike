using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{
    public float _moveTime = .1f;
    public LayerMask _blockingLayer;

    private BoxCollider2D __boxCollider;
    private Rigidbody2D __rigidBody;

    private float __inverseMoveTime;

	protected virtual void Start ()
    {
        __boxCollider = GetComponent<BoxCollider2D>();
        __rigidBody = GetComponent<Rigidbody2D>();
        __inverseMoveTime = 1f / _moveTime;
	}

    protected IEnumerator SmoothMovement(Vector3 end)
    {
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(__rigidBody.position, end, __inverseMoveTime * Time.deltaTime);
            __rigidBody.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }
    }

    protected abstract void OnCantMove<T>(T component)
        where T : Component;

    protected bool Move(int xDirection, int yDirection, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDirection, yDirection);

        __boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, _blockingLayer);
        __boxCollider.enabled = true;

        if (hit.transform == null)
        {
            StartCoroutine(SmoothMovement(end));
            return true;
        }

        return false;
    }

    protected virtual void AttemptMove<T>(int xDirection, int yDirection)
        where T : Component
    {
        RaycastHit2D hit;
        bool canMove = Move(xDirection, yDirection, out hit);

        if (hit.transform == null)
            return;

        T hitComponent = hit.transform.GetComponent<T>();

        if (!canMove && hitComponent != null)
            OnCantMove(hitComponent);
    }
}
