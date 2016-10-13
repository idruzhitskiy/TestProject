using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Clusterizer.Clusterizers;
using Clusterizer.EntitiesReaders;
using Clusterizer.EntitiesWriters;
using Clusterizer.RemoteDatabases;

namespace Clusterizer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Count() < 1)
            {
                Console.WriteLine("Использование:");
                Console.WriteLine($"{AppDomain.CurrentDomain.FriendlyName} -f in_filename out_filename num_of_clusters");
                Console.WriteLine($"{AppDomain.CurrentDomain.FriendlyName} -db out_filename num_of_clusters");
                Console.WriteLine($"{AppDomain.CurrentDomain.FriendlyName} -add filename");
                Console.WriteLine($"{AppDomain.CurrentDomain.FriendlyName} -rm filename");
                Console.WriteLine($"{AppDomain.CurrentDomain.FriendlyName} -clr");
                Console.WriteLine("Для выхода нажмите любую клавишу...");
                Console.ReadKey();
                return;
            }

            // Инициализация DI
            IKernel kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            switch (args[0])
            {
                case "-f":
                    if (ArgsNumberValid(args,4))
                    {
                        var in_filename = args[1];
                        var out_filename = args[2];
                        var numOfClusters = Convert.ToInt32(args[3]);
                        kernel.Rebind<TextReader>().ToConstant(new StreamReader(in_filename));
                        kernel.Rebind<TextWriter>().ToConstant(new StreamWriter(out_filename, false));
                        var entitiesWriter = kernel.Get<IEntitiesWriter>();
                        var clusterizer = kernel.Get<IClusterizer>();
                        var entitiesReader = kernel.Get<IEntitiesReader>();

                        entitiesWriter.Write(clusterizer.Clusterize(entitiesReader.Entities, numOfClusters));
                    }
                    break;
                case "-db":
                    if (ArgsNumberValid(args, 3))
                    {
                        var out_filename = args[1];
                        var numOfClusters = Convert.ToInt32(args[2]);
                        kernel.Rebind<TextWriter>().ToConstant(new StreamWriter(out_filename, false));
                        var entitiesWriter = kernel.Get<IEntitiesWriter>();
                        var clusterizer = kernel.Get<IClusterizer>();
                        var remoteDatabase = kernel.Get<IRemoteDatabase>();

                        entitiesWriter.Write(clusterizer.Clusterize(remoteDatabase.FindAllEntitites(), numOfClusters));
                    }
                    break;
                case "-add":
                    if (ArgsNumberValid(args,2))
                    {
                        var in_filename = args[1];
                        kernel.Rebind<TextReader>().ToConstant(new StreamReader(in_filename));
                        var remoteDatabase = kernel.Get<IRemoteDatabase>();
                        var entitiesReader = kernel.Get<IEntitiesReader>();

                        foreach (var entity in entitiesReader.Entities)
                        {
                            remoteDatabase.AddEntity(entity);
                        }
                    }
                    break;
                case "-rm":
                    if (ArgsNumberValid(args, 2))
                    {
                        var in_filename = args[1];
                        kernel.Rebind<TextReader>().ToConstant(new StreamReader(in_filename));
                        var entitiesReader = kernel.Get<IEntitiesReader>();
                        var remoteDatabase = kernel.Get<IRemoteDatabase>();

                        foreach (var entity in entitiesReader.Entities)
                        {
                            remoteDatabase.RemoveEntity(remoteDatabase.FindEntity(entity.TextAttributes));
                        }
                    }
                    break;
                case "-clr":
                    if (ArgsNumberValid(args,1))
                    {
                        kernel.Get<IRemoteDatabase>().DropDatabase();
                    }
                    break;
                default:
                    Console.WriteLine("Команда не найдена");
                    break;
            }
        }

        static bool ArgsNumberValid(string[] args, int n)
        {
            var result = true;
            if (args.Count() != n)
            {
                Console.WriteLine("Ошибка количества аргументов");
                result = false;
            }
            return result;
        }
    }
}
