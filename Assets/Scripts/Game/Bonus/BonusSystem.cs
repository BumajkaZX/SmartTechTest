namespace SmartTechTest.Game.Bonus
{
    using Fight;
    using Main.Player;
    using Main.Pool;
    using Main.State;
    using Mobs;
    using System.Collections.Generic;
    using UniRx;
    using UniRx.Triggers;
    using UnityEngine;
    using Zenject;

    /// <summary>
    /// Система выпадения\перемещения\получения бонусов
    /// </summary>
    public class BonusSystem : IStateStage
    {
        //0-100
        private const int BONUS_CHANCE = 15;

        private const string BONUS_PATH = "Bonus/BonusBase";

        private readonly LayerMask PlayerLayer = LayerMask.NameToLayer("Player");

        private readonly LayerMask BoundsLayer = LayerMask.NameToLayer("Bounds");
        
        [Inject]
        private IGamePool<BonusViewController> _bonusPool;

        [Inject]
        private IHitEvent _hitEvent;

        private bool _isPaused;

        private List<BonusViewController> _viewControllers;

        private BonusViewController _baseBonus;
        
        private CompositeDisposable _disposable;
        
        public void Init()
        {
            _baseBonus = Resources.Load<BonusViewController>(BONUS_PATH);
            
            _viewControllers = new List<BonusViewController>();
            _disposable = new CompositeDisposable();

            _hitEvent.OnHitDetected
                .Select(go => go.GetComponent<MobViewController>())
                .Where(view => view != null).Subscribe(BonusDrop)
                .AddTo(_disposable);

            Observable
                .EveryUpdate()
                .Subscribe(_ => BonusMove())
                .AddTo(_disposable);
        }

        public void Pause(bool isPaused)
        {
            _isPaused = isPaused;
        }

        public void Start(){}

        private void BonusMove()
        {
            if (_isPaused)
            {
                return;
            }
            
            foreach (var bonusView in _viewControllers)
            {
                bonusView.transform.position += Vector3.down * bonusView.DropSpeed * Time.deltaTime;
            }
        }

        private void BonusDrop(MobViewController viewController)
        {
            var random = Random.Range(0, 100);
            if (random > BONUS_CHANCE)
            {
                return;
            }

            var releaseAction = _bonusPool.RequestObject(_baseBonus, out var bonus);
            
            _viewControllers.Add(bonus);

            CompositeDisposable disposable = new CompositeDisposable();
            
            _disposable.Add(disposable);

            bonus.transform.position = viewController.transform.position;
            
            bonus.Collider2D.OnTriggerEnter2DAsObservable()
                .Where(collision => collision.gameObject.layer == PlayerLayer || collision.gameObject.layer == BoundsLayer)
                .Select(collision => collision.GetComponent<PlayerViewController>()).DoOnCancel(() =>
                {
                    _viewControllers.Remove(bonus);
                    releaseAction.Execute();
                })
                .Subscribe(
                playerView =>
                {
                    if (playerView != null)
                    {
                        playerView.SetGun(bonus.GunBonus, bonus.BonusTime);
                    }
                 
                    disposable.Clear();
                    _disposable.Remove(disposable);
                }).AddTo(disposable);
        }

        public void Stop(bool shouldClearResources)
        {
            _disposable.Clear();
            _bonusPool.Dispose();
        }
    }
}
