using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;

/// <summary>
/// Listens for touch events and performs an AR raycast from the screen touch point.
/// AR raycasts will only hit detected trackables like feature points and planes.
///
/// If a raycast hits a trackable, the <see cref="placedPrefab"/> is instantiated
/// and moved to the hit position.
/// </summary>
[RequireComponent(typeof(ARRaycastManager))]
public class PlacementController : MonoBehaviour
{
	public GameObject m_ItemPrefab, m_SpawnObject;
	[SerializeField] AudioSource audioSource;
	private FocusSquare m_FocusSquare;
	private float m_minimumThreshold = 0.001f;
	private Transform m_ItemTransform;

    public static event Action onPlacedObject;
    public static PlacementController Instance;

	public GameObject m_Preloader, m_ScreenSpaceUI;

	bool isInventoryApiCall = false;
	bool isBackBtn = false;

	private void Awake()
    {
        m_FocusSquare = GetComponent<FocusSquare>();
    }

    private void Start()
    {
		Instance = this;
		if (m_SpawnObject != null)
			m_SpawnObject.SetActive(false);
		m_objectPlacementState = PlacementState.NotPlaced;
		GameManager.Instance.safetyWindow.OpenWindow();// call Safetywindow popup
	}

	enum PlacementState
	{
        NotAvailable,
		NotPlaced,
		PlacementInProgress,
		Placed
	};

	public void onBackBtn()
    {
		isBackBtn = true;
		StopAllCoroutines();
    }


	private PlacementState m_objectPlacementState = PlacementState.NotAvailable;

	bool isDownloadFinished = false;
    public void UpdatePlacementState() {

		isDownloadFinished = true;
	}

	public bool isTapHitByUser()
    {
		return m_FocusSquare.isObjectPlaced;

	}

	void FixedUpdate()
	{
		if (isBackBtn)
			return;

		if (m_FocusSquare.SquareState == FocusState.Found)
		{
			switch (m_objectPlacementState)
			{
				case PlacementState.Placed:
                    {
						if (isDownloadFinished && m_SpawnObject != null)
						{
							if (PlaneTrackingController.Instance.isMaterialLoaded == false)
							{
								if (m_SpawnObject != null)
									m_SpawnObject.SetActive(false);
								m_Preloader.SetActive(true);
							}
							else
							{
								if (m_Preloader.activeSelf)
								{
									audioSource.Stop();
									audioSource.Play();
								}

								if (m_Preloader)
									m_Preloader.SetActive(false);

								if (m_SpawnObject != null)
									m_SpawnObject.SetActive(true);
							}

							if (!isInventoryApiCall)
							{ isInventoryApiCall = true; GameManager.Instance.OnCheckToUnlockModule(4); }
						}
					}
					break;
                case PlacementState.NotAvailable:
                    // Debug.Log("Object not available");
                break;
                case PlacementState.NotPlaced:
					if (Utils.WasTouchStartDetected())
					{
						//if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
						{
						    //if (IsPointerOverUIObject() == false)
						    {
								if(!isDownloadFinished)
									m_Preloader.SetActive(true);

								m_Preloader.transform.position = m_FocusSquare.transform.position;

								m_ItemTransform = m_ItemPrefab.transform;
								m_ItemTransform.transform.gameObject.SetActive(true);
								m_ItemTransform.position = m_FocusSquare.placementPose.position;
								//m_ItemTransform.GetChild(0).gameObject.SetActive(true);
								if (m_SpawnObject != null)
									m_SpawnObject.SetActive(true);

								if (audioSource.clip != null)
									audioSource.Play();

								m_ItemTransform.LookAt(Camera.main.transform.position);

								m_FocusSquare.isObjectPlaced = true;
								m_FocusSquare.placementIndicator.SetActive(false);
								Debug.Log("<color=green>Object Placed Once </color>");

								m_ItemTransform.SetPositionAndRotation(m_FocusSquare.placementPose.position, m_FocusSquare.placementPose.rotation);
								m_FocusSquare.PlacementFinished();
								m_objectPlacementState = PlacementState.Placed;

								Debug.Log("Before Object place event called :" + m_ItemPrefab.activeSelf.ToString());

                                if (onPlacedObject != null)
                                {
									Debug.Log("Event Called !! ");
									m_ScreenSpaceUI.SetActive(false);
									onPlacedObject();
                                }
                            }
						}

						if (m_SpawnObject != null && PlaneTrackingController.Instance.isMaterialLoaded == false)
						{
							if (m_SpawnObject != null)
								m_SpawnObject.SetActive(false);
							m_Preloader.SetActive(true);
						}
						else if (m_SpawnObject != null && PlaneTrackingController.Instance.isMaterialLoaded == true)
						{
							m_Preloader.SetActive(false);
							if (m_SpawnObject != null)
								m_SpawnObject.SetActive(true);
						}
					}
				break;
				default:
				break;
			}
		}
	}

	public bool IsPointerOverUIObject()
	{
		PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
		eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		List<RaycastResult> results = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
		return results.Count > 0;
	}

	public void StopPlaneTracking()
	{
		m_FocusSquare.TogglePlaneDetection(false);
	}

	public void StartPlaneTracking()
	{
		m_FocusSquare.TogglePlaneDetection(true);
	}

	public void ResetAll()
	{
		m_FocusSquare.isObjectInProgress = true;
		m_FocusSquare.isObjectPlaced = false;
		m_objectPlacementState = PlacementState.NotPlaced;
		m_ItemPrefab.SetActive(false);
		StartPlaneTracking();
	}

    public void AssignGameObject(GameObject obj)
    {
		m_ItemPrefab = obj;
	}
}
