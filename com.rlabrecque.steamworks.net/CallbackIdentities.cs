using System;

namespace Steamworks
{
	// Token: 0x02000177 RID: 375
	internal class CallbackIdentities
	{
		// Token: 0x060008C2 RID: 2242 RVA: 0x0000D040 File Offset: 0x0000B240
		public static int GetCallbackIdentity(Type callbackStruct)
		{
			object[] customAttributes = callbackStruct.GetCustomAttributes(typeof(CallbackIdentityAttribute), false);
			int num = 0;
			if (num >= customAttributes.Length)
			{
				throw new Exception("Callback number not found for struct " + ((callbackStruct != null) ? callbackStruct.ToString() : null));
			}
			return ((CallbackIdentityAttribute)customAttributes[num]).Identity;
		}
	}
}
