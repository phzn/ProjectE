using System;
using System.Buffers;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO
{
	/// <summary>Provides static methods for the creation, copying, deletion, moving, and opening of a single file, and aids in the creation of <see cref="T:System.IO.FileStream" /> objects.</summary>
	// Token: 0x02000B31 RID: 2865
	public static class File
	{
		/// <summary>Opens an existing UTF-8 encoded text file for reading.</summary>
		/// <param name="path">The file to be opened for reading.</param>
		/// <returns>A <see cref="T:System.IO.StreamReader" /> on the specified path.</returns>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, (for example, it is on an unmapped drive).</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The file specified in <paramref name="path" /> was not found.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="path" /> is in an invalid format.</exception>
		// Token: 0x06006752 RID: 26450 RVA: 0x001600AC File Offset: 0x0015E2AC
		public static StreamReader OpenText(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			return new StreamReader(path);
		}

		/// <summary>Creates or opens a file for writing UTF-8 encoded text. If the file already exists, its contents are overwritten.</summary>
		/// <param name="path">The file to be opened for writing.</param>
		/// <returns>A <see cref="T:System.IO.StreamWriter" /> that writes to the specified file using UTF-8 encoding.</returns>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="path" /> is in an invalid format.</exception>
		// Token: 0x06006753 RID: 26451 RVA: 0x001600C2 File Offset: 0x0015E2C2
		public static StreamWriter CreateText(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			return new StreamWriter(path, false);
		}

		/// <summary>Creates a <see cref="T:System.IO.StreamWriter" /> that appends UTF-8 encoded text to an existing file, or to a new file if the specified file does not exist.</summary>
		/// <param name="path">The path to the file to append to.</param>
		/// <returns>A stream writer that appends UTF-8 encoded text to the specified file or to a new file.</returns>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, the directory doesn't exist or it is on an unmapped drive).</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="path" /> is in an invalid format.</exception>
		// Token: 0x06006754 RID: 26452 RVA: 0x001600D9 File Offset: 0x0015E2D9
		public static StreamWriter AppendText(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			return new StreamWriter(path, true);
		}

		/// <summary>Copies an existing file to a new file. Overwriting a file of the same name is not allowed.</summary>
		/// <param name="sourceFileName">The file to copy.</param>
		/// <param name="destFileName">The name of the destination file. This cannot be a directory or an existing file.</param>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="sourceFileName" /> or <paramref name="destFileName" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.  
		/// -or-  
		/// <paramref name="sourceFileName" /> or <paramref name="destFileName" /> specifies a directory.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="sourceFileName" /> or <paramref name="destFileName" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The path specified in <paramref name="sourceFileName" /> or <paramref name="destFileName" /> is invalid (for example, it is on an unmapped drive).</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">
		///   <paramref name="sourceFileName" /> was not found.</exception>
		/// <exception cref="T:System.IO.IOException">
		///   <paramref name="destFileName" /> exists.  
		/// -or-  
		/// An I/O error has occurred.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="sourceFileName" /> or <paramref name="destFileName" /> is in an invalid format.</exception>
		// Token: 0x06006755 RID: 26453 RVA: 0x001600F0 File Offset: 0x0015E2F0
		public static void Copy(string sourceFileName, string destFileName)
		{
			File.Copy(sourceFileName, destFileName, false);
		}

		/// <summary>Copies an existing file to a new file. Overwriting a file of the same name is allowed.</summary>
		/// <param name="sourceFileName">The file to copy.</param>
		/// <param name="destFileName">The name of the destination file. This cannot be a directory.</param>
		/// <param name="overwrite">
		///   <see langword="true" /> if the destination file can be overwritten; otherwise, <see langword="false" />.</param>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.  
		///  -or-  
		///  <paramref name="destFileName" /> is read-only.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="sourceFileName" /> or <paramref name="destFileName" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.  
		/// -or-  
		/// <paramref name="sourceFileName" /> or <paramref name="destFileName" /> specifies a directory.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="sourceFileName" /> or <paramref name="destFileName" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The path specified in <paramref name="sourceFileName" /> or <paramref name="destFileName" /> is invalid (for example, it is on an unmapped drive).</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">
		///   <paramref name="sourceFileName" /> was not found.</exception>
		/// <exception cref="T:System.IO.IOException">
		///   <paramref name="destFileName" /> exists and <paramref name="overwrite" /> is <see langword="false" />.  
		/// -or-  
		/// An I/O error has occurred.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="sourceFileName" /> or <paramref name="destFileName" /> is in an invalid format.</exception>
		// Token: 0x06006756 RID: 26454 RVA: 0x001600FC File Offset: 0x0015E2FC
		public static void Copy(string sourceFileName, string destFileName, bool overwrite)
		{
			if (sourceFileName == null)
			{
				throw new ArgumentNullException("sourceFileName", "File name cannot be null.");
			}
			if (destFileName == null)
			{
				throw new ArgumentNullException("destFileName", "File name cannot be null.");
			}
			if (sourceFileName.Length == 0)
			{
				throw new ArgumentException("Empty file name is not legal.", "sourceFileName");
			}
			if (destFileName.Length == 0)
			{
				throw new ArgumentException("Empty file name is not legal.", "destFileName");
			}
			FileSystem.CopyFile(Path.GetFullPath(sourceFileName), Path.GetFullPath(destFileName), overwrite);
		}

		/// <summary>Creates or overwrites a file in the specified path.</summary>
		/// <param name="path">The path and name of the file to create.</param>
		/// <returns>A <see cref="T:System.IO.FileStream" /> that provides read/write access to the file specified in <paramref name="path" />.</returns>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.  
		///  -or-  
		///  <paramref name="path" /> specified a file that is read-only.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred while creating the file.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="path" /> is in an invalid format.</exception>
		// Token: 0x06006757 RID: 26455 RVA: 0x00160171 File Offset: 0x0015E371
		public static FileStream Create(string path)
		{
			return File.Create(path, 4096);
		}

		/// <summary>Creates or overwrites the specified file.</summary>
		/// <param name="path">The name of the file.</param>
		/// <param name="bufferSize">The number of bytes buffered for reads and writes to the file.</param>
		/// <returns>A <see cref="T:System.IO.FileStream" /> with the specified buffer size that provides read/write access to the file specified in <paramref name="path" />.</returns>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.  
		///  -or-  
		///  <paramref name="path" /> specified a file that is read-only.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred while creating the file.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="path" /> is in an invalid format.</exception>
		// Token: 0x06006758 RID: 26456 RVA: 0x0016017E File Offset: 0x0015E37E
		public static FileStream Create(string path, int bufferSize)
		{
			return new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None, bufferSize);
		}

		/// <summary>Creates or overwrites the specified file, specifying a buffer size and a <see cref="T:System.IO.FileOptions" /> value that describes how to create or overwrite the file.</summary>
		/// <param name="path">The name of the file.</param>
		/// <param name="bufferSize">The number of bytes buffered for reads and writes to the file.</param>
		/// <param name="options">One of the <see cref="T:System.IO.FileOptions" /> values that describes how to create or overwrite the file.</param>
		/// <returns>A new file with the specified buffer size.</returns>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.  
		///  -or-  
		///  <paramref name="path" /> specified a file that is read-only.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred while creating the file.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="path" /> is in an invalid format.</exception>
		// Token: 0x06006759 RID: 26457 RVA: 0x0016018A File Offset: 0x0015E38A
		public static FileStream Create(string path, int bufferSize, FileOptions options)
		{
			return new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None, bufferSize, options);
		}

		/// <summary>Deletes the specified file.</summary>
		/// <param name="path">The name of the file to be deleted. Wildcard characters are not supported.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
		/// <exception cref="T:System.IO.IOException">The specified file is in use.  
		///  -or-  
		///  There is an open handle on the file, and the operating system is Windows XP or earlier. This open handle can result from enumerating directories and files. For more information, see How to: Enumerate Directories and Files.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="path" /> is in an invalid format.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.  
		///  -or-  
		///  The file is an executable file that is in use.  
		///  -or-  
		///  <paramref name="path" /> is a directory.  
		///  -or-  
		///  <paramref name="path" /> specified a read-only file.</exception>
		// Token: 0x0600675A RID: 26458 RVA: 0x00160197 File Offset: 0x0015E397
		public static void Delete(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			FileSystem.DeleteFile(Path.GetFullPath(path));
		}

		/// <summary>Determines whether the specified file exists.</summary>
		/// <param name="path">The file to check.</param>
		/// <returns>
		///   <see langword="true" /> if the caller has the required permissions and <paramref name="path" /> contains the name of an existing file; otherwise, <see langword="false" />. This method also returns <see langword="false" /> if <paramref name="path" /> is <see langword="null" />, an invalid path, or a zero-length string. If the caller does not have sufficient permissions to read the specified file, no exception is thrown and the method returns <see langword="false" /> regardless of the existence of <paramref name="path" />.</returns>
		// Token: 0x0600675B RID: 26459 RVA: 0x001601B4 File Offset: 0x0015E3B4
		public static bool Exists(string path)
		{
			try
			{
				if (path == null)
				{
					return false;
				}
				if (path.Length == 0)
				{
					return false;
				}
				path = Path.GetFullPath(path);
				if (path.Length > 0 && PathInternal.IsDirectorySeparator(path[path.Length - 1]))
				{
					return false;
				}
				return FileSystem.FileExists(path);
			}
			catch (ArgumentException)
			{
			}
			catch (IOException)
			{
			}
			catch (UnauthorizedAccessException)
			{
			}
			return false;
		}

		/// <summary>Opens a <see cref="T:System.IO.FileStream" /> on the specified path with read/write access with no sharing.</summary>
		/// <param name="path">The file to open.</param>
		/// <param name="mode">A <see cref="T:System.IO.FileMode" /> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
		/// <returns>A <see cref="T:System.IO.FileStream" /> opened in the specified mode and path, with read/write access and not shared.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, (for example, it is on an unmapped drive).</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">
		///   <paramref name="path" /> specified a file that is read-only.  
		/// -or-  
		/// This operation is not supported on the current platform.  
		/// -or-  
		/// <paramref name="path" /> specified a directory.  
		/// -or-  
		/// The caller does not have the required permission.  
		/// -or-  
		/// <paramref name="mode" /> is <see cref="F:System.IO.FileMode.Create" /> and the specified file is a hidden file.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="mode" /> specified an invalid value.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The file specified in <paramref name="path" /> was not found.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="path" /> is in an invalid format.</exception>
		// Token: 0x0600675C RID: 26460 RVA: 0x0016023C File Offset: 0x0015E43C
		public static FileStream Open(string path, FileMode mode)
		{
			return File.Open(path, mode, (mode == FileMode.Append) ? FileAccess.Write : FileAccess.ReadWrite, FileShare.None);
		}

		/// <summary>Opens a <see cref="T:System.IO.FileStream" /> on the specified path, with the specified mode and access with no sharing.</summary>
		/// <param name="path">The file to open.</param>
		/// <param name="mode">A <see cref="T:System.IO.FileMode" /> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
		/// <param name="access">A <see cref="T:System.IO.FileAccess" /> value that specifies the operations that can be performed on the file.</param>
		/// <returns>An unshared <see cref="T:System.IO.FileStream" /> that provides access to the specified file, with the specified mode and access.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.  
		/// -or-  
		/// <paramref name="access" /> specified <see langword="Read" /> and <paramref name="mode" /> specified <see langword="Create" />, <see langword="CreateNew" />, <see langword="Truncate" />, or <see langword="Append" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, (for example, it is on an unmapped drive).</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">
		///   <paramref name="path" /> specified a file that is read-only and <paramref name="access" /> is not <see langword="Read" />.  
		/// -or-  
		/// <paramref name="path" /> specified a directory.  
		/// -or-  
		/// The caller does not have the required permission.  
		/// -or-  
		/// <paramref name="mode" /> is <see cref="F:System.IO.FileMode.Create" /> and the specified file is a hidden file.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="mode" /> or <paramref name="access" /> specified an invalid value.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The file specified in <paramref name="path" /> was not found.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="path" /> is in an invalid format.</exception>
		// Token: 0x0600675D RID: 26461 RVA: 0x0016024E File Offset: 0x0015E44E
		public static FileStream Open(string path, FileMode mode, FileAccess access)
		{
			return File.Open(path, mode, access, FileShare.None);
		}

		/// <summary>Opens a <see cref="T:System.IO.FileStream" /> on the specified path, having the specified mode with read, write, or read/write access and the specified sharing option.</summary>
		/// <param name="path">The file to open.</param>
		/// <param name="mode">A <see cref="T:System.IO.FileMode" /> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
		/// <param name="access">A <see cref="T:System.IO.FileAccess" /> value that specifies the operations that can be performed on the file.</param>
		/// <param name="share">A <see cref="T:System.IO.FileShare" /> value specifying the type of access other threads have to the file.</param>
		/// <returns>A <see cref="T:System.IO.FileStream" /> on the specified path, having the specified mode with read, write, or read/write access and the specified sharing option.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.  
		/// -or-  
		/// <paramref name="access" /> specified <see langword="Read" /> and <paramref name="mode" /> specified <see langword="Create" />, <see langword="CreateNew" />, <see langword="Truncate" />, or <see langword="Append" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, (for example, it is on an unmapped drive).</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">
		///   <paramref name="path" /> specified a file that is read-only and <paramref name="access" /> is not <see langword="Read" />.  
		/// -or-  
		/// <paramref name="path" /> specified a directory.  
		/// -or-  
		/// The caller does not have the required permission.  
		/// -or-  
		/// <paramref name="mode" /> is <see cref="F:System.IO.FileMode.Create" /> and the specified file is a hidden file.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="mode" />, <paramref name="access" />, or <paramref name="share" /> specified an invalid value.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The file specified in <paramref name="path" /> was not found.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="path" /> is in an invalid format.</exception>
		// Token: 0x0600675E RID: 26462 RVA: 0x00160259 File Offset: 0x0015E459
		public static FileStream Open(string path, FileMode mode, FileAccess access, FileShare share)
		{
			return new FileStream(path, mode, access, share);
		}

		// Token: 0x0600675F RID: 26463 RVA: 0x00160264 File Offset: 0x0015E464
		internal static DateTimeOffset GetUtcDateTimeOffset(DateTime dateTime)
		{
			if (dateTime.Kind == DateTimeKind.Unspecified)
			{
				return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
			}
			return dateTime.ToUniversalTime();
		}

		/// <summary>Sets the date and time the file was created.</summary>
		/// <param name="path">The file for which to set the creation date and time information.</param>
		/// <param name="creationTime">A <see cref="T:System.DateTime" /> containing the value to set for the creation date and time of <paramref name="path" />. This value is expressed in local time.</param>
		/// <exception cref="T:System.IO.FileNotFoundException">The specified path was not found.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred while performing the operation.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="creationTime" /> specifies a value outside the range of dates, times, or both permitted for this operation.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="path" /> is in an invalid format.</exception>
		// Token: 0x06006760 RID: 26464 RVA: 0x00160288 File Offset: 0x0015E488
		public static void SetCreationTime(string path, DateTime creationTime)
		{
			FileSystem.SetCreationTime(Path.GetFullPath(path), creationTime, false);
		}

		/// <summary>Sets the date and time, in coordinated universal time (UTC), that the file was created.</summary>
		/// <param name="path">The file for which to set the creation date and time information.</param>
		/// <param name="creationTimeUtc">A <see cref="T:System.DateTime" /> containing the value to set for the creation date and time of <paramref name="path" />. This value is expressed in UTC time.</param>
		/// <exception cref="T:System.IO.FileNotFoundException">The specified path was not found.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred while performing the operation.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="creationTime" /> specifies a value outside the range of dates, times, or both permitted for this operation.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="path" /> is in an invalid format.</exception>
		// Token: 0x06006761 RID: 26465 RVA: 0x0016029C File Offset: 0x0015E49C
		public static void SetCreationTimeUtc(string path, DateTime creationTimeUtc)
		{
			FileSystem.SetCreationTime(Path.GetFullPath(path), File.GetUtcDateTimeOffset(creationTimeUtc), false);
		}

		/// <summary>Returns the creation date and time of the specified file or directory.</summary>
		/// <param name="path">The file or directory for which to obtain creation date and time information.</param>
		/// <returns>A <see cref="T:System.DateTime" /> structure set to the creation date and time for the specified file or directory. This value is expressed in local time.</returns>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="path" /> is in an invalid format.</exception>
		// Token: 0x06006762 RID: 26466 RVA: 0x001602B0 File Offset: 0x0015E4B0
		public static DateTime GetCreationTime(string path)
		{
			return FileSystem.GetCreationTime(Path.GetFullPath(path)).LocalDateTime;
		}

		/// <summary>Returns the creation date and time, in coordinated universal time (UTC), of the specified file or directory.</summary>
		/// <param name="path">The file or directory for which to obtain creation date and time information.</param>
		/// <returns>A <see cref="T:System.DateTime" /> structure set to the creation date and time for the specified file or directory. This value is expressed in UTC time.</returns>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="path" /> is in an invalid format.</exception>
		// Token: 0x06006763 RID: 26467 RVA: 0x001602D0 File Offset: 0x0015E4D0
		public static DateTime GetCreationTimeUtc(string path)
		{
			return FileSystem.GetCreationTime(Path.GetFullPath(path)).UtcDateTime;
		}

		/// <summary>Sets the date and time the specified file was last accessed.</summary>
		/// <param name="path">The file for which to set the access date and time information.</param>
		/// <param name="lastAccessTime">A <see cref="T:System.DateTime" /> containing the value to set for the last access date and time of <paramref name="path" />. This value is expressed in local time.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The specified path was not found.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="path" /> is in an invalid format.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="lastAccessTime" /> specifies a value outside the range of dates or times permitted for this operation.</exception>
		// Token: 0x06006764 RID: 26468 RVA: 0x001602F0 File Offset: 0x0015E4F0
		public static void SetLastAccessTime(string path, DateTime lastAccessTime)
		{
			FileSystem.SetLastAccessTime(Path.GetFullPath(path), lastAccessTime, false);
		}

		/// <summary>Sets the date and time, in coordinated universal time (UTC), that the specified file was last accessed.</summary>
		/// <param name="path">The file for which to set the access date and time information.</param>
		/// <param name="lastAccessTimeUtc">A <see cref="T:System.DateTime" /> containing the value to set for the last access date and time of <paramref name="path" />. This value is expressed in UTC time.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The specified path was not found.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="path" /> is in an invalid format.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="lastAccessTimeUtc" /> specifies a value outside the range of dates or times permitted for this operation.</exception>
		// Token: 0x06006765 RID: 26469 RVA: 0x00160304 File Offset: 0x0015E504
		public static void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
		{
			FileSystem.SetLastAccessTime(Path.GetFullPath(path), File.GetUtcDateTimeOffset(lastAccessTimeUtc), false);
		}

		/// <summary>Returns the date and time the specified file or directory was last accessed.</summary>
		/// <param name="path">The file or directory for which to obtain access date and time information.</param>
		/// <returns>A <see cref="T:System.DateTime" /> structure set to the date and time that the specified file or directory was last accessed. This value is expressed in local time.</returns>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="path" /> is in an invalid format.</exception>
		// Token: 0x06006766 RID: 26470 RVA: 0x00160318 File Offset: 0x0015E518
		public static DateTime GetLastAccessTime(string path)
		{
			return FileSystem.GetLastAccessTime(Path.GetFullPath(path)).LocalDateTime;
		}

		/// <summary>Returns the date and time, in coordinated universal time (UTC), that the specified file or directory was last accessed.</summary>
		/// <param name="path">The file or directory for which to obtain access date and time information.</param>
		/// <returns>A <see cref="T:System.DateTime" /> structure set to the date and time that the specified file or directory was last accessed. This value is expressed in UTC time.</returns>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="path" /> is in an invalid format.</exception>
		// Token: 0x06006767 RID: 26471 RVA: 0x00160338 File Offset: 0x0015E538
		public static DateTime GetLastAccessTimeUtc(string path)
		{
			return FileSystem.GetLastAccessTime(Path.GetFullPath(path)).UtcDateTime;
		}

		/// <summary>Sets the date and time that the specified file was last written to.</summary>
		/// <param name="path">The file for which to set the date and time information.</param>
		/// <param name="lastWriteTime">A <see cref="T:System.DateTime" /> containing the value to set for the last write date and time of <paramref name="path" />. This value is expressed in local time.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The specified path was not found.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="path" /> is in an invalid format.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="lastWriteTime" /> specifies a value outside the range of dates or times permitted for this operation.</exception>
		// Token: 0x06006768 RID: 26472 RVA: 0x00160358 File Offset: 0x0015E558
		public static void SetLastWriteTime(string path, DateTime lastWriteTime)
		{
			FileSystem.SetLastWriteTime(Path.GetFullPath(path), lastWriteTime, false);
		}

		/// <summary>Sets the date and time, in coordinated universal time (UTC), that the specified file was last written to.</summary>
		/// <param name="path">The file for which to set the date and time information.</param>
		/// <param name="lastWriteTimeUtc">A <see cref="T:System.DateTime" /> containing the value to set for the last write date and time of <paramref name="path" />. This value is expressed in UTC time.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The specified path was not found.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="path" /> is in an invalid format.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="lastWriteTimeUtc" /> specifies a value outside the range of dates or times permitted for this operation.</exception>
		// Token: 0x06006769 RID: 26473 RVA: 0x0016036C File Offset: 0x0015E56C
		public static void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
		{
			FileSystem.SetLastWriteTime(Path.GetFullPath(path), File.GetUtcDateTimeOffset(lastWriteTimeUtc), false);
		}

		/// <summary>Returns the date and time the specified file or directory was last written to.</summary>
		/// <param name="path">The file or directory for which to obtain write date and time information.</param>
		/// <returns>A <see cref="T:System.DateTime" /> structure set to the date and time that the specified file or directory was last written to. This value is expressed in local time.</returns>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="path" /> is in an invalid format.</exception>
		// Token: 0x0600676A RID: 26474 RVA: 0x00160380 File Offset: 0x0015E580
		public static DateTime GetLastWriteTime(string path)
		{
			return FileSystem.GetLastWriteTime(Path.GetFullPath(path)).LocalDateTime;
		}

		/// <summary>Returns the date and time, in coordinated universal time (UTC), that the specified file or directory was last written to.</summary>
		/// <param name="path">The file or directory for which to obtain write date and time information.</param>
		/// <returns>A <see cref="T:System.DateTime" /> structure set to the date and time that the specified file or directory was last written to. This value is expressed in UTC time.</returns>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="path" /> is in an invalid format.</exception>
		// Token: 0x0600676B RID: 26475 RVA: 0x001603A0 File Offset: 0x0015E5A0
		public static DateTime GetLastWriteTimeUtc(string path)
		{
			return FileSystem.GetLastWriteTime(Path.GetFullPath(path)).UtcDateTime;
		}

		/// <summary>Gets the <see cref="T:System.IO.FileAttributes" /> of the file on the path.</summary>
		/// <param name="path">The path to the file.</param>
		/// <returns>The <see cref="T:System.IO.FileAttributes" /> of the file on the path.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is empty, contains only white spaces, or contains invalid characters.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="path" /> is in an invalid format.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">
		///   <paramref name="path" /> represents a file and is invalid, such as being on an unmapped drive, or the file cannot be found.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">
		///   <paramref name="path" /> represents a directory and is invalid, such as being on an unmapped drive, or the directory cannot be found.</exception>
		/// <exception cref="T:System.IO.IOException">This file is being used by another process.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
		// Token: 0x0600676C RID: 26476 RVA: 0x001603C0 File Offset: 0x0015E5C0
		public static FileAttributes GetAttributes(string path)
		{
			return FileSystem.GetAttributes(Path.GetFullPath(path));
		}

		/// <summary>Sets the specified <see cref="T:System.IO.FileAttributes" /> of the file on the specified path.</summary>
		/// <param name="path">The path to the file.</param>
		/// <param name="fileAttributes">A bitwise combination of the enumeration values.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is empty, contains only white spaces, contains invalid characters, or the file attribute is invalid.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="path" /> is in an invalid format.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, (for example, it is on an unmapped drive).</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The file cannot be found.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">
		///   <paramref name="path" /> specified a file that is read-only.  
		/// -or-  
		/// This operation is not supported on the current platform.  
		/// -or-  
		/// <paramref name="path" /> specified a directory.  
		/// -or-  
		/// The caller does not have the required permission.</exception>
		// Token: 0x0600676D RID: 26477 RVA: 0x001603D0 File Offset: 0x0015E5D0
		public static void SetAttributes(string path, FileAttributes fileAttributes)
		{
			if ((fileAttributes & (FileAttributes)(-2147483648)) == (FileAttributes)0)
			{
				FileSystem.SetAttributes(Path.GetFullPath(path), fileAttributes);
				return;
			}
			Path.Validate(path);
			MonoIOError monoIOError;
			if (!MonoIO.SetFileAttributes(path, fileAttributes, out monoIOError))
			{
				throw MonoIO.GetException(path, monoIOError);
			}
		}

		/// <summary>Opens an existing file for reading.</summary>
		/// <param name="path">The file to be opened for reading.</param>
		/// <returns>A read-only <see cref="T:System.IO.FileStream" /> on the specified path.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, (for example, it is on an unmapped drive).</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">
		///   <paramref name="path" /> specified a directory.  
		/// -or-  
		/// The caller does not have the required permission.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The file specified in <paramref name="path" /> was not found.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="path" /> is in an invalid format.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
		// Token: 0x0600676E RID: 26478 RVA: 0x0016040C File Offset: 0x0015E60C
		public static FileStream OpenRead(string path)
		{
			return new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
		}

		/// <summary>Opens an existing file or creates a new file for writing.</summary>
		/// <param name="path">The file to be opened for writing.</param>
		/// <returns>An unshared <see cref="T:System.IO.FileStream" /> object on the specified path with <see cref="F:System.IO.FileAccess.Write" /> access.</returns>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.  
		///  -or-  
		///  <paramref name="path" /> specified a read-only file or directory.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, (for example, it is on an unmapped drive).</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="path" /> is in an invalid format.</exception>
		// Token: 0x0600676F RID: 26479 RVA: 0x00160417 File Offset: 0x0015E617
		public static FileStream OpenWrite(string path)
		{
			return new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
		}

		/// <summary>Opens a text file, reads all the text in the file, and then closes the file.</summary>
		/// <param name="path">The file to open for reading.</param>
		/// <returns>A string containing all the text in the file.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">
		///   <paramref name="path" /> specified a file that is read-only.  
		/// -or-  
		/// This operation is not supported on the current platform.  
		/// -or-  
		/// <paramref name="path" /> specified a directory.  
		/// -or-  
		/// The caller does not have the required permission.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The file specified in <paramref name="path" /> was not found.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="path" /> is in an invalid format.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
		// Token: 0x06006770 RID: 26480 RVA: 0x00160422 File Offset: 0x0015E622
		public static string ReadAllText(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Empty path name is not legal.", "path");
			}
			return File.InternalReadAllText(path, Encoding.UTF8);
		}

		/// <summary>Opens a file, reads all text in the file with the specified encoding, and then closes the file.</summary>
		/// <param name="path">The file to open for reading.</param>
		/// <param name="encoding">The encoding applied to the contents of the file.</param>
		/// <returns>A string containing all text in the file.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">
		///   <paramref name="path" /> specified a file that is read-only.  
		/// -or-  
		/// This operation is not supported on the current platform.  
		/// -or-  
		/// <paramref name="path" /> specified a directory.  
		/// -or-  
		/// The caller does not have the required permission.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The file specified in <paramref name="path" /> was not found.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="path" /> is in an invalid format.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
		// Token: 0x06006771 RID: 26481 RVA: 0x00160455 File Offset: 0x0015E655
		public static string ReadAllText(string path, Encoding encoding)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Empty path name is not legal.", "path");
			}
			return File.InternalReadAllText(path, encoding);
		}

		// Token: 0x06006772 RID: 26482 RVA: 0x00160494 File Offset: 0x0015E694
		private static string InternalReadAllText(string path, Encoding encoding)
		{
			string text;
			using (StreamReader streamReader = new StreamReader(path, encoding, true))
			{
				text = streamReader.ReadToEnd();
			}
			return text;
		}

		/// <summary>Creates a new file, writes the specified string to the file, and then closes the file. If the target file already exists, it is overwritten.</summary>
		/// <param name="path">The file to write to.</param>
		/// <param name="contents">The string to write to the file.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is <see langword="null" /> or <paramref name="contents" /> is empty.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">
		///   <paramref name="path" /> specified a file that is read-only.  
		/// -or-  
		/// This operation is not supported on the current platform.  
		/// -or-  
		/// <paramref name="path" /> specified a directory.  
		/// -or-  
		/// The caller does not have the required permission.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="path" /> is in an invalid format.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
		// Token: 0x06006773 RID: 26483 RVA: 0x001604D0 File Offset: 0x0015E6D0
		public static void WriteAllText(string path, string contents)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Empty path name is not legal.", "path");
			}
			using (StreamWriter streamWriter = new StreamWriter(path))
			{
				streamWriter.Write(contents);
			}
		}

		/// <summary>Creates a new file, writes the specified string to the file using the specified encoding, and then closes the file. If the target file already exists, it is overwritten.</summary>
		/// <param name="path">The file to write to.</param>
		/// <param name="contents">The string to write to the file.</param>
		/// <param name="encoding">The encoding to apply to the string.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is <see langword="null" /> or <paramref name="contents" /> is empty.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">
		///   <paramref name="path" /> specified a file that is read-only.  
		/// -or-  
		/// This operation is not supported on the current platform.  
		/// -or-  
		/// <paramref name="path" /> specified a directory.  
		/// -or-  
		/// The caller does not have the required permission.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="path" /> is in an invalid format.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
		// Token: 0x06006774 RID: 26484 RVA: 0x00160530 File Offset: 0x0015E730
		public static void WriteAllText(string path, string contents, Encoding encoding)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Empty path name is not legal.", "path");
			}
			using (StreamWriter streamWriter = new StreamWriter(path, false, encoding))
			{
				streamWriter.Write(contents);
			}
		}

		/// <summary>Opens a binary file, reads the contents of the file into a byte array, and then closes the file.</summary>
		/// <param name="path">The file to open for reading.</param>
		/// <returns>A byte array containing the contents of the file.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">This operation is not supported on the current platform.  
		///  -or-  
		///  <paramref name="path" /> specified a directory.  
		///  -or-  
		///  The caller does not have the required permission.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The file specified in <paramref name="path" /> was not found.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="path" /> is in an invalid format.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
		// Token: 0x06006775 RID: 26485 RVA: 0x001605A0 File Offset: 0x0015E7A0
		public static byte[] ReadAllBytes(string path)
		{
			byte[] array;
			using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 1))
			{
				long length = fileStream.Length;
				if (length > 2147483647L)
				{
					throw new IOException("The file is too long. This operation is currently limited to supporting files less than 2 gigabytes in size.");
				}
				if (length == 0L)
				{
					array = File.ReadAllBytesUnknownLength(fileStream);
				}
				else
				{
					int num = 0;
					int i = (int)length;
					byte[] array2 = new byte[i];
					while (i > 0)
					{
						int num2 = fileStream.Read(array2, num, i);
						if (num2 == 0)
						{
							throw Error.GetEndOfFile();
						}
						num += num2;
						i -= num2;
					}
					array = array2;
				}
			}
			return array;
		}

		// Token: 0x06006776 RID: 26486 RVA: 0x00160638 File Offset: 0x0015E838
		private unsafe static byte[] ReadAllBytesUnknownLength(FileStream fs)
		{
			byte[] array = null;
			Span<byte> span = new Span<byte>(stackalloc byte[(UIntPtr)512], 512);
			byte[] array3;
			try
			{
				int num = 0;
				for (;;)
				{
					if (num == span.Length)
					{
						uint num2 = (uint)(span.Length * 2);
						if (num2 > 2147483591U)
						{
							num2 = (uint)Math.Max(2147483591, span.Length + 1);
						}
						byte[] array2 = ArrayPool<byte>.Shared.Rent((int)num2);
						span.CopyTo(array2);
						if (array != null)
						{
							ArrayPool<byte>.Shared.Return(array, false);
						}
						span = (array = array2);
					}
					int num3 = fs.Read(span.Slice(num));
					if (num3 == 0)
					{
						break;
					}
					num += num3;
				}
				array3 = span.Slice(0, num).ToArray();
			}
			finally
			{
				if (array != null)
				{
					ArrayPool<byte>.Shared.Return(array, false);
				}
			}
			return array3;
		}

		/// <summary>Creates a new file, writes the specified byte array to the file, and then closes the file. If the target file already exists, it is overwritten.</summary>
		/// <param name="path">The file to write to.</param>
		/// <param name="bytes">The bytes to write to the file.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is <see langword="null" /> or the byte array is empty.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">
		///   <paramref name="path" /> specified a file that is read-only.  
		/// -or-  
		/// This operation is not supported on the current platform.  
		/// -or-  
		/// <paramref name="path" /> specified a directory.  
		/// -or-  
		/// The caller does not have the required permission.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="path" /> is in an invalid format.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
		// Token: 0x06006777 RID: 26487 RVA: 0x00160718 File Offset: 0x0015E918
		public static void WriteAllBytes(string path, byte[] bytes)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path", "Path cannot be null.");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Empty path name is not legal.", "path");
			}
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			File.InternalWriteAllBytes(path, bytes);
		}

		// Token: 0x06006778 RID: 26488 RVA: 0x00160768 File Offset: 0x0015E968
		private static void InternalWriteAllBytes(string path, byte[] bytes)
		{
			using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read))
			{
				fileStream.Write(bytes, 0, bytes.Length);
			}
		}

		/// <summary>Opens a text file, reads all lines of the file, and then closes the file.</summary>
		/// <param name="path">The file to open for reading.</param>
		/// <returns>A string array containing all lines of the file.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">
		///   <paramref name="path" /> specified a file that is read-only.  
		/// -or-  
		/// This operation is not supported on the current platform.  
		/// -or-  
		/// <paramref name="path" /> specified a directory.  
		/// -or-  
		/// The caller does not have the required permission.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The file specified in <paramref name="path" /> was not found.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="path" /> is in an invalid format.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
		// Token: 0x06006779 RID: 26489 RVA: 0x001607A8 File Offset: 0x0015E9A8
		public static string[] ReadAllLines(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Empty path name is not legal.", "path");
			}
			return File.InternalReadAllLines(path, Encoding.UTF8);
		}

		/// <summary>Opens a file, reads all lines of the file with the specified encoding, and then closes the file.</summary>
		/// <param name="path">The file to open for reading.</param>
		/// <param name="encoding">The encoding applied to the contents of the file.</param>
		/// <returns>A string array containing all lines of the file.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">
		///   <paramref name="path" /> specified a file that is read-only.  
		/// -or-  
		/// This operation is not supported on the current platform.  
		/// -or-  
		/// <paramref name="path" /> specified a directory.  
		/// -or-  
		/// The caller does not have the required permission.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The file specified in <paramref name="path" /> was not found.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="path" /> is in an invalid format.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
		// Token: 0x0600677A RID: 26490 RVA: 0x001607DB File Offset: 0x0015E9DB
		public static string[] ReadAllLines(string path, Encoding encoding)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Empty path name is not legal.", "path");
			}
			return File.InternalReadAllLines(path, encoding);
		}

		// Token: 0x0600677B RID: 26491 RVA: 0x00160818 File Offset: 0x0015EA18
		private static string[] InternalReadAllLines(string path, Encoding encoding)
		{
			List<string> list = new List<string>();
			using (StreamReader streamReader = new StreamReader(path, encoding))
			{
				string text;
				while ((text = streamReader.ReadLine()) != null)
				{
					list.Add(text);
				}
			}
			return list.ToArray();
		}

		/// <summary>Reads the lines of a file.</summary>
		/// <param name="path">The file to read.</param>
		/// <returns>All the lines of the file, or the lines that are the result of a query.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters defined by the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">
		///   <paramref name="path" /> is invalid (for example, it is on an unmapped drive).</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The file specified by <paramref name="path" /> was not found.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">
		///   <paramref name="path" /> exceeds the system-defined maximum length.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">
		///   <paramref name="path" /> specifies a file that is read-only.  
		/// -or-  
		/// This operation is not supported on the current platform.  
		/// -or-  
		/// <paramref name="path" /> is a directory.  
		/// -or-  
		/// The caller does not have the required permission.</exception>
		// Token: 0x0600677C RID: 26492 RVA: 0x00160868 File Offset: 0x0015EA68
		public static IEnumerable<string> ReadLines(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Empty path name is not legal.", "path");
			}
			return ReadLinesIterator.CreateIterator(path, Encoding.UTF8);
		}

		/// <summary>Read the lines of a file that has a specified encoding.</summary>
		/// <param name="path">The file to read.</param>
		/// <param name="encoding">The encoding that is applied to the contents of the file.</param>
		/// <returns>All the lines of the file, or the lines that are the result of a query.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">
		///   <paramref name="path" /> is invalid (for example, it is on an unmapped drive).</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The file specified by <paramref name="path" /> was not found.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">
		///   <paramref name="path" /> exceeds the system-defined maximum length.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">
		///   <paramref name="path" /> specifies a file that is read-only.  
		/// -or-  
		/// This operation is not supported on the current platform.  
		/// -or-  
		/// <paramref name="path" /> is a directory.  
		/// -or-  
		/// The caller does not have the required permission.</exception>
		// Token: 0x0600677D RID: 26493 RVA: 0x0016089B File Offset: 0x0015EA9B
		public static IEnumerable<string> ReadLines(string path, Encoding encoding)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Empty path name is not legal.", "path");
			}
			return ReadLinesIterator.CreateIterator(path, encoding);
		}

		/// <summary>Creates a new file, write the specified string array to the file, and then closes the file.</summary>
		/// <param name="path">The file to write to.</param>
		/// <param name="contents">The string array to write to the file.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">Either <paramref name="path" /> or <paramref name="contents" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">
		///   <paramref name="path" /> specified a file that is read-only.  
		/// -or-  
		/// This operation is not supported on the current platform.  
		/// -or-  
		/// <paramref name="path" /> specified a directory.  
		/// -or-  
		/// The caller does not have the required permission.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="path" /> is in an invalid format.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
		// Token: 0x0600677E RID: 26494 RVA: 0x001608D8 File Offset: 0x0015EAD8
		public static void WriteAllLines(string path, string[] contents)
		{
			File.WriteAllLines(path, contents);
		}

		/// <summary>Creates a new file, writes a collection of strings to the file, and then closes the file.</summary>
		/// <param name="path">The file to write to.</param>
		/// <param name="contents">The lines to write to the file.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters defined by the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
		/// <exception cref="T:System.ArgumentNullException">Either <paramref name="path" /> or <paramref name="contents" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">
		///   <paramref name="path" /> is invalid (for example, it is on an unmapped drive).</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">
		///   <paramref name="path" /> exceeds the system-defined maximum length.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="path" /> is in an invalid format.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">
		///   <paramref name="path" /> specifies a file that is read-only.  
		/// -or-  
		/// This operation is not supported on the current platform.  
		/// -or-  
		/// <paramref name="path" /> is a directory.  
		/// -or-  
		/// The caller does not have the required permission.</exception>
		// Token: 0x0600677F RID: 26495 RVA: 0x001608E4 File Offset: 0x0015EAE4
		public static void WriteAllLines(string path, IEnumerable<string> contents)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (contents == null)
			{
				throw new ArgumentNullException("contents");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Empty path name is not legal.", "path");
			}
			File.InternalWriteAllLines(new StreamWriter(path), contents);
		}

		/// <summary>Creates a new file, writes the specified string array to the file by using the specified encoding, and then closes the file.</summary>
		/// <param name="path">The file to write to.</param>
		/// <param name="contents">The string array to write to the file.</param>
		/// <param name="encoding">An <see cref="T:System.Text.Encoding" /> object that represents the character encoding applied to the string array.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">Either <paramref name="path" /> or <paramref name="contents" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">
		///   <paramref name="path" /> specified a file that is read-only.  
		/// -or-  
		/// This operation is not supported on the current platform.  
		/// -or-  
		/// <paramref name="path" /> specified a directory.  
		/// -or-  
		/// The caller does not have the required permission.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="path" /> is in an invalid format.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
		// Token: 0x06006780 RID: 26496 RVA: 0x00160931 File Offset: 0x0015EB31
		public static void WriteAllLines(string path, string[] contents, Encoding encoding)
		{
			File.WriteAllLines(path, contents, encoding);
		}

		/// <summary>Creates a new file by using the specified encoding, writes a collection of strings to the file, and then closes the file.</summary>
		/// <param name="path">The file to write to.</param>
		/// <param name="contents">The lines to write to the file.</param>
		/// <param name="encoding">The character encoding to use.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters defined by the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
		/// <exception cref="T:System.ArgumentNullException">Either <paramref name="path" />, <paramref name="contents" />, or <paramref name="encoding" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">
		///   <paramref name="path" /> is invalid (for example, it is on an unmapped drive).</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">
		///   <paramref name="path" /> exceeds the system-defined maximum length.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="path" /> is in an invalid format.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">
		///   <paramref name="path" /> specifies a file that is read-only.  
		/// -or-  
		/// This operation is not supported on the current platform.  
		/// -or-  
		/// <paramref name="path" /> is a directory.  
		/// -or-  
		/// The caller does not have the required permission.</exception>
		// Token: 0x06006781 RID: 26497 RVA: 0x0016093C File Offset: 0x0015EB3C
		public static void WriteAllLines(string path, IEnumerable<string> contents, Encoding encoding)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (contents == null)
			{
				throw new ArgumentNullException("contents");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Empty path name is not legal.", "path");
			}
			File.InternalWriteAllLines(new StreamWriter(path, false, encoding), contents);
		}

		// Token: 0x06006782 RID: 26498 RVA: 0x0016099C File Offset: 0x0015EB9C
		private static void InternalWriteAllLines(TextWriter writer, IEnumerable<string> contents)
		{
			try
			{
				foreach (string text in contents)
				{
					writer.WriteLine(text);
				}
			}
			finally
			{
				if (writer != null)
				{
					((IDisposable)writer).Dispose();
				}
			}
		}

		/// <summary>Opens a file, appends the specified string to the file, and then closes the file. If the file does not exist, this method creates a file, writes the specified string to the file, then closes the file.</summary>
		/// <param name="path">The file to append the specified string to.</param>
		/// <param name="contents">The string to append to the file.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, the directory doesn't exist or it is on an unmapped drive).</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">
		///   <paramref name="path" /> specified a file that is read-only.  
		/// -or-  
		/// This operation is not supported on the current platform.  
		/// -or-  
		/// <paramref name="path" /> specified a directory.  
		/// -or-  
		/// The caller does not have the required permission.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="path" /> is in an invalid format.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
		// Token: 0x06006783 RID: 26499 RVA: 0x001609FC File Offset: 0x0015EBFC
		public static void AppendAllText(string path, string contents)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Empty path name is not legal.", "path");
			}
			using (StreamWriter streamWriter = new StreamWriter(path, true))
			{
				streamWriter.Write(contents);
			}
		}

		/// <summary>Appends the specified string to the file, creating the file if it does not already exist.</summary>
		/// <param name="path">The file to append the specified string to.</param>
		/// <param name="contents">The string to append to the file.</param>
		/// <param name="encoding">The character encoding to use.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, the directory doesn't exist or it is on an unmapped drive).</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">
		///   <paramref name="path" /> specified a file that is read-only.  
		/// -or-  
		/// This operation is not supported on the current platform.  
		/// -or-  
		/// <paramref name="path" /> specified a directory.  
		/// -or-  
		/// The caller does not have the required permission.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="path" /> is in an invalid format.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
		// Token: 0x06006784 RID: 26500 RVA: 0x00160A5C File Offset: 0x0015EC5C
		public static void AppendAllText(string path, string contents, Encoding encoding)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Empty path name is not legal.", "path");
			}
			using (StreamWriter streamWriter = new StreamWriter(path, true, encoding))
			{
				streamWriter.Write(contents);
			}
		}

		/// <summary>Appends lines to a file, and then closes the file. If the specified file does not exist, this method creates a file, writes the specified lines to the file, and then closes the file.</summary>
		/// <param name="path">The file to append the lines to. The file is created if it doesn't already exist.</param>
		/// <param name="contents">The lines to append to the file.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one more invalid characters defined by the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
		/// <exception cref="T:System.ArgumentNullException">Either <paramref name="path" /> or <paramref name="contents" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">
		///   <paramref name="path" /> is invalid (for example, the directory doesn't exist or it is on an unmapped drive).</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The file specified by <paramref name="path" /> was not found.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">
		///   <paramref name="path" /> exceeds the system-defined maximum length.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="path" /> is in an invalid format.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have permission to write to the file.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">
		///   <paramref name="path" /> specifies a file that is read-only.  
		/// -or-  
		/// This operation is not supported on the current platform.  
		/// -or-  
		/// <paramref name="path" /> is a directory.</exception>
		// Token: 0x06006785 RID: 26501 RVA: 0x00160ACC File Offset: 0x0015ECCC
		public static void AppendAllLines(string path, IEnumerable<string> contents)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (contents == null)
			{
				throw new ArgumentNullException("contents");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Empty path name is not legal.", "path");
			}
			File.InternalWriteAllLines(new StreamWriter(path, true), contents);
		}

		/// <summary>Appends lines to a file by using a specified encoding, and then closes the file. If the specified file does not exist, this method creates a file, writes the specified lines to the file, and then closes the file.</summary>
		/// <param name="path">The file to append the lines to. The file is created if it doesn't already exist.</param>
		/// <param name="contents">The lines to append to the file.</param>
		/// <param name="encoding">The character encoding to use.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one more invalid characters defined by the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
		/// <exception cref="T:System.ArgumentNullException">Either <paramref name="path" />, <paramref name="contents" />, or <paramref name="encoding" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">
		///   <paramref name="path" /> is invalid (for example, the directory doesn't exist or it is on an unmapped drive).</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The file specified by <paramref name="path" /> was not found.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">
		///   <paramref name="path" /> exceeds the system-defined maximum length.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="path" /> is in an invalid format.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">
		///   <paramref name="path" /> specifies a file that is read-only.  
		/// -or-  
		/// This operation is not supported on the current platform.  
		/// -or-  
		/// <paramref name="path" /> is a directory.  
		/// -or-  
		/// The caller does not have the required permission.</exception>
		// Token: 0x06006786 RID: 26502 RVA: 0x00160B1C File Offset: 0x0015ED1C
		public static void AppendAllLines(string path, IEnumerable<string> contents, Encoding encoding)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (contents == null)
			{
				throw new ArgumentNullException("contents");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Empty path name is not legal.", "path");
			}
			File.InternalWriteAllLines(new StreamWriter(path, true, encoding), contents);
		}

		/// <summary>Replaces the contents of a specified file with the contents of another file, deleting the original file, and creating a backup of the replaced file.</summary>
		/// <param name="sourceFileName">The name of a file that replaces the file specified by <paramref name="destinationFileName" />.</param>
		/// <param name="destinationFileName">The name of the file being replaced.</param>
		/// <param name="destinationBackupFileName">The name of the backup file.</param>
		/// <exception cref="T:System.ArgumentException">The path described by the <paramref name="destinationFileName" /> parameter was not of a legal form.  
		///  -or-  
		///  The path described by the <paramref name="destinationBackupFileName" /> parameter was not of a legal form.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="destinationFileName" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.DriveNotFoundException">An invalid drive was specified.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The file described by the current <see cref="T:System.IO.FileInfo" /> object could not be found.  
		///  -or-  
		///  The file described by the <paramref name="destinationBackupFileName" /> parameter could not be found.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.  
		/// -or-
		///  The <paramref name="sourceFileName" /> and <paramref name="destinationFileName" /> parameters specify the same file.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
		/// <exception cref="T:System.PlatformNotSupportedException">The operating system is Windows 98 Second Edition or earlier and the files system is not NTFS.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The <paramref name="sourceFileName" /> or <paramref name="destinationFileName" /> parameter specifies a file that is read-only.  
		///  -or-  
		///  This operation is not supported on the current platform.  
		///  -or-  
		///  Source or destination parameters specify a directory instead of a file.  
		///  -or-  
		///  The caller does not have the required permission.</exception>
		// Token: 0x06006787 RID: 26503 RVA: 0x00160B79 File Offset: 0x0015ED79
		public static void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName)
		{
			File.Replace(sourceFileName, destinationFileName, destinationBackupFileName, false);
		}

		/// <summary>Replaces the contents of a specified file with the contents of another file, deleting the original file, and creating a backup of the replaced file and optionally ignores merge errors.</summary>
		/// <param name="sourceFileName">The name of a file that replaces the file specified by <paramref name="destinationFileName" />.</param>
		/// <param name="destinationFileName">The name of the file being replaced.</param>
		/// <param name="destinationBackupFileName">The name of the backup file.</param>
		/// <param name="ignoreMetadataErrors">
		///   <see langword="true" /> to ignore merge errors (such as attributes and access control lists (ACLs)) from the replaced file to the replacement file; otherwise, <see langword="false" />.</param>
		/// <exception cref="T:System.ArgumentException">The path described by the <paramref name="destinationFileName" /> parameter was not of a legal form.  
		///  -or-  
		///  The path described by the <paramref name="destinationBackupFileName" /> parameter was not of a legal form.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="destinationFileName" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.DriveNotFoundException">An invalid drive was specified.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The file described by the current <see cref="T:System.IO.FileInfo" /> object could not be found.  
		///  -or-  
		///  The file described by the <paramref name="destinationBackupFileName" /> parameter could not be found.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.  
		/// -or-
		///  The <paramref name="sourceFileName" /> and <paramref name="destinationFileName" /> parameters specify the same file.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
		/// <exception cref="T:System.PlatformNotSupportedException">The operating system is Windows 98 Second Edition or earlier and the files system is not NTFS.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The <paramref name="sourceFileName" /> or <paramref name="destinationFileName" /> parameter specifies a file that is read-only.  
		///  -or-  
		///  This operation is not supported on the current platform.  
		///  -or-  
		///  Source or destination parameters specify a directory instead of a file.  
		///  -or-  
		///  The caller does not have the required permission.</exception>
		// Token: 0x06006788 RID: 26504 RVA: 0x00160B84 File Offset: 0x0015ED84
		public static void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
		{
			if (sourceFileName == null)
			{
				throw new ArgumentNullException("sourceFileName");
			}
			if (destinationFileName == null)
			{
				throw new ArgumentNullException("destinationFileName");
			}
			FileSystem.ReplaceFile(Path.GetFullPath(sourceFileName), Path.GetFullPath(destinationFileName), (destinationBackupFileName != null) ? Path.GetFullPath(destinationBackupFileName) : null, ignoreMetadataErrors);
		}

		/// <summary>Moves a specified file to a new location, providing the option to specify a new file name.</summary>
		/// <param name="sourceFileName">The name of the file to move. Can include a relative or absolute path.</param>
		/// <param name="destFileName">The new path and name for the file.</param>
		/// <exception cref="T:System.IO.IOException">The destination file already exists.  
		///  -or-  
		///  <paramref name="sourceFileName" /> was not found.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="sourceFileName" /> or <paramref name="destFileName" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="sourceFileName" /> or <paramref name="destFileName" /> is a zero-length string, contains only white space, or contains invalid characters as defined in <see cref="F:System.IO.Path.InvalidPathChars" />.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The path specified in <paramref name="sourceFileName" /> or <paramref name="destFileName" /> is invalid, (for example, it is on an unmapped drive).</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="sourceFileName" /> or <paramref name="destFileName" /> is in an invalid format.</exception>
		// Token: 0x06006789 RID: 26505 RVA: 0x00160BC0 File Offset: 0x0015EDC0
		public static void Move(string sourceFileName, string destFileName)
		{
			if (sourceFileName == null)
			{
				throw new ArgumentNullException("sourceFileName", "File name cannot be null.");
			}
			if (destFileName == null)
			{
				throw new ArgumentNullException("destFileName", "File name cannot be null.");
			}
			if (sourceFileName.Length == 0)
			{
				throw new ArgumentException("Empty file name is not legal.", "sourceFileName");
			}
			if (destFileName.Length == 0)
			{
				throw new ArgumentException("Empty file name is not legal.", "destFileName");
			}
			string fullPath = Path.GetFullPath(sourceFileName);
			string fullPath2 = Path.GetFullPath(destFileName);
			if (!FileSystem.FileExists(fullPath))
			{
				throw new FileNotFoundException(SR.Format("Could not find file '{0}'.", fullPath), fullPath);
			}
			FileSystem.MoveFile(fullPath, fullPath2);
		}

		/// <summary>Encrypts a file so that only the account used to encrypt the file can decrypt it.</summary>
		/// <param name="path">A path that describes a file to encrypt.</param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="path" /> parameter is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="path" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.DriveNotFoundException">An invalid drive was specified.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The file described by the <paramref name="path" /> parameter could not be found.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.  
		///  -or-  
		///  This operation is not supported on the current platform.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
		/// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Windows NT or later.</exception>
		/// <exception cref="T:System.NotSupportedException">The file system is not NTFS.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The <paramref name="path" /> parameter specified a file that is read-only.  
		///  -or-  
		///  This operation is not supported on the current platform.  
		///  -or-  
		///  The <paramref name="path" /> parameter specified a directory.  
		///  -or-  
		///  The caller does not have the required permission.</exception>
		// Token: 0x0600678A RID: 26506 RVA: 0x00160C52 File Offset: 0x0015EE52
		public static void Encrypt(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			throw new PlatformNotSupportedException("File encryption is not supported on this platform.");
		}

		/// <summary>Decrypts a file that was encrypted by the current account using the <see cref="M:System.IO.File.Encrypt(System.String)" /> method.</summary>
		/// <param name="path">A path that describes a file to decrypt.</param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="path" /> parameter is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="path" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.DriveNotFoundException">An invalid drive was specified.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The file described by the <paramref name="path" /> parameter could not be found.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file. For example, the encrypted file is already open.  
		///  -or-  
		///  This operation is not supported on the current platform.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
		/// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Windows NT or later.</exception>
		/// <exception cref="T:System.NotSupportedException">The file system is not NTFS.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The <paramref name="path" /> parameter specified a file that is read-only.  
		///  -or-  
		///  This operation is not supported on the current platform.  
		///  -or-  
		///  The <paramref name="path" /> parameter specified a directory.  
		///  -or-  
		///  The caller does not have the required permission.</exception>
		// Token: 0x0600678B RID: 26507 RVA: 0x00160C52 File Offset: 0x0015EE52
		public static void Decrypt(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			throw new PlatformNotSupportedException("File encryption is not supported on this platform.");
		}

		// Token: 0x170011EB RID: 4587
		// (get) Token: 0x0600678C RID: 26508 RVA: 0x00160C6C File Offset: 0x0015EE6C
		private static Encoding UTF8NoBOM
		{
			get
			{
				Encoding encoding;
				if ((encoding = File.s_UTF8NoBOM) == null)
				{
					encoding = (File.s_UTF8NoBOM = new UTF8Encoding(false, true));
				}
				return encoding;
			}
		}

		// Token: 0x0600678D RID: 26509 RVA: 0x00160C84 File Offset: 0x0015EE84
		private static StreamReader AsyncStreamReader(string path, Encoding encoding)
		{
			return new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.Asynchronous | FileOptions.SequentialScan), encoding, true);
		}

		// Token: 0x0600678E RID: 26510 RVA: 0x00160CA0 File Offset: 0x0015EEA0
		private static StreamWriter AsyncStreamWriter(string path, Encoding encoding, bool append)
		{
			return new StreamWriter(new FileStream(path, append ? FileMode.Append : FileMode.Create, FileAccess.Write, FileShare.Read, 4096, FileOptions.Asynchronous | FileOptions.SequentialScan), encoding);
		}

		// Token: 0x0600678F RID: 26511 RVA: 0x00160CC1 File Offset: 0x0015EEC1
		public static Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken = default(CancellationToken))
		{
			return File.ReadAllTextAsync(path, Encoding.UTF8, cancellationToken);
		}

		// Token: 0x06006790 RID: 26512 RVA: 0x00160CD0 File Offset: 0x0015EED0
		public static Task<string> ReadAllTextAsync(string path, Encoding encoding, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Empty path name is not legal.", "path");
			}
			if (!cancellationToken.IsCancellationRequested)
			{
				return File.InternalReadAllTextAsync(path, encoding, cancellationToken);
			}
			return Task.FromCanceled<string>(cancellationToken);
		}

		// Token: 0x06006791 RID: 26513 RVA: 0x00160D2C File Offset: 0x0015EF2C
		private static async Task<string> InternalReadAllTextAsync(string path, Encoding encoding, CancellationToken cancellationToken)
		{
			char[] buffer = null;
			StreamReader sr = File.AsyncStreamReader(path, encoding);
			string text;
			try
			{
				cancellationToken.ThrowIfCancellationRequested();
				buffer = ArrayPool<char>.Shared.Rent(sr.CurrentEncoding.GetMaxCharCount(4096));
				StringBuilder sb = new StringBuilder();
				for (;;)
				{
					int num = await sr.ReadAsync(new Memory<char>(buffer), cancellationToken).ConfigureAwait(false);
					if (num == 0)
					{
						break;
					}
					sb.Append(buffer, 0, num);
				}
				text = sb.ToString();
			}
			finally
			{
				sr.Dispose();
				if (buffer != null)
				{
					ArrayPool<char>.Shared.Return(buffer, false);
				}
			}
			return text;
		}

		// Token: 0x06006792 RID: 26514 RVA: 0x00160D7F File Offset: 0x0015EF7F
		public static Task WriteAllTextAsync(string path, string contents, CancellationToken cancellationToken = default(CancellationToken))
		{
			return File.WriteAllTextAsync(path, contents, File.UTF8NoBOM, cancellationToken);
		}

		// Token: 0x06006793 RID: 26515 RVA: 0x00160D90 File Offset: 0x0015EF90
		public static Task WriteAllTextAsync(string path, string contents, Encoding encoding, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Empty path name is not legal.", "path");
			}
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.FromCanceled(cancellationToken);
			}
			if (string.IsNullOrEmpty(contents))
			{
				new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read).Dispose();
				return Task.CompletedTask;
			}
			return File.InternalWriteAllTextAsync(File.AsyncStreamWriter(path, encoding, false), contents, cancellationToken);
		}

		// Token: 0x06006794 RID: 26516 RVA: 0x00160E0C File Offset: 0x0015F00C
		public static Task<byte[]> ReadAllBytesAsync(string path, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.FromCanceled<byte[]>(cancellationToken);
			}
			FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 1, FileOptions.Asynchronous | FileOptions.SequentialScan);
			bool flag = false;
			Task<byte[]> task;
			try
			{
				long length = fileStream.Length;
				if (length > 2147483647L)
				{
					task = Task.FromException<byte[]>(new IOException("The file is too long. This operation is currently limited to supporting files less than 2 gigabytes in size."));
				}
				else
				{
					flag = true;
					task = ((length > 0L) ? File.InternalReadAllBytesAsync(fileStream, (int)length, cancellationToken) : File.InternalReadAllBytesUnknownLengthAsync(fileStream, cancellationToken));
				}
			}
			finally
			{
				if (!flag)
				{
					fileStream.Dispose();
				}
			}
			return task;
		}

		// Token: 0x06006795 RID: 26517 RVA: 0x00160E94 File Offset: 0x0015F094
		private static async Task<byte[]> InternalReadAllBytesAsync(FileStream fs, int count, CancellationToken cancellationToken)
		{
			byte[] array;
			try
			{
				int index = 0;
				byte[] bytes = new byte[count];
				for (;;)
				{
					int num = await fs.ReadAsync(new Memory<byte>(bytes, index, count - index), cancellationToken).ConfigureAwait(false);
					if (num == 0)
					{
						break;
					}
					index += num;
					if (index >= count)
					{
						goto Block_3;
					}
				}
				throw Error.GetEndOfFile();
				Block_3:
				array = bytes;
			}
			finally
			{
				if (fs != null)
				{
					((IDisposable)fs).Dispose();
				}
			}
			return array;
		}

		// Token: 0x06006796 RID: 26518 RVA: 0x00160EE8 File Offset: 0x0015F0E8
		private static async Task<byte[]> InternalReadAllBytesUnknownLengthAsync(FileStream fs, CancellationToken cancellationToken)
		{
			byte[] rentedArray = ArrayPool<byte>.Shared.Rent(512);
			byte[] array2;
			try
			{
				int bytesRead = 0;
				for (;;)
				{
					if (bytesRead == rentedArray.Length)
					{
						uint num = (uint)(rentedArray.Length * 2);
						if (num > 2147483591U)
						{
							num = (uint)Math.Max(2147483591, rentedArray.Length + 1);
						}
						byte[] array = ArrayPool<byte>.Shared.Rent((int)num);
						Buffer.BlockCopy(rentedArray, 0, array, 0, bytesRead);
						ArrayPool<byte>.Shared.Return(rentedArray, false);
						rentedArray = array;
					}
					int num2 = await fs.ReadAsync(rentedArray.AsMemory<byte>(bytesRead), cancellationToken).ConfigureAwait(false);
					if (num2 == 0)
					{
						break;
					}
					bytesRead += num2;
				}
				array2 = rentedArray.AsSpan<byte>(0, bytesRead).ToArray();
			}
			finally
			{
				fs.Dispose();
				ArrayPool<byte>.Shared.Return(rentedArray, false);
			}
			return array2;
		}

		// Token: 0x06006797 RID: 26519 RVA: 0x00160F34 File Offset: 0x0015F134
		public static Task WriteAllBytesAsync(string path, byte[] bytes, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (path == null)
			{
				throw new ArgumentNullException("path", "Path cannot be null.");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Empty path name is not legal.", "path");
			}
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			if (!cancellationToken.IsCancellationRequested)
			{
				return File.InternalWriteAllBytesAsync(path, bytes, cancellationToken);
			}
			return Task.FromCanceled(cancellationToken);
		}

		// Token: 0x06006798 RID: 26520 RVA: 0x00160F94 File Offset: 0x0015F194
		private static async Task InternalWriteAllBytesAsync(string path, byte[] bytes, CancellationToken cancellationToken)
		{
			using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read, 4096, FileOptions.Asynchronous | FileOptions.SequentialScan))
			{
				await fs.WriteAsync(new ReadOnlyMemory<byte>(bytes), cancellationToken).ConfigureAwait(false);
				await fs.FlushAsync(cancellationToken).ConfigureAwait(false);
			}
			FileStream fs = null;
		}

		// Token: 0x06006799 RID: 26521 RVA: 0x00160FE7 File Offset: 0x0015F1E7
		public static Task<string[]> ReadAllLinesAsync(string path, CancellationToken cancellationToken = default(CancellationToken))
		{
			return File.ReadAllLinesAsync(path, Encoding.UTF8, cancellationToken);
		}

		// Token: 0x0600679A RID: 26522 RVA: 0x00160FF8 File Offset: 0x0015F1F8
		public static Task<string[]> ReadAllLinesAsync(string path, Encoding encoding, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Empty path name is not legal.", "path");
			}
			if (!cancellationToken.IsCancellationRequested)
			{
				return File.InternalReadAllLinesAsync(path, encoding, cancellationToken);
			}
			return Task.FromCanceled<string[]>(cancellationToken);
		}

		// Token: 0x0600679B RID: 26523 RVA: 0x00161054 File Offset: 0x0015F254
		private static async Task<string[]> InternalReadAllLinesAsync(string path, Encoding encoding, CancellationToken cancellationToken)
		{
			string[] array;
			using (StreamReader sr = File.AsyncStreamReader(path, encoding))
			{
				cancellationToken.ThrowIfCancellationRequested();
				List<string> lines = new List<string>();
				string text;
				while ((text = await sr.ReadLineAsync().ConfigureAwait(false)) != null)
				{
					lines.Add(text);
					cancellationToken.ThrowIfCancellationRequested();
				}
				array = lines.ToArray();
			}
			return array;
		}

		// Token: 0x0600679C RID: 26524 RVA: 0x001610A7 File Offset: 0x0015F2A7
		public static Task WriteAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken = default(CancellationToken))
		{
			return File.WriteAllLinesAsync(path, contents, File.UTF8NoBOM, cancellationToken);
		}

		// Token: 0x0600679D RID: 26525 RVA: 0x001610B8 File Offset: 0x0015F2B8
		public static Task WriteAllLinesAsync(string path, IEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (contents == null)
			{
				throw new ArgumentNullException("contents");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Empty path name is not legal.", "path");
			}
			if (!cancellationToken.IsCancellationRequested)
			{
				return File.InternalWriteAllLinesAsync(File.AsyncStreamWriter(path, encoding, false), contents, cancellationToken);
			}
			return Task.FromCanceled(cancellationToken);
		}

		// Token: 0x0600679E RID: 26526 RVA: 0x00161128 File Offset: 0x0015F328
		private static async Task InternalWriteAllLinesAsync(TextWriter writer, IEnumerable<string> contents, CancellationToken cancellationToken)
		{
			using (writer)
			{
				foreach (string text in contents)
				{
					cancellationToken.ThrowIfCancellationRequested();
					await writer.WriteLineAsync(text).ConfigureAwait(false);
				}
				IEnumerator<string> enumerator = null;
				cancellationToken.ThrowIfCancellationRequested();
				await writer.FlushAsync().ConfigureAwait(false);
			}
			TextWriter textWriter = null;
		}

		// Token: 0x0600679F RID: 26527 RVA: 0x0016117C File Offset: 0x0015F37C
		private static async Task InternalWriteAllTextAsync(StreamWriter sw, string contents, CancellationToken cancellationToken)
		{
			char[] buffer = null;
			try
			{
				buffer = ArrayPool<char>.Shared.Rent(4096);
				int count = contents.Length;
				int batchSize;
				for (int index = 0; index < count; index += batchSize)
				{
					batchSize = Math.Min(4096, count - index);
					contents.CopyTo(index, buffer, 0, batchSize);
					await sw.WriteAsync(new ReadOnlyMemory<char>(buffer, 0, batchSize), cancellationToken).ConfigureAwait(false);
				}
				cancellationToken.ThrowIfCancellationRequested();
				await sw.FlushAsync().ConfigureAwait(false);
			}
			finally
			{
				sw.Dispose();
				if (buffer != null)
				{
					ArrayPool<char>.Shared.Return(buffer, false);
				}
			}
		}

		// Token: 0x060067A0 RID: 26528 RVA: 0x001611CF File Offset: 0x0015F3CF
		public static Task AppendAllTextAsync(string path, string contents, CancellationToken cancellationToken = default(CancellationToken))
		{
			return File.AppendAllTextAsync(path, contents, File.UTF8NoBOM, cancellationToken);
		}

		// Token: 0x060067A1 RID: 26529 RVA: 0x001611E0 File Offset: 0x0015F3E0
		public static Task AppendAllTextAsync(string path, string contents, Encoding encoding, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Empty path name is not legal.", "path");
			}
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.FromCanceled(cancellationToken);
			}
			if (string.IsNullOrEmpty(contents))
			{
				new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read).Dispose();
				return Task.CompletedTask;
			}
			return File.InternalWriteAllTextAsync(File.AsyncStreamWriter(path, encoding, true), contents, cancellationToken);
		}

		// Token: 0x060067A2 RID: 26530 RVA: 0x0016125C File Offset: 0x0015F45C
		public static Task AppendAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken = default(CancellationToken))
		{
			return File.AppendAllLinesAsync(path, contents, File.UTF8NoBOM, cancellationToken);
		}

		// Token: 0x060067A3 RID: 26531 RVA: 0x0016126C File Offset: 0x0015F46C
		public static Task AppendAllLinesAsync(string path, IEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (contents == null)
			{
				throw new ArgumentNullException("contents");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Empty path name is not legal.", "path");
			}
			if (!cancellationToken.IsCancellationRequested)
			{
				return File.InternalWriteAllLinesAsync(File.AsyncStreamWriter(path, encoding, true), contents, cancellationToken);
			}
			return Task.FromCanceled(cancellationToken);
		}

		/// <summary>Creates or overwrites the specified file with the specified buffer size, file options, and file security.</summary>
		/// <param name="path">The name of the file.</param>
		/// <param name="bufferSize">The number of bytes buffered for reads and writes to the file.</param>
		/// <param name="options">One of the <see cref="T:System.IO.FileOptions" /> values that describes how to create or overwrite the file.</param>
		/// <param name="fileSecurity">One of the <see cref="T:System.Security.AccessControl.FileSecurity" /> values that determines the access control and audit security for the file.</param>
		/// <returns>A new file with the specified buffer size, file options, and file security.</returns>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.  
		///  -or-  
		///  <paramref name="path" /> specified a file that is read-only.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred while creating the file.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="path" /> is in an invalid format.</exception>
		// Token: 0x060067A4 RID: 26532 RVA: 0x0016018A File Offset: 0x0015E38A
		public static FileStream Create(string path, int bufferSize, FileOptions options, FileSecurity fileSecurity)
		{
			return new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None, bufferSize, options);
		}

		/// <summary>Gets a <see cref="T:System.Security.AccessControl.FileSecurity" /> object that encapsulates the access control list (ACL) entries for a specified file.</summary>
		/// <param name="path">The path to a file containing a <see cref="T:System.Security.AccessControl.FileSecurity" /> object that describes the file's access control list (ACL) information.</param>
		/// <returns>A <see cref="T:System.Security.AccessControl.FileSecurity" /> object that encapsulates the access control rules for the file described by the <paramref name="path" /> parameter.</returns>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
		/// <exception cref="T:System.Runtime.InteropServices.SEHException">The <paramref name="path" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.SystemException">The file could not be found.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The <paramref name="path" /> parameter specified a file that is read-only.  
		///  -or-  
		///  This operation is not supported on the current platform.  
		///  -or-  
		///  The <paramref name="path" /> parameter specified a directory.  
		///  -or-  
		///  The caller does not have the required permission.</exception>
		// Token: 0x060067A5 RID: 26533 RVA: 0x001612DA File Offset: 0x0015F4DA
		public static FileSecurity GetAccessControl(string path)
		{
			return File.GetAccessControl(path, AccessControlSections.Access | AccessControlSections.Owner | AccessControlSections.Group);
		}

		/// <summary>Gets a <see cref="T:System.Security.AccessControl.FileSecurity" /> object that encapsulates the specified type of access control list (ACL) entries for a particular file.</summary>
		/// <param name="path">The path to a file containing a <see cref="T:System.Security.AccessControl.FileSecurity" /> object that describes the file's access control list (ACL) information.</param>
		/// <param name="includeSections">One of the <see cref="T:System.Security.AccessControl.AccessControlSections" /> values that specifies the type of access control list (ACL) information to receive.</param>
		/// <returns>A <see cref="T:System.Security.AccessControl.FileSecurity" /> object that encapsulates the access control rules for the file described by the <paramref name="path" /> parameter.</returns>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
		/// <exception cref="T:System.Runtime.InteropServices.SEHException">The <paramref name="path" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.SystemException">The file could not be found.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The <paramref name="path" /> parameter specified a file that is read-only.  
		///  -or-  
		///  This operation is not supported on the current platform.  
		///  -or-  
		///  The <paramref name="path" /> parameter specified a directory.  
		///  -or-  
		///  The caller does not have the required permission.</exception>
		// Token: 0x060067A6 RID: 26534 RVA: 0x001612E4 File Offset: 0x0015F4E4
		public static FileSecurity GetAccessControl(string path, AccessControlSections includeSections)
		{
			return new FileSecurity(path, includeSections);
		}

		/// <summary>Applies access control list (ACL) entries described by a <see cref="T:System.Security.AccessControl.FileSecurity" /> object to the specified file.</summary>
		/// <param name="path">A file to add or remove access control list (ACL) entries from.</param>
		/// <param name="fileSecurity">A <see cref="T:System.Security.AccessControl.FileSecurity" /> object that describes an ACL entry to apply to the file described by the <paramref name="path" /> parameter.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
		/// <exception cref="T:System.Runtime.InteropServices.SEHException">The <paramref name="path" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.SystemException">The file could not be found.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The <paramref name="path" /> parameter specified a file that is read-only.  
		///  -or-  
		///  This operation is not supported on the current platform.  
		///  -or-  
		///  The <paramref name="path" /> parameter specified a directory.  
		///  -or-  
		///  The caller does not have the required permission.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="fileSecurity" /> parameter is <see langword="null" />.</exception>
		// Token: 0x060067A7 RID: 26535 RVA: 0x001612ED File Offset: 0x0015F4ED
		public static void SetAccessControl(string path, FileSecurity fileSecurity)
		{
			if (fileSecurity == null)
			{
				throw new ArgumentNullException("fileSecurity");
			}
			fileSecurity.PersistModifications(path);
		}

		// Token: 0x04003C30 RID: 15408
		private const int MaxByteArrayLength = 2147483591;

		// Token: 0x04003C31 RID: 15409
		private static Encoding s_UTF8NoBOM;

		// Token: 0x04003C32 RID: 15410
		internal const int DefaultBufferSize = 4096;
	}
}
