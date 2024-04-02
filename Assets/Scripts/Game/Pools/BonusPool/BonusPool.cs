namespace SmartTechTest.Main.Pool
{
    using Game.Bonus;
    using System.Collections.Generic;
    using System.Linq;
    using UniRx;
    using UnityEngine;
    using UnityEngine.Pool;

    public class BonusPool : IGamePool<BonusViewController>
    {
        private const string BONUS_TYPES_PATH = "Bonus/Types";
        
        private Dictionary<BonusViewController, ObjectPool<BonusViewController>> _poolDictionary;

        private List<Bonus> _bonusList;

        private CompositeDisposable _disposable;

        public BonusPool()
        {
            _poolDictionary = new Dictionary<BonusViewController, ObjectPool<BonusViewController>>();
            _disposable = new CompositeDisposable();
            _bonusList = Resources.LoadAll<Bonus>(BONUS_TYPES_PATH).ToList();
        }

        public ReactiveCommand RequestObject(BonusViewController baseObject, out BonusViewController returnedObjectFromPool)
        {
            if (!_poolDictionary.ContainsKey(baseObject))
            {
                CreatePool(baseObject);
            }

            ReactiveCommand releaseCommand = new ReactiveCommand();

            var bonus = _poolDictionary[baseObject].Get();

            var bonusType = _bonusList[Random.Range(0, _bonusList.Count)];
            
            bonus.SetBonus(bonusType);
            
            releaseCommand.Subscribe(_ =>
            {
                Release(baseObject, bonus);
                releaseCommand.Dispose();
            }).AddTo(_disposable);

            returnedObjectFromPool = bonus;

            return releaseCommand;
        }

        private void CreatePool(BonusViewController baseObject, int estimatedCapacity = 10)
        {
            if (_poolDictionary.ContainsKey(baseObject))
            {
                return;
            }

            ObjectPool<BonusViewController> newPool =
                new ObjectPool<BonusViewController>(
                    () => Object.Instantiate(baseObject),
                    view => view.gameObject.SetActive(true),
                    view => view.gameObject.SetActive(false),
                    view => Object.Destroy(view.gameObject),
                    defaultCapacity: estimatedCapacity);

            _poolDictionary.Add(baseObject, newPool);
        }

        private void Release(BonusViewController baseBonus, BonusViewController bonus)
        {
            if (!_poolDictionary.TryGetValue(baseBonus, out var pool))
            {
                Object.Destroy(bonus.gameObject);
                return;
            }

            pool.Release(bonus);
        }


        public void Dispose()
        {
            _disposable?.Dispose();
            _bonusList?.Clear();

            foreach (var keyValue in _poolDictionary)
            {
                keyValue.Value?.Dispose();
            }
        }
    }
}
