using System;

namespace UnityEngine.Localization.SmartFormat.PersistentVariables
{
	// Token: 0x0200005E RID: 94
	[Serializable]
	internal class VariableNameValuePair
	{
		// Token: 0x060002B0 RID: 688 RVA: 0x0000A2AF File Offset: 0x000084AF
		public override string ToString()
		{
			string text = this.name;
			string text2 = " - ";
			IVariable variable = this.variable;
			return text + text2 + ((variable != null) ? variable.GetType().Name : null);
		}

		// Token: 0x040000B9 RID: 185
		public string name;

		// Token: 0x040000BA RID: 186
		[SerializeReference]
		public IVariable variable;
	}
}
