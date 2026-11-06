using System;
using System.Runtime.Remoting.Activation;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x02000614 RID: 1556
	internal class ConstructionCallDictionary : MessageDictionary
	{
		// Token: 0x06003ACC RID: 15052 RVA: 0x000CE0F1 File Offset: 0x000CC2F1
		public ConstructionCallDictionary(IConstructionCallMessage message)
			: base(message)
		{
			base.MethodKeys = ConstructionCallDictionary.InternalKeys;
		}

		// Token: 0x06003ACD RID: 15053 RVA: 0x000CE108 File Offset: 0x000CC308
		protected override object GetMethodProperty(string key)
		{
			if (key == "__Activator")
			{
				return ((IConstructionCallMessage)this._message).Activator;
			}
			if (key == "__CallSiteActivationAttributes")
			{
				return ((IConstructionCallMessage)this._message).CallSiteActivationAttributes;
			}
			if (key == "__ActivationType")
			{
				return ((IConstructionCallMessage)this._message).ActivationType;
			}
			if (key == "__ContextProperties")
			{
				return ((IConstructionCallMessage)this._message).ContextProperties;
			}
			if (!(key == "__ActivationTypeName"))
			{
				return base.GetMethodProperty(key);
			}
			return ((IConstructionCallMessage)this._message).ActivationTypeName;
		}

		// Token: 0x06003ACE RID: 15054 RVA: 0x000CE1B4 File Offset: 0x000CC3B4
		protected override void SetMethodProperty(string key, object value)
		{
			if (key == "__Activator")
			{
				((IConstructionCallMessage)this._message).Activator = (IActivator)value;
				return;
			}
			if (!(key == "__CallSiteActivationAttributes") && !(key == "__ActivationType") && !(key == "__ContextProperties") && !(key == "__ActivationTypeName"))
			{
				base.SetMethodProperty(key, value);
				return;
			}
			throw new ArgumentException("key was invalid");
		}

		// Token: 0x0400268B RID: 9867
		public static string[] InternalKeys = new string[]
		{
			"__Uri", "__MethodName", "__TypeName", "__MethodSignature", "__Args", "__CallContext", "__CallSiteActivationAttributes", "__ActivationType", "__ContextProperties", "__Activator",
			"__ActivationTypeName"
		};
	}
}
