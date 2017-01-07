using Autofac;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Owin.AutofacMiddleware
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var builder = new ContainerBuilder();

            builder
                .RegisterInstance(app)
                .As<IAppBuilder>();

            builder
                .Register(x => new OAuthAuthorizationServerProvider
                {
                    OnMatchEndpoint = context =>
                    {
                        Debug.WriteLine("OnMatchEndpoint");
                        return Task.CompletedTask;
                    }
                })
                .As<IOAuthAuthorizationServerProvider>()
                .InstancePerDependency();

            builder
                .Register(x => new OAuthAuthorizationServerOptions
                {
                    // Resolve a dependency from the Autofac container
                    Provider = x.Resolve<IOAuthAuthorizationServerProvider>()
                })
                .AsSelf()
                .InstancePerLifetimeScope();

            // Register a middleware in the Autofac container so it'll get
            // added to the OWIN pipeline when calling `UseAutofacMiddleware`
            builder
                .RegisterType<OAuthAuthorizationServerMiddleware>()
                .AsSelf()
                .InstancePerLifetimeScope();

            var container = builder.Build();

            app.UseAutofacMiddleware(container);
        }
    }
}