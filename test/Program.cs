﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SLCMC.Authentication;
using SLCMC.GameFile.Version;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("1.Authentication");
                Console.WriteLine("2.Version");

                switch (Console.ReadKey().KeyChar)
                {
                    case '1':
                        Console.WriteLine();
                        AuthenticationTest();
                        break;
                    case '2':
                        Console.WriteLine();
                        VersionTest();
                        break;
                    default:
                        return;
                }
                Console.WriteLine("==================================================");
            }
                
        }

        static void AuthenticationTest()
        {
            Console.WriteLine("1.OfflineAuthenticator");
            Console.WriteLine("2.YggdrasilAuthenticator");

            switch (Console.ReadKey().KeyChar)
            {
                case '1':
                    Console.WriteLine();
                    OfflineAuthenticatorTest();
                    break;
                case '2':
                    Console.WriteLine();
                    YggdrasilAuthenticatorTest();
                    break;
                default:
                    return;
            }
        }

        static void OfflineAuthenticatorTest()
        {
            Console.Write("Input Name:");
            OfflineAuthenticator auth = new OfflineAuthenticator(Console.ReadLine());

            Console.WriteLine("Name:" + auth.Auth().Profile.Name);
            Console.WriteLine("Id:" + auth.Auth().Profile.Id.ToString("N"));
            Console.WriteLine("AccessToken:" + auth.Auth().AccessToken.ToString("N"));
            Console.WriteLine("Properties:");
            foreach(KeyValuePair<string, string> property in auth.Auth().Properties)
                Console.WriteLine("    " + property.Key + ":" + property.Value);
            Console.WriteLine("Type:" + auth.Auth().Type);
        }

        static void YggdrasilAuthenticatorTest()
        {
            Console.WriteLine("1.Authenticate");
            Console.WriteLine("2.Refresh");

            YggdrasilAuthenticator auth = new YggdrasilAuthenticator();
            string a,b;

            switch (Console.ReadKey().KeyChar)
            {
                case '1':
                    Console.WriteLine();
                    Console.Write("Input Username:");
                    a = Console.ReadLine();
                    Console.Write("Input Password:");
                    b = Console.ReadLine();
                    auth.Authenticate(a, b, Guid.NewGuid(), true);

                    Console.WriteLine("Name:" + auth.Auth().Profile.Name);
                    Console.WriteLine("Id:" + auth.Auth().Profile.Id.ToString("N"));
                    Console.WriteLine("AccessToken:" + auth.Auth().AccessToken.ToString("N"));
                    Console.WriteLine("Properties:");
                    foreach (KeyValuePair<string, string> property in auth.Auth().Properties)
                        Console.WriteLine("    " + property.Key + ":" + property.Value);
                    Console.WriteLine("Type:" + auth.Auth().Type);
                    break;
                case '2':
                    Console.WriteLine();
                    Console.Write("Input Username:");
                    a = Console.ReadLine();
                    Console.Write("Input Password:");
                    b = Console.ReadLine();
                    auth.Authenticate(a, b, Guid.NewGuid(), true);

                    Console.WriteLine("AuthenticateData:");

                    Console.WriteLine("Name:" + auth.Auth().Profile.Name);
                    Console.WriteLine("Id:" + auth.Auth().Profile.Id.ToString("N"));
                    Console.WriteLine("AccessToken:" + auth.Auth().AccessToken.ToString("N"));
                    Console.WriteLine("Properties:");
                    foreach (KeyValuePair<string, string> property in auth.Auth().Properties)
                        Console.WriteLine("    " + property.Key + ":" + property.Value);
                    Console.WriteLine("Type:" + auth.Auth().Type);
                    Console.WriteLine();

                    auth.Refresh(auth.AccessToken, auth.ClientToken, true);

                    Console.WriteLine("RefreshData:");
                    Console.WriteLine("Name:" + auth.Auth().Profile.Name);
                    Console.WriteLine("Id:" + auth.Auth().Profile.Id.ToString("N"));
                    Console.WriteLine("AccessToken:" + auth.Auth().AccessToken.ToString("N"));
                    Console.WriteLine("Properties:");
                    foreach (KeyValuePair<string, string> property in auth.Auth().Properties)
                        Console.WriteLine("    " + property.Key + ":" + property.Value);
                    Console.WriteLine("Type:" + auth.Auth().Type);

                    break;
                default:
                    return;
            }
        }

        static void VersionTest()
        {
            Console.Write("Input Json File Path:");
            string path = Console.ReadLine();

            VersionData data = VersionData.Parse(JObject.Parse(File.ReadAllText(path)));

            Console.WriteLine(data.Assets);
            foreach (var data2 in data.Libraries)
            {
                    Console.WriteLine(data2.GetFileInfo().Url);
            }
                
            Console.WriteLine(data.Id);
            Console.WriteLine(data.InheritsFrom);
            Console.WriteLine(data.Jar);
        }
    }
}
