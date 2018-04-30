using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class MouseContoller : MonoBehaviour {

    public GameObject mouseAnchorContainer;
    public GameObject mouseWatcher;
    public List<Transform> mouseAnchors;
    public NavMeshAgent agent;
    public Image cheeseSlider;
    public bool isWitness = false;
    public float myRating;
    public int intRating;
    public float anchorWaitTime;
    public float maxAnchorWaitTime = 20f;
    public Animator animator;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        mouseWatcher = transform.parent.gameObject;
        mouseAnchors = mouseWatcher.GetComponent<MouseWatcher>().mouseAnchors;

        cheeseSlider = mouseWatcher.GetComponent<MouseWatcher>().cheeseSlider;

        intRating = Random.Range(1, 6);
        myRating = (intRating/5f);
        
        anchorWaitTime = maxAnchorWaitTime / intRating;

        StartCoroutine(FindNewAnchor());
    }

    private void Update()
    {
        //float dist = Vector3.Distance(transform.position, agent.pathEndPosition);
        //if (dist < 1f)
        //{
        //    animator.SetBool("isWalking", false);
        //}
        //else
        //{
        //    animator.SetBool("isWalking", true);
        //}
    }

    public void HitByRay()
    {
        cheeseSlider.fillAmount = myRating;
    }

    private void OnDestroy()
    {
        mouseWatcher.GetComponent<MouseWatcher>().mice.Remove(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Exit"))
        {
            mouseWatcher.GetComponent<MouseWatcher>().UpdateRatings(intRating);
            Destroy(gameObject);
        }
    }

    private IEnumerator FindNewAnchor()
    {
        agent.SetDestination(mouseAnchors[Random.Range(0, mouseAnchors.Count)].position);
        yield return new WaitForSeconds(anchorWaitTime);
        StartCoroutine(FindNewAnchor());
    }
}
