using System;
using System.Collections.Concurrent;
using System.Threading;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace Features.PhoneCompanion
{
	// Token: 0x02000E54 RID: 3668
	internal sealed class PhoneVideoFrameEncoder : IDisposable
	{
		// Token: 0x170011FD RID: 4605
		// (get) Token: 0x06006D8B RID: 28043 RVA: 0x002BFF63 File Offset: 0x002BE163
		public int Backlog
		{
			get
			{
				return Volatile.Read(ref this._rawCount) + Volatile.Read(ref this._busy);
			}
		}

		// Token: 0x170011FE RID: 4606
		// (get) Token: 0x06006D8C RID: 28044 RVA: 0x002BFF7C File Offset: 0x002BE17C
		public bool HasPendingWork
		{
			get
			{
				return this.Backlog > 0;
			}
		}

		// Token: 0x170011FF RID: 4607
		// (get) Token: 0x06006D8D RID: 28045 RVA: 0x002BFF87 File Offset: 0x002BE187
		public string Error
		{
			get
			{
				return this._error;
			}
		}

		// Token: 0x06006D8E RID: 28046 RVA: 0x002BFF90 File Offset: 0x002BE190
		public PhoneVideoFrameEncoder(int width, int height, int quality, int maxQueuedFrames)
		{
			this._width = (uint)Math.Max(1, width);
			this._height = (uint)Math.Max(1, height);
			this._quality = Mathf.Clamp(quality, 35, 95);
			this._maxQueuedFrames = Mathf.Clamp(maxQueuedFrames, 1, 6);
			this._thread = new Thread(new ThreadStart(this.EncodeLoop))
			{
				IsBackground = true,
				Name = "Phone Video JPEG Encoder",
				Priority = ThreadPriority.BelowNormal
			};
			this._thread.Start();
		}

		// Token: 0x06006D8F RID: 28047 RVA: 0x002C004C File Offset: 0x002BE24C
		public bool TryEnqueue(int targetFrameIndex, byte[] rgb24)
		{
			if (!this._accepting || rgb24 == null || rgb24.Length == 0 || this.Backlog >= this._maxQueuedFrames)
			{
				return false;
			}
			this._rawFrames.Enqueue(new PhoneVideoFrameEncoder.RawFrame
			{
				targetFrameIndex = targetFrameIndex,
				rgb24 = rgb24
			});
			Interlocked.Increment(ref this._rawCount);
			this._wake.Set();
			return true;
		}

		// Token: 0x06006D90 RID: 28048 RVA: 0x002C00B1 File Offset: 0x002BE2B1
		public bool TryDequeue(out PhoneVideoFrameEncoder.EncodedFrame frame)
		{
			if (this._encodedFrames.TryDequeue(out frame))
			{
				Interlocked.Decrement(ref this._encodedCount);
				return true;
			}
			return false;
		}

		// Token: 0x06006D91 RID: 28049 RVA: 0x002C00D0 File Offset: 0x002BE2D0
		public void Complete()
		{
			this._accepting = false;
			this._wake.Set();
		}

		// Token: 0x06006D92 RID: 28050 RVA: 0x002C00E8 File Offset: 0x002BE2E8
		private void EncodeLoop()
		{
			while (this._running || !this._rawFrames.IsEmpty)
			{
				PhoneVideoFrameEncoder.RawFrame rawFrame;
				if (!this._rawFrames.TryDequeue(out rawFrame))
				{
					this._wake.WaitOne(20);
				}
				else
				{
					Interlocked.Decrement(ref this._rawCount);
					Interlocked.Exchange(ref this._busy, 1);
					try
					{
						byte[] array = ImageConversion.EncodeArrayToJPG(rawFrame.rgb24, GraphicsFormat.R8G8B8_UNorm, this._width, this._height, 0U, this._quality);
						if (array != null && array.Length > 4)
						{
							this._encodedFrames.Enqueue(new PhoneVideoFrameEncoder.EncodedFrame(rawFrame.targetFrameIndex, array));
							Interlocked.Increment(ref this._encodedCount);
						}
					}
					catch (Exception ex)
					{
						this._error = ex.Message;
						this._accepting = false;
					}
					finally
					{
						Interlocked.Exchange(ref this._busy, 0);
						rawFrame.rgb24 = null;
					}
				}
			}
		}

		// Token: 0x06006D93 RID: 28051 RVA: 0x002C01EC File Offset: 0x002BE3EC
		public void Dispose()
		{
			this._accepting = false;
			this._running = false;
			this._wake.Set();
			if (this._thread != null && this._thread.IsAlive && Thread.CurrentThread != this._thread)
			{
				this._thread.Join(2000);
			}
			if (this._thread == null || !this._thread.IsAlive)
			{
				this._wake.Dispose();
				PhoneVideoFrameEncoder.RawFrame rawFrame;
				while (this._rawFrames.TryDequeue(out rawFrame))
				{
				}
				Interlocked.Exchange(ref this._rawCount, 0);
			}
		}

		// Token: 0x04007029 RID: 28713
		private readonly ConcurrentQueue<PhoneVideoFrameEncoder.RawFrame> _rawFrames = new ConcurrentQueue<PhoneVideoFrameEncoder.RawFrame>();

		// Token: 0x0400702A RID: 28714
		private readonly ConcurrentQueue<PhoneVideoFrameEncoder.EncodedFrame> _encodedFrames = new ConcurrentQueue<PhoneVideoFrameEncoder.EncodedFrame>();

		// Token: 0x0400702B RID: 28715
		private readonly AutoResetEvent _wake = new AutoResetEvent(false);

		// Token: 0x0400702C RID: 28716
		private readonly Thread _thread;

		// Token: 0x0400702D RID: 28717
		private readonly uint _width;

		// Token: 0x0400702E RID: 28718
		private readonly uint _height;

		// Token: 0x0400702F RID: 28719
		private readonly int _quality;

		// Token: 0x04007030 RID: 28720
		private readonly int _maxQueuedFrames;

		// Token: 0x04007031 RID: 28721
		private volatile bool _accepting = true;

		// Token: 0x04007032 RID: 28722
		private volatile bool _running = true;

		// Token: 0x04007033 RID: 28723
		private int _rawCount;

		// Token: 0x04007034 RID: 28724
		private int _encodedCount;

		// Token: 0x04007035 RID: 28725
		private int _busy;

		// Token: 0x04007036 RID: 28726
		private string _error;

		// Token: 0x02000E55 RID: 3669
		internal readonly struct EncodedFrame
		{
			// Token: 0x06006D94 RID: 28052 RVA: 0x002C0286 File Offset: 0x002BE486
			public EncodedFrame(int targetFrameIndex, byte[] jpeg)
			{
				this.targetFrameIndex = targetFrameIndex;
				this.jpeg = jpeg;
			}

			// Token: 0x04007037 RID: 28727
			public readonly int targetFrameIndex;

			// Token: 0x04007038 RID: 28728
			public readonly byte[] jpeg;
		}

		// Token: 0x02000E56 RID: 3670
		private sealed class RawFrame
		{
			// Token: 0x04007039 RID: 28729
			public int targetFrameIndex;

			// Token: 0x0400703A RID: 28730
			public byte[] rgb24;
		}
	}
}
