using UnityEngine;
using UnityEngine.AI;

public class CreatureController : StateManager<CreatureController.CreatureState>
{
    public LocationManager locationManager;
    public NavMeshAgent agent;
    public enum CreatureState
    {
        Chase,
        Patrol,
        Search
    }
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        states.Add(CreatureState.Patrol, new CreaturePatrolState(this, CreatureState.Patrol, locationManager));

        currentState = states[CreatureState.Patrol];
    }

}
public class CreaturePatrolState : BaseState<CreatureController.CreatureState>
{
    CreatureController creature;
    Vector3 nodePos;
    public CreaturePatrolState(CreatureController creature, CreatureController.CreatureState state, LocationManager locationManager) : base(state)
    {
        this.creature = creature;
    }

    public override void EnterState()
    { 
        nodePos = creature.locationManager.GetNodeToPatrol();
        creature.agent.SetDestination(nodePos);
    }
    
    public override void ExitState()
    {
        
    }

    public override CreatureController.CreatureState GetNextState()
    {
        return CreatureController.CreatureState.Chase;
    }

    public override void UpdateState()
    {
        if (Vector3.Distance(creature.transform.position, nodePos) <= 2f)
        {
            SetNewNode();
            return;
        }
    }

    private void SetNewNode()
    {
        nodePos = creature.locationManager.GetNodeToPatrol();
        creature.agent.SetDestination(nodePos);
    }
}
