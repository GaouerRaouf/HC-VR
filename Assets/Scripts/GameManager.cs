using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;

public class GameManager : MonoBehaviour
{


    [Header("Spawn")]
    public List<GameObject> targets;
    private float spawnRate = 3f;
    public GameObject menu;
    [Header("Game & variables")]
    public bool isGameActive;
    private int lives = 3;
    private int timer = 90;
    private int score = 0;

    [Header("Texts")]
    public static GameManager Instance;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;
    public Canvas endCanvas;

    [Header("Game Over")]
    public XRRayInteractor rightRay;
    public XRRayInteractor leftRay;

    public enum Mode
    {
        Classic, Arcade
    }
    [SerializeField] Mode mode;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        StartLevel();
    }

    public void StartLevel()
    {
        rightRay.enabled = false;
        isGameActive = true;
        StartCoroutine(Spawntargets());
        endCanvas.gameObject.SetActive(false);
        UpdateScore(-score);
        lives = 3;
        livesText.text = "Lives: " + lives;
        if (mode == Mode.Arcade)
        {
            StartCoroutine(Timer());
        }
    }

    public void Menu()
    {
        menu.SetActive(true);
        endCanvas.gameObject.SetActive(false);
        transform.parent.gameObject.SetActive(false);
    }
    
    public void UpdateScore(int points)
    {
        score += points;
        scoreText.text = "Score: " + score;
    }

    public void Penality(int points)
    {
        switch (mode)
        {
            case Mode.Classic:
                if (lives <= 1f)
                {
                    GameOver();
                }
                lives--;    
                livesText.text = "Lives: " + lives;
                
                break;
            case Mode.Arcade:
                if (score >= 0)
                {
                    UpdateScore(-points);
                    Debug.Log("Penality");
                }
                else UpdateScore(-score);
                break;
            default:
                break;
        }        
    }

    public void GameOver()
    {
        StopAllCoroutines();
        endCanvas.gameObject.SetActive(true);
        isGameActive = false;
        rightRay.enabled = true;
        leftRay.enabled = true;
    }

    IEnumerator Spawntargets()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(spawnRate);
            int index = Random.Range(0, targets.Count);
            Instantiate(targets[index]);

        }
    }


    IEnumerator Timer()
    {
        while (timer>0)
        {
        yield return new WaitForSeconds(1f);
        timer--;
        timeText.text = "Time left: " + timer + "s";

        }

        GameOver();
    }
}
