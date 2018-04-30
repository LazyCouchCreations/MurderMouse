using UnityEngine;
using UnityEngine.SceneManagement;

public class Retry : MonoBehaviour {
    	
	public void ReloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
