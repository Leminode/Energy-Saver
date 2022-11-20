using static Energy_Saver.Services.NotificationService;

namespace Energy_Saver.Services
{
    public interface INotificationService
    {
        public void CreateNotification(object source, NotificationArgs args);
    }
}
