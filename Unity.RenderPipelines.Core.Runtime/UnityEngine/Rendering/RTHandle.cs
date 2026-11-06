using System;

namespace UnityEngine.Rendering
{
	// Token: 0x020000BA RID: 186
	public class RTHandle
	{
		// Token: 0x06000583 RID: 1411 RVA: 0x0001C9BE File Offset: 0x0001ABBE
		public void SetCustomHandleProperties(in RTHandleProperties properties)
		{
			this.m_UseCustomHandleScales = true;
			this.m_CustomHandleProperties = properties;
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x0001C9D3 File Offset: 0x0001ABD3
		public void ClearCustomHandleProperties()
		{
			this.m_UseCustomHandleScales = false;
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000585 RID: 1413 RVA: 0x0001C9DC File Offset: 0x0001ABDC
		// (set) Token: 0x06000586 RID: 1414 RVA: 0x0001C9E4 File Offset: 0x0001ABE4
		public Vector2 scaleFactor { get; internal set; }

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000587 RID: 1415 RVA: 0x0001C9ED File Offset: 0x0001ABED
		// (set) Token: 0x06000588 RID: 1416 RVA: 0x0001C9F5 File Offset: 0x0001ABF5
		public bool useScaling { get; internal set; }

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000589 RID: 1417 RVA: 0x0001C9FE File Offset: 0x0001ABFE
		// (set) Token: 0x0600058A RID: 1418 RVA: 0x0001CA06 File Offset: 0x0001AC06
		public Vector2Int referenceSize { get; internal set; }

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x0600058B RID: 1419 RVA: 0x0001CA0F File Offset: 0x0001AC0F
		public RTHandleProperties rtHandleProperties
		{
			get
			{
				if (!this.m_UseCustomHandleScales)
				{
					return this.m_Owner.rtHandleProperties;
				}
				return this.m_CustomHandleProperties;
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x0600058C RID: 1420 RVA: 0x0001CA2B File Offset: 0x0001AC2B
		public RenderTexture rt
		{
			get
			{
				return this.m_RT;
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x0600058D RID: 1421 RVA: 0x0001CA33 File Offset: 0x0001AC33
		public RenderTargetIdentifier nameID
		{
			get
			{
				return this.m_NameID;
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x0600058E RID: 1422 RVA: 0x0001CA3B File Offset: 0x0001AC3B
		public string name
		{
			get
			{
				return this.m_Name;
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x0600058F RID: 1423 RVA: 0x0001CA43 File Offset: 0x0001AC43
		public bool isMSAAEnabled
		{
			get
			{
				return this.m_EnableMSAA;
			}
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x0001CA4B File Offset: 0x0001AC4B
		internal RTHandle(RTHandleSystem owner)
		{
			this.m_Owner = owner;
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x0001CA5C File Offset: 0x0001AC5C
		public static implicit operator RenderTargetIdentifier(RTHandle handle)
		{
			if (handle == null)
			{
				return default(RenderTargetIdentifier);
			}
			return handle.nameID;
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x0001CA7C File Offset: 0x0001AC7C
		public static implicit operator Texture(RTHandle handle)
		{
			if (handle == null)
			{
				return null;
			}
			if (!(handle.rt != null))
			{
				return handle.m_ExternalTexture;
			}
			return handle.rt;
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x0001CA9E File Offset: 0x0001AC9E
		public static implicit operator RenderTexture(RTHandle handle)
		{
			if (handle == null)
			{
				return null;
			}
			return handle.rt;
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x0001CAAB File Offset: 0x0001ACAB
		internal void SetRenderTexture(RenderTexture rt)
		{
			this.m_RT = rt;
			this.m_ExternalTexture = null;
			this.m_NameID = new RenderTargetIdentifier(rt);
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x0001CAC7 File Offset: 0x0001ACC7
		internal void SetTexture(Texture tex)
		{
			this.m_RT = null;
			this.m_ExternalTexture = tex;
			this.m_NameID = new RenderTargetIdentifier(tex);
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x0001CAE3 File Offset: 0x0001ACE3
		internal void SetTexture(RenderTargetIdentifier tex)
		{
			this.m_RT = null;
			this.m_ExternalTexture = null;
			this.m_NameID = tex;
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x0001CAFC File Offset: 0x0001ACFC
		public int GetInstanceID()
		{
			if (this.m_RT != null)
			{
				return this.m_RT.GetInstanceID();
			}
			if (this.m_ExternalTexture != null)
			{
				return this.m_ExternalTexture.GetInstanceID();
			}
			return this.m_NameID.GetHashCode();
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x0001CB4E File Offset: 0x0001AD4E
		public void Release()
		{
			this.m_Owner.Remove(this);
			CoreUtils.Destroy(this.m_RT);
			this.m_NameID = BuiltinRenderTextureType.None;
			this.m_RT = null;
			this.m_ExternalTexture = null;
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x0001CB84 File Offset: 0x0001AD84
		public Vector2Int GetScaledSize(Vector2Int refSize)
		{
			if (!this.useScaling)
			{
				return refSize;
			}
			if (this.scaleFunc != null)
			{
				return this.scaleFunc(refSize);
			}
			return new Vector2Int(Mathf.RoundToInt(this.scaleFactor.x * (float)refSize.x), Mathf.RoundToInt(this.scaleFactor.y * (float)refSize.y));
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x0001CBE8 File Offset: 0x0001ADE8
		public Vector2Int GetScaledSize()
		{
			if (!this.useScaling)
			{
				return this.referenceSize;
			}
			if (this.scaleFunc != null)
			{
				return this.scaleFunc(this.referenceSize);
			}
			return new Vector2Int(Mathf.RoundToInt(this.scaleFactor.x * (float)this.referenceSize.x), Mathf.RoundToInt(this.scaleFactor.y * (float)this.referenceSize.y));
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x0001CC63 File Offset: 0x0001AE63
		public void SwitchToFastMemory(CommandBuffer cmd, float residencyFraction = 1f, FastMemoryFlags flags = FastMemoryFlags.SpillTop, bool copyContents = false)
		{
			residencyFraction = Mathf.Clamp01(residencyFraction);
			cmd.SwitchIntoFastMemory(this.m_RT, flags, residencyFraction, copyContents);
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x0001CC82 File Offset: 0x0001AE82
		public void CopyToFastMemory(CommandBuffer cmd, float residencyFraction = 1f, FastMemoryFlags flags = FastMemoryFlags.SpillTop)
		{
			this.SwitchToFastMemory(cmd, residencyFraction, flags, true);
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x0001CC8E File Offset: 0x0001AE8E
		public void SwitchOutFastMemory(CommandBuffer cmd, bool copyContents = true)
		{
			cmd.SwitchOutOfFastMemory(this.m_RT, copyContents);
		}

		// Token: 0x04000406 RID: 1030
		internal RTHandleSystem m_Owner;

		// Token: 0x04000407 RID: 1031
		internal RenderTexture m_RT;

		// Token: 0x04000408 RID: 1032
		internal Texture m_ExternalTexture;

		// Token: 0x04000409 RID: 1033
		internal RenderTargetIdentifier m_NameID;

		// Token: 0x0400040A RID: 1034
		internal bool m_EnableMSAA;

		// Token: 0x0400040B RID: 1035
		internal bool m_EnableRandomWrite;

		// Token: 0x0400040C RID: 1036
		internal bool m_EnableHWDynamicScale;

		// Token: 0x0400040D RID: 1037
		internal string m_Name;

		// Token: 0x0400040E RID: 1038
		internal bool m_UseCustomHandleScales;

		// Token: 0x0400040F RID: 1039
		internal RTHandleProperties m_CustomHandleProperties;

		// Token: 0x04000411 RID: 1041
		internal ScaleFunc scaleFunc;
	}
}
