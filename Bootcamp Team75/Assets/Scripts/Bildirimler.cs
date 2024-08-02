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
        // Bildirim izni kontrolü ve isteği
        if (!Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS"))
        {
            Permission.RequestUserPermission("android.permission.POST_NOTIFICATIONS");
        }
        else
        {
            // İzin verilmişse bildirim kanalını oluştur
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

        // İlk bildirim
        var firstNotification = new AndroidNotification
        {
            Title = "Oyun Vakti!",
            Text = "🌻 Kendi bahçeni oluşturmaya ne dersin? 🌼 ",
            FireTime = System.DateTime.Now.AddSeconds(10),
            LargeIcon = "logo", // Resources klasöründe bulunan logo.png
            SmallIcon = "logo"  // Küçük simgeyi de benzer şekilde ayarlayabilirsiniz, varsayılan ikon kullanılabilir.
        };

        firstNotificationId = AndroidNotificationCenter.SendNotification(firstNotification, "channel_id");

        // İkinci bildirim
        var secondNotification = new AndroidNotification
        {
            Title = "Çiçeğiniz Kuruyor!",
            Text = "💧 Bir an önce çiçeğinizi sulayın! 💧",
            FireTime = System.DateTime.Now.AddSeconds(120),
            LargeIcon = "logo", // Resources klasöründe bulunan logo.png
            SmallIcon = "logo"  // Küçük simgeyi de benzer şekilde ayarlayabilirsiniz, varsayılan ikon kullanılabilir.
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
