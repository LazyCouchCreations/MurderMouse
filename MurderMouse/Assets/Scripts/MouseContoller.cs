using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MouseContoller : MonoBehaviour {

    public GameObject mouseAnchorContainer;
    public List<Transform> mouseAnchors;
    public NavMeshAgent agent;
    public bool isWitness = false;
    public AudioSource audioSource;
    public AudioClip deathAudioClip;

    public float anchorWaitTime = 1f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        mouseAnchors.Clear();
        foreach (Transform tr in mouseAnchorContainer.transform)
        {
            mouseAnchors.Add(tr);
        }

        StartCoroutine(FindNewAnchor());
    }

    private void FixedUpdate()
    {
        
    }
    
    private void OnDestroy()
    {
        audioSource.PlayOneShot(deathAudioClip);
    }

    private IEnumerator FindNewAnchor()
    {
        agent.SetDestination(mouseAnchors[Random.Range(0, mouseAnchors.Count)].position);
        yield return new WaitForSeconds(anchorWaitTime);
        StartCoroutine(FindNewAnchor());
    }
}
