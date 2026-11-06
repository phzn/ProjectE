using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GPUInstancerPro.TerrainModule
{
	// Token: 0x02000008 RID: 8
	public abstract class GPUITerrainManager<T> : GPUIManagerWithPrototypeData<T> where T : GPUIPrototypeData, new()
	{
		// Token: 0x06000078 RID: 120 RVA: 0x00003F8A File Offset: 0x0000218A
		protected override void Awake()
		{
			base.Awake();
			if (this._gpuiTerrains == null)
			{
				this._gpuiTerrains = new List<GPUITerrain>();
			}
			this._isNullTerrainPredicate = new Predicate<GPUITerrain>(this.IsNullTerrain);
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00003FB8 File Offset: 0x000021B8
		protected override void LateUpdate()
		{
			base.LateUpdate();
			if (!base.IsInitialized)
			{
				return;
			}
			if (this._isTerrainsModified)
			{
				this.ApplyTerrainModifications();
			}
			foreach (GPUITerrain gpuiterrain in this.GetActiveTerrainValues())
			{
				if (gpuiterrain != null && gpuiterrain.IsInitialized)
				{
					gpuiterrain.NotifyTransformChanges();
				}
			}
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00004038 File Offset: 0x00002238
		public override bool IsValid(bool logError = true)
		{
			if (!base.IsValid(logError))
			{
				return false;
			}
			if (this.GetTerrainCount() == 0)
			{
				this.errorCode = -301;
				return false;
			}
			return true;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x0000405C File Offset: 0x0000225C
		public override void Initialize()
		{
			base.Initialize();
			this._isNullTerrainPredicate = new Predicate<GPUITerrain>(this.IsNullTerrain);
			this._toRemoveActiveTerrains = new HashSet<int>();
			this._gpuiTerrainsDict = new Dictionary<int, GPUITerrain>();
			this._activeTerrains = new Dictionary<int, GPUITerrain>();
			if (this._gpuiTerrains == null)
			{
				this._gpuiTerrains = new List<GPUITerrain>();
			}
			this.DeterminePrototypeIndexes();
			if (this._isAutoAddActiveTerrainsOnInitialization)
			{
				this.AddActiveTerrains();
				SceneManager.sceneLoaded += this.AddActiveTerrains;
			}
			this.UpdateActiveTerrains();
		}

		// Token: 0x0600007C RID: 124 RVA: 0x000040E0 File Offset: 0x000022E0
		public override void Dispose()
		{
			SceneManager.sceneLoaded -= this.AddActiveTerrains;
			if (this.isEnableDefaultRenderingWhenDisabled && this._activeTerrains != null)
			{
				foreach (GPUITerrain gpuiterrain in this._activeTerrains.Values)
				{
					if (gpuiterrain != null)
					{
						this.RemoveGPUITerrainManager(gpuiterrain);
					}
				}
			}
			base.Dispose();
			if (!base.IsInitialized)
			{
				return;
			}
			this._activeTerrains = null;
			this._toRemoveActiveTerrains = null;
			this._gpuiTerrainsDict = null;
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00004188 File Offset: 0x00002388
		public void RequireTerrainTransformChange(int frameCount)
		{
			if (!this._hasPendingTerrainTransformChanges)
			{
				this._hasPendingTerrainTransformChanges = true;
				this._pendingTerrainTransformChangesFrame = frameCount;
				return;
			}
			if (this._pendingTerrainTransformChangesFrame == frameCount)
			{
				return;
			}
			GPUIDetailManager gpuidetailManager = this as GPUIDetailManager;
			if (gpuidetailManager != null)
			{
				gpuidetailManager.RequireUpdate(true, false);
				return;
			}
			this.RequireUpdate();
		}

		// Token: 0x0600007E RID: 126 RVA: 0x000041D0 File Offset: 0x000023D0
		protected void CheckPendingTerrainTransformChanges()
		{
			if (this._isTerrainsModified)
			{
				this.ApplyTerrainModifications();
			}
			if (!this._hasPendingTerrainTransformChanges)
			{
				return;
			}
			Dictionary<int, GPUITerrain>.ValueCollection activeTerrainValues = this.GetActiveTerrainValues();
			if (activeTerrainValues.Count > 0)
			{
				int num = 0;
				Matrix4x4 matrix4x = GPUIConstants.IDENTITY_Matrix4x4;
				bool flag = true;
				foreach (GPUITerrain gpuiterrain in activeTerrainValues)
				{
					if (num == 0)
					{
						matrix4x = gpuiterrain.deltaLTW;
					}
					else if (!matrix4x.Approximately(gpuiterrain.deltaLTW, 0.0001f))
					{
						flag = false;
						break;
					}
					num++;
				}
				if (!flag)
				{
					GPUIDetailManager gpuidetailManager = this as GPUIDetailManager;
					if (gpuidetailManager != null)
					{
						gpuidetailManager.RequireUpdate(true, false);
					}
					else
					{
						this.RequireUpdate();
					}
				}
				else
				{
					GPUITransformBufferUtility.ApplyMatrixOffsetToTransforms(this, matrix4x);
				}
			}
			this._hasPendingTerrainTransformChanges = false;
		}

		// Token: 0x0600007F RID: 127 RVA: 0x000042A8 File Offset: 0x000024A8
		public void OnTerrainsModified()
		{
			this._isTerrainsModified = true;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x000042B1 File Offset: 0x000024B1
		private void ApplyTerrainModifications()
		{
			this.IsValid(Application.isPlaying);
			if (this._isAutoAddPrototypesBasedOnTerrains)
			{
				this.AddMissingPrototypes();
			}
			this.UpdateActiveTerrains();
			this._isTerrainsModified = false;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x000042DC File Offset: 0x000024DC
		private void UpdateActiveTerrains()
		{
			if (!base.IsInitialized)
			{
				return;
			}
			this._gpuiTerrainsDict.Clear();
			foreach (GPUITerrain gpuiterrain in this._gpuiTerrains)
			{
				if (gpuiterrain != null)
				{
					this._gpuiTerrainsDict[gpuiterrain.GetInstanceIDGPUI()] = gpuiterrain;
				}
			}
			bool flag = false;
			foreach (KeyValuePair<int, GPUITerrain> keyValuePair in this._activeTerrains)
			{
				if (!this._toRemoveActiveTerrains.Contains(keyValuePair.Key))
				{
					if (keyValuePair.Value == null)
					{
						this._toRemoveActiveTerrains.Add(keyValuePair.Key);
					}
					else if (!this._gpuiTerrainsDict.ContainsKey(keyValuePair.Key))
					{
						this._toRemoveActiveTerrains.Add(keyValuePair.Key);
						this.RemoveGPUITerrainManager(keyValuePair.Value);
					}
				}
			}
			foreach (int num in this._toRemoveActiveTerrains)
			{
				if (this._activeTerrains.ContainsKey(num))
				{
					this._activeTerrains.Remove(num);
					flag = true;
				}
			}
			this._toRemoveActiveTerrains.Clear();
			foreach (KeyValuePair<int, GPUITerrain> keyValuePair2 in this._gpuiTerrainsDict)
			{
				if (!this._activeTerrains.ContainsKey(keyValuePair2.Key))
				{
					this._activeTerrains.Add(keyValuePair2.Key, keyValuePair2.Value);
					this.SetGPUITerrainManager(keyValuePair2.Value);
					flag = true;
				}
			}
			if (flag)
			{
				this.RequireUpdate();
			}
		}

		// Token: 0x06000082 RID: 130 RVA: 0x000044EC File Offset: 0x000026EC
		public bool HasNullOrDuplicateTerrains()
		{
			return GPUIUtility.HasNullOrDuplicate<GPUITerrain>(this._gpuiTerrains);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x000044F9 File Offset: 0x000026F9
		public void RemoveNullOrDuplicateTerrains()
		{
			GPUIUtility.RemoveNullsAndDuplicates<GPUITerrain>(this._gpuiTerrains);
			this.OnTerrainsModified();
		}

		// Token: 0x06000084 RID: 132 RVA: 0x0000450C File Offset: 0x0000270C
		public void RemoveNullTerrains()
		{
			if (this._isNullTerrainPredicate == null)
			{
				this._isNullTerrainPredicate = new Predicate<GPUITerrain>(this.IsNullTerrain);
			}
			this._gpuiTerrains.RemoveAll(this._isNullTerrainPredicate);
			this.OnTerrainsModified();
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00004540 File Offset: 0x00002740
		public void AddTerrains(IEnumerable<Terrain> terrains)
		{
			if (terrains != null)
			{
				foreach (Terrain terrain in terrains)
				{
					this.AddTerrain(terrain);
				}
			}
		}

		// Token: 0x06000086 RID: 134 RVA: 0x0000458C File Offset: 0x0000278C
		public void AddTerrains(IEnumerable<GPUITerrain> terrains)
		{
			if (terrains != null)
			{
				foreach (GPUITerrain gpuiterrain in terrains)
				{
					this.AddTerrain(gpuiterrain);
				}
			}
		}

		// Token: 0x06000087 RID: 135 RVA: 0x000045D8 File Offset: 0x000027D8
		public bool AddTerrain(Terrain terrain)
		{
			if (terrain == null)
			{
				return false;
			}
			if (this._gpuiTerrains.Count == 0)
			{
				this.OnFirstTerrainAdded(terrain);
			}
			GPUITerrain gpuiterrain = terrain.GetComponent<GPUITerrain>();
			if (gpuiterrain == null)
			{
				gpuiterrain = terrain.AddOrGetComponent<GPUITerrainBuiltin>(true);
			}
			return this.AddTerrain(gpuiterrain);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00004623 File Offset: 0x00002823
		public bool AddTerrain(GPUITerrain terrain)
		{
			if (terrain == null)
			{
				return false;
			}
			if (this._gpuiTerrains.Contains(terrain))
			{
				return false;
			}
			this._gpuiTerrains.Add(terrain);
			this.OnTerrainsModified();
			return true;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00004653 File Offset: 0x00002853
		private void AddActiveTerrains(Scene arg0, LoadSceneMode arg1)
		{
			this.AddActiveTerrains();
		}

		// Token: 0x0600008A RID: 138 RVA: 0x0000465B File Offset: 0x0000285B
		public void AddActiveTerrains()
		{
			this.AddTerrains(Terrain.activeTerrains);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00004668 File Offset: 0x00002868
		public bool RemoveTerrain(Terrain terrain)
		{
			GPUITerrainBuiltin gpuiterrainBuiltin;
			return terrain.TryGetComponent<GPUITerrainBuiltin>(out gpuiterrainBuiltin) && this.RemoveTerrain(gpuiterrainBuiltin);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00004688 File Offset: 0x00002888
		public bool RemoveTerrain(GPUITerrain gpuiTerrain)
		{
			if (base.IsInitialized)
			{
				this._toRemoveActiveTerrains.Add(gpuiTerrain.GetInstanceIDGPUI());
			}
			int num = this._gpuiTerrains.IndexOf(gpuiTerrain);
			return num >= 0 && this.Internal_RemoveTerrainAtIndex(num);
		}

		// Token: 0x0600008D RID: 141 RVA: 0x000046C9 File Offset: 0x000028C9
		public bool RemoveTerrainAtIndex(int index)
		{
			return index >= 0 && this._gpuiTerrains.Count > index && this.Internal_RemoveTerrainAtIndex(index);
		}

		// Token: 0x0600008E RID: 142 RVA: 0x000046E6 File Offset: 0x000028E6
		private bool Internal_RemoveTerrainAtIndex(int index)
		{
			this._gpuiTerrains.RemoveAt(index);
			this.OnTerrainsModified();
			return true;
		}

		// Token: 0x0600008F RID: 143 RVA: 0x000046FC File Offset: 0x000028FC
		public bool ContainsTerrains(IEnumerable<Terrain> terrains)
		{
			if (terrains == null)
			{
				return true;
			}
			foreach (Terrain terrain in terrains)
			{
				GPUITerrain gpuiterrain;
				if (!this.ContainsTerrain(terrain) && (!terrain.TryGetComponent<GPUITerrain>(out gpuiterrain) || gpuiterrain is GPUITerrainBuiltin))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00004764 File Offset: 0x00002964
		public bool ContainsTerrains(IEnumerable<GPUITerrain> gpuiTerrains)
		{
			if (gpuiTerrains == null)
			{
				return true;
			}
			foreach (GPUITerrain gpuiterrain in gpuiTerrains)
			{
				if (!this.ContainsTerrain(gpuiterrain))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x000047BC File Offset: 0x000029BC
		public bool ContainsTerrain(Terrain terrain)
		{
			if (terrain == null)
			{
				return true;
			}
			foreach (GPUITerrain gpuiterrain in this._gpuiTerrains)
			{
				if (gpuiterrain != null)
				{
					GPUITerrainBuiltin gpuiterrainBuiltin = gpuiterrain as GPUITerrainBuiltin;
					if (gpuiterrainBuiltin != null && gpuiterrainBuiltin.GetTerrain() == terrain)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x0000483C File Offset: 0x00002A3C
		public bool ContainsTerrain(GPUITerrain gpuiTerrain)
		{
			return gpuiTerrain == null || this._gpuiTerrains.Contains(gpuiTerrain);
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00002599 File Offset: 0x00000799
		protected virtual void OnFirstTerrainAdded(Terrain terrain)
		{
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00004858 File Offset: 0x00002A58
		public void ResetPrototypesFromTerrains()
		{
			if (this.GetTerrainCount() == 0)
			{
				return;
			}
			bool isInitialized = base.IsInitialized;
			this.Dispose();
			this.DeterminePrototypeIndexes();
			this.RemoveUnusedPrototypes();
			this.AddMissingPrototypes();
			if (isInitialized)
			{
				this.Initialize();
			}
		}

		// Token: 0x06000095 RID: 149 RVA: 0x0000488C File Offset: 0x00002A8C
		private bool RemoveUnusedPrototypes()
		{
			int num = base.GetPrototypeCount();
			bool flag = false;
			if (num > 0)
			{
				for (int i = 0; i < num; i++)
				{
					if (!this.PrototypeHasMatchOnTerrains(i))
					{
						this.RemovePrototypeAtIndex(i);
						i--;
						num--;
						flag = true;
					}
				}
			}
			if (flag)
			{
				this.DeterminePrototypeIndexes();
			}
			return flag;
		}

		// Token: 0x06000096 RID: 150 RVA: 0x000048D8 File Offset: 0x00002AD8
		private bool AddMissingPrototypes()
		{
			bool flag = false;
			int terrainCount = this.GetTerrainCount();
			for (int i = 0; i < terrainCount; i++)
			{
				GPUITerrain terrain = this.GetTerrain(i);
				if (!(terrain == null))
				{
					flag |= this.AddMissingPrototypesFromTerrain(terrain);
				}
			}
			if (flag)
			{
				this.DeterminePrototypeIndexes();
			}
			return flag;
		}

		// Token: 0x06000097 RID: 151
		protected abstract bool AddMissingPrototypesFromTerrain(GPUITerrain gpuiTerrain);

		// Token: 0x06000098 RID: 152 RVA: 0x00004920 File Offset: 0x00002B20
		private bool PrototypeHasMatchOnTerrains(int prototypeIndex)
		{
			int terrainCount = this.GetTerrainCount();
			for (int i = 0; i < terrainCount; i++)
			{
				GPUITerrain terrain = this.GetTerrain(i);
				if (!(terrain == null) && this.GetTerrainPrototypeIndex(terrain, prototypeIndex) >= 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00004960 File Offset: 0x00002B60
		private void DeterminePrototypeIndexes()
		{
			this.BeginDeterminePrototypeIndexes();
			int terrainCount = this.GetTerrainCount();
			for (int i = 0; i < terrainCount; i++)
			{
				GPUITerrain terrain = this.GetTerrain(i);
				if (!(terrain == null))
				{
					this.DeterminePrototypeIndexes(terrain);
				}
			}
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00002599 File Offset: 0x00000799
		protected virtual void BeginDeterminePrototypeIndexes()
		{
		}

		// Token: 0x0600009B RID: 155
		protected abstract void DeterminePrototypeIndexes(GPUITerrain gpuiTerrain);

		// Token: 0x0600009C RID: 156
		protected abstract void SetGPUITerrainManager(GPUITerrain gpuiTerrain);

		// Token: 0x0600009D RID: 157
		protected abstract void RemoveGPUITerrainManager(GPUITerrain gpuiTerrain);

		// Token: 0x0600009E RID: 158
		protected abstract int[] GetTerrainPrototypeIndexes(GPUITerrain gpuiTerrain);

		// Token: 0x0600009F RID: 159 RVA: 0x000049A0 File Offset: 0x00002BA0
		protected int GetTerrainPrototypeIndex(GPUITerrain gpuiTerrain, int managerPrototypeIndex)
		{
			int[] terrainPrototypeIndexes = this.GetTerrainPrototypeIndexes(gpuiTerrain);
			if (terrainPrototypeIndexes == null)
			{
				return -1;
			}
			for (int i = 0; i < terrainPrototypeIndexes.Length; i++)
			{
				if (terrainPrototypeIndexes[i] == managerPrototypeIndex)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x000049D1 File Offset: 0x00002BD1
		public override void OnPrototypeEnabledStatusChanged(int prototypeIndex, bool isEnabled)
		{
			base.OnPrototypeEnabledStatusChanged(prototypeIndex, isEnabled);
			this.RequireUpdate();
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x000049E1 File Offset: 0x00002BE1
		public override void RemovePrototypeAtIndex(int index)
		{
			base.RemovePrototypeAtIndex(index);
			this.DeterminePrototypeIndexes();
		}

		// Token: 0x060000A2 RID: 162
		public abstract void RequireUpdate();

		// Token: 0x060000A3 RID: 163 RVA: 0x000049F0 File Offset: 0x00002BF0
		public int GetTerrainCount()
		{
			return this._gpuiTerrains.Count;
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00004A00 File Offset: 0x00002C00
		public void ReloadTerrains()
		{
			foreach (GPUITerrain gpuiterrain in this._gpuiTerrains)
			{
				if (!(gpuiterrain == null))
				{
					gpuiterrain.LoadTerrainData();
				}
			}
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00004A5C File Offset: 0x00002C5C
		public int GetActiveTerrainCount()
		{
			return this._activeTerrains.Count;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00004A69 File Offset: 0x00002C69
		public GPUITerrain GetTerrain(int terrainIndex)
		{
			if (terrainIndex < this._gpuiTerrains.Count)
			{
				return this._gpuiTerrains[terrainIndex];
			}
			return null;
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00004A87 File Offset: 0x00002C87
		public Dictionary<int, GPUITerrain>.ValueCollection GetActiveTerrainValues()
		{
			return this._activeTerrains.Values;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00004A94 File Offset: 0x00002C94
		private bool IsNullTerrain(GPUITerrain t)
		{
			return t == null;
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00004AA0 File Offset: 0x00002CA0
		public IEnumerable<TerrainLayer> GetAllTerrainLayers()
		{
			if (this._gpuiTerrains == null || this._gpuiTerrains.Count == 0)
			{
				return null;
			}
			List<TerrainLayer> list = new List<TerrainLayer>();
			foreach (GPUITerrain gpuiterrain in this._gpuiTerrains)
			{
				if (!(gpuiterrain == null))
				{
					TerrainLayer[] terrainLayers = gpuiterrain.GetTerrainLayers();
					if (terrainLayers != null && terrainLayers.Length != 0)
					{
						foreach (TerrainLayer terrainLayer in terrainLayers)
						{
							if (!list.Contains(terrainLayer))
							{
								list.Add(terrainLayer);
							}
						}
					}
				}
			}
			return list;
		}

		// Token: 0x04000029 RID: 41
		[SerializeField]
		private List<GPUITerrain> _gpuiTerrains;

		// Token: 0x0400002A RID: 42
		[SerializeField]
		protected bool _isAutoAddPrototypesBasedOnTerrains = true;

		// Token: 0x0400002B RID: 43
		[SerializeField]
		protected bool _isAutoAddActiveTerrainsOnInitialization;

		// Token: 0x0400002C RID: 44
		[NonSerialized]
		private Dictionary<int, GPUITerrain> _activeTerrains;

		// Token: 0x0400002D RID: 45
		[NonSerialized]
		protected bool _isTerrainsModified;

		// Token: 0x0400002E RID: 46
		[NonSerialized]
		private HashSet<int> _toRemoveActiveTerrains;

		// Token: 0x0400002F RID: 47
		[NonSerialized]
		private Dictionary<int, GPUITerrain> _gpuiTerrainsDict;

		// Token: 0x04000030 RID: 48
		[NonSerialized]
		protected bool _hasPendingTerrainTransformChanges;

		// Token: 0x04000031 RID: 49
		[NonSerialized]
		protected int _pendingTerrainTransformChangesFrame;

		// Token: 0x04000032 RID: 50
		private const int ERROR_CODE_ADDITION = 300;

		// Token: 0x04000033 RID: 51
		private Predicate<GPUITerrain> _isNullTerrainPredicate;
	}
}
