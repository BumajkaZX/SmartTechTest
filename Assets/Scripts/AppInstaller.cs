namespace SmartTechTest.Register
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Zenject;

    public class AppInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            RegisterInput();
        }

        private void RegisterInput()
        {
            PlayerControl control = new PlayerControl();
            control.Enable();

            Container.BindInstance(control).AsSingle();
        }
    }
}
