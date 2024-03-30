namespace SmartTechTest.Game.Fight
{
    using UniRx;
    using UnityEngine;

    public interface IProjectileRequest
    {
        public ReactiveCommand<ProjectileView> OnViewSpawned { get; }
        
        public ReactiveCommand<ProjectileView> OnViewReleased { get; }

        /// <summary>
        /// Запрос полета проджектайла
        /// </summary>
        /// <param name="gun">Из какого оружия проджектайл</param>
        /// <param name="pos">Позиция появления</param>
        /// <param name="dir">Направление атаки</param>
        /// <param name="target">Цель проджектайла</param>
        public void RequestProjectile(BaseGun gun, Vector3 pos, Vector3 dir, LayerMask target);
    }
}
