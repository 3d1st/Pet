using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Xsl;
using CodeJam.Collections;
using CodeJam.Strings;
using Contracts;
using Contracts.Exceptions;
using Contracts.Terms;
using EquationProcessor;
using Parser;

namespace TestWork
{
    static class Program
    {
        private static readonly IEquationParser _Parser = new SimpleParserImpl();
        private static readonly IEquationProcessor _Processor = new SimpleEquationProcessorImpl();

        public static async Task Main(string[] args)
        {
            var filePath = string.Empty;
            if (!args.IsNullOrEmpty())
            {
                if (File.Exists(args[0]))
                {
                    filePath = args[0];
                }
            }

            if (filePath.IsNullOrWhiteSpace())
            {
                Console.WriteLine("Input equation and press Enter. To stop application press Ctrl+C...");
                do
                {
                    string input = Console.ReadLine();
                    Console.WriteLine(Process(input));
                } while (true);   
            }
            else
            {
                string output = filePath + ".out";
                var data = await File.ReadAllLinesAsync(filePath);
                var processed = data.Select(Process);
                await File.WriteAllLinesAsync(output, processed);
            }
        }
        
        private static string Process(string input)
        {
            string result = input + "->";
            
            try
            {
                EquationTermBase left, right;
                (left, right) = _Parser.ParseEquation(input);

                var term = _Processor.SwapSides(left, right);

                var simplified = _Processor.Simplify(term);

                result += _Processor.Display(simplified) + "=0";
            }
            catch (ParsingExceptionBase ex)
            {
                result += ex.Message;
            }
            catch (ProcessingExceptionBase ex)
            {
                result += ex.Message;
            }
            catch (Exception ex)
            {
                result += "Unknown exception:" + ex.Message;
            }

            return result;
        }
    }
}