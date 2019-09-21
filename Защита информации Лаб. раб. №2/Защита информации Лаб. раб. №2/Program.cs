using System;
using System.Collections.Generic;

namespace Защита_информации_Лаб.раб._2
{
    // Реализация политики информационной безопасности в компьютерных системах
    // на примере дискреционной модели.
    public class User
    {
        static Random rand = new Random();
        public string name; // Имя пользователя
        public string[] objectsArray = new string[5]; //Список объектов доступа
        public string[] rightsArray = {"Полный запрет", "Запись", "Запись, Передача прав", "Чтение", "Чтение, Передача прав", "Чтение, Запись", "Полный доступ"}; //Список прав доступа
                   
        public User()
        {
            for (int i = 0; i < objectsArray.Length; i++) // Выбор случайных прав доступа
            {
                int a = rand.Next(rightsArray.Length);
                this.objectsArray[i] = rightsArray[a];
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.White;
            Console.Clear();
            List<User> userName = new List<User>();
            userName.Add(new User() { name = "Administrator" });
            for (int i = 0; i < userName[0].objectsArray.Length; i++) //Полный доступ для администратора
               userName[0].objectsArray[i] = userName[0].rightsArray[6];
            userName.Add(new User() { name = "Boris" }); 
            userName.Add(new User() { name = "Ivan" });
            userName.Add(new User() { name = "Oleg" });
            userName.Add(new User() { name = "Sergey" });
            userName.Add(new User() { name = "Pavel" });
            bool enter = true;
            while (enter)
            {
                Console.Write("User: ");
                string userID = Console.ReadLine();
                bool startPoint = false;
                int usersNameIndex = 0;
                for (int i = 0; i < userName.Count; i++) //Идентификация пользователя
                    {
                        if (userID == userName[i].name)
                        {
                            Console.WriteLine("Идентификация прошла успешно, добро пожаловать в систему");
                            startPoint = true;
                            usersNameIndex = i;
                        }
                        else continue;
                    }
                if (startPoint)
                {
                    Console.WriteLine("Перечень Ваших прав:");
                    for (int i = 0; i < userName[usersNameIndex].objectsArray.Length; i++)
                    {
                        Console.WriteLine("Объект № " + i + " " + userName[usersNameIndex].objectsArray[i]);
                    }

                    bool switchStartPoint = true;
                    while (switchStartPoint)
                    {
                        Console.Write("Жду ваших указаний > "); // Взаимодействие с объектом (чтение, запись, передача прав, выход) 
                        string command = Console.ReadLine();
                        switch (command)
                        {
                            case "read":
                                Console.Write("Над каким объектом производится операция? ");
                                int r = Convert.ToInt32(Console.ReadLine());
                                ReadMethod(userName[usersNameIndex], userName[usersNameIndex].objectsArray[r], r);
                                continue;
                            case "write":
                                Console.Write("Над каким объектом производится операция? ");
                                int w = Convert.ToInt32(Console.ReadLine());
                                WriteMethod(userName[usersNameIndex], userName[usersNameIndex].objectsArray[w], w);
                                continue;
                            case "grant":
                                Console.Write("Право на какой объект передается? ");
                                int g = Convert.ToInt32(Console.ReadLine());
                                GrantMethod(userName, userName[usersNameIndex], userName[usersNameIndex].objectsArray[g], g);
                                continue;
                            case "quit":
                                Console.WriteLine("Работа пользователя " + userName[usersNameIndex].name + " завершена. До свидания.");
                                switchStartPoint = false;
                                break;
                            default:
                                Console.WriteLine("Введена неверная команда!");
                                continue;
                        }
                    }
                }
                else Console.WriteLine("Задан неверный идентификатор доступа. Повторите попытку");
            }
            Console.ReadKey();
        }
        public static void ReadMethod(User ob, string s, int r)
        {
            if (s == "Чтение" || s == "Чтение, Передача прав" || s == "Чтение, Запись" || s == "Полный доступ")
            {
                Console.WriteLine("Операция прошла успешно");
            }
            else Console.WriteLine("Отказ в выполнении операции. У Вас нет прав для ее осуществления");
        }

        public static void WriteMethod(User ob, string s, int w)
        {
            if (s == "Запись" || s == "Запись, Передача прав" || s == "Чтение, Запись" || s == "Полный доступ")
            {
                Console.WriteLine("Операция прошла успешно");
            }
            else Console.WriteLine("Отказ в выполнении операции. У Вас нет прав для ее осуществления");
        }

        public static void GrantMethod(List<User> users, User ob, string s, int g)
        {
            if (s == "Чтение, Передача прав")
            {
                Console.Write("Какое право передается? ");
                string str = "";
                while (str != null)
                {
                    string command = Console.ReadLine();
                    switch (command)
                    {
                        case "read":
                            Console.WriteLine("Какому пользователю передается право? ");
                            str = Console.ReadLine();
                            for (int i = 0; i < users.Count; i++)
                            {
                                if (users[i].name == str)
                                {
                                    users[i].objectsArray[g] = "Чтение";
                                    Console.WriteLine("Операция прошла успешно");
                                }
                            }
                            str = null;
                            break;
                        case "write":
                            Console.WriteLine("Отказ в выполнении операции. У Вас нет прав для ее осуществления");
                            str = null;
                            break;
                        default:
                            Console.WriteLine("Введена неверная команда!");
                            continue;
                    }
                }
            }

            else if (s == "Запись, Передача прав")
            {
                Console.Write("Какое право передается? ");
                string str = "";
                while (str != null)
                {
                    string command = Console.ReadLine();
                    switch (command)
                    {
                        case "read":
                            Console.WriteLine("Отказ в выполнении операции. У Вас нет прав для ее осуществления");
                            str = null;
                            break;
                        case "write":
                            Console.WriteLine("Какому пользователю передается право? ");
                            str = Console.ReadLine();
                            for (int i = 0; i < users.Count; i++)
                            {
                                if (users[i].name == str)
                                {
                                    users[i].objectsArray[g] = "Запись";
                                    Console.WriteLine("Операция прошла успешно");
                                }
                            }
                            str = null;
                            break;
                        default:
                            Console.WriteLine("Введена неверная команда!");
                            continue;
                    }
                }
            }

            else if (s == "Полный доступ")
            {
                Console.Write("Какое право передается? ");
                string str = "";
                while (str != null)
                {
                    string command = Console.ReadLine();
                    switch (command)
                    {
                        case "read":
                            Console.WriteLine("Какому пользователю передается право? ");
                            str = Console.ReadLine();
                            for (int i = 0; i < users.Count; i++)
                            {
                                if (users[i].name == str)
                                {
                                    users[i].objectsArray[g] = "Чтение";
                                }
                            }
                            str = null;
                            break;
                        case "write":
                            Console.WriteLine("Какому пользователю передается право? ");
                            str = Console.ReadLine();
                            for (int i = 0; i < users.Count; i++)
                            {
                                if (users[i].name == str)
                                {
                                    users[i].objectsArray[g] = "Запись";
                                }
                            }
                            str = null;
                            break;
                        default:
                            Console.WriteLine("Введена неверная команда!");
                            continue;
                    }
                }
                Console.WriteLine("Операция прошла успешно");
            }
            else Console.WriteLine("Отказ в выполнении операции. У Вас нет прав для ее осуществления");
        }
    }
}
