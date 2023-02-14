using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RepositoryLayer.Context;
using RepositoryLayer.Interfaces;
using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using RepositoryLayer.Services;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MassTransit;

namespace FundooNotesApplication
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        

        // This method gets called by the runtime. Use this method to add services to the dependency injection container.
        //this method is used to configure required services
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(); //we configure the services required for Web API applications with no views
            services.AddDbContext<FundooDBContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:FundooDB"]));
            //transient-service lifetime
            //transient,scoped,singleton
            

            

            services.AddTransient<IUserRepository,UserRepository>();
            services.AddTransient<IUserBusiness,UserBusiness>();
            services.AddTransient<INoteRepository,NoteRepository>();
            services.AddTransient<INoteBusiness,NoteBusiness>();

            services.AddTransient<ILabelRepository, LabelRepository>();
            services.AddTransient<ILabelBusiness, LabelBusiness>();

            services.AddTransient<ICollabRepository, CollabRepository>();
            services.AddTransient<ICollabBusiness, CollabBusiness>();

            services.AddMemoryCache();
            services.AddStackExchangeRedisCache(
               options =>
               {
               options.Configuration = "localhost:6379";
                });

            //used for adding swagger generator to service collection
            services.AddSwaggerGen(a =>
            {
                //to customise our swagger doc
                //a.SwaggerDoc(
                //   "v1.0",
                //   new OpenApiInfo
                //   {
                //       Title = "FundooNotesApplication",
                //       Description = "Manage the notes and remainds you everything",
                //       Version = "1.0"
                //   });
                //
                a.AddSecurityDefinition(
                    "Bearer",
                    new OpenApiSecurityScheme()
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description = "JWT Authorization header using the Bearer Scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\\r\\n\\r\\nExample: \\\"Bearer 12345abcdef\\\"\"",

                    });
                a.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                    new OpenApiSecurityScheme()
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }

                    },
                    new string[]
                    {}
                    }
                });

            });
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
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.Configuration["JWT:SecretKey"]))

                };
            }
            );

            services.AddMassTransit(x =>
            {
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
                {
                    config.UseHealthCheck(provider);
                    config.Host(new Uri("rabbitmq://localhost"), h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                }));
            });
            services.AddMassTransitHostedService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request processing pipeline.
        //configure all the middlewares
        
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) //app-configure the middlewares, env-  info abt hosting environment
        {
            //used to display error page if route isnt specified for different resources
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication(); // to authenticate the user
            app.UseHttpsRedirection();

            app.UseRouting(); //are used to add and configure the routing middleware to the request processing pipeline



            //This middleware serves generated Swagger document as a JSON endpoint
            app.UseSwagger();

            // This middleware serves the Swagger documentation UI
            //lets your users test the API calls directly in the browser.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fundoo API V1");
            });

            app.UseAuthorization(); //used to authorise the user
            

            //no mapping was there in use routing, so here we tell mapping between url and resource
            app.UseEndpoints(endpoints =>
            {
                //Map Controller add endpoints without specifying any routes
                endpoints.MapControllers();
            });
        }
    }
}
