using UnityEngine;
using UnityEngine.UI;

// Система управления кнопками
public class Control : MonoBehaviour
{
    public static Control instance { get; private set; }

    [SerializeField] private Button _skipButton;
    [SerializeField] private Button _attackButton;
    [SerializeField] private Button _startButton;

    [HideInInspector] public bool enableTargeting;


    private void Awake()
    {
        instance = this;
    }

    public void EnableButtons()
    {
        _skipButton.interactable = true;
        _attackButton.interactable = true;
    }

    public void OnStartButtonPressed()
    {
        MoveQueue.instance.NextMove();
        _startButton.gameObject.SetActive(false);
    }

    public void OnSkipButtonPressed()
    {
        _skipButton.interactable = false;
        enableTargeting = false;
        _attackButton.interactable = false;
        MoveQueue.instance.movingCharacter.GetComponent<Backlight>().Light(false);
        MoveQueue.instance.NextMove();
    }

    public void OnAttackButtonPressed()
    {
        enableTargeting = true;
        _skipButton.interactable = false;
        _attackButton.interactable = false;
    }





}
