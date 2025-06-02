public class EnemyController : EnemyMovement
{
    public bool playerDetected { get; private set; }= false;
    
    public enum EnemyState
    {
        Patrolling,
        Looking,
        Chasing,
        Punching
    }

    private EnemyState currentState;
    
    private void Start()
    {
        ChangeState(EnemyState.Patrolling);
    }
    
    void HandleStateTransition()
    {
        switch (currentState)
        {
            case EnemyState.Patrolling:
                MoveToWaypoint();
                break;
            case EnemyState.Looking:
                LookingAround();
                break;
            case EnemyState.Chasing:
                playerDetected = true;
                MoveToPlayer();
                break;
            case EnemyState.Punching:
                Punching();
                break;
        }
    }

    public override void ChangeState(EnemyState newState)
    {
        currentState = newState;
        HandleStateTransition();
    }
    
}
