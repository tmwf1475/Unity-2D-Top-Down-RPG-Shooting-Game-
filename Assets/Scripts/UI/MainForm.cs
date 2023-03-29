using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainForm : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private TMP_Text healthText;

    private void OnEnable() 
    {
        GameEvents.OnPlayerHealthChanged += OnPlayerHealthChanged;
    }

    private void OnPlayerHealthChanged(float currentHealth, float startHealth)
    {
        healthBar.fillAmount = currentHealth / startHealth;
        healthText.SetText($"{currentHealth}/{startHealth}");
    }

    private void OnDisable() 
    {
        GameEvents.OnPlayerHealthChanged -= OnPlayerHealthChanged;
    }
}
