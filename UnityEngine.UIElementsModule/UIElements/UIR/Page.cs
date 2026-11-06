using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace UnityEngine.UIElements.UIR
{
	// Token: 0x0200043C RID: 1084
	internal class Page : IDisposable
	{
		// Token: 0x0600223B RID: 8763 RVA: 0x0008344A File Offset: 0x0008164A
		public Page(uint vertexMaxCount, uint indexMaxCount, uint maxQueuedFrameCount, bool mockPage)
		{
			vertexMaxCount = Math.Min(vertexMaxCount, 65536U);
			this.vertices = new Page.DataSet<Vertex>(Utility.GPUBufferType.Vertex, vertexMaxCount, maxQueuedFrameCount, 32U, mockPage);
			this.indices = new Page.DataSet<ushort>(Utility.GPUBufferType.Index, indexMaxCount, maxQueuedFrameCount, 32U, mockPage);
		}

		// Token: 0x170007D6 RID: 2006
		// (get) Token: 0x0600223C RID: 8764 RVA: 0x00083485 File Offset: 0x00081685
		// (set) Token: 0x0600223D RID: 8765 RVA: 0x0008348D File Offset: 0x0008168D
		private protected bool disposed { protected get; private set; }

		// Token: 0x0600223E RID: 8766 RVA: 0x00083496 File Offset: 0x00081696
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600223F RID: 8767 RVA: 0x000834A8 File Offset: 0x000816A8
		protected virtual void Dispose(bool disposing)
		{
			bool disposed = this.disposed;
			if (!disposed)
			{
				if (disposing)
				{
					this.indices.Dispose();
					this.vertices.Dispose();
				}
				this.disposed = true;
			}
		}

		// Token: 0x170007D7 RID: 2007
		// (get) Token: 0x06002240 RID: 8768 RVA: 0x000834EC File Offset: 0x000816EC
		public bool isEmpty
		{
			get
			{
				return this.vertices.allocator.isEmpty && this.indices.allocator.isEmpty;
			}
		}

		// Token: 0x04000EF3 RID: 3827
		public Page.DataSet<Vertex> vertices;

		// Token: 0x04000EF4 RID: 3828
		public Page.DataSet<ushort> indices;

		// Token: 0x04000EF5 RID: 3829
		public Page next;

		// Token: 0x04000EF6 RID: 3830
		public int framesEmpty;

		// Token: 0x0200043D RID: 1085
		public class DataSet<T> : IDisposable where T : struct
		{
			// Token: 0x06002241 RID: 8769 RVA: 0x00083524 File Offset: 0x00081724
			public DataSet(Utility.GPUBufferType bufferType, uint totalCount, uint maxQueuedFrameCount, uint updateRangePoolSize, bool mockBuffer)
			{
				bool flag = !mockBuffer;
				if (flag)
				{
					this.gpuData = new Utility.GPUBuffer<T>((int)totalCount, bufferType);
				}
				this.cpuData = new NativeArray<T>((int)totalCount, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
				this.allocator = new GPUBufferAllocator(totalCount);
				bool flag2 = !mockBuffer;
				if (flag2)
				{
					this.m_ElemStride = (uint)this.gpuData.ElementStride;
				}
				this.m_UpdateRangePoolSize = updateRangePoolSize;
				uint num = this.m_UpdateRangePoolSize * maxQueuedFrameCount;
				this.updateRanges = new NativeArray<GfxUpdateBufferRange>((int)num, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
				this.m_UpdateRangeMin = uint.MaxValue;
				this.m_UpdateRangeMax = 0U;
				this.m_UpdateRangesEnqueued = 0U;
				this.m_UpdateRangesBatchStart = 0U;
			}

			// Token: 0x170007D8 RID: 2008
			// (get) Token: 0x06002242 RID: 8770 RVA: 0x000835BE File Offset: 0x000817BE
			// (set) Token: 0x06002243 RID: 8771 RVA: 0x000835C6 File Offset: 0x000817C6
			private protected bool disposed { protected get; private set; }

			// Token: 0x06002244 RID: 8772 RVA: 0x000835CF File Offset: 0x000817CF
			public void Dispose()
			{
				this.Dispose(true);
				GC.SuppressFinalize(this);
			}

			// Token: 0x06002245 RID: 8773 RVA: 0x000835E4 File Offset: 0x000817E4
			public void Dispose(bool disposing)
			{
				bool disposed = this.disposed;
				if (!disposed)
				{
					if (disposing)
					{
						Utility.GPUBuffer<T> gpubuffer = this.gpuData;
						if (gpubuffer != null)
						{
							gpubuffer.Dispose();
						}
						this.cpuData.Dispose();
						this.updateRanges.Dispose();
					}
					this.disposed = true;
				}
			}

			// Token: 0x06002246 RID: 8774 RVA: 0x0008363C File Offset: 0x0008183C
			public void RegisterUpdate(uint start, uint size)
			{
				Debug.Assert((ulong)(start + size) <= (ulong)((long)this.cpuData.Length));
				int num = (int)(this.m_UpdateRangesBatchStart + this.m_UpdateRangesEnqueued);
				bool flag = this.m_UpdateRangesEnqueued > 0U;
				if (flag)
				{
					int num2 = num - 1;
					GfxUpdateBufferRange gfxUpdateBufferRange = this.updateRanges[num2];
					uint num3 = start * this.m_ElemStride;
					bool flag2 = gfxUpdateBufferRange.offsetFromWriteStart + gfxUpdateBufferRange.size == num3;
					if (flag2)
					{
						this.updateRanges[num2] = new GfxUpdateBufferRange
						{
							source = gfxUpdateBufferRange.source,
							offsetFromWriteStart = gfxUpdateBufferRange.offsetFromWriteStart,
							size = gfxUpdateBufferRange.size + size * this.m_ElemStride
						};
						this.m_UpdateRangeMax = Math.Max(this.m_UpdateRangeMax, start + size);
						return;
					}
				}
				this.m_UpdateRangeMin = Math.Min(this.m_UpdateRangeMin, start);
				this.m_UpdateRangeMax = Math.Max(this.m_UpdateRangeMax, start + size);
				bool flag3 = this.m_UpdateRangesEnqueued == this.m_UpdateRangePoolSize;
				if (flag3)
				{
					this.m_UpdateRangesSaturated = true;
				}
				else
				{
					UIntPtr uintPtr = new UIntPtr(this.cpuData.Slice<T>((int)start, (int)size).GetUnsafeReadOnlyPtr<T>());
					this.updateRanges[num] = new GfxUpdateBufferRange
					{
						source = uintPtr,
						offsetFromWriteStart = start * this.m_ElemStride,
						size = size * this.m_ElemStride
					};
					this.m_UpdateRangesEnqueued += 1U;
				}
			}

			// Token: 0x06002247 RID: 8775 RVA: 0x000837C8 File Offset: 0x000819C8
			private bool HasMappedBufferRange()
			{
				return Utility.HasMappedBufferRange();
			}

			// Token: 0x06002248 RID: 8776 RVA: 0x000837E0 File Offset: 0x000819E0
			public void SendUpdates()
			{
				bool flag = this.HasMappedBufferRange();
				if (flag)
				{
					this.SendPartialRanges();
				}
				else
				{
					this.SendFullRange();
				}
			}

			// Token: 0x06002249 RID: 8777 RVA: 0x00083808 File Offset: 0x00081A08
			public void SendFullRange()
			{
				uint num = (uint)((long)this.cpuData.Length * (long)((ulong)this.m_ElemStride));
				this.updateRanges[(int)this.m_UpdateRangesBatchStart] = new GfxUpdateBufferRange
				{
					source = new UIntPtr(this.cpuData.GetUnsafeReadOnlyPtr<T>()),
					offsetFromWriteStart = 0U,
					size = num
				};
				Utility.GPUBuffer<T> gpubuffer = this.gpuData;
				if (gpubuffer != null)
				{
					gpubuffer.UpdateRanges(this.updateRanges.Slice<GfxUpdateBufferRange>((int)this.m_UpdateRangesBatchStart, 1), 0, (int)num);
				}
				this.ResetUpdateState();
			}

			// Token: 0x0600224A RID: 8778 RVA: 0x0008389C File Offset: 0x00081A9C
			public void SendPartialRanges()
			{
				bool flag = this.m_UpdateRangesEnqueued == 0U;
				if (!flag)
				{
					bool updateRangesSaturated = this.m_UpdateRangesSaturated;
					if (updateRangesSaturated)
					{
						uint num = this.m_UpdateRangeMax - this.m_UpdateRangeMin;
						this.m_UpdateRangesEnqueued = 1U;
						this.updateRanges[(int)this.m_UpdateRangesBatchStart] = new GfxUpdateBufferRange
						{
							source = new UIntPtr(this.cpuData.Slice<T>((int)this.m_UpdateRangeMin, (int)num).GetUnsafeReadOnlyPtr<T>()),
							offsetFromWriteStart = this.m_UpdateRangeMin * this.m_ElemStride,
							size = num * this.m_ElemStride
						};
					}
					uint num2 = this.m_UpdateRangeMin * this.m_ElemStride;
					uint num3 = this.m_UpdateRangeMax * this.m_ElemStride;
					bool flag2 = num2 > 0U;
					if (flag2)
					{
						for (uint num4 = 0U; num4 < this.m_UpdateRangesEnqueued; num4 += 1U)
						{
							int num5 = (int)(num4 + this.m_UpdateRangesBatchStart);
							this.updateRanges[num5] = new GfxUpdateBufferRange
							{
								source = this.updateRanges[num5].source,
								offsetFromWriteStart = this.updateRanges[num5].offsetFromWriteStart - num2,
								size = this.updateRanges[num5].size
							};
						}
					}
					Utility.GPUBuffer<T> gpubuffer = this.gpuData;
					if (gpubuffer != null)
					{
						gpubuffer.UpdateRanges(this.updateRanges.Slice<GfxUpdateBufferRange>((int)this.m_UpdateRangesBatchStart, (int)this.m_UpdateRangesEnqueued), (int)num2, (int)num3);
					}
					this.ResetUpdateState();
				}
			}

			// Token: 0x0600224B RID: 8779 RVA: 0x00083A30 File Offset: 0x00081C30
			private void ResetUpdateState()
			{
				this.m_UpdateRangeMin = uint.MaxValue;
				this.m_UpdateRangeMax = 0U;
				this.m_UpdateRangesEnqueued = 0U;
				this.m_UpdateRangesBatchStart += this.m_UpdateRangePoolSize;
				bool flag = (ulong)this.m_UpdateRangesBatchStart >= (ulong)((long)this.updateRanges.Length);
				if (flag)
				{
					this.m_UpdateRangesBatchStart = 0U;
				}
				this.m_UpdateRangesSaturated = false;
			}

			// Token: 0x04000EF8 RID: 3832
			public Utility.GPUBuffer<T> gpuData;

			// Token: 0x04000EF9 RID: 3833
			public NativeArray<T> cpuData;

			// Token: 0x04000EFA RID: 3834
			public NativeArray<GfxUpdateBufferRange> updateRanges;

			// Token: 0x04000EFB RID: 3835
			public GPUBufferAllocator allocator;

			// Token: 0x04000EFC RID: 3836
			private readonly uint m_UpdateRangePoolSize;

			// Token: 0x04000EFD RID: 3837
			private uint m_ElemStride;

			// Token: 0x04000EFE RID: 3838
			private uint m_UpdateRangeMin;

			// Token: 0x04000EFF RID: 3839
			private uint m_UpdateRangeMax;

			// Token: 0x04000F00 RID: 3840
			private uint m_UpdateRangesEnqueued;

			// Token: 0x04000F01 RID: 3841
			private uint m_UpdateRangesBatchStart;

			// Token: 0x04000F02 RID: 3842
			private bool m_UpdateRangesSaturated;
		}
	}
}
