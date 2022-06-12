using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public enum TransitionMode { Closer, Farther, Idle}

// �������, ���������� �� ���������� �������� ����������
// �� �������� ����, �� �������� �����, ����������� �� �������� �������
public class ForegroundAction : MonoBehaviour
{
    public static ForegroundAction instance { get; private set; }

    [SerializeField] private Transform _allyActionPosition;
    [SerializeField] private Transform _enemyActionPosition;
    [SerializeField] private SpriteRenderer _fadeRenderer;

    private Character _ally;
    private Character _enemy;
    private Vector3 _allyIdlePosition;
    private Vector3 _enemyIdlePosition;
    private TransitionMode _transitionMode = TransitionMode.Idle;

    private float _transition;
    private float transition
    {
        get => _transition; 
        set 
        { 
            if (value < 0f) _transition = 0f;
            else if (value > 1f) _transition = 1f;
            else _transition = value;
        }
    }
    private const float transitionSpeed = 2f;
    private const float maxFadeIntensity = 0.7f;
    private const float delayUntilNextMove = 1f;

    private void Awake()
    {
        instance = this;
    }



    private void Update()
    {
        if (_transitionMode == TransitionMode.Idle) return;
        if (_transitionMode == TransitionMode.Closer) transition += Time.deltaTime * transitionSpeed;
        if (_transitionMode == TransitionMode.Farther) transition -= Time.deltaTime * transitionSpeed;
        if (transition == 1f || transition == 0f) _transitionMode = TransitionMode.Idle;

        // � ����������� �� �������� transition ��� ��������� ������ �������� ����� ��� ����� ��� ���� �������� �������
        Vector3 newAllyPosition = Vector3.Lerp(_allyIdlePosition, _allyActionPosition.position, transition);
        newAllyPosition.z = _ally.transform.position.z;
        _ally.transform.position = newAllyPosition;

        Vector3 newEnemyPosition = Vector3.Lerp(_enemyIdlePosition, _enemyActionPosition.position, transition);
        newEnemyPosition.z = _enemy.transform.position.z;
        _enemy.transform.position = newEnemyPosition;

        // ����������
        Color newColor = _fadeRenderer.color;
        newColor.a = transition * maxFadeIntensity;
        _fadeRenderer.color = newColor;


    }


    // ���������� �������� �����
    public void Action(Character attacking, Character attacked)
    {
        // ����������� ��� �� ��������� �������� ���������
        if (attacking.team == Team.Player)
        {
            _ally = attacking;
            _enemy = attacked;
            _allyIdlePosition = attacking.transform.position;
            _enemyIdlePosition = attacked.transform.position;

        }
        else 
        { 
            _ally = attacked;
            _enemy = attacking;
            _allyIdlePosition = attacked.transform.position;
            _enemyIdlePosition = attacking.transform.position;
        }

        // ����� ����� ���������� � ���������� �� �������� ����
        SortingGroup attackingLayer = attacking.gameObject.GetComponent<SortingGroup>();
        attackingLayer.sortingLayerName = "Foreground";
        attackingLayer.sortingOrder = 1;
        attacking.gameObject.GetComponentInChildren<Canvas>().sortingLayerName = "Foreground";

        SortingGroup attackedLayer = attacked.gameObject.GetComponent<SortingGroup>();
        attackedLayer.sortingLayerName = "Foreground";
        attackedLayer.sortingOrder = 0;
        attacked.gameObject.GetComponentInChildren<Canvas>().sortingLayerName = "Foreground";

        // ������ ������ �������� ���������� ����������
        _transitionMode = TransitionMode.Closer;

        // �������� ���������� ���������� ���� �� ������� � ������ �������� �����
        attacking.attackedCharacter = attacked;
        attacking.anim.SetAnimation(attacking.anim.attack);

    }

    // ����������� ���������� � ������������ �� �������� �������
    public void BackToPosition()
    {
        _transitionMode = TransitionMode.Farther;
        _ally.gameObject.GetComponent<SortingGroup>().sortingLayerName = "Middle";

        Canvas allyCanvas = _ally.gameObject.GetComponentInChildren<Canvas>();
        if (allyCanvas != null) allyCanvas.sortingLayerName = "Middle";

        _enemy.gameObject.GetComponent<SortingGroup>().sortingLayerName = "Middle";

        Canvas enemyCanvas = _enemy.gameObject.GetComponentInChildren<Canvas>();
        if (enemyCanvas != null) enemyCanvas.sortingLayerName = "Middle";

        // ���������� ��������� �������� ����� �����
        StartCoroutine(NextMoveWithDelay());
    }

    private IEnumerator NextMoveWithDelay()
    {
        yield return new WaitForSeconds(delayUntilNextMove);
        MoveQueue.instance.NextMove();
    }


}
