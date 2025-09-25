using System;
using System.Linq;

namespace Utilities {
    /// <summary> Handles display functions like quote strings and comma separated lists </summary>
    public static class StringProcessing {
        /// <summary> Adds quotation marks to the input string </summary>
        /// <param name="toQuote">string to be quoted</param>
        /// <returns>input string with quotation marks surrounding it</returns>
        public static string QuoteString(string toQuote) => $"\"{toQuote}\"";
        
        /// <summary> Take string of comma separated list and parse into component parts </summary>
        /// <param name="commaList">string with comma separated list</param>
        /// <param name="parseFunc">function to parse individual items in list</param>
        /// <typeparam name="T">type of the parsed items</typeparam>
        /// <returns>array of items of type T</returns>
        public static T[] CommaSeparated<T>(string commaList, Func<string, T> parseFunc) =>
            (from i in commaList.Split(',') select parseFunc(i)).ToArray();
    }
}
