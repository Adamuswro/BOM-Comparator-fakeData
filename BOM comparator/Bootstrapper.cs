using BOMComparator.Core.DataAccessDB;
using BOMComparator.Core.Models;
using BOMComparator.ViewModels;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace BOMComparator
{
    public class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer _container = new SimpleContainer();

        public Bootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            base.OnStartup(sender, e);
            DisplayRootViewFor<ShellViewModel>();
        }

        protected override void Configure()
        {
            _container.Singleton<IWindowManager, WindowManager>();
            _container.PerRequest<ShellViewModel>();
            _container.PerRequest<IDataAccessDB, DataAccessByMemoryCache>();
            //_container.PerRequest<IDataAccessDB, RandomDataForTests>();
            _container.Singleton<IEventAggregator, EventAggregator>();
            _container.Singleton<IMotorService, MotorService>();

            GetType().Assembly.GetTypes()
            .Where(type => type.IsClass)
            .Where(type => type.Name.EndsWith("ViewModel"))
            .ToList()
            .ForEach(viewModelType => _container.RegisterPerRequest(
                viewModelType, viewModelType.ToString(), viewModelType));

        }

        protected override object GetInstance(Type serviceType, string key)
        {
            return _container.GetInstance(serviceType, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return _container.GetAllInstances(serviceType);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }

        
    }
}
