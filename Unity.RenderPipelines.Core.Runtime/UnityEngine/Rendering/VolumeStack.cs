using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UnityEngine.Rendering
{
	// Token: 0x02000118 RID: 280
	public sealed class VolumeStack : IDisposable
	{
		// Token: 0x0600086B RID: 2155 RVA: 0x00027A67 File Offset: 0x00025C67
		internal VolumeStack()
		{
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x00027A84 File Offset: 0x00025C84
		internal void Clear()
		{
			foreach (KeyValuePair<Type, VolumeComponent> keyValuePair in this.components)
			{
				CoreUtils.Destroy(keyValuePair.Value);
			}
			this.components.Clear();
			if (this.defaultParameters != null)
			{
				ValueTuple<VolumeParameter, VolumeParameter>[] array = this.defaultParameters;
				for (int i = 0; i < array.Length; i++)
				{
					VolumeParameter item = array[i].Item2;
					if (item != null)
					{
						item.Release();
					}
				}
				this.defaultParameters = null;
			}
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x00027B24 File Offset: 0x00025D24
		internal void Reload(List<VolumeComponent> componentDefaultStates)
		{
			this.Clear();
			this.requiresReset = true;
			List<ValueTuple<VolumeParameter, VolumeParameter>> list = new List<ValueTuple<VolumeParameter, VolumeParameter>>();
			foreach (VolumeComponent volumeComponent in componentDefaultStates)
			{
				Type type = volumeComponent.GetType();
				VolumeComponent volumeComponent2 = (VolumeComponent)ScriptableObject.CreateInstance(type);
				this.components.Add(type, volumeComponent2);
				for (int i = 0; i < volumeComponent2.parameterList.Count; i++)
				{
					list.Add(new ValueTuple<VolumeParameter, VolumeParameter>
					{
						Item1 = volumeComponent2.parameters[i],
						Item2 = (volumeComponent.parameterList[i].Clone() as VolumeParameter)
					});
				}
			}
			this.defaultParameters = list.ToArray();
		}

		// Token: 0x0600086E RID: 2158 RVA: 0x00027C10 File Offset: 0x00025E10
		public T GetComponent<T>() where T : VolumeComponent
		{
			return (T)((object)this.GetComponent(typeof(T)));
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x00027C28 File Offset: 0x00025E28
		public VolumeComponent GetComponent(Type type)
		{
			VolumeComponent volumeComponent;
			this.components.TryGetValue(type, out volumeComponent);
			return volumeComponent;
		}

		// Token: 0x06000870 RID: 2160 RVA: 0x00027C45 File Offset: 0x00025E45
		public void Dispose()
		{
			this.Clear();
		}

		// Token: 0x040004F4 RID: 1268
		internal readonly Dictionary<Type, VolumeComponent> components = new Dictionary<Type, VolumeComponent>();

		// Token: 0x040004F5 RID: 1269
		[TupleElementNames(new string[] { "parameter", "defaultValue" })]
		internal ValueTuple<VolumeParameter, VolumeParameter>[] defaultParameters;

		// Token: 0x040004F6 RID: 1270
		internal bool requiresReset = true;
	}
}
