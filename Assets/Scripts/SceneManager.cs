using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Experiment{
    public class SceneManager : MonoBehaviour
    {
        public static SceneManager Instance { get; private set; }
        
        [SerializeField]
        private float m_globalSpeed;        

        private GameObject pilot;
        private GameObject sign;

        [SerializeField]
        private float sensitivity = 0;

        private RaycastController m_RaycastController;

        private GameObject LoadingBar;
        private bool isPlayingTrial = false;

        private bool canAccelerate = true;

        private int velocity = 0;

        private int m_index;        
        
        private Sprite m_sprite;
        private AudioClip m_audioClip;

        public GameObject plane;

        [SerializeField]
        private int num_pairs;

        [SerializeField]
        private GameObject fade;

        #region UNITY FUNCTIONS

        private void Awake(){
            if(Instance == null){
                Instance = this;
                DontDestroyOnLoad(gameObject);

                pilot = GameObject.Find("Pilot");
                sign = GameObject.Find("Sign");
                RaycastController = gameObject.GetComponent<RaycastController>();
                LoadingBar = GameObject.Find("LoadingBar");
                m_index = 0;

            }else{
                Destroy(gameObject);
            }
        }

        void Update(){
            float rotateHorizontal = Input.GetAxis ("Mouse X");
            float rotateVertical = Input.GetAxis ("Mouse Y");
            Camera.main.transform.Rotate(transform.up * rotateHorizontal * sensitivity);
            Camera.main.transform.Rotate(-transform.right * rotateVertical * sensitivity);
        }

        private void FixedUpdate(){
            if(canAccelerate)
            {
                CalculateDistance();
            }else{
                m_globalSpeed = 0;
                if(!isPlayingTrial) WaitForInteraction();
            }
            
        }
        #endregion

        private void CalculateDistance(){
            m_globalSpeed = Vector3.Distance(sign.transform.position, pilot.transform.position);
            m_globalSpeed = (m_globalSpeed/Vector3.Distance(pilot.transform.position, sign.GetComponent<Move_loop>().initial_pos))*2;
            if(m_globalSpeed+velocity < 0.1f){
                canAccelerate = false;
            }
            if(m_globalSpeed >= 0.9f){
                velocity = 0;
            }
        }

        private void WaitForInteraction(){
            if(RaycastController.GetRaycastCollision() == null) return;
            if(RaycastController.GetRaycastCollision().name == "Sign"){
                //Activate Loading Bar
                LoadingBar.GetComponent<Animator>().SetBool("isLoading", true);
            }else{
                //Deactivate Loading Bar
                LoadingBar.GetComponent<Animator>().SetBool("isLoading", false);
            }
        }

        public void PlayTrial(){
            //Play Sound + Image
            Debug.Log("Playing Trial...");
            m_audioClip = Resources.Load<AudioClip>("Sounds/pair" + index + "_audio");
            Debug.Log(m_audioClip);
            Camera.main.GetComponent<AudioSource>().PlayOneShot(m_audioClip);
            isPlayingTrial = true;
            StartCoroutine(WaitForStimulus());
        }

        private IEnumerator WaitForStimulus(){
            yield return new WaitForSeconds(3);
            Debug.Log("Time is done");                
            velocity = 1;
            canAccelerate = true;
            isPlayingTrial = false;
            plane.SetActive(false);

            index++;
            if(index+1 > num_pairs){
                fade.SetActive(true);
            }
        }

        #region GET & SET
        public int index {get{return m_index;} private set{m_index=value;}}
        public Sprite sprite {get{return m_sprite;} set{m_sprite=value;}}
        public AudioClip audioClip {get{return m_audioClip;} set{m_audioClip=value;}}
        public RaycastController RaycastController {get{return m_RaycastController;} private set{m_RaycastController=value;}}
        public float globalSpeed {get{return m_globalSpeed;} set{m_globalSpeed=value;}}

        #endregion
    }
}
