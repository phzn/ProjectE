using System;
using System.Collections.Generic;

namespace UnityEngine.Rendering
{
	// Token: 0x02000117 RID: 279
	public sealed class VolumeProfile : ScriptableObject
	{
		// Token: 0x06000859 RID: 2137 RVA: 0x000275E0 File Offset: 0x000257E0
		private void OnEnable()
		{
			this.components.RemoveAll((VolumeComponent x) => x == null);
		}

		// Token: 0x0600085A RID: 2138 RVA: 0x00027610 File Offset: 0x00025810
		internal void OnDisable()
		{
			if (this.components == null)
			{
				return;
			}
			for (int i = 0; i < this.components.Count; i++)
			{
				if (this.components[i] != null)
				{
					this.components[i].Release();
				}
			}
		}

		// Token: 0x0600085B RID: 2139 RVA: 0x00027661 File Offset: 0x00025861
		public void Reset()
		{
			this.isDirty = true;
		}

		// Token: 0x0600085C RID: 2140 RVA: 0x0002766A File Offset: 0x0002586A
		public T Add<T>(bool overrides = false) where T : VolumeComponent
		{
			return (T)((object)this.Add(typeof(T), overrides));
		}

		// Token: 0x0600085D RID: 2141 RVA: 0x00027684 File Offset: 0x00025884
		public VolumeComponent Add(Type type, bool overrides = false)
		{
			if (this.Has(type))
			{
				throw new InvalidOperationException("Component already exists in the volume");
			}
			VolumeComponent volumeComponent = (VolumeComponent)ScriptableObject.CreateInstance(type);
			volumeComponent.SetAllOverridesTo(overrides);
			this.components.Add(volumeComponent);
			this.isDirty = true;
			return volumeComponent;
		}

		// Token: 0x0600085E RID: 2142 RVA: 0x000276CC File Offset: 0x000258CC
		public void Remove<T>() where T : VolumeComponent
		{
			this.Remove(typeof(T));
		}

		// Token: 0x0600085F RID: 2143 RVA: 0x000276E0 File Offset: 0x000258E0
		public void Remove(Type type)
		{
			int num = -1;
			for (int i = 0; i < this.components.Count; i++)
			{
				if (this.components[i].GetType() == type)
				{
					num = i;
					break;
				}
			}
			if (num >= 0)
			{
				this.components.RemoveAt(num);
				this.isDirty = true;
			}
		}

		// Token: 0x06000860 RID: 2144 RVA: 0x00027739 File Offset: 0x00025939
		public bool Has<T>() where T : VolumeComponent
		{
			return this.Has(typeof(T));
		}

		// Token: 0x06000861 RID: 2145 RVA: 0x0002774C File Offset: 0x0002594C
		public bool Has(Type type)
		{
			using (List<VolumeComponent>.Enumerator enumerator = this.components.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.GetType() == type)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000862 RID: 2146 RVA: 0x000277AC File Offset: 0x000259AC
		public bool HasSubclassOf(Type type)
		{
			using (List<VolumeComponent>.Enumerator enumerator = this.components.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.GetType().IsSubclassOf(type))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000863 RID: 2147 RVA: 0x0002780C File Offset: 0x00025A0C
		public bool TryGet<T>(out T component) where T : VolumeComponent
		{
			return this.TryGet<T>(typeof(T), out component);
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x00027820 File Offset: 0x00025A20
		public bool TryGet<T>(Type type, out T component) where T : VolumeComponent
		{
			component = default(T);
			foreach (VolumeComponent volumeComponent in this.components)
			{
				if (volumeComponent.GetType() == type)
				{
					component = (T)((object)volumeComponent);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000865 RID: 2149 RVA: 0x00027894 File Offset: 0x00025A94
		public bool TryGetSubclassOf<T>(Type type, out T component) where T : VolumeComponent
		{
			component = default(T);
			foreach (VolumeComponent volumeComponent in this.components)
			{
				if (volumeComponent.GetType().IsSubclassOf(type))
				{
					component = (T)((object)volumeComponent);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000866 RID: 2150 RVA: 0x00027908 File Offset: 0x00025B08
		public bool TryGetAllSubclassOf<T>(Type type, List<T> result) where T : VolumeComponent
		{
			int count = result.Count;
			foreach (VolumeComponent volumeComponent in this.components)
			{
				if (volumeComponent.GetType().IsSubclassOf(type))
				{
					result.Add((T)((object)volumeComponent));
				}
			}
			return count != result.Count;
		}

		// Token: 0x06000867 RID: 2151 RVA: 0x00027984 File Offset: 0x00025B84
		public override int GetHashCode()
		{
			int num = 17;
			for (int i = 0; i < this.components.Count; i++)
			{
				num = num * 23 + this.components[i].GetHashCode();
			}
			return num;
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x000279C4 File Offset: 0x00025BC4
		internal int GetComponentListHashCode()
		{
			int num = 17;
			for (int i = 0; i < this.components.Count; i++)
			{
				num = num * 23 + this.components[i].GetType().GetHashCode();
			}
			return num;
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x00027A08 File Offset: 0x00025C08
		internal void Sanitize()
		{
			for (int i = this.components.Count - 1; i >= 0; i--)
			{
				if (this.components[i] == null)
				{
					this.components.RemoveAt(i);
				}
			}
		}

		// Token: 0x040004F2 RID: 1266
		public List<VolumeComponent> components = new List<VolumeComponent>();

		// Token: 0x040004F3 RID: 1267
		[NonSerialized]
		public bool isDirty = true;
	}
}
