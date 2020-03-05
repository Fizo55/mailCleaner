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
                mailRepository.GetReadMails();
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