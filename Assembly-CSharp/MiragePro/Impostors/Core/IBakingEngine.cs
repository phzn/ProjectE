using System;
using System.Collections.Generic;
using UnityEngine;

namespace MiragePro.Impostors.Core
{
	// Token: 0x02000274 RID: 628
	public abstract class IBakingEngine
	{
		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06001059 RID: 4185 RVA: 0x0006D0A0 File Offset: 0x0006B2A0
		// (set) Token: 0x0600105A RID: 4186 RVA: 0x0006D0A8 File Offset: 0x0006B2A8
		public List<Vector3> CameraPositions { get; protected set; }

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x0600105B RID: 4187 RVA: 0x0006D0B1 File Offset: 0x0006B2B1
		// (set) Token: 0x0600105C RID: 4188 RVA: 0x0006D0B9 File Offset: 0x0006B2B9
		public List<Mesh> Meshes { get; protected set; }

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x0600105D RID: 4189 RVA: 0x0006D0C2 File Offset: 0x0006B2C2
		// (set) Token: 0x0600105E RID: 4190 RVA: 0x0006D0CA File Offset: 0x0006B2CA
		public List<Material[]> Materials { get; protected set; }

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x0600105F RID: 4191 RVA: 0x0006D0D3 File Offset: 0x0006B2D3
		// (set) Token: 0x06001060 RID: 4192 RVA: 0x0006D0DB File Offset: 0x0006B2DB
		public List<Matrix4x4> MeshesTransforms { get; protected set; }

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06001061 RID: 4193 RVA: 0x0006D0E4 File Offset: 0x0006B2E4
		// (set) Token: 0x06001062 RID: 4194 RVA: 0x0006D0EC File Offset: 0x0006B2EC
		public int TextureSize { get; protected set; }

		// Token: 0x06001063 RID: 4195 RVA: 0x0006D0F5 File Offset: 0x0006B2F5
		protected void Initialize(List<Vector3> cameraPositions, List<Mesh> meshes, List<Material[]> materials, List<Matrix4x4> meshesTransforms, int textureSize)
		{
			this.CameraPositions = cameraPositions;
			this.Meshes = meshes;
			this.Materials = materials;
			this.MeshesTransforms = meshesTransforms;
			this.TextureSize = textureSize;
		}

		// Token: 0x06001064 RID: 4196
		public abstract Texture2D ComputeColorMaps();

		// Token: 0x06001065 RID: 4197
		public abstract Texture2D ComputeNormalMaps();

		// Token: 0x06001066 RID: 4198
		public abstract Texture2D ComputeMaskMaps();

		// Token: 0x06001067 RID: 4199
		public abstract void ApplyPostProcessing(ref Texture2D map, Texture2D colorDepthMap);

		// Token: 0x06001068 RID: 4200 RVA: 0x00004F31 File Offset: 0x00003131
		public virtual void Cleanup()
		{
		}
	}
}
