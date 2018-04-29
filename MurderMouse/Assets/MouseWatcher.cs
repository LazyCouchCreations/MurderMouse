using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWatcher : MonoBehaviour{

    public GameObject Mouse;
    public List<GameObject> mice;
    public GameObject mouseAnchorContainer;
    public List<Transform> mouseAnchors;
    public AudioSource audioSource;

    public float spawnCooldown = 5f;

    private void Start()
    {
        //find all the mouse anchors
        mouseAnchors.Clear();
        foreach (Transform tr in mouseAnchorContainer.transform)
        {
            mouseAnchors.Add(tr);
        }

        //find all the initial mice
        foreach (Transform child in gameObject.transform)
        {
            mice.Add(child.gameObject);
        }

        StartCoroutine(SpawnMouse());
    }

    private IEnumerator SpawnMouse()
    {
        mice.Add(Instantiate(Mouse, transform));
        yield return new WaitForSeconds(spawnCooldown);
        StartCoroutine(SpawnMouse());
    }

}
