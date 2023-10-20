using System.Text;
using Wallet.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Wallet.Settinngs;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Wallet.Application.Profiles;
using Wallet.Data;
using Wallet.Data.Repository;
using Wallet.Models.Profiles;
using Wallet.Services.Contract;
using Wallet.Services;

namespace Wallet
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public AuthorizationOption authoption { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<WalletContext>(options => options.UseSqlServer(connection));
            authoption = Configuration.GetSection("Option").Get<AuthorizationOption>();
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(authoption.Key));
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Data", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
             });
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,

                            ValidIssuer = authoption.Issuer,

                            ValidateAudience = true,

                            ValidAudience = authoption.Audience,

                            ValidateLifetime = true,

                            IssuerSigningKey = key,

                            ValidateIssuerSigningKey = true,
                        };
                    });

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserInfoRepository, UserInfoRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();

            services.AddScoped<ITransactionService, TransactionService>();

            services.AddSingleton<IConfiguration>(Configuration);

            services.AddAutoMapper(typeof(UserProfile), typeof(TransactionProfile), typeof(UserAppProfile) , typeof(TransactionProfile), typeof(TransactionAppProfile));
            services.AddControllersWithViews();
            services.AddSession();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                c.OAuthClientId("swagger-ui");
                c.OAuthClientSecret("swagger-ui-secret");
                c.OAuthRealm("swagger-ui-realm");
                c.OAuthAppName("Swagger UI");
                c.RoutePrefix = String.Empty;
            });

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
