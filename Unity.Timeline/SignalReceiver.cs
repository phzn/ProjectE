using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	// Token: 0x0200002D RID: 45
	public class SignalReceiver : MonoBehaviour, INotificationReceiver
	{
		// Token: 0x06000251 RID: 593 RVA: 0x0000874C File Offset: 0x0000694C
		public void OnNotify(Playable origin, INotification notification, object context)
		{
			SignalEmitter signalEmitter = notification as SignalEmitter;
			UnityEvent unityEvent;
			if (signalEmitter != null && signalEmitter.asset != null && this.m_Events.TryGetValue(signalEmitter.asset, out unityEvent) && unityEvent != null)
			{
				unityEvent.Invoke();
			}
		}

		// Token: 0x06000252 RID: 594 RVA: 0x00008798 File Offset: 0x00006998
		public void AddReaction(SignalAsset asset, UnityEvent reaction)
		{
			if (asset == null)
			{
				throw new ArgumentNullException("asset");
			}
			if (this.m_Events.signals.Contains(asset))
			{
				throw new ArgumentException("SignalAsset already used.");
			}
			this.m_Events.Append(asset, reaction);
		}

		// Token: 0x06000253 RID: 595 RVA: 0x000087E4 File Offset: 0x000069E4
		public int AddEmptyReaction(UnityEvent reaction)
		{
			this.m_Events.Append(null, reaction);
			return this.m_Events.events.Count - 1;
		}

		// Token: 0x06000254 RID: 596 RVA: 0x00008805 File Offset: 0x00006A05
		public void Remove(SignalAsset asset)
		{
			if (!this.m_Events.signals.Contains(asset))
			{
				throw new ArgumentException("The SignalAsset is not registered with this receiver.");
			}
			this.m_Events.Remove(asset);
		}

		// Token: 0x06000255 RID: 597 RVA: 0x00008831 File Offset: 0x00006A31
		public IEnumerable<SignalAsset> GetRegisteredSignals()
		{
			return this.m_Events.signals;
		}

		// Token: 0x06000256 RID: 598 RVA: 0x00008840 File Offset: 0x00006A40
		public UnityEvent GetReaction(SignalAsset key)
		{
			UnityEvent unityEvent;
			if (this.m_Events.TryGetValue(key, out unityEvent))
			{
				return unityEvent;
			}
			return null;
		}

		// Token: 0x06000257 RID: 599 RVA: 0x00008860 File Offset: 0x00006A60
		public int Count()
		{
			return this.m_Events.signals.Count;
		}

		// Token: 0x06000258 RID: 600 RVA: 0x00008874 File Offset: 0x00006A74
		public void ChangeSignalAtIndex(int idx, SignalAsset newKey)
		{
			if (idx < 0 || idx > this.m_Events.signals.Count - 1)
			{
				throw new IndexOutOfRangeException();
			}
			if (this.m_Events.signals[idx] == newKey)
			{
				return;
			}
			bool flag = this.m_Events.signals.Contains(newKey);
			if (newKey == null || this.m_Events.signals[idx] == null || !flag)
			{
				this.m_Events.signals[idx] = newKey;
			}
			if (newKey != null && flag)
			{
				throw new ArgumentException("SignalAsset already used.");
			}
		}

		// Token: 0x06000259 RID: 601 RVA: 0x00008919 File Offset: 0x00006B19
		public void RemoveAtIndex(int idx)
		{
			if (idx < 0 || idx > this.m_Events.signals.Count - 1)
			{
				throw new IndexOutOfRangeException();
			}
			this.m_Events.Remove(idx);
		}

		// Token: 0x0600025A RID: 602 RVA: 0x00008946 File Offset: 0x00006B46
		public void ChangeReactionAtIndex(int idx, UnityEvent reaction)
		{
			if (idx < 0 || idx > this.m_Events.events.Count - 1)
			{
				throw new IndexOutOfRangeException();
			}
			this.m_Events.events[idx] = reaction;
		}

		// Token: 0x0600025B RID: 603 RVA: 0x00008979 File Offset: 0x00006B79
		public UnityEvent GetReactionAtIndex(int idx)
		{
			if (idx < 0 || idx > this.m_Events.events.Count - 1)
			{
				throw new IndexOutOfRangeException();
			}
			return this.m_Events.events[idx];
		}

		// Token: 0x0600025C RID: 604 RVA: 0x000089AB File Offset: 0x00006BAB
		public SignalAsset GetSignalAssetAtIndex(int idx)
		{
			if (idx < 0 || idx > this.m_Events.signals.Count - 1)
			{
				throw new IndexOutOfRangeException();
			}
			return this.m_Events.signals[idx];
		}

		// Token: 0x0600025D RID: 605 RVA: 0x000089DD File Offset: 0x00006BDD
		private void OnEnable()
		{
		}

		// Token: 0x040000D0 RID: 208
		[SerializeField]
		private SignalReceiver.EventKeyValue m_Events = new SignalReceiver.EventKeyValue();

		// Token: 0x02000073 RID: 115
		[Serializable]
		private class EventKeyValue
		{
			// Token: 0x0600035D RID: 861 RVA: 0x0000BB88 File Offset: 0x00009D88
			public bool TryGetValue(SignalAsset key, out UnityEvent value)
			{
				int num = this.m_Signals.IndexOf(key);
				if (num != -1)
				{
					value = this.m_Events[num];
					return true;
				}
				value = null;
				return false;
			}

			// Token: 0x0600035E RID: 862 RVA: 0x0000BBBA File Offset: 0x00009DBA
			public void Append(SignalAsset key, UnityEvent value)
			{
				this.m_Signals.Add(key);
				this.m_Events.Add(value);
			}

			// Token: 0x0600035F RID: 863 RVA: 0x0000BBD4 File Offset: 0x00009DD4
			public void Remove(int idx)
			{
				if (idx != -1)
				{
					this.m_Signals.RemoveAt(idx);
					this.m_Events.RemoveAt(idx);
				}
			}

			// Token: 0x06000360 RID: 864 RVA: 0x0000BBF4 File Offset: 0x00009DF4
			public void Remove(SignalAsset key)
			{
				int num = this.m_Signals.IndexOf(key);
				if (num != -1)
				{
					this.m_Signals.RemoveAt(num);
					this.m_Events.RemoveAt(num);
				}
			}

			// Token: 0x170000CD RID: 205
			// (get) Token: 0x06000361 RID: 865 RVA: 0x0000BC2A File Offset: 0x00009E2A
			public List<SignalAsset> signals
			{
				get
				{
					return this.m_Signals;
				}
			}

			// Token: 0x170000CE RID: 206
			// (get) Token: 0x06000362 RID: 866 RVA: 0x0000BC32 File Offset: 0x00009E32
			public List<UnityEvent> events
			{
				get
				{
					return this.m_Events;
				}
			}

			// Token: 0x0400016D RID: 365
			[SerializeField]
			private List<SignalAsset> m_Signals = new List<SignalAsset>();

			// Token: 0x0400016E RID: 366
			[SerializeField]
			[CustomSignalEventDrawer]
			private List<UnityEvent> m_Events = new List<UnityEvent>();
		}
	}
}
