using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem.LowLevel;

namespace UnityEngine.InputSystem.EnhancedTouch
{
	// Token: 0x020000A0 RID: 160
	public struct TouchHistory : IReadOnlyList<Touch>, IEnumerable<Touch>, IEnumerable, IReadOnlyCollection<Touch>
	{
		// Token: 0x06000C6F RID: 3183 RVA: 0x000424D4 File Offset: 0x000406D4
		internal TouchHistory(Finger finger, InputStateHistory<TouchState> history, int startIndex = -1, int count = -1)
		{
			this.m_Finger = finger;
			this.m_History = history;
			this.m_Version = history.version;
			this.m_Count = ((count >= 0) ? count : this.m_History.Count);
			this.m_StartIndex = ((startIndex >= 0) ? startIndex : (this.m_History.Count - 1));
		}

		// Token: 0x06000C70 RID: 3184 RVA: 0x0004252F File Offset: 0x0004072F
		public IEnumerator<Touch> GetEnumerator()
		{
			return new TouchHistory.Enumerator(this);
		}

		// Token: 0x06000C71 RID: 3185 RVA: 0x0004253C File Offset: 0x0004073C
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x06000C72 RID: 3186 RVA: 0x00042544 File Offset: 0x00040744
		public int Count
		{
			get
			{
				return this.m_Count;
			}
		}

		// Token: 0x17000355 RID: 853
		public Touch this[int index]
		{
			get
			{
				this.CheckValid();
				if (index < 0 || index >= this.Count)
				{
					throw new ArgumentOutOfRangeException(string.Format("Index {0} is out of range for history with {1} entries", index, this.Count), "index");
				}
				return new Touch(this.m_Finger, this.m_History[this.m_StartIndex - index]);
			}
		}

		// Token: 0x06000C74 RID: 3188 RVA: 0x000425B0 File Offset: 0x000407B0
		internal void CheckValid()
		{
			if (this.m_Finger == null || this.m_History == null)
			{
				throw new InvalidOperationException("Touch history not initialized");
			}
			if (this.m_History.version != this.m_Version)
			{
				throw new InvalidOperationException("Touch history is no longer valid; the recorded history has been changed");
			}
		}

		// Token: 0x04000482 RID: 1154
		private readonly InputStateHistory<TouchState> m_History;

		// Token: 0x04000483 RID: 1155
		private readonly Finger m_Finger;

		// Token: 0x04000484 RID: 1156
		private readonly int m_Count;

		// Token: 0x04000485 RID: 1157
		private readonly int m_StartIndex;

		// Token: 0x04000486 RID: 1158
		private readonly uint m_Version;

		// Token: 0x020001FC RID: 508
		private class Enumerator : IEnumerator<Touch>, IEnumerator, IDisposable
		{
			// Token: 0x060014ED RID: 5357 RVA: 0x00061EBF File Offset: 0x000600BF
			internal Enumerator(TouchHistory owner)
			{
				this.m_Owner = owner;
				this.m_Index = -1;
			}

			// Token: 0x060014EE RID: 5358 RVA: 0x00061ED8 File Offset: 0x000600D8
			public bool MoveNext()
			{
				if (this.m_Index >= this.m_Owner.Count - 1)
				{
					return false;
				}
				this.m_Index++;
				return true;
			}

			// Token: 0x060014EF RID: 5359 RVA: 0x00061F0E File Offset: 0x0006010E
			public void Reset()
			{
				this.m_Index = -1;
			}

			// Token: 0x170005A0 RID: 1440
			// (get) Token: 0x060014F0 RID: 5360 RVA: 0x00061F18 File Offset: 0x00060118
			public Touch Current
			{
				get
				{
					return this.m_Owner[this.m_Index];
				}
			}

			// Token: 0x170005A1 RID: 1441
			// (get) Token: 0x060014F1 RID: 5361 RVA: 0x00061F39 File Offset: 0x00060139
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x060014F2 RID: 5362 RVA: 0x00061F46 File Offset: 0x00060146
			public void Dispose()
			{
			}

			// Token: 0x04000B49 RID: 2889
			private readonly TouchHistory m_Owner;

			// Token: 0x04000B4A RID: 2890
			private int m_Index;
		}
	}
}
