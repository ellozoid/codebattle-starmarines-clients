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
		private readonly string _player;
		private readonly List<ClientAction> _actions = new List<ClientAction>();

		private ClientWebSocket Socket { get; set; }
		private readonly string _path;

		private Thread _workThread;
		private Action _messageHandler;

		private void UpdateFunc()
		{
			Socket = new ClientWebSocket();
			Socket.ConnectAsync(new Uri(_path), CancellationToken.None).Wait();
			while (Socket.State == WebSocketState.Open)
			{
				var buffer = new ArraySegment<byte>(new byte[1024 * 4]);
				string message;
				using (var ms = new MemoryStream())
				{
					WebSocketReceiveResult result = null;
					do
					{
						result = Socket.ReceiveAsync(buffer, CancellationToken.None).Result;
						ms.Write(buffer.Array, buffer.Offset, result.Count);
					} while (!result.EndOfMessage);

					ms.Seek(0, SeekOrigin.Begin);

					using (var reader = new StreamReader(ms, Encoding.UTF8))
					{
						message = reader.ReadToEndAsync().Result;
					}
				}

				Snapshot = JsonConvert.DeserializeObject<GalaxySnapshot>(message);
				if (Snapshot != null)
					_messageHandler();
			}
		}

		public GameClientStarMarines(string server, string player, string token = "")
		{
			Snapshot = null;
			this._player = player;
			_path = "ws://" + server + "/galaxy" + "?token=" + token;
		}

		public void Run(Action messageHandler)
		{
			_messageHandler = messageHandler;
			_workThread = new Thread(this.UpdateFunc);
			_workThread.Start();
		}

		public void SendDrones(int from, int to, int drones)
		{
			_actions.Add(new ClientAction { Src = from, Dest = to, UnitCounts = drones });
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
			return Snapshot.Planets.Where(planet => planet.Neighbours.Any(p => p == planetId)).ToList();
		}

		public IEnumerable<PlanetInfo> GetMyPlanets()
		{
			return Snapshot.Planets.Where(planet => _player == planet.Owner).ToList();
		}

		public void Blank()
		{
			Send("");
		}

		public void SendMessage()
		{
			Send(JsonConvert.SerializeObject(new Command {Actions = _actions}));
			_actions.Clear();
		}

		private void Send(string msg)
		{
			Console.WriteLine("Sending: " + msg);
			Socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(msg)), WebSocketMessageType.Text, true,
				CancellationToken.None).Wait();
		}
	}
}
