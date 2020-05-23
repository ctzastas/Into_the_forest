//Gives all our Interact Methods a type and allows us to
//interact with specific objects depending on our interactor
using System;
using UnityEngine;

public abstract class IInteractor : MonoBehaviour
{
    protected bool canInteract;
    protected bool allowMovement;
    public virtual bool CanInteract() 
    {
        return canInteract;
    }
    public virtual void InteractCondition() 
    {
       
    }
    public virtual void Interact() 
    {
    }
}
