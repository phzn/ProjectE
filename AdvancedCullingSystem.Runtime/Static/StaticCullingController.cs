using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NGS.AdvancedCullingSystem.Static
{
	// Token: 0x02000020 RID: 32
	public class StaticCullingController : MonoBehaviour
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000AA RID: 170 RVA: 0x00004E8C File Offset: 0x0000308C
		public IReadOnlyList<CameraZone> CameraZones
		{
			get
			{
				return this._cameraZones;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000AB RID: 171 RVA: 0x00004E94 File Offset: 0x00003094
		// (set) Token: 0x060000AC RID: 172 RVA: 0x00004E9C File Offset: 0x0000309C
		public int GeometryTreeDepth
		{
			get
			{
				return this._geometryTreeDepth;
			}
			set
			{
				this._geometryTreeDepth = Mathf.Clamp(value, 7, 20);
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000AD RID: 173 RVA: 0x00004EAD File Offset: 0x000030AD
		// (set) Token: 0x060000AE RID: 174 RVA: 0x00004EB5 File Offset: 0x000030B5
		public float CellSize
		{
			get
			{
				return this._cellSize;
			}
			set
			{
				this._cellSize = Mathf.Max(value, 0.1f);
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000AF RID: 175 RVA: 0x00004EC8 File Offset: 0x000030C8
		public int TotalCellsCount
		{
			get
			{
				return this._totalCellsCount;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x00004ED0 File Offset: 0x000030D0
		// (set) Token: 0x060000B1 RID: 177 RVA: 0x00004ED8 File Offset: 0x000030D8
		public float RaysPerUnit
		{
			get
			{
				return this._raysPerUnit;
			}
			set
			{
				this._raysPerUnit = Mathf.Max(0.1f, value);
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x00004EEB File Offset: 0x000030EB
		// (set) Token: 0x060000B3 RID: 179 RVA: 0x00004EF3 File Offset: 0x000030F3
		public int MaxRaysPerSource
		{
			get
			{
				return this._maxRaysPerSource;
			}
			set
			{
				this._maxRaysPerSource = Mathf.Max(10, value);
			}
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00004F03 File Offset: 0x00003103
		public bool AddCameraZone(CameraZone zone)
		{
			if (zone == null)
			{
				return false;
			}
			if (this._cameraZones == null)
			{
				this._cameraZones = new List<CameraZone>();
			}
			if (this._cameraZones.Contains(zone))
			{
				return false;
			}
			this._cameraZones.Add(zone);
			return true;
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00004F40 File Offset: 0x00003140
		public bool RemoveCameraZone(CameraZone zone)
		{
			return !(zone == null) && this._cameraZones != null && this._cameraZones.Contains(zone) && this._cameraZones.Remove(zone);
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00004F74 File Offset: 0x00003174
		public void CreatePreviewGeometryTree()
		{
			string text;
			if (!this.ReadyToCreateGeometryTree(out text))
			{
				Debug.Log("Unable to create GeometryTree : " + text);
				return;
			}
			List<StaticCullingSource> list = new List<StaticCullingSource>();
			foreach (StaticCullingSource staticCullingSource in Object.FindObjectsOfType<StaticCullingSource>())
			{
				if (staticCullingSource.Validate())
				{
					staticCullingSource.PrepareForBaking();
					list.Add(staticCullingSource);
				}
			}
			this.CreateGeometryTree(list, out text);
			foreach (StaticCullingSource staticCullingSource2 in list)
			{
				staticCullingSource2.ClearAfterBaking();
				Object.DestroyImmediate(staticCullingSource2.gameObject.GetComponent<CullingTarget>());
			}
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x0000502C File Offset: 0x0000322C
		public void CreatePreviewCameraZones()
		{
			string text;
			if (!this.ReadyToBakeCameraZones(out text))
			{
				Debug.Log("Unable to bake camera zones : " + text);
				return;
			}
			if (!this.CreateVisibilityTrees(out text))
			{
				Debug.Log("Unable to bake camera zones : " + text);
			}
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00005070 File Offset: 0x00003270
		public void Bake()
		{
			string text;
			if (!this.ReadyToBake(out text))
			{
				Debug.Log("Unable to bake scene : " + text);
				return;
			}
			List<StaticCullingSource> list;
			if (!this.PrepareForBake(out list, out text))
			{
				Debug.Log("Baking process aborted");
				Debug.Log("Reason : " + text);
				this.ClearBakedData();
			}
			if (!this.CreateGeometryTree(list, out text))
			{
				Debug.Log("Baking process aborted");
				Debug.Log("Reason : " + text);
				this.ClearBakedData();
			}
			if (!this.CreateVisibilityTrees(out text))
			{
				Debug.Log("Baking process aborted");
				Debug.Log("Reason : " + text);
				this.ClearBakedData();
			}
			if (this.BakeScene(this._geometryTree, out text))
			{
				this.ClearAfterBaking(list);
				Debug.Log("Scene sucessfully baked!");
				return;
			}
			Debug.Log("Baking process aborted");
			Debug.Log("Reason : " + text);
			this.ClearBakedData();
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x0000515B File Offset: 0x0000335B
		public void Clear()
		{
			this.ClearBakedData();
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00005164 File Offset: 0x00003364
		private bool ReadyToCreateGeometryTree(out string error)
		{
			StaticCullingSource[] array = Object.FindObjectsOfType<StaticCullingSource>();
			if (array == null || array.Length == 0)
			{
				error = "StaticCullingSources not found. Add in 'Step 1'";
				return false;
			}
			StaticCullingSource[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				if (array2[i].Validate())
				{
					error = "";
					return true;
				}
			}
			error = "Valid StaticCullingSources not found. Check in 'Step 1'";
			return false;
		}

		// Token: 0x060000BB RID: 187 RVA: 0x000051B4 File Offset: 0x000033B4
		private bool ReadyToBakeCameraZones(out string error)
		{
			if (this._cameraZones == null || this._cameraZones.Count == 0)
			{
				error = "Camera Zones not added. Add in 'Step 3'";
				return false;
			}
			int i = 0;
			while (i < this._cameraZones.Count)
			{
				if (this._cameraZones[i] == null)
				{
					this._cameraZones.RemoveAt(i);
				}
				else
				{
					i++;
				}
			}
			if (this._cameraZones.Count == 0)
			{
				error = "Camera Zones not added. Add in 'Step 3'";
				return false;
			}
			error = "";
			return true;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00005235 File Offset: 0x00003435
		private bool ReadyToBake(out string error)
		{
			if (!this.ReadyToCreateGeometryTree(out error))
			{
				return false;
			}
			if (!this.ReadyToBakeCameraZones(out error))
			{
				return false;
			}
			error = "";
			return true;
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00005258 File Offset: 0x00003458
		private bool PrepareForBake(out List<StaticCullingSource> sources, out string error)
		{
			sources = new List<StaticCullingSource>();
			bool flag;
			try
			{
				foreach (StaticCullingSource staticCullingSource in Object.FindObjectsOfType<StaticCullingSource>())
				{
					if (staticCullingSource.Validate())
					{
						staticCullingSource.PrepareForBaking();
						sources.Add(staticCullingSource);
					}
				}
				error = "";
				flag = true;
			}
			catch (Exception ex)
			{
				error = ex.Message + ex.StackTrace;
				flag = false;
			}
			return flag;
		}

		// Token: 0x060000BE RID: 190 RVA: 0x000052D4 File Offset: 0x000034D4
		private bool CreateGeometryTree(List<StaticCullingSource> sources, out string error)
		{
			bool flag;
			try
			{
				this._geometryTree = new GeometryTree(sources.Select<StaticCullingSource, CullingTarget>((StaticCullingSource s) => s.CullingTarget).ToArray<CullingTarget>(), this._geometryTreeDepth);
				error = "";
				flag = true;
			}
			catch (Exception ex)
			{
				error = ex.Message + ex.StackTrace;
				flag = false;
			}
			return flag;
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00005350 File Offset: 0x00003550
		private bool CreateVisibilityTrees(out string error)
		{
			bool flag;
			try
			{
				this._totalCellsCount = 0;
				foreach (CameraZone cameraZone in this._cameraZones)
				{
					if (cameraZone != null)
					{
						cameraZone.ClearVisibilityTree();
						cameraZone.CreateVisibilityTree(this._cellSize);
						this._totalCellsCount += cameraZone.CellsCount;
					}
				}
				error = "";
				flag = true;
			}
			catch (Exception ex)
			{
				error = ex.Message + ex.StackTrace;
				flag = false;
			}
			return flag;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00005404 File Offset: 0x00003604
		private bool BakeScene(GeometryTree geometryTree, out string error)
		{
			StaticCullingBaker staticCullingBaker = new StaticCullingBaker(geometryTree);
			error = "";
			bool flag = false;
			foreach (CameraZone cameraZone in this._cameraZones)
			{
				if (!staticCullingBaker.Bake(cameraZone.VisibilityTree, this._raysPerUnit, this._maxRaysPerSource, out error))
				{
					flag = true;
					break;
				}
			}
			staticCullingBaker.Dispose();
			return !flag;
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x0000548C File Offset: 0x0000368C
		private void ClearAfterBaking(List<StaticCullingSource> sources)
		{
			foreach (StaticCullingSource staticCullingSource in sources)
			{
				staticCullingSource.ClearAfterBaking();
				Object.DestroyImmediate(staticCullingSource);
			}
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x000054E0 File Offset: 0x000036E0
		private void ClearBakedData()
		{
			StaticCullingSource[] array = Object.FindObjectsOfType<StaticCullingSource>();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].ClearAfterBaking();
			}
			int num = 0;
			foreach (CullingTarget cullingTarget in Object.FindObjectsOfType<CullingTarget>())
			{
				cullingTarget.gameObject.AddComponent<StaticCullingSource>();
				Object.DestroyImmediate(cullingTarget);
				num++;
			}
			int num2 = 0;
			if (this._cameraZones != null)
			{
				foreach (CameraZone cameraZone in this._cameraZones)
				{
					if (cameraZone != null)
					{
						cameraZone.ClearVisibilityTree();
						num2++;
					}
				}
			}
			Debug.Log("Cleared Culling Targets : " + num.ToString());
			Debug.Log("Cleared Camera Zones : " + num2.ToString());
		}

		// Token: 0x040000A3 RID: 163
		[SerializeField]
		private List<CameraZone> _cameraZones;

		// Token: 0x040000A4 RID: 164
		[SerializeField]
		private GeometryTree _geometryTree;

		// Token: 0x040000A5 RID: 165
		[SerializeField]
		private int _geometryTreeDepth = 11;

		// Token: 0x040000A6 RID: 166
		[SerializeField]
		private float _cellSize = 5f;

		// Token: 0x040000A7 RID: 167
		[SerializeField]
		private int _totalCellsCount;

		// Token: 0x040000A8 RID: 168
		[SerializeField]
		private float _raysPerUnit = 10f;

		// Token: 0x040000A9 RID: 169
		[SerializeField]
		private int _maxRaysPerSource = 300;
	}
}
