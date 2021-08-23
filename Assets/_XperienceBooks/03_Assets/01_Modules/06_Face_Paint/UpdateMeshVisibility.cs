using Ecommerce.Category;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class UpdateMeshVisibility : MonoBehaviour
{
    public MeshRenderer[] meshRenderers;
    public SkinnedMeshRenderer[] skinnedMeshRenderers;

    private ARFace arFace;

    private void SetVisible(bool visible)
    {
        if (visible == true)
            FaceController.Instance.m_additionalUI.gameObject.SetActive(visible);
        
        FaceController.Instance.isFaceFound = visible;

        foreach (MeshRenderer mesh in meshRenderers)
        {
            if(mesh != null)
                mesh.enabled = visible;
        }

        foreach(SkinnedMeshRenderer skinnedMesh in skinnedMeshRenderers)
        {
            if(skinnedMesh != null)
                skinnedMesh.enabled = visible;
        }

        if(visible == true)
        {
            FaceController.Instance.setMaterial();
        }
    }

    private void UpdateVisibility()
    {
        var visible = enabled && (arFace.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking) && (ARSession.state > ARSessionState.Ready);
        SetVisible(visible);
    }

    private void OnEnable()
    {
        arFace = transform.GetComponent<ARFace>();

        UpdateVisibility();
        arFace.updated += OnUpdated;
        ARSession.stateChanged += OnSystemStateChanged;
    }

    private void OnDisable()
    {
        arFace.updated -= OnUpdated;
        ARSession.stateChanged -= OnSystemStateChanged;
    }

    private void OnSystemStateChanged(ARSessionStateChangedEventArgs args)
    {
        UpdateVisibility();
    }

    private void OnUpdated(ARFaceUpdatedEventArgs args)
    {
        UpdateVisibility();
    }
}