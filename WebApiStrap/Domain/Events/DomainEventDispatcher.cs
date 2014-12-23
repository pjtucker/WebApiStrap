namespace WebApiStrap.Domain.Events
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Castle.Windsor;

    public static class DomainEventDispatcher
    {
        [ThreadStatic]
        private static List<Delegate> _actions;
        
        private static IWindsorContainer Container { get; set; }

        public static void SetContainer(IWindsorContainer container)
        {
            Container = container;
        }

        public static void Register<T>(Action<T> callback) where T : IDomainEvent
        {
            if (_actions == null)
                _actions = new List<Delegate>();
            _actions.Add(callback);
        }

        public static void ClearCallbacks()
        {
            _actions = null;
        }

        public static void Raise<T>(T domainEvent) where T: IDomainEvent
        {
            if (Container != null)
            {
                foreach (var handler in Container.ResolveAll<IHandler<T>>())
                {
                    handler.Handle(domainEvent);
                }
            }

            if (_actions == null)
                return;
            var callbacks = _actions.OfType<Action<T>>().ToList();
            callbacks.ForEach(callback => callback(domainEvent));
        }
    }
}