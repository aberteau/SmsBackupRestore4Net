using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using SmsBackupRestore4Net.Xml;
using Xceed.Words.NET;

namespace SmsBackupRestore4Net.DocXGenerator
{
    public class DocXHelper
    {
        private const Alignment ReceivedMessageAlignment = Alignment.left;
        private const Alignment SentMessageAlignment = Alignment.right;

        private static readonly Color ReceivedMessageColor = Color.DodgerBlue;
        private static readonly Color SentMessageColor = Color.Orange;

        private const int MessageLayoutCellMarginBottom = 10;
        private const int MessageCellWidth = 350;
        private const int MessageBodyFontSize = 10;
        private const int MessageDateFontSize = 8;

        private static DocX CreateDocX(string fileName, IEnumerable<PhoneShortMessage> messages)
        {
            DocX document = DocX.Create(fileName);
            document.PageLayout.Orientation = Orientation.Portrait;
            SetContent(document, messages);
            return document;
        }

        public static void CreateDocX(string messageXmlFilepath, string outputPath)
        {
            IEnumerable<PhoneShortMessage> messages = PhoneShortMessageXmlHelper.GetMessages(messageXmlFilepath);
            DocX docX = CreateDocX(outputPath, messages);
            docX.Save();
        }


        private static void SetContent(DocX document, IEnumerable<PhoneShortMessage> messages)
        {
            Table t = AddTable(messages, document);
            document.InsertTable(t);
        }

        private static Table AddTable(IEnumerable<PhoneShortMessage> dtos, DocX document)
        {
            int rowCount = dtos.Count();
            Table layoutTable = document.AddTable(rowCount, 1);
            int i = 0;
            Border border = new Border(BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Transparent);
            layoutTable.SetBorder(TableBorderType.Bottom, border);
            layoutTable.SetBorder(TableBorderType.Top, border);
            layoutTable.SetBorder(TableBorderType.Left, border);
            layoutTable.SetBorder(TableBorderType.Right, border);
            layoutTable.SetBorder(TableBorderType.InsideH, border);
            layoutTable.SetBorder(TableBorderType.InsideV, border);

            foreach (PhoneShortMessage dto in dtos)
            {
                Alignment alignment = GetAlignment(dto.Type);

                Cell messageLayoutCell = layoutTable.Rows[i].Cells[0];

                Paragraph firstParagraph = messageLayoutCell.Paragraphs.FirstOrDefault();
                messageLayoutCell.RemoveParagraph(firstParagraph);

                messageLayoutCell.MarginBottom = MessageLayoutCellMarginBottom;
                FillMessageLayoutCell(messageLayoutCell, alignment, dto);

                i++;
            }
            return layoutTable;
        }

        private static void FillMessageLayoutCell(Cell messageLayoutCell, Alignment alignment, PhoneShortMessage dto)
        {
            Table messageTable = messageLayoutCell.InsertTable(1, 1);
            messageTable.Alignment = alignment;

            Cell messageCell = messageTable.Rows[0].Cells[0];
            messageCell.Width = MessageCellWidth;
            messageCell.FillColor = GetColor(dto.Type);

            Paragraph bodyParagraph = messageCell.Paragraphs.First();
            bodyParagraph.Append(dto.Body);
            bodyParagraph.FontSize(MessageBodyFontSize);

            Paragraph dateParagraph = bodyParagraph.InsertParagraphAfterSelf($"{dto.Date}");
            dateParagraph.Alignment = alignment;
            dateParagraph.FontSize(MessageDateFontSize);
        }

        private static Alignment GetAlignment(int typeId)
        {
            return (typeId == 1) ? ReceivedMessageAlignment : SentMessageAlignment;
        }

        private static Color GetColor(int typeId)
        {
            return (typeId == 1) ? ReceivedMessageColor : SentMessageColor;
        }

        public static MemoryStream GetDocXMemoryStream(string fileName, IEnumerable<PhoneShortMessage> dtos)
        {
            DocX document = CreateDocX(fileName, dtos);
            MemoryStream ms = new MemoryStream();
            document.SaveAs(ms);
            return ms;
        }

        public static byte[] GetDocXBytes(string fileName, IEnumerable<PhoneShortMessage> dtos)
        {
            MemoryStream memoryStream = GetDocXMemoryStream(fileName, dtos);
            byte[] byteArray = memoryStream.ToArray();
            return byteArray;
        }
    }
}
