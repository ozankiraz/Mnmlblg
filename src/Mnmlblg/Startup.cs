using System.IO;
using System.Text;
using MarkdownSharp;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Mnmlblg.Configuration;
using Mnmlblg;

namespace Mnmlblg
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
            configurationBuilder.AddJsonFile("appsettings.json");
            var settings = configurationBuilder.Build();

            services.Configure<MarkdownOptions>(options => settings.GetSection("MarkdownOptions").Bind(options));
            services.Configure<BlogConfiguration>(options => settings.GetSection("BlogConfiguration").Bind(options));

            services.AddSingleton<IMarkDown, Markdown>();

            services.AddSingleton<IRss2FeedGenerator, Rss2FeedGenerator>();
            services
                .AddMvc()
                .AddXmlSerializerFormatters()
                .AddXmlDataContractSerializerFormatters();

            services.AddSingleton<IPostRepository, SamplePostRepository>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseStaticFiles();

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("windows-1254");
            Encoding.GetEncoding("Cyrillic");
        }
    }
}
