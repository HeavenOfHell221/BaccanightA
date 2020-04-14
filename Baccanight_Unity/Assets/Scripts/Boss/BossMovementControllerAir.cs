using UnityEngine;

public class BossMovementControllerAir : MovementControllerAir
{
    public Vector2 Destination { get; private set; }
    public Vector2 Origin { get; private set; }
    public bool IsMoving { get; private set; }

    private void Start()
    {
        Origin = Destination = transform.position;
    }

    protected override void FixedUpdate()
    {
        IsMoving = Rigidbody.velocity.magnitude > GameConstants.LimitDicretePosition;
    }

    public override void ApplyMovement()
    {
        Origin = transform.position;
        Vector3 direction = (Destination - Origin).normalized;
        Rigidbody.velocity = direction * Speed;
    }

    private void OnDisable()
    {
        Rigidbody.velocity = Vector3.zero;
    }

    private void OnEnable()
    {
        Origin = Destination = transform.position;
    }

    public void NewDestination(Vector2 destination)
    {
        Destination = destination;
    }

    public float DistanceFromDestination(Vector2 destination)
    {
        return Vector2.Distance(destination, transform.position);
    }
}
