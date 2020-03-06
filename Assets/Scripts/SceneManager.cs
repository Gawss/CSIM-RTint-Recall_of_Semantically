using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Experiment{
    public class SceneManager : MonoBehaviour
    {
        public static SceneManager Instance { get; private set; }
        
        [SerializeField]
        private float m_globalSpeed;
        public float globalSpeed {get{return m_globalSpeed;} set{m_globalSpeed=value;}}

        private GameObject pilot;
        private GameObject sign;

        [SerializeField]
        private float sensitivity = 0;

        private RaycastController m_RaycastController;
        public RaycastController RaycastController {get{return m_RaycastController;} private set{m_RaycastController=value;}}

        private GameObject LoadingBar;
        private bool isPlayingTrial = false;

        private bool canAccelerate = true;

        private int velocity = 0;

        private void Awake(){
            if(Instance == null){
                Instance = this;
                DontDestroyOnLoad(gameObject);

                pilot = GameObject.Find("Pilot");
                sign = GameObject.Find("Sign");
                RaycastController = gameObject.GetComponent<RaycastController>();
                LoadingBar = GameObject.Find("LoadingBar");

            }else{
                Destroy(gameObject);
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            
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
            isPlayingTrial = true;
            StartCoroutine(WaitForStimulus());
        }

        private IEnumerator WaitForStimulus(){
            yield return new WaitForSeconds(5);
            Debug.Log("Time is done");                
            velocity = 1;
            canAccelerate = true;
            isPlayingTrial = false;
        }
    }
}
