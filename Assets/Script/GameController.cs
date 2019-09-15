using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Text text;
    public int maxPlatforms = 3;
    public int Lives = 3;
    public int Points = 0;
    public int BestPoints;
    public GameObject GameOver;
    public Text text1;

    void Start()
    {
        Time.timeScale = 1;
        GameOver.SetActive(false);
        BestPoints = PlayerPrefs.GetInt("Best");
    }

    void Update()
    {
        text.text = $"Live:{Lives}\nPoints:{Points}";

        if (Lives == 0)
        {
            if (Points > BestPoints)
            {
                PlayerPrefs.SetInt("Best", Points);
            }
            Time.timeScale = 0;
            GameOver.SetActive(true);
            text1.text = $"Points: {Points} Best: {BestPoints}";
        }
    }
}
