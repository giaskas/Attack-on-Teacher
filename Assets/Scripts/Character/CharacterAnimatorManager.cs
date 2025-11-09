using UnityEngine;
using Unity.Netcode;
public class CharacterAnimatorManager : MonoBehaviour

{
    CharacterManager character;

    int vertical;
    int horizontal;
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
        bool isPerformingAction,
        bool applyRootMotion = true,
        bool canRotate = false,
        bool canMove = false)
    {
        character.applyRootMotion = applyRootMotion;


        int layerIndex = character.animator.GetLayerIndex("Action Override");
        character.animator.CrossFade(targetAnimation, 0.1f, layerIndex);


        character.isPerformingAction = isPerformingAction;
        character.canRotate = canRotate;
        character.canMove = canMove;

        character.characterNetworkManager.NotifyTheServerOfActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
        

        
    }

}
