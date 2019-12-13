using System;
using System.Threading.Tasks;
using Grpc.Core;
using Protobuf;

namespace FlipACoinServer
{
	public class FlipACoinImpl : FlipACoin.FlipACoinBase
	{
		public override async Task Start(IAsyncStreamReader<Coin> requestStream, IServerStreamWriter<Coin> responseStream, ServerCallContext context)
		{
			var rand = new Random();
			while (await requestStream.MoveNext())
			{
				var coin = requestStream.Current;
				var head = rand.Next(0, 2) == 0;

				Console.WriteLine(coin.Head == head ? "win" : "lost");

				await responseStream.WriteAsync(new Coin {Head = head});
			}
			Console.WriteLine("closed");
		}
	}
}
