using MovieManagement.Models;
using FromScratch_.net_5.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;

namespace FromScratch_.net_5
{
    public class Startup
    {
        private IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication().AddGoogle(options =>
            {
                options.ClientId = "671522115078-tudn2cop6uit38u3i3vljkkb688r6gm9.apps.googleusercontent.com";
                options.ClientSecret = "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";
            });

            services.AddLocalization(opt => { opt.ResourcesPath = "Resources"; });
            services.AddDbContextPool<AppDbContext>(
                options => options.UseSqlServer(_config.GetConnectionString("EmployeeDBConnection")));

            services.Configure<IdentityOptions>(options =>
            {
                //over-riding the password options
                options.Password.RequiredLength = 5;

            });
            services.AddMvc(options => {
                options.EnableEndpointRouting = false;
             
                //var policy = new AuthorizationPolicyBuilder()
                //.RequireAuthenticatedUser().Build();
                //options.Filters.Add(new AuthorizeFilter(policy));

            });

            //services.AddMvc().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix).AddDataAnnotationsLocalization();

          
                

            services.AddScoped<IMovieRepository, SQLMovieRepository>();
            services.AddScoped<Models.paid>();
            //to display xml format


            //Add identity service
            //Identity user has onl;y a limited prperty so we can inherit the class to add new stuffs
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();

            //Creating claims policy
            services.AddAuthorization(options =>
            {
            
                options.AddPolicy("CreateMoviePolicy",
                   policy => policy.RequireClaim("Create Movie"));
                options.AddPolicy("EditMoviePolicy",
                   policy => policy.RequireClaim("Edit Movie"));
                options.AddPolicy("DeleteMoviePolicy",
                   policy => policy.RequireClaim("Delete Movie"));
            });

           


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApplicationBuilder app1,ILogger<Startup> logger
            )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseRouting();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapGet("/", async context =>
            //    {
            //        await context.Response.WriteAsync("Hello World");
            //    });
            //});

            //app.Use(async(context,next) =>
            //{
            //    //await context.Response.WriteAsync("Hello from 1st delegate.");
            //    logger.LogInformation("MW1 : Incoming request");
            //    await next();
            //    logger.LogInformation("MW1 : Outgoing request");
            //});

            //app.Use(async (context, next) =>
            //{
            //    //await context.Response.WriteAsync("Hello from 2nd delegate.");
            //    logger.LogInformation("MW2 : Incoming request");
            //    await next();
            //    logger.LogInformation("MW2 : Outgoing request");
            //});

            
          app.UseStaticFiles();
            //  app.UseRouting();
            // app.UseMvcWithDefaultRoute(); //looks for home controller

            app.UseAuthentication();    

            app.UseMvc(routes => {
                routes.MapRoute("default", "{controller=Home}/{action=Home}/{id?}"); 
               });

            var supportedCultures = new[] { "en", "am" };
            var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapRazorPages();
            //    //endpoints.MapControllerRoute(
            //    //    name: "default",
            //    //    pattern: "{controller}/{action}/{id?}");
            //});
            app.UseRequestLocalization(localizationOptions);
            //for static files in wwwroot folder 
            //app.UseDefaultFiles();
            //app.UseStaticFiles();

            //app.Run(async (context) =>
            //{
            //    //logger.LogInformation("Request handled and response produced");
            //    await context.Response.WriteAsync("Hello world!");
            //});

           // app.Run();


        }
         
        //System.Diagnostics.Process.GetCurrentProcess().ProcessName


        //register another middleware for static files
        //all static files must be present in the wwwroot folder

    }
}





//dotnet watch run