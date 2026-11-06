using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DA_Assets.Networking
{
	// Token: 0x02000006 RID: 6
	public static class StreamExtensions
	{
		// Token: 0x06000021 RID: 33 RVA: 0x00002544 File Offset: 0x00000744
		public static async Task CopyToAsync(this Stream source, Stream destination, int bufferSize, IProgress<long> progress = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (!source.CanRead)
			{
				throw new ArgumentException("Has to be readable", "source");
			}
			if (destination == null)
			{
				throw new ArgumentNullException("destination");
			}
			if (!destination.CanWrite)
			{
				throw new ArgumentException("Has to be writable", "destination");
			}
			if (bufferSize < 0)
			{
				throw new ArgumentOutOfRangeException("bufferSize");
			}
			byte[] buffer = new byte[bufferSize];
			long totalBytesRead = 0L;
			int bytesRead;
			while ((bytesRead = await source.ReadAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false)) != 0)
			{
				await destination.WriteAsync(buffer, 0, bytesRead, cancellationToken).ConfigureAwait(false);
				totalBytesRead += (long)bytesRead;
				if (progress != null)
				{
					progress.Report(totalBytesRead);
				}
			}
		}
	}
}
