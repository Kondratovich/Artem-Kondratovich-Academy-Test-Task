using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Artem_Kondratovich_Academy_Test_Task {
	public class Edge {
		public readonly Node From;
		public readonly Node To;
		public Edge(Node first, Node second) {
			this.From = first;
			this.To = second;
		}
		public bool IsIncident(Node node) {
			return From == node || To == node;
		}
		public Node OtherNode(Node node) {
			if (!IsIncident(node)) throw new ArgumentException();
			if (From == node) return To;
			return From;
		}
	}

	public class Node {
		readonly List<Edge> edges = new List<Edge>();
		public readonly int NodeNumber;

		public Node(int number) {
			NodeNumber = number;
		}

		//public IEnumerable<Node> IncidentNodes {
		//	get {
		//		return edges.Select(z => z.OtherNode(this));
		//	}
		//}
		public IEnumerable<Edge> IncidentEdges {
			get {
				foreach (var e in edges) yield return e;
			}
		}
		public static Edge Connect(Node node1, Node node2) {
			var edge = new Edge(node1, node2);
			node1.edges.Add(edge);
			node2.edges.Add(edge);
			return edge;
		}
		//public static void Disconnect(Edge edge) {
		//	edge.From.edges.Remove(edge);
		//	edge.To.edges.Remove(edge);
		//}
	}

	public class Graph {
		private Node[] nodes;

		public Graph(int nodesCount) {
			//nodes = Enumerable.Range(0, nodesCount).Select(z => new Node(z)).ToArray();
			nodes = new Node[nodesCount];
            for (int i = 0; i < nodesCount; i++) {
				nodes[i] = new Node(i);
            }
		}

		//public int Length { get { return nodes.Length; } }

		public Node this[int index] { get { return nodes[index]; } }

		public IEnumerable<Node> Nodes {
			get {
				foreach (var node in nodes) yield return node;
			}
		}

		//public void Connect(int index1, int index2) {
		//	Node.Connect(nodes[index1], nodes[index2]);
		//}

		//public void Delete(Edge edge) {
		//	Node.Disconnect(edge);
		//}

		//public IEnumerable<Edge> Edges {
		//	get {
		//		return nodes.SelectMany(z => z.IncidentEdges).Distinct();
		//	}
		//}

		//public static Graph MakeGraph(params int[] incidentNodes) {
		//	var graph = new Graph(incidentNodes.Max() + 1);
		//	for (int i = 0; i < incidentNodes.Length - 1; i += 2)
		//		graph.Connect(incidentNodes[i], incidentNodes[i + 1]);
		//	return graph;
		//}
	}

	class DijkstraData {
		public Node Previous { get; set; }
		public double Price { get; set; }
	}

	public class Rover {
		static int rows;
		static int columns;

		public static void CalculateRoverPath(int[,] map) {
			rows = map.GetUpperBound(0) + 1;
			columns = map.Length / rows;
			var weights = new Dictionary<Edge, double>();
			Graph graph = new Graph(map.Length);
			Node start = graph[0];
			Node end = graph[map.Length - 1];
			for (int i = 0; i < rows; i++) {
				for (int j = 0; j < columns; j++) {
					if (j == columns - 1)
						break;
					weights[Node.Connect(graph[columns * i + j], graph[(columns * i + j) + 1])] = Math.Abs(map[i, j] - map[i, j + 1]);
					weights[Node.Connect(graph[(columns * i + j) + 1], graph[columns * i + j])] = Math.Abs(map[i, j] - map[i, j + 1]);
				}
			}
			for (int j = 0; j < columns; j++) {
				for (int i = 0; i < rows; i++) {
					if (i == rows - 1)
						break;
					weights[Node.Connect(graph[i * columns + j], graph[(i * columns + j) + columns])] = Math.Abs(map[i, j] - map[i + 1, j]);
					weights[Node.Connect(graph[(i * columns + j) + columns], graph[i * columns + j])] = Math.Abs(map[i, j] - map[i + 1, j]);
				}
			}
			Dijkstra(graph, weights, start, end);
		}

		public static void OutputResult(List<Node> nodes, int steps, double liftingFuel) {
			StringBuilder sb = new StringBuilder();
			sb.Append("path-plan.txt\n");
			foreach (var node in nodes) {
				sb.Append($"[{node.NodeNumber / columns}][{node.NodeNumber - (node.NodeNumber / columns) * columns}]");
				if (node != nodes.Last()) {
					sb.Append("->");
				}
			}
			sb.Append($"\nsteps: {steps}");
			sb.Append($"\nfuel: {liftingFuel+steps}");
			using (FileStream fstream = new FileStream("path-plan.txt", FileMode.Create)) {
				byte[] array = System.Text.Encoding.Default.GetBytes(sb.ToString());
				fstream.Write(array, 0, array.Length);
			}
			Console.WriteLine(sb);
		}

		public static void Dijkstra(Graph graph, Dictionary<Edge, double> weights, Node start, Node end) {
			var notVisited = graph.Nodes.ToList();
			var track = new Dictionary<Node, DijkstraData>();
			track[start] = new DijkstraData { Price = 0, Previous = null };

			while (true) {
				Node toOpen = null;
				var bestPrice = double.PositiveInfinity;
				foreach (var e in notVisited) {
					if (track.ContainsKey(e) && track[e].Price < bestPrice) {
						bestPrice = track[e].Price;
						toOpen = e;
					}
				}

				if (toOpen == null) return;
				if (toOpen == end) break;

				foreach (var e in toOpen.IncidentEdges.Where(z => z.From == toOpen)) {
					var currentPrice = track[toOpen].Price + weights[e] + 1;
					var nextNode = e.OtherNode(toOpen);
					if (!track.ContainsKey(nextNode) || track[nextNode].Price > currentPrice) {
						track[nextNode] = new DijkstraData { Previous = toOpen, Price = currentPrice };
					}
				}

				notVisited.Remove(toOpen);
			}

			var result = new List<Node>();
			double liftingFuel = 0;
			int steps = -1;
			while (end != null) {
				//if (track[end].Previous != null) { 
				//	liftingFuel += weights[end.IncidentEdges.FirstOrDefault(e => e.IsIncident(track[end].Previous))];
				//}
				if (track[end].Previous != null) {
                    foreach (var edge in end.IncidentEdges) {
                        if (edge.IsIncident(track[end].Previous)) {
							liftingFuel += weights[edge];
							break;
                        }
                    }
				}
				result.Add(end);
				end = track[end].Previous;
				steps++;
			}
			result.Reverse(); 
			OutputResult(result, steps, liftingFuel);
			return;
		}
	}
}
