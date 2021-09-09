using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;
using MySql.Data.MySqlClient;
using System.Data;
using System;

namespace TGBot.Models.Commands
{
    public class NewMessage : Command
    {

        string MySQLConnectionString = "";
        MySqlConnection connection;
        string query;

        public override string Name => "newmessage";

        public override void Execute(Message message, TelegramBotClient client)
        {
            var chatId = message.Chat.Id;
            var messageId = message.MessageId;


            client.SendTextMessageAsync(chatId, "Input text and password of your message like this: password-text");
        }

        private bool DB_load(string name, string password, string text, string date)
        {
            connection = new MySqlConnection(MySQLConnectionString);
            try
            {
                connection.Open();

                query = "insert into table (name, text, date_of_birth, password) values ('" + name + "', '" + text + "', '" + date + "','" + password + "');";
                MySqlCommand command = new MySqlCommand(query, connection);

                command.ExecuteNonQuery();

                connection.Close();

                return true;
            }
            catch (Exception exc)
            {
                connection.Close();
                throw new Exception("Oh no, looks like i can`t connect to database :(");
            }

        }

        private string GenerateName(int n = 10)
        {
            string pass = "";
            string chars = "1234567890";
            Random rnd = new Random();
            for (int i = 0; i < n; i++)
            {
                pass += chars[rnd.Next(chars.Length)];
            }

            return pass;
        }

        public bool SetText(Message message, TelegramBotClient client)
        {
            var chatId = message.Chat.Id;
            var messageId = message.MessageId;
            
            if (message.Text.Contains("-"))
            {
                char[] charSeparators = new char[] { '-' };
                string[] arr = message.Text.Split(charSeparators, 2);
                string name = GenerateName();
                arr[0] = Regex.Replace(arr[0], @"\n", "");
                arr[0] = arr[0].Trim();
                string password = arr[0];
                string text = arr[1].TrimStart();
                string date = System.DateTime.Now.AddDays(30).ToString("yyyy-MM-dd");
                try
                {
                    DB_load(name, password, text, date);
                    client.SendTextMessageAsync(chatId, "ID of your message: \n" + name + "\nPassword is:\n" + password + "\nText is:\n" + text + "\nWill self-destruct on: " + date);
                    return true;
                } catch (Exception exc)
                {
                    client.SendTextMessageAsync(chatId, "Error:\n" + exc.Message);
                    return false;
                }
            }
            else
            {
                client.SendTextMessageAsync(chatId, "Incorect format. Input your password and text like this: password-text", replyToMessageId: messageId);
                return false;
            }
            
            
        }
    }
}