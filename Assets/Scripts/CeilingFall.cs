using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CeilingFallController : MonoBehaviour
{
    private GameObject ceiling;
    private GameObject movingFloor;

    private Vector3 initialCeilingPosition;
    private Vector3 initialMovingFloorPosition;

    private bool playerMoved = false;
    private bool ceilingShouldFall = false;
    private bool floorHasDropped = false;
    private bool ceilingDisappeared = false;

    public float fallSpeed = 1f;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Level4")
        {
            ceiling = GameObject.FindGameObjectWithTag("FallingCeiling");
            movingFloor = GameObject.FindGameObjectWithTag("MovingFloor");

            if (ceiling != null)
                initialCeilingPosition = ceiling.transform.position;

            if (movingFloor != null)
                initialMovingFloorPosition = movingFloor.transform.position;

            FirstPersonController.OnPlayerDeath += ResetLevelState;
        }
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name != "Level4") return;

        if (!playerMoved && (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0))
        {
            playerMoved = true;
            ceilingShouldFall = true;
        }

        if (ceilingShouldFall && ceiling != null && !ceilingDisappeared)
        {
            Vector3 pos = ceiling.transform.position;
            pos.y -= fallSpeed * Time.deltaTime;
            ceiling.transform.position = pos;

            if (pos.y <= 3f)
            {
                ceilingShouldFall = false;
                StartCoroutine(DisappearAfterDelay());
            }
        }

        if (!floorHasDropped && playerMoved)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null && player.transform.position.z > 27f && movingFloor != null)
            {
                Vector3 pos = movingFloor.transform.position;
                pos.y -= 1f;
                movingFloor.transform.position = pos;
                floorHasDropped = true;
            }
        }
    }

    private IEnumerator DisappearAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        if (ceiling != null && ceiling.transform.position.y <= 3f && floorHasDropped)
        {
            ceiling.SetActive(false);
            ceilingDisappeared = true;
        }
    }

    private void ResetLevelState()
    {
        if (ceiling != null)
        {
            ceiling.transform.position = initialCeilingPosition;
            ceiling.SetActive(true);
        }

        if (movingFloor != null)
        {
            movingFloor.transform.position = initialMovingFloorPosition;
        }

        playerMoved = false;
        ceilingShouldFall = false;
        floorHasDropped = false;
        ceilingDisappeared = false;
    }

    void OnDestroy()
    {
        FirstPersonController.OnPlayerDeath -= ResetLevelState;
    }
}
