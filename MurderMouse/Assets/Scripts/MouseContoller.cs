using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MouseContoller : MonoBehaviour {

    public GameObject mouseAnchorContainer;
    public List<Transform> mouseAnchors;
    public NavMeshAgent agent;

    public float anchorWaitTime = 1f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //agent.updateRotation = false;

        foreach (Transform tr in mouseAnchorContainer.transform)
        {
            mouseAnchors.Add(tr);
        }

        StartCoroutine(FindNewAnchor());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator FindNewAnchor()
    {
        agent.SetDestination(mouseAnchors[Random.Range(0, mouseAnchors.Count)].position);
        yield return new WaitForSeconds(anchorWaitTime);
        StartCoroutine(FindNewAnchor());
    }
}
