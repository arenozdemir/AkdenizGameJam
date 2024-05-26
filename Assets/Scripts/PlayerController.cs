using DG.Tweening;
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
        Interacting,
        Dead
    }

    public static float breatheAmount;
    public float noiseRadius;
    public float searchArea;
    public Action makeNoise;
    public Animator animator;

    public Inputs inputs;
    private void Awake()
    {
        inputs = new Inputs();
        animator = GetComponent<Animator>();
        
        inputs.PlayerActions.Movement.started += Movement;
        inputs.PlayerActions.HoldBreath.started += HoldBreath;
        inputs.PlayerActions.HoldBreath.canceled += ctx => TransitionToState(PlayerStates.Idle);
        inputs.PlayerActions.Interaction.started += Interact;

        states.Add(PlayerStates.Idle, new PlayerIdleState(this, PlayerStates.Idle));
        states.Add(PlayerStates.Moving, new PlayerMovingState(this, PlayerStates.Moving));
        states.Add(PlayerStates.HoldBreath, new PlayerHoldBreathState(this, PlayerStates.HoldBreath));
        states.Add(PlayerStates.Interacting, new PlayerInteractionState(this, PlayerStates.Interacting));
        states.Add(PlayerStates.Dead, new PlayerDeathState(this, PlayerStates.Dead));

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
    public bool CreatureDetecting()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, noiseRadius);
        foreach (var col in colliders)
        {
            if (col.TryGetComponent(out CreatureController creature))
            {
                return true;
            }
        }
        return false;
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
        if (player.CreatureDetecting())
        {
            player.makeNoise?.Invoke();
            player.TransitionToState(PlayerController.PlayerStates.Dead);
        }
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
        player.animator.SetBool("isWalking", true);
    }

    public override void ExitState()
    {
        player.animator.SetBool("isWalking", false);
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
        if (movement != Vector2.zero)
        {
            Vector3 moveDirection = new Vector3(movement.x, 0, movement.y).normalized;
            player.transform.Translate(moveDirection * Time.deltaTime, Space.World);

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            player.transform.rotation = Quaternion.Lerp(player.transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
        else
        {
            player.TransitionToState(PlayerController.PlayerStates.Idle);
        }
        if (player.CreatureDetecting())
        {
            player.makeNoise?.Invoke();
            player.TransitionToState(PlayerController.PlayerStates.Dead);
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
        if (PlayerController.breatheAmount <= 0) player.TransitionToState(PlayerController.PlayerStates.Idle);
        Debug.Log(PlayerController.breatheAmount);
    }
}

public class PlayerInteractionState : BaseState<PlayerController.PlayerStates>
{
    PlayerController player;
    Collider[] colliders;
    InterfaceInteractable interactable;
    public PlayerInteractionState(PlayerController player, PlayerController.PlayerStates state) : base(state)
    {
        this.player = player;
    }

    public override void EnterState()
    {
        colliders = Physics.OverlapSphere(player.transform.position, player.noiseRadius);
        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent(out InterfaceInteractable interactable))
            {
                this.interactable = interactable;
                Transform targetLoc = collider.GetComponentInChildren<InteractLocation>().transform;
                player.transform.DOMove(targetLoc.position, 3f).OnComplete(() => interactable.Interact());
                player.transform.DOLookAt(targetLoc.position, 3f).onComplete += () => player.animator.SetBool("isInteract",true);
            }
        }
    }

    public override void ExitState()
    {
        interactable.OutInteract();
        player.animator.SetBool("isInteract", false);
    }

    public override PlayerController.PlayerStates GetNextState()
    {
        return PlayerController.PlayerStates.Idle;
    }

    public override void UpdateState()
    {
        if (player.CreatureDetecting())
        {
            player.makeNoise?.Invoke();
            player.TransitionToState(PlayerController.PlayerStates.Dead);
        }
    }
}

public class PlayerDeathState : BaseState<PlayerController.PlayerStates>
{
    PlayerController player;
    public PlayerDeathState(PlayerController player, PlayerController.PlayerStates state) : base(state)
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
        Debug.Log("Death state");
    }
}