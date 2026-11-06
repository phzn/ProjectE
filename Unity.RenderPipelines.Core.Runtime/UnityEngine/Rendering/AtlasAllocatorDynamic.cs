using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace UnityEngine.Rendering
{
	// Token: 0x020000C1 RID: 193
	internal class AtlasAllocatorDynamic
	{
		// Token: 0x060005F9 RID: 1529 RVA: 0x0001E988 File Offset: 0x0001CB88
		public AtlasAllocatorDynamic(int width, int height, int capacityAllocations)
		{
			int num = capacityAllocations * 2;
			this.m_Pool = new AtlasAllocatorDynamic.AtlasNodePool((short)num);
			this.m_NodeFromID = new Dictionary<int, short>(capacityAllocations);
			short num2 = -1;
			this.m_Root = this.m_Pool.AtlasNodeCreate(num2);
			this.m_Pool.m_Nodes[(int)this.m_Root].m_Rect.Set((float)width, (float)height, 0f, 0f);
			this.m_Width = width;
			this.m_Height = height;
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x0001EA08 File Offset: 0x0001CC08
		public bool Allocate(out Vector4 result, int key, int width, int height)
		{
			short num = this.m_Pool.m_Nodes[(int)this.m_Root].Allocate(this.m_Pool, width, height);
			if (num >= 0)
			{
				result = this.m_Pool.m_Nodes[(int)num].m_Rect;
				this.m_NodeFromID.Add(key, num);
				return true;
			}
			result = Vector4.zero;
			return false;
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x0001EA78 File Offset: 0x0001CC78
		public void Release(int key)
		{
			short num;
			if (this.m_NodeFromID.TryGetValue(key, out num))
			{
				this.m_Pool.m_Nodes[(int)num].ReleaseAndMerge(this.m_Pool);
				this.m_NodeFromID.Remove(key);
				return;
			}
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x0001EAC0 File Offset: 0x0001CCC0
		public void Release()
		{
			this.m_Pool.Clear();
			this.m_Root = this.m_Pool.AtlasNodeCreate(-1);
			this.m_Pool.m_Nodes[(int)this.m_Root].m_Rect.Set((float)this.m_Width, (float)this.m_Height, 0f, 0f);
			this.m_NodeFromID.Clear();
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x0001EB30 File Offset: 0x0001CD30
		public string DebugStringFromRoot(int depthMax = -1)
		{
			string text = "";
			this.DebugStringFromNode(ref text, this.m_Root, 0, depthMax);
			return text;
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x0001EB54 File Offset: 0x0001CD54
		private void DebugStringFromNode(ref string res, short n, int depthCurrent = 0, int depthMax = -1)
		{
			res = string.Concat(new string[]
			{
				res,
				"{[",
				depthCurrent.ToString(),
				"], isOccupied = ",
				this.m_Pool.m_Nodes[(int)n].IsOccupied() ? "true" : "false",
				", self = ",
				this.m_Pool.m_Nodes[(int)n].m_Self.ToString(),
				", ",
				this.m_Pool.m_Nodes[(int)n].m_Rect.x.ToString(),
				",",
				this.m_Pool.m_Nodes[(int)n].m_Rect.y.ToString(),
				", ",
				this.m_Pool.m_Nodes[(int)n].m_Rect.z.ToString(),
				", ",
				this.m_Pool.m_Nodes[(int)n].m_Rect.w.ToString(),
				"}\n"
			});
			if (depthMax == -1 || depthCurrent < depthMax)
			{
				if (this.m_Pool.m_Nodes[(int)n].m_LeftChild >= 0)
				{
					this.DebugStringFromNode(ref res, this.m_Pool.m_Nodes[(int)n].m_LeftChild, depthCurrent + 1, depthMax);
				}
				if (this.m_Pool.m_Nodes[(int)n].m_RightChild >= 0)
				{
					this.DebugStringFromNode(ref res, this.m_Pool.m_Nodes[(int)n].m_RightChild, depthCurrent + 1, depthMax);
				}
			}
		}

		// Token: 0x04000435 RID: 1077
		private int m_Width;

		// Token: 0x04000436 RID: 1078
		private int m_Height;

		// Token: 0x04000437 RID: 1079
		private AtlasAllocatorDynamic.AtlasNodePool m_Pool;

		// Token: 0x04000438 RID: 1080
		private short m_Root;

		// Token: 0x04000439 RID: 1081
		private Dictionary<int, short> m_NodeFromID;

		// Token: 0x020001C5 RID: 453
		private class AtlasNodePool
		{
			// Token: 0x06000B49 RID: 2889 RVA: 0x0002F777 File Offset: 0x0002D977
			public AtlasNodePool(short capacity)
			{
				this.m_Nodes = new AtlasAllocatorDynamic.AtlasNode[(int)capacity];
				this.m_Next = 0;
				this.m_FreelistHead = -1;
			}

			// Token: 0x06000B4A RID: 2890 RVA: 0x0002F799 File Offset: 0x0002D999
			public void Dispose()
			{
				this.Clear();
				this.m_Nodes = null;
			}

			// Token: 0x06000B4B RID: 2891 RVA: 0x0002F7A8 File Offset: 0x0002D9A8
			public void Clear()
			{
				this.m_Next = 0;
				this.m_FreelistHead = -1;
			}

			// Token: 0x06000B4C RID: 2892 RVA: 0x0002F7B8 File Offset: 0x0002D9B8
			public short AtlasNodeCreate(short parent)
			{
				if (this.m_FreelistHead != -1)
				{
					short freelistNext = this.m_Nodes[(int)this.m_FreelistHead].m_FreelistNext;
					this.m_Nodes[(int)this.m_FreelistHead] = new AtlasAllocatorDynamic.AtlasNode(this.m_FreelistHead, parent);
					short freelistHead = this.m_FreelistHead;
					this.m_FreelistHead = freelistNext;
					return freelistHead;
				}
				this.m_Nodes[(int)this.m_Next] = new AtlasAllocatorDynamic.AtlasNode(this.m_Next, parent);
				short next = this.m_Next;
				this.m_Next = next + 1;
				return next;
			}

			// Token: 0x06000B4D RID: 2893 RVA: 0x0002F83F File Offset: 0x0002DA3F
			public void AtlasNodeFree(short index)
			{
				this.m_Nodes[(int)index].m_FreelistNext = this.m_FreelistHead;
				this.m_FreelistHead = index;
			}

			// Token: 0x04000754 RID: 1876
			internal AtlasAllocatorDynamic.AtlasNode[] m_Nodes;

			// Token: 0x04000755 RID: 1877
			private short m_Next;

			// Token: 0x04000756 RID: 1878
			private short m_FreelistHead;
		}

		// Token: 0x020001C6 RID: 454
		[StructLayout(LayoutKind.Explicit, Size = 32)]
		private struct AtlasNode
		{
			// Token: 0x06000B4E RID: 2894 RVA: 0x0002F85F File Offset: 0x0002DA5F
			public AtlasNode(short self, short parent)
			{
				this.m_Self = self;
				this.m_Parent = parent;
				this.m_LeftChild = -1;
				this.m_RightChild = -1;
				this.m_Flags = 0;
				this.m_FreelistNext = -1;
				this.m_Rect = Vector4.zero;
			}

			// Token: 0x06000B4F RID: 2895 RVA: 0x0002F896 File Offset: 0x0002DA96
			public bool IsOccupied()
			{
				return (this.m_Flags & 1) > 0;
			}

			// Token: 0x06000B50 RID: 2896 RVA: 0x0002F8A4 File Offset: 0x0002DAA4
			public void SetIsOccupied()
			{
				ushort num = 1;
				this.m_Flags |= num;
			}

			// Token: 0x06000B51 RID: 2897 RVA: 0x0002F8C4 File Offset: 0x0002DAC4
			public void ClearIsOccupied()
			{
				ushort num = 1;
				this.m_Flags &= ~num;
			}

			// Token: 0x06000B52 RID: 2898 RVA: 0x0002F8E4 File Offset: 0x0002DAE4
			public bool IsLeafNode()
			{
				return this.m_LeftChild == -1;
			}

			// Token: 0x06000B53 RID: 2899 RVA: 0x0002F8F0 File Offset: 0x0002DAF0
			public short Allocate(AtlasAllocatorDynamic.AtlasNodePool pool, int width, int height)
			{
				if (Mathf.Min(width, height) < 1)
				{
					return -1;
				}
				if (!this.IsLeafNode())
				{
					short num = pool.m_Nodes[(int)this.m_LeftChild].Allocate(pool, width, height);
					if (num == -1)
					{
						num = pool.m_Nodes[(int)this.m_RightChild].Allocate(pool, width, height);
					}
					return num;
				}
				if (this.IsOccupied())
				{
					return -1;
				}
				if ((float)width > this.m_Rect.x || (float)height > this.m_Rect.y)
				{
					return -1;
				}
				this.m_LeftChild = pool.AtlasNodeCreate(this.m_Self);
				this.m_RightChild = pool.AtlasNodeCreate(this.m_Self);
				float num2 = this.m_Rect.x - (float)width;
				float num3 = this.m_Rect.y - (float)height;
				if (num2 >= num3)
				{
					pool.m_Nodes[(int)this.m_LeftChild].m_Rect.x = (float)width;
					pool.m_Nodes[(int)this.m_LeftChild].m_Rect.y = this.m_Rect.y;
					pool.m_Nodes[(int)this.m_LeftChild].m_Rect.z = this.m_Rect.z;
					pool.m_Nodes[(int)this.m_LeftChild].m_Rect.w = this.m_Rect.w;
					pool.m_Nodes[(int)this.m_RightChild].m_Rect.x = num2;
					pool.m_Nodes[(int)this.m_RightChild].m_Rect.y = this.m_Rect.y;
					pool.m_Nodes[(int)this.m_RightChild].m_Rect.z = this.m_Rect.z + (float)width;
					pool.m_Nodes[(int)this.m_RightChild].m_Rect.w = this.m_Rect.w;
					if (num3 < 1f)
					{
						pool.m_Nodes[(int)this.m_LeftChild].SetIsOccupied();
						return this.m_LeftChild;
					}
					short num4 = pool.m_Nodes[(int)this.m_LeftChild].Allocate(pool, width, height);
					if (num4 >= 0)
					{
						pool.m_Nodes[(int)num4].SetIsOccupied();
					}
					return num4;
				}
				else
				{
					pool.m_Nodes[(int)this.m_LeftChild].m_Rect.x = this.m_Rect.x;
					pool.m_Nodes[(int)this.m_LeftChild].m_Rect.y = (float)height;
					pool.m_Nodes[(int)this.m_LeftChild].m_Rect.z = this.m_Rect.z;
					pool.m_Nodes[(int)this.m_LeftChild].m_Rect.w = this.m_Rect.w;
					pool.m_Nodes[(int)this.m_RightChild].m_Rect.x = this.m_Rect.x;
					pool.m_Nodes[(int)this.m_RightChild].m_Rect.y = num3;
					pool.m_Nodes[(int)this.m_RightChild].m_Rect.z = this.m_Rect.z;
					pool.m_Nodes[(int)this.m_RightChild].m_Rect.w = this.m_Rect.w + (float)height;
					if (num2 < 1f)
					{
						pool.m_Nodes[(int)this.m_LeftChild].SetIsOccupied();
						return this.m_LeftChild;
					}
					short num5 = pool.m_Nodes[(int)this.m_LeftChild].Allocate(pool, width, height);
					if (num5 >= 0)
					{
						pool.m_Nodes[(int)num5].SetIsOccupied();
					}
					return num5;
				}
			}

			// Token: 0x06000B54 RID: 2900 RVA: 0x0002FCB0 File Offset: 0x0002DEB0
			public void ReleaseChildren(AtlasAllocatorDynamic.AtlasNodePool pool)
			{
				if (this.IsLeafNode())
				{
					return;
				}
				pool.m_Nodes[(int)this.m_LeftChild].ReleaseChildren(pool);
				pool.m_Nodes[(int)this.m_RightChild].ReleaseChildren(pool);
				pool.AtlasNodeFree(this.m_LeftChild);
				pool.AtlasNodeFree(this.m_RightChild);
				this.m_LeftChild = -1;
				this.m_RightChild = -1;
			}

			// Token: 0x06000B55 RID: 2901 RVA: 0x0002FD1C File Offset: 0x0002DF1C
			public void ReleaseAndMerge(AtlasAllocatorDynamic.AtlasNodePool pool)
			{
				short num = this.m_Self;
				do
				{
					pool.m_Nodes[(int)num].ReleaseChildren(pool);
					pool.m_Nodes[(int)num].ClearIsOccupied();
					num = pool.m_Nodes[(int)num].m_Parent;
				}
				while (num >= 0 && pool.m_Nodes[(int)num].IsMergeNeeded(pool));
			}

			// Token: 0x06000B56 RID: 2902 RVA: 0x0002FD80 File Offset: 0x0002DF80
			public bool IsMergeNeeded(AtlasAllocatorDynamic.AtlasNodePool pool)
			{
				return pool.m_Nodes[(int)this.m_LeftChild].IsLeafNode() && !pool.m_Nodes[(int)this.m_LeftChild].IsOccupied() && pool.m_Nodes[(int)this.m_RightChild].IsLeafNode() && !pool.m_Nodes[(int)this.m_RightChild].IsOccupied();
			}

			// Token: 0x04000757 RID: 1879
			[FieldOffset(0)]
			public short m_Self;

			// Token: 0x04000758 RID: 1880
			[FieldOffset(2)]
			public short m_Parent;

			// Token: 0x04000759 RID: 1881
			[FieldOffset(4)]
			public short m_LeftChild;

			// Token: 0x0400075A RID: 1882
			[FieldOffset(6)]
			public short m_RightChild;

			// Token: 0x0400075B RID: 1883
			[FieldOffset(8)]
			public short m_FreelistNext;

			// Token: 0x0400075C RID: 1884
			[FieldOffset(10)]
			public ushort m_Flags;

			// Token: 0x0400075D RID: 1885
			[FieldOffset(16)]
			public Vector4 m_Rect;

			// Token: 0x020001FF RID: 511
			private enum AtlasNodeFlags : uint
			{
				// Token: 0x040007EE RID: 2030
				IsOccupied = 1U
			}
		}
	}
}
