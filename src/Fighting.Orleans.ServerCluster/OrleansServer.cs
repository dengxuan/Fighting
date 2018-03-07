using Fighting.Orleans.Abstractions;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Fighting.Orleans.ServerCluster
{
    internal class OrleansServer<TCluster> where TCluster : IOrleansCluster
    {
        internal Task StartAsync()
        {
            // define the cluster configuration
            var siloPort = 11111;
            int gatewayPort = 30000;
            var siloAddress = IPAddress.Loopback;
            var builder = new SiloHostBuilder()
                .Configure(options => options.ClusterId = "helloworldcluster")
                .UseDevelopmentClustering(options => options.PrimarySiloEndpoint = new IPEndPoint(siloAddress, siloPort))
                .ConfigureEndpoints(siloAddress, siloPort, gatewayPort)
                .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(TCluster).Assembly).WithReferences());
                //.ConfigureLogging(logging => logging.AddConsole());

            var host = builder.Build();
           return host.StartAsync();
        }
    }
}
