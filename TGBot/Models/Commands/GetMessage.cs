using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TGBot.Models.Commands
{
    public class GetMessage : Command
    {
        public override string Name => "getmessage";

        string MySQLConnectionString = "";
        MySqlConnection connection;
        string query;

        private string DB_get(string name, string password)
        {
            string text = "";
            connection = new MySqlConnection(MySQLConnectionString);
            try
            {
                connection.Open();

                query = "select text, date_of_birth, password from table where name='" + name + "';";
                MySqlCommand command = new MySqlCommand(query, connection);

                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    if (reader[2].ToString() != password)
                    {
                        connection.Close();
                        throw new Exception("1");
                    } else
                    {
                        if (reader[1].ToString().CompareTo(System.DateTime.Now.ToString("yyyy-MM-dd")) > 0)
                        {
                            text = reader[0].ToString();
                        }
                        else
                        {
                            connection.Close();
                            throw new Exception("2");
                        }
                    }
                        
                }
                else
                {

                    connection.Close();
                    throw new Exception("3");
                }

                

                connection.Close();
                return text;
            }
            catch (Exception exc)
            {
                if (exc.Message == "1")
                    throw new Exception("Wrong password");
                else if (exc.Message == "2")
                    throw new Exception("Message is outdated");
                else if (exc.Message == "3")
                    throw new Exception("No such message");
                else
                    throw new Exception("Oh no, looks like i can`t connect to database :(");
            }

        }

        public override void Execute(Message message, TelegramBotClient client)
        {
            var chatId = message.Chat.Id;
            var messageId = message.MessageId;
           
            client.SendTextMessageAsync(chatId, "Input message ID and password like this: ID-password");
            
        }

        public bool GetMsg(Message message, TelegramBotClient client)
        {
            var chatId = message.Chat.Id;
            var messageId = message.MessageId;

            
            if (message.Text.Contains("-"))
            {
                char[] charSeparators = new char[] { '-' };
                string[] arr = message.Text.Split(charSeparators, 2);
                arr[0] = Regex.Replace(arr[0], @"\n", "");
                arr[0] = arr[0].Trim();
                arr[1] = Regex.Replace(arr[1], @"\n", "");
                arr[1] = arr[1].Trim();
                string password = arr[1];
                string name = arr[0];
                try
                {
                    client.SendTextMessageAsync(chatId, "Trying to find your message 🧐. Please wait...\n");
                    string text = DB_get(name, password);
                    client.SendTextMessageAsync(chatId, "Text of message:\n" + text);
                    return true;
                }
                catch (Exception exc)
                {
                    client.SendTextMessageAsync(chatId, "Error:\n" + exc.Message);
                    return false;
                }
            }
            client.SendTextMessageAsync(chatId, "Incorect format. Input message ID and password like this: ID-password", replyToMessageId: messageId);
            return false;
           
        }
    }
}