using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace txtParser
{
    public static class StringHelper
    {
        /// <summary>
        /// Удаляет указанные символы из строки и возвращает чистую от них строку
        /// </summary>
        /// <param name="stringToChange">Строка для обработки</param>
        /// <param name="charsToDelete">Строка с символами для удаления</param>
        /// <returns>Чистая от указанных символов строка</returns>
        /// <exception cref="ArgumentNullException">Если на вход поданы пустые строки любая из 2х)</exception>
        public static string DeleteMultipleChars(string stringToChange, string charsToDelete)
        {
            if (string.IsNullOrWhiteSpace(stringToChange)) throw new ArgumentNullException("На вход метода DeleteMultipleChars подана пустая строка");
            if (string.IsNullOrEmpty(charsToDelete)) throw new ArgumentNullException("На вход метода DeleteMultipleChars не переданы символы для удаления");

            string result = stringToChange.Trim();
            foreach (char c in charsToDelete)
            {
                result = result.Replace(c.ToString(), "");
            }

            return result;
        }

        /// <summary>
        /// Удаляет указанные символы из строки и возвращает чистую от них строку. Делает всё то же самое, что и DeleteMultipleChars(),
        /// но оптимальнее с точки зрения расходования ресурсов
        /// </summary>
        /// <param name="stringToChange">Строка для обработки</param>
        /// <param name="charsToDelete">Строка с символами для удаления</param>
        /// <returns>Чистая от указанных символов строка</returns>
        /// <exception cref="ArgumentNullException">Если на вход поданы пустые строки любая из 2х</exception>
        public static string DeleteMultipleCharsOptimized(string stringToChange)
        {
            if (string.IsNullOrEmpty(stringToChange)) throw new ArgumentNullException("На вход метода DeleteMultipleChars подана пустая строка");

            StringBuilder result = new StringBuilder();

            foreach (char ch in stringToChange)
            {
                if (char.IsLetter(ch))
                    result.Append(ch);
                if (char.IsWhiteSpace(ch))
                    result.Append(" ");
            }

            return result.ToString().ToLower();
        }

    }
}
