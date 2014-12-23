namespace $rootnamespace$
{
	using System.Web.Http;
	using System.Web.Http.Dispatcher;
	using Castle.MicroKernel.Resolvers.SpecializedResolvers;
	using Castle.Windsor;
	using Castle.Windsor.Installer;
	using WebApiStrap.Application.Windsor;

    public static class Container
	{
        public static IWindsorContainer Configure(HttpConfiguration configuration)
		{
			var container = new WindsorContainer();
			container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel, true));
			container.Install(FromAssembly.This());
			var dependencyResolver = new WindsorDependencyResolver(container);
			configuration.DependencyResolver = dependencyResolver;
			configuration.Services.Replace(typeof(IHttpControllerActivator), new WindsorCompositionRoot(container));
			return container;
        }
    }
}