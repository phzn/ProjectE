using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Profiling;

namespace Pathfinding.Util
{
	// Token: 0x020002A0 RID: 672
	[HelpURL("https://arongranberg.com/astar/documentation/stable/batchedevents.html")]
	public class BatchedEvents : VersionedMonoBehaviour
	{
		// Token: 0x060010D3 RID: 4307 RVA: 0x00065751 File Offset: 0x00063951
		private void OnEnable()
		{
			if (BatchedEvents.instance == null)
			{
				BatchedEvents.instance = this;
			}
			BatchedEvents.instance != this;
		}

		// Token: 0x060010D4 RID: 4308 RVA: 0x00065772 File Offset: 0x00063972
		private void OnDisable()
		{
			if (BatchedEvents.instance == this)
			{
				BatchedEvents.instance = null;
			}
		}

		// Token: 0x060010D5 RID: 4309 RVA: 0x00065787 File Offset: 0x00063987
		private static void CreateInstance()
		{
			GameObject gameObject = new GameObject("Batch Helper");
			gameObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.DontSaveInEditor | HideFlags.NotEditable | HideFlags.DontSaveInBuild | HideFlags.DontUnloadUnusedAsset;
			BatchedEvents.instance = gameObject.AddComponent<BatchedEvents>();
			Object.DontDestroyOnLoad(gameObject);
		}

		// Token: 0x060010D6 RID: 4310 RVA: 0x000657AC File Offset: 0x000639AC
		public static T Find<T, K>(K key, Func<T, K, bool> predicate) where T : class, IEntityIndex
		{
			Type typeFromHandle = typeof(T);
			for (int i = 0; i < BatchedEvents.data.Length; i++)
			{
				if (BatchedEvents.data[i].type == typeFromHandle)
				{
					T[] array = BatchedEvents.data[i].objects as T[];
					for (int j = 0; j < BatchedEvents.data[i].objectCount; j++)
					{
						if (predicate(array[j], key))
						{
							return array[j];
						}
					}
				}
			}
			return default(T);
		}

		// Token: 0x060010D7 RID: 4311 RVA: 0x00065844 File Offset: 0x00063A44
		public static void Remove<T>(T obj) where T : IEntityIndex
		{
			int num = obj.EntityIndex;
			if (num == 0)
			{
				return;
			}
			int num2 = ((num & 1069547520) >> 22) - 1;
			num &= -1069547521;
			if (BatchedEvents.isIterating && BatchedEvents.isIteratingOverTypeIndex == num2)
			{
				throw new Exception("Cannot add or remove entities during an event (Update/LateUpdate/...) that this helper initiated");
			}
			BatchedEvents.data[num2].Remove(num);
		}

		// Token: 0x060010D8 RID: 4312 RVA: 0x000658A4 File Offset: 0x00063AA4
		public static int GetComponents<T>(BatchedEvents.Event eventTypes, out TransformAccessArray transforms, out T[] components) where T : Component, IEntityIndex
		{
			if (BatchedEvents.instance == null)
			{
				BatchedEvents.CreateInstance();
			}
			int num = (int)(eventTypes * (BatchedEvents.Event)12582917);
			if (BatchedEvents.isIterating && BatchedEvents.isIteratingOverTypeIndex == num)
			{
				throw new Exception("Cannot add or remove entities during an event (Update/LateUpdate/...) that this helper initiated");
			}
			Type typeFromHandle = typeof(T);
			for (int i = 0; i < BatchedEvents.data.Length; i++)
			{
				if (BatchedEvents.data[i].type == typeFromHandle && BatchedEvents.data[i].variant == num)
				{
					transforms = BatchedEvents.data[i].transforms;
					components = BatchedEvents.data[i].objects as T[];
					return BatchedEvents.data[i].objectCount;
				}
			}
			transforms = default(TransformAccessArray);
			components = null;
			return 0;
		}

		// Token: 0x060010D9 RID: 4313 RVA: 0x00065976 File Offset: 0x00063B76
		public static bool Has<T>(T obj) where T : IEntityIndex
		{
			return obj.EntityIndex != 0;
		}

