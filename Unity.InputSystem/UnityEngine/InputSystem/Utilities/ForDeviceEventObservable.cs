using System;
using UnityEngine.InputSystem.LowLevel;

namespace UnityEngine.InputSystem.Utilities
{
	// Token: 0x0200013D RID: 317
	internal class ForDeviceEventObservable : IObservable<InputEventPtr>
	{
		// Token: 0x06001197 RID: 4503 RVA: 0x000543F4 File Offset: 0x000525F4
		public ForDeviceEventObservable(IObservable<InputEventPtr> source, Type deviceType, InputDevice device)
		{
			this.m_Source = source;
			this.m_DeviceType = deviceType;
			this.m_Device = device;
		}

		// Token: 0x06001198 RID: 4504 RVA: 0x00054411 File Offset: 0x00052611
		public IDisposable Subscribe(IObserver<InputEventPtr> observer)
		{
			return this.m_Source.Subscribe(new ForDeviceEventObservable.ForDevice(this.m_DeviceType, this.m_Device, observer));
		}

		// Token: 0x0400071A RID: 1818
		private IObservable<InputEventPtr> m_Source;

		// Token: 0x0400071B RID: 1819
		private InputDevice m_Device;

		// Token: 0x0400071C RID: 1820
		private Type m_DeviceType;

		// Token: 0x0200024C RID: 588
		private class ForDevice : IObserver<InputEventPtr>
		{
			// Token: 0x06001649 RID: 5705 RVA: 0x000669EA File Offset: 0x00064BEA
			public ForDevice(Type deviceType, InputDevice device, IObserver<InputEventPtr> observer)
			{
				this.m_Device = device;
				this.m_DeviceType = deviceType;
				this.m_Observer = observer;
			}

			// Token: 0x0600164A RID: 5706 RVA: 0x00066A07 File Offset: 0x00064C07
			public void OnCompleted()
			{
			}

			// Token: 0x0600164B RID: 5707 RVA: 0x00066A09 File Offset: 0x00064C09
			public void OnError(Exception error)
			{
				Debug.LogException(error);
			}

			// Token: 0x0600164C RID: 5708 RVA: 0x00066A14 File Offset: 0x00064C14
			public void OnNext(InputEventPtr value)
			{
				if (this.m_DeviceType != null)
				{
					InputDevice deviceById = InputSystem.GetDeviceById(value.deviceId);
					if (deviceById == null)
					{
						return;
					}
					if (!this.m_DeviceType.IsInstanceOfType(deviceById))
					{
						return;
					}
				}
				if (this.m_Device != null && value.deviceId != this.m_Device.deviceId)
				{
					return;
				}
				this.m_Observer.OnNext(value);
			}

			// Token: 0x04000C92 RID: 3218
			private IObserver<InputEventPtr> m_Observer;

			// Token: 0x04000C93 RID: 3219
			private InputDevice m_Device;

			// Token: 0x04000C94 RID: 3220
			private Type m_DeviceType;
		}
	}
}
