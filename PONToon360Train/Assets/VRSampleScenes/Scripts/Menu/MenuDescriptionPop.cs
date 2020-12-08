using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VRStandardAssets.Utils;


namespace VRStandardAssets.Menu
{
    // This script is for loading scenes from the main menu.
    // Each 'button' will be a rendering showing the scene
    // that will be loaded and use the SelectionRadial.
    [RequireComponent(typeof(AudioSource))] //added
    public class MenuDescriptionPop : MonoBehaviour
    {
        public event Action<MenuDescriptionPop> OnButtonSelected;                   // This event is triggered when the selection of the button has finished.


        [SerializeField] private string m_SceneToLoad;                      // The name of the scene to load.
        [SerializeField] private VRCameraFade m_CameraFade;                 // This fades the scene out when a new scene is about to be loaded.
        [SerializeField] private SelectionRadial m_SelectionRadial;         // This controls when the selection is complete.
        [SerializeField] private VRInteractiveItem m_InteractiveItem;       // The interactive item for where the user should click to load the level.
        [SerializeField] public Image informationImage;                     // Information UI that will pop up.
        [SerializeField] public Text informationText;
        [SerializeField] private Animation m_InteractiveAnimation;
        [SerializeField] private Animator m_InteractiveAnimator;
        [SerializeField] public string m_informationBoxClip;
        public AudioClip PlaySound;
        

        private bool m_GazeOver;                                            // Whether the user is looking at the VRInteractiveItem currently.
        private bool m_ToggleInformation;
        private AudioSource audioSrc;
        private AudioSource[] allAudioSources;

        private void Awake()
        {
            m_ToggleInformation = false;
            informationImage.enabled = false;
            if (informationText != null)
            {
                informationText.enabled = false;
            }
            m_InteractiveAnimation = GetComponent<Animation>();
            m_InteractiveAnimator = GetComponent<Animator>();
            audioSrc = GetComponent<AudioSource>();
        }

        private void OnEnable ()
        {
            m_InteractiveItem.OnOver += HandleOver;
            m_InteractiveItem.OnOut += HandleOut;
            m_SelectionRadial.OnSelectionComplete += HandleSelectionComplete;
           // audioSrc.PlayOneShot(PlaySound);
            
        }


        private void OnDisable ()
        {
            m_InteractiveItem.OnOver -= HandleOver;
            m_InteractiveItem.OnOut -= HandleOut;
            m_SelectionRadial.OnSelectionComplete -= HandleSelectionComplete;
        }
        
        private void HandleOver()
        {
            // When the user looks at the rendering of the scene, show the radial.
            m_SelectionRadial.Show();
            m_GazeOver = true;
            //if(m_InteractiveAnimation != null)
            //{
            //    m_InteractiveAnimation.Play("EyeIconClip");
            //}
        }

        private void HandleOut()
        {
            // When the user looks away from the rendering of the scene, hide the radial.
            m_SelectionRadial.Hide();
            m_GazeOver = false;
            //if(m_InteractiveAnimation != null)
            //{
            //    m_InteractiveAnimation.Rewind("EyeIconClip");
            //    m_InteractiveAnimation.Play("EyeIconClip");
            //    m_InteractiveAnimation.Sample();
            //    m_InteractiveAnimation.Stop("EyeIconClip");
            //}
        }

        private void HandleSelectionComplete()
        {
            // If the user is gazing at the button, flip flop between showing the information and hiding it after each gaze.
            if (m_GazeOver == true)
            {
                m_ToggleInformation = true;
                if(informationImage.enabled == true)
                {
                    m_ToggleInformation = false;
                    informationImage.enabled = false;
                    informationText.enabled = false;
                    audioSrc.clip = PlaySound;
                    audioSrc.Stop();
                }
            }
            if(m_ToggleInformation == true)
            {
                m_InteractiveAnimator.Play("LargeInformationBoxClip");
                informationImage.enabled = true;
                informationText.enabled = true;
                m_ToggleInformation = false;
                allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
                foreach (AudioSource audioS in allAudioSources)
                {
                    audioS.Stop();
                }
                audioSrc.clip = PlaySound;
                audioSrc.Play();
            }
        }

        // OLD LOGIC FOR SCENE FADE
        //private IEnumerator ActivateButton()
        //{
        //    // If the camera is already fading, ignore.
        //    if (m_CameraFade.IsFading)
        //        yield break;

        //    // If anything is subscribed to the OnButtonSelected event, call it.
        //    if (OnButtonSelected != null)
        //        OnButtonSelected(this);

        //    // Wait for the camera to fade out.
        //    yield return StartCoroutine(m_CameraFade.BeginFadeOut(true));

        //    // Load the level.
        //    SceneManager.LoadScene(m_SceneToLoad, LoadSceneMode.Single);
        //}
    }
}