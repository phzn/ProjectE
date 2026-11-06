using System;
using System.Collections;
using System.ComponentModel;
using System.Security.Permissions;
using System.Timers;

namespace Microsoft.Win32
{
	/// <summary>Provides access to system event notifications. This class cannot be inherited.</summary>
	// Token: 0x0200012A RID: 298
	[PermissionSet(SecurityAction.LinkDemand, Unrestricted = true)]
	public sealed class SystemEvents
	{
		// Token: 0x060006DF RID: 1759 RVA: 0x0000219B File Offset: 0x0000039B
		private SystemEvents()
		{
		}

		/// <summary>Creates a new window timer associated with the system events window.</summary>
		/// <param name="interval">Specifies the interval between timer notifications, in milliseconds.</param>
		/// <returns>The ID of the new timer.</returns>
		/// <exception cref="T:System.ArgumentException">The interval is less than or equal to zero.</exception>
		/// <exception cref="T:System.InvalidOperationException">System event notifications are not supported under the current context. Server processes, for example, might not support global system event notifications.</exception>
		/// <exception cref="T:System.Runtime.InteropServices.ExternalException">The attempt to create a system events window thread did not succeed, or the attempt to create the timer did not succeed.</exception>
		// Token: 0x060006E0 RID: 1760 RVA: 0x00013704 File Offset: 0x00011904
		public static IntPtr CreateTimer(int interval)
		{
			int hashCode = Guid.NewGuid().GetHashCode();
			Timer timer = new Timer((double)interval);
			timer.Elapsed += SystemEvents.InternalTimerElapsed;
			SystemEvents.TimerStore.Add(hashCode, timer);
			return new IntPtr(hashCode);
		}

