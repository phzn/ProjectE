using System;
using System.Collections.Generic;

namespace UnityEngine.Timeline
{
	// Token: 0x0200004D RID: 77
	public interface IPropertyCollector
	{
		// Token: 0x060002D7 RID: 727
		void PushActiveGameObject(GameObject gameObject);

		// Token: 0x060002D8 RID: 728
		void PopActiveGameObject();

		// Token: 0x060002D9 RID: 729
		void AddFromClip(AnimationClip clip);

		// Token: 0x060002DA RID: 730
		void AddFromClips(IEnumerable<AnimationClip> clips);

		// Token: 0x060002DB RID: 731
		void AddFromName<T>(string name) where T : Component;

		// Token: 0x060002DC RID: 732
		void AddFromName(string name);

		// Token: 0x060002DD RID: 733
		void AddFromClip(GameObject obj, AnimationClip clip);

		// Token: 0x060002DE RID: 734
		void AddFromClips(GameObject obj, IEnumerable<AnimationClip> clips);

		// Token: 0x060002DF RID: 735
		void AddFromName<T>(GameObject obj, string name) where T : Component;

		// Token: 0x060002E0 RID: 736
		void AddFromName(GameObject obj, string name);

		// Token: 0x060002E1 RID: 737
		void AddFromName(Component component, string name);

		// Token: 0x060002E2 RID: 738
		void AddFromComponent(GameObject obj, Component component);

		// Token: 0x060002E3 RID: 739
		void AddObjectProperties(Object obj, AnimationClip clip);
	}
}
