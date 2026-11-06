using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001A5 RID: 421
	[EventCategory(EventCategory.Default)]
	public abstract class EventBase<T> : EventBase where T : EventBase<T>, new()
	{
		// Token: 0x06000CFE RID: 3326 RVA: 0x000331C1 File Offset: 0x000313C1
		internal static void SetCreateFunction(Func<T> createMethod)
		{
			EventBase<T>.s_Pool.CreateFunc = createMethod;
		}

		// Token: 0x06000CFF RID: 3327 RVA: 0x000331CF File Offset: 0x000313CF
		protected EventBase()
			: base(EventBase<T>.EventCategory)
		{
			this.m_RefCount = 0;
		}

		// Token: 0x06000D00 RID: 3328 RVA: 0x000331E8 File Offset: 0x000313E8
		public static long TypeId()
		{
			return EventBase<T>.s_TypeId;
		}

		// Token: 0x06000D01 RID: 3329 RVA: 0x00033200 File Offset: 0x00031400
		protected override void Init()
		{
			base.Init();
			bool flag = this.m_RefCount != 0;
			if (flag)
			{
				Debug.Log("Event improperly released.");
				this.m_RefCount = 0;
			}
		}

		// Token: 0x06000D02 RID: 3330 RVA: 0x00033238 File Offset: 0x00031438
		public static T GetPooled()
		{
			T t = EventBase<T>.s_Pool.Get();
			t.Init();
			t.pooled = true;
			t.Acquire();
			return t;
		}

		// Token: 0x06000D03 RID: 3331 RVA: 0x0003327C File Offset: 0x0003147C
		internal static T GetPooled(EventBase e)
		{
			T pooled = EventBase<T>.GetPooled();
			bool flag = e != null;
			if (flag)
			{
				pooled.SetTriggerEventId(e.eventId);
			}
			return pooled;
		}

		// Token: 0x06000D04 RID: 3332 RVA: 0x000332B4 File Offset: 0x000314B4
		private static void ReleasePooled(T evt)
		{
			bool pooled = evt.pooled;
			if (pooled)
			{
				evt.Init();
				EventBase<T>.s_Pool.Release(evt);
				evt.pooled = false;
			}
		}

		// Token: 0x06000D05 RID: 3333 RVA: 0x000332F8 File Offset: 0x000314F8
		internal override void Acquire()
		{
			this.m_RefCount++;
		}

		// Token: 0x06000D06 RID: 3334 RVA: 0x0003330C File Offset: 0x0003150C
		public sealed override void Dispose()
		{
			int num = this.m_RefCount - 1;
			this.m_RefCount = num;
			bool flag = num == 0;
			if (flag)
			{
				EventBase<T>.ReleasePooled((T)((object)this));
			}
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06000D07 RID: 3335 RVA: 0x00033340 File Offset: 0x00031540
		public override long eventTypeId
		{
			get
			{
				return EventBase<T>.s_TypeId;
			}
		}

		// Token: 0x04000628 RID: 1576
		private static readonly long s_TypeId = EventBase.RegisterEventType();

		// Token: 0x04000629 RID: 1577
		private static readonly ObjectPool<T> s_Pool = new ObjectPool<T>(() => new T(), 100);

		// Token: 0x0400062A RID: 1578
		private int m_RefCount;

		// Token: 0x0400062B RID: 1579
		internal static readonly EventCategory EventCategory = EventInterestReflectionUtils.GetEventCategory(typeof(T));
	}
}
