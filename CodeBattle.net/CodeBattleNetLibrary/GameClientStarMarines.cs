using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;

namespace CodeBattleNetLibrary
{
	public class GameClientStarMarines
	{
		public GalaxySnapshot Snapshot { get; private set; }
		private readonly string player;
		private List<ClientAction> actions = new List<ClientAction>();

		private ClientWebSocket Socket { get; set; }
		private string path;

		private bool is_running;
		private Thread work_thread;
		private Action message_handler;

		public void UpdateFunc()
		{
			Socket = new ClientWebSocket();
			Socket.ConnectAsync(new Uri(path), CancellationToken.None).Wait();
			while (is_running)
			{
				ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024 * 4]);
				string message = null;
				WebSocketReceiveResult result = null;
				using (var ms = new MemoryStream())
				{
					do
					{
						result = Socket.ReceiveAsync(buffer, CancellationToken.None).Result;
						ms.Write(buffer.Array, buffer.Offset, result.Count);
					}
					while (!result.EndOfMessage);

					ms.Seek(0, SeekOrigin.Begin);

					using (var reader = new StreamReader(ms, Encoding.UTF8))
					{
						message = reader.ReadToEndAsync().Result;
					}
				}
				Snapshot = JsonConvert.DeserializeObject<GalaxySnapshot>(message);
				if (Snapshot != null)
					message_handler();
			}
		}

		public GameClientStarMarines(string _server, string player, string token = "")
		{
			Snapshot = null;
			this.player = player;
			path = "ws://" + _server + "/galaxy" + "?token=" + token;
			is_running = false;
		}

		public void Run(Action _message_handler)
		{
			message_handler = _message_handler;
			is_running = true;
			work_thread = new Thread(this.UpdateFunc);
			work_thread.Start();
		}

		public void SendDrones(int from, int to, int drones)
		{
			ClientAction action = new ClientAction { Src = from, Dest = to, UnitCounts = drones };
			actions.Add(action);
		}

		public List<string> GetErrors()
		{
			return Snapshot.Errors.ToList();
		}

		public PlanetInfo GetPlanetById(int id)
		{
			foreach (var planet in Snapshot.Planets)
			{
				if (planet.Id == id)
				{
					return planet;
				}
			}
			return null;
		}

		public List<PlanetInfo> GetNeighbours(int planetId)
		{
			List<PlanetInfo> neighbours = new List<PlanetInfo>();
			foreach (var planet in Snapshot.Planets)
			{
				if (planet.Neighbours.Any(p => p == planetId))
				{
					neighbours.Add(planet);
				}
			}
			return neighbours;
		}

		public List<PlanetInfo> GetMyPlanets()
		{
			List<PlanetInfo> myPlanets = new List<PlanetInfo>();
			foreach (var planet in Snapshot.Planets)
			{
				if (player == planet.Owner)
				{
					myPlanets.Add(planet);
				}
			}
			return myPlanets;
		}

		public void Blank()
		{
			Send("");
		}

		public void SendMessage()
		{
			string commands =
				"{ \"actions\":[" +
				string.Join(",", actions.Select(a => "{\"from\":" + a.Src + ", \"to\":" + a.Dest + ", \"unitsCount\":" + a.UnitCounts + "}")) +
				"]}";
			Send(commands);
		}

		private void Send(string msg)
		{
			Console.WriteLine("Sending: " + msg);
			Socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(msg)), WebSocketMessageType.Text, true, CancellationToken.None).Wait();
		}
	};
}
