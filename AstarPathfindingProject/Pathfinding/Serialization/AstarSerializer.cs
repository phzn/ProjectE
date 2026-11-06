using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Pathfinding.Ionic.Zip;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding.Serialization
{
	// Token: 0x02000244 RID: 580
	public class AstarSerializer
	{
		// Token: 0x06000E86 RID: 3718 RVA: 0x00059677 File Offset: 0x00057877
		private static StringBuilder GetStringBuilder()
		{
			AstarSerializer._stringBuilder.Length = 0;
			return AstarSerializer._stringBuilder;
		}

		// Token: 0x06000E87 RID: 3719 RVA: 0x00059689 File Offset: 0x00057889
		public AstarSerializer(AstarData data, GameObject contextRoot)
			: this(data, SerializeSettings.Settings, contextRoot)
		{
		}

		// Token: 0x06000E88 RID: 3720 RVA: 0x00059698 File Offset: 0x00057898
		public AstarSerializer(AstarData data, SerializeSettings settings, GameObject contextRoot)
		{
			this.data = data;
			this.contextRoot = contextRoot;
			this.settings = settings;
		}

		// Token: 0x06000E89 RID: 3721 RVA: 0x000596C7 File Offset: 0x000578C7
		private void AddChecksum(byte[] bytes)
		{
			this.checksum = Checksum.GetChecksum(bytes, this.checksum);
		}

		// Token: 0x06000E8A RID: 3722 RVA: 0x000596DB File Offset: 0x000578DB
		private void AddEntry(string name, byte[] bytes)
		{
			this.zip.AddEntry(name, bytes);
		}

		// Token: 0x06000E8B RID: 3723 RVA: 0x000596EB File Offset: 0x000578EB
		public uint GetChecksum()
		{
			return this.checksum;
		}

		// Token: 0x06000E8C RID: 3724 RVA: 0x000596F4 File Offset: 0x000578F4
		public void OpenSerialize()
		{
			this.zipStream = new MemoryStream();
			this.zip = new ZipFile();
			this.zip.AlternateEncoding = Encoding.UTF8;
			this.zip.AlternateEncodingUsage = ZipOption.Always;
			this.zip.ParallelDeflateThreshold = -1L;
			this.meta = new GraphMeta();
		}

		// Token: 0x06000E8D RID: 3725 RVA: 0x0005974C File Offset: 0x0005794C
		public byte[] CloseSerialize()
		{
			byte[] array = this.SerializeMeta();
			this.AddChecksum(array);
			this.AddEntry("meta.json", array);
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			foreach (ZipEntry zipEntry in this.zip.Entries)
			{
				zipEntry.AccessedTime = dateTime;
				zipEntry.CreationTime = dateTime;
				zipEntry.LastModified = dateTime;
				zipEntry.ModifiedTime = dateTime;
			}
			this.zip.Save(this.zipStream);
			this.zip.Dispose();
			array = this.zipStream.ToArray();
			this.zip = null;
			this.zipStream = null;
			return array;
		}

		// Token: 0x06000E8E RID: 3726 RVA: 0x00059814 File Offset: 0x00057A14
		public void SerializeGraphs(NavGraph[] _graphs)
		{
			if (this.graphs != null)
			{
				throw new InvalidOperationException("Cannot serialize graphs multiple times.");
			}
			this.graphs = _graphs;
			if (this.zip == null)
			{
				throw new NullReferenceException("You must not call CloseSerialize before a call to this function");
			}
			if (this.graphs == null)
			{
				this.graphs = new NavGraph[0];
			}
			this.persistentGraphs = new bool[this.data.graphs.Length];
			for (int i = 0; i < this.graphs.Length; i++)
			{
				if (this.graphs[i] != null)
				{
					this.persistentGraphs[(int)this.graphs[i].graphIndex] = this.graphs[i] != null && this.graphs[i].persistent;
					if (this.persistentGraphs[(int)this.graphs[i].graphIndex])
					{
						byte[] array = this.Serialize(this.graphs[i]);
						this.AddChecksum(array);
						this.AddEntry("graph" + i.ToString() + ".json", array);
					}
				}
			}
		}

		// Token: 0x06000E8F RID: 3727 RVA: 0x00059914 File Offset: 0x00057B14
		private byte[] SerializeMeta()
		{
			if (this.graphs == null)
			{
				throw new Exception("No call to SerializeGraphs has been done");
			}
			this.meta.version = AstarPath.Version;
			this.meta.graphs = this.graphs.Length;
			this.meta.guids = new List<string>();
			this.meta.typeNames = new List<string>();
			for (int i = 0; i < this.graphs.Length; i++)
			{
				if (this.graphs[i] != null && this.persistentGraphs[(int)this.graphs[i].graphIndex])
				{
					this.meta.guids.Add(this.graphs[i].guid.ToString());
					this.meta.typeNames.Add(this.graphs[i].GetType().FullName);
				}
				else
				{
					this.meta.guids.Add(null);
					this.meta.typeNames.Add(null);
				}
			}
			StringBuilder stringBuilder = AstarSerializer.GetStringBuilder();
			TinyJsonSerializer.Serialize(this.meta, stringBuilder);
			return this.encoding.GetBytes(stringBuilder.ToString());
		}

		// Token: 0x06000E90 RID: 3728 RVA: 0x00059A44 File Offset: 0x00057C44
		public byte[] Serialize(NavGraph graph)
		{
			StringBuilder stringBuilder = AstarSerializer.GetStringBuilder();
			TinyJsonSerializer.Serialize(graph, stringBuilder);
			return this.encoding.GetBytes(stringBuilder.ToString());
		}

		// Token: 0x06000E91 RID: 3729 RVA: 0x00059A70 File Offset: 0x00057C70
		private static int GetMaxNodeIndexInAllGraphs(NavGraph[] graphs)
		{
			int num = 0;
			for (int i = 0; i < graphs.Length; i++)
			{
				if (graphs[i] != null && graphs[i].persistent)
				{
					graphs[i].GetNodes<int>(delegate(GraphNode node, ref int maxIndex)
					{
						maxIndex = Math.Max((int)node.NodeIndex, maxIndex);
						if (node.Destroyed)
						{
							Debug.LogError("Graph contains destroyed nodes. This is a bug.");
						}
					}, ref num);
				}
			}
			return num;
		}

		// Token: 0x06000E92 RID: 3730 RVA: 0x00059AC8 File Offset: 0x00057CC8
		private static byte[] SerializeNodeIndices(NavGraph[] graphs)
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(memoryStream);
			int maxNodeIndexInAllGraphs = AstarSerializer.GetMaxNodeIndexInAllGraphs(graphs);
			writer.Write(maxNodeIndexInAllGraphs);
			int maxNodeIndex2 = 0;
			Action<GraphNode> <>9__0;
			for (int i = 0; i < graphs.Length; i++)
			{
				if (graphs[i] != null && graphs[i].persistent)
				{
					NavGraph navGraph = graphs[i];
					Action<GraphNode> action;
					if ((action = <>9__0) == null)
					{
						action = (<>9__0 = delegate(GraphNode node)
						{
							maxNodeIndex2 = Math.Max((int)node.NodeIndex, maxNodeIndex2);
							writer.Write(node.NodeIndex);
						});
					}
					navGraph.GetNodes(action);
				}
			}
			if (maxNodeIndex2 != maxNodeIndexInAllGraphs)
			{
				throw new Exception("Some graphs are not consistent in their GetNodes calls, sequential calls give different results.");
			}
			byte[] array = memoryStream.ToArray();
			writer.Close();
			return array;
		}

		// Token: 0x06000E93 RID: 3731 RVA: 0x00059B74 File Offset: 0x00057D74
		private static byte[] SerializeGraphExtraInfo(NavGraph graph, bool[] persistentGraphs)
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			GraphSerializationContext graphSerializationContext = new GraphSerializationContext(binaryWriter, persistentGraphs);
			((IGraphInternals)graph).SerializeExtraInfo(graphSerializationContext);
			byte[] array = memoryStream.ToArray();
			binaryWriter.Close();
			return array;
		}

		// Token: 0x06000E94 RID: 3732 RVA: 0x00059BA8 File Offset: 0x00057DA8
		private static byte[] SerializeGraphNodeReferences(NavGraph graph, bool[] persistentGraphs)
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			GraphSerializationContext ctx = new GraphSerializationContext(binaryWriter, persistentGraphs);
			graph.GetNodes(delegate(GraphNode node)
			{
				node.SerializeReferences(ctx);
			});
			binaryWriter.Close();
			return memoryStream.ToArray();
		}

		// Token: 0x06000E95 RID: 3733 RVA: 0x00059BF4 File Offset: 0x00057DF4
		public void SerializeExtraInfo()
		{
			if (!this.settings.nodes)
			{
				return;
			}
			if (this.graphs == null)
			{
				throw new InvalidOperationException("Cannot serialize extra info with no serialized graphs (call SerializeGraphs first)");
			}
			byte[] array = AstarSerializer.SerializeNodeIndices(this.graphs);
			this.AddChecksum(array);
			this.AddEntry("graph_references.binary", array);
			for (int i = 0; i < this.graphs.Length; i++)
			{
				if (this.graphs[i] != null && this.graphs[i].persistent)
				{
					array = AstarSerializer.SerializeGraphExtraInfo(this.graphs[i], this.persistentGraphs);
					this.AddChecksum(array);
					this.AddEntry("graph" + i.ToString() + "_extra.binary", array);
					array = AstarSerializer.SerializeGraphNodeReferences(this.graphs[i], this.persistentGraphs);
					this.AddChecksum(array);
					this.AddEntry("graph" + i.ToString() + "_references.binary", array);
				}
			}
		}

		// Token: 0x06000E96 RID: 3734 RVA: 0x00059CE3 File Offset: 0x00057EE3
		private ZipEntry GetEntry(string name)
		{
			return this.zip[name];
		}

		// Token: 0x06000E97 RID: 3735 RVA: 0x00059CF1 File Offset: 0x00057EF1
		private bool ContainsEntry(string name)
		{
			return this.GetEntry(name) != null;
		}

		// Token: 0x06000E98 RID: 3736 RVA: 0x00059D00 File Offset: 0x00057F00
		public bool OpenDeserialize(byte[] bytes)
		{
			this.zipStream = new MemoryStream();
			this.zipStream.Write(bytes, 0, bytes.Length);
			this.zipStream.Position = 0L;
			try
			{
				this.zip = ZipFile.Read(this.zipStream);
				this.zip.ParallelDeflateThreshold = -1L;
			}
			catch (Exception ex)
			{
				string text = "Caught exception when loading from zip\n";
				Exception ex2 = ex;
				Debug.LogError(text + ((ex2 != null) ? ex2.ToString() : null));
				this.zipStream.Dispose();
				return false;
			}
			if (this.ContainsEntry("meta.json"))
			{
				this.meta = this.DeserializeMeta(this.GetEntry("meta.json"));
			}
			else
			{
				if (!this.ContainsEntry("meta.binary"))
				{
					throw new Exception("No metadata found in serialized data.");
				}
				this.meta = this.DeserializeBinaryMeta(this.GetEntry("meta.binary"));
			}
			if (AstarSerializer.FullyDefinedVersion(this.meta.version) > AstarSerializer.FullyDefinedVersion(AstarPath.Version))
			{
				string[] array = new string[5];
				array[0] = "Trying to load data from a newer version of the A* Pathfinding Project\nCurrent version: ";
				int num = 1;
				Version version = AstarPath.Version;
				array[num] = ((version != null) ? version.ToString() : null);
				array[2] = " Data version: ";
				int num2 = 3;
				Version version2 = this.meta.version;
				array[num2] = ((version2 != null) ? version2.ToString() : null);
				array[4] = "\nThis is usually fine as the stored data is usually backwards and forwards compatible.\nHowever node data (not settings) can get corrupted between versions (even though I try my best to keep compatibility), so it is recommended to recalculate any caches (those for faster startup) and resave any files. Even if it seems to load fine, it might cause subtle bugs.\n";
				Debug.LogWarning(string.Concat(array));
			}
			return true;
		}

		// Token: 0x06000E99 RID: 3737 RVA: 0x00059E68 File Offset: 0x00058068
		private static Version FullyDefinedVersion(Version v)
		{
			return new Version(Mathf.Max(v.Major, 0), Mathf.Max(v.Minor, 0), Mathf.Max(v.Build, 0), Mathf.Max(v.Revision, 0));
		}

		// Token: 0x06000E9A RID: 3738 RVA: 0x00059E9F File Offset: 0x0005809F
		public void CloseDeserialize()
		{
			this.zipStream.Dispose();
			this.zip.Dispose();
			this.zip = null;
			this.zipStream = null;
		}

		// Token: 0x06000E9B RID: 3739 RVA: 0x00059EC8 File Offset: 0x000580C8
		private NavGraph DeserializeGraph(int zipIndex, int graphIndex, Type[] availableGraphTypes)
		{
			Type graphType = this.meta.GetGraphType(zipIndex, availableGraphTypes);
			if (object.Equals(graphType, null))
			{
				return null;
			}
			NavGraph navGraph = this.data.CreateGraph(graphType);
			navGraph.graphIndex = (uint)graphIndex;
			string text = "graph" + zipIndex.ToString() + ".json";
			if (!this.ContainsEntry(text))
			{
				throw new FileNotFoundException(string.Concat(new string[]
				{
					"Could not find data for graph ",
					zipIndex.ToString(),
					" in zip. Entry 'graph",
					zipIndex.ToString(),
					".json' does not exist"
				}));
			}
			TinyJsonDeserializer.Deserialize(AstarSerializer.GetString(this.GetEntry(text)), graphType, navGraph, this.contextRoot);
			if (navGraph.guid.ToString() != this.meta.guids[zipIndex])
			{
				string text2 = "Guid in graph file not equal to guid defined in meta file. Have you edited the data manually?\n";
				Guid guid = navGraph.guid;
				throw new Exception(text2 + guid.ToString() + " != " + this.meta.guids[zipIndex]);
			}
			return navGraph;
		}

		// Token: 0x06000E9C RID: 3740 RVA: 0x00059FE0 File Offset: 0x000581E0
		public NavGraph[] DeserializeGraphs(Type[] availableGraphTypes, bool allowLoadingNodes, Func<int> nextGraphIndex)
		{
			List<NavGraph> list = new List<NavGraph>();
			this.graphIndexInZip = new Dictionary<NavGraph, int>();
			for (int i = 0; i < this.meta.graphs; i++)
			{
				NavGraph navGraph = this.DeserializeGraph(i, nextGraphIndex(), availableGraphTypes);
				if (navGraph != null)
				{
					list.Add(navGraph);
					this.graphIndexInZip[navGraph] = i;
				}
			}
			this.graphs = list.ToArray();
			this.DeserializeEditorSettingsCompatibility();
			if (allowLoadingNodes)
			{
				this.DeserializeExtraInfo();
			}
			return this.graphs;
		}

		// Token: 0x06000E9D RID: 3741 RVA: 0x0005A05C File Offset: 0x0005825C
		private bool DeserializeExtraInfo(NavGraph graph)
		{
			ZipEntry entry = this.GetEntry("graph" + this.graphIndexInZip[graph].ToString() + "_extra.binary");
			if (entry == null)
			{
				return false;
			}
			GraphSerializationContext graphSerializationContext = new GraphSerializationContext(AstarSerializer.GetBinaryReader(entry), null, graph.graphIndex, this.meta);
			((IGraphInternals)graph).DeserializeExtraInfo(graphSerializationContext);
			return true;
		}

		// Token: 0x06000E9E RID: 3742 RVA: 0x0005A0BC File Offset: 0x000582BC
		private bool AnyDestroyedNodesInGraphs()
		{
			bool result = false;
			Action<GraphNode> <>9__0;
			for (int i = 0; i < this.graphs.Length; i++)
			{
				NavGraph navGraph = this.graphs[i];
				Action<GraphNode> action;
				if ((action = <>9__0) == null)
				{
					action = (<>9__0 = delegate(GraphNode node)
					{
						if (node.Destroyed)
						{
							result = true;
						}
					});
				}
				navGraph.GetNodes(action);
			}
			return result;
		}

		// Token: 0x06000E9F RID: 3743 RVA: 0x0005A11C File Offset: 0x0005831C
		private GraphNode[] DeserializeNodeReferenceMap()
		{
			ZipEntry entry = this.GetEntry("graph_references.binary");
			if (entry == null)
			{
				throw new Exception("Node references not found in the data. Was this loaded from an older version of the A* Pathfinding Project?");
			}
			BinaryReader reader = AstarSerializer.GetBinaryReader(entry);
			int num = reader.ReadInt32();
			GraphNode[] int2Node = new GraphNode[num + 1];
			try
			{
				Action<GraphNode> <>9__0;
				for (int i = 0; i < this.graphs.Length; i++)
				{
					NavGraph navGraph = this.graphs[i];
					Action<GraphNode> action;
					if ((action = <>9__0) == null)
					{
						action = (<>9__0 = delegate(GraphNode node)
						{
							int num2 = reader.ReadInt32();
							int2Node[num2] = node;
						});
					}
					navGraph.GetNodes(action);
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Some graph(s) has thrown an exception during GetNodes, or some graph(s) have deserialized more or fewer nodes than were serialized", ex);
			}
			if (reader.BaseStream.Position != reader.BaseStream.Length)
			{
				throw new Exception((reader.BaseStream.Length / 4L).ToString() + " nodes were serialized, but only data for " + (reader.BaseStream.Position / 4L).ToString() + " nodes was found. The data looks corrupt.");
			}
			reader.Close();
			return int2Node;
		}

		// Token: 0x06000EA0 RID: 3744 RVA: 0x0005A254 File Offset: 0x00058454
		private void DeserializeNodeReferences(NavGraph graph, GraphNode[] int2Node)
		{
			int num = this.graphIndexInZip[graph];
			ZipEntry entry = this.GetEntry("graph" + num.ToString() + "_references.binary");
			if (entry == null)
			{
				throw new Exception("Node references for graph " + num.ToString() + " not found in the data. Was this loaded from an older version of the A* Pathfinding Project?");
			}
			GraphSerializationContext graphSerializationContext = new GraphSerializationContext(AstarSerializer.GetBinaryReader(entry), int2Node, graph.graphIndex, this.meta);
			graph.GetNodes<GraphSerializationContext>(delegate(GraphNode node, ref GraphSerializationContext ctx)
			{
				node.DeserializeReferences(ctx);
			}, ref graphSerializationContext);
		}

		// Token: 0x06000EA1 RID: 3745 RVA: 0x0005A2E8 File Offset: 0x000584E8
		private void DeserializeAndRemoveOldNodeLinks(GraphSerializationContext ctx)
		{
			int num = ctx.reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				ctx.reader.ReadUInt64();
				GraphNode graphNode = ctx.DeserializeNodeReference();
				GraphNode graphNode2 = ctx.DeserializeNodeReference();
				GraphNode graphNode3 = ctx.DeserializeNodeReference();
				GraphNode graphNode4 = ctx.DeserializeNodeReference();
				ctx.DeserializeVector3();
				ctx.DeserializeVector3();
				ctx.reader.ReadBoolean();
				graphNode.ClearConnections(true);
				graphNode2.ClearConnections(true);
				graphNode.Walkable = false;
				graphNode2.Walkable = false;
				GraphNode.Disconnect(graphNode3, graphNode);
				GraphNode.Disconnect(graphNode4, graphNode2);
			}
			bool flag = false;
			int num2 = 0;
			while (num2 < this.graphs.Length && !flag)
			{
				if (this.graphs[num2] != null)
				{
					PointGraph pointGraph = this.graphs[num2] as PointGraph;
					if (pointGraph != null)
					{
						bool anyWalkable = false;
						int count2 = 0;
						pointGraph.GetNodes(delegate(GraphNode node)
						{
							anyWalkable |= node.Walkable;
							int count = count2;
							count2 = count + 1;
						});
						if (!anyWalkable && pointGraph.root == null && 2 * num == count2 && (count2 > 0 || pointGraph.name.Contains("used for node links")))
						{
							((IGraphInternals)this.graphs[num2]).DestroyAllNodes();
							List<NavGraph> list = new List<NavGraph>(this.graphs);
							list.RemoveAt(num2);
							this.graphs = list.ToArray();
							flag = true;
						}
						if (pointGraph.name == "PointGraph (used for node links)")
						{
							pointGraph.name = "PointGraph";
						}
					}
				}
				num2++;
			}
			if (!flag && num > 0)
			{
				Debug.LogWarning("Old off-mesh links were present in the serialized graph data. Not everything could be cleaned up properly. It is recommended that you re-scan all graphs and save the cache or graph file again. An attempt to migrate the old links was made, but a stray point graph may have been left behind.");
			}
		}

		// Token: 0x06000EA2 RID: 3746 RVA: 0x0005A494 File Offset: 0x00058694
		private void DeserializeExtraInfo()
		{
			bool flag = false;
			for (int i = 0; i < this.graphs.Length; i++)
			{
				flag |= this.DeserializeExtraInfo(this.graphs[i]);
			}
			if (!flag)
			{
				return;
			}
			if (this.AnyDestroyedNodesInGraphs())
			{
				Debug.LogError("Graph contains destroyed nodes. This is a bug.");
			}
			GraphNode[] array = this.DeserializeNodeReferenceMap();
			for (int j = 0; j < this.graphs.Length; j++)
			{
				this.DeserializeNodeReferences(this.graphs[j], array);
			}
			if (this.meta.version < AstarSerializer.V4_3_85)
			{
				ZipEntry entry = this.GetEntry("node_link2.binary");
				if (entry != null)
				{
					GraphSerializationContext graphSerializationContext = new GraphSerializationContext(AstarSerializer.GetBinaryReader(entry), array, 0U, this.meta);
					this.DeserializeAndRemoveOldNodeLinks(graphSerializationContext);
				}
			}
		}

		// Token: 0x06000EA3 RID: 3747 RVA: 0x0005A54C File Offset: 0x0005874C
		public void PostDeserialization()
		{
			for (int i = 0; i < this.graphs.Length; i++)
			{
				GraphSerializationContext graphSerializationContext = new GraphSerializationContext(null, null, 0U, this.meta);
				((IGraphInternals)this.graphs[i]).PostDeserialization(graphSerializationContext);
			}
		}

		// Token: 0x06000EA4 RID: 3748 RVA: 0x0005A58C File Offset: 0x0005878C
		private void DeserializeEditorSettingsCompatibility()
		{
			for (int i = 0; i < this.graphs.Length; i++)
			{
				ZipEntry entry = this.GetEntry("graph" + this.graphIndexInZip[this.graphs[i]].ToString() + "_editor.json");
				if (entry != null)
				{
					((IGraphInternals)this.graphs[i]).SerializedEditorSettings = AstarSerializer.GetString(entry);
				}
			}
		}

		// Token: 0x06000EA5 RID: 3749 RVA: 0x0005A5F4 File Offset: 0x000587F4
		private static BinaryReader GetBinaryReader(ZipEntry entry)
		{
			MemoryStream memoryStream = new MemoryStream((int)entry.UncompressedSize);
			entry.Extract(memoryStream);
			memoryStream.Position = 0L;
			return new BinaryReader(memoryStream);
		}

		// Token: 0x06000EA6 RID: 3750 RVA: 0x0005A624 File Offset: 0x00058824
		private static string GetString(ZipEntry entry)
		{
			MemoryStream memoryStream = new MemoryStream((int)entry.UncompressedSize);
			entry.Extract(memoryStream);
			memoryStream.Position = 0L;
			StreamReader streamReader = new StreamReader(memoryStream);
			string text = streamReader.ReadToEnd();
			streamReader.Dispose();
			return text;
		}

		// Token: 0x06000EA7 RID: 3751 RVA: 0x0005A660 File Offset: 0x00058860
		private GraphMeta DeserializeMeta(ZipEntry entry)
		{
			return TinyJsonDeserializer.Deserialize(AstarSerializer.GetString(entry), typeof(GraphMeta), null, null) as GraphMeta;
		}

		// Token: 0x06000EA8 RID: 3752 RVA: 0x0005A680 File Offset: 0x00058880
		private GraphMeta DeserializeBinaryMeta(ZipEntry entry)
		{
			GraphMeta graphMeta = new GraphMeta();
			BinaryReader binaryReader = AstarSerializer.GetBinaryReader(entry);
			if (binaryReader.ReadString() != "A*")
			{
				throw new Exception("Invalid magic number in saved data");
			}
			int num = binaryReader.ReadInt32();
			int num2 = binaryReader.ReadInt32();
			int num3 = binaryReader.ReadInt32();
			int num4 = binaryReader.ReadInt32();
			if (num < 0)
			{
				graphMeta.version = new Version(0, 0);
			}
			else if (num2 < 0)
			{
				graphMeta.version = new Version(num, 0);
			}
			else if (num3 < 0)
			{
				graphMeta.version = new Version(num, num2);
			}
			else if (num4 < 0)
			{
				graphMeta.version = new Version(num, num2, num3);
			}
			else
			{
				graphMeta.version = new Version(num, num2, num3, num4);
			}
			graphMeta.graphs = binaryReader.ReadInt32();
			graphMeta.guids = new List<string>();
			int num5 = binaryReader.ReadInt32();
			for (int i = 0; i < num5; i++)
			{
				graphMeta.guids.Add(binaryReader.ReadString());
			}
			graphMeta.typeNames = new List<string>();
			num5 = binaryReader.ReadInt32();
			for (int j = 0; j < num5; j++)
			{
				graphMeta.typeNames.Add(binaryReader.ReadString());
			}
			binaryReader.Close();
			return graphMeta;
		}

		// Token: 0x06000EA9 RID: 3753 RVA: 0x0005A7B4 File Offset: 0x000589B4
		public static void SaveToFile(string path, byte[] data)
		{
			using (FileStream fileStream = new FileStream(path, FileMode.Create))
			{
				fileStream.Write(data, 0, data.Length);
			}
		}

		// Token: 0x06000EAA RID: 3754 RVA: 0x0005A7F0 File Offset: 0x000589F0
		public static byte[] LoadFromFile(string path)
		{
			byte[] array2;
			using (FileStream fileStream = new FileStream(path, FileMode.Open))
			{
				byte[] array = new byte[(int)fileStream.Length];
				fileStream.Read(array, 0, (int)fileStream.Length);
				array2 = array;
			}
			return array2;
		}

		// Token: 0x04000AE5 RID: 2789
		private AstarData data;

		// Token: 0x04000AE6 RID: 2790
		private ZipFile zip;

		// Token: 0x04000AE7 RID: 2791
		private MemoryStream zipStream;

		// Token: 0x04000AE8 RID: 2792
		private GraphMeta meta;

		// Token: 0x04000AE9 RID: 2793
		private SerializeSettings settings;

		// Token: 0x04000AEA RID: 2794
		private GameObject contextRoot;

		// Token: 0x04000AEB RID: 2795
		private NavGraph[] graphs;

		// Token: 0x04000AEC RID: 2796
		private bool[] persistentGraphs;

		// Token: 0x04000AED RID: 2797
		private Dictionary<NavGraph, int> graphIndexInZip;

		// Token: 0x04000AEE RID: 2798
		private const string binaryExt = ".binary";

		// Token: 0x04000AEF RID: 2799
		private const string jsonExt = ".json";

		// Token: 0x04000AF0 RID: 2800
		private uint checksum = uint.MaxValue;

		// Token: 0x04000AF1 RID: 2801
		private UTF8Encoding encoding = new UTF8Encoding();

		// Token: 0x04000AF2 RID: 2802
		private static StringBuilder _stringBuilder = new StringBuilder();

		// Token: 0x04000AF3 RID: 2803
		public static readonly Version V3_8_3 = new Version(3, 8, 3);

		// Token: 0x04000AF4 RID: 2804
		public static readonly Version V3_9_0 = new Version(3, 9, 0);

		// Token: 0x04000AF5 RID: 2805
		public static readonly Version V4_1_0 = new Version(4, 1, 0);

		// Token: 0x04000AF6 RID: 2806
		public static readonly Version V4_3_2 = new Version(4, 3, 2);

		// Token: 0x04000AF7 RID: 2807
		public static readonly Version V4_3_6 = new Version(4, 3, 6);

		// Token: 0x04000AF8 RID: 2808
		public static readonly Version V4_3_37 = new Version(4, 3, 37);

		// Token: 0x04000AF9 RID: 2809
		public static readonly Version V4_3_12 = new Version(4, 3, 12);

		// Token: 0x04000AFA RID: 2810
		public static readonly Version V4_3_68 = new Version(4, 3, 68);

		// Token: 0x04000AFB RID: 2811
		public static readonly Version V4_3_74 = new Version(4, 3, 74);

		// Token: 0x04000AFC RID: 2812
		public static readonly Version V4_3_80 = new Version(4, 3, 80);

		// Token: 0x04000AFD RID: 2813
		public static readonly Version V4_3_83 = new Version(4, 3, 83);

		// Token: 0x04000AFE RID: 2814
		public static readonly Version V4_3_85 = new Version(4, 3, 85);

		// Token: 0x04000AFF RID: 2815
		public static readonly Version V4_3_87 = new Version(4, 3, 87);

		// Token: 0x04000B00 RID: 2816
		public static readonly Version V5_1_0 = new Version(5, 1, 0);

		// Token: 0x04000B01 RID: 2817
		public static readonly Version V5_2_0 = new Version(5, 2, 0);
	}
}
