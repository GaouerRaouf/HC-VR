using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{


    [Header("Spawn")]
    public List<GameObject> targets;
    [SerializeField] private float spawnRate = 3f;
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
    public GameObject endCanvas;

    [Header("Game Over")]
    [SerializeField] XRRayInteractor rightRay;
    [SerializeField] XRRayInteractor leftRay;
    private GameObject[] swords;

    public enum Mode
    {
        Classic, Arcade
    }
    [SerializeField] Mode mode;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        swords = GameObject.FindGameObjectsWithTag("Sword");
        StartLevel();
    }

    public void StartLevel()
    {
        rightRay.enabled = false;
        leftRay.enabled = false;
        foreach(GameObject sword in swords)
        {
            sword.SetActive(true);
        }
        isGameActive = true;
        StartCoroutine(Spawntargets());
        endCanvas.SetActive(false);
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
        endCanvas.SetActive(false);
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
        foreach (GameObject sword in swords)
        {
            sword.SetActive(false);
        }
        endCanvas.SetActive(true);
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
            GameObject newTarget = Instantiate(targets[index]);

            // Enable slicing by adding a collider and rigidbody to the target
            Collider targetCollider = newTarget.GetComponent<Collider>();
            if (targetCollider != null)
            {
                targetCollider.enabled = true;
            }

            Rigidbody targetRigidbody = newTarget.GetComponent<Rigidbody>();
            if (targetRigidbody != null)
            {
                targetRigidbody.isKinematic = false;
            }

            // Cast a ray from each sword to check for slicing
            foreach (GameObject sword in swords)
            {
                Ray ray = new Ray(sword.transform.position, sword.transform.forward);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject == newTarget)
                    {
                        // Get the SwordSlicer script from the sword object
                        SwordSlicer swordSlicer = hit.collider.gameObject.GetComponent<SwordSlicer>();
                        if (swordSlicer != null)
                        {
                            // Call the SliceObject method on the SwordSlicer script
                            swordSlicer.SliceObject(newTarget);
                        }
                    }
                }
            }
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
