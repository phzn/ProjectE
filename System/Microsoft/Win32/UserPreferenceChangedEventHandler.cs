using System;

namespace Microsoft.Win32
{
	/// <summary>Represents the method that will handle the <see cref="E:Microsoft.Win32.SystemEvents.UserPreferenceChanged" /> event.</summary>
	/// <param name="sender">The source of the event. When this event is raised by the <see cref="T:Microsoft.Win32.SystemEvents" /> class, this object is always <see langword="null" />.</param>
	/// <param name="e">A <see cref="T:Microsoft.Win32.UserPreferenceChangedEventArgs" /> that contains the event data.</param>
	// Token: 0x0200012F RID: 303
	// (Invoke) Token: 0x0600070A RID: 1802
	public delegate void UserPreferenceChangedEventHandler(object sender, UserPreferenceChangedEventArgs e);
}
