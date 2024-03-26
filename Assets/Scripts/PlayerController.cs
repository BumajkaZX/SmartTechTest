namespace SmartTechTest.Main.Player
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UniRx;
    using UnityEngine;
    using Zenject;

    public class PlayerController : MonoBehaviour
    {
        [Inject]
        private PlayerControl _playerControl;

        [SerializeField]
        private PlayerView _playerView;

        [Header("Player settings")]

        [SerializeField, Min(0.01f)]
        private float _moveSpeed;

        [SerializeField]
        private float _minXPosition;

        [SerializeField]
        private float _maxXPosition;

        private CompositeDisposable _disposable = new CompositeDisposable();

        private void Awake()
        {
            Observable.EveryUpdate()
                .Select(_ => _playerControl.Player.Move.ReadValue<Vector2>())
                .Subscribe(Move)
                .AddTo(_disposable);

            Observable.EveryUpdate()
                .Where(_ => _playerControl.Player.Fire.WasPressedThisFrame())
                .Subscribe(_ => Fire())
                .AddTo(_disposable);
        }

        private void Move(Vector2 dir)
        {
            _playerView.UpdateView(dir);

            if (dir == Vector2.zero)
            {
                return;
            }

            Vector3 newPos =  transform.position + new Vector3(dir.x, 0, 0) * _moveSpeed * Time.deltaTime;

            newPos = new Vector3(ClampPosition(newPos.x), newPos.y, newPos.z);

            transform.position = newPos;
        }
        
        private void Fire()
        {
            
        }

        private float ClampPosition(float xPos)
        {
            if (xPos > _maxXPosition)
            {
                return _maxXPosition;
            }

            if (xPos < _minXPosition)
            {
                return _minXPosition;
            }

            return xPos;
        }
        
        private void OnDestroy()
        {
            _disposable.Clear();
        }
    }
}
