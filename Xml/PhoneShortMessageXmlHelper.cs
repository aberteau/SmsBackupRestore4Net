using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace SmsBackupRestore4Net.Xml
{
    public class PhoneShortMessageXmlHelper
    {
        public static IEnumerable<PhoneShortMessage> GetMessages(string xmlDoc)
        {
            XDocument xDoc = XDocument.Load(xmlDoc);
            IEnumerable<XElement> messageElements = xDoc.Root.Elements("sms");
            return messageElements.Select(e => Map(e));
        }

        private static PhoneShortMessage Map(XElement messageElement)
        {
            PhoneShortMessage phoneCall = new PhoneShortMessage();
            phoneCall.Number = GetNumber(messageElement);
            phoneCall.Type = GetType(messageElement);
            phoneCall.Date = GetDate(messageElement);
            phoneCall.Subject = GetSubject(messageElement);
            phoneCall.Body = GetBody(messageElement);
            phoneCall.SentDate = GetSentDate(messageElement);
            phoneCall.IsRead = GetIsRead(messageElement);
            phoneCall.Status = GetStatus(messageElement);
            phoneCall.CachedName = GetCachedName(messageElement);
            return phoneCall;
        }

        private static String GetNumber(XElement messageElement)
        {
            return messageElement.Attributes("address").SingleOrDefault().Value;
        }

        private static Int32 GetType(XElement messageElement)
        {
            Int32 intType = Int32.Parse(messageElement.Attributes("type").SingleOrDefault().Value);
            return intType;
        }

        private static DateTime GetDate(XElement messageElement)
        {
            string attributeValue = messageElement.Attributes("date").SingleOrDefault().Value;
            long attributeValueInt64 = Int64.Parse(attributeValue);
            return DateTimeHelper.JavaTimeStampToDateTime(attributeValueInt64);
        }

        private static String GetSubject(XElement messageElement)
        {
            string attributeValue = messageElement.Attributes("subject").SingleOrDefault().Value;

            if (String.IsNullOrWhiteSpace(attributeValue))
                return null;

            if (attributeValue.Equals("null", StringComparison.InvariantCultureIgnoreCase))
                return null;

            return attributeValue;
        }

        private static String GetBody(XElement messageElement)
        {
            string attributeValue = messageElement.Attributes("body").SingleOrDefault().Value;

            if (String.IsNullOrWhiteSpace(attributeValue))
                return null;

            if (attributeValue.Equals("null", StringComparison.InvariantCultureIgnoreCase))
                return null;

            return attributeValue;
        }

        private static Nullable<DateTime> GetSentDate(XElement messageElement)
        {
            string attributeValue = messageElement.Attributes("date_sent").SingleOrDefault().Value;
            if (String.IsNullOrWhiteSpace(attributeValue))
                return null;

            long attributeValueInt64 = Int64.Parse(attributeValue);

            if (attributeValueInt64 == 0)
                return null;
            return DateTimeHelper.JavaTimeStampToDateTime(attributeValueInt64);
        }

        private static int GetStatus(XElement messageElement)
        {
            string attributeValue = messageElement.Attributes("status").SingleOrDefault().Value;
            Int32 status = Int32.Parse(attributeValue);
            return status;
        }

        private static bool GetIsRead(XElement messageElement)
        {
            string attributeValue = messageElement.Attributes("read").SingleOrDefault().Value;
            Int32 intIsRead = Int32.Parse(attributeValue);
            return (intIsRead == 1);
        }

        private static String GetCachedName(XElement messageElement)
        {
            string attributeValue = messageElement.Attributes("contact_name").SingleOrDefault().Value;
            return attributeValue;
        }
    }
}
