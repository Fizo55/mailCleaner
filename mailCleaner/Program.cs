using System;
using mailCleaner.Helper;

namespace mailCleaner
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            try
            {
                var mailRepository = new MailHelper("imap.gmail.com", 993, true, "YOUREMAIL@gmail.com", "YOURPASSWORD");
                Console.WriteLine("0. Delete unseen mails\n1. Delete seen message");
                mailRepository.GetReadMails(Convert.ToInt32(Console.ReadLine()));
            }
            catch (AggregateException ex)
            {
                foreach (var e in ex.InnerExceptions)
                {
                    Console.WriteLine($"ERROR : {e.Message}");
                }
            }

            Console.ReadLine();
        }
    }
}