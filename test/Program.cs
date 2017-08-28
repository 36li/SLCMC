using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SLCMC.Authentication;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("1.Authentication");

                switch (Console.ReadKey().KeyChar)
                {
                    case '1':
                        Console.WriteLine();
                        AuthenticationTest();
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

            Console.WriteLine("Name:" + auth.Auth().Name);
            Console.WriteLine("Id:" + auth.Auth().Id.ToString("N"));
            Console.WriteLine("AccessToken:" + auth.Auth().AccessToken.ToString("N"));
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

                    Console.WriteLine("Name:" + auth.Auth().Name);
                    Console.WriteLine("Id:" + auth.Auth().Id.ToString("N"));
                    Console.WriteLine("AccessToken:" + auth.Auth().AccessToken.ToString("N"));
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

                    Console.WriteLine("AccessToken:" + auth.AccessToken);
                    Console.WriteLine("ClientToken:" + auth.ClientToken);
                    Console.WriteLine("Name:" + auth.Auth().Name);
                    Console.WriteLine("Id:" + auth.Auth().Id.ToString("N"));
                    Console.WriteLine("Type:" + auth.Auth().Type);
                    Console.WriteLine();

                    auth.Refresh(auth.AccessToken, auth.ClientToken, true);

                    Console.WriteLine("RefreshData:");
                    Console.WriteLine("AccessToken:" + auth.AccessToken);
                    Console.WriteLine("ClientToken:" + auth.ClientToken);
                    Console.WriteLine("Name:" + auth.Auth().Name);
                    Console.WriteLine("Id:" + auth.Auth().Id.ToString("N"));
                    Console.WriteLine("Type:" + auth.Auth().Type);

                    break;
                default:
                    return;
            }
        }
    }
}
