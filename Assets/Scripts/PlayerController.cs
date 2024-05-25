using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

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

    public static float breatheAmount;
    public float noiseRadius;
    public float searchArea;
    //public Action interact;

    public Inputs inputs;
    private void Awake()
    {
        inputs = new Inputs();
        inputs.PlayerActions.Movement.started += Movement;
        inputs.PlayerActions.HoldBreath.started += HoldBreath;
        inputs.PlayerActions.HoldBreath.canceled += ctx => TransitionToState(PlayerStates.Idle);
        inputs.PlayerActions.Interaction.started += Interact;

        states.Add(PlayerStates.Idle, new PlayerIdleState(this, PlayerStates.Idle));
        states.Add(PlayerStates.Moving, new PlayerMovingState(this, PlayerStates.Moving));
        states.Add(PlayerStates.HoldBreath, new PlayerHoldBreathState(this, PlayerStates.HoldBreath));
        states.Add(PlayerStates.Interacting, new PlayerInteractionState(this, PlayerStates.Interacting));

        currentState = states[PlayerStates.Idle];
        breatheAmount = 100f;
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
        PlayerController.breatheAmount += Time.deltaTime * 10f;
        PlayerController.breatheAmount = Mathf.Clamp(PlayerController.breatheAmount, 0, 100);
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
        PlayerController.breatheAmount += Time.deltaTime * 10f;
        PlayerController.breatheAmount = Mathf.Clamp(PlayerController.breatheAmount, 0, 100);
        
        Vector2 movement = player.inputs.PlayerActions.Movement.ReadValue<Vector2>();
        player.transform.Translate(new Vector3(movement.x, 0, movement.y) * Time.deltaTime * 2f);
        if(movement == Vector2.zero)
        {
            player.TransitionToState(PlayerController.PlayerStates.Idle);
        }
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
        SoundManager.PlaySound(SoundType.PlayerBreatheIn);
    }

    public override void ExitState()
    {
        SoundManager.PlaySound(SoundType.PlayerBreatheOut);
    }

    public override PlayerController.PlayerStates GetNextState()
    {
        return PlayerController.PlayerStates.Idle;
    }

    public override void UpdateState()
    {
        PlayerController.breatheAmount -= Time.deltaTime * 5f;
        PlayerController.breatheAmount = Mathf.Clamp(PlayerController.breatheAmount, 0, 100);
        Debug.Log(PlayerController.breatheAmount);
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
        colliders = Physics.OverlapSphere(player.transform.position, player.noiseRadius);
        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent(out PC pc))
            {
                //player.interact?.Invoke();
                pc.Interact(pc);
            }
        }
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