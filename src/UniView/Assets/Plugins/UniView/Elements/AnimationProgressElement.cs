using UnityEngine;

namespace Wosylus.UniView.Elements
{
    public class AnimationProgressElement : ViewElement<float>
    {
        [SerializeField] private Animator _animator = default;
        [SerializeField] private int _layer = default;

        public override void Display(float content)
        {
            var state = _animator.GetCurrentAnimatorStateInfo(_layer);
            var stateHash = state.shortNameHash;
            _animator.speed = 0;
            _animator.Play(stateHash, _layer, content);
        }

        public override void Clear()
        {
            Display(default);
        }
    }
}