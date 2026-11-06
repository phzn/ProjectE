using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;

namespace System.Text
{
	// Token: 0x020003C3 RID: 963
	internal static class EncodingHelper
	{
		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x06002841 RID: 10305 RVA: 0x00092660 File Offset: 0x00090860
		internal static Encoding UTF8Unmarked
		{
			get
			{
				if (EncodingHelper.utf8EncodingWithoutMarkers == null)
				{
					object obj = EncodingHelper.lockobj;
					lock (obj)
					{
						if (EncodingHelper.utf8EncodingWithoutMarkers == null)
						{
							EncodingHelper.utf8EncodingWithoutMarkers = new UTF8Encoding(false, false);
							EncodingHelper.utf8EncodingWithoutMarkers.setReadOnly(true);
						}
					}
				}
				return EncodingHelper.utf8EncodingWithoutMarkers;
			}
		}

		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x06002842 RID: 10306 RVA: 0x000926D0 File Offset: 0x000908D0
		internal static Encoding UTF8UnmarkedUnsafe
		{
			get
			{
				if (EncodingHelper.utf8EncodingUnsafe == null)
				{
					object obj = EncodingHelper.lockobj;
					lock (obj)
					{
						if (EncodingHelper.utf8EncodingUnsafe == null)
						{
							EncodingHelper.utf8EncodingUnsafe = new UTF8Encoding(false, false);
							EncodingHelper.utf8EncodingUnsafe.setReadOnly(false);
							EncodingHelper.utf8EncodingUnsafe.DecoderFallback = new DecoderReplacementFallback(string.Empty);
							EncodingHelper.utf8EncodingUnsafe.setReadOnly(true);
						}
					}
				}
				return EncodingHelper.utf8EncodingUnsafe;
			}
		}

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x06002843 RID: 10307 RVA: 0x00092760 File Offset: 0x00090960
		internal static Encoding BigEndianUTF32
		{
			get
			{
				if (EncodingHelper.bigEndianUTF32Encoding == null)
				{
					object obj = EncodingHelper.lockobj;
					lock (obj)
					{
						if (EncodingHelper.bigEndianUTF32Encoding == null)
						{
							EncodingHelper.bigEndianUTF32Encoding = new UTF32Encoding(true, true);
							EncodingHelper.bigEndianUTF32Encoding.setReadOnly(true);
						}
					}
				}
				return EncodingHelper.bigEndianUTF32Encoding;
			}
		}

		// Token: 0x06002844 RID: 10308
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string InternalCodePage(ref int code_page);

		// Token: 0x06002845 RID: 10309 RVA: 0x000927D0 File Offset: 0x000909D0
		internal static Encoding GetDefaultEncoding()
		{
			Encoding encoding = null;
			int num = 1;
			string text = EncodingHelper.InternalCodePage(ref num);
			try
			{
				if (num == -1)
				{
					encoding = Encoding.GetEncoding(text);
				}
				else
				{
					num &= 268435455;
					switch (num)
					{
					case 1:
						num = 20127;
						break;
					case 2:
						num = 65007;
						break;
					case 3:
						num = 65001;
						break;
					case 4:
						num = 1200;
						break;
					case 5:
						num = 1201;
						break;
					case 6:
						num = 1252;
						break;
					}
					encoding = Encoding.GetEncoding(num);
				}
			}
			catch (NotSupportedException)
			{
				encoding = EncodingHelper.UTF8Unmarked;
			}
			catch (ArgumentException)
			{
				encoding = EncodingHelper.UTF8Unmarked;
			}
			return encoding;
		}

		// Token: 0x06002846 RID: 10310 RVA: 0x00092888 File Offset: 0x00090A88
		internal static object InvokeI18N(string name, params object[] args)
		{
			object obj = EncodingHelper.lockobj;
			object obj2;
			lock (obj)
			{
				if (EncodingHelper.i18nDisabled)
				{
					obj2 = null;
				}
				else
				{
					if (EncodingHelper.i18nAssembly == null)
					{
						try
						{
							try
							{
								EncodingHelper.i18nAssembly = Assembly.Load("I18N, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0738eb9f132ed756");
							}
							catch (NotImplementedException)
							{
								EncodingHelper.i18nDisabled = true;
								return null;
							}
							if (EncodingHelper.i18nAssembly == null)
							{
								return null;
							}
						}
						catch (SystemException)
						{
							return null;
						}
					}
					Type type;
					try
					{
						type = EncodingHelper.i18nAssembly.GetType("I18N.Common.Manager");
					}
					catch (NotImplementedException)
					{
						EncodingHelper.i18nDisabled = true;
						return null;
					}
					if (type == null)
					{
						obj2 = null;
					}
					else
					{
						object obj3;
						try
						{
							obj3 = type.InvokeMember("PrimaryManager", BindingFlags.Static | BindingFlags.Public | BindingFlags.GetProperty, null, null, null, null, null, null);
							if (obj3 == null)
							{
								return null;
							}
						}
						catch (MissingMethodException)
						{
							return null;
						}
						catch (SecurityException)
						{
							return null;
						}
						catch (NotImplementedException)
						{
							EncodingHelper.i18nDisabled = true;
							return null;
						}
						try
						{
							obj2 = type.InvokeMember(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod, null, obj3, args, null, null, null);
						}
						catch (MissingMethodException)
						{
							obj2 = null;
						}
						catch (SecurityException)
						{
							obj2 = null;
						}
					}
				}
			}
			return obj2;
		}

		// Token: 0x04001E88 RID: 7816
		private static volatile Encoding utf8EncodingWithoutMarkers;

		// Token: 0x04001E89 RID: 7817
		private static volatile Encoding utf8EncodingUnsafe;

		// Token: 0x04001E8A RID: 7818
		private static volatile Encoding bigEndianUTF32Encoding;

		// Token: 0x04001E8B RID: 7819
		private static readonly object lockobj = new object();

		// Token: 0x04001E8C RID: 7820
		private static Assembly i18nAssembly;

		// Token: 0x04001E8D RID: 7821
		private static bool i18nDisabled;
	}
}
