using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeWall : MonoBehaviour
{
    private bool isActivated = false;
    private Transform playerTransform;
    private bool hasStartedFalling = false;
    private bool hasFinishedFirstFall = false;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (isActivated && !hasStartedFalling)
        {
            hasStartedFalling = true;
            Debug.Log("Starting fall sequence for rotating spikes");
            StartCoroutine(FallSequence());
        }
    }

    IEnumerator FallSequence()
    {
        
        yield return new WaitForSeconds(0.2f);

        Vector3 targetPos1 = new Vector3(0f, 1f, 23f);
        Quaternion targetRot1 = Quaternion.Euler(-90f, 0f, 0f);
        Quaternion targetRot2 = Quaternion.Euler(-180f, 0f, 0f);
        
        float duration = 0.4f;
        float elapsed = 0f;
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(startPos, targetPos1, t);
            transform.rotation = Quaternion.Lerp(startRot, targetRot1, t);
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);


        elapsed = 0f;
        duration = 0.3f;
        Vector3 targetPos2 = new Vector3(0f, 1f, 20f);
        startPos = transform.position;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(startPos, targetPos2, t);
            yield return null;
        }
        yield return new WaitForSeconds(0.2f);


        
        startRot = transform.rotation;
        elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            transform.rotation = Quaternion.Lerp(startRot, targetRot2, t);
            yield return null;
        }

        
        gameObject.SetActive(false);
    }

    public void Activate()
    {
        Debug.Log("Activating rotating spikes");
        isActivated = true;
        gameObject.SetActive(true); 
    }

    public void Deactivate()
    {
        Debug.Log("Deactivating rotating spikes");
        transform.position = new Vector3(0f, 0f, 23f);
        transform.rotation = Quaternion.identity;
        hasStartedFalling = false;
        isActivated = false;
        gameObject.SetActive(false);
        StopAllCoroutines();
    }
}