		// Token: 0x060010DA RID: 4314 RVA: 0x00065988 File Offset: 0x00063B88
		public static void Add<T>(T obj, BatchedEvents.Event eventTypes, Action<T[], int> action, int archetypeVariant = 0) where T : Component, IEntityIndex
		{
			BatchedEvents.Add<T>(obj, eventTypes, null, action, archetypeVariant);
		}

		// Token: 0x060010DB RID: 4315 RVA: 0x00065994 File Offset: 0x00063B94
		public static void Add<T>(T obj, BatchedEvents.Event eventTypes, Action<T[], int, TransformAccessArray, BatchedEvents.Event> action, int archetypeVariant = 0) where T : Component, IEntityIndex
		{
			BatchedEvents.Add<T>(obj, eventTypes, action, null, archetypeVariant);
		}

		// Token: 0x060010DC RID: 4316 RVA: 0x000659A0 File Offset: 0x00063BA0
		private static void Add<T>(T obj, BatchedEvents.Event eventTypes, Action<T[], int, TransformAccessArray, BatchedEvents.Event> action1, Action<T[], int> action2, int archetypeVariant = 0) where T : Component, IEntityIndex
		{
			if (obj.EntityIndex != 0)
			{
				throw new ArgumentException("This object is already registered. Call Remove before adding the object again.");
			}
			if (BatchedEvents.instance == null)
			{
				BatchedEvents.CreateInstance();
			}
			archetypeVariant = (int)(eventTypes * (BatchedEvents.Event)12582917);
			if (BatchedEvents.isIterating && BatchedEvents.isIteratingOverTypeIndex == archetypeVariant)
			{
				throw new Exception("Cannot add or remove entities during an event (Update/LateUpdate/...) that this helper initiated");
			}
			Type type = obj.GetType();
			for (int i = 0; i < BatchedEvents.data.Length; i++)
			{
				if (BatchedEvents.data[i].type == type && BatchedEvents.data[i].variant == archetypeVariant)
				{
					BatchedEvents.data[i].Add(obj);
					return;
				}
			}
			Memory.Realloc<BatchedEvents.Archetype>(ref BatchedEvents.data, BatchedEvents.data.Length + 1);
			Action<T[], int, TransformAccessArray, BatchedEvents.Event> ac1 = action1;
			Action<T[], int> ac2 = action2;
			Action<object[], int, TransformAccessArray, BatchedEvents.Event> action3 = delegate(object[] objs, int count, TransformAccessArray tr, BatchedEvents.Event ev)
			{
				ac1((T[])objs, count, tr, ev);
			};
			Action<object[], int, TransformAccessArray, BatchedEvents.Event> action4 = delegate(object[] objs, int count, TransformAccessArray tr, BatchedEvents.Event ev)
			{
				ac2((T[])objs, count);
			};
			BatchedEvents.data[BatchedEvents.data.Length - 1] = new BatchedEvents.Archetype
			{
				type = type,
				events = eventTypes,
				variant = archetypeVariant,
				archetypeIndex = BatchedEvents.data.Length - 1 + 1,
				action = ((ac1 != null) ? action3 : action4),
				sampler = CustomSampler.Create(type.Name, false)
			};
			BatchedEvents.data[BatchedEvents.data.Length - 1].Add(obj);
		}

		// Token: 0x060010DD RID: 4317 RVA: 0x00065B2C File Offset: 0x00063D2C
		private void Process(BatchedEvents.Event eventType, Type typeFilter)
		{
			try
			{
				BatchedEvents.isIterating = true;
				for (int i = 0; i < BatchedEvents.data.Length; i++)
				{
					ref BatchedEvents.Archetype ptr = ref BatchedEvents.data[i];
					if (ptr.objectCount > 0 && (ptr.events & eventType) != BatchedEvents.Event.None && (typeFilter == null || typeFilter == ptr.type))
					{
						BatchedEvents.isIteratingOverTypeIndex = ptr.variant;
						try
						{
							ptr.action(ptr.objects, ptr.objectCount, ptr.transforms, eventType);
						}
						finally
						{
						}
					}
				}
			}
			finally
			{
				BatchedEvents.isIterating = false;
			}
		}

		// Token: 0x060010DE RID: 4318 RVA: 0x00065BD8 File Offset: 0x00063DD8
		public static void ProcessEvent<T>(BatchedEvents.Event eventType)
		{
			BatchedEvents batchedEvents = BatchedEvents.instance;
			if (batchedEvents == null)
			{
				return;
			}
			batchedEvents.Process(eventType, typeof(T));
		}

