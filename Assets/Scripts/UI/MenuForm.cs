using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class MenuForm : MonoBehaviour
{
    [SerializeField] private ToggleGroup toggleGroup;

    private Toggle currentSelection =>toggleGroup.GetFirstActiveToggle();
    private Toggle onToggle;

    private void Start()
    {
        var toggles = toggleGroup.GetComponentsInChildren<Toggle>();
        foreach(var toggle in toggles)
        {
            toggle.onValueChanged.AddListener(_ => OnToggleValueChanged(toggle));
        }

        currentSelection.onValueChanged?.Invoke(true);
    }

    private void OnToggleValueChanged(Toggle toggle)
    {
        if (currentSelection == onToggle)
        {
            switch (toggle.name)
            {
                case"GameStart":
                    SceneManager.LoadScene("Main");
                    break;
                case"Settings":
                    Debug.LogWarning("TODO: Open settings form...");
                    break;
                case"Sponsor":
                    Debug.LogWarning("TODO: Open sponsor form...");
                    break;
                case"Quit":
                {
                    Application.Quit();
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#endif
                }
                    break;
                default:
                    throw new UnityException("Toggle name is Invalid.");
                
            }
            return;
        }
        
        if (toggle.isOn)
        {
            onToggle = toggle;
            onToggle.transform.Find("Label").GetComponent<TMP_Text>().color = Color.yellow;
        }
        else
        {
            onToggle.transform.Find("Label").GetComponent<TMP_Text>().color = Color.white;
        }
    }
}
