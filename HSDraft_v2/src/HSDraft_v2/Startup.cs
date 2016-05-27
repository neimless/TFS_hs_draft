using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.Framework.DependencyInjection;
using HSDraft_v2.Models;
using Microsoft.Data.Entity.SqlServer;
using Microsoft.Framework.Runtime;
using System.IO;
using HSDraft_v2.BusinessLogic;
using Microsoft.AspNet.Identity.EntityFramework;

namespace HSDraft_v2
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddEntityFramework().AddSqlServer().AddDbContext<CardAppContext>();
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<CardAppContext>();
        }

        public void Configure(IApplicationBuilder app, IApplicationEnvironment env)
        {
            app.UseMvc();
            var cardBasePath = Path.Combine(env.ApplicationBasePath, @"Files\CardBase.json");
            var cards = FileReader.GetCardBase(cardBasePath);
            //CreateSampleData(app.ApplicationServices, cards).Wait();
        }

        private static async Task CreateSampleData(IServiceProvider applicationServices, List<Card> cards)
        {
            using (var dbContext = applicationServices.GetService<CardAppContext>())
            {
                var sqlServerDatabase = dbContext.Database as SqlServerDatabase;
                if (sqlServerDatabase != null)
                {
                    //sqlServerDatabase.Delete();
                    //sqlServerDatabase.Create();
                    sqlServerDatabase.CreateTables();
                    cards.ForEach(m => dbContext.Cards.Add(m));
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
