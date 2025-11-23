using UnityEngine;

public class CharacterLocomotionManager : MonoBehaviour
{
    CharacterManager character;
    [Header ("Grounding")]

    [SerializeField] float gravityForce = -30f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckSphereRadius=1;
    [SerializeField] protected Vector3 yVelocity;
    [SerializeField] protected float groundedYVelocity = -20;
    [SerializeField] protected float fallStartVelocity = -5;
    protected bool  fallingVelocityHasBeenSet = false;
    protected float inAirTimer=0;


    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }
    protected virtual void Update()
    {
        GroundedCheck();
        if (character.isGrounded)
        {
            if(yVelocity.y < 0)
            {
                inAirTimer=0;
                fallingVelocityHasBeenSet=false;
                yVelocity.y = groundedYVelocity;
            }
        }
            else
            {

                if (!fallingVelocityHasBeenSet)
                {
                    fallingVelocityHasBeenSet = true;
                    yVelocity.y = fallStartVelocity;


                }
                inAirTimer= inAirTimer +Time.deltaTime;
                character.animator.SetFloat("inAirTimer", inAirTimer);
                yVelocity.y = gravityForce * Time.deltaTime;

                character.characterController.Move( yVelocity *Time.deltaTime);
            }

        
    }

    protected void GroundedCheck()
    {
        character.isGrounded = Physics.CheckSphere(character.transform.position,groundCheckSphereRadius ,groundLayer);

    }

}
 