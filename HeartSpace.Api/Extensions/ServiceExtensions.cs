using HeartSpace.Api.Middleware;
using HeartSpace.Application.Configuration;
using HeartSpace.Application.Services.AppointmentService;
using HeartSpace.Application.Services.AuthService;
using HeartSpace.Application.Services.ConsultantService;
using HeartSpace.Application.Services.ConsultingService;
using HeartSpace.Application.Services.PaymentRequestService;
using HeartSpace.Application.Services.PaymentService;
using HeartSpace.Application.Services.ScheduleService;
using HeartSpace.Application.Services.StatisticService;
using HeartSpace.Application.Services.TokenService;
using HeartSpace.Application.Services.UserService;
using HeartSpace.Domain.Repositories;
using HeartSpace.Infrastructure;
using HeartSpace.Infrastructure.Persistence;
using HeartSpace.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using System.Text;




namespace HeartSpace.Api.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
            // ✅ Register repositories first
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IConsultantProfileRepository, ConsultantProfileRepository>();
            services.AddScoped<IConsultantConsultingRepository, ConsultantConsultingRepository>();
            services.AddScoped<IConsultingRepository, ConsultingRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IScheduleRepository, ScheduleRepository>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IPaymentRequestRepository, PaymentRequestRepository>();


            // ✅ Then UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IScheduleService, ScheduleService>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<IConsultantService, ConsultantService>();
            services.AddScoped<IConsultingService, ConsultingService>();
            services.AddScoped<PayOsService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IPaymentRequestService, PaymentRequestService>();
            services.AddScoped<IStatisticService, StatisticService>();



        }
        public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            Console.WriteLine(configuration.GetConnectionString("HeartSpaceDb"));
            services.AddDbContext<RepositoryContext>(options =>
                                                     options.UseSqlServer(configuration.GetConnectionString("HeartSpaceDb")));

        }
        public static void ConfigureCors(this IServiceCollection services) => services.AddCors(options =>
        {
            //options.AddPolicy("CorsPolicy", builder =>
            //	builder.WithOrigins("http://localhost:5173")
            //	.AllowAnyMethod()
            //	.AllowAnyHeader()
            //	.AllowCredentials()
            //	.WithExposedHeaders("X-Pagination"));

            options.AddPolicy("AllowHeartSpace", policy =>
            {
                policy.WithOrigins("http://localhost:3000", "https://localhost:3000", "http://localhost:5173", "http://127.0.0.1:5500", "http://127.0.0.1:5501", "https://b7e30a277c99.ngrok-free.app", "http://127.0.0.1:5500",
                    "https://75588b973a34.ngrok-free.app",
                    "https://exe-201-heart-space-be.vercel.app")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        public static void ConfigureSwaggerForAuthentication(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description =
                        "JWT Authorization header using the Bearer scheme. \r\n\r\n" +
                        "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
                        "Example: \"Bearer [token]\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                });
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "TestSetUp", Version = "v1" });
                options.OperationFilter<SecurityRequirementsOperationFilter>();
                //**Main project's XML docs
                var apiXml = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var apiXmlPath = Path.Combine(AppContext.BaseDirectory, apiXml);
                options.IncludeXmlComments(apiXmlPath, includeControllerXmlComments: true);

                //**TestSetUp.Domain** XML docs for QueryParams ...
                var businessXml = Path.Combine(AppContext.BaseDirectory, "TestSetUp.Domain.xml");
                if (File.Exists(businessXml))
                {
                    options.IncludeXmlComments(businessXml);
                }
            });
        }
        public static void ConfigureGlobalException(this IServiceCollection services)
        {
            services.AddProblemDetails();
            services.AddExceptionHandler<GlobalExceptionMiddleware>();
        }
        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["secretKey"];

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["validIssuer"],
                    ValidAudience = jwtSettings["validAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });
        }

        public async static void Seed(this IApplicationBuilder builder)
        {
            using (var scope = builder.ApplicationServices.CreateScope())
            {
                await DbInitializer.SeedAsync(scope.ServiceProvider);
            }
        }
    }
}
