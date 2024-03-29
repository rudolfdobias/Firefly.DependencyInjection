using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Firefly.DependencyInjection.Tests
{
	public class Tests
	{
		[Fact]
		public void TestBasicSetup()
		{
			// :( We cannot (simply) test default loading because the CallingAssembly in unit tests
			// is not "Firefly.DependencyInjection.Tests" but some internal IDE test runner namespace.

			// var sc = new ServiceCollection();
			// sc.AddFireflyServiceRegistration();
		}

		[Fact]
		public void TestAssemblyLoad()
		{
			var sc = new ServiceCollection();
			sc.AddFireflyServiceRegistration(builder =>
			{
				builder.UseAssembly("Firefly.DependencyInjection.Tests");
				Assert.NotEmpty(builder.GetUsedAssemblies());
			});

			var provider = sc.BuildServiceProvider();
			AssertBasicRegistration(provider);

			// Test multiple instances under interface
			var impls = provider.GetServices<IMultipleServiceInterface>();
			Assert.NotNull(impls);
			var array = impls.ToArray();
			Assert.NotEmpty(array);
			Assert.True(array.Length == 3); // There are 3 of them
			Assert.Contains(array, x => x is MultipleInstanceClass1);
			Assert.Contains(array, x => x is MultipleInstanceClass2);
			Assert.Contains(array, x => x is MultipleInstanceClass3);

			// Test service lifetimes
			var scopedInstA = provider.GetRequiredService<MyScopedService>();
			var scopedInstA2 = provider.GetRequiredService<MyScopedService>();
			var singletonInstA = provider.GetRequiredService<MyServiceSingleton>();
			var transientInstA = provider.GetRequiredService<MyTransientService>();
			var transientInstA2 = provider.GetRequiredService<MyTransientService>();

			Assert.Equal(scopedInstA, scopedInstA2); // Must be same on the same scope
			Assert.NotEqual(transientInstA, transientInstA2); // Must be different every time

			using var scope = provider.CreateScope();
			var scopedInstB = scope.ServiceProvider.GetRequiredService<MyScopedService>();
			var singletonInstB = provider.GetRequiredService<MyServiceSingleton>();
			var transientInstB = provider.GetRequiredService<MyTransientService>();

			Assert.NotEqual(scopedInstA, scopedInstB); // Scoped must differ
			Assert.NotEqual(transientInstA, transientInstB); // Transient must differ
			Assert.Equal(singletonInstA, singletonInstB); // Singleton must be same

			// Test registration of concrete types.
			// Concrete should not be present if it hasn't been turned on with builder.RegisterAllImplementations(true)

			// MyImplementingService should not be there
			var shouldBeMissing = provider.GetService<MyImplementingService>();
			Assert.Null(shouldBeMissing);
		}

		[Fact]
		public void TestSingleAndMultiInstance()
		{
			var sc = new ServiceCollection();
			sc.AddFireflyServiceRegistration(builder =>
			{
				builder
					.UseAssembly("Firefly.DependencyInjection.Tests")
					.PickSingleImplementation<IMultipleServiceInterface>(typeof(MultipleInstanceClass2))
					;
			});

			var provider = sc.BuildServiceProvider();
			var impl = provider.GetService<IMultipleServiceInterface>();
			Assert.NotNull(impl);
			Assert.IsAssignableFrom<IMultipleServiceInterface>(impl);
			Assert.IsType<MultipleInstanceClass2>(impl);
		}

		[Fact]
		public void TestBadInput()
		{
			var sc = new ServiceCollection();

			// Missing implementation of PickSingleImplementation
			Assert.ThrowsAny<RegistrationBuilderException>(() =>
				sc.AddFireflyServiceRegistration(builder =>
				{
					builder
						.UseAssembly("Firefly.DependencyInjection.Tests")
						.PickSingleImplementation<IMultipleServiceInterface>(typeof(BadlyDefinedMultipleInstanceClass4))
						;
				})
			);

			// Multiple "PickSingleImplementation" for same interface
			Assert.ThrowsAny<RegistrationBuilderException>(() =>
				sc.AddFireflyServiceRegistration(builder =>
				{
					builder
						.UseAssembly("Firefly.DependencyInjection.Tests")
						.PickSingleImplementation<IMultipleServiceInterface>(typeof(MultipleInstanceClass1))
						.PickSingleImplementation<IMultipleServiceInterface>(typeof(MultipleInstanceClass2))
						;
				})
			);
		}

		[Fact]
		public void TestBuilder()
		{
			var builder = new DiRegistrationBuilder();
			builder.PickSingleImplementation<IMultipleServiceInterface>(typeof(MultipleInstanceClass2));
			var var1 = builder.GetSingleImplementations();

			builder = new DiRegistrationBuilder();
			builder.PickSingleImplementation(typeof(IMultipleServiceInterface), typeof(MultipleInstanceClass2));
			var var2 = builder.GetSingleImplementations();

			builder = new DiRegistrationBuilder();
			builder.PickSingleImplementation<IMultipleServiceInterface, MultipleInstanceClass2>();
			var var3 = builder.GetSingleImplementations();

			Assert.Equal(var1, var2);
			Assert.Equal(var2, var3);
		}

		[Fact]
		public void TestConcreteRegistration()
		{
			var sc = new ServiceCollection();
			sc.AddFireflyServiceRegistration(builder =>
			{
				builder
					.UseAssembly("Firefly.DependencyInjection.Tests")
					.RegisterAllImplementations();
			});
			var provider = sc.BuildServiceProvider();
			// Both interfaces and concrete types should be available now

			var ifaceImpl = provider.GetService<IMyService>();
			var concrete = provider.GetService<MyImplementingService>();

			Assert.NotNull(ifaceImpl);
			Assert.NotNull(concrete);
			Assert.IsType<MyImplementingService>(ifaceImpl);
			Assert.IsType<MyImplementingService>(concrete);
			
			// Let's try multiple concretes
			var all = provider.GetServices<IMultipleServiceInterface>();
			var one = provider.GetService<MultipleInstanceClass1>();
			var two = provider.GetService<MultipleInstanceClass2>();
			var three = provider.GetService<MultipleInstanceClass3>();
			Assert.NotNull(all);
			Assert.NotNull(two);
			Assert.NotNull(one);
			Assert.NotNull(three);
			
			Assert.Equal(3, all.Count());
			Assert.IsType<MultipleInstanceClass1>(one);
			Assert.IsType<MultipleInstanceClass2>(two);
			Assert.IsType<MultipleInstanceClass3>(three);
		}

		private void AssertBasicRegistration(ServiceProvider provider)
		{
			var s1 = provider.GetService<MyService>();
			var s1Singleton = provider.GetService<MyServiceSingleton>();
			var s2 = provider.GetService<IMyService>();
			var s3 = provider.GetService<MyBaseService>();

			Assert.NotNull(s1);
			Assert.NotNull(s1Singleton);
			Assert.NotNull(s2);
			Assert.NotNull(s3);
			Assert.IsType<MyService>(s1);
			Assert.IsType<MyServiceSingleton>(s1Singleton);
			Assert.IsType<MyImplementingService>(s2);
			Assert.IsType<MyDerivedService>(s3);

			Assert.Equal(0, s1.Counter);
			Assert.Equal(0, s1Singleton.Counter);
			s1.Counter++;
			s1Singleton.Counter++;
			var s1Again = provider.GetService<MyService>();
			var s1SingletonAgain = provider.GetService<MyServiceSingleton>();
			Assert.Equal(0, s1Again.Counter);
			Assert.Equal(1, s1SingletonAgain.Counter);
			Assert.NotNull(provider.GetService<MyScopedService>());
		}
	}
}