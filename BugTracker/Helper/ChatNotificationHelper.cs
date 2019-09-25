using BugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Helper
{
    public class ChatNotificationHelper : CommonHelper
    {

        public static void  GenerateNewChatNotification(ChatNotification chatnotification)
        {
            var notification = new ChatNotification
            {
                Created = DateTime.Now,
                Subject = $"You are requested to come to chat room on {DateTime.Now}",
                Read = false,
                RecipientId = chatnotification.RecipientId,
                SenderId = HttpContext.Current.User.Identity.GetUserId(),
                NotificationBody = $"Mark Read, if you have read this notification",
                ChatId = chatnotification.ChatId
            };
            db.ChatNotifications.Add(notification);
            db.SaveChanges();

        }

        public static void ChatNotify(ChatNotification chatnotification)

        {

            if (chatnotification.SenderId != chatnotification.RecipientId)

            {

                var notification = new ChatNotification

                {

                    Created = DateTime.Now,

                    NotificationBody = db.Users.Find(chatnotification.SenderId).FullName + " invited to chat room ",

                    SenderId = chatnotification.RecipientId,

                    ChatId = chatnotification.ChatId

                };

                db.ChatNotifications.Add(notification);

                db.SaveChanges();

            }



        }
    }
}