using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GPUInstancerPro
{
	// Token: 0x0200000E RID: 14
	public abstract class GPUIPrefabBase : GPUIPrefabDefinition
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600006E RID: 110 RVA: 0x000033BF File Offset: 0x000015BF
		// (set) Token: 0x0600006F RID: 111 RVA: 0x000033C7 File Offset: 0x000015C7
		public int renderKey { get; protected set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000070 RID: 112 RVA: 0x000033D0 File Offset: 0x000015D0
		// (set) Token: 0x06000071 RID: 113 RVA: 0x000033D8 File Offset: 0x000015D8
		public int bufferIndex { get; protected set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000072 RID: 114 RVA: 0x000033E1 File Offset: 0x000015E1
		public bool IsInstanced
		{
			get
			{
				return this.renderKey != 0;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000073 RID: 115 RVA: 0x000033EC File Offset: 0x000015EC
		public Transform CachedTransform
		{
			get
			{
				if (this._cachedTransform == null)
				{
					this._cachedTransform = base.transform;
				}
				return this._cachedTransform;
			}
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003410 File Offset: 0x00001610
		internal int AddOptionalRenderer(GPUIOptionalRenderer optionalRenderer)
		{
			if (this.childOptionalRenderers == null)
			{
				this.childOptionalRenderers = new List<GPUIOptionalRenderer>();
			}
			int num = this.childOptionalRenderers.IndexOf(optionalRenderer);
			if (num == -1)
			{
				num = this.childOptionalRenderers.Count;
				this.childOptionalRenderers.Add(optionalRenderer);
			}
			return num;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x0000345C File Offset: 0x0000165C
		internal void SetOptionalRendererEnabled(GPUIOptionalRenderer optionalRenderer, bool enabled)
		{
			if (optionalRenderer.optionalRendererNo <= 0)
			{
				Debug.LogWarning(GPUIConstants.LOG_PREFIX + "Optional renderer number should be a positive number. Given number: " + optionalRenderer.optionalRendererNo.ToString());
				return;
			}
			int num = optionalRenderer.optionalRendererNo - 1;
			if (optionalRenderer.optionalRendererNo > 32)
			{
				int num2 = num / 32 - 1;
				if (this.optionalRendererStatusExtra == null)
				{
					this.optionalRendererStatusExtra = new uint[num2 + 1];
				}
				else if (this.optionalRendererStatusExtra.Length <= num2)
				{
					Array.Resize<uint>(ref this.optionalRendererStatusExtra, num2 + 1);
				}
				if (enabled)
				{
					this.optionalRendererStatusExtra[num2] |= 1U << num;
				}
				else
				{
					this.optionalRendererStatusExtra[num2] &= ~(1U << num);
				}
				this.OnOptionalRendererStatusExtraChanged();
				return;
			}
			if (enabled)
			{
				this.optionalRendererStatus |= 1U << num;
			}
			else
			{
				this.optionalRendererStatus &= ~(1U << num);
			}
			this.OnOptionalRendererStatusChanged();
		}

		// Token: 0x06000076 RID: 118 RVA: 0x000024BC File Offset: 0x000006BC
		public virtual void OnOptionalRendererStatusChanged()
		{
		}

		// Token: 0x06000077 RID: 119 RVA: 0x000024BC File Offset: 0x000006BC
		public virtual void OnOptionalRendererStatusExtraChanged()
		{
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00003548 File Offset: 0x00001748
		public int GetPrefabID()
		{
			return this._prefabID;
		}

		// Token: 0x06000079 RID: 121 RVA: 0x000024BC File Offset: 0x000006BC
		public virtual void SetMaterialVariation(int index, Vector4 variationValue)
		{
		}

		// Token: 0x0400001A RID: 26
		[SerializeField]
		protected int _prefabID;

		// Token: 0x0400001B RID: 27
		[SerializeField]
		public List<GPUIOptionalRenderer> childOptionalRenderers;

		// Token: 0x0400001E RID: 30
		protected Transform _cachedTransform;

		// Token: 0x0400001F RID: 31
		public UnityAction OnInstancingStatusModified;

		// Token: 0x04000020 RID: 32
		public Action<int> OnBufferIndexModified;

		// Token: 0x04000021 RID: 33
		[NonSerialized]
		public uint optionalRendererStatus;

		// Token: 0x04000022 RID: 34
		[NonSerialized]
		public uint[] optionalRendererStatusExtra;
	}
}
