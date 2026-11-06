using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.XR
{
	// Token: 0x02000025 RID: 37
	[NativeType(Header = "Modules/XR/Subsystems/Input/XRInputSubsystem.h")]
	[NativeConditional("ENABLE_XR")]
	[UsedByNativeCode]
	public class XRInputSubsystem : IntegratedSubsystem<XRInputSubsystemDescriptor>
	{
		// Token: 0x06000124 RID: 292
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern uint GetIndex();

		// Token: 0x06000125 RID: 293
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool TryRecenter();

		// Token: 0x06000126 RID: 294 RVA: 0x000043F8 File Offset: 0x000025F8
		public bool TryGetInputDevices(List<InputDevice> devices)
		{
			bool flag = devices == null;
			if (flag)
			{
				throw new ArgumentNullException("devices");
			}
			devices.Clear();
			bool flag2 = this.m_DeviceIdsCache == null;
			if (flag2)
			{
				this.m_DeviceIdsCache = new List<ulong>();
			}
			this.m_DeviceIdsCache.Clear();
			this.TryGetDeviceIds_AsList(this.m_DeviceIdsCache);
			for (int i = 0; i < this.m_DeviceIdsCache.Count; i++)
			{
				devices.Add(new InputDevice(this.m_DeviceIdsCache[i]));
			}
			return true;
		}

		// Token: 0x06000127 RID: 295
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool TrySetTrackingOriginMode(TrackingOriginModeFlags origin);

		// Token: 0x06000128 RID: 296
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern TrackingOriginModeFlags GetTrackingOriginMode();

		// Token: 0x06000129 RID: 297
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern TrackingOriginModeFlags GetSupportedTrackingOriginModes();

		// Token: 0x0600012A RID: 298 RVA: 0x0000448C File Offset: 0x0000268C
		public bool TryGetBoundaryPoints(List<Vector3> boundaryPoints)
		{
			bool flag = boundaryPoints == null;
			if (flag)
			{
				throw new ArgumentNullException("boundaryPoints");
			}
			return this.TryGetBoundaryPoints_AsList(boundaryPoints);
		}

		// Token: 0x0600012B RID: 299
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool TryGetBoundaryPoints_AsList(List<Vector3> boundaryPoints);

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x0600012C RID: 300 RVA: 0x000044B8 File Offset: 0x000026B8
		// (remove) Token: 0x0600012D RID: 301 RVA: 0x000044F0 File Offset: 0x000026F0
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<XRInputSubsystem> trackingOriginUpdated;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x0600012E RID: 302 RVA: 0x00004528 File Offset: 0x00002728
		// (remove) Token: 0x0600012F RID: 303 RVA: 0x00004560 File Offset: 0x00002760
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<XRInputSubsystem> boundaryChanged;

		// Token: 0x06000130 RID: 304 RVA: 0x00004598 File Offset: 0x00002798
		[RequiredByNativeCode(GenerateProxy = true)]
		private static void InvokeTrackingOriginUpdatedEvent(IntPtr internalPtr)
		{
			IntegratedSubsystem integratedSubsystemByPtr = SubsystemManager.GetIntegratedSubsystemByPtr(internalPtr);
			XRInputSubsystem xrinputSubsystem = integratedSubsystemByPtr as XRInputSubsystem;
			bool flag = xrinputSubsystem != null && xrinputSubsystem.trackingOriginUpdated != null;
			if (flag)
			{
				xrinputSubsystem.trackingOriginUpdated(xrinputSubsystem);
			}
		}

		// Token: 0x06000131 RID: 305 RVA: 0x000045D4 File Offset: 0x000027D4
		[RequiredByNativeCode(GenerateProxy = true)]
		private static void InvokeBoundaryChangedEvent(IntPtr internalPtr)
		{
			IntegratedSubsystem integratedSubsystemByPtr = SubsystemManager.GetIntegratedSubsystemByPtr(internalPtr);
			XRInputSubsystem xrinputSubsystem = integratedSubsystemByPtr as XRInputSubsystem;
			bool flag = xrinputSubsystem != null && xrinputSubsystem.boundaryChanged != null;
			if (flag)
			{
				xrinputSubsystem.boundaryChanged(xrinputSubsystem);
			}
		}

		// Token: 0x06000132 RID: 306
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void TryGetDeviceIds_AsList(List<ulong> deviceIds);

		// Token: 0x040000F0 RID: 240
		private List<ulong> m_DeviceIdsCache;
	}
}
