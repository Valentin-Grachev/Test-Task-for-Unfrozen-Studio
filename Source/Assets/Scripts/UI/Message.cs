using TMPro;
using UnityEngine;

// Сообщение о победе/поражении
public class Message : MonoBehaviour
{
    [SerializeField] private string _winMessage;
    [SerializeField] private Color _colorWinMessage;
    [SerializeField] private string _defeatMessage;
    [SerializeField] private Color _colorDefeatMessage;

    private TextMeshProUGUI _text;


    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        MoveQueue.instance.onEndGame += OnEndGame;
    }

    private void OnEndGame(bool playerWon)
    {
        if (playerWon)
        {
            _text.text = _winMessage;
            _text.color = _colorWinMessage;
        }
        else
        {
            _text.text = _defeatMessage;
            _text.color = _colorDefeatMessage;
        }
    }



    
}
