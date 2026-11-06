using System;
using System.Collections.Generic;

namespace UnityEngine.Localization.Pseudo
{
	// Token: 0x020000B4 RID: 180
	public static class TypicalCharacterSets
	{
		// Token: 0x060004FC RID: 1276 RVA: 0x00011F9C File Offset: 0x0001019C
		public static char[] GetTypicalCharactersForLanguage(SystemLanguage language)
		{
			char[] array;
			if (TypicalCharacterSets.s_TypicalCharacterSets.TryGetValue(language, out array))
			{
				return array;
			}
			return null;
		}

		// Token: 0x04000173 RID: 371
		internal static readonly Dictionary<SystemLanguage, char[]> s_TypicalCharacterSets = new Dictionary<SystemLanguage, char[]>
		{
			{
				SystemLanguage.Czech,
				"áčďéěíňóřšťúůýÁČĎÉĚÍŇÓŘŠŤÚŮÝ‚„".ToCharArray()
			},
			{
				SystemLanguage.Danish,
				"åæøÅÆØ".ToCharArray()
			},
			{
				SystemLanguage.Dutch,
				"àáèéêëïóöÀÁÈÉÊËÏÓÖ".ToCharArray()
			},
			{
				SystemLanguage.Finnish,
				"åäöšÅÄÖŠ".ToCharArray()
			},
			{
				SystemLanguage.French,
				"àâæéèêëîïôœùûüçÀÂÆÉÈÊËÎÏÔŒÙÛÜÇ".ToCharArray()
			},
			{
				SystemLanguage.German,
				"ÄÖÜẞäöüß‚„".ToCharArray()
			},
			{
				SystemLanguage.Italian,
				"àéèìòùÀÉÈÌÒÙªº".ToCharArray()
			},
			{
				SystemLanguage.Norwegian,
				"åæøÅÆØ".ToCharArray()
			},
			{
				SystemLanguage.Polish,
				"ąćęłńóśżźĄĆĘŁŃÓŚŻŹ‚„".ToCharArray()
			},
			{
				SystemLanguage.Portuguese,
				"àáâãçéêíóôõúüÀÁÂÃÇÉÊÍÓÔÕÚÜ".ToCharArray()
			},
			{
				SystemLanguage.Russian,
				"абвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ№".ToCharArray()
			},
			{
				SystemLanguage.Spanish,
				"áéíóúüñÁÉÍÓÚÜÑ¿¡ªº".ToCharArray()
			},
			{
				SystemLanguage.Swedish,
				"åäöÅÄÖ".ToCharArray()
			}
		};
	}
}
