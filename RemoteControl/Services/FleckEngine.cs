﻿using Fleck;
using RemoteControl.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace RemoteControl.Services
{
    class FleckEngine : IRemoteControlEngine
    {
        private WebSocketServer _server;
        private IWebSocketConnection _socket;
        public event EventHandler<RemoteControlOnMessageArgs> OnMessage;

        public void Connect()
        {
            _server = new WebSocketServer("ws://0.0.0.0:8181");
            _server.Start(Configure);
        }

        public void Configure(IWebSocketConnection socket)
        {
            _socket = socket;
            _socket.OnOpen = () => {
                Trace.WriteLine("Connected");
            };
            _socket.OnClose = () => {
                Trace.WriteLine("Disconnected");
            };
            _socket.OnMessage = HandlerMessage;
        }

        private void HandlerMessage(string message)
        {
            RemoteControlOnMessageArgs args = new RemoteControlOnMessageArgs();
            args.message = message;
            OnMessage(this, args);
        }
    }
}