		/// <summary>Terminates the timer specified by the given id.</summary>
		/// <param name="timerId">The ID of the timer to terminate.</param>
		/// <exception cref="T:System.InvalidOperationException">System event notifications are not supported under the current context. Server processes, for example, might not support global system event notifications.</exception>
		/// <exception cref="T:System.Runtime.InteropServices.ExternalException">The attempt to create a system events window thread did not succeed, or the attempt to terminate the timer did not succeed.</exception>
		// Token: 0x060006E1 RID: 1761 RVA: 0x00013758 File Offset: 0x00011958
		public static void KillTimer(IntPtr timerId)
		{
			Timer timer = (Timer)SystemEvents.TimerStore[timerId.GetHashCode()];
			timer.Stop();
			timer.Elapsed -= SystemEvents.InternalTimerElapsed;
			timer.Dispose();
			SystemEvents.TimerStore.Remove(timerId.GetHashCode());
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x000137B3 File Offset: 0x000119B3
		private static void InternalTimerElapsed(object e, ElapsedEventArgs args)
		{
			if (SystemEvents.TimerElapsed != null)
			{
				SystemEvents.TimerElapsed(null, new TimerElapsedEventArgs(IntPtr.Zero));
			}
		}

		/// <summary>Invokes the specified delegate using the thread that listens for system events.</summary>
		/// <param name="method">A delegate to invoke using the thread that listens for system events.</param>
		/// <exception cref="T:System.InvalidOperationException">System event notifications are not supported under the current context. Server processes, for example, might not support global system event notifications.</exception>
		/// <exception cref="T:System.Runtime.InteropServices.ExternalException">The attempt to create a system events window thread did not succeed.</exception>
		// Token: 0x060006E3 RID: 1763 RVA: 0x0000829A File Offset: 0x0000649A
		[MonoTODO]
		public static void InvokeOnEventsThread(Delegate method)
		{
			throw new NotImplementedException();
		}

		/// <summary>Occurs when the user changes the display settings.</summary>
		/// <exception cref="T:System.InvalidOperationException">System event notifications are not supported under the current context. Server processes, for example, might not support global system event notifications.</exception>
		/// <exception cref="T:System.Runtime.InteropServices.ExternalException">The attempt to create a system events window thread did not succeed.</exception>
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060006E4 RID: 1764 RVA: 0x00003917 File Offset: 0x00001B17
		// (remove) Token: 0x060006E5 RID: 1765 RVA: 0x00003917 File Offset: 0x00001B17
		[MonoTODO]
		public static event EventHandler DisplaySettingsChanged
		{
			add
			{
			}
			remove
			{
			}
		}

		/// <summary>Occurs when the display settings are changing.</summary>
		/// <exception cref="T:System.InvalidOperationException">System event notifications are not supported under the current context. Server processes, for example, might not support global system event notifications.</exception>
		/// <exception cref="T:System.Runtime.InteropServices.ExternalException">The attempt to create a system events window thread did not succeed.</exception>
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060006E6 RID: 1766 RVA: 0x00003917 File Offset: 0x00001B17
		// (remove) Token: 0x060006E7 RID: 1767 RVA: 0x00003917 File Offset: 0x00001B17
		[MonoTODO("Currently does nothing on Mono")]
		public static event EventHandler DisplaySettingsChanging
		{
			add
			{
			}
			remove
			{
			}
		}

		/// <summary>Occurs before the thread that listens for system events is terminated.</summary>
		/// <exception cref="T:System.InvalidOperationException">System event notifications are not supported under the current context. Server processes, for example, might not support global system event notifications.</exception>
		/// <exception cref="T:System.Runtime.InteropServices.ExternalException">The attempt to create a system events window thread did not succeed.</exception>
		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060006E8 RID: 1768 RVA: 0x00003917 File Offset: 0x00001B17
		// (remove) Token: 0x060006E9 RID: 1769 RVA: 0x00003917 File Offset: 0x00001B17
		[MonoTODO("Currently does nothing on Mono")]
		public static event EventHandler EventsThreadShutdown
		{
			add
			{
			}
			remove
			{
			}
		}

		/// <summary>Occurs when the user adds fonts to or removes fonts from the system.</summary>
		/// <exception cref="T:System.InvalidOperationException">System event notifications are not supported under the current context. Server processes, for example, might not support global system event notifications.</exception>
		/// <exception cref="T:System.Runtime.InteropServices.ExternalException">The attempt to create a system events window thread did not succeed.</exception>
		// Token: 0x14000005 RID: 5
		// (add) Token: 0x060006EA RID: 1770 RVA: 0x00003917 File Offset: 0x00001B17
		// (remove) Token: 0x060006EB RID: 1771 RVA: 0x00003917 File Offset: 0x00001B17
		[MonoTODO("Currently does nothing on Mono")]
		public static event EventHandler InstalledFontsChanged
		{
			add
			{
			}
			remove
			{
			}
		}

		/// <summary>Occurs when the system is running out of available RAM.</summary>
		/// <exception cref="T:System.InvalidOperationException">System event notifications are not supported under the current context. Server processes, for example, might not support global system event notifications.</exception>
		/// <exception cref="T:System.Runtime.InteropServices.ExternalException">The attempt to create a system events window thread did not succeed.</exception>
		// Token: 0x14000006 RID: 6
		// (add) Token: 0x060006EC RID: 1772 RVA: 0x00003917 File Offset: 0x00001B17
		// (remove) Token: 0x060006ED RID: 1773 RVA: 0x00003917 File Offset: 0x00001B17
		[MonoTODO("Currently does nothing on Mono")]
		[Browsable(false)]
		[Obsolete("")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static event EventHandler LowMemory
		{
			add
			{
			}
			remove
			{
			}
		}

		/// <summary>Occurs when the user switches to an application that uses a different palette.</summary>
		/// <exception cref="T:System.InvalidOperationException">System event notifications are not supported under the current context. Server processes, for example, might not support global system event notifications.</exception>
		/// <exception cref="T:System.Runtime.InteropServices.ExternalException">The attempt to create a system events window thread did not succeed.</exception>
		// Token: 0x14000007 RID: 7
		// (add) Token: 0x060006EE RID: 1774 RVA: 0x00003917 File Offset: 0x00001B17
		// (remove) Token: 0x060006EF RID: 1775 RVA: 0x00003917 File Offset: 0x00001B17
		[MonoTODO("Currently does nothing on Mono")]
		public static event EventHandler PaletteChanged
		{
			add
			{
			}
			remove
			{
			}
		}

		/// <summary>Occurs when the user suspends or resumes the system.</summary>
		/// <exception cref="T:System.InvalidOperationException">System event notifications are not supported under the current context. Server processes, for example, might not support global system event notifications.</exception>
		/// <exception cref="T:System.Runtime.InteropServices.ExternalException">The attempt to create a system events window thread did not succeed.</exception>
		// Token: 0x14000008 RID: 8
		// (add) Token: 0x060006F0 RID: 1776 RVA: 0x00003917 File Offset: 0x00001B17
		// (remove) Token: 0x060006F1 RID: 1777 RVA: 0x00003917 File Offset: 0x00001B17
		[MonoTODO("Currently does nothing on Mono")]
		public static event PowerModeChangedEventHandler PowerModeChanged
		{
			add
			{
			}
			remove
			{
			}
		}

		/// <summary>Occurs when the user is logging off or shutting down the system.</summary>
		/// <exception cref="T:System.InvalidOperationException">System event notifications are not supported under the current context. Server processes, for example, might not support global system event notifications.</exception>
		/// <exception cref="T:System.Runtime.InteropServices.ExternalException">The attempt to create a system events window thread did not succeed.</exception>
		// Token: 0x14000009 RID: 9
		// (add) Token: 0x060006F2 RID: 1778 RVA: 0x00003917 File Offset: 0x00001B17
		// (remove) Token: 0x060006F3 RID: 1779 RVA: 0x00003917 File Offset: 0x00001B17
		[MonoTODO("Currently does nothing on Mono")]
		public static event SessionEndedEventHandler SessionEnded
		{
			add
			{
			}
			remove
			{
			}
		}

		/// <summary>Occurs when the user is trying to log off or shut down the system.</summary>
		/// <exception cref="T:System.InvalidOperationException">System event notifications are not supported under the current context. Server processes, for example, might not support global system event notifications.</exception>
		/// <exception cref="T:System.Runtime.InteropServices.ExternalException">The attempt to create a system events window thread did not succeed.</exception>
		// Token: 0x1400000A RID: 10
		// (add) Token: 0x060006F4 RID: 1780 RVA: 0x00003917 File Offset: 0x00001B17
		// (remove) Token: 0x060006F5 RID: 1781 RVA: 0x00003917 File Offset: 0x00001B17
		[MonoTODO("Currently does nothing on Mono")]
		public static event SessionEndingEventHandler SessionEnding
		{
			add
			{
			}
			remove
			{
			}
		}

		/// <summary>Occurs when the currently logged-in user has changed.</summary>
		/// <exception cref="T:System.InvalidOperationException">System event notifications are not supported under the current context. Server processes, for example, might not support global system event notifications.</exception>
		/// <exception cref="T:System.Runtime.InteropServices.ExternalException">The attempt to create a system events window thread did not succeed.</exception>
		// Token: 0x1400000B RID: 11
		// (add) Token: 0x060006F6 RID: 1782 RVA: 0x00003917 File Offset: 0x00001B17
		// (remove) Token: 0x060006F7 RID: 1783 RVA: 0x00003917 File Offset: 0x00001B17
		[MonoTODO("Currently does nothing on Mono")]
		public static event SessionSwitchEventHandler SessionSwitch
		{
			add
			{
			}
			remove
			{
			}
		}

		/// <summary>Occurs when the user changes the time on the system clock.</summary>
		/// <exception cref="T:System.InvalidOperationException">System event notifications are not supported under the current context. Server processes, for example, might not support global system event notifications.</exception>
		/// <exception cref="T:System.Runtime.InteropServices.ExternalException">The attempt to create a system events window thread did not succeed.</exception>
		// Token: 0x1400000C RID: 12
		// (add) Token: 0x060006F8 RID: 1784 RVA: 0x00003917 File Offset: 0x00001B17
		// (remove) Token: 0x060006F9 RID: 1785 RVA: 0x00003917 File Offset: 0x00001B17
		[MonoTODO("Currently does nothing on Mono")]
		public static event EventHandler TimeChanged
		{
			add
			{
			}
			remove
			{
			}
		}

		/// <summary>Occurs when a windows timer interval has expired.</summary>
		/// <exception cref="T:System.InvalidOperationException">System event notifications are not supported under the current context. Server processes, for example, might not support global system event notifications.</exception>
		/// <exception cref="T:System.Runtime.InteropServices.ExternalException">The attempt to create a system events window thread did not succeed.</exception>
		// Token: 0x1400000D RID: 13
		// (add) Token: 0x060006FA RID: 1786 RVA: 0x000137D4 File Offset: 0x000119D4
		// (remove) Token: 0x060006FB RID: 1787 RVA: 0x00013808 File Offset: 0x00011A08
		public static event TimerElapsedEventHandler TimerElapsed;

		/// <summary>Occurs when a user preference has changed.</summary>
		/// <exception cref="T:System.InvalidOperationException">System event notifications are not supported under the current context. Server processes, for example, might not support global system event notifications.</exception>
		/// <exception cref="T:System.Runtime.InteropServices.ExternalException">The attempt to create a system events window thread did not succeed.</exception>
		// Token: 0x1400000E RID: 14
		// (add) Token: 0x060006FC RID: 1788 RVA: 0x00003917 File Offset: 0x00001B17
		// (remove) Token: 0x060006FD RID: 1789 RVA: 0x00003917 File Offset: 0x00001B17
		[MonoTODO("Currently does nothing on Mono")]
		public static event UserPreferenceChangedEventHandler UserPreferenceChanged
		{
			add
			{
			}
			remove
			{
			}
		}

		/// <summary>Occurs when a user preference is changing.</summary>
		/// <exception cref="T:System.InvalidOperationException">System event notifications are not supported under the current context. Server processes, for example, might not support global system event notifications.</exception>
		/// <exception cref="T:System.Runtime.InteropServices.ExternalException">The attempt to create a system events window thread did not succeed.</exception>
		// Token: 0x1400000F RID: 15
		// (add) Token: 0x060006FE RID: 1790 RVA: 0x00003917 File Offset: 0x00001B17
		// (remove) Token: 0x060006FF RID: 1791 RVA: 0x00003917 File Offset: 0x00001B17
		[MonoTODO("Currently does nothing on Mono")]
		public static event UserPreferenceChangingEventHandler UserPreferenceChanging
		{
			add
			{
			}
			remove
			{
			}
		}

		// Token: 0x040004E3 RID: 1251
		private static Hashtable TimerStore = new Hashtable();
	}
}
