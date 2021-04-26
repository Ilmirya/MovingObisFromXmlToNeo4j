using Neo4jClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace MovingObisFromXmlToNeo4j
{
    public class Neo4j
    {
        private readonly GraphClient _client;
        private static Neo4j _instance = new Neo4j();
        public static Neo4j Instance => _instance;

        private Neo4j()
        {
            string url = "http://neo4j:matrix@localhost:7474/db/data";
            _client = new GraphClient(new Uri(url));
            _client.ConnectAsync().Wait();
        }

        public bool AddNode(string obis, string description, string type, dynamic body)
        {
            var query = _client.Cypher
                        .Merge($"(t:{type} {{obis: {{obis}}, description: {{description}} }})").Set("t = {body}")
                        .WithParams(new { obis = obis, description = description, body = body })
                        .Return(t => t.As<string>());
            return query.ResultsAsync.Result != null;
        }
    }
}
