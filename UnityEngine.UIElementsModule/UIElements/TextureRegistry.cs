using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x020002B5 RID: 693
	internal class TextureRegistry
	{
		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x06001419 RID: 5145 RVA: 0x00047831 File Offset: 0x00045A31
		public static TextureRegistry instance { get; } = new TextureRegistry();

		// Token: 0x0600141A RID: 5146 RVA: 0x00047838 File Offset: 0x00045A38
		public Texture GetTexture(TextureId id)
		{
			bool flag = id.index < 0 || id.index >= this.m_Textures.Count;
			Texture texture;
			if (flag)
			{
				Debug.LogError(string.Format("Attempted to get an invalid texture (index={0}).", id.index));
				texture = null;
			}
			else
			{
				TextureRegistry.TextureInfo textureInfo = this.m_Textures[id.index];
				bool flag2 = textureInfo.refCount < 1;
				if (flag2)
				{
					Debug.LogError(string.Format("Attempted to get a texture (index={0}) that is not allocated.", id.index));
					texture = null;
				}
				else
				{
					texture = textureInfo.texture;
				}
			}
			return texture;
		}

		// Token: 0x0600141B RID: 5147 RVA: 0x000478DC File Offset: 0x00045ADC
		public TextureId AllocAndAcquireDynamic()
		{
			return this.AllocAndAcquire(null, true);
		}

		// Token: 0x0600141C RID: 5148 RVA: 0x000478F8 File Offset: 0x00045AF8
		public void UpdateDynamic(TextureId id, Texture texture)
		{
			bool flag = id.index < 0 || id.index >= this.m_Textures.Count;
			if (flag)
			{
				Debug.LogError(string.Format("Attempted to update an invalid dynamic texture (index={0}).", id.index));
			}
			else
			{
				TextureRegistry.TextureInfo textureInfo = this.m_Textures[id.index];
				bool flag2 = !textureInfo.dynamic;
				if (flag2)
				{
					Debug.LogError(string.Format("Attempted to update a texture (index={0}) that is not dynamic.", id.index));
				}
				else
				{
					bool flag3 = textureInfo.refCount < 1;
					if (flag3)
					{
						Debug.LogError(string.Format("Attempted to update a dynamic texture (index={0}) that is not allocated.", id.index));
					}
					else
					{
						textureInfo.texture = texture;
						this.m_Textures[id.index] = textureInfo;
					}
				}
			}
		}

		// Token: 0x0600141D RID: 5149 RVA: 0x000479D8 File Offset: 0x00045BD8
		private TextureId AllocAndAcquire(Texture texture, bool dynamic)
		{
			TextureRegistry.TextureInfo textureInfo = new TextureRegistry.TextureInfo
			{
				texture = texture,
				dynamic = dynamic,
				refCount = 1
			};
			bool flag = this.m_FreeIds.Count > 0;
			TextureId textureId;
			if (flag)
			{
				textureId = this.m_FreeIds.Pop();
				this.m_Textures[textureId.index] = textureInfo;
			}
			else
			{
				bool flag2 = this.m_Textures.Count == 2048;
				if (flag2)
				{
					Debug.LogError(string.Format("Failed to allocate a {0} because the limit of {1} textures is reached.", "TextureId", 2048));
					return TextureId.invalid;
				}
				textureId = new TextureId(this.m_Textures.Count);
				this.m_Textures.Add(textureInfo);
			}
			bool flag3 = !dynamic;
			if (flag3)
			{
				this.m_TextureToId[texture] = textureId;
			}
			return textureId;
		}

		// Token: 0x0600141E RID: 5150 RVA: 0x00047AC0 File Offset: 0x00045CC0
		public TextureId Acquire(Texture tex)
		{
			TextureId textureId;
			bool flag = this.m_TextureToId.TryGetValue(tex, out textureId);
			TextureId textureId2;
			if (flag)
			{
				TextureRegistry.TextureInfo textureInfo = this.m_Textures[textureId.index];
				Debug.Assert(textureInfo.refCount > 0);
				Debug.Assert(!textureInfo.dynamic);
				textureInfo.refCount++;
				this.m_Textures[textureId.index] = textureInfo;
				textureId2 = textureId;
			}
			else
			{
				textureId2 = this.AllocAndAcquire(tex, false);
			}
			return textureId2;
		}

		// Token: 0x0600141F RID: 5151 RVA: 0x00047B44 File Offset: 0x00045D44
		public void Acquire(TextureId id)
		{
			bool flag = id.index < 0 || id.index >= this.m_Textures.Count;
			if (flag)
			{
				Debug.LogError(string.Format("Attempted to acquire an invalid texture (index={0}).", id.index));
			}
			else
			{
				TextureRegistry.TextureInfo textureInfo = this.m_Textures[id.index];
				bool flag2 = textureInfo.refCount < 1;
				if (flag2)
				{
					Debug.LogError(string.Format("Attempted to acquire a texture (index={0}) that is not allocated.", id.index));
				}
				else
				{
					textureInfo.refCount++;
					this.m_Textures[id.index] = textureInfo;
				}
			}
		}

		// Token: 0x06001420 RID: 5152 RVA: 0x00047BF8 File Offset: 0x00045DF8
		public void Release(TextureId id)
		{
			bool flag = id.index < 0 || id.index >= this.m_Textures.Count;
			if (flag)
			{
				Debug.LogError(string.Format("Attempted to release an invalid texture (index={0}).", id.index));
			}
			else
			{
				TextureRegistry.TextureInfo textureInfo = this.m_Textures[id.index];
				bool flag2 = textureInfo.refCount < 1;
				if (flag2)
				{
					Debug.LogError(string.Format("Attempted to release a texture (index={0}) that is not allocated.", id.index));
				}
				else
				{
					textureInfo.refCount--;
					bool flag3 = textureInfo.refCount == 0;
					if (flag3)
					{
						bool flag4 = !textureInfo.dynamic;
						if (flag4)
						{
							this.m_TextureToId.Remove(textureInfo.texture);
						}
						textureInfo.texture = null;
						textureInfo.dynamic = false;
						this.m_FreeIds.Push(id);
					}
					this.m_Textures[id.index] = textureInfo;
				}
			}
		}

		// Token: 0x06001421 RID: 5153 RVA: 0x00047CFC File Offset: 0x00045EFC
		public TextureId TextureToId(Texture texture)
		{
			TextureId textureId;
			bool flag = this.m_TextureToId.TryGetValue(texture, out textureId);
			TextureId textureId2;
			if (flag)
			{
				textureId2 = textureId;
			}
			else
			{
				textureId2 = TextureId.invalid;
			}
			return textureId2;
		}

		// Token: 0x06001422 RID: 5154 RVA: 0x00047D2C File Offset: 0x00045F2C
		public TextureRegistry.Statistics GatherStatistics()
		{
			TextureRegistry.Statistics statistics = default(TextureRegistry.Statistics);
			statistics.freeIdsCount = this.m_FreeIds.Count;
			statistics.createdIdsCount = this.m_Textures.Count;
			statistics.allocatedIdsTotalCount = this.m_Textures.Count - this.m_FreeIds.Count;
			statistics.allocatedIdsDynamicCount = statistics.allocatedIdsTotalCount - this.m_TextureToId.Count;
			statistics.allocatedIdsStaticCount = statistics.allocatedIdsTotalCount - statistics.allocatedIdsDynamicCount;
			statistics.availableIdsCount = 2048 - statistics.allocatedIdsTotalCount;
			return statistics;
		}

		// Token: 0x04000954 RID: 2388
		private List<TextureRegistry.TextureInfo> m_Textures = new List<TextureRegistry.TextureInfo>(128);

		// Token: 0x04000955 RID: 2389
		private Dictionary<Texture, TextureId> m_TextureToId = new Dictionary<Texture, TextureId>(128);

		// Token: 0x04000956 RID: 2390
		private Stack<TextureId> m_FreeIds = new Stack<TextureId>();

		// Token: 0x04000957 RID: 2391
		internal const int maxTextures = 2048;

		// Token: 0x020002B6 RID: 694
		private struct TextureInfo
		{
			// Token: 0x04000959 RID: 2393
			public Texture texture;

			// Token: 0x0400095A RID: 2394
			public bool dynamic;

			// Token: 0x0400095B RID: 2395
			public int refCount;
		}

		// Token: 0x020002B7 RID: 695
		public struct Statistics
		{
			// Token: 0x0400095C RID: 2396
			public int freeIdsCount;

			// Token: 0x0400095D RID: 2397
			public int createdIdsCount;

			// Token: 0x0400095E RID: 2398
			public int allocatedIdsTotalCount;

			// Token: 0x0400095F RID: 2399
			public int allocatedIdsDynamicCount;

			// Token: 0x04000960 RID: 2400
			public int allocatedIdsStaticCount;

			// Token: 0x04000961 RID: 2401
			public int availableIdsCount;
		}
	}
}
