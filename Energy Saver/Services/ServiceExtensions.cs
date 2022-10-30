namespace Energy_Saver.Services
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddTableServices(this IServiceCollection services)
        {
            return services.AddScoped<ITableService, TableService>();
        }

        public static IServiceCollection AddChartServices(this IServiceCollection services)
        {
            return services.AddScoped<IChartService, ChartService>();
        }
    }
}
