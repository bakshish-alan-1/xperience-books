using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

public class FaceMeshController : MonoBehaviour
{
    ARFace arFace;

    void OnEnable()
    {
#if UNITY_IOS
        arFace = transform.GetComponent<ARFace>();
        
        arFace.updated += OnUpdated;
        ARSession.stateChanged += OnSystemStateChanged;
#endif
    }

    void OnDisable()
    {
#if UNITY_IOS
        arFace.updated -= OnUpdated;
        ARSession.stateChanged -= OnSystemStateChanged;
#endif
    }

    private void OnUpdated(ARFaceUpdatedEventArgs args)
    {
        UpdateVisibility();
    }

    private void OnSystemStateChanged(ARSessionStateChangedEventArgs args)
    {
        UpdateVisibility();
    }

    private void UpdateVisibility()
    {
        var visible = enabled && (arFace.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking) && (ARSession.state > ARSessionState.Ready);
        SetVisible(visible);
    }

    private void SetVisible(bool visible)
    {
        if(SceneManager.GetActiveScene().buildIndex == 6) // Face neck hole scene
        {
            if (FaceNeckController.Instance.cameraBtn != null)
            {
                if (arFace.transform.childCount > 1 && arFace.transform.GetChild(1).gameObject != null)
                    arFace.transform.GetChild(1).gameObject.SetActive(visible);

                //if (FaceNeckController.Instance.isTextureAvailable == false)
                    //FaceNeckController.Instance.LoadingUI.SetActive(visible);

                FaceNeckController.Instance.cameraBtn.SetActive(visible);
            }
        }
        else // 3D face Prop scene
        {
            if (FacePropertyController.Instance.m_RootObject != null)
            {
                FacePropertyController.Instance.m_RootObject.SetActive(visible);
                if (FacePropertyController.Instance.m_ScreenShotCameraBtn != null)
                    FacePropertyController.Instance.m_ScreenShotCameraBtn.SetActive(visible);
            }
        }
    }
}
