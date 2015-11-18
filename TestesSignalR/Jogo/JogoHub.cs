using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Jogo
{
    /// <summary>
    /// Hub de controle do jogo
    /// </summary>
    public class GameHub : Hub
    {
        private static Broadcaster _broadcaster;

        public GameHub()
        {
            // Definição do controlador de broadcast.
            if (_broadcaster == null)
                _broadcaster = new Broadcaster();
        }

        public override System.Threading.Tasks.Task OnConnected()
        {
            // Inclusão de jogador ao conectar.
            _broadcaster.AddCharacter(Context.ConnectionId);
            return base.OnConnected();
        }

        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            // retirada de jogador ao fechar o navegador ou ir para outro site.
            _broadcaster.RemoveCharacter(Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }

        public void UpdatePosition(Character position)
        {
            // informando ao controlador de broadcast uma atualização de posição
            _broadcaster.UpdatePosition(Context.ConnectionId, position);
        }
    }

    public class Broadcaster
    {
        // intervalo de atualização dos jogadores conectados.
        private readonly TimeSpan _interval = TimeSpan.FromMilliseconds(40);
        private IDictionary<string, Character> _characters = new Dictionary<string, Character>();
        private readonly IHubContext _hubContext;
        private Timer _broadcastLoop;
        private int _caught = 0;

        public Broadcaster()
        {
            // Obtendo o contexto do hub.
            _hubContext = GlobalHost.ConnectionManager.GetHubContext<GameHub>();

            // ativando atualizações periódicas dos jogadores.
            _broadcastLoop = new Timer(InformPosition, null, _interval, _interval);
        }

        public void AddCharacter(string connId)
        {
            if (_characters.Count >= 2)
            {
                // Recusa de usuário caso já existam 2 jogadores ativos
                _hubContext.Clients.Client(connId).OverLimit();
                return;
            }

            var _pos = new Character
            {
                Type = _characters.Any(c => c.Value.Type != "hero") ? "hero" : "monster",
                x = 0,
                y = 0
            };

            _characters.Add(connId, _pos);
            // Informando ao cliente o personagem que controlará.
            _hubContext.Clients.Client(connId).DefineCharacter(_pos.Type, _characters.Count == 2);
            // Exibir inimigo caso tenha conectado.
            if (_characters.Count == 2)
                _hubContext.Clients.AllExcept(connId).ShowEnemy();
            _caught = 0;
        }

        public void RemoveCharacter(string connId)
        {
            if (_characters.ContainsKey(connId))
            {
                _characters.Remove(connId);
                _caught = 0;
            }
        }

        public void UpdatePosition(string connId, Character position)
        {
            if (!_characters.ContainsKey(connId))
                return;

            var character = _characters[connId];
            character.x = position.x;
            character.y = position.y;
            character.Updated = true;

            if (_characters.Count == 2)
                VerifyCatch();
        }

        /// <summary>
        /// Verificação de captura do monstro.
        /// </summary>
        private void VerifyCatch()
        {
            var first = _characters.Values.First();
            var last = _characters.Values.Last();

            if (Math.Abs(first.x - last.x) <= 32 && Math.Abs(first.y - last.y) <= 32)
            {
                _caught++;
                _hubContext.Clients.All.Caught(_caught);
            }
        }

        public void InformPosition(object state)
        {
            if (_characters.Count == 2)
                foreach (var c in _characters)
                    if (c.Value.Updated)
                    {
                        _hubContext.Clients.AllExcept(c.Key).informPosition(c.Value);
                        c.Value.Updated = false;
                    }
        }
    }

    public class Character
    {
        [JsonIgnore]
        public string Type { get; set; }
        [JsonIgnore]
        public bool Updated { get; set; }

        public decimal x { get; set; }
        public decimal y { get; set; }
    }
}