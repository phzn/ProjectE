using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding.Serialization;
using Pathfinding.Util;
using Pathfinding.WindowsStore;
using Unity.Profiling;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000048 RID: 72
	[Serializable]
	public class AstarData
	{
		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060002AC RID: 684 RVA: 0x0000C0A7 File Offset: 0x0000A2A7
		[Obsolete("Use navmeshGraph instead")]
		public NavMeshGraph navmesh
		{
			get
			{
				return this.navmeshGraph;
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060002AD RID: 685 RVA: 0x0000C0AF File Offset: 0x0000A2AF
		// (set) Token: 0x060002AE RID: 686 RVA: 0x0000C0B7 File Offset: 0x0000A2B7
		public NavMeshGraph navmeshGraph { get; private set; }

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060002AF RID: 687 RVA: 0x0000C0C0 File Offset: 0x0000A2C0
		// (set) Token: 0x060002B0 RID: 688 RVA: 0x0000C0C8 File Offset: 0x0000A2C8
		public GridGraph gridGraph { get; private set; }

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060002B1 RID: 689 RVA: 0x0000C0D1 File Offset: 0x0000A2D1
		// (set) Token: 0x060002B2 RID: 690 RVA: 0x0000C0D9 File Offset: 0x0000A2D9
		public LayerGridGraph layerGridGraph { get; private set; }

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060002B3 RID: 691 RVA: 0x0000C0E2 File Offset: 0x0000A2E2
		// (set) Token: 0x060002B4 RID: 692 RVA: 0x0000C0EA File Offset: 0x0000A2EA
		public PointGraph pointGraph { get; private set; }

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060002B5 RID: 693 RVA: 0x0000C0F3 File Offset: 0x0000A2F3
		// (set) Token: 0x060002B6 RID: 694 RVA: 0x0000C0FB File Offset: 0x0000A2FB
		public RecastGraph recastGraph { get; private set; }

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060002B7 RID: 695 RVA: 0x0000C104 File Offset: 0x0000A304
		// (set) Token: 0x060002B8 RID: 696 RVA: 0x0000C10C File Offset: 0x0000A30C
		public LinkGraph linkGraph { get; private set; }

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060002B9 RID: 697 RVA: 0x0000C115 File Offset: 0x0000A315
		// (set) Token: 0x060002BA RID: 698 RVA: 0x0000C11C File Offset: 0x0000A31C
		public static Type[] graphTypes { get; private set; }

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060002BB RID: 699 RVA: 0x0000C124 File Offset: 0x0000A324
		// (set) Token: 0x060002BC RID: 700 RVA: 0x0000C152 File Offset: 0x0000A352
		private byte[] data
		{
			get
			{
				byte[] array = ((this.dataString != null) ? Convert.FromBase64String(this.dataString) : null);
				if (array != null && array.Length == 0)
				{
					return null;
				}
				return array;
			}
			set
			{
				this.dataString = ((value != null) ? Convert.ToBase64String(value) : null);
			}
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000C166 File Offset: 0x0000A366
		internal AstarData(AstarPath active)
		{
			this.active = active;
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000C18C File Offset: 0x0000A38C
		public byte[] GetData()
		{
			return this.data;
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0000C194 File Offset: 0x0000A394
		public void SetData(byte[] data)
		{
			this.data = data;
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000C1A0 File Offset: 0x0000A3A0
		public void OnEnable()
		{
			this.FindGraphTypes();
			if (this.graphs == null)
			{
				this.graphs = new NavGraph[0];
			}
			if (this.cacheStartup && this.file_cachedStartup != null && Application.isPlaying)
			{
				this.LoadFromCache();
				return;
			}
			this.DeserializeGraphs();
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0000C1F1 File Offset: 0x0000A3F1
		internal void LockGraphStructure(bool allowAddingGraphs = false)
		{
			this.graphStructureLocked.Add(allowAddingGraphs);
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000C1FF File Offset: 0x0000A3FF
		internal void UnlockGraphStructure()
		{
			if (this.graphStructureLocked.Count == 0)
			{
				throw new InvalidOperationException();
			}
			this.graphStructureLocked.RemoveAt(this.graphStructureLocked.Count - 1);
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0000C22C File Offset: 0x0000A42C
		private PathProcessor.GraphUpdateLock AssertSafe(bool onlyAddingGraph = false)
		{
			if (this.graphStructureLocked.Count > 0)
			{
				bool flag = true;
				for (int i = 0; i < this.graphStructureLocked.Count; i++)
				{
					flag &= this.graphStructureLocked[i];
				}
				if (!onlyAddingGraph || !flag)
				{
					throw new InvalidOperationException("Graphs cannot be added, removed or serialized while the graph structure is locked. This is the case when a graph is currently being scanned and when executing graph updates and work items.\nHowever as a special case, graphs can be added inside work items.");
				}
			}
			PathProcessor.GraphUpdateLock graphUpdateLock = this.active.PausePathfinding();
			if (!this.active.IsInsideWorkItem)
			{
				this.active.FlushWorkItems();
				this.active.pathReturnQueue.ReturnPaths(false);
			}
			return graphUpdateLock;
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000C2B4 File Offset: 0x0000A4B4
		public void GetNodes(Action<GraphNode> callback)
		{
			for (int i = 0; i < this.graphs.Length; i++)
			{
				if (this.graphs[i] != null)
				{
					this.graphs[i].GetNodes(callback);
				}
			}
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000C2EC File Offset: 0x0000A4EC
		public void UpdateShortcuts()
		{
			this.navmeshGraph = (NavMeshGraph)this.FindGraphOfType(typeof(NavMeshGraph));
			this.gridGraph = (GridGraph)this.FindGraphOfType(typeof(GridGraph));
			this.layerGridGraph = (LayerGridGraph)this.FindGraphOfType(typeof(LayerGridGraph));
			this.pointGraph = (PointGraph)this.FindGraphOfType(typeof(PointGraph));
			this.recastGraph = (RecastGraph)this.FindGraphOfType(typeof(RecastGraph));
			this.linkGraph = (LinkGraph)this.FindGraphOfType(typeof(LinkGraph));
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000C39C File Offset: 0x0000A59C
		public void LoadFromCache()
		{
			using (AstarData.MarkerLoadFromCache.Auto())
			{
				using (this.AssertSafe(false))
				{
					if (this.file_cachedStartup != null)
					{
						byte[] bytes = this.file_cachedStartup.bytes;
						this.DeserializeGraphs(bytes);
						GraphModifier.TriggerEvent(GraphModifier.EventType.PostCacheLoad);
					}
					else
					{
						Debug.LogError("Can't load from cache since the cache is empty");
					}
				}
			}
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0000C430 File Offset: 0x0000A630
		public byte[] SerializeGraphs()
		{
			return this.SerializeGraphs(SerializeSettings.Settings);
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0000C440 File Offset: 0x0000A640
		public byte[] SerializeGraphs(SerializeSettings settings)
		{
			uint num;
			return this.SerializeGraphs(settings, out num);
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0000C456 File Offset: 0x0000A656
		public byte[] SerializeGraphs(SerializeSettings settings, out uint checksum)
		{
			return this.SerializeGraphs(settings, this.graphs, out checksum);
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000C468 File Offset: 0x0000A668
		public byte[] SerializeGraph(SerializeSettings settings, NavGraph graph)
		{
			uint num;
			return this.SerializeGraph(settings, graph, out num);
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000C47F File Offset: 0x0000A67F
		public byte[] SerializeGraph(SerializeSettings settings, NavGraph graph, out uint checksum)
		{
			if (graph == null)
			{
				throw new ArgumentNullException("graph");
			}
			return this.SerializeGraphs(settings, new NavGraph[] { graph }, out checksum);
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000C4A4 File Offset: 0x0000A6A4
		public byte[] SerializeGraphs(SerializeSettings settings, NavGraph[] graphs, out uint checksum)
		{
			byte[] array2;
			using (this.AssertSafe(false))
			{
				for (int i = 0; i < graphs.Length; i++)
				{
					if (graphs[i] != null)
					{
						int num = Array.IndexOf<NavGraph>(this.graphs, graphs[i]);
						if (num == -1)
						{
							throw new InvalidOperationException("One of the specified graphs is not loaded. It cannot be serialized.");
						}
						if ((long)num != (long)((ulong)graphs[i].graphIndex))
						{
							throw new InvalidOperationException("One of the specified graphs has an invalid graph index.");
						}
					}
				}
				AstarSerializer astarSerializer = new AstarSerializer(this, settings, this.active.gameObject);
				astarSerializer.OpenSerialize();
				astarSerializer.SerializeGraphs(graphs);
				astarSerializer.SerializeExtraInfo();
				byte[] array = astarSerializer.CloseSerialize();
				checksum = astarSerializer.GetChecksum();
				array2 = array;
			}
			return array2;
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0000C558 File Offset: 0x0000A758
		public void DeserializeGraphs()
		{
			byte[] data = this.data;
			if (data != null)
			{
				this.DeserializeGraphs(data);
			}
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0000C578 File Offset: 0x0000A778
		public void ClearGraphs()
		{
			using (this.AssertSafe(false))
			{
				this.ClearGraphsInternal();
			}
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000C5B4 File Offset: 0x0000A7B4
		private void ClearGraphsInternal()
		{
			if (this.graphs == null)
			{
				return;
			}
			using (this.AssertSafe(false))
			{
				for (int i = 0; i < this.graphs.Length; i++)
				{
					if (this.graphs[i] != null)
					{
						this.active.DirtyBounds(this.graphs[i].bounds);
						((IGraphInternals)this.graphs[i]).OnDestroy();
						this.graphs[i].active = null;
					}
				}
				this.graphs = new NavGraph[0];
				this.UpdateShortcuts();
			}
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000C654 File Offset: 0x0000A854
		public void DisposeUnmanagedData()
		{
			if (this.graphs == null)
			{
				return;
			}
			using (this.AssertSafe(false))
			{
				for (int i = 0; i < this.graphs.Length; i++)
				{
					if (this.graphs[i] != null)
					{
						((IGraphInternals)this.graphs[i]).DisposeUnmanagedData();
					}
				}
			}
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000C6BC File Offset: 0x0000A8BC
		internal void DestroyAllNodes()
		{
			if (this.graphs == null)
			{
				return;
			}
			using (this.AssertSafe(false))
			{
				for (int i = 0; i < this.graphs.Length; i++)
				{
					if (this.graphs[i] != null)
					{
						((IGraphInternals)this.graphs[i]).DestroyAllNodes();
					}
				}
			}
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0000C724 File Offset: 0x0000A924
		public void OnDestroy()
		{
			this.ClearGraphsInternal();
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0000C72C File Offset: 0x0000A92C
		public NavGraph[] DeserializeGraphs(byte[] bytes)
		{
			NavGraph[] array;
			using (this.AssertSafe(false))
			{
				this.ClearGraphs();
				array = this.DeserializeGraphsAdditive(bytes);
			}
			return array;
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0000C770 File Offset: 0x0000A970
		public NavGraph[] DeserializeGraphsAdditive(byte[] bytes)
		{
			return this.DeserializeGraphsAdditive(bytes, true);
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000C77C File Offset: 0x0000A97C
		private NavGraph[] DeserializeGraphsAdditive(byte[] bytes, bool warnIfDuplicateGuids)
		{
			NavGraph[] array2;
			using (this.AssertSafe(false))
			{
				try
				{
					if (bytes == null)
					{
						throw new ArgumentNullException("bytes");
					}
					AstarSerializer astarSerializer = new AstarSerializer(this, this.active.gameObject);
					if (!astarSerializer.OpenDeserialize(bytes))
					{
						throw new ArgumentException("Invalid data file (cannot read zip).\nThe data is either corrupt or it was saved using a 3.0.x or earlier version of the system");
					}
					NavGraph[] array = this.DeserializeGraphsPartAdditive(astarSerializer, warnIfDuplicateGuids);
					astarSerializer.CloseDeserialize();
					this.UpdateShortcuts();
					GraphModifier.TriggerEvent(GraphModifier.EventType.PostGraphLoad);
					array2 = array;
				}
				catch (Exception ex)
				{
					Debug.LogException(new Exception("Caught exception while deserializing data.", ex));
					this.graphs = new NavGraph[0];
					this.UpdateShortcuts();
					throw;
				}
				finally
				{
				}
			}
			return array2;
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0000C84C File Offset: 0x0000AA4C
		private NavGraph[] DeserializeGraphsPartAdditive(AstarSerializer sr, bool warnIfDuplicateGuids)
		{
			if (this.graphs == null)
			{
				this.graphs = new NavGraph[0];
			}
			List<NavGraph> gr = new List<NavGraph>(this.graphs);
			while (gr.Count > 0 && gr[gr.Count - 1] == null)
			{
				gr.RemoveAt(gr.Count - 1);
			}
			int count = gr.Count;
			this.FindGraphTypes();
			bool flag = this.active == AstarPath.active;
			int lastUsedGraphIndex = -1;
			NavGraph[] newGraphs = sr.DeserializeGraphs(AstarData.graphTypes, flag, delegate
			{
				int num = lastUsedGraphIndex;
				lastUsedGraphIndex = num + 1;
				while (lastUsedGraphIndex < gr.Count && gr[lastUsedGraphIndex] != null)
				{
					num = lastUsedGraphIndex;
					lastUsedGraphIndex = num + 1;
				}
				return lastUsedGraphIndex;
			});
			for (int m = 0; m < newGraphs.Length; m++)
			{
				while (gr.Count < (int)(newGraphs[m].graphIndex + 1U))
				{
					gr.Add(null);
				}
				gr[(int)newGraphs[m].graphIndex] = newGraphs[m];
			}
			if ((long)gr.Count > 255L)
			{
				throw new InvalidOperationException("Graph Count Limit Reached. You cannot have more than " + 254U.ToString() + " graphs.");
			}
			this.graphs = gr.ToArray();
			bool flag2 = false;
			int i;
			int i2;
			for (i = count; i < this.graphs.Length; i = i2 + 1)
			{
				if (this.graphs[i] != null)
				{
					this.graphs[i].GetNodes(delegate(GraphNode node)
					{
						node.GraphIndex = (uint)i;
					});
					flag2 |= this.graphs[i].isScanned;
				}
				i2 = i;
			}
			for (int j = 0; j < this.graphs.Length; j++)
			{
				for (int k = j + 1; k < this.graphs.Length; k++)
				{
					if (this.graphs[j] != null && this.graphs[k] != null && this.graphs[j].guid == this.graphs[k].guid)
					{
						if (warnIfDuplicateGuids)
						{
							Debug.LogWarning("Guid Conflict when importing graphs additively. Imported graph will get a new Guid.\nThis message is (relatively) harmless.");
						}
						this.graphs[j].guid = Guid.NewGuid();
						break;
					}
				}
			}
			sr.PostDeserialization();
			if (flag2)
			{
				this.active.AddWorkItem(delegate(IWorkItemContext ctx)
				{
					for (int l = 0; l < newGraphs.Length; l++)
					{
						if (newGraphs[l].isScanned)
						{
							ctx.DirtyBounds(newGraphs[l].bounds);
						}
					}
				});
				this.active.FlushWorkItems();
			}
			return newGraphs;
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0000CAF6 File Offset: 0x0000ACF6
		public void FindGraphTypes()
		{
			if (AstarData.graphTypes != null)
			{
				return;
			}
			AstarData.graphTypes = AssemblySearcher.FindTypesInheritingFrom<NavGraph>().ToArray();
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0000CB0F File Offset: 0x0000AD0F
		internal NavGraph CreateGraph(Type type)
		{
			NavGraph navGraph = Activator.CreateInstance(type) as NavGraph;
			navGraph.active = this.active;
			return navGraph;
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0000CB28 File Offset: 0x0000AD28
		public T AddGraph<T>() where T : NavGraph
		{
			return this.AddGraph(typeof(T)) as T;
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0000CB44 File Offset: 0x0000AD44
		public NavGraph AddGraph(Type type)
		{
			NavGraph navGraph = null;
			for (int i = 0; i < AstarData.graphTypes.Length; i++)
			{
				if (object.Equals(AstarData.graphTypes[i], type))
				{
					navGraph = this.CreateGraph(AstarData.graphTypes[i]);
				}
			}
			if (navGraph == null)
			{
				Debug.LogError(string.Concat(new string[]
				{
					"No NavGraph of type '",
					(type != null) ? type.ToString() : null,
					"' could be found, ",
					AstarData.graphTypes.Length.ToString(),
					" graph types are avaliable"
				}));
				return null;
			}
			this.AddGraph(navGraph);
			return navGraph;
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0000CBDC File Offset: 0x0000ADDC
		private void AddGraph(NavGraph graph)
		{
			using (this.AssertSafe(true))
			{
				int num = Array.IndexOf<NavGraph>(this.graphs, null);
				if (num == -1)
				{
					if ((long)this.graphs.Length >= 254L)
					{
						throw new Exception(string.Format("Graph Count Limit Reached. You cannot have more than {0} graphs.", 254U));
					}
					Memory.Realloc<NavGraph>(ref this.graphs, this.graphs.Length + 1);
					num = this.graphs.Length - 1;
				}
				this.graphs[num] = graph;
				graph.graphIndex = (uint)num;
				graph.active = this.active;
				this.UpdateShortcuts();
			}
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0000CC90 File Offset: 0x0000AE90
		public bool RemoveGraph(NavGraph graph)
		{
			if (graph == null)
			{
				throw new ArgumentNullException("graph");
			}
			bool flag;
			using (this.AssertSafe(false))
			{
				this.active.DirtyBounds(graph.bounds);
				((IGraphInternals)graph).OnDestroy();
				graph.active = null;
				int num = Array.IndexOf<NavGraph>(this.graphs, graph);
				if (num != -1)
				{
					this.graphs[num] = null;
				}
				this.UpdateShortcuts();
				if (AstarPath.active == this.active)
				{
					this.active.AddWorkItem(delegate
					{
						this.active.offMeshLinks.Refresh();
					});
					this.active.FlushWorkItems();
				}
				flag = num != -1;
			}
			return flag;
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0000CD50 File Offset: 0x0000AF50
		public NavGraph DuplicateGraph(NavGraph graph)
		{
			if (graph == null)
			{
				throw new ArgumentNullException("graph");
			}
			if (Array.IndexOf<NavGraph>(this.graphs, graph) == -1)
			{
				throw new ArgumentException("Graph doesn't exist");
			}
			byte[] array = this.SerializeGraph(SerializeSettings.Settings, graph);
			return this.DeserializeGraphsAdditive(array, false)[0];
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0000CD9C File Offset: 0x0000AF9C
		public static NavGraph GetGraph(GraphNode node)
		{
			if (node == null || node.Destroyed)
			{
				return null;
			}
			AstarPath astarPath = AstarPath.active;
			if (astarPath == null)
			{
				return null;
			}
			AstarData data = astarPath.data;
			if (data == null || data.graphs == null)
			{
				return null;
			}
			uint graphIndex = node.GraphIndex;
			return data.graphs[(int)graphIndex];
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0000CDE4 File Offset: 0x0000AFE4
		public NavGraph FindGraph(Func<NavGraph, bool> predicate)
		{
			if (this.graphs != null)
			{
				for (int i = 0; i < this.graphs.Length; i++)
				{
					if (this.graphs[i] != null && predicate(this.graphs[i]))
					{
						return this.graphs[i];
					}
				}
			}
			return null;
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0000CE30 File Offset: 0x0000B030
		public NavGraph FindGraphOfType(Type type)
		{
			return this.FindGraph((NavGraph graph) => object.Equals(graph.GetType(), type));
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0000CE5C File Offset: 0x0000B05C
		public NavGraph FindGraphWhichInheritsFrom(Type type)
		{
			return this.FindGraph((NavGraph graph) => WindowsStoreCompatibility.GetTypeInfo(type).IsAssignableFrom(WindowsStoreCompatibility.GetTypeInfo(graph.GetType())));
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000CE88 File Offset: 0x0000B088
		public IEnumerable FindGraphsOfType(Type type)
		{
			if (this.graphs == null)
			{
				yield break;
			}
			int num;
			for (int i = 0; i < this.graphs.Length; i = num + 1)
			{
				if (this.graphs[i] != null && object.Equals(this.graphs[i].GetType(), type))
				{
					yield return this.graphs[i];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000CE9F File Offset: 0x0000B09F
		public IEnumerable GetUpdateableGraphs()
		{
			if (this.graphs == null)
			{
				yield break;
			}
			int num;
			for (int i = 0; i < this.graphs.Length; i = num + 1)
			{
				if (this.graphs[i] is IUpdatableGraph)
				{
					yield return this.graphs[i];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0000CEAF File Offset: 0x0000B0AF
		public int GetGraphIndex(NavGraph graph)
		{
			if (graph == null)
			{
				throw new ArgumentNullException("graph");
			}
			if (this.graphs == null)
			{
				throw new ArgumentException("No graphs exist");
			}
			int num = Array.IndexOf<NavGraph>(this.graphs, graph);
			if (num == -1)
			{
				throw new ArgumentException("Graph doesn't exist");
			}
			return num;
		}

		// Token: 0x040001D9 RID: 473
		private AstarPath active;

		// Token: 0x040001E1 RID: 481
		[NonSerialized]
		public NavGraph[] graphs = new NavGraph[0];

		// Token: 0x040001E2 RID: 482
		[SerializeField]
		private string dataString;

		// Token: 0x040001E3 RID: 483
		public TextAsset file_cachedStartup;

		// Token: 0x040001E4 RID: 484
		[SerializeField]
		public bool cacheStartup;

		// Token: 0x040001E5 RID: 485
		private List<bool> graphStructureLocked = new List<bool>();

		// Token: 0x040001E6 RID: 486
		private static readonly ProfilerMarker MarkerLoadFromCache = new ProfilerMarker("LoadFromCache");

		// Token: 0x040001E7 RID: 487
		private static readonly ProfilerMarker MarkerDeserializeGraphs = new ProfilerMarker("DeserializeGraphs");

		// Token: 0x040001E8 RID: 488
		private static readonly ProfilerMarker MarkerSerializeGraphs = new ProfilerMarker("SerializeGraphs");

		// Token: 0x040001E9 RID: 489
		private static readonly ProfilerMarker MarkerFindGraphTypes = new ProfilerMarker("FindGraphTypes");
	}
}
