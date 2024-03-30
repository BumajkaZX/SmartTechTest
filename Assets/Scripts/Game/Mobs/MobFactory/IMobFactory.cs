namespace SmartTechTest.Main.Mob
{
    using Game.Mobs;
    using System.Collections.Generic;
    using UniRx;

    /// <summary>
    /// Фабрика для мобов
    /// </summary>
    public interface IMobFactory
    {
        public ReactiveCommand<List<MobViewController>> OnMobSpawned { get; }
        
        /// <summary>
        /// Запрос спавна моба
        /// </summary>
        /// <param name="count">количество мобов в линии</param>
        /// <param name="lines">количество линий мобов</param>
        /// <param name="arg">Дополнительные параметры</param>
        public List<MobViewController> SpawnMob(int count, int lines, params string[] arg);

        public void ReleaseMob(MobViewController mob);
        
        public void ReleaseMob(List<MobViewController> mobs);
    }
}
