namespace SmartTechTest.Game.Fight
{
    using Main.State;
    using System;
    using System.Collections.Generic;
    using UniRx;
    using UnityEngine;
    using Zenject;

    /// <summary>
    /// Система перемещения проджектайлов
    /// </summary>
    public class ProjectileMovementSystem : IPauseStage, IDisposable
    {
        [Inject]
        private IProjectileRequest _projectileRequest;

        private List<ProjectileView> _projectileViews  = new List<ProjectileView>();

        private CompositeDisposable _disposable = new CompositeDisposable();

        private bool _isPaused;

        public void Init()
        {
            _projectileRequest.OnViewSpawned.Subscribe(view =>
                {
                    _projectileViews.Add(view);
                })
                .AddTo(_disposable);

            _projectileRequest.OnViewReleased.Subscribe(releasedView =>
            {
                if (_projectileViews.Contains(releasedView))
                {
                    _projectileViews.Remove(releasedView);
                }
            }).AddTo(_disposable);

            Observable.EveryUpdate()
                .Where(_ => !_isPaused)
                .Subscribe(_ => MoveUpdate()).AddTo(_disposable);
        }
        
        private void MoveUpdate()
        {
            foreach (var view in _projectileViews)
            {
                view.transform.SetPositionAndRotation(  
                    view.transform.position + new Vector3(view.MoveDirectory.x, view.MoveDirectory.y, 0) * Time.deltaTime,
                           Quaternion.LookRotation(view.transform.forward, view.MoveDirectory));
            }
        }

        public void Pause(bool isPaused)
        {
            _isPaused = isPaused;
        }

        public void Dispose()
        {
            _disposable?.Clear();
            _projectileViews?.Clear();
        }
    }
}
