using System;
using System.IO;

namespace MoreMountains.Tools
{
	// Token: 0x02000142 RID: 322
	public interface IMMSaveLoadManagerMethod
	{
		// Token: 0x06000730 RID: 1840
		void Save(object objectToSave, FileStream saveFile);

		// Token: 0x06000731 RID: 1841
		object Load(Type objectType, FileStream saveFile);
	}
}
