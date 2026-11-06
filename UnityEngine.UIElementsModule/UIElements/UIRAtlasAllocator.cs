using System;
using Unity.Profiling;
using UnityEngine.Assertions;

namespace UnityEngine.UIElements
{
	// Token: 0x020002A2 RID: 674
	internal class UIRAtlasAllocator : IDisposable
	{
		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x0600134B RID: 4939 RVA: 0x000436C6 File Offset: 0x000418C6
		public int maxAtlasSize { get; }

		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x0600134C RID: 4940 RVA: 0x000436CE File Offset: 0x000418CE
		public int maxImageWidth { get; }

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x0600134D RID: 4941 RVA: 0x000436D6 File Offset: 0x000418D6
		public int maxImageHeight { get; }

		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x0600134E RID: 4942 RVA: 0x000436DE File Offset: 0x000418DE
		// (set) Token: 0x0600134F RID: 4943 RVA: 0x000436E6 File Offset: 0x000418E6
		public int virtualWidth { get; private set; }

		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x06001350 RID: 4944 RVA: 0x000436EF File Offset: 0x000418EF
		// (set) Token: 0x06001351 RID: 4945 RVA: 0x000436F7 File Offset: 0x000418F7
		public int virtualHeight { get; private set; }

		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x06001352 RID: 4946 RVA: 0x00043700 File Offset: 0x00041900
		// (set) Token: 0x06001353 RID: 4947 RVA: 0x00043708 File Offset: 0x00041908
		public int physicalWidth { get; private set; }

		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x06001354 RID: 4948 RVA: 0x00043711 File Offset: 0x00041911
		// (set) Token: 0x06001355 RID: 4949 RVA: 0x00043719 File Offset: 0x00041919
		public int physicalHeight { get; private set; }

		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x06001356 RID: 4950 RVA: 0x00043722 File Offset: 0x00041922
		// (set) Token: 0x06001357 RID: 4951 RVA: 0x0004372A File Offset: 0x0004192A
		private protected bool disposed { protected get; private set; }

		// Token: 0x06001358 RID: 4952 RVA: 0x00043733 File Offset: 0x00041933
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06001359 RID: 4953 RVA: 0x00043748 File Offset: 0x00041948
		protected virtual void Dispose(bool disposing)
		{
			bool disposed = this.disposed;
			if (!disposed)
			{
				if (disposing)
				{
					for (int i = 0; i < this.m_OpenRows.Length; i++)
					{
						UIRAtlasAllocator.Row row = this.m_OpenRows[i];
						bool flag = row != null;
						if (flag)
						{
							row.Release();
						}
					}
					this.m_OpenRows = null;
					UIRAtlasAllocator.AreaNode next;
					for (UIRAtlasAllocator.AreaNode areaNode = this.m_FirstUnpartitionedArea; areaNode != null; areaNode = next)
					{
						next = areaNode.next;
						areaNode.Release();
					}
					this.m_FirstUnpartitionedArea = null;
				}
				this.disposed = true;
			}
		}

		// Token: 0x0600135A RID: 4954 RVA: 0x000437E0 File Offset: 0x000419E0
		private static int GetLog2OfNextPower(int n)
		{
			float num = (float)Mathf.NextPowerOfTwo(n);
			float num2 = Mathf.Log(num, 2f);
			return Mathf.RoundToInt(num2);
		}

		// Token: 0x0600135B RID: 4955 RVA: 0x0004380C File Offset: 0x00041A0C
		public UIRAtlasAllocator(int initialAtlasSize, int maxAtlasSize, int sidePadding = 1)
		{
			Assert.IsTrue(initialAtlasSize > 0 && initialAtlasSize <= maxAtlasSize);
			Assert.IsTrue(initialAtlasSize == Mathf.NextPowerOfTwo(initialAtlasSize));
			Assert.IsTrue(maxAtlasSize == Mathf.NextPowerOfTwo(maxAtlasSize));
			this.m_1SidePadding = sidePadding;
			this.m_2SidePadding = sidePadding << 1;
			this.maxAtlasSize = maxAtlasSize;
			this.maxImageWidth = maxAtlasSize;
			this.maxImageHeight = ((initialAtlasSize == maxAtlasSize) ? (maxAtlasSize / 2 + this.m_2SidePadding) : (maxAtlasSize / 4 + this.m_2SidePadding));
			this.virtualWidth = initialAtlasSize;
			this.virtualHeight = initialAtlasSize;
			int num = UIRAtlasAllocator.GetLog2OfNextPower(maxAtlasSize) + 1;
			this.m_OpenRows = new UIRAtlasAllocator.Row[num];
			RectInt rectInt = new RectInt(0, 0, initialAtlasSize, initialAtlasSize);
			this.m_FirstUnpartitionedArea = UIRAtlasAllocator.AreaNode.Acquire(rectInt);
			this.BuildAreas();
		}

