using UnityEngine;

public enum Team { Player, Enemy}


// Класс с характеристиками персонажа
public class Character : MonoBehaviour
{
    public delegate void OnHealthChanged(int health, int maxHealth);
    public delegate void OnDeath();
    public event OnHealthChanged onHealthChanged;
    public event OnDeath onDeath;

    [HideInInspector] public Character attackedCharacter;


    [SerializeField] private Team _team;
    public Team team { get => _team; }
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _health;
    public int health
    {
        get => _health;
        set
        {
            if (value <= 0) { _health = 0; _isDeath = true; onDeath?.Invoke(); }
            else if (value > _maxHealth) _health = _maxHealth;
            else _health = value;
            onHealthChanged?.Invoke(_health, _maxHealth);
        }
    }

    [SerializeField] private int _damage;
    public int damage { get => _damage; }

    private CharacterAnimation _animation;
    public CharacterAnimation anim { get => _animation; }

    private bool _isDeath = false;
    public bool isDeath { get => _isDeath; }




    private void Start()
    {
        if (TryGetComponent(out OverlaySkin skin)) skin.Initialize();
        _animation = GetComponent<CharacterAnimation>();
        onHealthChanged?.Invoke(_health, _maxHealth);
    }


    



}
