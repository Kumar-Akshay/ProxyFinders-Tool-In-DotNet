using FindProxyTool.AuthGG;
using System;
using System.Diagnostics;
using System.Windows;

namespace FindProxyTool
{
    class Program
    {
        static void Main(string[] args)
        {

            var Appname = "MultaWare";
            var AID = "316212";
            var ProgramSecret = "Ug1rd9JTTZj4EuxkWD5QP9nYUc4bprhj7KN";
            var Version = "1.0";

            //This connects your file to the Auth.GG API, and sends back your application settings and such
            OnProgramStart.Initialize(Appname, AID, ProgramSecret, Version);
            var option = MainMenu();
            if (option == "1")
            {
                if (!ApplicationSettings.Register)
                {
                    //Register is disabled in application settings, will show a messagebox that it is not enabled
                    System.Windows.MessageBox.Show("Register is not enabled, please try again later!", OnProgramStart.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                    Process.GetCurrentProcess().Kill(); //closes the application
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine("Username:");
                    string username = Console.ReadLine();
                    Console.WriteLine("Password:");
                    string password = Console.ReadLine();
                    Console.WriteLine("Email:");
                    string email = Console.ReadLine();
                    Console.WriteLine("License:");
                    string license = Console.ReadLine();
                    if (API.Register(username, password, email, license))
                    {
                        System.Windows.MessageBox.Show("You have successfully registered!", OnProgramStart.Name, MessageBoxButton.OK, MessageBoxImage.Information);
                        // Do code of what you want after successful register here!
                        Console.ReadKey();
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("You have not registered!", OnProgramStart.Name, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            else if (option == "2")
            {
                if (!ApplicationSettings.Login)
                {
                    //Register is disabled in application settings, will show a messagebox that it is not enabled
                    System.Windows.MessageBox.Show("Login is not enabled, please try again later!", OnProgramStart.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                    Process.GetCurrentProcess().Kill(); //closes the application
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine("Username:");
                    string username = Console.ReadLine();
                    Console.WriteLine("Password:");
                    string password = Console.ReadLine();
                    if (API.Login(username, password))
                    {
                        System.Windows.MessageBox.Show("You have successfully logged in!", OnProgramStart.Name, MessageBoxButton.OK, MessageBoxImage.Information);
                        Console.Clear();
                        // Success login stuff goes here
                        Console.ForegroundColor = ConsoleColor.White;
                        API.Log(username, "Logged in!"); //Logs this action to your web-panel, you can do this anywhere and for anything!
                        Console.WriteLine("***************************************************");
                        Console.WriteLine("All user information:");
                        Console.WriteLine("***************************************************");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"User ID -> {User.ID}");
                        Console.WriteLine($"Username -> {User.Username}");
                        Console.WriteLine($"Password -> {User.Password}");
                        Console.WriteLine($"Email -> {User.Email}");
                        Console.WriteLine($"HWID -> {User.HWID}");
                        Console.WriteLine($"User Variable -> {User.UserVariable}");
                        Console.WriteLine($"User Rank -> {User.Rank}");
                        Console.WriteLine($"User IP -> {User.IP}");
                        Console.WriteLine($"Expiry -> {User.Expiry}");
                        Console.WriteLine($"Last Login -> {User.LastLogin}");
                        Console.WriteLine($"Register Date -> {User.RegisterDate}");
                        Console.WriteLine($"Variable -> {App.GrabVariable("Ug1rd9JTTZj4EuxkWD5QP9nYUc4bprhj7KN")}"); // Replace put variable secret here with the secret of the variable in your panel - https://i.imgur.com/v3q2a6e.png
                        Console.WriteLine("***************************************************");
                        Console.WriteLine("***************************************************");
                        Console.WriteLine("\n\n");
                        var proxy = new ProxyFinder();
                        proxy.FindProxyTool();
                        Console.WriteLine("***************************************************");
                        Console.WriteLine("***************************************************");
                        Console.ReadKey();
                    }
                }
            }
            else if (option == "3")
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("Username:");
                string username = Console.ReadLine();
                Console.WriteLine("Password:");
                string password = Console.ReadLine();
                Console.WriteLine("License Token:");
                string token = Console.ReadLine();
                if (API.ExtendSubscription(username, password, token))
                {
                    System.Windows.MessageBox.Show("You have successfully extended your subscription!", OnProgramStart.Name, MessageBoxButton.OK, MessageBoxImage.Information);
                    // Do code of what you want after successful extend here!
                    Console.ReadKey();
                }
            }
        }

        public static string MainMenu()
        {
            PrintLogo();
            Console.WriteLine("[1] Register");
            Console.WriteLine("[2] Login");
            Console.WriteLine("[3] Extend Subscription");
            while (true)
            {
                Console.Write("\nEnter your choice: ");
                string option = Console.ReadLine();
                if (option == "1" || option == "2" || option == "3")
                {
                    return option;
                }
                Console.WriteLine("\nInvaild Choice, Try again");
            }
        }
    
        public static void PrintLogo()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine();
            string Logo = @"
  _____   _____    ____ __   ____     __  ______  _____  _   _  _____   ______  _____  
 |  __ \ |  __ \  / __ \\ \ / /\ \   / / |  ____||_   _|| \ | ||  __ \ |  ____||  __ \ 
 | |__) || |__) || |  | |\ V /  \ \_/ /  | |__     | |  |  \| || |  | || |__   | |__) |
 |  ___/ |  _  / | |  | | > <    \   /   |  __|    | |  | . ` || |  | ||  __|  |  _  / 
 | |     | | \ \ | |__| |/ . \    | |    | |      _| |_ | |\  || |__| || |____ | | \ \ 
 |_|     |_|  \_\ \____//_/ \_\   |_|    |_|     |_____||_| \_||_____/ |______||_|  \_\
                                                                                       ";
            Console.WriteLine(Logo);
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
        }

    }
}