		// Token: 0x0600135C RID: 4956 RVA: 0x000438D4 File Offset: 0x00041AD4
		public bool TryAllocate(int width, int height, out RectInt location)
		{
			bool flag;
			using (UIRAtlasAllocator.s_MarkerTryAllocate.Auto())
			{
				location = default(RectInt);
				bool disposed = this.disposed;
				if (disposed)
				{
					flag = false;
				}
				else
				{
					bool flag2 = width < 1 || height < 1;
					if (flag2)
					{
						flag = false;
					}
					else
					{
						bool flag3 = width > this.maxImageWidth || height > this.maxImageHeight;
						if (flag3)
						{
							flag = false;
						}
						else
						{
							int log2OfNextPower = UIRAtlasAllocator.GetLog2OfNextPower(Mathf.Max(height - this.m_2SidePadding, 1));
							int num = (1 << log2OfNextPower) + this.m_2SidePadding;
							UIRAtlasAllocator.Row row = this.m_OpenRows[log2OfNextPower];
							bool flag4 = row != null && row.width - row.Cursor < width;
							if (flag4)
							{
								row = null;
							}
							bool flag5 = row == null;
							if (flag5)
							{
								for (UIRAtlasAllocator.AreaNode areaNode = this.m_FirstUnpartitionedArea; areaNode != null; areaNode = areaNode.next)
								{
									bool flag6 = this.TryPartitionArea(areaNode, log2OfNextPower, num, width);
									if (flag6)
									{
										row = this.m_OpenRows[log2OfNextPower];
										break;
									}
								}
								bool flag7 = row == null;
								if (flag7)
								{
									return false;
								}
							}
							location = new RectInt(row.offsetX + row.Cursor, row.offsetY, width, height);
							row.Cursor += width;
							Assert.IsTrue(row.Cursor <= row.width);
							this.physicalWidth = Mathf.NextPowerOfTwo(Mathf.Max(this.physicalWidth, location.xMax));
							this.physicalHeight = Mathf.NextPowerOfTwo(Mathf.Max(this.physicalHeight, location.yMax));
							flag = true;
						}
					}
				}
			}
			return flag;
		}

		// Token: 0x0600135D RID: 4957 RVA: 0x00043A9C File Offset: 0x00041C9C
		private bool TryPartitionArea(UIRAtlasAllocator.AreaNode areaNode, int rowIndex, int rowHeight, int minWidth)
		{
			RectInt rect = areaNode.rect;
			bool flag = rect.height < rowHeight || rect.width < minWidth;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				UIRAtlasAllocator.Row row = this.m_OpenRows[rowIndex];
				bool flag3 = row != null;
				if (flag3)
				{
					row.Release();
				}
				row = UIRAtlasAllocator.Row.Acquire(rect.x, rect.y, rect.width, rowHeight);
				this.m_OpenRows[rowIndex] = row;
				rect.y += rowHeight;
				rect.height -= rowHeight;
				bool flag4 = rect.height == 0;
				if (flag4)
				{
					bool flag5 = areaNode == this.m_FirstUnpartitionedArea;
					if (flag5)
					{
						this.m_FirstUnpartitionedArea = areaNode.next;
					}
					areaNode.RemoveFromChain();
					areaNode.Release();
				}
				else
				{
					areaNode.rect = rect;
				}
				flag2 = true;
			}
			return flag2;
		}

		// Token: 0x0600135E RID: 4958 RVA: 0x00043B7C File Offset: 0x00041D7C
		private void BuildAreas()
		{
			UIRAtlasAllocator.AreaNode areaNode = this.m_FirstUnpartitionedArea;
			while (this.virtualWidth < this.maxAtlasSize || this.virtualHeight < this.maxAtlasSize)
			{
				bool flag = this.virtualWidth > this.virtualHeight;
				RectInt rectInt;
				if (flag)
				{
					rectInt = new RectInt(0, this.virtualHeight, this.virtualWidth, this.virtualHeight);
					this.virtualHeight *= 2;
				}
				else
				{
					rectInt = new RectInt(this.virtualWidth, 0, this.virtualWidth, this.virtualHeight);
					this.virtualWidth *= 2;
				}
				UIRAtlasAllocator.AreaNode areaNode2 = UIRAtlasAllocator.AreaNode.Acquire(rectInt);
				areaNode2.AddAfter(areaNode);
				areaNode = areaNode2;
			}
		}

		// Token: 0x040008C5 RID: 2245
		private UIRAtlasAllocator.AreaNode m_FirstUnpartitionedArea;

		// Token: 0x040008C6 RID: 2246
		private UIRAtlasAllocator.Row[] m_OpenRows;

		// Token: 0x040008C7 RID: 2247
		private int m_1SidePadding;

		// Token: 0x040008C8 RID: 2248
		private int m_2SidePadding;

		// Token: 0x040008C9 RID: 2249
		private static ProfilerMarker s_MarkerTryAllocate = new ProfilerMarker("UIRAtlasAllocator.TryAllocate");

