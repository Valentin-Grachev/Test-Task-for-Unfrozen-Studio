using UnityEngine;

public class CharacterCanvas : MonoBehaviour
{
    

    void Start()
    {
        GetComponentInParent<Character>().onDeath += Character_OnDeath;
    }

    private void Character_OnDeath() => gameObject.SetActive(false);

}
