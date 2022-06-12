using UnityEngine;
using UnityEngine.UI;

public class HealthSlider : MonoBehaviour
{
    private Slider _slider;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        GetComponentInParent<Character>().onHealthChanged += OnHealthChanged;
    }

    private void OnHealthChanged(int health, int maxHealth)
    {
        _slider.value = (float)health/ (float)maxHealth;
    }

}
