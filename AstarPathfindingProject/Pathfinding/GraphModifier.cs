using System;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000074 RID: 116
	[ExecuteInEditMode]
	public abstract class GraphModifier : VersionedMonoBehaviour
	{
		// Token: 0x0600043A RID: 1082 RVA: 0x000146C4 File Offset: 0x000128C4
		protected static List<T> GetModifiersOfType<T>() where T : GraphModifier
		{
			GraphModifier graphModifier = GraphModifier.root;
			List<T> list = new List<T>();
			while (graphModifier != null)
			{
				T t = graphModifier as T;
				if (t != null)
				{
					list.Add(t);
				}
				graphModifier = graphModifier.next;
			}
			return list;
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x00014714 File Offset: 0x00012914
		public static void FindAllModifiers()
		{
			GraphModifier[] array = UnityCompatibility.FindObjectsByTypeSorted<GraphModifier>();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].enabled && array[i].next == null)
				{
					array[i].enabled = false;
					array[i].enabled = true;
				}
			}
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x00014764 File Offset: 0x00012964
		public static void TriggerEvent(GraphModifier.EventType type)
		{
			if (!Application.isPlaying)
			{
				GraphModifier.FindAllModifiers();
			}
			try
			{
				GraphModifier graphModifier = GraphModifier.root;
				if (type <= GraphModifier.EventType.PostUpdate)
				{
					switch (type)
					{
					case GraphModifier.EventType.PostScan:
						while (graphModifier != null)
						{
							graphModifier.OnPostScan();
							graphModifier = graphModifier.next;
						}
						break;
					case GraphModifier.EventType.PreScan:
						while (graphModifier != null)
						{
							graphModifier.OnPreScan();
							graphModifier = graphModifier.next;
						}
						break;
					case (GraphModifier.EventType)3:
						break;
					case GraphModifier.EventType.LatePostScan:
						while (graphModifier != null)
						{
							graphModifier.OnLatePostScan();
							graphModifier = graphModifier.next;
						}
						break;
					default:
						if (type != GraphModifier.EventType.PreUpdate)
						{
							if (type == GraphModifier.EventType.PostUpdate)
							{
								while (graphModifier != null)
								{
									graphModifier.OnGraphsPostUpdate();
									graphModifier = graphModifier.next;
								}
							}
						}
						else
						{
							while (graphModifier != null)
							{
								graphModifier.OnGraphsPreUpdate();
								graphModifier = graphModifier.next;
							}
						}
						break;
					}
				}
				else if (type != GraphModifier.EventType.PostCacheLoad)
				{
					if (type != GraphModifier.EventType.PostUpdateBeforeAreaRecalculation)
					{
						if (type == GraphModifier.EventType.PostGraphLoad)
						{
							while (graphModifier != null)
							{
								graphModifier.OnPostGraphLoad();
								graphModifier = graphModifier.next;
							}
						}
					}
					else
					{
						while (graphModifier != null)
						{
							graphModifier.OnGraphsPostUpdateBeforeAreaRecalculation();
							graphModifier = graphModifier.next;
						}
					}
				}
				else
				{
					while (graphModifier != null)
					{
						graphModifier.OnPostCacheLoad();
						graphModifier = graphModifier.next;
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x000148C0 File Offset: 0x00012AC0
		protected virtual void OnEnable()
		{
			this.RemoveFromLinkedList();
			this.AddToLinkedList();
			this.ConfigureUniqueID();
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x000148D4 File Offset: 0x00012AD4
		protected virtual void OnDisable()
		{
			this.RemoveFromLinkedList();
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x000148DC File Offset: 0x00012ADC
		protected override void Awake()
		{
			base.Awake();
			this.ConfigureUniqueID();
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x000148EC File Offset: 0x00012AEC
		private void ConfigureUniqueID()
		{
			GraphModifier graphModifier;
			if (GraphModifier.usedIDs.TryGetValue(this.uniqueID, out graphModifier) && graphModifier != this)
			{
				this.Reset();
			}
			GraphModifier.usedIDs[this.uniqueID] = this;
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x0001492D File Offset: 0x00012B2D
		private void AddToLinkedList()
		{
			if (GraphModifier.root == null)
			{
				GraphModifier.root = this;
				return;
			}
			this.next = GraphModifier.root;
			GraphModifier.root.prev = this;
			GraphModifier.root = this;
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x00014960 File Offset: 0x00012B60
		private void RemoveFromLinkedList()
		{
			if (GraphModifier.root == this)
			{
				GraphModifier.root = this.next;
				if (GraphModifier.root != null)
				{
					GraphModifier.root.prev = null;
				}
			}
			else
			{
				if (this.prev != null)
				{
					this.prev.next = this.next;
				}
				if (this.next != null)
				{
					this.next.prev = this.prev;
				}
			}
			this.prev = null;
			this.next = null;
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x000149EB File Offset: 0x00012BEB
		protected virtual void OnDestroy()
		{
			GraphModifier.usedIDs.Remove(this.uniqueID);
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x00003788 File Offset: 0x00001988
		public virtual void OnPostScan()
		{
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x00003788 File Offset: 0x00001988
		public virtual void OnPreScan()
		{
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x00003788 File Offset: 0x00001988
		public virtual void OnLatePostScan()
		{
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x00003788 File Offset: 0x00001988
		public virtual void OnPostCacheLoad()
		{
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x00003788 File Offset: 0x00001988
		public virtual void OnPostGraphLoad()
		{
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x00003788 File Offset: 0x00001988
		public virtual void OnGraphsPreUpdate()
		{
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x00003788 File Offset: 0x00001988
		public virtual void OnGraphsPostUpdate()
		{
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x00003788 File Offset: 0x00001988
		public virtual void OnGraphsPostUpdateBeforeAreaRecalculation()
		{
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x00014A00 File Offset: 0x00012C00
		protected override void Reset()
		{
			base.Reset();
			ulong num = (ulong)((long)Random.Range(0, int.MaxValue));
			ulong num2 = (ulong)((ulong)((long)Random.Range(0, int.MaxValue)) << 32);
			this.uniqueID = num | num2;
			GraphModifier.usedIDs[this.uniqueID] = this;
		}

		// Token: 0x040002A0 RID: 672
		private static GraphModifier root;

		// Token: 0x040002A1 RID: 673
		private GraphModifier prev;

		// Token: 0x040002A2 RID: 674
		private GraphModifier next;

		// Token: 0x040002A3 RID: 675
		[SerializeField]
		[HideInInspector]
		protected ulong uniqueID;

		// Token: 0x040002A4 RID: 676
		protected static Dictionary<ulong, GraphModifier> usedIDs = new Dictionary<ulong, GraphModifier>();

		// Token: 0x02000075 RID: 117
		public enum EventType
		{
			// Token: 0x040002A6 RID: 678
			PostScan = 1,
			// Token: 0x040002A7 RID: 679
			PreScan,
			// Token: 0x040002A8 RID: 680
			LatePostScan = 4,
			// Token: 0x040002A9 RID: 681
			PreUpdate = 8,
			// Token: 0x040002AA RID: 682
			PostUpdate = 16,
			// Token: 0x040002AB RID: 683
			PostCacheLoad = 32,
			// Token: 0x040002AC RID: 684
			PostUpdateBeforeAreaRecalculation = 64,
			// Token: 0x040002AD RID: 685
			PostGraphLoad = 128
		}
	}
}
