using System;
using System.Collections.Generic;
using Pathfinding.Collections;
using Pathfinding.Serialization;
using Pathfinding.Util;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pathfinding
{
	// Token: 0x02000111 RID: 273
	[AddComponentMenu("Pathfinding/Navmesh/RecastNavmeshModifier")]
	[DisallowMultipleComponent]
	[HelpURL("https://arongranberg.com/astar/documentation/stable/recastnavmeshmodifier.html")]
	public class RecastNavmeshModifier : VersionedMonoBehaviour, RecastMeshObj
	{
		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06000962 RID: 2402 RVA: 0x00030A7C File Offset: 0x0002EC7C
		// (set) Token: 0x06000963 RID: 2403 RVA: 0x00030ABB File Offset: 0x0002ECBB
		[Obsolete("Use mode and surfaceID instead")]
		public int area
		{
			get
			{
				switch (this.mode)
				{
				case RecastNavmeshModifier.Mode.UnwalkableSurface:
					return -1;
				case RecastNavmeshModifier.Mode.WalkableSurfaceWithSeam:
					return this.surfaceID;
				case RecastNavmeshModifier.Mode.WalkableSurfaceWithTag:
					return this.surfaceID;
				}
				return 0;
			}
			set
			{
				if (value <= -1)
				{
					this.mode = RecastNavmeshModifier.Mode.UnwalkableSurface;
				}
				if (value == 0)
				{
					this.mode = RecastNavmeshModifier.Mode.WalkableSurface;
				}
				if (value > 0)
				{
					this.mode = RecastNavmeshModifier.Mode.WalkableSurfaceWithSeam;
					this.surfaceID = value;
				}
			}
		}

		// Token: 0x06000964 RID: 2404 RVA: 0x00030AE4 File Offset: 0x0002ECE4
		private void OnEnable()
		{
			this.surfaceID = Mathf.Clamp(this.surfaceID, 0, 33554432);
			if (!this.treeKey.isValid)
			{
				this.treeKey = RecastNavmeshModifier.tree.Add(this.CalculateBounds(), this);
				if (this.dynamic)
				{
					BatchedEvents.Add<RecastNavmeshModifier>(this, BatchedEvents.Event.Custom, new Action<RecastNavmeshModifier[], int>(RecastNavmeshModifier.OnUpdate), 0);
				}
			}
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x00030B48 File Offset: 0x0002ED48
		private void OnDisable()
		{
			BatchedEvents.Remove<RecastNavmeshModifier>(this);
			Bounds bounds = RecastNavmeshModifier.tree.Remove(this.treeKey);
			this.treeKey = default(AABBTree<RecastNavmeshModifier>.Key);
			if (!this.dynamic)
			{
				Bounds bounds2 = this.CalculateBounds();
				bounds.Expand(0.001f);
				bounds2.Encapsulate(bounds);
				if ((bounds2.center - bounds.center).sqrMagnitude > 0.0001f || (bounds2.extents - bounds.extents).sqrMagnitude > 0.0001f)
				{
					string text = "The RecastNavmeshModifier has been moved or resized since it was enabled. You should set dynamic to true for moving objects, or disable the component while moving it. The bounds changed from ";
					Bounds bounds3 = bounds;
					string text2 = bounds3.ToString();
					string text3 = " to ";
					bounds3 = bounds2;
					Debug.LogError(text + text2 + text3 + bounds3.ToString(), this);
				}
			}
		}

		// Token: 0x06000966 RID: 2406 RVA: 0x00030C18 File Offset: 0x0002EE18
		private static void OnUpdate(RecastNavmeshModifier[] components, int _)
		{
			foreach (RecastNavmeshModifier recastNavmeshModifier in components)
			{
				if (recastNavmeshModifier != null && recastNavmeshModifier.transform.hasChanged)
				{
					Bounds bounds = recastNavmeshModifier.CalculateBounds();
					if (RecastNavmeshModifier.tree.GetBounds(recastNavmeshModifier.treeKey) != bounds)
					{
						RecastNavmeshModifier.tree.Move(recastNavmeshModifier.treeKey, bounds);
					}
					recastNavmeshModifier.transform.hasChanged = false;
				}
			}
		}

		// Token: 0x06000967 RID: 2407 RVA: 0x00030C8C File Offset: 0x0002EE8C
		public static void GetAllInBounds(List<RecastNavmeshModifier> buffer, Bounds bounds)
		{
			BatchedEvents.ProcessEvent<RecastNavmeshModifier>(BatchedEvents.Event.Custom);
			if (!Application.isPlaying)
			{
				RecastNavmeshModifier[] array = UnityCompatibility.FindObjectsByTypeSorted<RecastNavmeshModifier>();
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].enabled && bounds.Intersects(array[i].CalculateBounds()))
					{
						buffer.Add(array[i]);
					}
				}
				return;
			}
			if (Time.timeSinceLevelLoad == 0f)
			{
				RecastNavmeshModifier[] array2 = UnityCompatibility.FindObjectsByTypeUnsorted<RecastNavmeshModifier>();
				for (int j = 0; j < array2.Length; j++)
				{
					array2[j].OnEnable();
				}
			}
			RecastNavmeshModifier.tree.Query(bounds, buffer);
		}

		// Token: 0x06000968 RID: 2408 RVA: 0x00030D14 File Offset: 0x0002EF14
		public void ResolveMeshSource(out MeshFilter meshFilter, out Collider collider, out Collider2D collider2D)
		{
			meshFilter = null;
			collider = null;
			collider2D = null;
			switch (this.geometrySource)
			{
			case RecastNavmeshModifier.GeometrySource.Auto:
			{
				MeshRenderer meshRenderer;
				if (base.TryGetComponent<MeshRenderer>(out meshRenderer) && base.TryGetComponent<MeshFilter>(out meshFilter) && meshFilter.sharedMesh != null)
				{
					return;
				}
				if (base.TryGetComponent<Collider>(out collider))
				{
					return;
				}
				base.TryGetComponent<Collider2D>(out collider2D);
				return;
			}
			case RecastNavmeshModifier.GeometrySource.MeshFilter:
				base.TryGetComponent<MeshFilter>(out meshFilter);
				return;
			case RecastNavmeshModifier.GeometrySource.Collider:
				if (base.TryGetComponent<Collider>(out collider))
				{
					return;
				}
				base.TryGetComponent<Collider2D>(out collider2D);
				return;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x06000969 RID: 2409 RVA: 0x00030D9C File Offset: 0x0002EF9C
		private Bounds CalculateBounds()
		{
			MeshFilter meshFilter;
			Collider collider;
			Collider2D collider2D;
			this.ResolveMeshSource(out meshFilter, out collider, out collider2D);
			if (collider != null)
			{
				return collider.bounds;
			}
			if (collider2D != null)
			{
				return collider2D.bounds;
			}
			if (!(meshFilter != null))
			{
				Debug.LogError("Could not find an attached mesh source", this);
				return new Bounds(Vector3.zero, Vector3.one);
			}
			MeshRenderer meshRenderer;
			if (base.TryGetComponent<MeshRenderer>(out meshRenderer))
			{
				return meshRenderer.bounds;
			}
			Debug.LogError("Cannot use a MeshFilter as a geomtry source without a MeshRenderer attached to the same GameObject.", this);
			return new Bounds(Vector3.zero, Vector3.one);
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x00030E24 File Offset: 0x0002F024
		protected override void OnUpgradeSerializedData(ref Migrations migrations, bool unityThread)
		{
			int num;
			if (migrations.TryMigrateFromLegacyFormat(out num))
			{
				if (num == 1)
				{
					this.area = this.surfaceID;
				}
				if (num <= 2)
				{
					this.includeInScan = RecastNavmeshModifier.ScanInclusion.AlwaysInclude;
				}
				if (this.mode == (RecastNavmeshModifier.Mode)0)
				{
					this.includeInScan = RecastNavmeshModifier.ScanInclusion.AlwaysExclude;
				}
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x0600096B RID: 2411 RVA: 0x00030E65 File Offset: 0x0002F065
		// (set) Token: 0x0600096C RID: 2412 RVA: 0x00030E6D File Offset: 0x0002F06D
		bool RecastMeshObj.dynamic
		{
			get
			{
				return this.dynamic;
			}
			set
			{
				this.dynamic = value;
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x0600096D RID: 2413 RVA: 0x00030E76 File Offset: 0x0002F076
		// (set) Token: 0x0600096E RID: 2414 RVA: 0x00030E7E File Offset: 0x0002F07E
		bool RecastMeshObj.solid
		{
			get
			{
				return this.solid;
			}
			set
			{
				this.solid = value;
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x0600096F RID: 2415 RVA: 0x00030E87 File Offset: 0x0002F087
		// (set) Token: 0x06000970 RID: 2416 RVA: 0x00030E8F File Offset: 0x0002F08F
		RecastNavmeshModifier.GeometrySource RecastMeshObj.geometrySource
		{
			get
			{
				return this.geometrySource;
			}
			set
			{
				this.geometrySource = value;
			}
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06000971 RID: 2417 RVA: 0x00030E98 File Offset: 0x0002F098
		// (set) Token: 0x06000972 RID: 2418 RVA: 0x00030EA0 File Offset: 0x0002F0A0
		RecastNavmeshModifier.ScanInclusion RecastMeshObj.includeInScan
		{
			get
			{
				return this.includeInScan;
			}
			set
			{
				this.includeInScan = value;
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000973 RID: 2419 RVA: 0x00030EA9 File Offset: 0x0002F0A9
		// (set) Token: 0x06000974 RID: 2420 RVA: 0x00030EB1 File Offset: 0x0002F0B1
		int RecastMeshObj.surfaceID
		{
			get
			{
				return this.surfaceID;
			}
			set
			{
				this.surfaceID = value;
			}
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000975 RID: 2421 RVA: 0x00030EBA File Offset: 0x0002F0BA
		// (set) Token: 0x06000976 RID: 2422 RVA: 0x00030EC2 File Offset: 0x0002F0C2
		RecastNavmeshModifier.Mode RecastMeshObj.mode
		{
			get
			{
				return this.mode;
			}
			set
			{
				this.mode = value;
			}
		}

		// Token: 0x06000979 RID: 2425 RVA: 0x00030EF4 File Offset: 0x0002F0F4
		bool RecastMeshObj.get_enabled()
		{
			return base.enabled;
		}

		// Token: 0x0600097A RID: 2426 RVA: 0x00030EFC File Offset: 0x0002F0FC
		void RecastMeshObj.set_enabled(bool value)
		{
			base.enabled = value;
		}

		// Token: 0x040005E7 RID: 1511
		protected static AABBTree<RecastNavmeshModifier> tree = new AABBTree<RecastNavmeshModifier>();

		// Token: 0x040005E8 RID: 1512
		public bool dynamic = true;

		// Token: 0x040005E9 RID: 1513
		public bool solid;

		// Token: 0x040005EA RID: 1514
		public RecastNavmeshModifier.GeometrySource geometrySource;

		// Token: 0x040005EB RID: 1515
		public RecastNavmeshModifier.ScanInclusion includeInScan;

		// Token: 0x040005EC RID: 1516
		[FormerlySerializedAs("area")]
		public int surfaceID = 1;

		// Token: 0x040005ED RID: 1517
		public RecastNavmeshModifier.Mode mode = RecastNavmeshModifier.Mode.WalkableSurface;

		// Token: 0x040005EE RID: 1518
		private AABBTree<RecastNavmeshModifier>.Key treeKey;

		// Token: 0x02000112 RID: 274
		public enum ScanInclusion
		{
			// Token: 0x040005F0 RID: 1520
			Auto,
			// Token: 0x040005F1 RID: 1521
			AlwaysExclude,
			// Token: 0x040005F2 RID: 1522
			AlwaysInclude
		}

		// Token: 0x02000113 RID: 275
		public enum GeometrySource
		{
			// Token: 0x040005F4 RID: 1524
			Auto,
			// Token: 0x040005F5 RID: 1525
			MeshFilter,
			// Token: 0x040005F6 RID: 1526
			Collider
		}

		// Token: 0x02000114 RID: 276
		public enum Mode
		{
			// Token: 0x040005F8 RID: 1528
			UnwalkableSurface = 1,
			// Token: 0x040005F9 RID: 1529
			WalkableSurface,
			// Token: 0x040005FA RID: 1530
			WalkableSurfaceWithSeam,
			// Token: 0x040005FB RID: 1531
			WalkableSurfaceWithTag
		}
	}
}
