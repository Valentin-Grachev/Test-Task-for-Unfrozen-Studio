using UnityEngine;

public class Backlight : MonoBehaviour
{
    [SerializeField] private GameObject _backLight;
    public void Light(bool enable) => _backLight.SetActive(enable);


}
