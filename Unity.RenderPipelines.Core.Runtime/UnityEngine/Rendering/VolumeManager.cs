using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace UnityEngine.Rendering
{
	// Token: 0x020000EC RID: 236
	public sealed class VolumeManager
	{
		// Token: 0x1700012A RID: 298
		// (get) Token: 0x060007BA RID: 1978 RVA: 0x00025ED0 File Offset: 0x000240D0
		public static VolumeManager instance
		{
			get
			{
				return VolumeManager.s_Instance.Value;
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x060007BB RID: 1979 RVA: 0x00025EDC File Offset: 0x000240DC
		// (set) Token: 0x060007BC RID: 1980 RVA: 0x00025EE4 File Offset: 0x000240E4
		public VolumeStack stack { get; set; }

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x060007BD RID: 1981 RVA: 0x00025EED File Offset: 0x000240ED
		// (set) Token: 0x060007BE RID: 1982 RVA: 0x00025EF5 File Offset: 0x000240F5
		[Obsolete("Please use baseComponentTypeArray instead.")]
		public IEnumerable<Type> baseComponentTypes
		{
			get
			{
				return this.baseComponentTypeArray;
			}
			private set
			{
				this.baseComponentTypeArray = value.ToArray<Type>();
			}
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x00025F04 File Offset: 0x00024104
		internal static List<ValueTuple<string, Type>> GetSupportedVolumeComponents(Type currentPipelineType)
		{
			List<ValueTuple<string, Type>> list;
			if (VolumeManager.s_SupportedVolumeComponentsForRenderPipeline.TryGetValue(currentPipelineType, out list))
			{
				return list;
			}
			list = VolumeManager.FilterVolumeComponentTypes(VolumeManager.instance.baseComponentTypeArray, currentPipelineType);
			VolumeManager.s_SupportedVolumeComponentsForRenderPipeline[currentPipelineType] = list;
			return list;
		}

		// Token: 0x060007C0 RID: 1984 RVA: 0x00025F40 File Offset: 0x00024140
		private static List<ValueTuple<string, Type>> FilterVolumeComponentTypes(Type[] types, Type currentPipelineType)
		{
			List<ValueTuple<string, Type>> list = new List<ValueTuple<string, Type>>();
			foreach (Type type in types)
			{
				string text = string.Empty;
				object[] customAttributes = type.GetCustomAttributes(false);
				bool flag = false;
				foreach (object obj in customAttributes)
				{
					VolumeComponentMenu volumeComponentMenu = obj as VolumeComponentMenu;
					if (volumeComponentMenu == null)
					{
						if (obj is HideInInspector || obj is ObsoleteAttribute)
						{
							flag = true;
						}
					}
					else
					{
						text = volumeComponentMenu.menu;
						VolumeComponentMenuForRenderPipeline volumeComponentMenuForRenderPipeline = volumeComponentMenu as VolumeComponentMenuForRenderPipeline;
						if (volumeComponentMenuForRenderPipeline != null)
						{
							flag |= !volumeComponentMenuForRenderPipeline.pipelineTypes.Contains(currentPipelineType);
						}
					}
				}
				if (!flag)
				{
					if (string.IsNullOrEmpty(text))
					{
						text = type.Name;
					}
					list.Add(new ValueTuple<string, Type>(text, type));
				}
			}
			return list.OrderBy<ValueTuple<string, Type>, string>((ValueTuple<string, Type> i) => i.Item1).ToList<ValueTuple<string, Type>>();
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x060007C1 RID: 1985 RVA: 0x00026038 File Offset: 0x00024238
		// (set) Token: 0x060007C2 RID: 1986 RVA: 0x00026040 File Offset: 0x00024240
		public Type[] baseComponentTypeArray { get; private set; }

		// Token: 0x060007C3 RID: 1987 RVA: 0x0002604C File Offset: 0x0002424C
		internal VolumeComponent GetDefaultVolumeComponent(Type volumeComponentType)
		{
			foreach (VolumeComponent volumeComponent in this.m_ComponentsDefaultState)
			{
				if (volumeComponent.GetType() == volumeComponentType)
				{
					return volumeComponent;
				}
			}
			return null;
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x000260B0 File Offset: 0x000242B0
		private VolumeManager()
		{
			this.m_SortedVolumes = new Dictionary<int, List<Volume>>();
			this.m_Volumes = new List<Volume>();
			this.m_SortNeeded = new Dictionary<int, bool>();
			this.m_TempColliders = new List<Collider>(8);
			this.m_ComponentsDefaultState = new List<VolumeComponent>();
			this.ReloadBaseTypes();
			this.m_DefaultStack = this.CreateStack();
			this.stack = this.m_DefaultStack;
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x00026119 File Offset: 0x00024319
		public VolumeStack CreateStack()
		{
			VolumeStack volumeStack = new VolumeStack();
			volumeStack.Reload(this.m_ComponentsDefaultState);
			return volumeStack;
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x0002612C File Offset: 0x0002432C
		public void ResetMainStack()
		{
			this.stack = this.m_DefaultStack;
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x0002613A File Offset: 0x0002433A
		public void DestroyStack(VolumeStack stack)
		{
			stack.Dispose();
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x00026144 File Offset: 0x00024344
		private void ReloadBaseTypes()
		{
			this.m_ComponentsDefaultState.Clear();
			this.baseComponentTypeArray = (from t in CoreUtils.GetAllTypesDerivedFrom<VolumeComponent>()
				where !t.IsAbstract
				select t).ToArray<Type>();
			BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
			foreach (Type type in this.baseComponentTypeArray)
			{
				MethodInfo method = type.GetMethod("Init", bindingFlags);
				if (method != null)
				{
					method.Invoke(null, null);
				}
				VolumeComponent volumeComponent = (VolumeComponent)ScriptableObject.CreateInstance(type);
				this.m_ComponentsDefaultState.Add(volumeComponent);
			}
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x000261DC File Offset: 0x000243DC
		public void Register(Volume volume, int layer)
		{
			if (this.m_Volumes.Contains(volume))
			{
				return;
			}
			this.m_Volumes.Add(volume);
			foreach (KeyValuePair<int, List<Volume>> keyValuePair in this.m_SortedVolumes)
			{
				if ((keyValuePair.Key & (1 << layer)) != 0 && !keyValuePair.Value.Contains(volume))
				{
					keyValuePair.Value.Add(volume);
				}
			}
			this.SetLayerDirty(layer);
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x00026278 File Offset: 0x00024478
		public void Unregister(Volume volume, int layer)
		{
			if (this.m_Volumes.Remove(volume))
			{
				foreach (KeyValuePair<int, List<Volume>> keyValuePair in this.m_SortedVolumes)
				{
					if ((keyValuePair.Key & (1 << layer)) != 0)
					{
						keyValuePair.Value.Remove(volume);
					}
				}
			}
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x000262F0 File Offset: 0x000244F0
		public bool IsComponentActiveInMask<T>(LayerMask layerMask) where T : VolumeComponent
		{
			int value = layerMask.value;
			foreach (KeyValuePair<int, List<Volume>> keyValuePair in this.m_SortedVolumes)
			{
				if (keyValuePair.Key == value)
				{
					foreach (Volume volume in keyValuePair.Value)
					{
						T t;
						if (volume.enabled && !(volume.profileRef == null) && volume.profileRef.TryGet<T>(out t) && t.active)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x000263D0 File Offset: 0x000245D0
		internal void SetLayerDirty(int layer)
		{
			foreach (KeyValuePair<int, List<Volume>> keyValuePair in this.m_SortedVolumes)
			{
				int key = keyValuePair.Key;
				if ((key & (1 << layer)) != 0)
				{
					this.m_SortNeeded[key] = true;
				}
			}
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x0002643C File Offset: 0x0002463C
		internal void UpdateVolumeLayer(Volume volume, int prevLayer, int newLayer)
		{
			this.Unregister(volume, prevLayer);
			this.Register(volume, newLayer);
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x00026450 File Offset: 0x00024650
		private void OverrideData(VolumeStack stack, List<VolumeComponent> components, float interpFactor)
		{
			int count = components.Count;
			for (int i = 0; i < count; i++)
			{
				VolumeComponent volumeComponent = components[i];
				if (volumeComponent.active)
				{
					VolumeComponent component = stack.GetComponent(volumeComponent.GetType());
					volumeComponent.Override(component, interpFactor);
				}
			}
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x00026498 File Offset: 0x00024698
		internal void ReplaceData(VolumeStack stack)
		{
			ValueTuple<VolumeParameter, VolumeParameter>[] defaultParameters = stack.defaultParameters;
			int num = defaultParameters.Length;
			for (int i = 0; i < num; i++)
			{
				ValueTuple<VolumeParameter, VolumeParameter> valueTuple = defaultParameters[i];
				VolumeParameter item = valueTuple.Item1;
				item.overrideState = false;
				item.SetValue(valueTuple.Item2);
			}
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x000264DC File Offset: 0x000246DC
		[Conditional("UNITY_EDITOR")]
		public void CheckBaseTypes()
		{
			if (this.m_ComponentsDefaultState == null || (this.m_ComponentsDefaultState.Count > 0 && this.m_ComponentsDefaultState[0] == null))
			{
				this.ReloadBaseTypes();
			}
		}

		// Token: 0x060007D1 RID: 2001 RVA: 0x00026510 File Offset: 0x00024710
		[Conditional("UNITY_EDITOR")]
		public void CheckStack(VolumeStack stack)
		{
			Dictionary<Type, VolumeComponent> components = stack.components;
			if (components == null)
			{
				stack.Reload(this.m_ComponentsDefaultState);
				return;
			}
			foreach (KeyValuePair<Type, VolumeComponent> keyValuePair in components)
			{
				if (keyValuePair.Key == null || keyValuePair.Value == null)
				{
					stack.Reload(this.m_ComponentsDefaultState);
					break;
				}
			}
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x0002659C File Offset: 0x0002479C
		private bool CheckUpdateRequired(VolumeStack stack)
		{
			if (this.m_Volumes.Count != 0)
			{
				stack.requiresReset = true;
				return true;
			}
			if (stack.requiresReset)
			{
				stack.requiresReset = false;
				return true;
			}
			return false;
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x000265C6 File Offset: 0x000247C6
		public void Update(Transform trigger, LayerMask layerMask)
		{
			this.Update(this.stack, trigger, layerMask);
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x000265D8 File Offset: 0x000247D8
		public void Update(VolumeStack stack, Transform trigger, LayerMask layerMask)
		{
			if (!this.CheckUpdateRequired(stack))
			{
				return;
			}
			this.ReplaceData(stack);
			bool flag = trigger == null;
			Vector3 vector = (flag ? Vector3.zero : trigger.position);
			List<Volume> list = this.GrabVolumes(layerMask);
			Camera camera = null;
			if (!flag)
			{
				trigger.TryGetComponent<Camera>(out camera);
			}
			int count = list.Count;
			for (int i = 0; i < count; i++)
			{
				Volume volume = list[i];
				if (!(volume == null) && volume.enabled && !(volume.profileRef == null) && volume.weight > 0f)
				{
					if (volume.isGlobal)
					{
						this.OverrideData(stack, volume.profileRef.components, Mathf.Clamp01(volume.weight));
					}
					else if (!flag)
					{
						List<Collider> tempColliders = this.m_TempColliders;
						volume.GetComponents<Collider>(tempColliders);
						if (tempColliders.Count != 0)
						{
							float num = float.PositiveInfinity;
							int count2 = tempColliders.Count;
							for (int j = 0; j < count2; j++)
							{
								Collider collider = tempColliders[j];
								if (collider.enabled)
								{
									float sqrMagnitude = (collider.ClosestPoint(vector) - vector).sqrMagnitude;
									if (sqrMagnitude < num)
									{
										num = sqrMagnitude;
									}
								}
							}
							tempColliders.Clear();
							float num2 = volume.blendDistance * volume.blendDistance;
							if (num <= num2)
							{
								float num3 = 1f;
								if (num2 > 0f)
								{
									num3 = 1f - num / num2;
								}
								this.OverrideData(stack, volume.profileRef.components, num3 * Mathf.Clamp01(volume.weight));
							}
						}
					}
				}
			}
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x0002678F File Offset: 0x0002498F
		public Volume[] GetVolumes(LayerMask layerMask)
		{
			List<Volume> list = this.GrabVolumes(layerMask);
			list.RemoveAll((Volume v) => v == null);
			return list.ToArray();
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x000267C4 File Offset: 0x000249C4
		private List<Volume> GrabVolumes(LayerMask mask)
		{
			List<Volume> list;
			if (!this.m_SortedVolumes.TryGetValue(mask, out list))
			{
				list = new List<Volume>();
				int count = this.m_Volumes.Count;
				for (int i = 0; i < count; i++)
				{
					Volume volume = this.m_Volumes[i];
					if ((mask & (1 << volume.gameObject.layer)) != 0)
					{
						list.Add(volume);
						this.m_SortNeeded[mask] = true;
					}
				}
				this.m_SortedVolumes.Add(mask, list);
			}
			bool flag;
			if (this.m_SortNeeded.TryGetValue(mask, out flag) && flag)
			{
				this.m_SortNeeded[mask] = false;
				VolumeManager.SortByPriority(list);
			}
			return list;
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x00026888 File Offset: 0x00024A88
		private static void SortByPriority(List<Volume> volumes)
		{
			for (int i = 1; i < volumes.Count; i++)
			{
				Volume volume = volumes[i];
				int num = i - 1;
				while (num >= 0 && volumes[num].priority > volume.priority)
				{
					volumes[num + 1] = volumes[num];
					num--;
				}
				volumes[num + 1] = volume;
			}
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x000268EA File Offset: 0x00024AEA
		private static bool IsVolumeRenderedByCamera(Volume volume, Camera camera)
		{
			return true;
		}

		// Token: 0x040004C7 RID: 1223
		private static readonly Lazy<VolumeManager> s_Instance = new Lazy<VolumeManager>(() => new VolumeManager());

		// Token: 0x040004C9 RID: 1225
		private static readonly Dictionary<Type, List<ValueTuple<string, Type>>> s_SupportedVolumeComponentsForRenderPipeline = new Dictionary<Type, List<ValueTuple<string, Type>>>();

		// Token: 0x040004CB RID: 1227
		private const int k_MaxLayerCount = 32;

		// Token: 0x040004CC RID: 1228
		private readonly Dictionary<int, List<Volume>> m_SortedVolumes;

		// Token: 0x040004CD RID: 1229
		private readonly List<Volume> m_Volumes;

		// Token: 0x040004CE RID: 1230
		private readonly Dictionary<int, bool> m_SortNeeded;

		// Token: 0x040004CF RID: 1231
		private readonly List<VolumeComponent> m_ComponentsDefaultState;

		// Token: 0x040004D0 RID: 1232
		private readonly List<Collider> m_TempColliders;

		// Token: 0x040004D1 RID: 1233
		private VolumeStack m_DefaultStack;
	}
}
