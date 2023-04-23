using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeSelect : MonoBehaviour
{
    [SerializeField] private GameObject mode;
    private GameManager gameManager;
    public ParticleSystem explosionParticle;
    public AudioSource audio;
    public AudioClip slash;



    private void Awake()
    {
        gameManager = GameManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Sword"))
        {
            StartCoroutine(DisableMenu());
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        
    }

    IEnumerator DisableMenu()
    {

        Instantiate(explosionParticle, transform.position, transform.rotation);
        audio.PlayOneShot(slash, 1f);
        mode.SetActive(true);
        
        yield return new WaitForSeconds(0.1f);
        gameObject.transform.parent.gameObject.SetActive(false);
    }
}
