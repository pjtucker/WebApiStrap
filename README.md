WebApiStrap
===========

Get up and running with a new ASP.Net WebAPI project quickly with this collection commonly used libraries.

#####Configure Windsor in Global.asax.cs#####
````
private IWindsorContainer _container;

protected void Application_Start()
{
    _container = Container.Configure(GlobalConfiguration.Configuration);
}

protected void Application_End()
{
    if (_container != null)
        _container.Dispose();
}
````
