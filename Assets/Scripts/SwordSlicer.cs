using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SwordSlicer : MonoBehaviour
{
    public LayerMask sliceLayerMask;
    public float sliceForce = 5f;
    public float sliceRadius = 0.1f;

    

    

    void OnTriggerEnter(Collider other)
    {
        if (CanSliceObject(other.gameObject))
        {
            SliceObject(other.gameObject);
        }
    }

    bool CanSliceObject(GameObject objectToSlice)
    {
        // Check if the object is on the slice layer mask
        if ((sliceLayerMask.value & (1 << objectToSlice.layer)) != 0)
        {
            // Check if the object has a rigidbody
            Rigidbody objectRigidbody = objectToSlice.GetComponent<Rigidbody>();
            if (objectRigidbody != null)
            {
                return true;
            }
        }

        return false;
    }

    public void SliceObject(GameObject objectToSlice)
    {
        // Play slice sound
        ModeSelect mode = objectToSlice.GetComponent<ModeSelect>();
        mode.audioSource.PlayOneShot(mode.sliceSoundPrefab, 1f);
        mode.mode.SetActive(true);
        // Instantiate the slice effect prefab at the object's position
        if (mode.sliceEffectPrefab != null)
        {
            Instantiate(mode.sliceEffectPrefab, objectToSlice.transform.position, objectToSlice.transform.rotation);
        }

        // Get the rigidbody of the object to slice
        Rigidbody objectRigidbody = objectToSlice.GetComponent<Rigidbody>();

        if (objectRigidbody != null)
        {
            // Apply force to split the object
            Vector3 sliceDirection = transform.forward;
            objectRigidbody.AddForce(sliceDirection * sliceForce, ForceMode.Impulse);
        }

        // Disable or destroy the sliced object
        
        StartCoroutine(DisableMenu());
        objectToSlice.transform.parent.gameObject.SetActive(false);
        // Or use the line below to destroy the object:
        // Destroy(objectToSlice);
    }
    IEnumerator DisableMenu()
    {
        yield return new WaitForSeconds(1);
    }

}
