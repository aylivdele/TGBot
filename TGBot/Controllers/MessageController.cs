using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Telegram.Bot.Types;
using TGBot.Models;
using TGBot.Models.Commands;

namespace TGBot.Controllers
{
    public class MessageController : ApiController
    {
        private static Dictionary<long, string> LastCommand = new Dictionary<long, string>();
        [Route(@"api/message/update")] 
        public async Task<OkResult> Update([FromBody] Update update)
        {
            var commands = Bot.Commands;
            var message = update.Message;
            var client = await Bot.Get();
            bool found = false;
            long chatId = message.Chat.Id;
            if (message.Text != null)
            {
                foreach (var command in commands)
                {
                    if (command.Contains(message.Text))
                    {
                        try
                        {
                            command.Execute(message, client);
                            if (LastCommand.ContainsKey(chatId))
                                LastCommand[chatId] = command.Name;
                            else
                                LastCommand.Add(chatId, command.Name);
                            found = true;
                            break;
                        }
                        catch (Exception e)
                        {
                            await client.SendTextMessageAsync(chatId, "Error\n" + e.Message);
                        }
                    }
                }
                try
                {
                    if (!found)
                        switch (LastCommand[chatId])
                        {
                            case "newmessage":
                                NewMessage newtemp = new NewMessage();
                                if (newtemp.SetText(message, client))
                                {
                                    LastCommand[chatId] = "start";
                                }
                                break;
                            case "getmessage":
                                GetMessage gettemp = new GetMessage();
                                if (gettemp.GetMsg(message, client))
                                {
                                    LastCommand[chatId] = "start";
                                }
                                break;
                            default:

                                break;
                        }
                }
                catch (Exception e)
                {
                    await client.SendTextMessageAsync(chatId, "Error:\n" + e.Message);
                }
            }

            return Ok();
        }

    }
}