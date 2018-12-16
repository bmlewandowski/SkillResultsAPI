using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace SkillResultsAPI
{
    public class GetValue
    {
        /// <summary>
        /// Converts string to lowercase dashed value for exists comparison
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string Converted(string name)
        {
            //trim leading and trailing spaces
            string trimname = name.Trim();

            //replace spaces and multiple spaces with dash
            string dashname = Regex.Replace(trimname, @"\s+", "-").Replace(' ', '-');

            //covert to lowercase
            string value = dashname.ToLower();

            return value;

        }
    }
}