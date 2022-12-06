namespace Energy_Saver.Services
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddChartServices(this IServiceCollection services)
        {
            return services.AddScoped<IChartService, ChartService>();
        }

        public static IServiceCollection AddSuggestionServices(this IServiceCollection services)
        {
            return services.AddScoped<ISuggestionsService, SuggestionsService>();
        }

        public static IServiceCollection AddNotificationServices(this IServiceCollection services)
        {
            return services.AddScoped<INotificationService, NotificationService>();
        }

        public static IServiceCollection AddEmailDataExtractorService(this IServiceCollection services)
        {
            return services.AddScoped<IEmailDataExtractorService, EmailDataExtractorService>();
        }

        public static IServiceCollection AddUserProfileServices(this IServiceCollection services)
        {
            return services.AddScoped<IUserProfileService, UserProfileService>();
        }
    }
}
