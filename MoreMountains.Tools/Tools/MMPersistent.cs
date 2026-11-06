using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200013A RID: 314
	public class MMPersistent : MMPersistentBase
	{
		// Token: 0x0600070A RID: 1802 RVA: 0x00022618 File Offset: 0x00020818
		public override string OnSave()
		{
			List<MMPersistent.ComponentData> list = null;
			if (this.SaveEnabledStates)
			{
				list = this.GetCurrentComponents();
			}
			return JsonUtility.ToJson(new MMPersistent.Data
			{
				Position = base.transform.position,
				LocalRotation = base.transform.localRotation,
				LocalScale = base.transform.localScale,
				ActiveState = base.gameObject.activeSelf,
				ComponentEnabledStates = list
			});
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x0002269C File Offset: 0x0002089C
		public override void OnLoad(string data)
		{
			if (this.SavePosition)
			{
				base.transform.position = JsonUtility.FromJson<MMPersistent.Data>(data).Position;
			}
			if (this.SaveLocalRotation)
			{
				base.transform.localRotation = JsonUtility.FromJson<MMPersistent.Data>(data).LocalRotation;
			}
			if (this.SaveLocalScale)
			{
				base.transform.localScale = JsonUtility.FromJson<MMPersistent.Data>(data).LocalScale;
			}
			if (this.SaveActiveState)
			{
				base.gameObject.SetActive(JsonUtility.FromJson<MMPersistent.Data>(data).ActiveState);
			}
			if (this.SaveEnabledStates)
			{
				List<MMPersistent.ComponentData> componentEnabledStates = JsonUtility.FromJson<MMPersistent.Data>(data).ComponentEnabledStates;
				Behaviour[] components = base.gameObject.GetComponents<Behaviour>();
				Renderer[] components2 = base.gameObject.GetComponents<Renderer>();
				if (componentEnabledStates.Count != components.Length + components2.Length)
				{
					return;
				}
				int num = 0;
				for (int i = 0; i < components.Length; i++)
				{
					if (componentEnabledStates[i].Name == components[i].name)
					{
						components[i].enabled = componentEnabledStates[i].EnabledState;
					}
					num++;
				}
				for (int j = 0; j < components2.Length; j++)
				{
					if (componentEnabledStates[num + j].Name == components2[j].name)
					{
						components2[j].enabled = componentEnabledStates[num + j].EnabledState;
					}
				}
			}
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x000227F4 File Offset: 0x000209F4
		protected virtual List<MMPersistent.ComponentData> GetCurrentComponents()
		{
			List<MMPersistent.ComponentData> list = new List<MMPersistent.ComponentData>();
			Behaviour[] components = base.gameObject.GetComponents<Behaviour>();
			Renderer[] components2 = base.gameObject.GetComponents<Renderer>();
			foreach (Behaviour behaviour in components)
			{
				list.Add(new MMPersistent.ComponentData
				{
					Name = behaviour.name,
					EnabledState = behaviour.enabled
				});
			}
			foreach (Renderer renderer in components2)
			{
				list.Add(new MMPersistent.ComponentData
				{
					Name = renderer.name,
					EnabledState = renderer.enabled
				});
			}
			return list;
		}

		// Token: 0x040008A6 RID: 2214
		[Header("Properties")]
		[Tooltip("whether or not to save this object's position")]
		public bool SavePosition = true;

		// Token: 0x040008A7 RID: 2215
		[Tooltip("whether or not to save this object's rotation")]
		public bool SaveLocalRotation = true;

		// Token: 0x040008A8 RID: 2216
		[Tooltip("whether or not to save this object's scale")]
		public bool SaveLocalScale = true;

		// Token: 0x040008A9 RID: 2217
		[Tooltip("whether or not to save this object's active state")]
		public bool SaveActiveState = true;

		// Token: 0x040008AA RID: 2218
		[Tooltip("whether or not to save this object's components' enabled states")]
		public bool SaveEnabledStates;

		// Token: 0x02000445 RID: 1093
		[Serializable]
		public struct Data
		{
			// Token: 0x04001FD8 RID: 8152
			public Vector3 Position;

			// Token: 0x04001FD9 RID: 8153
			public Quaternion LocalRotation;

			// Token: 0x04001FDA RID: 8154
			public Vector3 LocalScale;

			// Token: 0x04001FDB RID: 8155
			public bool ActiveState;

			// Token: 0x04001FDC RID: 8156
			public List<MMPersistent.ComponentData> ComponentEnabledStates;
		}

		// Token: 0x02000446 RID: 1094
		[Serializable]
		public struct ComponentData
		{
			// Token: 0x04001FDD RID: 8157
			public string Name;

			// Token: 0x04001FDE RID: 8158
			public bool EnabledState;
		}
	}
}
