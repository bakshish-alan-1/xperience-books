using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Ecommerce
{

    public class WindowManager : MonoBehaviour
    {
        [System.Serializable]
        public class WindowItems
        {
            //get name of page for animation
            public string m_PageName;
            public GameObject m_windowGameobject;
        }


        public List<WindowItems> _Windows = new List<WindowItems>();



        private GameObject m_currentWindow;
        private GameObject m_NexWindow;


        private Animator m_CurrentAnimation;
        private Animator m_NextAnimator;

        public int m_CurrentWindowIndex = -1;
        public int m_newWindowIndex;


        public string InAnimation = "";
        public string OutAnimation = "";

        // Start is called before the first frame update
        void Start()
        {

           // m_currentWindow = _Windows[m_CurrentWindowIndex].m_windowGameobject;
         //   m_CurrentAnimation = m_currentWindow.GetComponent<Animator>();

          //  m_CurrentAnimation.Play(InAnimation);
        }

       
        //Open window on base of name or index
        public void OpenWindows(string windowName)
        {
            for (int i = 0; i < _Windows.Count; i++)
            {
                if (_Windows[i].m_PageName == windowName)
                {
                    m_newWindowIndex = i;
                }
            }

            if (m_CurrentWindowIndex == -1)
            {
                m_CurrentWindowIndex = m_newWindowIndex;
                m_currentWindow = _Windows[m_CurrentWindowIndex].m_windowGameobject;
                m_CurrentAnimation = m_currentWindow.GetComponent<Animator>();
                m_CurrentAnimation.Play(InAnimation);
                return;
            }

            if (m_newWindowIndex != m_CurrentWindowIndex)
            {
                m_currentWindow = _Windows[m_CurrentWindowIndex].m_windowGameobject;
                m_CurrentAnimation = m_currentWindow.GetComponent<Animator>();

                m_CurrentWindowIndex = m_newWindowIndex;

                m_NexWindow = _Windows[m_CurrentWindowIndex].m_windowGameobject;
                m_NextAnimator = m_NexWindow.GetComponent<Animator>();

                m_CurrentAnimation.Play(OutAnimation);
                m_NextAnimator.Play(InAnimation);
            }
        }
    }

    }