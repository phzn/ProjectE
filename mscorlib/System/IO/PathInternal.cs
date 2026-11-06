using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.IO
{
	// Token: 0x02000B0D RID: 2829
	internal static class PathInternal
	{
		// Token: 0x0600650F RID: 25871 RVA: 0x00157B12 File Offset: 0x00155D12
		internal static bool IsValidDriveChar(char value)
		{
			return (value >= 'A' && value <= 'Z') || (value >= 'a' && value <= 'z');
		}

		// Token: 0x06006510 RID: 25872 RVA: 0x00157B30 File Offset: 0x00155D30
		internal static bool EndsWithPeriodOrSpace(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return false;
			}
			char c = path[path.Length - 1];
			return c == ' ' || c == '.';
		}

		// Token: 0x06006511 RID: 25873 RVA: 0x00157B62 File Offset: 0x00155D62
		internal static string EnsureExtendedPrefixIfNeeded(string path)
		{
			if (path != null && (path.Length >= 260 || PathInternal.EndsWithPeriodOrSpace(path)))
			{
				return PathInternal.EnsureExtendedPrefix(path);
			}
			return path;
		}

		// Token: 0x06006512 RID: 25874 RVA: 0x00157B84 File Offset: 0x00155D84
		internal static string EnsureExtendedPrefixOverMaxPath(string path)
		{
			if (path != null && path.Length >= 260)
			{
				return PathInternal.EnsureExtendedPrefix(path);
			}
			return path;
		}

		// Token: 0x06006513 RID: 25875 RVA: 0x00157B9E File Offset: 0x00155D9E
		internal static string EnsureExtendedPrefix(string path)
		{
			if (PathInternal.IsPartiallyQualified(path) || PathInternal.IsDevice(path))
			{
				return path;
			}
			if (path.StartsWith("\\\\", StringComparison.OrdinalIgnoreCase))
			{
				return path.Insert(2, "?\\UNC\\");
			}
			return "\\\\?\\" + path;
		}

		// Token: 0x06006514 RID: 25876 RVA: 0x00157BE0 File Offset: 0x00155DE0
		internal unsafe static bool IsDevice(ReadOnlySpan<char> path)
		{
			return PathInternal.IsExtended(path) || (path.Length >= 4 && PathInternal.IsDirectorySeparator((char)(*path[0])) && PathInternal.IsDirectorySeparator((char)(*path[1])) && (*path[2] == 46 || *path[2] == 63) && PathInternal.IsDirectorySeparator((char)(*path[3])));
		}

		// Token: 0x06006515 RID: 25877 RVA: 0x00157C4C File Offset: 0x00155E4C
		internal unsafe static bool IsDeviceUNC(ReadOnlySpan<char> path)
		{
			return path.Length >= 8 && PathInternal.IsDevice(path) && PathInternal.IsDirectorySeparator((char)(*path[7])) && *path[4] == 85 && *path[5] == 78 && *path[6] == 67;
		}

		// Token: 0x06006516 RID: 25878 RVA: 0x00157CA4 File Offset: 0x00155EA4
		internal unsafe static bool IsExtended(ReadOnlySpan<char> path)
		{
			return path.Length >= 4 && *path[0] == 92 && (*path[1] == 92 || *path[1] == 63) && *path[2] == 63 && *path[3] == 92;
		}

		// Token: 0x06006517 RID: 25879 RVA: 0x00157D00 File Offset: 0x00155F00
		internal unsafe static bool HasWildCardCharacters(ReadOnlySpan<char> path)
		{
			for (int i = (PathInternal.IsDevice(path) ? "\\\\?\\".Length : 0); i < path.Length; i++)
			{
				char c = (char)(*path[i]);
				if (c <= '?' && (c == '"' || c == '<' || c == '>' || c == '*' || c == '?'))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006518 RID: 25880 RVA: 0x00157D60 File Offset: 0x00155F60
		internal unsafe static int GetRootLength(ReadOnlySpan<char> path)
		{
			int length = path.Length;
			int i = 0;
			bool flag = PathInternal.IsDevice(path);
			bool flag2 = flag && PathInternal.IsDeviceUNC(path);
			if ((!flag || flag2) && length > 0 && PathInternal.IsDirectorySeparator((char)(*path[0])))
			{
				if (flag2 || (length > 1 && PathInternal.IsDirectorySeparator((char)(*path[1]))))
				{
					i = (flag2 ? 8 : 2);
					int num = 2;
					while (i < length)
					{
						if (PathInternal.IsDirectorySeparator((char)(*path[i])) && --num <= 0)
						{
							break;
						}
						i++;
					}
				}
				else
				{
					i = 1;
				}
			}
			else if (flag)
			{
				i = 4;
				while (i < length && !PathInternal.IsDirectorySeparator((char)(*path[i])))
				{
					i++;
				}
				if (i < length && i > 4 && PathInternal.IsDirectorySeparator((char)(*path[i])))
				{
					i++;
				}
			}
			else if (length >= 2 && *path[1] == 58 && PathInternal.IsValidDriveChar((char)(*path[0])))
			{
				i = 2;
				if (length > 2 && PathInternal.IsDirectorySeparator((char)(*path[2])))
				{
					i++;
				}
			}
			return i;
		}

		// Token: 0x06006519 RID: 25881 RVA: 0x00157E70 File Offset: 0x00156070
		internal unsafe static bool IsPartiallyQualified(ReadOnlySpan<char> path)
		{
			if (path.Length < 2)
			{
				return true;
			}
			if (PathInternal.IsDirectorySeparator((char)(*path[0])))
			{
				return *path[1] != 63 && !PathInternal.IsDirectorySeparator((char)(*path[1]));
			}
			return path.Length < 3 || *path[1] != 58 || !PathInternal.IsDirectorySeparator((char)(*path[2])) || !PathInternal.IsValidDriveChar((char)(*path[0]));
		}

		// Token: 0x0600651A RID: 25882 RVA: 0x00157EF4 File Offset: 0x001560F4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static bool IsDirectorySeparator(char c)
		{
			return c == '\\' || c == '/';
		}

		// Token: 0x0600651B RID: 25883 RVA: 0x00157F04 File Offset: 0x00156104
		internal static string NormalizeDirectorySeparators(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return path;
			}
			bool flag = true;
			for (int i = 0; i < path.Length; i++)
			{
				char c = path[i];
				if (PathInternal.IsDirectorySeparator(c) && (c != '\\' || (i > 0 && i + 1 < path.Length && PathInternal.IsDirectorySeparator(path[i + 1]))))
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				return path;
			}
			StringBuilder stringBuilder = new StringBuilder(path.Length);
			int num = 0;
			if (PathInternal.IsDirectorySeparator(path[num]))
			{
				num++;
				stringBuilder.Append('\\');
			}
			int j = num;
			while (j < path.Length)
			{
				char c = path[j];
				if (!PathInternal.IsDirectorySeparator(c))
				{
					goto IL_00C1;
				}
				if (j + 1 >= path.Length || !PathInternal.IsDirectorySeparator(path[j + 1]))
				{
					c = '\\';
					goto IL_00C1;
				}
				IL_00C9:
				j++;
				continue;
				IL_00C1:
				stringBuilder.Append(c);
				goto IL_00C9;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600651C RID: 25884 RVA: 0x00157FF0 File Offset: 0x001561F0
		internal unsafe static bool IsEffectivelyEmpty(ReadOnlySpan<char> path)
		{
			if (path.IsEmpty)
			{
				return true;
			}
			ReadOnlySpan<char> readOnlySpan = path;
			for (int i = 0; i < readOnlySpan.Length; i++)
			{
				if (*readOnlySpan[i] != 32)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600651D RID: 25885 RVA: 0x0015802C File Offset: 0x0015622C
		internal unsafe static bool EndsInDirectorySeparator(ReadOnlySpan<char> path)
		{
			return path.Length > 0 && PathInternal.IsDirectorySeparator((char)(*path[path.Length - 1]));
		}

		// Token: 0x0600651E RID: 25886 RVA: 0x00158050 File Offset: 0x00156250
		internal unsafe static bool StartsWithDirectorySeparator(ReadOnlySpan<char> path)
		{
			return path.Length > 0 && PathInternal.IsDirectorySeparator((char)(*path[0]));
		}

		// Token: 0x0600651F RID: 25887 RVA: 0x0015806C File Offset: 0x0015626C
		internal static string EnsureTrailingSeparator(string path)
		{
			if (!PathInternal.EndsInDirectorySeparator(path))
			{
				return path + "\\";
			}
			return path;
		}

		// Token: 0x06006520 RID: 25888 RVA: 0x00158088 File Offset: 0x00156288
		internal static string TrimEndingDirectorySeparator(string path)
		{
			if (!PathInternal.EndsInDirectorySeparator(path) || PathInternal.IsRoot(path))
			{
				return path;
			}
			return path.Substring(0, path.Length - 1);
		}

		// Token: 0x06006521 RID: 25889 RVA: 0x001580B5 File Offset: 0x001562B5
		internal static ReadOnlySpan<char> TrimEndingDirectorySeparator(ReadOnlySpan<char> path)
		{
			if (!PathInternal.EndsInDirectorySeparator(path) || PathInternal.IsRoot(path))
			{
				return path;
			}
			return path.Slice(0, path.Length - 1);
		}

		// Token: 0x06006522 RID: 25890 RVA: 0x001580DA File Offset: 0x001562DA
		internal static bool IsRoot(ReadOnlySpan<char> path)
		{
			return path.Length == PathInternal.GetRootLength(path);
		}

		// Token: 0x06006523 RID: 25891 RVA: 0x001580EC File Offset: 0x001562EC
		internal static int GetCommonPathLength(string first, string second, bool ignoreCase)
		{
			int num = PathInternal.EqualStartingCharacterCount(first, second, ignoreCase);
			if (num == 0)
			{
				return num;
			}
			if (num == first.Length && (num == second.Length || PathInternal.IsDirectorySeparator(second[num])))
			{
				return num;
			}
			if (num == second.Length && PathInternal.IsDirectorySeparator(first[num]))
			{
				return num;
			}
			while (num > 0 && !PathInternal.IsDirectorySeparator(first[num - 1]))
			{
				num--;
			}
			return num;
		}

		// Token: 0x06006524 RID: 25892 RVA: 0x0015815C File Offset: 0x0015635C
		internal unsafe static int EqualStartingCharacterCount(string first, string second, bool ignoreCase)
		{
			if (string.IsNullOrEmpty(first) || string.IsNullOrEmpty(second))
			{
				return 0;
			}
			int num = 0;
			fixed (string text = first)
			{
				char* ptr = text;
				if (ptr != null)
				{
					ptr += RuntimeHelpers.OffsetToStringData / 2;
				}
				fixed (string text2 = second)
				{
					char* ptr2 = text2;
					if (ptr2 != null)
					{
						ptr2 += RuntimeHelpers.OffsetToStringData / 2;
					}
					char* ptr3 = ptr;
					char* ptr4 = ptr2;
					char* ptr5 = ptr3 + first.Length;
					char* ptr6 = ptr4 + second.Length;
					while (ptr3 != ptr5 && ptr4 != ptr6 && (*ptr3 == *ptr4 || (ignoreCase && char.ToUpperInvariant(*ptr3) == char.ToUpperInvariant(*ptr4))))
					{
						num++;
						ptr3++;
						ptr4++;
					}
				}
			}
			return num;
		}

		// Token: 0x06006525 RID: 25893 RVA: 0x00158204 File Offset: 0x00156404
		internal static bool AreRootsEqual(string first, string second, StringComparison comparisonType)
		{
			int rootLength = PathInternal.GetRootLength(first);
			int rootLength2 = PathInternal.GetRootLength(second);
			return rootLength == rootLength2 && string.Compare(first, 0, second, 0, rootLength, comparisonType) == 0;
		}

		// Token: 0x06006526 RID: 25894 RVA: 0x00158240 File Offset: 0x00156440
		internal unsafe static string RemoveRelativeSegments(string path, int rootLength)
		{
			bool flag = false;
			int num = rootLength;
			if (PathInternal.IsDirectorySeparator(path[num - 1]))
			{
				num--;
			}
			Span<char> span = new Span<char>(stackalloc byte[(UIntPtr)520], 260);
			ValueStringBuilder valueStringBuilder = new ValueStringBuilder(span);
			if (num > 0)
			{
				valueStringBuilder.Append(path.AsSpan(0, num));
			}
			int i = num;
			while (i < path.Length)
			{
				char c = path[i];
				if (!PathInternal.IsDirectorySeparator(c) || i + 1 >= path.Length)
				{
					goto IL_0165;
				}
				if (!PathInternal.IsDirectorySeparator(path[i + 1]))
				{
					if ((i + 2 == path.Length || PathInternal.IsDirectorySeparator(path[i + 2])) && path[i + 1] == '.')
					{
						i++;
					}
					else
					{
						if (i + 2 >= path.Length || (i + 3 != path.Length && !PathInternal.IsDirectorySeparator(path[i + 3])) || path[i + 1] != '.' || path[i + 2] != '.')
						{
							goto IL_0165;
						}
						int j;
						for (j = valueStringBuilder.Length - 1; j >= num; j--)
						{
							if (PathInternal.IsDirectorySeparator(*valueStringBuilder[j]))
							{
								valueStringBuilder.Length = ((i + 3 >= path.Length && j == num) ? (j + 1) : j);
								break;
							}
						}
						if (j < num)
						{
							valueStringBuilder.Length = num;
						}
						i += 2;
					}
				}
				IL_0180:
				i++;
				continue;
				IL_0165:
				if (c != '\\' && c == '/')
				{
					c = '\\';
					flag = true;
				}
				valueStringBuilder.Append(c);
				goto IL_0180;
			}
			if (!flag && valueStringBuilder.Length == path.Length)
			{
				valueStringBuilder.Dispose();
				return path;
			}
			if (valueStringBuilder.Length >= rootLength)
			{
				return valueStringBuilder.ToString();
			}
			return path.Substring(0, rootLength);
		}

		// Token: 0x170011B7 RID: 4535
		// (get) Token: 0x06006527 RID: 25895 RVA: 0x0015841B File Offset: 0x0015661B
		internal static StringComparison StringComparison
		{
			get
			{
				if (!PathInternal.s_isCaseSensitive)
				{
					return StringComparison.OrdinalIgnoreCase;
				}
				return StringComparison.Ordinal;
			}
		}

		// Token: 0x170011B8 RID: 4536
		// (get) Token: 0x06006528 RID: 25896 RVA: 0x00158427 File Offset: 0x00156627
		internal static bool IsCaseSensitive
		{
			get
			{
				return PathInternal.s_isCaseSensitive;
			}
		}

		// Token: 0x06006529 RID: 25897 RVA: 0x00158430 File Offset: 0x00156630
		private static bool GetIsCaseSensitive()
		{
			bool flag;
			try
			{
				string text = Path.Combine(Path.GetTempPath(), "CASESENSITIVETEST" + Guid.NewGuid().ToString("N"));
				using (new FileStream(text, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None, 4096, FileOptions.DeleteOnClose))
				{
					flag = !File.Exists(text.ToLowerInvariant());
				}
			}
			catch (Exception)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x0600652A RID: 25898 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public static bool IsPartiallyQualified(string path)
		{
			return false;
		}

		// Token: 0x0600652B RID: 25899 RVA: 0x001584B8 File Offset: 0x001566B8
		public static bool HasIllegalCharacters(string path, bool checkAdditional)
		{
			return path.IndexOfAny(Path.InvalidPathChars) != -1;
		}

		// Token: 0x04003B56 RID: 15190
		internal const char DirectorySeparatorChar = '\\';

		// Token: 0x04003B57 RID: 15191
		internal const char AltDirectorySeparatorChar = '/';

		// Token: 0x04003B58 RID: 15192
		internal const char VolumeSeparatorChar = ':';

		// Token: 0x04003B59 RID: 15193
		internal const char PathSeparator = ';';

		// Token: 0x04003B5A RID: 15194
		internal const string DirectorySeparatorCharAsString = "\\";

		// Token: 0x04003B5B RID: 15195
		internal const string ExtendedPathPrefix = "\\\\?\\";

		// Token: 0x04003B5C RID: 15196
		internal const string UncPathPrefix = "\\\\";

		// Token: 0x04003B5D RID: 15197
		internal const string UncExtendedPrefixToInsert = "?\\UNC\\";

		// Token: 0x04003B5E RID: 15198
		internal const string UncExtendedPathPrefix = "\\\\?\\UNC\\";

		// Token: 0x04003B5F RID: 15199
		internal const string DevicePathPrefix = "\\\\.\\";

		// Token: 0x04003B60 RID: 15200
		internal const string ParentDirectoryPrefix = "..\\";

		// Token: 0x04003B61 RID: 15201
		internal const int MaxShortPath = 260;

		// Token: 0x04003B62 RID: 15202
		internal const int MaxShortDirectoryPath = 248;

		// Token: 0x04003B63 RID: 15203
		internal const int DevicePrefixLength = 4;

		// Token: 0x04003B64 RID: 15204
		internal const int UncPrefixLength = 2;

		// Token: 0x04003B65 RID: 15205
		internal const int UncExtendedPrefixLength = 8;

		// Token: 0x04003B66 RID: 15206
		private static readonly bool s_isCaseSensitive = PathInternal.GetIsCaseSensitive();
	}
}