		// Token: 0x020002A3 RID: 675
		private class Row
		{
			// Token: 0x17000417 RID: 1047
			// (get) Token: 0x06001360 RID: 4960 RVA: 0x00043C49 File Offset: 0x00041E49
			// (set) Token: 0x06001361 RID: 4961 RVA: 0x00043C51 File Offset: 0x00041E51
			public int offsetX { get; private set; }

			// Token: 0x17000418 RID: 1048
			// (get) Token: 0x06001362 RID: 4962 RVA: 0x00043C5A File Offset: 0x00041E5A
			// (set) Token: 0x06001363 RID: 4963 RVA: 0x00043C62 File Offset: 0x00041E62
			public int offsetY { get; private set; }

			// Token: 0x17000419 RID: 1049
			// (get) Token: 0x06001364 RID: 4964 RVA: 0x00043C6B File Offset: 0x00041E6B
			// (set) Token: 0x06001365 RID: 4965 RVA: 0x00043C73 File Offset: 0x00041E73
			public int width { get; private set; }

			// Token: 0x1700041A RID: 1050
			// (get) Token: 0x06001366 RID: 4966 RVA: 0x00043C7C File Offset: 0x00041E7C
			// (set) Token: 0x06001367 RID: 4967 RVA: 0x00043C84 File Offset: 0x00041E84
			public int height { get; private set; }

			// Token: 0x06001368 RID: 4968 RVA: 0x00043C90 File Offset: 0x00041E90
			public static UIRAtlasAllocator.Row Acquire(int offsetX, int offsetY, int width, int height)
			{
				UIRAtlasAllocator.Row row = UIRAtlasAllocator.Row.s_Pool.Get();
				row.offsetX = offsetX;
				row.offsetY = offsetY;
				row.width = width;
				row.height = height;
				row.Cursor = 0;
				return row;
			}

			// Token: 0x06001369 RID: 4969 RVA: 0x00043CD5 File Offset: 0x00041ED5
			public void Release()
			{
				UIRAtlasAllocator.Row.s_Pool.Release(this);
				this.offsetX = -1;
				this.offsetY = -1;
				this.width = -1;
				this.height = -1;
				this.Cursor = -1;
			}

			// Token: 0x040008CB RID: 2251
			private static ObjectPool<UIRAtlasAllocator.Row> s_Pool = new ObjectPool<UIRAtlasAllocator.Row>(() => new UIRAtlasAllocator.Row(), 100);

			// Token: 0x040008D0 RID: 2256
			public int Cursor;
		}

		// Token: 0x020002A5 RID: 677
		private class AreaNode
		{
			// Token: 0x0600136F RID: 4975 RVA: 0x00043D3C File Offset: 0x00041F3C
			public static UIRAtlasAllocator.AreaNode Acquire(RectInt rect)
			{
				UIRAtlasAllocator.AreaNode areaNode = UIRAtlasAllocator.AreaNode.s_Pool.Get();
				areaNode.rect = rect;
				areaNode.previous = null;
				areaNode.next = null;
				return areaNode;
			}

			// Token: 0x06001370 RID: 4976 RVA: 0x00043D6F File Offset: 0x00041F6F
			public void Release()
			{
				UIRAtlasAllocator.AreaNode.s_Pool.Release(this);
			}

			// Token: 0x06001371 RID: 4977 RVA: 0x00043D80 File Offset: 0x00041F80
			public void RemoveFromChain()
			{
				bool flag = this.previous != null;
				if (flag)
				{
					this.previous.next = this.next;
				}
				bool flag2 = this.next != null;
				if (flag2)
				{
					this.next.previous = this.previous;
				}
				this.previous = null;
				this.next = null;
			}

			// Token: 0x06001372 RID: 4978 RVA: 0x00043DD8 File Offset: 0x00041FD8
			public void AddAfter(UIRAtlasAllocator.AreaNode previous)
			{
				Assert.IsNull<UIRAtlasAllocator.AreaNode>(this.previous);
				Assert.IsNull<UIRAtlasAllocator.AreaNode>(this.next);
				this.previous = previous;
				bool flag = previous != null;
				if (flag)
				{
					this.next = previous.next;
					previous.next = this;
				}
				bool flag2 = this.next != null;
				if (flag2)
				{
					this.next.previous = this;
				}
			}

			// Token: 0x040008D2 RID: 2258
			private static ObjectPool<UIRAtlasAllocator.AreaNode> s_Pool = new ObjectPool<UIRAtlasAllocator.AreaNode>(() => new UIRAtlasAllocator.AreaNode(), 100);

			// Token: 0x040008D3 RID: 2259
			public RectInt rect;

			// Token: 0x040008D4 RID: 2260
			public UIRAtlasAllocator.AreaNode previous;

			// Token: 0x040008D5 RID: 2261
			public UIRAtlasAllocator.AreaNode next;
		}
	}
}
