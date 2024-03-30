namespace SmartTechTest.Game.Fight
{
    using UniRx;
    using UnityEngine;

    /// <summary>
    /// Ивент о попадании
    /// </summary>
    public interface IHitEvent
    {
        public ReactiveCommand<GameObject> OnHitDetected { get; }
    }
}
