using System.Text;

namespace txtParser
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите путь к файлу ниже\n");
            string pathToFile = Console.ReadLine();

            if (String.IsNullOrWhiteSpace(pathToFile))
            {
                Console.WriteLine("Данные не были введены.\nПрограмма будет закрыта");
                return;
            }

            if (!File.Exists(pathToFile))
            {
                Console.WriteLine("Указанный файл не существует. Проверьте правильность ввода\nТак же рекомендуем использовать функцию вставки");
                // Славься, о великий Ctrl+V!
                return;
            }

            DbHelper dbHelper = new DbHelper();

            using (StreamReader reader = new StreamReader(pathToFile))
            {
                var encoding = reader.CurrentEncoding;
                string encode = encoding.BodyName; // BodyName = "utf-8"
                if (encode != "utf-8")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Вы пытаетесь открыть файл в кодировке, отличной от utf-8.\nПрограмма будет закрыта");
                    Console.ResetColor();
                }

                string line;
                string charToRemove = "\a1234567890-=+_[]{};':\"/.,'>?|-_=+!@#$%^&*()–’_+~№*&";

                Dictionary<string, int> wordsDict = new Dictionary<string, int>();
                

                while ((line = reader.ReadLine()) != null)
                {
                    if (String.IsNullOrWhiteSpace(line)) continue;


                    string clearLine = StringHelper.DeleteMultipleCharsOptimized(line);

                    //Console.WriteLine(clearLine);
                    //System.Threading.Thread.Sleep(100);

                    var wordArr = clearLine.Split(" ");

                    foreach (var word in wordArr.Where(x => x.Length >= 3 && x.Length <= 20))
                    {
                        // Если элемент есть в словаре - пишем кол-во совпадений
                        if (wordsDict.ContainsKey(word))
                        {
                            wordsDict[word]++;
                        }
                        else
                        {
                            wordsDict.Add(word, 1);
                        }

                    }

                }

                foreach (var item in wordsDict.Where(x => x.Value >= 4))
                {
                    // Если слово есть в бд
                    Int64 count = dbHelper.FindWordInDb(item.Key);
                    if (count > 0)
                    {
                        Int64 newCount = count + item.Value;
                        dbHelper.UpdateDataOnDb(item.Key, newCount);
                    }
                    else
                    {
                        dbHelper.AddNewToDB(item.Key, item.Value);
                    }
                }

                Console.WriteLine(line);
            }


            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Обработка завершена");
            Console.ResetColor();
        }
    }
}
