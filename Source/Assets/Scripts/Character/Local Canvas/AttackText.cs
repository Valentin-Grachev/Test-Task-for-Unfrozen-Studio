using TMPro;
using UnityEngine;

public class AttackText : MonoBehaviour
{

    private void Awake()
    {
        GetComponent<TextMeshProUGUI>().text = GetComponentInParent<Character>().damage.ToString();
    }
}
