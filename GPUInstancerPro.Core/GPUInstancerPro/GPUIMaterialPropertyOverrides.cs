using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUInstancerPro
{
	// Token: 0x0200001F RID: 31
	public class GPUIMaterialPropertyOverrides
	{
		// Token: 0x06000104 RID: 260 RVA: 0x00006684 File Offset: 0x00004884
		public void AddOverride(int lodIndex, int rendererIndex, int nameID, object value, bool isPersistent, bool isAppliedDirectlyToMBP)
		{
			if (this._overrides == null)
			{
				this._overrides = new List<GPUIMaterialPropertyOverrides.GPUIMaterialPropertyOverride>();
			}
			GPUIMaterialPropertyOverrides.GPUIMaterialPropertyOverride gpuimaterialPropertyOverride = new GPUIMaterialPropertyOverrides.GPUIMaterialPropertyOverride
			{
				lodIndex = lodIndex,
				rendererIndex = rendererIndex,
				nameID = nameID,
				value = value,
				isPersistent = isPersistent,
				isAppliedDirectlyToMBP = isAppliedDirectlyToMBP
			};
			int num = this._overrides.IndexOf(gpuimaterialPropertyOverride);
			if (num != -1)
			{
				this._overrides[num] = gpuimaterialPropertyOverride;
				return;
			}
			this._overrides.Add(gpuimaterialPropertyOverride);
		}

		// Token: 0x06000105 RID: 261 RVA: 0x0000670C File Offset: 0x0000490C
		public void RemoveMaterialPropertyOverrides(int nameID)
		{
			if (this._overrides == null)
			{
				return;
			}
			for (int i = 0; i < this._overrides.Count; i++)
			{
				GPUIMaterialPropertyOverrides.GPUIMaterialPropertyOverride gpuimaterialPropertyOverride = this._overrides[i];
				if (!gpuimaterialPropertyOverride.isPersistent && gpuimaterialPropertyOverride.nameID == nameID)
				{
					this._overrides.RemoveAt(i);
					i--;
				}
			}
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00006768 File Offset: 0x00004968
		public void ClearOverrides()
		{
			if (this._overrides == null)
			{
				return;
			}
			for (int i = 0; i < this._overrides.Count; i++)
			{
				if (!this._overrides[i].isPersistent)
				{
					this._overrides.RemoveAt(i);
					i--;
				}
			}
		}

		// Token: 0x06000107 RID: 263 RVA: 0x000067B8 File Offset: 0x000049B8
		public void ApplyOverrides(MaterialPropertyBlock mpb, int lodIndex, int rendererIndex)
		{
			if (this._overrides == null)
			{
				return;
			}
			for (int i = 0; i < this._overrides.Count; i++)
			{
				GPUIMaterialPropertyOverrides.GPUIMaterialPropertyOverride gpuimaterialPropertyOverride = this._overrides[i];
				if (!gpuimaterialPropertyOverride.isAppliedDirectlyToMBP && gpuimaterialPropertyOverride.lodIndex == lodIndex && gpuimaterialPropertyOverride.rendererIndex == rendererIndex)
				{
					gpuimaterialPropertyOverride.ApplyOverride(mpb);
				}
			}
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00006814 File Offset: 0x00004A14
		public void ApplyDirectOverrides(MaterialPropertyBlock mpb)
		{
			if (this._overrides == null)
			{
				return;
			}
			for (int i = 0; i < this._overrides.Count; i++)
			{
				GPUIMaterialPropertyOverrides.GPUIMaterialPropertyOverride gpuimaterialPropertyOverride = this._overrides[i];
				if (gpuimaterialPropertyOverride.isAppliedDirectlyToMBP)
				{
					gpuimaterialPropertyOverride.ApplyOverride(mpb);
				}
			}
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00006860 File Offset: 0x00004A60
		public object GetOverrideValue(int lodIndex, int rendererIndex, int nameID)
		{
			if (this._overrides == null)
			{
				return null;
			}
			for (int i = 0; i < this._overrides.Count; i++)
			{
				GPUIMaterialPropertyOverrides.GPUIMaterialPropertyOverride gpuimaterialPropertyOverride = this._overrides[i];
				if (gpuimaterialPropertyOverride.lodIndex == lodIndex && gpuimaterialPropertyOverride.rendererIndex == rendererIndex && gpuimaterialPropertyOverride.nameID == nameID)
				{
					return gpuimaterialPropertyOverride.value;
				}
			}
			return null;
		}

		// Token: 0x0600010A RID: 266 RVA: 0x000068BD File Offset: 0x00004ABD
		public int GetOverrideCount()
		{
			if (this._overrides == null)
			{
				return 0;
			}
			return this._overrides.Count;
		}

		// Token: 0x0400008B RID: 139
		private List<GPUIMaterialPropertyOverrides.GPUIMaterialPropertyOverride> _overrides;

		// Token: 0x02000020 RID: 32
		internal struct GPUIMaterialPropertyOverride : IEquatable<GPUIMaterialPropertyOverrides.GPUIMaterialPropertyOverride>
		{
			// Token: 0x0600010C RID: 268 RVA: 0x000068D4 File Offset: 0x00004AD4
			public void ApplyOverride(MaterialPropertyBlock mpb)
			{
				if (this.value == null)
				{
					return;
				}
				mpb.SetValue(this.nameID, this.value);
			}

			// Token: 0x0600010D RID: 269 RVA: 0x000068F1 File Offset: 0x00004AF1
			public override int GetHashCode()
			{
				return GPUIUtility.GenerateHash(new int[]
				{
					this.lodIndex + 1,
					this.rendererIndex + 1,
					this.nameID
				});
			}

			// Token: 0x0600010E RID: 270 RVA: 0x00006920 File Offset: 0x00004B20
			public bool Equals(GPUIMaterialPropertyOverrides.GPUIMaterialPropertyOverride other)
			{
				return this.lodIndex == other.lodIndex && this.rendererIndex == other.rendererIndex && this.nameID == other.nameID && this.isPersistent == other.isPersistent && this.isAppliedDirectlyToMBP == other.isAppliedDirectlyToMBP;
			}

			// Token: 0x0600010F RID: 271 RVA: 0x00006978 File Offset: 0x00004B78
			public override bool Equals(object obj)
			{
				if (obj is GPUIMaterialPropertyOverrides.GPUIMaterialPropertyOverride)
				{
					GPUIMaterialPropertyOverrides.GPUIMaterialPropertyOverride gpuimaterialPropertyOverride = (GPUIMaterialPropertyOverrides.GPUIMaterialPropertyOverride)obj;
					return this.Equals(gpuimaterialPropertyOverride);
				}
				return base.Equals(obj);
			}

			// Token: 0x0400008C RID: 140
			public int lodIndex;

			// Token: 0x0400008D RID: 141
			public int rendererIndex;

			// Token: 0x0400008E RID: 142
			public int nameID;

			// Token: 0x0400008F RID: 143
			public object value;

			// Token: 0x04000090 RID: 144
			public bool isPersistent;

			// Token: 0x04000091 RID: 145
			public bool isAppliedDirectlyToMBP;
		}
	}
}
