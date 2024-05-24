using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : StateManager<PlayerController.PlayerStates>
{
    public enum PlayerStates
    {
        Idle,
        Moving,
        Running,
        HoldBreath,
        Interacting
    }
    public Action<PC> OnInteract;
    public Action<PC> OutInteract;
    
    public float noiseRadius;
    public float searchArea;

    public Inputs inputs;
    private void Awake()
    {
        inputs = new Inputs();
        inputs.PlayerActions.Movement.started += Movement;
        inputs.PlayerActions.HoldBreath.started += HoldBreath;
        inputs.PlayerActions.Interaction.started += Interact;

        states.Add(PlayerStates.Idle, new PlayerIdleState(this, PlayerStates.Idle));
        states.Add(PlayerStates.Moving, new PlayerMovingState(this, PlayerStates.Moving));
        states.Add(PlayerStates.HoldBreath, new PlayerHoldBreathState(this, PlayerStates.HoldBreath));
        states.Add(PlayerStates.Interacting, new PlayerInteractionState(this, PlayerStates.Interacting));

        currentState = states[PlayerStates.Idle];
    }
    private void Movement(InputAction.CallbackContext ctx)
    {
        TransitionToState(PlayerStates.Moving);
    }
    private void HoldBreath(InputAction.CallbackContext ctx)
    {
        TransitionToState(PlayerStates.HoldBreath);
    }
    private void Interact(InputAction.CallbackContext ctx)
    {
        TransitionToState(PlayerStates.Interacting);
    }
    private void OnEnable()
    {
        inputs.Enable();
    }
    private void OnDisable()
    {
        inputs.Disable();
    }
}
public class PlayerIdleState : BaseState<PlayerController.PlayerStates>
{
    PlayerController player;
    public PlayerIdleState(PlayerController player, PlayerController.PlayerStates state) : base(state)
    {
        this.player = player;
    }

    public override void EnterState()
    {
        Debug.Log("Idle");
    }

    public override void ExitState()
    {

    }

    public override PlayerController.PlayerStates GetNextState()
    {
        return PlayerController.PlayerStates.Moving;
    }

    public override void UpdateState()
    {

    }
}

public class PlayerMovingState : BaseState<PlayerController.PlayerStates>
{
    PlayerController player;
    public PlayerMovingState(PlayerController player, PlayerController.PlayerStates state) : base(state)
    {
        this.player = player;
    }

    public override void EnterState()
    {
        
    }

    public override void ExitState()
    {

    }

    public override PlayerController.PlayerStates GetNextState()
    {
        return PlayerController.PlayerStates.Idle;
    }

    public override void UpdateState()
    {
        Vector2 movement = player.inputs.PlayerActions.Movement.ReadValue<Vector2>();
        player.transform.Translate(new Vector3(movement.x, 0, movement.y) * Time.deltaTime * 2f);
        
    }
}

public class PlayerHoldBreathState : BaseState<PlayerController.PlayerStates>
{
    PlayerController player;
    public PlayerHoldBreathState(PlayerController player, PlayerController.PlayerStates state) : base(state)
    {
        this.player = player;
    }

    public override void EnterState()
    {
        
    }

    public override void ExitState()
    {

    }

    public override PlayerController.PlayerStates GetNextState()
    {
        return PlayerController.PlayerStates.Idle;
    }

    public override void UpdateState()
    {

    }
}

public class PlayerInteractionState : BaseState<PlayerController.PlayerStates>
{
    PlayerController player;
    Collider[] colliders;
    PC pc;
    public PlayerInteractionState(PlayerController player, PlayerController.PlayerStates state) : base(state)
    {
        this.player = player;
    }

    public override void EnterState()
    {
        colliders = Physics.OverlapSphere(player.transform.position, player.searchArea);
        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent(out PC pc))
            {
                this.pc = pc;
                player.OnInteract?.Invoke(pc);
            }
        }
    }

    public override void ExitState()
    {
        player.OutInteract?.Invoke(pc);
        colliders = null;
    }

    public override PlayerController.PlayerStates GetNextState()
    {
        return PlayerController.PlayerStates.Idle;
    }

    public override void UpdateState()
    {

    }
}