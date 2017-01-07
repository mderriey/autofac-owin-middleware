This repository shows how to resolve OWIN middlewares from an Autofac container, so that you don't have to instantiate the middleware options when you register the middleware but the container will instantiate them once per request.

The sample takes the `OAuthAuthorizationServerMiddleware` as an example, so we can resolve some properties like `Provider` of type `IOAuthAuthorizationServerProvider` from the container.

To make sure it works, you can put a breakpoint in `Startup.cs` on line 24, debug the application, and see that it's being hit.
