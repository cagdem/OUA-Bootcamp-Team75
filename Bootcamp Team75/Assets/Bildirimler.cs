using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.Android;
using UnityEngine;
using UnityEngine.Android;

public class Bildirimler : MonoBehaviour
{
    private bool isPaused;
    private bool notificationsScheduled = false;

    // Bildirim kimlikleri
    private int firstNotificationId;
    private int secondNotificationId;

    // Start is called before the first frame update
    void Start()
    {
        // Bildirim izni kontrolü ve isteði
        if (!Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS"))
        {
            Permission.RequestUserPermission("android.permission.POST_NOTIFICATIONS");
        }
        else
        {
            // Ýzin verilmiþse bildirim kanalýný oluþtur
            CreateNotificationChannel();
        }
    }

    private void CreateNotificationChannel()
    {
        var channel = new AndroidNotificationChannel()
        {
            Id = "channel_id",
            Name = "Notifications Channel",
            Importance = Importance.Default,
            Description = "Generic notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }

    private void ScheduleNotifications()
    {
        if (notificationsScheduled) return;

        // Ýlk bildirim
        var firstNotification = new AndroidNotification
        {
            Title = "Sulama Vakti!",
            Text = "Çiçeklerinin suya ihtiyacý var gibi görünüyor!",
            FireTime = System.DateTime.Now.AddSeconds(10),
            LargeIcon = "logo", // Resources klasöründe bulunan logo.png
            SmallIcon = "logo"  // Küçük simgeyi de benzer þekilde ayarlayabilirsiniz, varsayýlan ikon kullanýlabilir.
        };

        firstNotificationId = AndroidNotificationCenter.SendNotification(firstNotification, "channel_id");

        // Ýkinci bildirim
        var secondNotification = new AndroidNotification
        {
            Title = "Kontrol Zamaný!",
            Text = "Bahçenizin durumunu kontrol edin!",
            FireTime = System.DateTime.Now.AddSeconds(15),
            LargeIcon = "logo", // Resources klasöründe bulunan logo.png
            SmallIcon = "logo"  // Küçük simgeyi de benzer þekilde ayarlayabilirsiniz, varsayýlan ikon kullanýlabilir.
        };

        secondNotificationId = AndroidNotificationCenter.SendNotification(secondNotification, "channel_id");

        notificationsScheduled = true;
    }

    private void CancelScheduledNotifications()
    {
        // Sadece planlanan bildirimleri iptal et
        if (AndroidNotificationCenter.CheckScheduledNotificationStatus(firstNotificationId) == NotificationStatus.Scheduled)
        {
            AndroidNotificationCenter.CancelNotification(firstNotificationId);
        }

        if (AndroidNotificationCenter.CheckScheduledNotificationStatus(secondNotificationId) == NotificationStatus.Scheduled)
        {
            AndroidNotificationCenter.CancelNotification(secondNotificationId);
        }

        notificationsScheduled = false;
    }

    void OnApplicationPause(bool pauseStatus)
    {
        isPaused = pauseStatus;

        if (isPaused)
        {
            ScheduleNotifications();
        }
        else
        {
            CancelScheduledNotifications();
        }
    }

    void OnApplicationFocus(bool focusStatus)
    {
        if (!focusStatus)
        {
            ScheduleNotifications();
        }
        else
        {
            CancelScheduledNotifications();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
