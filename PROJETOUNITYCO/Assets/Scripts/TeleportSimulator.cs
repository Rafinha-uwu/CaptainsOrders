using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class TeleportSimulator : MonoBehaviour
{
    public XRInteractionManager interactionManager;
    public XRRayInteractor rayInteractor;
    public BaseTeleportationInteractable[] teleportTargets;

    public void SimulateSelectPressAll()
    {
        if (interactionManager == null || rayInteractor == null || teleportTargets == null)
            return;

        IXRSelectInteractor selectInteractor = rayInteractor as IXRSelectInteractor;
        if (selectInteractor == null)
            return;

        foreach (var teleportTarget in teleportTargets)
        {
            if (teleportTarget == null)
                continue;

            IXRSelectInteractable selectInteractable = teleportTarget as IXRSelectInteractable;
            if (selectInteractable == null)
                continue;

            interactionManager.SelectEnter(selectInteractor, selectInteractable);
            interactionManager.SelectExit(selectInteractor, selectInteractable);
        }
    }
}
