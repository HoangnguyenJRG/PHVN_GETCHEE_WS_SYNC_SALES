namespace PHVN_WS_CORE.Workers
{
    public static class WorkerExtensions
    {
        public static void AddWorkders(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddScoped(typeof(BrazeWorker));
            services.AddScoped(typeof(GetCheeWorker));
        }
    }
}
