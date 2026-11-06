using System;
using System.Runtime.CompilerServices;

namespace UnityEngine.UIElements.UIR
{
	// Token: 0x0200046D RID: 1133
	internal class TextureSlotManager
	{
		// Token: 0x0600232E RID: 9006 RVA: 0x00088C50 File Offset: 0x00086E50
		static TextureSlotManager()
		{
			for (int i = 0; i < TextureSlotManager.k_SlotCount; i++)
			{
				TextureSlotManager.slotIds[i] = Shader.PropertyToID(string.Format("_Texture{0}", i));
			}
		}

		// Token: 0x0600232F RID: 9007 RVA: 0x00088CC4 File Offset: 0x00086EC4
		public TextureSlotManager()
		{
			this.m_Textures = new TextureId[TextureSlotManager.k_SlotCount];
			this.m_Tickets = new int[TextureSlotManager.k_SlotCount];
			this.m_GpuTextures = new Vector4[TextureSlotManager.k_SlotCount * TextureSlotManager.k_SlotSize];
			this.Reset();
		}

		// Token: 0x06002330 RID: 9008 RVA: 0x00088D2C File Offset: 0x00086F2C
		public void Reset()
		{
			this.m_CurrentTicket = 0;
			this.m_FirstUsedTicket = 0;
			for (int i = 0; i < TextureSlotManager.k_SlotCount; i++)
			{
				this.m_Textures[i] = TextureId.invalid;
				this.m_Tickets[i] = -1;
				this.SetGpuData(i, TextureId.invalid, 1, 1, 0f);
			}
		}

		// Token: 0x06002331 RID: 9009 RVA: 0x00088D8C File Offset: 0x00086F8C
		public void StartNewBatch()
		{
			int num = this.m_CurrentTicket + 1;
			this.m_CurrentTicket = num;
			this.m_FirstUsedTicket = num;
			this.FreeSlots = TextureSlotManager.k_SlotCount;
		}

		// Token: 0x06002332 RID: 9010 RVA: 0x00088DC0 File Offset: 0x00086FC0
		public int IndexOf(TextureId id)
		{
			for (int i = 0; i < TextureSlotManager.k_SlotCount; i++)
			{
				bool flag = this.m_Textures[i].index == id.index;
				if (flag)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06002333 RID: 9011 RVA: 0x00088E0C File Offset: 0x0008700C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void MarkUsed(int slotIndex)
		{
			int num = this.m_Tickets[slotIndex];
			bool flag = num < this.m_FirstUsedTicket;
			int num2;
			if (flag)
			{
				num2 = this.FreeSlots - 1;
				this.FreeSlots = num2;
			}
			int[] tickets = this.m_Tickets;
			num2 = this.m_CurrentTicket + 1;
			this.m_CurrentTicket = num2;
			tickets[slotIndex] = num2;
		}

		// Token: 0x170007FA RID: 2042
		// (get) Token: 0x06002334 RID: 9012 RVA: 0x00088E5A File Offset: 0x0008705A
		// (set) Token: 0x06002335 RID: 9013 RVA: 0x00088E62 File Offset: 0x00087062
		public int FreeSlots { get; private set; } = TextureSlotManager.k_SlotCount;

		// Token: 0x06002336 RID: 9014 RVA: 0x00088E6C File Offset: 0x0008706C
		public int FindOldestSlot()
		{
			int num = this.m_Tickets[0];
			int num2 = 0;
			for (int i = 1; i < TextureSlotManager.k_SlotCount; i++)
			{
				bool flag = this.m_Tickets[i] < num;
				if (flag)
				{
					num = this.m_Tickets[i];
					num2 = i;
				}
			}
			return num2;
		}

		// Token: 0x06002337 RID: 9015 RVA: 0x00088EC0 File Offset: 0x000870C0
		public void Bind(TextureId id, float sdfScale, int slot, MaterialPropertyBlock mat)
		{
			Texture texture = this.textureRegistry.GetTexture(id);
			bool flag = texture == null;
			if (flag)
			{
				texture = Texture2D.whiteTexture;
			}
			this.m_Textures[slot] = id;
			this.MarkUsed(slot);
			this.SetGpuData(slot, id, texture.width, texture.height, sdfScale);
			mat.SetTexture(TextureSlotManager.slotIds[slot], texture);
			mat.SetVectorArray(TextureSlotManager.textureTableId, this.m_GpuTextures);
		}

		// Token: 0x06002338 RID: 9016 RVA: 0x00088F3C File Offset: 0x0008713C
		public void SetGpuData(int slotIndex, TextureId id, int textureWidth, int textureHeight, float sdfScale)
		{
			int num = slotIndex * TextureSlotManager.k_SlotSize;
			float num2 = 1f / (float)textureWidth;
			float num3 = 1f / (float)textureHeight;
			this.m_GpuTextures[num] = new Vector4(id.ConvertToGpu(), num2, num3, sdfScale);
			this.m_GpuTextures[num + 1] = new Vector4((float)textureWidth, (float)textureHeight, 0f, 0f);
		}

		// Token: 0x04001053 RID: 4179
		internal static readonly int k_SlotCount = (UIRenderDevice.shaderModelIs35 ? 8 : 4);

		// Token: 0x04001054 RID: 4180
		internal static readonly int k_SlotSize = 2;

		// Token: 0x04001055 RID: 4181
		internal static readonly int[] slotIds = new int[TextureSlotManager.k_SlotCount];

		// Token: 0x04001056 RID: 4182
		internal static readonly int textureTableId = Shader.PropertyToID("_TextureInfo");

		// Token: 0x04001057 RID: 4183
		private TextureId[] m_Textures;

		// Token: 0x04001058 RID: 4184
		private int[] m_Tickets;

		// Token: 0x04001059 RID: 4185
		private int m_CurrentTicket;

		// Token: 0x0400105A RID: 4186
		private int m_FirstUsedTicket;

		// Token: 0x0400105B RID: 4187
		private Vector4[] m_GpuTextures;

		// Token: 0x0400105D RID: 4189
		internal TextureRegistry textureRegistry = TextureRegistry.instance;
	}
}
