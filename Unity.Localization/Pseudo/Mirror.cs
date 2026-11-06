using System;

namespace UnityEngine.Localization.Pseudo
{
	// Token: 0x020000B1 RID: 177
	[Serializable]
	public class Mirror : IPseudoLocalizationMethod
	{
		// Token: 0x060004ED RID: 1261 RVA: 0x00011BD8 File Offset: 0x0000FDD8
		public void Transform(Message message)
		{
			foreach (MessageFragment messageFragment in message.Fragments)
			{
				WritableMessageFragment writableMessageFragment = messageFragment as WritableMessageFragment;
				if (writableMessageFragment != null)
				{
					this.MirrorFragment(writableMessageFragment);
				}
			}
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x00011C34 File Offset: 0x0000FE34
		private void MirrorFragment(WritableMessageFragment writableMessageFragment)
		{
			char[] array = new char[writableMessageFragment.Length];
			int i = writableMessageFragment.Length - 1;
			int num;
			for (int j = writableMessageFragment.Length - 1; j >= 0; j--)
			{
				if (writableMessageFragment[j] == '\n')
				{
					array[j] = '\n';
					num = j + 1;
					while (i > j)
					{
						array[num++] = writableMessageFragment[i--];
					}
					i = j - 1;
				}
			}
			num = 0;
			while (i >= 0)
			{
				array[num++] = writableMessageFragment[i--];
			}
			writableMessageFragment.Text = new string(array);
		}
	}
}
