using System.Collections.Generic;
using UnityEngine;

// ������� ����� �� �������. �������� ������ ForegroundAction ��� ���� ����� ���������
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

        // ����������� ����������� � ��������� � ���� ������ ����������
        foreach (var item in _allies) _characters.Add(item);
        foreach (var item in _enemies) _characters.Add(item);
    }



    public void NextMove()
    {
        // �������� �� ��������� ����
        if (CheckEndGame(out bool playerWon)) {onEndGame?.Invoke(playerWon); return; }

        // ���� ������� ����� - ������� �����, ����������� ���������� � ��������� �������
        if (_characterIndices.Count == 0) _characterIndices = MyLibrary.CreateRandomQueue(_characters.Count);

        // ���������� �� ������� ������ ���������, ������� ����� ������
        _movingCharacter = _characters[_characterIndices.Dequeue()];

        // ���� ���� �������� �����, �� �� ���������� ���
        if (_movingCharacter.isDeath) { NextMove();  return; }

        // ���� �������� �������� ��������� - ������ ��������� � ���������� ������ ��������
        if (_movingCharacter.team == Team.Player)
        {
            Control.instance.EnableButtons();
            _movingCharacter.gameObject.GetComponent<Backlight>().Light(true);
        }
        // ���� �������� �������� ������ - �� �������� ��������� ���� �� ���������� � �����
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
