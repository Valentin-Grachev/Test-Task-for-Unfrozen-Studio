using Spine.Unity;
using UnityEngine;


// Класс управления анимацией персонажа
public class CharacterAnimation : MonoBehaviour
{
    [SerializeField] private AnimationReferenceAsset _idle;
    [SerializeField] private AnimationReferenceAsset _attack;
    public AnimationReferenceAsset attack { get => _attack; }

    [SerializeField] private AnimationReferenceAsset _damage;
    public AnimationReferenceAsset damage { get => _damage;}

    private SkeletonAnimation _animation;
    private Character _character;

    private void Start()
    {
        _animation = GetComponent<SkeletonAnimation>();
        _character = GetComponent<Character>();
        _animation.state.Event += State_Event;
        _animation.state.Complete += State_Complete;
    }


    private void State_Event(Spine.TrackEntry trackEntry, Spine.Event e)
    {
        if (e.Data.name == "Hit")
        {
            _character.attackedCharacter.health -= _character.damage;
            CharacterAnimation attackedAnimation = _character.attackedCharacter.anim;
            attackedAnimation.SetAnimation(attackedAnimation.damage);
        }
            
    }
    private void State_Complete(Spine.TrackEntry trackEntry)
    {
        if (trackEntry.animation == _attack.Animation) ForegroundAction.instance.BackToPosition();
        if (trackEntry.animation != _idle.Animation) SetAnimation(_idle, true);
    }

    public void SetAnimation(AnimationReferenceAsset anim, bool loop = false, float timeScale = 1f)
    {
        _animation.state.SetAnimation(0, anim, loop).TimeScale = timeScale;
    }



}
