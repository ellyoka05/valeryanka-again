using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingSpikes : MonoBehaviour
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
        if (isActivated && playerTransform.position.z < 12f && !hasStartedFalling)
        {
            hasStartedFalling = true;
            Debug.Log("Starting fall sequence for rotating spikes");
            StartCoroutine(FallSequence());
        }
    }

    IEnumerator FallSequence()
    {
        
        Vector3 targetPos1 = new Vector3(0f, 5f, 11.5f);
        Quaternion targetRot1 = Quaternion.Euler(90f, 0f, 0f);
        
        float duration = 1f;
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
        transform.position += Vector3.up * 0.5f;
        yield return new WaitForSeconds(0.2f);


        Vector3 targetPos2 = new Vector3(0f, 3f, 8.5f);
        startPos = transform.position;
        elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(startPos, targetPos2, t);
            yield return null;
        }


        gameObject.SetActive(false);
    }

    public void Activate()
    {
        Debug.Log("Activating rotating spikes");
        isActivated = true;
    }

    public void Deactivate()
    {
        Debug.Log("Deactivating rotating spikes");
        transform.position = new Vector3(0f, 6f, 14.5f);
        transform.rotation = Quaternion.identity;
        hasStartedFalling = false;
        isActivated = false;
        gameObject.SetActive(true);
        StopAllCoroutines();
    }
}
