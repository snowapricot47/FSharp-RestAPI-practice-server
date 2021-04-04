namespace backend

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.HttpsPolicy
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting

type Startup(configuration: IConfiguration) =
    [<Literal>]
    let AllowAll = "AllowAll"

    member _.Configuration = configuration

    // This method gets called by the runtime. Use this method to add services to the container.
    member _.ConfigureServices(services: IServiceCollection) =
        // Add framework services.
        services.AddCors
            (fun options ->
                options.AddPolicy(
                    name = AllowAll,
                    configurePolicy =
                        (fun builder ->
                            builder
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowAnyOrigin()
                            |> ignore)
                ))
        |> ignore

        services.AddControllers() |> ignore

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    member _.Configure(app: IApplicationBuilder, env: IWebHostEnvironment) =
        if (env.IsDevelopment()) then
            app.UseDeveloperExceptionPage() |> ignore

        app
            .UseHttpsRedirection()
            .UseRouting()
            .UseCors(AllowAll)
            .UseAuthorization()
            .UseEndpoints(fun endpoints -> endpoints.MapControllers() |> ignore)
        |> ignore
