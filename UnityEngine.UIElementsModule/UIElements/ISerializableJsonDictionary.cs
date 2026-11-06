using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200026E RID: 622
	internal interface ISerializableJsonDictionary
	{
		// Token: 0x060011B1 RID: 4529
		void Set<T>(string key, T value) where T : class;

		// Token: 0x060011B2 RID: 4530
		T Get<T>(string key) where T : class;

		// Token: 0x060011B3 RID: 4531
		T GetScriptable<T>(string key) where T : ScriptableObject;

		// Token: 0x060011B4 RID: 4532
		void Overwrite(object obj, string key);

		// Token: 0x060011B5 RID: 4533
		bool ContainsKey(string key);

		// Token: 0x060011B6 RID: 4534
		void OnBeforeSerialize();

		// Token: 0x060011B7 RID: 4535
		void OnAfterDeserialize();
	}
}
