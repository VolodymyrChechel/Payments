using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ninject;
using Ninject.Web.Common;
using Ninject.Extensions.Conventions;
using Payments.BLL.Interfaces;
using Payments.BLL.Services;


namespace Payments.WEB.Util
{
    public class NinjectDependecyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependecyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            kernel.Bind<IUserService>().To<UserService>();
            kernel.Bind<IManageService>().To<ManageService>();
            kernel.Bind<IAccountsService>().To<AccountsService>();
            kernel.Bind<ICardsService>().To<CardsService>();
        }
    }
}