		// Token: 0x060010DF RID: 4319 RVA: 0x00065BF4 File Offset: 0x00063DF4
		private void Update()
		{
			this.Process(BatchedEvents.Event.Update, null);
		}

		// Token: 0x060010E0 RID: 4320 RVA: 0x00065BFE File Offset: 0x00063DFE
		private void LateUpdate()
		{
			this.Process(BatchedEvents.Event.LateUpdate, null);
		}

		// Token: 0x060010E1 RID: 4321 RVA: 0x00065C08 File Offset: 0x00063E08
		private void FixedUpdate()
		{
			this.Process(BatchedEvents.Event.FixedUpdate, null);
		}

		// Token: 0x04000C0C RID: 3084
		private const int ArchetypeOffset = 22;

		// Token: 0x04000C0D RID: 3085
		private const int ArchetypeMask = 1069547520;

		// Token: 0x04000C0E RID: 3086
		private static BatchedEvents.Archetype[] data = new BatchedEvents.Archetype[0];

		// Token: 0x04000C0F RID: 3087
		private static BatchedEvents instance;

		// Token: 0x04000C10 RID: 3088
		private static int isIteratingOverTypeIndex = -1;

		// Token: 0x04000C11 RID: 3089
		private static bool isIterating = false;

		// Token: 0x020002A1 RID: 673
		[Flags]
		public enum Event
		{
			// Token: 0x04000C13 RID: 3091
			Update = 1,
			// Token: 0x04000C14 RID: 3092
			LateUpdate = 2,
			// Token: 0x04000C15 RID: 3093
			FixedUpdate = 4,
			// Token: 0x04000C16 RID: 3094
			Custom = 8,
			// Token: 0x04000C17 RID: 3095
			None = 0
		}

		// Token: 0x020002A2 RID: 674
		private struct Archetype
		{
			// Token: 0x060010E4 RID: 4324 RVA: 0x00065C2C File Offset: 0x00063E2C
			public void Add(Component obj)
			{
				this.objectCount++;
				if (this.objects == null)
				{
					this.objects = (object[])Array.CreateInstance(this.type, math.ceilpow2(this.objectCount));
				}
				if (this.objectCount > this.objects.Length)
				{
					Array array = Array.CreateInstance(this.type, math.ceilpow2(this.objectCount));
					this.objects.CopyTo(array, 0);
					this.objects = (object[])array;
				}
				this.objects[this.objectCount - 1] = obj;
				if (!this.transforms.isCreated)
				{
					this.transforms = new TransformAccessArray(16, -1);
				}
				this.transforms.Add(obj.transform);
				((IEntityIndex)obj).EntityIndex = (this.archetypeIndex << 22) | (this.objectCount - 1);
			}

			// Token: 0x060010E5 RID: 4325 RVA: 0x00065D0C File Offset: 0x00063F0C
			public void Remove(int index)
			{
				this.objectCount--;
				((IEntityIndex)this.objects[this.objectCount]).EntityIndex = (this.archetypeIndex << 22) | index;
				((IEntityIndex)this.objects[index]).EntityIndex = 0;
				this.objects[index] = this.objects[this.objectCount];
				this.objects[this.objectCount] = null;
				this.transforms.RemoveAtSwapBack(index);
				if (this.objectCount == 0)
				{
					this.transforms.Dispose();
				}
			}

			// Token: 0x04000C18 RID: 3096
			public object[] objects;

			// Token: 0x04000C19 RID: 3097
			public int objectCount;

			// Token: 0x04000C1A RID: 3098
			public Type type;

			// Token: 0x04000C1B RID: 3099
			public TransformAccessArray transforms;

			// Token: 0x04000C1C RID: 3100
			public int variant;

			// Token: 0x04000C1D RID: 3101
			public int archetypeIndex;

			// Token: 0x04000C1E RID: 3102
			public BatchedEvents.Event events;

			// Token: 0x04000C1F RID: 3103
			public Action<object[], int, TransformAccessArray, BatchedEvents.Event> action;

			// Token: 0x04000C20 RID: 3104
			public CustomSampler sampler;
		}
	}
}
