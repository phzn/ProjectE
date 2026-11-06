using System;
using Pathfinding.Graphs.Navmesh;
using Pathfinding.Jobs;
using Pathfinding.Serialization;
using Pathfinding.Sync;
using Pathfinding.Util;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pathfinding
{
	// Token: 0x0200007E RID: 126
	[AddComponentMenu("Pathfinding/Navmesh Prefab")]
	[HelpURL("https://arongranberg.com/astar/documentation/stable/navmeshprefab.html")]
	public class NavmeshPrefab : VersionedMonoBehaviour
	{
		// Token: 0x06000470 RID: 1136 RVA: 0x00016478 File Offset: 0x00014678
		protected override void Reset()
		{
			base.Reset();
			AstarPath.FindAstarPath();
			if (AstarPath.active != null && AstarPath.active.data.recastGraph != null)
			{
				RecastGraph recastGraph = AstarPath.active.data.recastGraph;
				this.bounds = new Bounds(Vector3.zero, new Vector3(recastGraph.TileWorldSizeX, recastGraph.forcedBoundsSize.y, recastGraph.TileWorldSizeZ));
			}
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x000164EA File Offset: 0x000146EA
		[ContextMenu("Snap to closest tile alignment")]
		public void SnapToClosestTileAlignment()
		{
			AstarPath.FindAstarPath();
			if (AstarPath.active != null && AstarPath.active.data.recastGraph != null)
			{
				this.SnapToClosestTileAlignment(AstarPath.active.data.recastGraph);
			}
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x00016524 File Offset: 0x00014724
		[ContextMenu("Apply here")]
		public void Apply()
		{
			AstarPath.FindAstarPath();
			if (AstarPath.active != null && AstarPath.active.data.recastGraph != null)
			{
				RecastGraph recastGraph = AstarPath.active.data.recastGraph;
				this.Apply(recastGraph);
			}
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x0001656C File Offset: 0x0001476C
		public void SnapToClosestTileAlignment(RecastGraph graph)
		{
			TileLayout tileLayout = TileLayout.FromGraph(graph);
			IntRect intRect;
			int num;
			float num2;
			NavmeshPrefab.SnapToGraph(tileLayout, base.transform.position, base.transform.rotation, this.bounds, out intRect, out num, out num2);
			Bounds tileBoundsInGraphSpace = tileLayout.GetTileBoundsInGraphSpace(intRect.xmin, intRect.ymin, intRect.Width, intRect.Height);
			Vector3 vector = new Vector3(tileBoundsInGraphSpace.center.x, num2, tileBoundsInGraphSpace.center.z);
			base.transform.rotation = Quaternion.Euler(graph.rotation) * Quaternion.Euler(0f, (float)(num * 90), 0f);
			base.transform.position = tileLayout.transform.Transform(vector) + base.transform.rotation * (-this.bounds.center + new Vector3(0f, this.bounds.extents.y, 0f));
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x0001667B File Offset: 0x0001487B
		public void SnapSizeToClosestTileMultiple(RecastGraph graph)
		{
			this.bounds = NavmeshPrefab.SnapSizeToClosestTileMultiple(graph, this.bounds);
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x00016690 File Offset: 0x00014890
		private void Start()
		{
			this.startHasRun = true;
			if (this.applyOnStart && this.serializedNavmesh != null && AstarPath.active != null && AstarPath.active.data.recastGraph != null)
			{
				this.Apply(AstarPath.active.data.recastGraph);
			}
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x000166F0 File Offset: 0x000148F0
		private void OnEnable()
		{
			if (this.startHasRun && this.applyOnStart && this.serializedNavmesh != null && AstarPath.active != null && AstarPath.active.data.recastGraph != null)
			{
				this.Apply(AstarPath.active.data.recastGraph);
			}
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x00016750 File Offset: 0x00014950
		private void OnDisable()
		{
			if (this.removeTilesWhenDisabled && this.serializedNavmesh != null && AstarPath.active != null)
			{
				Vector3 pos = base.transform.position;
				Quaternion rot = base.transform.rotation;
				AstarPath.active.AddWorkItem(delegate(IWorkItemContext ctx)
				{
					RecastGraph recastGraph = AstarPath.active.data.recastGraph;
					if (recastGraph != null)
					{
						IntRect intRect;
						int num;
						float num2;
						NavmeshPrefab.SnapToGraph(TileLayout.FromGraph(recastGraph), pos, rot, this.bounds, out intRect, out num, out num2);
						recastGraph.ClearTiles(intRect);
					}
				});
			}
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x000167C8 File Offset: 0x000149C8
		public static Bounds SnapSizeToClosestTileMultiple(RecastGraph graph, Bounds bounds)
		{
			float num = Mathf.Max((float)graph.editorTileSize * graph.cellSize, 0.001f);
			Vector2 vector = new Vector2(bounds.size.x / num, bounds.size.z / num);
			Vector2Int vector2Int = new Vector2Int(Mathf.Max(1, Mathf.RoundToInt(vector.x)), Mathf.Max(1, Mathf.RoundToInt(vector.y)));
			return new Bounds(bounds.center, new Vector3((float)vector2Int.x * num, bounds.size.y, (float)vector2Int.y * num));
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x0001686C File Offset: 0x00014A6C
		public static void SnapToGraph(TileLayout tileLayout, Vector3 position, Quaternion rotation, Bounds bounds, out IntRect tileRect, out int snappedRotation, out float yOffset)
		{
			Vector3 vector = tileLayout.transform.InverseTransformVector(rotation * Vector3.right);
			snappedRotation = -Mathf.RoundToInt(Mathf.Atan2(vector.z, vector.x) / 1.5707964f);
			Quaternion quaternion = Quaternion.Euler(0f, (float)(snappedRotation * 90), 0f);
			Matrix4x4 matrix4x = tileLayout.transform.inverseMatrix * Matrix4x4.TRS(position + quaternion * bounds.center, quaternion, Vector3.one);
			Vector3 vector2 = matrix4x.MultiplyPoint3x4(-bounds.extents);
			Vector3 vector3 = matrix4x.MultiplyPoint3x4(bounds.extents);
			Vector3 vector4 = Vector3.Min(vector2, vector3);
			Vector3 vector5 = Vector3.Scale(vector4, new Vector3(1f / tileLayout.TileWorldSizeX, 1f, 1f / tileLayout.TileWorldSizeZ));
			Vector2Int vector2Int = new Vector2Int(Mathf.RoundToInt(vector5.x), Mathf.RoundToInt(vector5.z));
			Vector2 vector6 = new Vector2(bounds.size.x, bounds.size.z);
			if ((snappedRotation % 2 + 2) % 2 == 1)
			{
				Memory.Swap<float>(ref vector6.x, ref vector6.y);
			}
			int num = Mathf.Max(1, Mathf.RoundToInt(vector6.x / tileLayout.TileWorldSizeX));
			int num2 = Mathf.Max(1, Mathf.RoundToInt(vector6.y / tileLayout.TileWorldSizeZ));
			tileRect = new IntRect(vector2Int.x, vector2Int.y, vector2Int.x + num - 1, vector2Int.y + num2 - 1);
			yOffset = vector4.y;
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x00016A20 File Offset: 0x00014C20
		public void Apply(RecastGraph graph)
		{
			if (this.serializedNavmesh == null)
			{
				throw new InvalidOperationException("Cannot Apply NavmeshPrefab because no serialized data has been set");
			}
			AstarPath.active.AddWorkItem(delegate
			{
				IntRect intRect;
				int num;
				float num2;
				NavmeshPrefab.SnapToGraph(TileLayout.FromGraph(graph), this.transform.position, this.transform.rotation, this.bounds, out intRect, out num, out num2);
				TileMeshes tileMeshes = TileMeshes.Deserialize(this.serializedNavmesh.bytes);
				tileMeshes.Rotate(num);
				if (tileMeshes.tileRect.Width != intRect.Width || tileMeshes.tileRect.Height != intRect.Height)
				{
					throw new Exception(string.Concat(new string[]
					{
						"NavmeshPrefab has been scanned with a different size than it is right now (or with a different graph). Expected to find ",
						intRect.Width.ToString(),
						"x",
						intRect.Height.ToString(),
						" tiles, but found ",
						tileMeshes.tileRect.Width.ToString(),
						"x",
						tileMeshes.tileRect.Height.ToString()
					}));
				}
				tileMeshes.tileRect = intRect;
				graph.ReplaceTiles(tileMeshes, num2);
			});
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x00016A70 File Offset: 0x00014C70
		public byte[] Scan()
		{
			AstarPath.FindAstarPath();
			if (AstarPath.active == null || AstarPath.active.data.recastGraph == null)
			{
				throw new InvalidOperationException("There's no recast graph in the scene. Add one if you want to scan this navmesh prefab.");
			}
			return this.Scan(AstarPath.active.data.recastGraph);
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x00016AC0 File Offset: 0x00014CC0
		public byte[] Scan(RecastGraph graph)
		{
			NavmeshPrefab.SerializedOutput serializedOutput = this.ScanAsync(graph).Complete();
			byte[] data = serializedOutput.data;
			serializedOutput.Dispose();
			return data;
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x00016AEC File Offset: 0x00014CEC
		public Promise<NavmeshPrefab.SerializedOutput> ScanAsync(RecastGraph graph)
		{
			DisposeArena disposeArena = new DisposeArena();
			TileLayout tileLayout = new TileLayout(new Bounds(base.transform.position + base.transform.rotation * this.bounds.center, this.bounds.size), base.transform.rotation, graph.cellSize, graph.editorTileSize, graph.useTiles);
			tileLayout.graphSpaceSize.x = float.PositiveInfinity;
			tileLayout.graphSpaceSize.z = float.PositiveInfinity;
			TileBuilder tileBuilder = RecastBuilder.BuildTileMeshes(graph, tileLayout, new IntRect(0, 0, tileLayout.tileCount.x - 1, tileLayout.tileCount.y - 1));
			Scene scene = base.gameObject.scene;
			tileBuilder.collectionSettings.physicsScene = new PhysicsScene?(scene.GetPhysicsScene());
			tileBuilder.collectionSettings.physicsScene2D = new PhysicsScene2D?(scene.GetPhysicsScene2D());
			Promise<TileBuilder.TileBuilderOutput> promise = tileBuilder.Schedule(disposeArena);
			NavmeshPrefab.SerializedOutput serializedOutput = new NavmeshPrefab.SerializedOutput
			{
				promise = promise,
				arena = disposeArena
			};
			return new Promise<NavmeshPrefab.SerializedOutput>(new NavmeshPrefab.SerializeJob
			{
				tileMeshesPromise = promise,
				output = serializedOutput
			}.ScheduleManaged(promise.handle), serializedOutput);
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x00016C34 File Offset: 0x00014E34
		protected override void OnUpgradeSerializedData(ref Migrations migrations, bool unityThread)
		{
			int num;
			migrations.TryMigrateFromLegacyFormat(out num);
			if (migrations.AddAndMaybeRunMigration(1, true))
			{
				this.removeTilesWhenDisabled = false;
			}
			base.OnUpgradeSerializedData(ref migrations, unityThread);
		}

		// Token: 0x040002CC RID: 716
		public TextAsset serializedNavmesh;

		// Token: 0x040002CD RID: 717
		public bool applyOnStart = true;

		// Token: 0x040002CE RID: 718
		public bool removeTilesWhenDisabled = true;

		// Token: 0x040002CF RID: 719
		public Bounds bounds = new Bounds(Vector3.zero, new Vector3(10f, 10f, 10f));

		// Token: 0x040002D0 RID: 720
		private bool startHasRun;

		// Token: 0x0200007F RID: 127
		public class SerializedOutput : IProgress, IDisposable
		{
			// Token: 0x170000E9 RID: 233
			// (get) Token: 0x06000480 RID: 1152 RVA: 0x00016C9D File Offset: 0x00014E9D
			public float Progress
			{
				get
				{
					return this.promise.Progress;
				}
			}

			// Token: 0x06000481 RID: 1153 RVA: 0x00016CAA File Offset: 0x00014EAA
			public void Dispose()
			{
				this.promise.Dispose();
				this.arena.DisposeAll();
			}

			// Token: 0x040002D1 RID: 721
			public Promise<TileBuilder.TileBuilderOutput> promise;

			// Token: 0x040002D2 RID: 722
			public byte[] data;

			// Token: 0x040002D3 RID: 723
			public DisposeArena arena;
		}

		// Token: 0x02000080 RID: 128
		private struct SerializeJob : IJob
		{
			// Token: 0x06000483 RID: 1155 RVA: 0x00016CC4 File Offset: 0x00014EC4
			public void Execute()
			{
				TileBuilder.TileBuilderOutput value = this.tileMeshesPromise.GetValue();
				this.output.data = value.tileMeshes.ToManaged().Serialize();
			}

			// Token: 0x040002D4 RID: 724
			public Promise<TileBuilder.TileBuilderOutput> tileMeshesPromise;

			// Token: 0x040002D5 RID: 725
			public NavmeshPrefab.SerializedOutput output;
		}
	}
}
