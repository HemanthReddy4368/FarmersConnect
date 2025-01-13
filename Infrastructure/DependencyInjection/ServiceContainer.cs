using Application.Contracts;
using FarmersConnect.Core.Entites;
using Infrastructure.Authorization;
using Infrastructure.Data;
using Infrastructure.Repos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DependencyInjection
{
    public static class SeviceContainer
    {
        public static IServiceCollection InfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
            b => b.MigrationsAssembly("Infrastructure")),
            ServiceLifetime.Scoped);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))

                };
            });

            // Add Authorization policies
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.RequireAdminRole, policy =>
                    policy.RequireRole(UserRole.Admin.ToString()));

                options.AddPolicy(Policies.RequireFarmerRole, policy =>
                    policy.RequireRole(UserRole.Farmer.ToString()));

                options.AddPolicy(Policies.RequireBuyerRole, policy =>
                    policy.RequireRole(UserRole.Buyer.ToString()));
            });
            services.AddScoped<IUser, UserRepo>();
            return services;
        }
    }
}
