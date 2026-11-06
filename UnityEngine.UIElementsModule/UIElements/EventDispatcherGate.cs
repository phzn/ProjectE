using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000183 RID: 387
	public struct EventDispatcherGate : IDisposable, IEquatable<EventDispatcherGate>
	{
		// Token: 0x06000C44 RID: 3140 RVA: 0x000312CC File Offset: 0x0002F4CC
		public EventDispatcherGate(EventDispatcher d)
		{
			bool flag = d == null;
			if (flag)
			{
				throw new ArgumentNullException("d");
			}
			this.m_Dispatcher = d;
			this.m_Dispatcher.CloseGate();
		}

		// Token: 0x06000C45 RID: 3141 RVA: 0x00031301 File Offset: 0x0002F501
		public void Dispose()
		{
			this.m_Dispatcher.OpenGate();
		}

		// Token: 0x06000C46 RID: 3142 RVA: 0x00031310 File Offset: 0x0002F510
		public bool Equals(EventDispatcherGate other)
		{
			return object.Equals(this.m_Dispatcher, other.m_Dispatcher);
		}

		// Token: 0x06000C47 RID: 3143 RVA: 0x00031334 File Offset: 0x0002F534
		public override bool Equals(object obj)
		{
			bool flag = obj == null;
			return !flag && obj is EventDispatcherGate && this.Equals((EventDispatcherGate)obj);
		}

		// Token: 0x06000C48 RID: 3144 RVA: 0x0003136C File Offset: 0x0002F56C
		public override int GetHashCode()
		{
			return (this.m_Dispatcher != null) ? this.m_Dispatcher.GetHashCode() : 0;
		}

		// Token: 0x06000C49 RID: 3145 RVA: 0x00031394 File Offset: 0x0002F594
		public static bool operator ==(EventDispatcherGate left, EventDispatcherGate right)
		{
			return left.Equals(right);
		}

		// Token: 0x06000C4A RID: 3146 RVA: 0x000313B0 File Offset: 0x0002F5B0
		public static bool operator !=(EventDispatcherGate left, EventDispatcherGate right)
		{
			return !left.Equals(right);
		}

		// Token: 0x040005DC RID: 1500
		private readonly EventDispatcher m_Dispatcher;
	}
}
