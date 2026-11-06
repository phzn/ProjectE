using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUInstancerPro
{
	// Token: 0x02000013 RID: 19
	public class GPUIBillboard : ScriptableObject
	{
		// Token: 0x0600008A RID: 138 RVA: 0x000035C6 File Offset: 0x000017C6
		public override string ToString()
		{
			return this.prefabObject.name;
		}

		// Token: 0x0600008B RID: 139 RVA: 0x000035D3 File Offset: 0x000017D3
		public Texture GetAlbedoTexture()
		{
			if (this.albedoAtlasTexture != null)
			{
				return this.albedoAtlasTexture;
			}
			return this.albedoAtlasRT;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x000035F0 File Offset: 0x000017F0
		public Texture GetNormalTexture()
		{
			if (this.normalAtlasTexture != null)
			{
				return this.normalAtlasTexture;
			}
			return this.normalAtlasRT;
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00003610 File Offset: 0x00001810
		public GPUIBillboard.GPUIBillboardCustomShaderProperties GetShaderCustomProperty(string shaderName)
		{
			if (string.IsNullOrEmpty(shaderName) || this.customShaderProperties == null)
			{
				return null;
			}
			foreach (GPUIBillboard.GPUIBillboardCustomShaderProperties gpuibillboardCustomShaderProperties in this.customShaderProperties)
			{
				if (shaderName.Equals(gpuibillboardCustomShaderProperties.shaderName))
				{
					return gpuibillboardCustomShaderProperties;
				}
			}
			return null;
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00003684 File Offset: 0x00001884
		public bool ClearUnusedCustomShaderProperties(IEnumerable<Shader> shaders)
		{
			if (this.customShaderProperties == null || this.customShaderProperties.Count == 0)
			{
				return false;
			}
			bool flag = false;
			for (int i = 0; i < this.customShaderProperties.Count; i++)
			{
				GPUIBillboard.GPUIBillboardCustomShaderProperties gpuibillboardCustomShaderProperties = this.customShaderProperties[i];
				if (gpuibillboardCustomShaderProperties == null || string.IsNullOrEmpty(gpuibillboardCustomShaderProperties.shaderName) || Shader.Find(gpuibillboardCustomShaderProperties.shaderName) == null)
				{
					this.customShaderProperties.RemoveAt(i);
					i--;
					flag = true;
				}
				else
				{
					bool flag2 = false;
					foreach (Shader shader in shaders)
					{
						if (shader != null && shader.name == gpuibillboardCustomShaderProperties.shaderName)
						{
							flag2 = true;
							break;
						}
					}
					if (!flag2)
					{
						this.customShaderProperties.RemoveAt(i);
						i--;
						flag = true;
					}
				}
			}
			return flag;
		}

		// Token: 0x04000024 RID: 36
		[SerializeField]
		public GameObject prefabObject;

		// Token: 0x04000025 RID: 37
		[SerializeField]
		public GPUIBillboard.GPUIBillboardResolution atlasResolution = GPUIBillboard.GPUIBillboardResolution.x2048;

		// Token: 0x04000026 RID: 38
		[Range(1f, 32f)]
		[SerializeField]
		public int frameCount = 8;

		// Token: 0x04000027 RID: 39
		[Range(0f, 1f)]
		[SerializeField]
		public float brightness = 0.5f;

		// Token: 0x04000028 RID: 40
		[Range(0f, 1f)]
		[SerializeField]
		public float cutoffOverride = 0.5f;

		// Token: 0x04000029 RID: 41
		[Range(0f, 1f)]
		[SerializeField]
		public float normalStrength = 0.5f;

		// Token: 0x0400002A RID: 42
		[SerializeField]
		public GPUIBillboard.GPUIBillboardShaderType billboardShaderType;

		// Token: 0x0400002B RID: 43
		[SerializeField]
		public List<GPUIBillboard.GPUIBillboardCustomShaderProperties> customShaderProperties;

		// Token: 0x0400002C RID: 44
		[SerializeField]
		public Vector2 quadSize;

		// Token: 0x0400002D RID: 45
		[SerializeField]
		public float yPivotOffset;

		// Token: 0x0400002E RID: 46
		[SerializeField]
		public Texture2D albedoAtlasTexture;

		// Token: 0x0400002F RID: 47
		[SerializeField]
		public Texture2D normalAtlasTexture;

		// Token: 0x04000030 RID: 48
		[NonSerialized]
		public RenderTexture albedoAtlasRT;

		// Token: 0x04000031 RID: 49
		[NonSerialized]
		public RenderTexture normalAtlasRT;

		// Token: 0x04000032 RID: 50
		[NonSerialized]
		internal Mesh _quadMesh;

		// Token: 0x04000033 RID: 51
		[NonSerialized]
		internal Material _billboardMaterial;

		// Token: 0x02000014 RID: 20
		public enum GPUIBillboardResolution
		{
			// Token: 0x04000035 RID: 53
			x256 = 256,
			// Token: 0x04000036 RID: 54
			x512 = 512,
			// Token: 0x04000037 RID: 55
			x1024 = 1024,
			// Token: 0x04000038 RID: 56
			x2048 = 2048,
			// Token: 0x04000039 RID: 57
			x4096 = 4096,
			// Token: 0x0400003A RID: 58
			x8192 = 8192
		}

		// Token: 0x02000015 RID: 21
		public enum GPUIBillboardShaderType
		{
			// Token: 0x0400003C RID: 60
			Default,
			// Token: 0x0400003D RID: 61
			SpeedTree,
			// Token: 0x0400003E RID: 62
			TreeCreator,
			// Token: 0x0400003F RID: 63
			SoftOcclusion
		}

		// Token: 0x02000016 RID: 22
		[Serializable]
		public class GPUIBillboardCustomShaderProperties
		{
			// Token: 0x04000040 RID: 64
			[SerializeField]
			public string shaderName;

			// Token: 0x04000041 RID: 65
			[SerializeField]
			public string mainTextureProperty;

			// Token: 0x04000042 RID: 66
			[SerializeField]
			public string mainColorProperty;

			// Token: 0x04000043 RID: 67
			[SerializeField]
			public bool useRedChannelCutoff;
		}
	}
}
