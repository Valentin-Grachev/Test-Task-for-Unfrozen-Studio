using TMPro;
using UnityEngine;

public class HealthText : MonoBehaviour
{
    private TextMeshProUGUI _text;

    

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        GetComponentInParent<Character>().onHealthChanged += OnHealthChanged;
    }

    private void OnHealthChanged(int health, int maxHealth)
    {
        _text.text = health.ToString();
    }

}
