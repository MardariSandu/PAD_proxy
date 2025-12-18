using Common.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MovieAPI.Repositories;
using MovieAPI.Services;
using MovieAPI.Settings;

namespace MovieAPI
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

            services.AddHttpContextAccessor();
            services.Configure<MongoDbSettings>(Configuration.GetSection("MongoDbSettings"));
            services.Configure<SyncServiceSettings>(Configuration.GetSection("SyncServiceSettings"));

            services.AddSingleton<IMongoDbSettings>(provider =>
                provider.GetRequiredService<IOptions<MongoDbSettings>>().Value);

            services.AddSingleton<ISyncServiceSettings>(provider =>
                provider.GetRequiredService<IOptions<SyncServiceSettings>>().Value);

            services.AddScoped<IMongoRepository<Movie>, MongoRepository<Movie>>();
            services.AddScoped<ISyncService<Movie>, SyncService<Movie>>();

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
