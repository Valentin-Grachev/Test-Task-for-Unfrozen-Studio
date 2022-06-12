using System.Collections.Generic;
using UnityEngine;

// Система ходов по очереди. Сообщает классу ForegroundAction кто кого будет атаковать
public class MoveQueue : MonoBehaviour
{
    public static MoveQueue instance { get; private set; }

    public delegate void OnEndGame(bool playerWon);
    public event OnEndGame onEndGame;

    [SerializeField] private List<Character> _allies;
    [SerializeField] private List<Character> _enemies;
    private List<Character> _characters;

    private Queue<int> _characterIndices;
    private Character _movingCharacter;
    public Character movingCharacter { get => _movingCharacter; }


    private void Awake()
    {
        instance = this;
        _characters = new List<Character>();
        _characterIndices = new Queue<int>();

        // Объединение противников и союзников в один список персонажей
        foreach (var item in _allies) _characters.Add(item);
        foreach (var item in _enemies) _characters.Add(item);
    }



    public void NextMove()
    {
        // Проверка на окончание игры
        if (CheckEndGame(out bool playerWon)) {onEndGame?.Invoke(playerWon); return; }

        // Если очередь пуста - создаем новую, заполненную элементами в рандомном порядке
        if (_characterIndices.Count == 0) _characterIndices = MyLibrary.CreateRandomQueue(_characters.Count);

        // Извлечение из очереди номера персонажа, который будет ходить
        _movingCharacter = _characters[_characterIndices.Dequeue()];

        // Если этот персонаж мертв, то он пропускает ход
        if (_movingCharacter.isDeath) { NextMove();  return; }

        // Если персонаж является союзником - делаем подсветку и активируем кнопки действий
        if (_movingCharacter.team == Team.Player)
        {
            Control.instance.EnableButtons();
            _movingCharacter.gameObject.GetComponent<Backlight>().Light(true);
        }
        // Если персонаж является врагом - он выбирает рандомную цель из оставшихся в живых
        else if (_movingCharacter.team == Team.Enemy)
        {
            Character attacked = _allies[Random.Range(0, _allies.Count)];
            while (attacked.isDeath) attacked = _allies[Random.Range(0, _allies.Count)];
            ForegroundAction.instance.Action(_movingCharacter, attacked);
        }
    }

    private bool CheckEndGame(out bool playerWon)
    {
        bool alliesAlive = false, enemiesAlive = false;
        foreach (Character character in _characters)
        {
            if (!character.isDeath && character.team == Team.Player) alliesAlive = true;
            else if (!character.isDeath && character.team == Team.Enemy) enemiesAlive = true;
        }

        if (alliesAlive && enemiesAlive) { playerWon = false; return false; }
        else if (alliesAlive && !enemiesAlive) { playerWon = true; return true; }
        else { playerWon = false; return true; }

    }



}
