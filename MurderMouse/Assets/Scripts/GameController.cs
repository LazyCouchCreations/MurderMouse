using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public Image hotelCheeseSlider;
    public GameObject gameOverPanel;
    public GameObject mainMenu;
    public AudioSource music;
    public AudioClip gameOverMusic;
    public Text scoreText;
    public int score = 0;

	// Use this for initialization
	void Start () {
        Time.timeScale = 0f;
        mainMenu.SetActive(true);
        UpdateScore(0);
    }
	
	// Update is called once per frame
	void Update () {
        if (hotelCheeseSlider.fillAmount <= 0)
        {
            if (!gameOverPanel.activeInHierarchy)
            {
                music.clip = gameOverMusic;
                music.Play();
            }
            Time.timeScale = 0f;
            gameOverPanel.SetActive(true);                       
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (mainMenu.activeInHierarchy)
            {
                Time.timeScale = 1f;
                mainMenu.SetActive(false);
            }
            else
            {
                Time.timeScale = 0f;
                mainMenu.SetActive(true);
            }
        }
    }

    public void UpdateHotelRating(int mouseRating)
    {
        switch (mouseRating)
        {
            case 1:
                hotelCheeseSlider.fillAmount -= .6f;
                break;
            case 2:
                hotelCheeseSlider.fillAmount -= .2f;
                break;
            case 3:
                hotelCheeseSlider.fillAmount -= .2f;
                break;
            case 4:
                hotelCheeseSlider.fillAmount += .2f;
                UpdateScore(40);
                break;
            case 5:
                hotelCheeseSlider.fillAmount += .4f;
                UpdateScore(50);
                break;
            default:
                break;
        }
    }

    public void UpdateScore(int scoreIncrease)
    {
        score += scoreIncrease;
        scoreText.text = score.ToString();
    }
}
