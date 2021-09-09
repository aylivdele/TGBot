using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TGBot.Models.Commands
{
    public class StartCommand : Command
    {
        public override string Name => "start";

        public override void Execute(Message message, TelegramBotClient client)
        {
            var chatId = message.Chat.Id;
            var messageId = message.MessageId;


            client.SendTextMessageAsync(chatId, "Hello, i`m bot with which you can send an anonymous password protected message. Messages self-destruct after 30 days. \nTo create new message use command /newmessage\nTo get existing message use /getmessage");
        }
    }
}