namespace SmartTechTest.Game.Fight
{
    using Main.State;
    using UniRx;
    using UniRx.Triggers;
    using UnityEngine;
    using Zenject;
    
    /// <summary>
    /// Система проверки попаданий 
    /// </summary>
    public class HitDetectionSystem : IStateStage
    {
        private readonly LayerMask IngoreLayer = LayerMask.NameToLayer("Ignore Raycast");
        
        private readonly LayerMask BoundsLayer = LayerMask.NameToLayer("Bounds");
        
        [Inject]
        private IProjectileRequest _projectileRequest;

        [Inject]
        private IHitEvent _hitEvent;

        private CompositeDisposable _disposable = new CompositeDisposable();
        
        public void Init()
        {
            _projectileRequest.OnViewSpawned.Subscribe(view =>
            {
                CompositeDisposable hitDisposable = new CompositeDisposable();
                
                view.Collider.OnTriggerEnter2DAsObservable().Select(collision => collision.gameObject).Where(go => go.layer != IngoreLayer).Subscribe(hitObject =>
                {
                    if (view.Target != hitObject.layer)
                    {
                        return;
                    }
                    
                    _hitEvent.OnHitDetected.Execute(hitObject);
                    view.gameObject.SetActive(false);
                    hitDisposable.Clear();
                }).AddTo(hitDisposable);
                
                //Проверка на вылет за границы
                view.Collider.OnTriggerEnter2DAsObservable().Where(collision => collision.gameObject.layer == BoundsLayer).Subscribe(_ =>
                {
                    view.gameObject.SetActive(false);
                    hitDisposable.Clear();
                } ).AddTo(hitDisposable);

                _disposable.Add(hitDisposable);
            }).AddTo(_disposable);
        }
        
        public void Start(){}

        public void Stop(bool shouldClearResources)
        {
            _disposable.Clear();
        }

        public void Pause(bool isPaused)
        {
        }
    }
}
