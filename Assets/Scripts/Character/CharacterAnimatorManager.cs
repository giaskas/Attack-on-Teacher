using UnityEngine;
using Unity.Netcode;
public class CharacterAnimatorManager : MonoBehaviour

{
    CharacterManager character;

    int vertical;
    int horizontal;

    [Header("Damage Animations")]
    public string hit_Forward = "hit_Forward";
    public string hit_Backward = "hit_Backward";
    public string hit_Left = "hit_Left";
    public string hit_Right = "hit_Right";

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
    }
    public void UpdateAnimatorMovementParameters(float horizontalValue, float verticalValue, bool isSprinting)
    {
        float horizontalAmount = horizontalValue; 
        float verticalAmount = verticalValue;
        if (isSprinting)
        {
            verticalAmount = 2;
        }
        character.animator.SetFloat(horizontal, horizontalAmount, 0.1f, Time.deltaTime);
        character.animator.SetFloat(vertical, verticalAmount, 0.1f, Time.deltaTime);
    }

    public virtual void PlayerTargetActionAnimation(
        string targetAnimation,
        bool isPerformingAction = true,
        bool applyRootMotion = true,
        bool canRotate = false,
        bool canMove = false,
        string overrideLayerName = "Action Override") // <--- 1. Nuevo parámetro con valor por defecto
    {
        character.applyRootMotion = applyRootMotion;

        // 2. Obtenemos el índice del layer basado en el nombre que pasamos
        int layerIndex = character.animator.GetLayerIndex(overrideLayerName);

        // 3. IMPORTANTE: Pasamos 'layerIndex' al CrossFade. 
        // Si no lo pasas, Unity intenta adivinar o usa la capa base.
        character.animator.CrossFade(targetAnimation, 0.1f, layerIndex);

        character.isPerformingAction = isPerformingAction;
        character.canRotate = canRotate;
        character.canMove = canMove;

        // Nota: Si tu RPC de red también necesita saber la capa para reproducirlo en otros clientes,
        // tendrías que actualizar el RPC también. Por ahora, esto arregla tu cliente local.
        character.characterNetworkManager.NotifyTheServerOfActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
    }

    public virtual void PlayerTargetAttackActionAnimation(AttackType attackType,
        string targetAnimation,
        bool isPerformingAction = true,
        bool applyRootMotion = true,
        bool canRotate = false,
        bool canMove = false,
        string overrideLayerName = "Action Override") 
    {

        //saber si le pica varias al boton de atacar para saber si hace combos
        //saber que tipo de ataque es (light attack or heavy attack)
        //
        character.characterCombatManager.currentAttackType = attackType;
        character.applyRootMotion = applyRootMotion;
        int layerIndex = character.animator.GetLayerIndex(overrideLayerName);
        character.animator.CrossFade(targetAnimation, 0.1f, layerIndex);
        character.isPerformingAction = isPerformingAction;
        character.canRotate = canRotate;
        character.canMove = canMove;

        character.characterNetworkManager.NotifyTheServerOfAttackActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
    }

}
