using System;
using UnityEngine.Events;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Networking.PlayerConnection;

namespace UnityEngine.InputSystem
{
	// Token: 0x02000046 RID: 70
	[Serializable]
	internal class RemoteInputPlayerConnection : ScriptableObject, IObserver<InputRemoting.Message>, IObservable<InputRemoting.Message>
	{
		// Token: 0x060007C9 RID: 1993 RVA: 0x0002D8D0 File Offset: 0x0002BAD0
		public void Bind(IEditorPlayerConnection connection, bool isConnected)
		{
			if (this.m_Connection == null)
			{
				connection.RegisterConnection(new UnityAction<int>(this.OnConnected));
				connection.RegisterDisconnection(new UnityAction<int>(this.OnDisconnected));
				connection.Register(RemoteInputPlayerConnection.kNewDeviceMsg, new UnityAction<MessageEventArgs>(this.OnNewDevice));
				connection.Register(RemoteInputPlayerConnection.kNewLayoutMsg, new UnityAction<MessageEventArgs>(this.OnNewLayout));
				connection.Register(RemoteInputPlayerConnection.kNewEventsMsg, new UnityAction<MessageEventArgs>(this.OnNewEvents));
				connection.Register(RemoteInputPlayerConnection.kRemoveDeviceMsg, new UnityAction<MessageEventArgs>(this.OnRemoveDevice));
				connection.Register(RemoteInputPlayerConnection.kChangeUsagesMsg, new UnityAction<MessageEventArgs>(this.OnChangeUsages));
				connection.Register(RemoteInputPlayerConnection.kStartSendingMsg, new UnityAction<MessageEventArgs>(this.OnStartSending));
				connection.Register(RemoteInputPlayerConnection.kStopSendingMsg, new UnityAction<MessageEventArgs>(this.OnStopSending));
				this.m_Connection = connection;
				if (isConnected)
				{
					this.OnConnected(0);
				}
				return;
			}
			if (this.m_Connection == connection)
			{
				return;
			}
			throw new InvalidOperationException("Already bound to an IEditorPlayerConnection");
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x0002D9D0 File Offset: 0x0002BBD0
		public IDisposable Subscribe(IObserver<InputRemoting.Message> observer)
		{
			if (observer == null)
			{
				throw new ArgumentNullException("observer");
			}
			RemoteInputPlayerConnection.Subscriber subscriber = new RemoteInputPlayerConnection.Subscriber
			{
				owner = this,
				observer = observer
			};
			ArrayHelpers.Append<RemoteInputPlayerConnection.Subscriber>(ref this.m_Subscribers, subscriber);
			if (this.m_ConnectedIds != null)
			{
				foreach (int num in this.m_ConnectedIds)
				{
					observer.OnNext(new InputRemoting.Message
					{
						type = InputRemoting.MessageType.Connect,
						participantId = num
					});
				}
			}
			return subscriber;
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x0002DA4E File Offset: 0x0002BC4E
		private void OnConnected(int id)
		{
			if (this.m_ConnectedIds != null && ArrayHelpers.Contains<int>(this.m_ConnectedIds, id))
			{
				return;
			}
			ArrayHelpers.Append<int>(ref this.m_ConnectedIds, id);
			this.SendToSubscribers(InputRemoting.MessageType.Connect, new MessageEventArgs
			{
				playerId = id
			});
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x0002DA87 File Offset: 0x0002BC87
		private void OnDisconnected(int id)
		{
			if (this.m_ConnectedIds == null || !ArrayHelpers.Contains<int>(this.m_ConnectedIds, id))
			{
				return;
			}
			ArrayHelpers.Erase<int>(ref this.m_ConnectedIds, id);
			this.SendToSubscribers(InputRemoting.MessageType.Disconnect, new MessageEventArgs
			{
				playerId = id
			});
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x0002DAC0 File Offset: 0x0002BCC0
		private void OnNewDevice(MessageEventArgs args)
		{
			this.SendToSubscribers(InputRemoting.MessageType.NewDevice, args);
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x0002DACA File Offset: 0x0002BCCA
		private void OnNewLayout(MessageEventArgs args)
		{
			this.SendToSubscribers(InputRemoting.MessageType.NewLayout, args);
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x0002DAD4 File Offset: 0x0002BCD4
		private void OnNewEvents(MessageEventArgs args)
		{
			this.SendToSubscribers(InputRemoting.MessageType.NewEvents, args);
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x0002DADE File Offset: 0x0002BCDE
		private void OnRemoveDevice(MessageEventArgs args)
		{
			this.SendToSubscribers(InputRemoting.MessageType.RemoveDevice, args);
		}

		// Token: 0x060007D1 RID: 2001 RVA: 0x0002DAE8 File Offset: 0x0002BCE8
		private void OnChangeUsages(MessageEventArgs args)
		{
			this.SendToSubscribers(InputRemoting.MessageType.ChangeUsages, args);
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x0002DAF2 File Offset: 0x0002BCF2
		private void OnStartSending(MessageEventArgs args)
		{
			this.SendToSubscribers(InputRemoting.MessageType.StartSending, args);
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x0002DAFC File Offset: 0x0002BCFC
		private void OnStopSending(MessageEventArgs args)
		{
			this.SendToSubscribers(InputRemoting.MessageType.StopSending, args);
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x0002DB08 File Offset: 0x0002BD08
		private void SendToSubscribers(InputRemoting.MessageType type, MessageEventArgs args)
		{
			if (this.m_Subscribers == null)
			{
				return;
			}
			InputRemoting.Message message = new InputRemoting.Message
			{
				participantId = args.playerId,
				type = type,
				data = args.data
			};
			for (int i = 0; i < this.m_Subscribers.Length; i++)
			{
				this.m_Subscribers[i].observer.OnNext(message);
			}
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x0002DB70 File Offset: 0x0002BD70
		void IObserver<InputRemoting.Message>.OnNext(InputRemoting.Message msg)
		{
			if (this.m_Connection == null)
			{
				return;
			}
			switch (msg.type)
			{
			case InputRemoting.MessageType.NewLayout:
				this.m_Connection.Send(RemoteInputPlayerConnection.kNewLayoutMsg, msg.data);
				return;
			case InputRemoting.MessageType.NewDevice:
				this.m_Connection.Send(RemoteInputPlayerConnection.kNewDeviceMsg, msg.data);
				return;
			case InputRemoting.MessageType.NewEvents:
				this.m_Connection.Send(RemoteInputPlayerConnection.kNewEventsMsg, msg.data);
				return;
			case InputRemoting.MessageType.RemoveDevice:
				this.m_Connection.Send(RemoteInputPlayerConnection.kRemoveDeviceMsg, msg.data);
				break;
			case InputRemoting.MessageType.RemoveLayout:
				break;
			case InputRemoting.MessageType.ChangeUsages:
				this.m_Connection.Send(RemoteInputPlayerConnection.kChangeUsagesMsg, msg.data);
				return;
			default:
				return;
			}
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x0002DC20 File Offset: 0x0002BE20
		void IObserver<InputRemoting.Message>.OnError(Exception error)
		{
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x0002DC22 File Offset: 0x0002BE22
		void IObserver<InputRemoting.Message>.OnCompleted()
		{
		}

		// Token: 0x0400024B RID: 587
		public static readonly Guid kNewDeviceMsg = new Guid("fcd9651ded40425995dfa6aeb78f1f1c");

		// Token: 0x0400024C RID: 588
		public static readonly Guid kNewLayoutMsg = new Guid("fccfec2b7369466d88502a9dd38505f4");

		// Token: 0x0400024D RID: 589
		public static readonly Guid kNewEventsMsg = new Guid("53546641df1347bc8aa315278a603586");

		// Token: 0x0400024E RID: 590
		public static readonly Guid kRemoveDeviceMsg = new Guid("e5e299b2d9e44255b8990bb71af8922d");

		// Token: 0x0400024F RID: 591
		public static readonly Guid kChangeUsagesMsg = new Guid("b9fe706dfc854d7ca109a5e38d7db730");

		// Token: 0x04000250 RID: 592
		public static readonly Guid kStartSendingMsg = new Guid("0d58e99045904672b3ef34b8797d23cb");

		// Token: 0x04000251 RID: 593
		public static readonly Guid kStopSendingMsg = new Guid("548716b2534a45369ab0c9323fc8b4a8");

		// Token: 0x04000252 RID: 594
		[SerializeField]
		private IEditorPlayerConnection m_Connection;

		// Token: 0x04000253 RID: 595
		[NonSerialized]
		private RemoteInputPlayerConnection.Subscriber[] m_Subscribers;

		// Token: 0x04000254 RID: 596
		[SerializeField]
		private int[] m_ConnectedIds;

		// Token: 0x020001AD RID: 429
		private class Subscriber : IDisposable
		{
			// Token: 0x0600143D RID: 5181 RVA: 0x0005E1AC File Offset: 0x0005C3AC
			public void Dispose()
			{
				ArrayHelpers.Erase<RemoteInputPlayerConnection.Subscriber>(ref this.owner.m_Subscribers, this);
			}

			// Token: 0x0400091F RID: 2335
			public RemoteInputPlayerConnection owner;

			// Token: 0x04000920 RID: 2336
			public IObserver<InputRemoting.Message> observer;
		}
	}
}
