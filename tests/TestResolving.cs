using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Firefly.DependencyInjection.Tests
{
    public class TestResolving
    {
        [Fact]
        public void TestBasicSetup()
        {
            var sc = new ServiceCollection();
            sc.UseInlineDiRegistration(Assembly.Load(new AssemblyName("Firefly.DependencyInjection.Tests")));

            var provider = sc.BuildServiceProvider();

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
            
            // Unfortunately, cannot test Scoped lifetime without ASP.NET app
        }
    }

    [RegisterTransient]
    internal class MyService
    {
        public int Counter { get; set; }
    }
    
    [RegisterSingleton]
    internal class MyServiceSingleton
    {
        public int Counter { get; set; }
    }

    [RegisterTransient(Type = typeof(MyBaseService))]
    internal class MyDerivedService : MyBaseService
    {
    }

    [RegisterTransient(Type = typeof(IMyService))]
    internal class MyImplementingService : IMyService
    {
    }
    
    internal interface IMyService
    {
    }

    internal abstract class MyBaseService
    {
    }
}