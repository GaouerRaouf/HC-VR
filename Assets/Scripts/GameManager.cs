using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class GameManager : MonoBehaviour
{


    [Header("Spawn")]
    public List<GameObject> targets;
    private float spawnRate = 3f;
    [Header("Game & variables")]
    public bool isGameActive;
    private int lives = 3;
    private int timer = 90;
    private int score = 0;

    [Header("Texts")]
    public static GameManager Instance;
    public Text livesText;
    public Text scoreText;
    public Text timeText;
    public Text notif;


    public enum Mode
    {
        Classic, Arcade
    }
    [SerializeField] Mode mode;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        StartCoroutine(Spawntargets());
        UpdateScore(0);
        if (mode == Mode.Arcade)
        {
            StartCoroutine(Timer());
            
        }
    }

    public void AddTime(int value)
    {
        notif.text = "+" + value.ToString() + " sec";
        timer += value;
    }
    public void EndGame()
    {
        Debug.Log("ENDGAME");
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
                    isGameActive = false;
                }
                lives--;
                livesText.text = "Lives :" + lives;

                break;
            case Mode.Arcade:
                UpdateScore(-points);
                Debug.Log("Penality");
                
                break;
            default:
                break;
        }
        
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
            if (!notif.text.Equals("")) notif.text = "";
        timeText.text = "Time left: " + timer + "s";

        }

        isGameActive = false;
    }
}

