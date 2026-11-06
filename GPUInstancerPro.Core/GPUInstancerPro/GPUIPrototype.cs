using System;
using UnityEngine;
using UnityEngine.Events;

namespace GPUInstancerPro
{
	// Token: 0x02000027 RID: 39
	[Serializable]
	public class GPUIPrototype : IEquatable<GPUIPrototype>
	{
		// Token: 0x06000146 RID: 326 RVA: 0x00007AC3 File Offset: 0x00005CC3
		public GPUIPrototype(GameObject prefabObject, GPUIProfile profile)
		{
			this.prototypeType = GPUIPrototypeType.Prefab;
			this.prefabObject = prefabObject;
			this.profile = profile;
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00007AF9 File Offset: 0x00005CF9
		public GPUIPrototype(GPUILODGroupData gpuiLODGroupData, GPUIProfile profile)
		{
			this.prototypeType = GPUIPrototypeType.LODGroupData;
			this.gpuiLODGroupData = gpuiLODGroupData;
			this.profile = profile;
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00007B2F File Offset: 0x00005D2F
		public GPUIPrototype(Mesh mesh, Material[] materials, GPUIProfile profile)
		{
			this.prototypeType = GPUIPrototypeType.MeshAndMaterial;
			this.prototypeMesh = mesh;
			this.prototypeMaterials = materials;
			this.profile = profile;
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00007B6C File Offset: 0x00005D6C
		public bool IsValid(bool logError)
		{
			this.errorCode = 0;
			this.errorFixAction = null;
			if (this.profile == null)
			{
				this.profile = GPUIProfile.DefaultProfile;
			}
			switch (this.prototypeType)
			{
			case GPUIPrototypeType.Prefab:
				if (this.prefabObject == null)
				{
					if (logError)
					{
						Debug.LogError(GPUIConstants.LOG_PREFIX + ((this != null) ? this.ToString() : null) + " prefabObject is null.");
					}
					this.errorCode = 1001;
					return false;
				}
				break;
			case GPUIPrototypeType.LODGroupData:
				if (this.gpuiLODGroupData == null)
				{
					if (logError)
					{
						Debug.LogError(GPUIConstants.LOG_PREFIX + ((this != null) ? this.ToString() : null) + " gpuiLODGroupData is null.");
					}
					this.errorCode = 1002;
					return false;
				}
				break;
			case GPUIPrototypeType.MeshAndMaterial:
				if (this.prototypeMesh == null)
				{
					if (logError)
					{
						Debug.LogError(GPUIConstants.LOG_PREFIX + ((this != null) ? this.ToString() : null) + " mesh is null.");
					}
					this.errorCode = 1003;
					return false;
				}
				if (this.prototypeMaterials == null)
				{
					if (logError)
					{
						Debug.LogError(GPUIConstants.LOG_PREFIX + ((this != null) ? this.ToString() : null) + " materials is null.");
					}
					this.errorCode = 1004;
					return false;
				}
				break;
			}
			return true;
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00007CBC File Offset: 0x00005EBC
		public bool Equals(GPUIPrototype other)
		{
			if (base.Equals(other))
			{
				return true;
			}
			if (this.prototypeType == GPUIPrototypeType.Prefab && other.prototypeType == GPUIPrototypeType.Prefab && this.prefabObject != null)
			{
				return this.prefabObject.Equals(other.prefabObject);
			}
			return this.prototypeType == GPUIPrototypeType.LODGroupData && other.prototypeType == GPUIPrototypeType.LODGroupData && this.gpuiLODGroupData != null && this.gpuiLODGroupData.Equals(other.gpuiLODGroupData);
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00007D38 File Offset: 0x00005F38
		public override bool Equals(object obj)
		{
			GPUIPrototype gpuiprototype = obj as GPUIPrototype;
			if (gpuiprototype != null)
			{
				return this.Equals(gpuiprototype);
			}
			return base.Equals(obj);
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00007D60 File Offset: 0x00005F60
		public void GenerateBillboard(bool forceNew = true)
		{
			if (this.isGenerateBillboard && this.prefabObject != null)
			{
				if (this.billboardAsset == null)
				{
					this.billboardAsset = GPUIBillboardUtility.FindBillboardAsset(this.prefabObject);
					if (Application.isPlaying)
					{
						return;
					}
					if (this.billboardAsset == null)
					{
						this.billboardAsset = GPUIBillboardUtility.GenerateBillboardData(this.prefabObject);
					}
				}
				if (forceNew || this.billboardAsset.albedoAtlasTexture == null)
				{
					GPUIBillboardUtility.GenerateBillboardDelayed(this.billboardAsset, true);
				}
			}
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00007DEC File Offset: 0x00005FEC
		public int GetLODCount()
		{
			int num = 0;
			if (this.prototypeType == GPUIPrototypeType.Prefab && this.prefabObject != null)
			{
				num = this.prefabObject.GetLODCount();
				if (this.isGenerateBillboard)
				{
					if (this.billboardAsset == null)
					{
						this.billboardAsset = GPUIBillboardUtility.FindBillboardAsset(this.prefabObject);
					}
					if (this.billboardAsset != null)
					{
						num++;
					}
				}
			}
			else if (this.prototypeType == GPUIPrototypeType.LODGroupData && this.gpuiLODGroupData != null)
			{
				num = this.gpuiLODGroupData.Length;
			}
			else if (this.prototypeType == GPUIPrototypeType.MeshAndMaterial && this.prototypeMesh != null && this.prototypeMaterials != null && this.prototypeMaterials[0] != null)
			{
				num = 1;
			}
			return num;
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00007EB4 File Offset: 0x000060B4
		public override int GetHashCode()
		{
			if (this.prototypeType == GPUIPrototypeType.Prefab && this.prefabObject != null)
			{
				return this.prefabObject.GetHashCode();
			}
			if (this.prototypeType == GPUIPrototypeType.LODGroupData && this.gpuiLODGroupData != null)
			{
				return this.gpuiLODGroupData.GetHashCode();
			}
			return base.GetHashCode();
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00007F0C File Offset: 0x0000610C
		public int GetKey()
		{
			if (this.prototypeType == GPUIPrototypeType.Prefab && this.prefabObject != null)
			{
				return this.prefabObject.GetInstanceIDGPUI();
			}
			if (this.prototypeType == GPUIPrototypeType.LODGroupData && this.gpuiLODGroupData != null)
			{
				return this.gpuiLODGroupData.GetInstanceIDGPUI();
			}
			return this.GetHashCode();
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00007F64 File Offset: 0x00006164
		public Bounds GetBounds()
		{
			if (this.prototypeType == GPUIPrototypeType.Prefab && this.prefabObject != null)
			{
				return this.prefabObject.GetBounds(false);
			}
			if (this.prototypeType == GPUIPrototypeType.LODGroupData && this.gpuiLODGroupData != null)
			{
				return this.gpuiLODGroupData.bounds;
			}
			if (this.prototypeType == GPUIPrototypeType.MeshAndMaterial && this.prototypeMesh != null && this.prototypeMaterials != null && this.prototypeMaterials[0] != null)
			{
				return this.prototypeMesh.bounds;
			}
			return new Bounds(Vector3.zero, Vector3.one);
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00008004 File Offset: 0x00006204
		public override string ToString()
		{
			if (!string.IsNullOrEmpty(this.name))
			{
				return this.name;
			}
			switch (this.prototypeType)
			{
			case GPUIPrototypeType.Prefab:
				if (this.prefabObject != null)
				{
					this.name = this.prefabObject.name;
					return this.name;
				}
				break;
			case GPUIPrototypeType.LODGroupData:
				if (this.gpuiLODGroupData != null)
				{
					this.name = this.gpuiLODGroupData.ToString();
					return this.name;
				}
				break;
			case GPUIPrototypeType.MeshAndMaterial:
				if (this.prototypeMesh != null)
				{
					this.name = this.prototypeMesh.name;
					return this.name;
				}
				break;
			}
			return "[GPUIPrototype]";
		}

		// Token: 0x040000D5 RID: 213
		[SerializeField]
		public GPUIPrototypeType prototypeType;

		// Token: 0x040000D6 RID: 214
		[SerializeField]
		public GPUIProfile profile;

		// Token: 0x040000D7 RID: 215
		[SerializeField]
		public GameObject prefabObject;

		// Token: 0x040000D8 RID: 216
		[SerializeField]
		public GPUILODGroupData gpuiLODGroupData;

		// Token: 0x040000D9 RID: 217
		[SerializeField]
		public Mesh prototypeMesh;

		// Token: 0x040000DA RID: 218
		[SerializeField]
		public Material[] prototypeMaterials;

		// Token: 0x040000DB RID: 219
		[SerializeField]
		public int layer;

		// Token: 0x040000DC RID: 220
		[SerializeField]
		public bool isGenerateBillboard;

		// Token: 0x040000DD RID: 221
		[SerializeField]
		public bool isBillboardReplaceLODCulled = true;

		// Token: 0x040000DE RID: 222
		[SerializeField]
		[Range(0f, 1f)]
		public float billboardDistance = 0.9f;

		// Token: 0x040000DF RID: 223
		[SerializeField]
		public GPUIBillboard billboardAsset;

		// Token: 0x040000E0 RID: 224
		[SerializeField]
		public bool isRequireTreeProxy;

		// Token: 0x040000E1 RID: 225
		[SerializeField]
		public bool isEnabled = true;

		// Token: 0x040000E2 RID: 226
		[SerializeField]
		public string name;

		// Token: 0x040000E3 RID: 227
		[NonSerialized]
		public int errorCode;

		// Token: 0x040000E4 RID: 228
		[NonSerialized]
		public UnityAction errorFixAction;

		// Token: 0x040000E5 RID: 229
		private const int ERROR_CODE_ADDITION = 1000;

		// Token: 0x040000E6 RID: 230
		private const string DEFAULT_PROTOTYPE_NAME = "[GPUIPrototype]";
	}
}
