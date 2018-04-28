using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour {

    public GameObject playerAnchorContainer;
    public List<Transform> playerAnchors;
    public NavMeshAgent agent;

    private KeyCode[] keyCodes = { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8 };

	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
        //agent.updateRotation = false;

        foreach (Transform tr in playerAnchorContainer.transform)
        {
            playerAnchors.Add(tr);
        }
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
	}

    void MovePlayer(Transform dest)
    {
        agent.SetDestination(dest.position);
    }
}
