using UnityEngine;


// —истема наведени€ атаки на вражеского персонажа
public class MouseTargeting : MonoBehaviour
{
    private Backlight _backlight;

    private void Start()
    {
        _backlight = GetComponent<Backlight>();
        GetComponent<Character>().onDeath += Character_OnDeath;
    }

    private void Character_OnDeath() => Destroy(this);

    private void OnMouseEnter()
    {
        if (Control.instance.enableTargeting) _backlight.Light(true);
        else _backlight.Light(false);
    }

    private void OnMouseExit()
    {
        _backlight.Light(false);
    }

    private void OnMouseDown()
    {
        if (Control.instance.enableTargeting)
        {
            _backlight.Light(false);
            Control.instance.enableTargeting = false;
            ForegroundAction.instance.Action(MoveQueue.instance.movingCharacter, GetComponent<Character>());
            MoveQueue.instance.movingCharacter.gameObject.GetComponent<Backlight>().Light(false);
        }
    }


}
