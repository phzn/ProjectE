using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000201 RID: 513
	[EventCategory(EventCategory.Navigation)]
	public abstract class NavigationEventBase<T> : EventBase<T>, INavigationEvent where T : NavigationEventBase<T>, new()
	{
		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06000F1A RID: 3866 RVA: 0x00038CC3 File Offset: 0x00036EC3
		// (set) Token: 0x06000F1B RID: 3867 RVA: 0x00038CCB File Offset: 0x00036ECB
		public EventModifiers modifiers { get; protected set; }

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06000F1C RID: 3868 RVA: 0x00038CD4 File Offset: 0x00036ED4
		public bool shiftKey
		{
			get
			{
				return (this.modifiers & EventModifiers.Shift) > EventModifiers.None;
			}
		}

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06000F1D RID: 3869 RVA: 0x00038CF4 File Offset: 0x00036EF4
		public bool ctrlKey
		{
			get
			{
				return (this.modifiers & EventModifiers.Control) > EventModifiers.None;
			}
		}

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x06000F1E RID: 3870 RVA: 0x00038D14 File Offset: 0x00036F14
		public bool commandKey
		{
			get
			{
				return (this.modifiers & EventModifiers.Command) > EventModifiers.None;
			}
		}

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x06000F1F RID: 3871 RVA: 0x00038D34 File Offset: 0x00036F34
		public bool altKey
		{
			get
			{
				return (this.modifiers & EventModifiers.Alt) > EventModifiers.None;
			}
		}

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06000F20 RID: 3872 RVA: 0x00038D54 File Offset: 0x00036F54
		public bool actionKey
		{
			get
			{
				bool flag = Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer;
				bool flag2;
				if (flag)
				{
					flag2 = this.commandKey;
				}
				else
				{
					flag2 = this.ctrlKey;
				}
				return flag2;
			}
		}

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x06000F21 RID: 3873 RVA: 0x00038D8D File Offset: 0x00036F8D
		NavigationDeviceType INavigationEvent.deviceType
		{
			get
			{
				return this.deviceType;
			}
		}

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06000F22 RID: 3874 RVA: 0x00038D95 File Offset: 0x00036F95
		// (set) Token: 0x06000F23 RID: 3875 RVA: 0x00038D9D File Offset: 0x00036F9D
		internal NavigationDeviceType deviceType { get; private set; }

		// Token: 0x06000F24 RID: 3876 RVA: 0x00038DA6 File Offset: 0x00036FA6
		protected NavigationEventBase()
		{
			this.LocalInit();
		}

		// Token: 0x06000F25 RID: 3877 RVA: 0x00038DB7 File Offset: 0x00036FB7
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06000F26 RID: 3878 RVA: 0x00038DC8 File Offset: 0x00036FC8
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.Bubbles | EventBase.EventPropagation.TricklesDown | EventBase.EventPropagation.Cancellable | EventBase.EventPropagation.SkipDisabledElements;
			base.propagateToIMGUI = false;
			this.modifiers = EventModifiers.None;
			this.deviceType = NavigationDeviceType.Unknown;
		}

		// Token: 0x06000F27 RID: 3879 RVA: 0x00038DEC File Offset: 0x00036FEC
		public static T GetPooled(EventModifiers modifiers = EventModifiers.None)
		{
			T pooled = EventBase<T>.GetPooled();
			pooled.modifiers = modifiers;
			pooled.deviceType = NavigationDeviceType.Unknown;
			return pooled;
		}

		// Token: 0x06000F28 RID: 3880 RVA: 0x00038E20 File Offset: 0x00037020
		internal static T GetPooled(NavigationDeviceType deviceType, EventModifiers modifiers = EventModifiers.None)
		{
			T pooled = EventBase<T>.GetPooled();
			pooled.modifiers = modifiers;
			pooled.deviceType = deviceType;
			return pooled;
		}
	}
}
