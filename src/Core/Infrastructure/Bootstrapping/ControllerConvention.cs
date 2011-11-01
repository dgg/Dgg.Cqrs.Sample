using System;
using System.Web.Mvc;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace Dgg.Cqrs.Sample.Core.Infrastructure.Bootstrapping
{
	internal class ControllerConvention : IRegistrationConvention
	{
		internal static readonly string _controllerSuffix = "Controller";

		internal static string ApplyConvention<T>() where T : class, IController
		{
			return ApplyConvention(typeof(T));
		}

		internal static string ApplyConvention(Type controller)
		{
			return controller.Name.Replace(_controllerSuffix, string.Empty);
		}

		private static readonly Predicate<Type> _isController = t => !t.IsInterface &&
			t.Namespace != null &&
			t.Namespace.Contains("Controllers") &&
			t.Name.EndsWith(_controllerSuffix);

		private static readonly Predicate<Type> _canBeResolvedByName = t => _isController(t) && typeof(IController).IsAssignableFrom(t);

		public void Process(Type type, Registry registry)
		{
			if (_isController(type))
			{
				registry.AddType(type, type);
				if (_canBeResolvedByName(type))
				{
					registry.AddType(typeof(IController), type, ApplyConvention(type));
				}
			}
		}
	}
}