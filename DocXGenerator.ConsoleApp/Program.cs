using System;
using System.IO;
using System.Reflection;
using NDesk.Options;

namespace SmsBackupRestore4Net.DocXGenerator.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            bool helpFlag = false;
            String outputPath = null;
            String messageXmlFilepath = null;

            OptionSet optionSet = new OptionSet() {
                { "xmlPath=", "the {PATH} of Message XML Backup file", v => messageXmlFilepath = v },
                { "outputPath=", "the {PATH} of genereated docx files.", v => outputPath = v },
                { "h|help",  "show this message and exit", v => helpFlag = v != null },
            };

            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            string applicationName = Path.GetFileName(codeBase);

            try
            {
                optionSet.Parse(args);

                if (String.IsNullOrWhiteSpace(messageXmlFilepath))
                    throw new Exception("messageXmlFilepath must be provided");

                if (String.IsNullOrWhiteSpace(outputPath))
                    throw new Exception("outputPath must be provided");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine($"Use '{applicationName} --help' for more information.");
                return;
            }

            if (helpFlag)
            {
                ShowHelp(applicationName, optionSet);
                return;
            }

            Console.WriteLine($"The files will be generated in folder '{outputPath}'");
            Console.WriteLine("Press [Enter] key to generate files");
            Console.ReadLine();

            Console.WriteLine();
            Console.WriteLine($"Generating files in folder '{outputPath}' ...");

            DocXHelper.CreateDocX(messageXmlFilepath, outputPath);

            Console.WriteLine();
            Console.WriteLine($"Generation succeeded in folder '{outputPath}'");
            Console.WriteLine("Press [Enter] key to quit...");
            Console.ReadLine();
        }

        static void ShowHelp(string applicationName, OptionSet p)
        {
            Console.WriteLine($"Usage: '{applicationName}' [OPTIONS]");
            Console.WriteLine();
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);
        }
    }
}
}
