using System;
using System.Collections.Generic;
using System.Linq;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;

namespace mailCleaner.Helper
{
    public class MailHelper
    {
        private readonly string mailServer, login, password;
        private readonly int port;
        private readonly bool ssl;

        public MailHelper(string mailServer, int port, bool ssl, string login, string password)
        {
            this.mailServer = mailServer;
            this.port = port;
            this.ssl = ssl;
            this.login = login;
            this.password = password;
        }

        public void GetReadMails(int messageType)
        {
            var messages = new List<string>();

            using var client = new ImapClient();
            client.Connect(mailServer, port, ssl);
            client.AuthenticationMechanisms.Remove("XOAUTH2");

            client.Authenticate(login, password);

            var inbox = client.Inbox;
            inbox.Open(FolderAccess.ReadWrite);
            
            var results = messageType switch
            {
                0 => inbox.Search(SearchOptions.All, SearchQuery.Seen),
                1 => inbox.Search(SearchOptions.All, SearchQuery.Seen),
                _ => inbox.Search(SearchOptions.All, SearchQuery.All)
            };

            messages.AddRange(results?.UniqueIds.Select(uniqueId => inbox.GetMessage(uniqueId)).Select(message => message.HtmlBody));

            DeleteMails(inbox, client);
        }


        private static void DeleteMails(IMailFolder inbox, ImapClient client)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            
            for (int i = 0; i < inbox.Count; i++)
            {
                inbox.AddFlags(i, MessageFlags.Deleted, true);
            }
            
            inbox.Expunge();
            client.Disconnect(true);
            Console.WriteLine("Your mail have been deleted");
        }
    }
}