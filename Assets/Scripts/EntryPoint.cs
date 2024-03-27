namespace SmartTechTest.Entry
{
    using Game.Mobs;
    using Main.State;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using Zenject;

    public class EntryPoint : MonoBehaviour
    {
        private AppStateSystem _appStateSystem;

        private DiContainer _container;
        
    #region Game classes

        private MobsController _mobsController;
    
    #endregion

        [Inject]
        public void Construct(DiContainer container, AppStateSystem appStateSystem)
        {
            _container = container;
            _appStateSystem = appStateSystem;
        }
        
        private void Awake()
        {
            _mobsController = new MobsController();
            _container.Inject(_mobsController);
            
        }

        private void Start()
        {
            _appStateSystem.ChangeState(new MenuState());
        }

        private void OnDestroy()
        {
            
        }
    }
}
