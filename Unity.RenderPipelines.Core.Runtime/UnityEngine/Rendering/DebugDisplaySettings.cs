using System;
using System.Collections.Generic;

namespace UnityEngine.Rendering
{
	// Token: 0x0200005B RID: 91
	public abstract class DebugDisplaySettings<T> : IDebugDisplaySettings, IDebugDisplaySettingsQuery where T : IDebugDisplaySettings, new()
	{
		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060002E4 RID: 740 RVA: 0x0000CBAA File Offset: 0x0000ADAA
		public static T Instance
		{
			get
			{
				return DebugDisplaySettings<T>.s_Instance.Value;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060002E5 RID: 741 RVA: 0x0000CBB8 File Offset: 0x0000ADB8
		public virtual bool AreAnySettingsActive
		{
			get
			{
				using (HashSet<IDebugDisplaySettingsData>.Enumerator enumerator = this.m_Settings.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.AreAnySettingsActive)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060002E6 RID: 742 RVA: 0x0000CC14 File Offset: 0x0000AE14
		public virtual bool IsPostProcessingAllowed
		{
			get
			{
				bool flag = true;
				foreach (IDebugDisplaySettingsData debugDisplaySettingsData in this.m_Settings)
				{
					flag &= debugDisplaySettingsData.IsPostProcessingAllowed;
				}
				return flag;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060002E7 RID: 743 RVA: 0x0000CC6C File Offset: 0x0000AE6C
		public virtual bool IsLightingActive
		{
			get
			{
				bool flag = true;
				foreach (IDebugDisplaySettingsData debugDisplaySettingsData in this.m_Settings)
				{
					flag &= debugDisplaySettingsData.IsLightingActive;
				}
				return flag;
			}
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000CCC4 File Offset: 0x0000AEC4
		protected TData Add<TData>(TData newData) where TData : IDebugDisplaySettingsData
		{
			this.m_Settings.Add(newData);
			return newData;
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0000CCDC File Offset: 0x0000AEDC
		public void ForEach(Action<IDebugDisplaySettingsData> onExecute)
		{
			foreach (IDebugDisplaySettingsData debugDisplaySettingsData in this.m_Settings)
			{
				onExecute(debugDisplaySettingsData);
			}
		}

		// Token: 0x060002EA RID: 746 RVA: 0x0000CD30 File Offset: 0x0000AF30
		public virtual void Reset()
		{
			this.m_Settings.Clear();
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0000CD40 File Offset: 0x0000AF40
		public virtual bool TryGetScreenClearColor(ref Color color)
		{
			using (HashSet<IDebugDisplaySettingsData>.Enumerator enumerator = this.m_Settings.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.TryGetScreenClearColor(ref color))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x040001A3 RID: 419
		protected readonly HashSet<IDebugDisplaySettingsData> m_Settings = new HashSet<IDebugDisplaySettingsData>();

		// Token: 0x040001A4 RID: 420
		private static readonly Lazy<T> s_Instance = new Lazy<T>(delegate
		{
			T t = new T();
			t.Reset();
			return t;
		});
	}
}
