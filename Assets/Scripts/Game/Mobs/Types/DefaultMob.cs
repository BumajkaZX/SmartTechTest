namespace SmartTechTest.Main.Mob
{
    using Pool;
    using UniRx;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Game/Mob", fileName = "DefaultMob")]
    public class DefaultMob : AbstractMob
    {
        public override bool HitDetect(Transform currentPos, IGamePool<ParticleSystem> particlesPool)
        {
           var releaseCallback = particlesPool.RequestObject(_destroyParticles, out var pooledParticles);

           pooledParticles.transform.position = currentPos.position;
           
           pooledParticles.Play();
           
           CompositeDisposable disposable = new CompositeDisposable();
           pooledParticles.ObserveEveryValueChanged(particle => particle.isPlaying)
               .Where(isPlaying => !isPlaying)
               .Subscribe(_ =>
               {
                   releaseCallback(pooledParticles);
                   disposable.Clear();
               })
               .AddTo(disposable);

           return true;
        }
    }
}
