namespace SmartTechTest.Game.Mobs
{
    using Main.Mob;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    
    //TODO:Пул
    public class MobFactory : IMobFactory
    {
        private const string MOBS_PATH = "Mobs/";

        private const string BASE_PREFAB_MOB = "Mobs/Base";

        private List<AbstractMob> _mobs;

        private MobViewController _mobViewController;

        public MobFactory()
        {
            _mobs = Resources.LoadAll<AbstractMob>(MOBS_PATH).ToList();
            _mobViewController = Resources.Load<MobViewController>(BASE_PREFAB_MOB);
        }

        public MobViewController SpawnMob(Vector3 pos, params string[] arg)
        {
            var currentMob = _mobs[Random.Range(0, _mobs.Count)];

            MobViewController spawnedMob = Object.Instantiate(_mobViewController, pos, Quaternion.identity);
            spawnedMob.Init(currentMob.Sprite, currentMob);
            return spawnedMob;
        }
    }
}
