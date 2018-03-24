using System;

namespace SmsBackupRestore4Net.Xml
{
    public class PhoneShortMessage
    {
        public String Number { get; set; }

        public Int32 Type { get; set; }

        public DateTime Date { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public Nullable<DateTime> SentDate { get; set; }

        public bool IsRead { get; set; }

        public Int32 Status { get; set; }

        public String CachedName { get; set; }
    }
}
