using System;
using Grpc.Core;
using Protobuf;

namespace FlipACoinServer
{
	class Program
	{
		public static void Main(string[] args)
		{
			var server = new Server
						{
							Services = {FlipACoin.BindService(new FlipACoinImpl())},
							Ports = {new ServerPort("localhost", 8080, ServerCredentials.Insecure)}
						};
			server.Start();

			Console.WriteLine("Press any key to stop the server...");
			Console.ReadKey();

			server.ShutdownAsync().Wait();
		}
	}
}
