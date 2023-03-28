using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeSelect : MonoBehaviour
{
    [SerializeField] private GameObject mode;
    private GameManager gameManager;
    public ParticleSystem explosionParticle;
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

        explosionParticle.Play();
        mode.SetActive(true);
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        yield return new WaitForSeconds(0.1f);
        gameObject.transform.parent.gameObject.SetActive(false);
    }
}
