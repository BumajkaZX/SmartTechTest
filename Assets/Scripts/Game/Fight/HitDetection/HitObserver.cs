namespace SmartTechTest.Game.Fight
{
    using UniRx;
    using UnityEngine;
    
    public class HitObserver : IHitEvent
    {
        public ReactiveCommand<GameObject> OnHitDetected { get; } = new ReactiveCommand<GameObject>();
    }
}
