using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ModeSelect : MonoBehaviour
{
    public GameObject mode;
    

    private GameManager gameManager;
    public AudioSource audioSource;
    public ParticleSystem sliceEffectPrefab;
    public AudioClip sliceSoundPrefab;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        gameManager = FindObjectOfType<GameManager>();
    }
    public void SetSliceEffect(ParticleSystem effectPrefab)
    {
        sliceEffectPrefab = effectPrefab;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Sword"))
        {
            SwordSlicer swordSlicer = other.gameObject.GetComponent<SwordSlicer>();
            if(swordSlicer != null)
            {
                swordSlicer.SliceObject(gameObject);
                
            }
            
        }
    }

    
}
