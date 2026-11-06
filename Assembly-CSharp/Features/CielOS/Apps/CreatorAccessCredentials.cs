using System;
using System.Security.Cryptography;

namespace Features.CielOS.Apps
{
	// Token: 0x0200141C RID: 5148
	public static class CreatorAccessCredentials
	{
		// Token: 0x06009EDE RID: 40670 RVA: 0x00416333 File Offset: 0x00414533
		public static bool Verify(string candidate)
		{
			return !string.IsNullOrEmpty(candidate) && candidate.Length <= 128 && CreatorAccessCredentials.VerifyDerivedKey(candidate, CreatorAccessCredentials.Salt, CreatorAccessCredentials.Verifier, 100000);
		}

		// Token: 0x06009EDF RID: 40671 RVA: 0x00416364 File Offset: 0x00414564
		private static bool VerifyDerivedKey(string candidate, byte[] salt, byte[] expected, int iterations)
		{
			byte[] array = null;
			bool flag;
			try
			{
				using (Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(candidate, salt, iterations, HashAlgorithmName.SHA256))
				{
					array = rfc2898DeriveBytes.GetBytes(32);
				}
				if (expected.Length != array.Length)
				{
					flag = false;
				}
				else
				{
					int num = 0;
					for (int i = 0; i < array.Length; i++)
					{
						num |= (int)(array[i] ^ expected[i]);
					}
					flag = num == 0;
				}
			}
			finally
			{
				if (array != null)
				{
					Array.Clear(array, 0, array.Length);
				}
			}
			return flag;
		}

		// Token: 0x0400A302 RID: 41730
		private const int MaximumCredentialLength = 128;

		// Token: 0x0400A303 RID: 41731
		private const int Iterations = 100000;

		// Token: 0x0400A304 RID: 41732
		private const int DerivedKeyLength = 32;

		// Token: 0x0400A305 RID: 41733
		private static readonly byte[] Salt = Convert.FromBase64String("89CxuktYDHIOLGuVzfACx4u93+KeGama0zFHX99muL0=");

		// Token: 0x0400A306 RID: 41734
		private static readonly byte[] Verifier = Convert.FromBase64String("phaUvbNiT9LjiAcXj9HYiCKxxDG5tb07S31bUTNi4p4=");
	}
}
