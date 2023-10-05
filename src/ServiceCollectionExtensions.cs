using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Firefly.DependencyInjection
{
	public static class ServiceCollectionExtensions
	{
		public static void AddFireflyServiceRegistration(this IServiceCollection me)
			=> AddFireflyServiceRegistration(me, _ => { });

		public static void AddFireflyServiceRegistration(this IServiceCollection me,
			Action<DiRegistrationBuilder> builder)
		{
			try
			{
				var builderObject = new DiRegistrationBuilder();
				builder.Invoke(builderObject);

				if (builderObject.Assemblies.Any() == false)
				{
					builderObject.Assemblies.Add(Assembly.GetCallingAssembly());
				}

				var binder = new Binder(me);

				binder.LoadServices(builderObject);
				binder.RegisterWithServiceCollection();
			}
			catch (Exception crap)
			{
				throw new RegistrationBuilderException($"DI Registration misconfigured: {crap.Message}", crap);
			}
		}
	}
}