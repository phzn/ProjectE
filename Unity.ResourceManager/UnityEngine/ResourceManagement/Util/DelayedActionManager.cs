using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace UnityEngine.ResourceManagement.Util
{
	// Token: 0x02000016 RID: 22
	internal class DelayedActionManager : ComponentSingleton<DelayedActionManager>
	{
		// Token: 0x060000BF RID: 191 RVA: 0x000045B9 File Offset: 0x000027B9
		private LinkedListNode<DelayedActionManager.DelegateInfo> GetNode(ref DelayedActionManager.DelegateInfo del)
		{
			if (this.m_NodeCache.Count > 0)
			{
				LinkedListNode<DelayedActionManager.DelegateInfo> linkedListNode = this.m_NodeCache.Pop();
				linkedListNode.Value = del;
				return linkedListNode;
			}
			return new LinkedListNode<DelayedActionManager.DelegateInfo>(del);
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x000045EC File Offset: 0x000027EC
		public static void Clear()
		{
			if (ComponentSingleton<DelayedActionManager>.Exists)
			{
				ComponentSingleton<DelayedActionManager>.Instance.DestroyWhenComplete();
			}
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x000045FF File Offset: 0x000027FF
		private void DestroyWhenComplete()
		{
			this.m_DestroyOnCompletion = true;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00004608 File Offset: 0x00002808
		public static void AddAction(Delegate action, float delay = 0f, params object[] parameters)
		{
			ComponentSingleton<DelayedActionManager>.Instance.AddActionInternal(action, delay, parameters);
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00004618 File Offset: 0x00002818
		private void AddActionInternal(Delegate action, float delay, params object[] parameters)
		{
			DelayedActionManager.DelegateInfo delegateInfo = new DelayedActionManager.DelegateInfo(action, Time.unscaledTime + delay, parameters);
			if (delay <= 0f)
			{
				this.m_Actions[this.m_CollectionIndex].Add(delegateInfo);
				return;
			}
			if (this.m_DelayedActions.Count == 0)
			{
				this.m_DelayedActions.AddFirst(this.GetNode(ref delegateInfo));
				return;
			}
			LinkedListNode<DelayedActionManager.DelegateInfo> linkedListNode = this.m_DelayedActions.Last;
			while (linkedListNode != null && linkedListNode.Value.InvocationTime > delegateInfo.InvocationTime)
			{
				linkedListNode = linkedListNode.Previous;
			}
			if (linkedListNode == null)
			{
				this.m_DelayedActions.AddFirst(this.GetNode(ref delegateInfo));
				return;
			}
			this.m_DelayedActions.AddBefore(linkedListNode, this.GetNode(ref delegateInfo));
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x000046CC File Offset: 0x000028CC
		public static bool IsActive
		{
			get
			{
				if (!ComponentSingleton<DelayedActionManager>.Exists)
				{
					return false;
				}
				if (ComponentSingleton<DelayedActionManager>.Instance.m_DelayedActions.Count > 0)
				{
					return true;
				}
				for (int i = 0; i < ComponentSingleton<DelayedActionManager>.Instance.m_Actions.Length; i++)
				{
					if (ComponentSingleton<DelayedActionManager>.Instance.m_Actions[i].Count > 0)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00004724 File Offset: 0x00002924
		public static bool Wait(float timeout = 0f, float timeAdvanceAmount = 0f)
		{
			if (!DelayedActionManager.IsActive)
			{
				return true;
			}
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			float num = Time.unscaledTime;
			do
			{
				ComponentSingleton<DelayedActionManager>.Instance.InternalLateUpdate(num);
				if (timeAdvanceAmount >= 0f)
				{
					num += timeAdvanceAmount;
				}
				else
				{
					num = Time.unscaledTime;
				}
			}
			while (DelayedActionManager.IsActive && (timeout <= 0f || stopwatch.Elapsed.TotalSeconds < (double)timeout));
			return !DelayedActionManager.IsActive;
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00004794 File Offset: 0x00002994
		private void LateUpdate()
		{
			this.InternalLateUpdate(Time.unscaledTime);
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x000047A4 File Offset: 0x000029A4
		private void InternalLateUpdate(float t)
		{
			int num = 0;
			while (this.m_DelayedActions.Count > 0 && this.m_DelayedActions.First.Value.InvocationTime <= t)
			{
				this.m_Actions[this.m_CollectionIndex].Add(this.m_DelayedActions.First.Value);
				this.m_NodeCache.Push(this.m_DelayedActions.First);
				this.m_DelayedActions.RemoveFirst();
			}
			do
			{
				int collectionIndex = this.m_CollectionIndex;
				this.m_CollectionIndex = (this.m_CollectionIndex + 1) % 2;
				List<DelayedActionManager.DelegateInfo> list = this.m_Actions[collectionIndex];
				if (list.Count > 0)
				{
					for (int i = 0; i < list.Count; i++)
					{
						list[i].Invoke();
					}
					list.Clear();
				}
				num++;
			}
			while (this.m_Actions[this.m_CollectionIndex].Count > 0);
			if (this.m_DestroyOnCompletion && !DelayedActionManager.IsActive)
			{
				Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x000048A8 File Offset: 0x00002AA8
		private void OnApplicationQuit()
		{
			if (ComponentSingleton<DelayedActionManager>.Exists)
			{
				Object.Destroy(ComponentSingleton<DelayedActionManager>.Instance.gameObject);
			}
		}

		// Token: 0x04000045 RID: 69
		private List<DelayedActionManager.DelegateInfo>[] m_Actions = new List<DelayedActionManager.DelegateInfo>[]
		{
			new List<DelayedActionManager.DelegateInfo>(),
			new List<DelayedActionManager.DelegateInfo>()
		};

		// Token: 0x04000046 RID: 70
		private LinkedList<DelayedActionManager.DelegateInfo> m_DelayedActions = new LinkedList<DelayedActionManager.DelegateInfo>();

		// Token: 0x04000047 RID: 71
		private Stack<LinkedListNode<DelayedActionManager.DelegateInfo>> m_NodeCache = new Stack<LinkedListNode<DelayedActionManager.DelegateInfo>>(10);

		// Token: 0x04000048 RID: 72
		private int m_CollectionIndex;

		// Token: 0x04000049 RID: 73
		private bool m_DestroyOnCompletion;

		// Token: 0x02000072 RID: 114
		private struct DelegateInfo
		{
			// Token: 0x0600034C RID: 844 RVA: 0x0000B80A File Offset: 0x00009A0A
			public DelegateInfo(Delegate d, float invocationTime, params object[] p)
			{
				this.m_Delegate = d;
				this.m_Id = DelayedActionManager.DelegateInfo.s_Id++;
				this.m_Target = p;
				this.InvocationTime = invocationTime;
			}

			// Token: 0x170000D5 RID: 213
			// (get) Token: 0x0600034D RID: 845 RVA: 0x0000B834 File Offset: 0x00009A34
			// (set) Token: 0x0600034E RID: 846 RVA: 0x0000B83C File Offset: 0x00009A3C
			public float InvocationTime { readonly get; private set; }

			// Token: 0x0600034F RID: 847 RVA: 0x0000B848 File Offset: 0x00009A48
			public override string ToString()
			{
				if (this.m_Delegate == null || this.m_Delegate.Method.DeclaringType == null)
				{
					return "Null m_delegate for " + this.m_Id.ToString();
				}
				string[] array = new string[8];
				array[0] = this.m_Id.ToString();
				array[1] = " (target=";
				int num = 2;
				object target = this.m_Delegate.Target;
				array[num] = ((target != null) ? target.ToString() : null);
				array[3] = ") ";
				array[4] = this.m_Delegate.Method.DeclaringType.Name;
				array[5] = ".";
				array[6] = this.m_Delegate.Method.Name;
				array[7] = "(";
				string text = string.Concat(array);
				string text2 = "";
				foreach (object obj in this.m_Target)
				{
					text = text + text2 + ((obj != null) ? obj.ToString() : null);
					text2 = ", ";
				}
				return text + ") @" + this.InvocationTime.ToString();
			}

			// Token: 0x06000350 RID: 848 RVA: 0x0000B960 File Offset: 0x00009B60
			public void Invoke()
			{
				try
				{
					this.m_Delegate.DynamicInvoke(this.m_Target);
				}
				catch (Exception ex)
				{
					Debug.LogErrorFormat("Exception thrown in DynamicInvoke: {0} {1}", new object[] { ex, this });
				}
			}

			// Token: 0x04000151 RID: 337
			private static int s_Id;

			// Token: 0x04000152 RID: 338
			private int m_Id;

			// Token: 0x04000153 RID: 339
			private Delegate m_Delegate;

			// Token: 0x04000154 RID: 340
			private object[] m_Target;
		}
	}
}
