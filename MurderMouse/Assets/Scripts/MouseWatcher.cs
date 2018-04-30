using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseWatcher : MonoBehaviour{

    public GameObject Mouse;
    public List<GameObject> mice;
    public GameObject mouseAnchorContainer;
    public List<Transform> mouseAnchors;
    public Image cheeseSlider;
    public Image hotelCheeseSlider;

    public int TotalRatingsCount = 1;
    public float TotalRatingsRaw = .5f;

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

    public void UpdateRatings(float mouseRating)
    {
        TotalRatingsCount += 1;
        TotalRatingsRaw += mouseRating;
        hotelCheeseSlider.fillAmount = TotalRatingsRaw / TotalRatingsCount;
    }

    private IEnumerator SpawnMouse()
    {
        mice.Add(Instantiate(Mouse, transform));
        yield return new WaitForSeconds(spawnCooldown);
        StartCoroutine(SpawnMouse());
    }

}
