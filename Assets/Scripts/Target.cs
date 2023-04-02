using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Target : MonoBehaviour
{
    public ParticleSystem explosionParticle;
    private Rigidbody targetRb;
    private GameManager gameManager;
     

    private float minSpeed = 8f;
    private float maxSpeed = 10f;
    private float minTorque = 10;
    private float maxTorque = 10;
    private float xRange = 1.5f;
    public int pointValue;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        targetRb = GetComponent<Rigidbody>();
        targetRb.AddForce(RandomForce(), ForceMode.Impulse);
        targetRb.AddTorque(RandomTorque(), ForceMode.Impulse);
        transform.position = RandomPosition();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Sword"))
        {
            Instantiate(explosionParticle, transform.position, transform.rotation);
            gameManager.UpdateScore(pointValue);
            Destroy(gameObject);
            if (gameObject.tag.Equals("Bomb"))          //If the target is a bomb the game is over
            {
                gameManager.EndGame();
            }
            if (gameObject.tag.Equals("Gift"))         //If the target is a gift time will be added
            {
                gameManager.AddTime(5);
                Debug.Log("Added 5 seconds");
            }
        }
        if (other.CompareTag("Dead"))
        {
            gameManager.Penality(pointValue);
            Destroy(gameObject);
        }
    }
    Vector3 RandomForce()
    {
        return Vector3.up * Random.Range(minSpeed, maxSpeed);
    }
    Vector3 RandomTorque()
    {
        return new Vector3(Random.Range(-minTorque, maxTorque), Random.Range(-minTorque, maxTorque), 0);
    }
    Vector3 RandomPosition()
    {
        return new Vector3(Random.Range(-xRange, xRange), 0, 4.5f);
    }

        
    public void DestroyTarget()
    {
        if (gameManager.isGameActive)
        {
            Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
            Destroy(gameObject);
            
        }
    }


    

}
