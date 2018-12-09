using LMS.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Helper
{
    public static class SeedData
    {
        public static void Initialize(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<DataContext>();
                // Seed the database.
                if (!context.Books.Any())
                {
                    context.AddRange(SampleData.Books);
                }
                if (!context.Students.Any())
                {
                    context.AddRange(SampleData.Students);
                }
                if (!context.IssuedBooks.Any())
                {
                    context.AddRange(SampleData.IssuedBooks);
                }
                context.SaveChanges();
            }
        }
    }
}
