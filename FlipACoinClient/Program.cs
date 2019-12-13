using System;
using System.Threading.Tasks;
using Grpc.Core;
using Protobuf;

namespace FlipACoinClient
{
	class Program
	{
		static async Task Main(string[] args)
		{
			var channel = new Channel("127.0.0.1:8080", ChannelCredentials.Insecure);
			var client = new FlipACoin.FlipACoinClient(channel);

			var stream = client.Start(Metadata.Empty);
			while (true)
			{
				Console.WriteLine("head = 0; tail = 1; quit game = 2");
				var input = Console.ReadLine();
				if (string.Equals(input, "0"))
				{
					await stream.RequestStream.WriteAsync(new Coin {Head = true});
					await CheckWin(stream.ResponseStream, true);
				} else if (string.Equals(input, "1"))
				{
					await stream.RequestStream.WriteAsync(new Coin {Head = false});
					await CheckWin(stream.ResponseStream, false);
				} else if (string.Equals(input, "2"))
				{
					await stream.RequestStream.CompleteAsync();
					await channel.ShutdownAsync();
					Console.WriteLine("client closed");
					Console.ReadKey();
					break;
				}
				else
				{
					Console.WriteLine("wrong input");
				}
			}
		}

		private static async Task CheckWin(IAsyncStreamReader<Coin> stream, bool head)
		{
			if (await stream.MoveNext())
			{
				Console.WriteLine(stream.Current.Head == head ? "win" : "lost");
			}
		}

	}
}
