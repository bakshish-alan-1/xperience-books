using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public enum FocusState
{
    Initializing,
    Finding,
    Found
}

public class FocusSquare : MonoBehaviour
{
    public GameObject placementIndicator;

    public bool isObjectPlaced = false;
    public bool isObjectInProgress = true;

    private FocusState squareState;
    public FocusState SquareState
    {
        get
        {
            return squareState;
        }
        set
        {
            squareState = value;
            placementIndicator.SetActive(squareState == FocusState.Found);
            placementIndicator.SetActive(squareState != FocusState.Found);
        }
    }

   
    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
        m_ARPlaneManager = GetComponent<ARPlaneManager>();

      //  m_ARPointCloudManager = GetComponent<ARPointCloudManager>();
    }


    // Use this for initialization
    void Start()
    {
        SquareState = FocusState.Initializing;
       
    }


    void Update()
    {

        if (isObjectInProgress) {
            UpdatePlacementPose();
        }

        if (!isObjectPlaced) {
            UpdatePlacementIndicator();
        }
    }



    private void UpdatePlacementIndicator()
    {
//        Debug.Log("Update Indicator");
            if (placementPoseIsValid)
            {
                SquareState = FocusState.Found;
                placementIndicator.SetActive(true);
                placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
            }
            else
            {
                SquareState = FocusState.Finding;
                placementIndicator.SetActive(false);
            }
    }

    public float findingSquareDist = 0.5f;

    private void UpdatePlacementPose()
    {
//        Debug.Log("Placement Pos Updates");
            Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, findingSquareDist);
            if (m_RaycastManager.Raycast(screenCenter, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                placementPoseIsValid = s_Hits.Count > 0;
                if (placementPoseIsValid)
                {
                    placementPose = s_Hits[0].pose;
                    var cameraForward = Camera.current.transform.forward;
                    var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
                    placementPose.rotation = Quaternion.LookRotation(cameraBearing);
                }
            }
    }


    public void TogglePlaneDetection(bool val)
    {
        m_ARPlaneManager.enabled = val;

       // m_ARPointCloudManager.enabled = val;

        //  string planeDetectionMessage = "";
        if (m_ARPlaneManager.enabled)
        {
            // planeDetectionMessage = "Disable Plane Detection and Hide Existing";
            SetAllPlanesActive(true);
        }
        else
        {
            //    planeDetectionMessage = "Enable Plane Detection and Show Existing";
            SetAllPlanesActive(false);
        }

        // if (togglePlaneDetectionText != null)
        //    togglePlaneDetectionText.text = planeDetectionMessage;
    }

    void SetAllPlanesActive(bool value)
    {
        foreach (var plane in m_ARPlaneManager.trackables)
            plane.gameObject.SetActive(value);
        //Destroy(plane.gameObject);

    }

    bool PlanesFound()
    {
        if (m_ARPlaneManager == null)
            return false;

        return m_ARPlaneManager.trackables.count > 0;
    }


    public void PlacementFinished() {
        if (isObjectInProgress)
        {
            isObjectInProgress = false;
        }
    }

    public void PlacementReset()
    {
        if (!isObjectInProgress)
        {
            isObjectInProgress = true;
        }
    }



    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    ARRaycastManager m_RaycastManager;

    ARPlaneManager m_ARPlaneManager;
    //ARPointCloudManager m_ARPointCloudManager;

    public Pose placementPose;
    private bool placementPoseIsValid = false;




}
