using NToastNotify;

namespace Energy_Saver.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IToastNotification _toastNotification;

        public NotificationService(IToastNotification toastNotification)
        {
            _toastNotification = toastNotification;
        }

        public void CreateNotification(object? source, NotificationArgs args)
        {
            switch (args.Type)
            {
                case NotificationType.Success:
                    _toastNotification.AddSuccessToastMessage(args.Message);
                    break;
                case NotificationType.Info:
                    _toastNotification.AddInfoToastMessage(args.Message);
                    break;
                case NotificationType.Error:
                    _toastNotification.AddErrorToastMessage(args.Message);
                    break;
                case NotificationType.Warning:
                    _toastNotification.AddWarningToastMessage(args.Message);
                    break;
                default:
                    break;
            }
            
        }

        public class NotificationArgs : EventArgs
        {
            public string Message { get; set; } = "";
            public NotificationType Type { get; set; }
        }

        public enum NotificationType
        {
            Success,
            Info,
            Error,
            Warning
        }
    }
}
