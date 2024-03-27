namespace SmartTechTest.Main.Mob
{
    using Game.Mobs;
    using UnityEngine;

    /// <summary>
    /// Фабрика для мобов
    /// </summary>
    public interface IMobFactory
    {
        /// <summary>
        /// Запрос спавна моба
        /// </summary>
        /// <param name="pos">Позиция спавна</param>
        /// <param name="arg">Дополнительные параметры</param>
        public MobViewController SpawnMob(Vector3 pos, params string[] arg);
    }
}
