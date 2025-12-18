using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using SyncNode.Settings;
using SyncNode.Services;

namespace SyncNode
{
    public class Startup
    {
        public Startup(IConfiguration configuration) 
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

            services.Configure<MovieAPISettings>(Configuration.GetSection("MovieAPISettings"));

            services.AddSingleton<IMovieAPISettings>(provider =>
                provider.GetRequiredService<IOptions<MovieAPISettings>>().Value);

            services.AddSingleton<SyncWorkJobService>();
            services.AddHostedService(provider => provider.GetService<SyncWorkJobService>());

            services.AddControllers();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
