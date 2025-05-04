using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [Header("Button Text Colors")]
    [SerializeField] private Color normalTextColor = Color.white;
    [SerializeField] private Color hoverTextColor = new Color(0.9f, 0.8f, 0.1f); // Gold hover

    private void Start()
    {
        SetupButtonWithHoverText(GameObject.Find("Play").GetComponent<EventTrigger>());
        SetupButtonWithHoverText(GameObject.Find("Surrender").GetComponent<EventTrigger>());
    }

    public void SetupButtonWithHoverText(EventTrigger buttonTrigger)
    {
        TMP_Text[] allTexts = buttonTrigger.GetComponentsInChildren<TMP_Text>(true);

        foreach (TMP_Text text in allTexts)
        {
            text.color = normalTextColor;
        }

        EventTrigger.Entry pointerEnter = new EventTrigger.Entry();
        pointerEnter.eventID = EventTriggerType.PointerEnter;
        pointerEnter.callback.AddListener((data) => {
            foreach (TMP_Text text in allTexts)
            {
                text.color = hoverTextColor;
            }
        });

        EventTrigger.Entry pointerExit = new EventTrigger.Entry();
        pointerExit.eventID = EventTriggerType.PointerExit;
        pointerExit.callback.AddListener((data) => {
            foreach (TMP_Text text in allTexts)
            {
                text.color = normalTextColor;
            }
        });

        buttonTrigger.triggers.Add(pointerEnter);
        buttonTrigger.triggers.Add(pointerExit);
    }

    public void PlayButton()
    {
        SceneManager.LoadScene("Level1");
    }

    public void ExitButton()
    {
        Application.Quit();
        print("Game was closed");
    }
}