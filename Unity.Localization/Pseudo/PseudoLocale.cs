using System;
using System.Collections.Generic;

namespace UnityEngine.Localization.Pseudo
{
	// Token: 0x020000B3 RID: 179
	[CreateAssetMenu(menuName = "Localization/Pseudo-Locale", fileName = "Pseudo-Locale(pseudo)")]
	public class PseudoLocale : Locale
	{
		// Token: 0x17000125 RID: 293
		// (get) Token: 0x060004F6 RID: 1270 RVA: 0x00011E44 File Offset: 0x00010044
		public List<IPseudoLocalizationMethod> Methods
		{
			get
			{
				return this.m_Methods;
			}
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x00011E4C File Offset: 0x0001004C
		public static PseudoLocale CreatePseudoLocale()
		{
			PseudoLocale pseudoLocale = ScriptableObject.CreateInstance<PseudoLocale>();
			pseudoLocale.name = "PseudoLocale";
			return pseudoLocale;
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x00011E60 File Offset: 0x00010060
		private PseudoLocale()
		{
			base.Identifier = new LocaleIdentifier("en");
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x00011EBC File Offset: 0x000100BC
		internal void Reset()
		{
			foreach (IPseudoLocalizationMethod pseudoLocalizationMethod in this.Methods)
			{
				CharacterSubstitutor characterSubstitutor = pseudoLocalizationMethod as CharacterSubstitutor;
				if (characterSubstitutor != null)
				{
					characterSubstitutor.m_ReplacementsPosition = 0;
				}
			}
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x00011F18 File Offset: 0x00010118
		public virtual string GetPseudoString(string input)
		{
			Message message = Message.CreateMessage(input);
			foreach (IPseudoLocalizationMethod pseudoLocalizationMethod in this.Methods)
			{
				if (pseudoLocalizationMethod != null)
				{
					pseudoLocalizationMethod.Transform(message);
				}
			}
			string text = message.ToString();
			message.Release();
			return text;
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x00011F84 File Offset: 0x00010184
		public override string ToString()
		{
			return "Pseudo (" + base.ToString() + ")";
		}

		// Token: 0x04000172 RID: 370
		[SerializeReference]
		private List<IPseudoLocalizationMethod> m_Methods = new List<IPseudoLocalizationMethod>
		{
			new PreserveTags(),
			new Expander(),
			new Accenter(),
			new Encapsulator()
		};
	}
}
