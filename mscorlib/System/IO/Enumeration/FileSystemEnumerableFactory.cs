using System;
using System.Collections.Generic;

namespace System.IO.Enumeration
{
	// Token: 0x02000B7E RID: 2942
	internal static class FileSystemEnumerableFactory
	{
		// Token: 0x06006B1F RID: 27423 RVA: 0x0016E37C File Offset: 0x0016C57C
		internal static void NormalizeInputs(ref string directory, ref string expression, EnumerationOptions options)
		{
			if (Path.IsPathRooted(expression))
			{
				throw new ArgumentException("Second path fragment must not be a drive or UNC name.", "expression");
			}
			ReadOnlySpan<char> directoryName = Path.GetDirectoryName(expression.AsSpan());
			if (directoryName.Length != 0)
			{
				directory = Path.Join(directory, directoryName);
				expression = expression.Substring(directoryName.Length + 1);
			}
			MatchType matchType = options.MatchType;
			if (matchType == MatchType.Simple)
			{
				return;
			}
			if (matchType != MatchType.Win32)
			{
				throw new ArgumentOutOfRangeException("options");
			}
			if (string.IsNullOrEmpty(expression) || expression == "." || expression == "*.*")
			{
				expression = "*";
				return;
			}
			if (Path.DirectorySeparatorChar != '\\' && expression.IndexOfAny(FileSystemEnumerableFactory.s_unixEscapeChars) != -1)
			{
				expression = expression.Replace("\\", "\\\\");
				expression = expression.Replace("\"", "\\\"");
				expression = expression.Replace(">", "\\>");
				expression = expression.Replace("<", "\\<");
			}
			expression = FileSystemName.TranslateWin32Expression(expression);
		}

		// Token: 0x06006B20 RID: 27424 RVA: 0x0016E494 File Offset: 0x0016C694
		private static bool MatchesPattern(string expression, ReadOnlySpan<char> name, EnumerationOptions options)
		{
			bool flag = (options.MatchCasing == MatchCasing.PlatformDefault && !PathInternal.IsCaseSensitive) || options.MatchCasing == MatchCasing.CaseInsensitive;
			MatchType matchType = options.MatchType;
			if (matchType == MatchType.Simple)
			{
				return FileSystemName.MatchesSimpleExpression(expression, name, flag);
			}
			if (matchType != MatchType.Win32)
			{
				throw new ArgumentOutOfRangeException("options");
			}
			return FileSystemName.MatchesWin32Expression(expression, name, flag);
		}

		// Token: 0x06006B21 RID: 27425 RVA: 0x0016E4F4 File Offset: 0x0016C6F4
		internal static IEnumerable<string> UserFiles(string directory, string expression, EnumerationOptions options)
		{
			return new FileSystemEnumerable<string>(directory, delegate(ref FileSystemEntry entry)
			{
				return entry.ToSpecifiedFullPath();
			}, options)
			{
				ShouldIncludePredicate = delegate(ref FileSystemEntry entry)
				{
					return !entry.IsDirectory && FileSystemEnumerableFactory.MatchesPattern(expression, entry.FileName, options);
				}
			};
		}

		// Token: 0x06006B22 RID: 27426 RVA: 0x0016E554 File Offset: 0x0016C754
		internal static IEnumerable<string> UserDirectories(string directory, string expression, EnumerationOptions options)
		{
			return new FileSystemEnumerable<string>(directory, delegate(ref FileSystemEntry entry)
			{
				return entry.ToSpecifiedFullPath();
			}, options)
			{
				ShouldIncludePredicate = delegate(ref FileSystemEntry entry)
				{
					return entry.IsDirectory && FileSystemEnumerableFactory.MatchesPattern(expression, entry.FileName, options);
				}
			};
		}

		// Token: 0x06006B23 RID: 27427 RVA: 0x0016E5B4 File Offset: 0x0016C7B4
		internal static IEnumerable<string> UserEntries(string directory, string expression, EnumerationOptions options)
		{
			return new FileSystemEnumerable<string>(directory, delegate(ref FileSystemEntry entry)
			{
				return entry.ToSpecifiedFullPath();
			}, options)
			{
				ShouldIncludePredicate = delegate(ref FileSystemEntry entry)
				{
					return FileSystemEnumerableFactory.MatchesPattern(expression, entry.FileName, options);
				}
			};
		}

		// Token: 0x06006B24 RID: 27428 RVA: 0x0016E614 File Offset: 0x0016C814
		internal static IEnumerable<FileInfo> FileInfos(string directory, string expression, EnumerationOptions options)
		{
			return new FileSystemEnumerable<FileInfo>(directory, delegate(ref FileSystemEntry entry)
			{
				return (FileInfo)entry.ToFileSystemInfo();
			}, options)
			{
				ShouldIncludePredicate = delegate(ref FileSystemEntry entry)
				{
					return !entry.IsDirectory && FileSystemEnumerableFactory.MatchesPattern(expression, entry.FileName, options);
				}
			};
		}

		// Token: 0x06006B25 RID: 27429 RVA: 0x0016E674 File Offset: 0x0016C874
		internal static IEnumerable<DirectoryInfo> DirectoryInfos(string directory, string expression, EnumerationOptions options)
		{
			return new FileSystemEnumerable<DirectoryInfo>(directory, delegate(ref FileSystemEntry entry)
			{
				return (DirectoryInfo)entry.ToFileSystemInfo();
			}, options)
			{
				ShouldIncludePredicate = delegate(ref FileSystemEntry entry)
				{
					return entry.IsDirectory && FileSystemEnumerableFactory.MatchesPattern(expression, entry.FileName, options);
				}
			};
		}

		// Token: 0x06006B26 RID: 27430 RVA: 0x0016E6D4 File Offset: 0x0016C8D4
		internal static IEnumerable<FileSystemInfo> FileSystemInfos(string directory, string expression, EnumerationOptions options)
		{
			return new FileSystemEnumerable<FileSystemInfo>(directory, delegate(ref FileSystemEntry entry)
			{
				return entry.ToFileSystemInfo();
			}, options)
			{
				ShouldIncludePredicate = delegate(ref FileSystemEntry entry)
				{
					return FileSystemEnumerableFactory.MatchesPattern(expression, entry.FileName, options);
				}
			};
		}

		// Token: 0x04003DB3 RID: 15795
		private static readonly char[] s_unixEscapeChars = new char[] { '\\', '"', '<', '>' };
	}
}
