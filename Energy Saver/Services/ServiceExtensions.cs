namespace Energy_Saver.Services
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddTableServices(this IServiceCollection services)
        {
            return services.AddTransient<ITableService, TableService>();
        }

        public static IServiceCollection AddChartServices(this IServiceCollection services)
        {
            return services.AddTransient<IChartService, ChartService>();
        }

        public static IServiceCollection AddSuggestionServices(this IServiceCollection services)
        {
            return services.AddTransient<ISuggestionsService, SuggestionsService>();
        }
    }
}
