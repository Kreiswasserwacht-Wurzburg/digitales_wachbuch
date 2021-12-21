using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Wasserwacht.DigitalGuardBook.Common.Authorization.Handlers;
using Wasserwacht.DigitalGuardBook.Common.Authorization.Requirements;
using Wasserwacht.DigitalGuardBook.Ui.Areas.Identity;

namespace Wasserwacht.DigitalGuardBook.Ui
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
#if DEBUG
            services.AddDbContext<Common.Data.CommonDataContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"))
                .EnableSensitiveDataLogging(), ServiceLifetime.Transient);
#else
            services.AddDbContext<Common.Data.CommonDataContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
#endif

            services.AddDefaultIdentity<Common.Data.Person>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
            })
                .AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<Common.Data.CommonDataContext>();

            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<Common.Data.Person>>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.Requirements.Add(new AdminRequirement()));
            });

            services.AddSingleton<IAuthorizationHandler, AdminHandler>();

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddTransient<Common.Logic.Services.StationService>();
            services.AddTransient<Common.Logic.Services.OrganisationService>();
            services.AddTransient<Common.Logic.Services.PersonService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });

            UpdateDatabase(app);
            SeedDatabase(app);
        }

        private static void UpdateDatabase(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<Common.Data.CommonDataContext>();

            context.Database.EnsureCreated();
            context.Database.Migrate();
        }

        private static void SeedDatabase(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<Common.Data.CommonDataContext>();
            using var userManager = serviceScope.ServiceProvider.GetService<UserManager<Common.Data.Person>>();
            using var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole<Guid>>>();

            if (!roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult())
            {
                var res = roleManager.CreateAsync(new IdentityRole<Guid>("Admin")).GetAwaiter().GetResult();
            }

            var admin = userManager.FindByEmailAsync("admin@example.com").GetAwaiter().GetResult();
            if (admin == null)
            {
                admin = new Common.Data.Person();
                admin.Email = "admin@example.com";
                admin.UserName = "admin@example.com";
                admin.EmailConfirmed = true;
                admin.FirstName = "Admin";
                admin.LastName = "Admin";

                var res = userManager.CreateAsync(admin).GetAwaiter().GetResult();
                res = userManager.AddToRoleAsync(admin, "Admin").GetAwaiter().GetResult();
                res = userManager.AddPasswordAsync(admin, "Start123!").GetAwaiter().GetResult();
            }
        }
    }
}
