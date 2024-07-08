using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace txtParser
{
    public class DbHelper
    {
        string connString = "Host=localhost;Port=5432;Username=Chris;Password=2113;Database=TxtParser";

        NpgsqlDataSourceBuilder dataSourceBuilder;
        NpgsqlDataSource dataSource;
        NpgsqlConnection conn;

        public DbHelper()
        {
            dataSourceBuilder = new NpgsqlDataSourceBuilder(connString);
            dataSource = dataSourceBuilder.Build();

            conn = dataSource.OpenConnection();
        }

        /// <summary>
        /// Ищет слово в базе данных
        /// </summary>
        /// <returns>True, если найдено, инчаче - false</returns>
        public Int64 FindWordInDb(string wordToFind)
        {
            using (var cmd = new NpgsqlCommand($"SELECT \"Count\" FROM public.\"Word\" where \"Name\" = @p ;", conn))
            {
                cmd.Parameters.AddWithValue("p", wordToFind);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        try
                        {
                            //var count = int.Parse(reader.GetString(0));
                            Int64 count = reader.GetInt64(0);
                            return count;
                        }
                        catch
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Что то пошло не так при попытке получения данных с бд по слову {wordToFind}");
                            Console.ResetColor();
                            return 0;
                        }

                    }

                }
            }

            

            return 0;
        }

        public void AddNewToDB(string name, Int64 count)
        {
            //using (var cmd = new NpgsqlCommand("INSERT INTO public.\"Word\"(\"Id\", \"Name\", \"Count\") VALUES (?, ?, ?);", conn))
            using (var cmd = new NpgsqlCommand("INSERT INTO public.\"Word\" (\"Name\", \"Count\") VALUES (@p1, @p2);", conn))
            {
                cmd.Parameters.AddWithValue("p1", name);
                cmd.Parameters.AddWithValue("p2", count);
                cmd.ExecuteNonQuery();
            }


        }
        
        /// <summary>
        /// Обновление данных в базе
        /// </summary>
        /// <param name="name">Название слова</param>
        /// <param name="count">Сколько повторений нужно записать в DB (Внимание! Не добавить, а сразу записать!)</param>
        public void UpdateDataOnDb(string name, Int64 count)
        {
            //using (var cmd = new NpgsqlCommand($"SELECT \"Count\" FROM public.\"Word\" where \"Name\" = @p;", conn))
            using (var cmd = new NpgsqlCommand($"UPDATE public.\"Word\" SET \"Count\"=13 where \"Name\" = @p;", conn))
            {
                cmd.Parameters.AddWithValue("p", name);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
