using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StarMarines.Models;
using StarMarines.Strategies;

namespace StarMarines {
    //Контекст, использующий стратегию для решения поставленной задачи
    public class Context
    {
        
        private ClientWebSocket _clientWebSocket { get; set; }
        
        //Ссылка на интерфейс IStrategy
        //используется при переключении между конкретными реализациями
        //(проще говоря, это выбор конкретной стратегии)
        private IStrategy _strategy;
        private string _uri;
        private string _token;
        private string _botName;
        private bool _debug;
        public bool isActive { get => _clientWebSocket.State == WebSocketState.Open; }
        public string BotName { get => _botName; }
        public string Token {get => _token; }

        //Конструктор контекста
        //Инициализирует объект контекста заданной стратегией
        public Context(string botName, string server, string token, bool debug)
        {
            _uri = $"ws://{server}/galaxy?token={token}";
            _token = token;
            _botName = botName;
            _debug = debug;
        }

        //Метод для установки стратегии
        //Используется для смены стратегии во время выполнения программы
        public void SetStrategy(IStrategy strategy)
        {
            _strategy = strategy;
            _strategy.BotName = _botName;
        }

        public async Task StartConnectionAsync(Action<Screen> handleMessage)
        {
            // also check if connection was lost, that's probably why we get called multiple times.
            if (_clientWebSocket == null || _clientWebSocket.State != WebSocketState.Open)
            {
                // create a new web-socket so the next connect call works.
                _clientWebSocket?.Dispose();
                _clientWebSocket = new ClientWebSocket();
            }
            // don't do anything, we are already connected.
            else return;

            await _clientWebSocket.ConnectAsync(new Uri(_uri), CancellationToken.None).ConfigureAwait(false);
            await Receive(_clientWebSocket, async (message) =>
            {
                handleMessage(message);
            });
        }

        public async Task StopConnectionAsync()
        {
            if (_debug) Console.WriteLine("Connection is closed");
            await _clientWebSocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None).ConfigureAwait(false);
        }

        public async Task SendAsync(Command command)
        {
            if (command != null)
            {
                command.Token = _token;
                var commandString = JsonConvert.SerializeObject(command);
                if (_debug)
                {
                    Console.WriteLine("Ваша команда: " + commandString);
                }
                var commandBytes = Encoding.UTF8.GetBytes(commandString);
                await _clientWebSocket.SendAsync(new ArraySegment<byte>(commandBytes), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        public async Task Receive(Action<Screen> handleMessage){
            await Receive(_clientWebSocket, handleMessage);
        }

        private async Task Receive(ClientWebSocket clientWebSocket, Action<Screen> handleMessage)
        {
            while (_clientWebSocket.State == WebSocketState.Open)
            {
                ArraySegment<Byte> buffer = new ArraySegment<byte>(new Byte[1024 * 4]);
                string serializedMessage = null;
                WebSocketReceiveResult result = null;
                using (var ms = new MemoryStream())
                {
                    do
                    {
                        result = await clientWebSocket.ReceiveAsync(buffer, CancellationToken.None).ConfigureAwait(false);
                        ms.Write(buffer.Array, buffer.Offset, result.Count);
                    }
                    while (!result.EndOfMessage);

                    ms.Seek(0, SeekOrigin.Begin);

                    using (var reader = new StreamReader(ms, Encoding.UTF8))
                    {
                        serializedMessage = await reader.ReadToEndAsync().ConfigureAwait(false);
                    }
                }

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    if (_debug) Console.WriteLine("Входное сообщение: " + serializedMessage);
                    handleMessage(JsonConvert.DeserializeObject<Screen>(serializedMessage));
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    await StopConnectionAsync();
                    break;
                }
            }
        }

    }

}