using System;
using System.Threading;
using Pathfinding.Collections;

namespace Pathfinding.Sync
{
	// Token: 0x02000238 RID: 568
	internal class BlockableChannel<T> where T : class
	{
		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000E4B RID: 3659 RVA: 0x00058AC4 File Offset: 0x00056CC4
		// (set) Token: 0x06000E4C RID: 3660 RVA: 0x00058ACC File Offset: 0x00056CCC
		public int numReceivers { get; private set; }

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06000E4D RID: 3661 RVA: 0x00058AD5 File Offset: 0x00056CD5
		// (set) Token: 0x06000E4E RID: 3662 RVA: 0x00058ADD File Offset: 0x00056CDD
		public bool isClosed { get; private set; }

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06000E4F RID: 3663 RVA: 0x00058AE8 File Offset: 0x00056CE8
		public bool isEmpty
		{
			get
			{
				object obj = this.lockObj;
				bool flag2;
				lock (obj)
				{
					flag2 = this.queue.Length == 0;
				}
				return flag2;
			}
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000E50 RID: 3664 RVA: 0x00058B34 File Offset: 0x00056D34
		public bool allReceiversBlocked
		{
			get
			{
				return this.blocked && this.waitingReceivers == this.numReceivers;
			}
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000E51 RID: 3665 RVA: 0x00058B52 File Offset: 0x00056D52
		// (set) Token: 0x06000E52 RID: 3666 RVA: 0x00058B5C File Offset: 0x00056D5C
		public bool isBlocked
		{
			get
			{
				return this.blocked;
			}
			set
			{
				object obj = this.lockObj;
				lock (obj)
				{
					this.blocked = value;
					if (!this.isClosed)
					{
						this.isStarving = value || this.queue.Length == 0;
					}
				}
			}
		}

		// Token: 0x06000E53 RID: 3667 RVA: 0x00058BC4 File Offset: 0x00056DC4
		public void Close()
		{
			object obj = this.lockObj;
			lock (obj)
			{
				this.isClosed = true;
				this.isStarving = false;
			}
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000E54 RID: 3668 RVA: 0x00058C0C File Offset: 0x00056E0C
		// (set) Token: 0x06000E55 RID: 3669 RVA: 0x00058C1D File Offset: 0x00056E1D
		private bool isStarving
		{
			get
			{
				return !this.starving.WaitOne(0);
			}
			set
			{
				if (value)
				{
					this.starving.Reset();
					return;
				}
				this.starving.Set();
			}
		}

		// Token: 0x06000E56 RID: 3670 RVA: 0x00058C3C File Offset: 0x00056E3C
		public void Reopen()
		{
			object obj = this.lockObj;
			lock (obj)
			{
				if (this.numReceivers != 0)
				{
					throw new InvalidOperationException("Can only reopen a channel after Close has been called on all receivers");
				}
				this.isClosed = false;
				this.isBlocked = false;
			}
		}

		// Token: 0x06000E57 RID: 3671 RVA: 0x00058C98 File Offset: 0x00056E98
		public BlockableChannel<T>.Receiver AddReceiver()
		{
			object obj = this.lockObj;
			BlockableChannel<T>.Receiver receiver;
			lock (obj)
			{
				if (this.isClosed)
				{
					throw new InvalidOperationException("Channel is closed");
				}
				int numReceivers = this.numReceivers;
				this.numReceivers = numReceivers + 1;
				receiver = new BlockableChannel<T>.Receiver(this);
			}
			return receiver;
		}

		// Token: 0x06000E58 RID: 3672 RVA: 0x00058D00 File Offset: 0x00056F00
		public void PushFront(T path)
		{
			object obj = this.lockObj;
			lock (obj)
			{
				if (this.isClosed)
				{
					throw new InvalidOperationException("Channel is closed");
				}
				this.queue.PushStart(path);
				if (!this.blocked)
				{
					this.isStarving = false;
				}
			}
		}

		// Token: 0x06000E59 RID: 3673 RVA: 0x00058D6C File Offset: 0x00056F6C
		public void Push(T path)
		{
			object obj = this.lockObj;
			lock (obj)
			{
				if (this.isClosed)
				{
					throw new InvalidOperationException("Channel is closed");
				}
				this.queue.PushEnd(path);
				if (!this.blocked)
				{
					this.isStarving = false;
				}
			}
		}

		// Token: 0x04000AC6 RID: 2758
		private readonly object lockObj = new object();

		// Token: 0x04000AC7 RID: 2759
		private CircularBuffer<T> queue = new CircularBuffer<T>(16);

		// Token: 0x04000AC9 RID: 2761
		private volatile int waitingReceivers;

		// Token: 0x04000ACA RID: 2762
		private ManualResetEvent starving = new ManualResetEvent(false);

		// Token: 0x04000ACB RID: 2763
		private volatile bool blocked;

		// Token: 0x02000239 RID: 569
		public enum PopState
		{
			// Token: 0x04000ACE RID: 2766
			Ok,
			// Token: 0x04000ACF RID: 2767
			Wait,
			// Token: 0x04000AD0 RID: 2768
			Closed
		}

		// Token: 0x0200023A RID: 570
		public struct Receiver
		{
			// Token: 0x06000E5B RID: 3675 RVA: 0x00058E04 File Offset: 0x00057004
			public Receiver(BlockableChannel<T> channel)
			{
				this.channel = channel;
			}

			// Token: 0x06000E5C RID: 3676 RVA: 0x00058E10 File Offset: 0x00057010
			public void Close()
			{
				object lockObj = this.channel.lockObj;
				lock (lockObj)
				{
					BlockableChannel<T> blockableChannel = this.channel;
					int numReceivers = blockableChannel.numReceivers;
					blockableChannel.numReceivers = numReceivers - 1;
				}
				this.channel = null;
			}

			// Token: 0x06000E5D RID: 3677 RVA: 0x00058E6C File Offset: 0x0005706C
			public BlockableChannel<T>.PopState Receive(out T item)
			{
				Interlocked.Increment(ref this.channel.waitingReceivers);
				BlockableChannel<T>.PopState popState;
				for (;;)
				{
					this.channel.starving.WaitOne();
					object lockObj = this.channel.lockObj;
					lock (lockObj)
					{
						if (this.channel.isClosed)
						{
							Interlocked.Decrement(ref this.channel.waitingReceivers);
							item = default(T);
							popState = BlockableChannel<T>.PopState.Closed;
						}
						else
						{
							if (this.channel.queue.Length == 0)
							{
								this.channel.isStarving = true;
							}
							if (this.channel.isStarving)
							{
								continue;
							}
							Interlocked.Decrement(ref this.channel.waitingReceivers);
							item = this.channel.queue.PopStart();
							popState = BlockableChannel<T>.PopState.Ok;
						}
					}
					break;
				}
				return popState;
			}

			// Token: 0x06000E5E RID: 3678 RVA: 0x00058F50 File Offset: 0x00057150
			public BlockableChannel<T>.PopState ReceiveNoBlock(bool blockedBefore, out T item)
			{
				item = default(T);
				if (!blockedBefore)
				{
					Interlocked.Increment(ref this.channel.waitingReceivers);
				}
				while (!this.channel.isStarving)
				{
					object lockObj = this.channel.lockObj;
					BlockableChannel<T>.PopState popState;
					lock (lockObj)
					{
						if (this.channel.isClosed)
						{
							Interlocked.Decrement(ref this.channel.waitingReceivers);
							popState = BlockableChannel<T>.PopState.Closed;
						}
						else
						{
							if (this.channel.queue.Length == 0)
							{
								this.channel.isStarving = true;
							}
							if (this.channel.isStarving)
							{
								continue;
							}
							Interlocked.Decrement(ref this.channel.waitingReceivers);
							item = this.channel.queue.PopStart();
							popState = BlockableChannel<T>.PopState.Ok;
						}
					}
					return popState;
				}
				return BlockableChannel<T>.PopState.Wait;
			}

			// Token: 0x04000AD1 RID: 2769
			private BlockableChannel<T> channel;
		}
	}
}
