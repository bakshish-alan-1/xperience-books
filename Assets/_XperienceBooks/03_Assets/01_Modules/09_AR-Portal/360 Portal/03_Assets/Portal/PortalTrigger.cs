using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PortalTrigger : MonoBehaviour
{
   // [SerializeField]
    //private List<Material> m_mats;
    [SerializeField]
    private Transform m_device;
    private Camera m_cam;
    private MeshRenderer m_meshRend;
    private bool m_isInRoom = false;
    private float m_swapThreshold = -0.05f;

    private bool m_hasUsedDoor = false;

    bool isColliding;


    public AudioSource audioSource;

    private void Awake()
    {
        if (m_device == null) {
            m_device = Camera.main.transform;
        }
    }

    private void Start()
    {
        ChangeMaterials(false);
        m_cam = Camera.main;
        m_meshRend = GetComponent<MeshRenderer>();


    }

    private void Update()
    {

        if (!isColliding)
            return;


        bool inRoom = IsInRoom();

        
        if (m_hasUsedDoor && inRoom != m_isInRoom)
        {
            Debug.Log("In room : " + inRoom);

            m_isInRoom = inRoom;


            if (m_isInRoom)
            {
                Debug.Log("Entering Room");
                 m_meshRend.enabled = false;
                 ChangeMaterials(true);
               
            }
            else
            {
                
                m_meshRend.enabled = false;
                 ChangeMaterials(false);
                Debug.Log("Exit Room");
            }
        }
    }

    private bool IsInRoom()
    {
        bool inRoom = (transform.InverseTransformPoint(m_device.position + m_device.forward).z > m_swapThreshold);
        return inRoom;
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.transform != m_device)
            return;

        m_hasUsedDoor = true;

        isColliding = true;
      //  StartCoroutine("DelayChnageMeshRenderer");
    }

    private void OnTriggerExit(Collider _other)
    {
        if (_other.transform != m_device)
            return;

        if (!IsInRoom())
            m_hasUsedDoor = false;

        isColliding = false;
        StartCoroutine("DelayChnageMeshRenderer");
    }

    private void ChangeMaterials(bool _fullRender)
    {
        try
        {
            if (_fullRender) {
                audioSource.Play();
            }
            else
            {
                audioSource.Stop();
            }

        }
        catch (Exception Ex) {
            Debug.Log("Audio Missng " + Ex);
        }
        var stencilTest = _fullRender ? CompareFunction.NotEqual : CompareFunction.Equal;
        Shader.SetGlobalInt("_StencilTest", (int)stencilTest);
    }


    IEnumerator DelayChnageMeshRenderer()
    {

        yield return new WaitForSeconds(0.1f);
        m_meshRend.enabled = true;

    }


    void OnDestroy()
    {
        //ensure geometry renders in the editor
        ChangeMaterials(true);
    }


}