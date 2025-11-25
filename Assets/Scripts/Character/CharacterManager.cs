using UnityEngine;
using Unity.Netcode;
using System.Collections;
using System.Collections.Generic;

public class CharacterManager : NetworkBehaviour
{
    [Header("Status")]
    public NetworkVariable<bool> isDead=new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);
    [HideInInspector] public CharacterController characterController;
    [HideInInspector] public Animator animator;
    [HideInInspector] public CharacterNetworkManager characterNetworkManager;
    [HideInInspector] public CharacterEffectsManager characterEffectsManager;
    [HideInInspector] public CharacterAnimatorManager characterAnimatorManager;
    [HideInInspector] public CharacterCombatManager characterCombatManager;

    [Header("Flags")]
    public bool isPerformingAction = false;
    public bool applyRootMotion = false;
    public bool canRotate = true;
    public bool canMove = true;
    public bool isGrounded = true;



    PlayerManager playerManager; 

    protected virtual void Awake()
    {
        DontDestroyOnLoad(gameObject);
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerManager = GetComponent<PlayerManager>(); 
        characterNetworkManager = GetComponent<CharacterNetworkManager>();
        characterEffectsManager = GetComponent<CharacterEffectsManager>();
        characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
        characterCombatManager= GetComponent<CharacterCombatManager>();
        
    }

    protected virtual void Start()
    {
        IgnoreMyOwnColliders();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsOwner)
        {
            if (PlayerCamara.instance != null)
            {
                PlayerCamara.instance.player = playerManager;
            }
        }
    }

    protected virtual void Update()
    {
        animator.SetBool("isGrounded",isGrounded );
        if (IsOwner)
        {
            characterNetworkManager.networkPosition.Value = transform.position;
            characterNetworkManager.networkRotation.Value = transform.rotation;
        }
        else
        {
            //... tu código de interpolación
            transform.position = Vector3.SmoothDamp
                (transform.position,
                characterNetworkManager.networkPosition.Value,
                ref characterNetworkManager.networkPositionVelocity,
                characterNetworkManager.networkPositionSmoothTime);

            transform.rotation = Quaternion.Slerp
                (transform.rotation,
                characterNetworkManager.networkRotation.Value,
                characterNetworkManager.networkRotationSmoothTime);
        }
    }

    protected virtual void LateUpdate()
    {
        if (!IsOwner)
            return;

        PlayerCamara.instance.HandleAllCameraActions();
    }
  

    public virtual IEnumerator ProcessDeathEvent(bool manuallySelectDamageAnimation = false)
    {
        if (IsOwner)
        {
            characterNetworkManager.currentHealth.Value=0;
            isDead.Value =true;

            if (!manuallySelectDamageAnimation)
            {
                characterAnimatorManager.PlayerTargetActionAnimation("Dead_01",true);
            }
        }


        yield return new WaitForSeconds(5);
    }

    protected virtual void IgnoreMyOwnColliders()
    {
        Collider characterControllerCollider= GetComponent<Collider>();
        Collider[] damageableCharacterColliders= GetComponentsInChildren<Collider>();

        List<Collider> ignoreColliders = new List<Collider>();
        
        foreach(var Collider in damageableCharacterColliders)
        {
            ignoreColliders.Add(Collider);
        }

        ignoreColliders.Add(characterControllerCollider);

        foreach(var Collider in ignoreColliders)
        {
            foreach(var otherCollider in ignoreColliders)
            {
                Physics.IgnoreCollision(Collider,otherCollider, true);
            }
        }
    }
}