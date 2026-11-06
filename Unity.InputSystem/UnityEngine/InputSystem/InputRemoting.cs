using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem
{
	// Token: 0x02000045 RID: 69
	public sealed class InputRemoting : IObservable<InputRemoting.Message>, IObserver<InputRemoting.Message>
	{
		// Token: 0x170001FB RID: 507
		// (get) Token: 0x060007AF RID: 1967 RVA: 0x0002D2BE File Offset: 0x0002B4BE
		// (set) Token: 0x060007B0 RID: 1968 RVA: 0x0002D2CB File Offset: 0x0002B4CB
		public bool sending
		{
			get
			{
				return (this.m_Flags & InputRemoting.Flags.Sending) == InputRemoting.Flags.Sending;
			}
			private set
			{
				if (value)
				{
					this.m_Flags |= InputRemoting.Flags.Sending;
					return;
				}
				this.m_Flags &= ~InputRemoting.Flags.Sending;
			}
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x0002D2EE File Offset: 0x0002B4EE
		internal InputRemoting(InputManager manager, bool startSendingOnConnect = false)
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			this.m_LocalManager = manager;
			if (startSendingOnConnect)
			{
				this.m_Flags |= InputRemoting.Flags.StartSendingOnConnect;
			}
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x0002D31C File Offset: 0x0002B51C
		public void StartSending()
		{
			if (this.sending)
			{
				return;
			}
			this.m_LocalManager.onEvent += this.SendEvent;
			this.m_LocalManager.onDeviceChange += this.SendDeviceChange;
			this.m_LocalManager.onLayoutChange += this.SendLayoutChange;
			this.sending = true;
			this.SendInitialMessages();
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x0002D384 File Offset: 0x0002B584
		public void StopSending()
		{
			if (!this.sending)
			{
				return;
			}
			this.m_LocalManager.onEvent -= this.SendEvent;
			this.m_LocalManager.onDeviceChange -= this.SendDeviceChange;
			this.m_LocalManager.onLayoutChange -= this.SendLayoutChange;
			this.sending = false;
		}

		// Token: 0x060007B4 RID: 1972 RVA: 0x0002D3E8 File Offset: 0x0002B5E8
		void IObserver<InputRemoting.Message>.OnNext(InputRemoting.Message msg)
		{
			switch (msg.type)
			{
			case InputRemoting.MessageType.Connect:
				InputRemoting.ConnectMsg.Process(this);
				return;
			case InputRemoting.MessageType.Disconnect:
				InputRemoting.DisconnectMsg.Process(this, msg);
				return;
			case InputRemoting.MessageType.NewLayout:
				InputRemoting.NewLayoutMsg.Process(this, msg);
				return;
			case InputRemoting.MessageType.NewDevice:
				InputRemoting.NewDeviceMsg.Process(this, msg);
				return;
			case InputRemoting.MessageType.NewEvents:
				InputRemoting.NewEventsMsg.Process(this, msg);
				return;
			case InputRemoting.MessageType.RemoveDevice:
				InputRemoting.RemoveDeviceMsg.Process(this, msg);
				return;
			case InputRemoting.MessageType.RemoveLayout:
				break;
			case InputRemoting.MessageType.ChangeUsages:
				InputRemoting.ChangeUsageMsg.Process(this, msg);
				return;
			case InputRemoting.MessageType.StartSending:
				InputRemoting.StartSendingMsg.Process(this);
				return;
			case InputRemoting.MessageType.StopSending:
				InputRemoting.StopSendingMsg.Process(this);
				break;
			default:
				return;
			}
		}

		// Token: 0x060007B5 RID: 1973 RVA: 0x0002D46F File Offset: 0x0002B66F
		void IObserver<InputRemoting.Message>.OnError(Exception error)
		{
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x0002D471 File Offset: 0x0002B671
		void IObserver<InputRemoting.Message>.OnCompleted()
		{
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x0002D474 File Offset: 0x0002B674
		public IDisposable Subscribe(IObserver<InputRemoting.Message> observer)
		{
			if (observer == null)
			{
				throw new ArgumentNullException("observer");
			}
			InputRemoting.Subscriber subscriber = new InputRemoting.Subscriber
			{
				owner = this,
				observer = observer
			};
			ArrayHelpers.Append<InputRemoting.Subscriber>(ref this.m_Subscribers, subscriber);
			return subscriber;
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x0002D4B1 File Offset: 0x0002B6B1
		private void SendInitialMessages()
		{
			this.SendAllGeneratedLayouts();
			this.SendAllDevices();
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x0002D4C0 File Offset: 0x0002B6C0
		private void SendAllGeneratedLayouts()
		{
			foreach (KeyValuePair<InternedString, Func<InputControlLayout>> keyValuePair in this.m_LocalManager.m_Layouts.layoutBuilders)
			{
				this.SendLayout(keyValuePair.Key);
			}
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x0002D528 File Offset: 0x0002B728
		private void SendLayout(string layoutName)
		{
			if (this.m_Subscribers == null)
			{
				return;
			}
			InputRemoting.Message? message = InputRemoting.NewLayoutMsg.Create(this, layoutName);
			if (message != null)
			{
				this.Send(message.Value);
			}
		}

		// Token: 0x060007BB RID: 1979 RVA: 0x0002D55C File Offset: 0x0002B75C
		private void SendAllDevices()
		{
			foreach (InputDevice inputDevice in this.m_LocalManager.devices)
			{
				this.SendDevice(inputDevice);
			}
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x0002D5B8 File Offset: 0x0002B7B8
		private void SendDevice(InputDevice device)
		{
			if (this.m_Subscribers == null)
			{
				return;
			}
			if (device.remote)
			{
				return;
			}
			InputRemoting.Message message = InputRemoting.NewDeviceMsg.Create(device);
			this.Send(message);
			InputRemoting.Message message2 = InputRemoting.NewEventsMsg.CreateStateEvent(device);
			this.Send(message2);
		}

		// Token: 0x060007BD RID: 1981 RVA: 0x0002D5F4 File Offset: 0x0002B7F4
		private void SendEvent(InputEventPtr eventPtr, InputDevice device)
		{
			if (this.m_Subscribers == null)
			{
				return;
			}
			if (device != null && device.remote)
			{
				return;
			}
			InputRemoting.Message message = InputRemoting.NewEventsMsg.Create(eventPtr.data, 1);
			this.Send(message);
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x0002D62C File Offset: 0x0002B82C
		private void SendDeviceChange(InputDevice device, InputDeviceChange change)
		{
			if (this.m_Subscribers == null)
			{
				return;
			}
			if (device.remote)
			{
				return;
			}
			InputRemoting.Message message;
			if (change != InputDeviceChange.Added)
			{
				if (change != InputDeviceChange.Removed)
				{
					switch (change)
					{
					case InputDeviceChange.UsageChanged:
						message = InputRemoting.ChangeUsageMsg.Create(device);
						break;
					case InputDeviceChange.ConfigurationChanged:
						return;
					case InputDeviceChange.SoftReset:
						message = InputRemoting.NewEventsMsg.CreateResetEvent(device, false);
						break;
					case InputDeviceChange.HardReset:
						message = InputRemoting.NewEventsMsg.CreateResetEvent(device, true);
						break;
					default:
						return;
					}
				}
				else
				{
					message = InputRemoting.RemoveDeviceMsg.Create(device);
				}
			}
			else
			{
				message = InputRemoting.NewDeviceMsg.Create(device);
			}
			this.Send(message);
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x0002D6A4 File Offset: 0x0002B8A4
		private void SendLayoutChange(string layout, InputControlLayoutChange change)
		{
			if (this.m_Subscribers == null)
			{
				return;
			}
			if (!this.m_LocalManager.m_Layouts.IsGeneratedLayout(new InternedString(layout)))
			{
				return;
			}
			if (change != InputControlLayoutChange.Added && change != InputControlLayoutChange.Replaced)
			{
				return;
			}
			InputRemoting.Message? message = InputRemoting.NewLayoutMsg.Create(this, layout);
			if (message != null)
			{
				this.Send(message.Value);
			}
		}

		// Token: 0x060007C0 RID: 1984 RVA: 0x0002D6FC File Offset: 0x0002B8FC
		private void Send(InputRemoting.Message msg)
		{
			InputRemoting.Subscriber[] subscribers = this.m_Subscribers;
			for (int i = 0; i < subscribers.Length; i++)
			{
				subscribers[i].observer.OnNext(msg);
			}
		}

		// Token: 0x060007C1 RID: 1985 RVA: 0x0002D72C File Offset: 0x0002B92C
		private int FindOrCreateSenderRecord(int senderId)
		{
			if (this.m_Senders != null)
			{
				int num = this.m_Senders.Length;
				for (int i = 0; i < num; i++)
				{
					if (this.m_Senders[i].senderId == senderId)
					{
						return i;
					}
				}
			}
			InputRemoting.RemoteSender remoteSender = new InputRemoting.RemoteSender
			{
				senderId = senderId
			};
			return ArrayHelpers.Append<InputRemoting.RemoteSender>(ref this.m_Senders, remoteSender);
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x0002D78A File Offset: 0x0002B98A
		private static InternedString BuildLayoutNamespace(int senderId)
		{
			return new InternedString(string.Format("Remote::{0}", senderId));
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x0002D7A4 File Offset: 0x0002B9A4
		private int FindLocalDeviceId(int remoteDeviceId, int senderIndex)
		{
			InputRemoting.RemoteInputDevice[] devices = this.m_Senders[senderIndex].devices;
			if (devices != null)
			{
				int num = devices.Length;
				for (int i = 0; i < num; i++)
				{
					if (devices[i].remoteId == remoteDeviceId)
					{
						return devices[i].localId;
					}
				}
			}
			return 0;
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x0002D7F4 File Offset: 0x0002B9F4
		private InputDevice TryGetDeviceByRemoteId(int remoteDeviceId, int senderIndex)
		{
			int num = this.FindLocalDeviceId(remoteDeviceId, senderIndex);
			return this.m_LocalManager.TryGetDeviceById(num);
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x060007C5 RID: 1989 RVA: 0x0002D816 File Offset: 0x0002BA16
		internal InputManager manager
		{
			get
			{
				return this.m_LocalManager;
			}
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x0002D820 File Offset: 0x0002BA20
		public void RemoveRemoteDevices(int participantId)
		{
			int num = this.FindOrCreateSenderRecord(participantId);
			InputRemoting.RemoteInputDevice[] devices = this.m_Senders[num].devices;
			if (devices != null)
			{
				foreach (InputRemoting.RemoteInputDevice remoteInputDevice in devices)
				{
					InputDevice inputDevice = this.m_LocalManager.TryGetDeviceById(remoteInputDevice.localId);
					if (inputDevice != null)
					{
						this.m_LocalManager.RemoveDevice(inputDevice, false);
					}
				}
			}
			ArrayHelpers.EraseAt<InputRemoting.RemoteSender>(ref this.m_Senders, num);
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x0002D898 File Offset: 0x0002BA98
		private static byte[] SerializeData<TData>(TData data)
		{
			string text = JsonUtility.ToJson(data);
			return Encoding.UTF8.GetBytes(text);
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x0002D8BC File Offset: 0x0002BABC
		private static TData DeserializeData<TData>(byte[] data)
		{
			return JsonUtility.FromJson<TData>(Encoding.UTF8.GetString(data));
		}

		// Token: 0x04000247 RID: 583
		private InputRemoting.Flags m_Flags;

		// Token: 0x04000248 RID: 584
		private InputManager m_LocalManager;

		// Token: 0x04000249 RID: 585
		private InputRemoting.Subscriber[] m_Subscribers;

		// Token: 0x0400024A RID: 586
		private InputRemoting.RemoteSender[] m_Senders;

		// Token: 0x0200019E RID: 414
		public enum MessageType
		{
			// Token: 0x04000907 RID: 2311
			Connect,
			// Token: 0x04000908 RID: 2312
			Disconnect,
			// Token: 0x04000909 RID: 2313
			NewLayout,
			// Token: 0x0400090A RID: 2314
			NewDevice,
			// Token: 0x0400090B RID: 2315
			NewEvents,
			// Token: 0x0400090C RID: 2316
			RemoveDevice,
			// Token: 0x0400090D RID: 2317
			RemoveLayout,
			// Token: 0x0400090E RID: 2318
			ChangeUsages,
			// Token: 0x0400090F RID: 2319
			StartSending,
			// Token: 0x04000910 RID: 2320
			StopSending
		}

		// Token: 0x0200019F RID: 415
		public struct Message
		{
			// Token: 0x04000911 RID: 2321
			public int participantId;

			// Token: 0x04000912 RID: 2322
			public InputRemoting.MessageType type;

			// Token: 0x04000913 RID: 2323
			public byte[] data;
		}

		// Token: 0x020001A0 RID: 416
		[Flags]
		private enum Flags
		{
			// Token: 0x04000915 RID: 2325
			Sending = 1,
			// Token: 0x04000916 RID: 2326
			StartSendingOnConnect = 2
		}

		// Token: 0x020001A1 RID: 417
		[Serializable]
		internal struct RemoteSender
		{
			// Token: 0x04000917 RID: 2327
			public int senderId;

			// Token: 0x04000918 RID: 2328
			public InternedString[] layouts;

			// Token: 0x04000919 RID: 2329
			public InputRemoting.RemoteInputDevice[] devices;
		}

		// Token: 0x020001A2 RID: 418
		[Serializable]
		internal struct RemoteInputDevice
		{
			// Token: 0x0400091A RID: 2330
			public int remoteId;

			// Token: 0x0400091B RID: 2331
			public int localId;

			// Token: 0x0400091C RID: 2332
			public InputDeviceDescription description;
		}

		// Token: 0x020001A3 RID: 419
		internal class Subscriber : IDisposable
		{
			// Token: 0x0600142B RID: 5163 RVA: 0x0005DA19 File Offset: 0x0005BC19
			public void Dispose()
			{
				ArrayHelpers.Erase<InputRemoting.Subscriber>(ref this.owner.m_Subscribers, this);
			}

			// Token: 0x0400091D RID: 2333
			public InputRemoting owner;

			// Token: 0x0400091E RID: 2334
			public IObserver<InputRemoting.Message> observer;
		}

		// Token: 0x020001A4 RID: 420
		private static class ConnectMsg
		{
			// Token: 0x0600142D RID: 5165 RVA: 0x0005DA35 File Offset: 0x0005BC35
			public static void Process(InputRemoting receiver)
			{
				if (receiver.sending)
				{
					receiver.SendInitialMessages();
					return;
				}
				if ((receiver.m_Flags & InputRemoting.Flags.StartSendingOnConnect) == InputRemoting.Flags.StartSendingOnConnect)
				{
					receiver.StartSending();
				}
			}
		}

		// Token: 0x020001A5 RID: 421
		private static class StartSendingMsg
		{
			// Token: 0x0600142E RID: 5166 RVA: 0x0005DA57 File Offset: 0x0005BC57
			public static void Process(InputRemoting receiver)
			{
				receiver.StartSending();
			}
		}

		// Token: 0x020001A6 RID: 422
		private static class StopSendingMsg
		{
			// Token: 0x0600142F RID: 5167 RVA: 0x0005DA5F File Offset: 0x0005BC5F
			public static void Process(InputRemoting receiver)
			{
				receiver.StopSending();
			}
		}

		// Token: 0x020001A7 RID: 423
		private static class DisconnectMsg
		{
			// Token: 0x06001430 RID: 5168 RVA: 0x0005DA67 File Offset: 0x0005BC67
			public static void Process(InputRemoting receiver, InputRemoting.Message msg)
			{
				Debug.Log("DisconnectMsg.Process");
				receiver.RemoveRemoteDevices(msg.participantId);
				receiver.StopSending();
			}
		}

		// Token: 0x020001A8 RID: 424
		private static class NewLayoutMsg
		{
			// Token: 0x06001431 RID: 5169 RVA: 0x0005DA88 File Offset: 0x0005BC88
			public static InputRemoting.Message? Create(InputRemoting sender, string layoutName)
			{
				InputControlLayout inputControlLayout;
				try
				{
					inputControlLayout = sender.m_LocalManager.TryLoadControlLayout(new InternedString(layoutName));
					if (inputControlLayout == null)
					{
						Debug.Log(string.Format("Could not find layout '{0}' meant to be sent through remote connection; this should not happen", layoutName));
						return null;
					}
				}
				catch (Exception ex)
				{
					Debug.Log(string.Format("Could not load layout '{0}'; not sending to remote listeners (exception: {1})", layoutName, ex));
					return null;
				}
				InputRemoting.NewLayoutMsg.Data data = new InputRemoting.NewLayoutMsg.Data
				{
					name = layoutName,
					layoutJson = inputControlLayout.ToJson(),
					isOverride = inputControlLayout.isOverride
				};
				return new InputRemoting.Message?(new InputRemoting.Message
				{
					type = InputRemoting.MessageType.NewLayout,
					data = InputRemoting.SerializeData<InputRemoting.NewLayoutMsg.Data>(data)
				});
			}

			// Token: 0x06001432 RID: 5170 RVA: 0x0005DB4C File Offset: 0x0005BD4C
			public static void Process(InputRemoting receiver, InputRemoting.Message msg)
			{
				InputRemoting.NewLayoutMsg.Data data = InputRemoting.DeserializeData<InputRemoting.NewLayoutMsg.Data>(msg.data);
				int num = receiver.FindOrCreateSenderRecord(msg.participantId);
				InternedString internedString = new InternedString(data.name);
				receiver.m_LocalManager.RegisterControlLayout(data.layoutJson, data.name, data.isOverride);
				ArrayHelpers.Append<InternedString>(ref receiver.m_Senders[num].layouts, internedString);
			}

			// Token: 0x02000278 RID: 632
			[Serializable]
			public struct Data
			{
				// Token: 0x04000D01 RID: 3329
				public string name;

				// Token: 0x04000D02 RID: 3330
				public string layoutJson;

				// Token: 0x04000D03 RID: 3331
				public bool isOverride;
			}
		}

		// Token: 0x020001A9 RID: 425
		private static class NewDeviceMsg
		{
			// Token: 0x06001433 RID: 5171 RVA: 0x0005DBB4 File Offset: 0x0005BDB4
			public static InputRemoting.Message Create(InputDevice device)
			{
				InputRemoting.NewDeviceMsg.Data data = default(InputRemoting.NewDeviceMsg.Data);
				data.name = device.name;
				data.layout = device.layout;
				data.deviceId = device.deviceId;
				data.description = device.description;
				data.usages = device.usages.Select<InternedString, string>((InternedString x) => x.ToString()).ToArray<string>();
				InputRemoting.NewDeviceMsg.Data data2 = data;
				return new InputRemoting.Message
				{
					type = InputRemoting.MessageType.NewDevice,
					data = InputRemoting.SerializeData<InputRemoting.NewDeviceMsg.Data>(data2)
				};
			}

			// Token: 0x06001434 RID: 5172 RVA: 0x0005DC58 File Offset: 0x0005BE58
			public static void Process(InputRemoting receiver, InputRemoting.Message msg)
			{
				int num = receiver.FindOrCreateSenderRecord(msg.participantId);
				InputRemoting.NewDeviceMsg.Data data = InputRemoting.DeserializeData<InputRemoting.NewDeviceMsg.Data>(msg.data);
				InputRemoting.RemoteInputDevice[] devices = receiver.m_Senders[num].devices;
				if (devices != null)
				{
					InputRemoting.RemoteInputDevice[] array = devices;
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i].remoteId == data.deviceId)
						{
							Debug.LogError(string.Format("Already received device with id {0} (layout '{1}', description '{3}) from remote {2}", new object[] { data.deviceId, data.layout, msg.participantId, data.description }));
							return;
						}
					}
				}
				InputDevice inputDevice;
				try
				{
					InternedString internedString = new InternedString(data.layout);
					inputDevice = receiver.m_LocalManager.AddDevice(internedString, data.name, default(InternedString));
					inputDevice.m_ParticipantId = msg.participantId;
				}
				catch (Exception ex)
				{
					Debug.LogError(string.Format("Could not create remote device '{0}' with layout '{1}' locally (exception: {2})", data.description, data.layout, ex));
					return;
				}
				inputDevice.m_Description = data.description;
				inputDevice.m_DeviceFlags |= InputDevice.DeviceFlags.Remote;
				foreach (string text in data.usages)
				{
					receiver.m_LocalManager.AddDeviceUsage(inputDevice, new InternedString(text));
				}
				InputRemoting.RemoteInputDevice remoteInputDevice = new InputRemoting.RemoteInputDevice
				{
					remoteId = data.deviceId,
					localId = inputDevice.deviceId,
					description = data.description
				};
				ArrayHelpers.Append<InputRemoting.RemoteInputDevice>(ref receiver.m_Senders[num].devices, remoteInputDevice);
			}

			// Token: 0x02000279 RID: 633
			[Serializable]
			public struct Data
			{
				// Token: 0x04000D04 RID: 3332
				public string name;

				// Token: 0x04000D05 RID: 3333
				public string layout;

				// Token: 0x04000D06 RID: 3334
				public int deviceId;

				// Token: 0x04000D07 RID: 3335
				public string[] usages;

				// Token: 0x04000D08 RID: 3336
				public InputDeviceDescription description;
			}
		}

		// Token: 0x020001AA RID: 426
		private static class NewEventsMsg
		{
			// Token: 0x06001435 RID: 5173 RVA: 0x0005DE1C File Offset: 0x0005C01C
			public unsafe static InputRemoting.Message CreateResetEvent(InputDevice device, bool isHardReset)
			{
				DeviceResetEvent deviceResetEvent = DeviceResetEvent.Create(device.deviceId, isHardReset, -1.0);
				return InputRemoting.NewEventsMsg.Create((InputEvent*)UnsafeUtility.AddressOf<DeviceResetEvent>(ref deviceResetEvent), 1);
			}

			// Token: 0x06001436 RID: 5174 RVA: 0x0005DE4C File Offset: 0x0005C04C
			public static InputRemoting.Message CreateStateEvent(InputDevice device)
			{
				InputEventPtr inputEventPtr;
				InputRemoting.Message message;
				using (StateEvent.From(device, out inputEventPtr, Allocator.Temp))
				{
					message = InputRemoting.NewEventsMsg.Create(inputEventPtr.data, 1);
				}
				return message;
			}

			// Token: 0x06001437 RID: 5175 RVA: 0x0005DE94 File Offset: 0x0005C094
			public unsafe static InputRemoting.Message Create(InputEvent* events, int eventCount)
			{
				uint num = 0U;
				InputEventPtr inputEventPtr = new InputEventPtr(events);
				int i = 0;
				while (i < eventCount)
				{
					num = num.AlignToMultipleOf(4U) + inputEventPtr.sizeInBytes;
					i++;
					inputEventPtr = inputEventPtr.Next();
				}
				byte[] array = new byte[num];
				byte[] array2;
				byte* ptr;
				if ((array2 = array) == null || array2.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array2[0];
				}
				UnsafeUtility.MemCpy((void*)ptr, (void*)events, (long)((ulong)num));
				array2 = null;
				return new InputRemoting.Message
				{
					type = InputRemoting.MessageType.NewEvents,
					data = array
				};
			}

			// Token: 0x06001438 RID: 5176 RVA: 0x0005DF1C File Offset: 0x0005C11C
			public unsafe static void Process(InputRemoting receiver, InputRemoting.Message msg)
			{
				InputManager localManager = receiver.m_LocalManager;
				byte[] array;
				byte* ptr;
				if ((array = msg.data) == null || array.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array[0];
				}
				IntPtr intPtr = new IntPtr((void*)(ptr + msg.data.Length));
				int num = 0;
				InputEventPtr inputEventPtr = new InputEventPtr((InputEvent*)ptr);
				int num2 = receiver.FindOrCreateSenderRecord(msg.participantId);
				while (inputEventPtr.data < (InputEvent*)intPtr.ToPointer())
				{
					int deviceId = inputEventPtr.deviceId;
					int num3 = receiver.FindLocalDeviceId(deviceId, num2);
					inputEventPtr.deviceId = num3;
					if (num3 != 0)
					{
						localManager.QueueEvent(inputEventPtr);
					}
					num++;
					inputEventPtr = inputEventPtr.Next();
				}
				array = null;
			}
		}

		// Token: 0x020001AB RID: 427
		private static class ChangeUsageMsg
		{
			// Token: 0x06001439 RID: 5177 RVA: 0x0005DFC4 File Offset: 0x0005C1C4
			public static InputRemoting.Message Create(InputDevice device)
			{
				InputRemoting.ChangeUsageMsg.Data data = default(InputRemoting.ChangeUsageMsg.Data);
				data.deviceId = device.deviceId;
				data.usages = device.usages.Select<InternedString, string>((InternedString x) => x.ToString()).ToArray<string>();
				InputRemoting.ChangeUsageMsg.Data data2 = data;
				return new InputRemoting.Message
				{
					type = InputRemoting.MessageType.ChangeUsages,
					data = InputRemoting.SerializeData<InputRemoting.ChangeUsageMsg.Data>(data2)
				};
			}

			// Token: 0x0600143A RID: 5178 RVA: 0x0005E044 File Offset: 0x0005C244
			public static void Process(InputRemoting receiver, InputRemoting.Message msg)
			{
				int num = receiver.FindOrCreateSenderRecord(msg.participantId);
				InputRemoting.ChangeUsageMsg.Data data = InputRemoting.DeserializeData<InputRemoting.ChangeUsageMsg.Data>(msg.data);
				InputDevice inputDevice = receiver.TryGetDeviceByRemoteId(data.deviceId, num);
				if (inputDevice != null)
				{
					foreach (InternedString internedString in inputDevice.usages)
					{
						if (!data.usages.Contains(internedString))
						{
							receiver.m_LocalManager.RemoveDeviceUsage(inputDevice, new InternedString(internedString));
						}
					}
					foreach (string text in data.usages)
					{
						InternedString internedString2 = new InternedString(text);
						if (!inputDevice.usages.Contains(internedString2))
						{
							receiver.m_LocalManager.AddDeviceUsage(inputDevice, new InternedString(text));
						}
					}
				}
			}

			// Token: 0x0200027B RID: 635
			[Serializable]
			public struct Data
			{
				// Token: 0x04000D0B RID: 3339
				public int deviceId;

				// Token: 0x04000D0C RID: 3340
				public string[] usages;
			}
		}

		// Token: 0x020001AC RID: 428
		private static class RemoveDeviceMsg
		{
			// Token: 0x0600143B RID: 5179 RVA: 0x0005E13C File Offset: 0x0005C33C
			public static InputRemoting.Message Create(InputDevice device)
			{
				return new InputRemoting.Message
				{
					type = InputRemoting.MessageType.RemoveDevice,
					data = BitConverter.GetBytes(device.deviceId)
				};
			}

			// Token: 0x0600143C RID: 5180 RVA: 0x0005E16C File Offset: 0x0005C36C
			public static void Process(InputRemoting receiver, InputRemoting.Message msg)
			{
				int num = receiver.FindOrCreateSenderRecord(msg.participantId);
				int num2 = BitConverter.ToInt32(msg.data, 0);
				InputDevice inputDevice = receiver.TryGetDeviceByRemoteId(num2, num);
				if (inputDevice != null)
				{
					receiver.m_LocalManager.RemoveDevice(inputDevice, false);
				}
			}
		}
	}
}
