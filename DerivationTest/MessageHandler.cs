/*
Derivation
  
Written in 2015 by <Olga Miller> <olga.rgb@googlemail.com>
To the extent possible under law, the author(s) have dedicated all copyright and related and neighboring rights to this software to the public domain worldwide.
This software is distributed without any warranty.
You should have received a copy of the CC0 Public Domain Dedication along with this software. If not, see <http://creativecommons.org/publicdomain/zero/1.0/>.
*/

using Derivation.Nodes;
using Derivation.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DerivationTest
{
    public static class MessageHandler
    {
        public static string GetMessage(Exception ex, string input, string expected, string actual)
        {
            string message;

            if (ex is AssertFailedException)
                message = string.Format("\n\nFunction:\n\t{0}\nExpected:\n\t{1}\nActual:\n\t{2}\n", input, expected, actual);
            else if (ex is SyntaxException)
                message = string.Format("\n\nSyntax Error:\n{0}\n\nInput:\n{1}\n", ex.Message, input);
            else
                message = string.Format("\n\nProgram Error:\nMessage:\n\t{0}\nFunction:\n\t{1}\n", ex.Message, input);

            return message;
        }

        public static string GetMessage(Exception ex, string input, Type expectedExceptionType, Exception actualException)
        {
            string title = string.Empty;
            string content = string.Empty;

            if (ex is SyntaxException)
            {
                title = "Wrong Arguments in:\n\t" + typeof(MessageHandler).Name;
                content = "Expected:\n\t" + expectedExceptionType.Name;
                throw new ArgumentException(FormatMessage(title, content, input));
            }

            if (ex is AssertFailedException)
            {
                title = "Failed:";
                content = string.Format("Expected:\n\t{0}\nActual:\n\t{1}\nMessage:\n{2}",
                    expectedExceptionType.Name, actualException.GetType().Name, actualException.Message);
            }
            else if (ex is ArgumentException)
            {
                title = "Argument Exception (Test or Program Error):";
                content = string.Format("Expected:\n\t{0}", expectedExceptionType.Name);
            }
            else
            {
                title = "Program Error:";
                content = string.Format("Exception Type:\n\t{0}\nMessage:\n\t{1}", ex.GetType().Name, ex.Message);
            }

            return FormatMessage(title, content, input);
        }

        public static string GetMessage(Node n1, Node n2, bool expected)
        {
            string title = "Node Equal Test Failed:";
            string content = string.Format("Expected:\n\t{0}\nNode1:\n\t{1}\nNode2:\n\t{2}", expected, n1, n2);

            return FormatMessage(title, content);
        }

        private static string FormatMessage(string title, string content, string input)
        {
            return string.Format("\n\n{0}\n\nFunction:\n\t{1}\n{2}\n", title, input, content);
        }

        private static string FormatMessage(string title, string content)
        {
            return string.Format("\n\n{0}\n\n{1}\n", title, content);
        }
    }
}
