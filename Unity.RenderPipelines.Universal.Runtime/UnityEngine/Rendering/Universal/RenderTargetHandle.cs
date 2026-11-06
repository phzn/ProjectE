using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000CD RID: 205
	[Obsolete("Deprecated in favor of RTHandle")]
	public struct RenderTargetHandle
	{
		// Token: 0x1700018F RID: 399
		// (get) Token: 0x0600066B RID: 1643 RVA: 0x00027399 File Offset: 0x00025599
		// (set) Token: 0x0600066A RID: 1642 RVA: 0x00027390 File Offset: 0x00025590
		public int id { readonly get; set; }

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x0600066D RID: 1645 RVA: 0x000273AA File Offset: 0x000255AA
		// (set) Token: 0x0600066C RID: 1644 RVA: 0x000273A1 File Offset: 0x000255A1
		private RenderTargetIdentifier rtid { readonly get; set; }

		// Token: 0x0600066E RID: 1646 RVA: 0x000273B2 File Offset: 0x000255B2
		public RenderTargetHandle(RenderTargetIdentifier renderTargetIdentifier)
		{
			this.id = -2;
			this.rtid = renderTargetIdentifier;
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x000273C4 File Offset: 0x000255C4
		public RenderTargetHandle(RTHandle rtHandle)
		{
			if (rtHandle.nameID == BuiltinRenderTextureType.CameraTarget)
			{
				this.id = -1;
			}
			else if (rtHandle.name.Length == 0)
			{
				this.id = -2;
			}
			else
			{
				this.id = Shader.PropertyToID(rtHandle.name);
			}
			this.rtid = rtHandle.nameID;
			if (rtHandle.rt != null && this.id != this.rtid)
			{
				this.id = -2;
			}
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x0002744F File Offset: 0x0002564F
		internal static RenderTargetHandle GetCameraTarget(ref CameraData cameraData)
		{
			if (cameraData.xr.enabled)
			{
				return new RenderTargetHandle(cameraData.xr.renderTarget);
			}
			return RenderTargetHandle.CameraTarget;
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x00027474 File Offset: 0x00025674
		public void Init(string shaderProperty)
		{
			this.id = Shader.PropertyToID(shaderProperty);
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x00027482 File Offset: 0x00025682
		public void Init(RenderTargetIdentifier renderTargetIdentifier)
		{
			this.id = -2;
			this.rtid = renderTargetIdentifier;
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x00027493 File Offset: 0x00025693
		public RenderTargetIdentifier Identifier()
		{
			if (this.id == -1)
			{
				return BuiltinRenderTextureType.CameraTarget;
			}
			if (this.id == -2)
			{
				return this.rtid;
			}
			return new RenderTargetIdentifier(this.id, 0, CubemapFace.Unknown, -1);
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x000274C4 File Offset: 0x000256C4
		public bool HasInternalRenderTargetId()
		{
			return this.id == -2;
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x000274D0 File Offset: 0x000256D0
		public bool Equals(RenderTargetHandle other)
		{
			if (this.id == -2 || other.id == -2)
			{
				return this.Identifier() == other.Identifier();
			}
			return this.id == other.id;
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x00027509 File Offset: 0x00025709
		public override bool Equals(object obj)
		{
			return obj != null && obj is RenderTargetHandle && this.Equals((RenderTargetHandle)obj);
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x00027526 File Offset: 0x00025726
		public override int GetHashCode()
		{
			return this.id;
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x0002752E File Offset: 0x0002572E
		public static bool operator ==(RenderTargetHandle c1, RenderTargetHandle c2)
		{
			return c1.Equals(c2);
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x00027538 File Offset: 0x00025738
		public static bool operator !=(RenderTargetHandle c1, RenderTargetHandle c2)
		{
			return !c1.Equals(c2);
		}

		// Token: 0x04000531 RID: 1329
		public static readonly RenderTargetHandle CameraTarget = new RenderTargetHandle
		{
			id = -1
		};
	}
}
