﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNetRestFul.Filters;
using ASPNetRestFul.Infrastructure;
using ASPNetRestFul.Models;
using ASPNetRestFul.Services;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ASPNetRestFul
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<HotelInfo>(
                Configuration.GetSection("Info"));

            //AddScoped- a new instance of RoomService will be created
            //4 evry incoming rqst, rather than a Singeton svc wc wll 
            //only be created once
            services.AddScoped<IRoomService, DefaultRoomService>();

            services.AddDbContext<HotelApiDbContext>( 
                options => options.UseInMemoryDatabase("manchesterdb")
            );

           

            services
                .AddMvc(options =>
                {
                    options.Filters.Add<JsonExceptionFilter>();
                    options.Filters.Add<RequireHttpsOrCloseAttribute>();
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services
                .AddRouting(options => options.LowercaseUrls = true);

            services
                .AddApiVersioning(options =>
                {
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                    options.ApiVersionReader = new MediaTypeApiVersionReader();
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.ReportApiVersions = true;
                    options.ApiVersionSelector =
                                   new CurrentImplementationApiVersionSelector(options);
                });

            services
                .AddCors(options =>
                {
                    options.AddPolicy("AllowMyApp",
                        policy => policy
                            .AllowAnyOrigin());
                });

            services.AddAutoMapper(typeof(MappingProfile));
            //    .AddAutoMapper(
            //   options => options.AddProfile<MappingProfile>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //CORS: above any method tt generates a response eg MVC
            app.UseCors("AllowMyApp");

            app.UseMvc();
          
        }
    }
}
