using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour {

    public GameObject playerAnchorContainer;
    public List<Transform> playerAnchors;
    public NavMeshAgent agent;
    public Vector3 mouseLocation;
    public Camera cam;
    public LineRenderer lineRenderer;
    public Ray cursorRay;
    public float laserFireTime = 1f;
    private float laserCurrentFireTime = 0f;
    public bool laserIsFiring = false;
    public float laserMinimumWidth = 0.01f;
    public float laserMaximumWidth = 0.75f;
    public AudioSource audioSource;
    public AudioSource laserPopAudioSource;
    public AudioClip laserChargeAudioClip;
    public AudioClip laserFireAudioClip;
    public AudioClip laserPop;
    public bool isLaserOnCooldown = false;
    public float laserCooldownTime = 1f;
    private KeyCode[] keyCodes = { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8 };
    public GameObject MainMenu; 

	// Use this for initialization
	void Start () {
        lineRenderer = GetComponent<LineRenderer>();
        cam = Camera.main;
        agent = GetComponent<NavMeshAgent>();

        foreach (Transform tr in playerAnchorContainer.transform)
        {
            playerAnchors.Add(tr);
        }

        MainMenu.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        for(int i = 0; i < keyCodes.Length; i++)
        {
            if (Input.GetKeyDown(keyCodes[i]))
            {
                try
                {
                    MovePlayer(playerAnchors[i]);
                }
                catch
                {
                    Debug.Log(keyCodes[i].ToString() + " not valid");
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (MainMenu.activeInHierarchy)
            {
                Time.timeScale = 1f;
                MainMenu.SetActive(false);
            }
            else
            {
                Time.timeScale = 0f;
                MainMenu.SetActive(true);
            }
        }

    }

    private IEnumerator LaserReload()
    {
        yield return new WaitForSeconds(laserCooldownTime);
        isLaserOnCooldown = false;
        lineRenderer.enabled = true;
    }

    private void FireLaser(RaycastHit target)
    {
        StartCoroutine(LaserReload());
        laserCurrentFireTime = 0f;
        isLaserOnCooldown = true;
        lineRenderer.enabled = false;
        audioSource.PlayOneShot(laserFireAudioClip);
        if (target.collider.tag == "Mouse")
        {
            Destroy(target.collider.gameObject);
            laserPopAudioSource.PlayOneShot(laserPop);
        }        
    }

    //player enters a player anchor
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerAnchor"))
        {
            lineRenderer.enabled = true;
        }        
    }
    
    //player leaves a player anchor
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerAnchor"))
        {
            lineRenderer.enabled = false;
        }
    }
    
    //player remains in player anchor, can aim and fire.
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PlayerAnchor"))
        {
            cursorRay = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit cursorHit;

            RaycastHit lastHit = new RaycastHit();
            if (Physics.Raycast(cursorRay, out cursorHit))
            {
                Vector3 lookAtMe = cursorHit.point;
                lookAtMe.y = playerAnchorContainer.transform.position.y;

                gameObject.transform.LookAt(lookAtMe); //make the player look at the cursor

                Ray laserRay = new Ray
                {
                    origin = other.transform.position,
                    direction = lookAtMe - other.transform.position
                };

                RaycastHit laserHit1;
                if(Physics.Raycast(laserRay, out laserHit1))
                {
                    lastHit = laserHit1;
                    lineRenderer.SetPosition(0, laserRay.origin);
                    lineRenderer.SetPosition(1, laserHit1.point);


                    //first reflection
                    if(laserHit1.collider.tag == "MirroredSurface")
                    {
                        Vector3 reflectDir = Vector3.Reflect(laserRay.direction, laserHit1.normal);
                        reflectDir.y = 0;

                        Ray laserRay2 = new Ray
                        {
                            origin = laserHit1.point,
                            direction = reflectDir
                        };

                        RaycastHit laserHit2;
                        if(Physics.Raycast(laserRay2, out laserHit2))
                        {
                            lastHit = laserHit2;
                            lineRenderer.positionCount = 3;
                            lineRenderer.SetPosition(2, laserHit2.point);

                            //second reflect
                            if (laserHit2.collider.tag == "MirroredSurface")
                            {
                                Vector3 reflectDir2 = Vector3.Reflect(laserRay2.direction, laserHit2.normal);
                                reflectDir2.y = 0;

                                Ray laserRay3 = new Ray
                                {
                                    origin = laserHit2.point,
                                    direction = reflectDir2
                                };

                                RaycastHit laserHit3;
                                if (Physics.Raycast(laserRay3, out laserHit3))
                                {
                                    lastHit = laserHit3;
                                    lineRenderer.positionCount = 4;
                                    lineRenderer.SetPosition(3, laserHit3.point);
                                }
                            }
                            else
                            {
                                lineRenderer.positionCount = 3;
                            }
                        }         
                    }
                    else
                    {
                        lineRenderer.positionCount = 2;
                    }
                }
            }

            try
            {
                lastHit.transform.SendMessage("HitByRay");
            }
            catch
            {
                //do nothing
            }
            

            #region laser lerp and sound
            if (!isLaserOnCooldown)
            {
                if (Input.GetButton("Fire1"))
                {
                    if (laserCurrentFireTime < laserFireTime)
                    {
                        laserCurrentFireTime += Time.deltaTime;
                    }
                    else if (laserCurrentFireTime > laserFireTime)
                    {
                        FireLaser(lastHit);
                    }
                }
                else
                {
                    laserCurrentFireTime = 0f;
                }

                if (Input.GetButtonDown("Fire1"))
                {
                    audioSource.PlayOneShot(laserChargeAudioClip);
                }
                if (Input.GetButtonUp("Fire1"))
                {
                    audioSource.Stop();
                }

                lineRenderer.startWidth = Mathf.Lerp(laserMinimumWidth, laserMaximumWidth, laserCurrentFireTime);
            }            
            #endregion
        }
    }
    
    void MovePlayer(Transform dest)
    {
        agent.SetDestination(dest.position);
    }
}
