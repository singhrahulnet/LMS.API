using System;
using System.IO;
using System.Reflection;
using LMS.Domain;
using LMS.Helper;
using LMS.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace LMS
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "BookStore API",
                    Description = " ASP.NET Core BookStore Web API",
                    Contact = new Contact
                    {
                        Name = "Rahul",
                        Email = "rahul@email.com",
                        Url = "https://github.com/rahul"
                    }
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.XML";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                c.IncludeXmlComments(xmlPath);
            });

            services.AddCors();
            services.AddDbContext<DataContext>(x => x.UseInMemoryDatabase("TestDb"));

            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IBookAllocationService, BookAllocationService>();
            services.AddScoped<IBookStoreService, BookStoreService>();
            services.AddScoped<IDbContext,DataContext>();
            services.AddScoped<ITransactionManager, TransactionManager>();
            services.AddScoped(typeof(IRepository<>),typeof(Repository<>));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookStore API V1");
                c.RoutePrefix = string.Empty;
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            SeedData.Initialize(app);
            app.UseMvc();
        }
    }
}
