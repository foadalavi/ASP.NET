using Application.Services.DataContexts;
using Application.Services.Helper;
using Application.Services.Identity;
using Application.Services.Infrastructure.Authorizaion.Requirements;
using Application.Services.Model;
using Application.Services.Model.TypeSafe;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Application.Services.Infrastructure
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, string? connectionStringConfigName)
        {
            services.AddDbContext<UserIdentityContext>(options =>
            {
                options.UseSqlServer(connectionStringConfigName);
            });
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }

        public static IdentityBuilder AddApplicationIdentity(this IServiceCollection services)
        {
            return services.AddDefaultIdentity<IdentityUser>(options =>
            {
                // configuration can be written here:
                // builder.Services.Configure<IdentityOptions>
                options.SignIn.RequireConfirmedAccount = true;

                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 3;
                options.Password.RequiredUniqueChars = 0;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(60);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<UserIdentityContext>();
        }

        public static IServiceCollection AddApplicationCookieAuth(this IServiceCollection services)
        {
            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = "My_Cookie_Name_In_Browser";
                    // Cookie settings
                    // configuration can be written here:
                    // builder.Services.ConfigureApplicationCookie

                    options.Cookie.HttpOnly = true;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);

                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/Account/AccessDenied";
                    options.SlidingExpiration = true;
                });

            return services;
        }

        public static IServiceCollection AddApplicationJwtAuth(this IServiceCollection services, JwtConfiguration configuration)
        {
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateActor = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        RequireExpirationTime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration.Issuer,
                        ValidAudience = configuration.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.Key))
                    };
                });

            return services;
        }

        public static IServiceCollection AddApplicationAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                //// Policy-based Role authorization
                //// ClassRoomController
                //options.AddPolicy(TS.Policies.FullControlPolicy, policy =>
                //{
                //    policy.RequireRole(TS.Roles.Admin);
                //});

                //options.AddPolicy(TS.Policies.ReadAndWritePolicy, policy =>
                //{
                //    policy.RequireRole(
                //        TS.Roles.Contributor,
                //        TS.Roles.Admin);
                //});

                //options.AddPolicy(TS.Policies.ReadPolicy, policy =>
                //{
                //    policy.RequireRole(
                //        TS.Roles.User,
                //        TS.Roles.Contributor,
                //        TS.Roles.Admin);
                //});


                //// Calim-based authorization
                //// StudentController
                //options.AddPolicy("CaimBasedPolicy", policy =>
                //{
                //    policy.RequireClaim("Student");
                //});


                //// Calim-based authorization using value
                //// StudentController
                //options.AddPolicy(TS.Policies.FullControlPolicy, policy =>
                //{
                //    policy.RequireClaim(TS.Contoller.Student,
                //        TS.Permissions.Delete.ToString(),
                //        TS.Permissions.Update.ToString());
                //});

                //options.AddPolicy(TS.Policies.ReadAndWritePolicy, policy =>
                //{
                //    policy.RequireClaim(TS.Contoller.Student,
                //        TS.Permissions.Write.ToString());
                //});

                //options.AddPolicy(TS.Policies.ReadPolicy, policy =>
                //{
                //    policy.RequireClaim(TS.Contoller.Student,
                //        TS.Permissions.Read.ToString());
                //});


                //Policy-based requierment authorization
                //ModuleController
                options.AddPolicy(TS.Policies.FullControlPolicy, policy =>
                {
                    policy.Requirements.Add(new AdminRequirements());
                });

                options.AddPolicy(TS.Policies.ReadAndWritePolicy, policy =>
                {
                    policy.Requirements.Add(new ContributorRequirements());
                });

                options.AddPolicy(TS.Policies.ReadPolicy, policy =>
                {
                    policy.Requirements.Add(new UserRequirements());
                });



                options.AddPolicy(TS.Policies.GenericPolicy, policy =>
                {
                    policy.Requirements.Add(new ConventionBasedRequirements());
                });
            });

            //services.AddSingleton<IAuthorizationHandler, AdminRequirementHandler>();
            //services.AddSingleton<IAuthorizationHandler, ContributorRequirementHandler>();
            //services.AddSingleton<IAuthorizationHandler, UserRequirementHandler>();

            services.AddSingleton<IAuthorizationHandler, GenericRequirmentsHandler>();

            services.AddSingleton<IAuthorizationHandler, ConventionBasedRequirementHandler>();

            return services;
        }

        public static async Task<IApplicationBuilder> SeedDataAsync(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {

                var cntx = scope.ServiceProvider.GetRequiredService<UserIdentityContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                await cntx.Database.EnsureDeletedAsync();
                if (await cntx.Database.EnsureCreatedAsync())
                {
                    // Creating Role Entities
                    var adminRole = new IdentityRole(TS.Roles.Admin);
                    var contributorRole = new IdentityRole(TS.Roles.Contributor);
                    var userRole = new IdentityRole(TS.Roles.User);

                    // Adding Roles
                    await roleManager.CreateAsync(adminRole);
                    await roleManager.CreateAsync(contributorRole);
                    await roleManager.CreateAsync(userRole);

                    // Creating User Entities
                    var adminUser = new IdentityUser() { UserName = "admin", Email = "admin@test.com" };
                    var contributorUser = new IdentityUser() { UserName = "cont", Email = "c@test.com" };
                    var user = new IdentityUser() { UserName = "user", Email = "user@test.com" };

                    // Adding Users with Password
                    await userManager.CreateAsync(adminUser, "123");
                    await userManager.CreateAsync(contributorUser, "123");
                    await userManager.CreateAsync(user, "123");

                    // Ading Claims to Users
                    await userManager.AddClaimAsync(adminUser, GetAdminClaims(TS.Contoller.Student));
                    await userManager.AddClaimAsync(contributorUser, GetcontributorClaims(TS.Contoller.Student));
                    await userManager.AddClaimAsync(user, GetUserClaims(TS.Contoller.Student));

                    // Adding Roles to Users
                    await userManager.AddToRoleAsync(adminUser, TS.Roles.Admin);
                    await userManager.AddToRoleAsync(contributorUser, TS.Roles.Contributor);
                    await userManager.AddToRoleAsync(user, TS.Roles.User);


                    //Ading Claims to Roles
                    await roleManager.AddClaimAsync(adminRole, GetAdminClaims(TS.Contoller.Module));
                    await roleManager.AddClaimAsync(contributorRole, GetcontributorClaims(TS.Contoller.Module));
                    await roleManager.AddClaimAsync(userRole, GetUserClaims(TS.Contoller.Module));


                    await roleManager.AddClaimAsync(adminRole, GetAdminClaims(TS.Contoller.Teacher));
                    await roleManager.AddClaimAsync(contributorRole, GetcontributorClaims(TS.Contoller.Teacher));
                    await roleManager.AddClaimAsync(userRole, GetUserClaims(TS.Contoller.Teacher));

                }
            }
            return app;
        }

        private static Claim GetAdminClaims(string controllerName)
        {
            return new Claim(controllerName,
                        ClaimHelper.SerializePermissions(
                            TS.Permissions.Read,
                            TS.Permissions.Write,
                            TS.Permissions.Update,
                            TS.Permissions.Delete
                        ));
        }
        private static Claim GetcontributorClaims(string controllerName)
        {
            return new Claim(controllerName,
                        ClaimHelper.SerializePermissions(
                            TS.Permissions.Read,
                            TS.Permissions.Write
                        ));
        }
        private static Claim GetUserClaims(string controllerName)
        {
            return new Claim(controllerName,
                        ClaimHelper.SerializePermissions(
                            TS.Permissions.Read
                        ));
        }
    }
}
