using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
    [SerializeField]
    private IInteractor interactor;

    public void Interact()
    {

        if (interactor.CanInteract())
        {
            interactor.Interact();
            GetComponentInParent<PlayerInput>().SetPlayerState(PlayerInput.PlayerState.Interacting);
        }
    }
    private void Update()
    {
        interactor.InteractCondition();
    }
    public void SetInteractor(IInteractor interactor) 
    {
        this.interactor = interactor;
    }
    public IInteractor GetInteractor() 
    {
        return interactor;
    }
}
