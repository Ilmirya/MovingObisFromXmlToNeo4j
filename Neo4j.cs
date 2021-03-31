using Neo4jClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace MovingObisFromXmlToNeo4j
{
    class Neo4j
    {
        private readonly GraphClient _client;
        private Neo4j _instance;
        private Neo4j()
        {
            string url = "http://neo4j:matrix@localhost:7474/db/data";
            _client = new GraphClient(new Uri(url));
            _client.ConnectAsync().Wait();
        }

        public Neo4j Instance()
        {
            if (_instance == null) _instance = new Neo4j();
            return _instance;
        }

        public bool AddNode(Guid id, string type, dynamic body)
        {
            var query = _client.Cypher
                        .Merge($"(t:{type} {{id: {{id}} }})").Set("t = {body}")
                        .WithParams(new { id = id, body = body })
                        .Return(t => t.As<string>());
            return query.ResultsAsync.Result != null;
        }
    }
    public static class Extantion
    {
        public static dynamic ToDynamic(this string result)
        {
            return JsonConvert.DeserializeObject<ExpandoObject>(result);
        }
        public static IEnumerable<dynamic> ToDynamic(this IEnumerable<string> nodes)
        {
            foreach (var node in nodes)
            {
                yield return node.ToDynamic();
            }
        }
    }
}
