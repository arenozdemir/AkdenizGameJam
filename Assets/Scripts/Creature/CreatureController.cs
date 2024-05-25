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
        Search,
        Attack
    }
    private void Awake()
    {
        FindObjectOfType<PlayerController>().makeNoise += Attack;
        PC.makeNoise += Chase;

        agent = GetComponent<NavMeshAgent>();

        states.Add(CreatureState.Patrol, new CreaturePatrolState(this, CreatureState.Patrol));
        states.Add(CreatureState.Chase, new CreatureChaseState(this, CreatureState.Chase));
        states.Add(CreatureState.Search, new CreatureSearchState(this, CreatureState.Search));
        states.Add(CreatureState.Attack, new CreatureAttackState(this, CreatureState.Attack));

        currentState = states[CreatureState.Patrol];
    }
    private void Chase()
    {
        TransitionToState(CreatureState.Chase);
    }
    private void Attack()
    {
        TransitionToState(CreatureState.Attack);
    }
}
public class CreaturePatrolState : BaseState<CreatureController.CreatureState>
{
    CreatureController creature;
    Vector3 nodePos;
    public CreaturePatrolState(CreatureController creature, CreatureController.CreatureState state) : base(state)
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
public class CreatureChaseState : BaseState<CreatureController.CreatureState>
{
    CreatureController creature;
    Vector3 playerPos;
    public CreatureChaseState(CreatureController creature, CreatureController.CreatureState state) : base(state)
    {
        this.creature = creature;
    }

    public override void EnterState()
    {
        playerPos = creature.locationManager.GetPlayerPosition();
        creature.agent.speed = 5f;
        creature.agent.SetDestination(playerPos);
    }

    public override void ExitState()
    {
        
    }

    public override CreatureController.CreatureState GetNextState()
    {
        return CreatureController.CreatureState.Search;
    }

    public override void UpdateState()
    {
        if (Vector3.Distance(creature.transform.position, playerPos) <= 2f)
        {
            creature.TransitionToState(CreatureController.CreatureState.Search);
        }
    }
}
public class CreatureSearchState : BaseState<CreatureController.CreatureState>
{
    CreatureController creature;
    Vector3 nodePos;
    float timer;
    float searchTime;
    int searchCount;
    public CreatureSearchState(CreatureController creature, CreatureController.CreatureState state) : base(state)
    {
        this.creature = creature;
    }

    public override void EnterState()
    {
        timer = 0f;
        searchTime = 5f;
        searchCount = 0;
        nodePos = creature.locationManager.GetSearchNode();
        creature.agent.SetDestination(nodePos);
    }

    public override void ExitState()
    {
        timer = 0f;
        searchCount = 0;
        creature.agent.speed = 1f;
    }

    public override CreatureController.CreatureState GetNextState()
    {
        return CreatureController.CreatureState.Patrol;
    }

    public override void UpdateState()
    {
        if (Vector3.Distance(creature.transform.position, nodePos) <= 1f)
        {
            timer += Time.deltaTime;
            if (timer >= searchTime)
            {
                searchCount++;
                timer = 0;
                SetNewNode();
            }
        }
        else if (searchCount >= 3)
        {
            creature.TransitionToState(CreatureController.CreatureState.Patrol);
        }
    }
    private void SetNewNode()
    {
        nodePos = creature.locationManager.GetSearchNode();
        creature.agent.SetDestination(nodePos);
    }
}
public class CreatureAttackState : BaseState<CreatureController.CreatureState>
{
    CreatureController creature;
    Vector3 playerPos;
    public CreatureAttackState(CreatureController creature, CreatureController.CreatureState state) : base(state)
    {
        this.creature = creature;
    }

    public override void EnterState()
    {
        creature.agent.isStopped = true;
    }

    public override void ExitState()
    {
        creature.agent.isStopped = false;
    }

    public override CreatureController.CreatureState GetNextState()
    {
        return CreatureController.CreatureState.Patrol;
    }

    public override void UpdateState()
    {
        Debug.Log("Attacking");
    }
}
