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
    public float anchorWaitTime;
    public float maxAnchorWaitTime = 20f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        mouseWatcher = transform.parent.gameObject;
        mouseAnchors = mouseWatcher.GetComponent<MouseWatcher>().mouseAnchors;

        cheeseSlider = mouseWatcher.GetComponent<MouseWatcher>().cheeseSlider;

        int temp = Random.Range(1, 6);
        myRating = (temp/5f);

        anchorWaitTime = maxAnchorWaitTime / temp;

        StartCoroutine(FindNewAnchor());
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
            mouseWatcher.GetComponent<MouseWatcher>().UpdateRatings(myRating);
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
