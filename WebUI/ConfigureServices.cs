using Application.Common.Interfaces.Auth;
using Application.Common.Interfaces.Services;
using Application.Common.InterFaces.DbContext;
using Domain.Enum;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;
using Unchase.Swashbuckle.AspNetCore.Extensions.Extensions;
using Unchase.Swashbuckle.AspNetCore.Extensions.Options;
using WebUI.Auth;
using WebUI.Services;

namespace WebUI
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddWebUIServices(this IServiceCollection services, IConfiguration configuration)
        {
           
            
            //services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddControllers(options =>
            {
                
                options.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();
            });
            //.AddFluentValidation(x => x.AutomaticValidationEnabled = false);
            services.AddCustomAuthentication(configuration);
            services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();

            services.AddHttpContextAccessor();


            services.AddHealthChecks();
            


            //// Customise default API behaviour
            services.Configure<ApiBehaviorOptions>(options =>
                options.SuppressModelStateInvalidFilter = true);

            services.AddSingleton<ICurrentUserService, CurrentUserService>();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "JWTToken_Auth_API",
                    Version = "v1"
                });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement {
            {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] { }
                }
            });


                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.AddEnumsWithValuesFixFilters(o =>
                {
                    // add schema filter to fix enums(add 'x-enumNames' for NSwag or its alias from XEnumNamesAlias) in schema

                    o.ApplySchemaFilter = true;

                    // alias for replacing 'x-enumNames' in swagger document
                    o.XEnumNamesAlias = "x-enum-varnames";

                    // alias for replacing 'x-enumDescriptions' in swagger document
                    o.XEnumDescriptionsAlias = "x-enum-descriptions";

                    // add parameter filter to fix enums (add 'x-enumNames' for NSwag or its alias from XEnumNamesAlias) in schema parameters
                    o.ApplyParameterFilter = true;

                    // add document filter to fix enums displaying in swagger document
                    o.ApplyDocumentFilter = true;

                    // add descriptions from DescriptionAttribute or xml-comments to fix enums (add 'x-enumDescriptions' or its alias from XEnumDescriptionsAlias for schema extensions) for applied filters
                    o.IncludeDescriptions = true;

                    // add remarks for descriptions from xml-comments
                    o.IncludeXEnumRemarks = true;

                    // get descriptions from DescriptionAttribute then from xml-comments
                    o.DescriptionSource = DescriptionSources.DescriptionAttributesThenXmlComments;

                    o.IncludeXmlCommentsFrom(Path.Combine(AppContext.BaseDirectory, xmlFilename));

                });

                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "WebUI.xml"));

            });



            services.AddMvc()
            .AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
            });



            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            return services;
        }

        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services,
                IConfiguration configuration)
        {
            var key = Encoding.ASCII.GetBytes(configuration.GetValue<string>("Auth:Secret"));
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            var x = context.Exception.GetType();
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Headers.Add("Token-Expired", "True");

                                updateSession(context.HttpContext);
                            }
                            context.Response.Headers.Add("access-control-allow-origin", "*");
                            return Task.CompletedTask;
                        },
                    };

                });
            return services;
        }

        private static async Task updateSession(HttpContext context)
        {
            var tokenService = context.RequestServices.GetRequiredService<ITokenService>();
            var dbContext = context.RequestServices.GetRequiredService<IAdminDbContext>();
            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var session = dbContext.Session.Where(m => m.Token == token && m.Status == (int)Status.Active).FirstOrDefault();
            if (session != null)
            {
                if (session.ExpiresAt < DateTime.Now)
                {
                    session.Status = (int)Status.Inactive;
                    dbContext.Session.Update(session);
                    if (await dbContext.SaveChangesAsync(new CancellationToken()) <= 0) throw new Exception("کاربر گرامی در ذخیره سازی اطلاعات خطایی رخ داده است");
                }

            }

        }
    }
    
}
