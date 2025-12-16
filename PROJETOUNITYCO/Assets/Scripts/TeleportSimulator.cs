using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class SimulateTeleportPress : MonoBehaviour
{
    public XRInteractionManager interactionManager;
    public XRRayInteractor rayInteractor;
    public BaseTeleportationInteractable teleportTarget;

    public void SimulateSelectPress()
    {
        if (interactionManager == null || rayInteractor == null || teleportTarget == null)
            return;

        IXRSelectInteractor selectInteractor = rayInteractor as IXRSelectInteractor;
        IXRSelectInteractable selectInteractable = teleportTarget as IXRSelectInteractable;

        if (selectInteractor == null || selectInteractable == null)
            return;

        interactionManager.SelectEnter(selectInteractor, selectInteractable);
        interactionManager.SelectExit(selectInteractor, selectInteractable);
    }
}
