using Telegram.Bot;
using Telegram.Bot.Types;

namespace TGBot.Models.Commands
{
    public class HelloCommand : Command
    {
        public override string Name => "hello";

        public override void Execute(Message message, TelegramBotClient client)
        {
            var chatId = message.Chat.Id;
            var messageId = message.MessageId;


            client.SendTextMessageAsync(chatId, "Hello!", replyToMessageId: messageId);
        }
    }
}