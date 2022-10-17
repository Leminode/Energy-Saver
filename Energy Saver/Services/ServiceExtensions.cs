namespace Energy_Saver.Services
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddTableServices(this IServiceCollection services)
        {
            return services.AddTransient<ITableService, TableService>();
        }
    }
}
