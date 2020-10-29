using System;
using System.CommandLine.Rendering.Views;
using System.Threading;
using System.Threading.Tasks;
using LiteNetLib;

static class Program
{
    static void Main()
    {

        var serverListener = new EventBasedNetListener();
        serverListener.ConnectionRequestEvent += (request) => { request.Accept(); };
        serverListener.DeliveryEvent += delegate { Console.WriteLine("Server: DeliveryEvent"); };
        serverListener.NetworkErrorEvent += delegate { Console.WriteLine("Server: NetworkErrorEvent"); };
        // serverListener.NetworkLatencyUpdateEvent += delegate { Console.WriteLine("Server: NetworkLatencyUpdateEvent"); };
        serverListener.NetworkReceiveEvent += delegate { Console.WriteLine("Server: NetworkReceiveEvent"); };
        serverListener.NetworkReceiveUnconnectedEvent += delegate { Console.WriteLine("Server: NetworkReceiveUnconnectedEvent"); };
        serverListener.PeerConnectedEvent += delegate { Console.WriteLine("Server: PeerConnectedEvent"); };
        serverListener.PeerDisconnectedEvent += delegate { Console.WriteLine("Server: PeerDisconnectedEvent"); };


        var clientListener = new EventBasedNetListener();
        clientListener.ConnectionRequestEvent += delegate { Console.WriteLine("Client: ConnectionRequestEvent"); };
        clientListener.DeliveryEvent += delegate { Console.WriteLine("Client: DeliveryEvent"); };
        clientListener.NetworkErrorEvent += delegate { Console.WriteLine("Client: NetworkErrorEvent"); };
        // clientListener.NetworkLatencyUpdateEvent += delegate { Console.WriteLine("Client: NetworkLatencyUpdateEvent"); };
        clientListener.NetworkReceiveEvent += delegate { Console.WriteLine("Client: NetworkReceiveEvent"); };
        clientListener.NetworkReceiveUnconnectedEvent += delegate { Console.WriteLine("Client: NetworkReceiveUnconnectedEvent"); };
        clientListener.PeerConnectedEvent += delegate { Console.WriteLine("Client: PeerConnectedEvent"); };
        clientListener.PeerDisconnectedEvent += delegate { Console.WriteLine("Client: PeerDisconnectedEvent"); };

        var server = new NetManager(serverListener);

        if (!server.Start(9050))
        {
            Console.WriteLine("NOPE");
            return;
        }
        Console.WriteLine("Server listening at port 9050...");

        var client = new NetManager(clientListener);

        if (!client.Start())
        {
            Console.WriteLine("NOPE");
            return;
        }
        Console.WriteLine($"Client listening at port {client.LocalPort}...");

        client.Connect("localhost", 9050, "");

        while (!Console.KeyAvailable)
        {
            client.PollEvents();
            server.PollEvents();
            Thread.Sleep(15);
        }
    }
}
