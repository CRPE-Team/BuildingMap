using System;
using System.IO;
using System.Security.Cryptography;

namespace BuildingMap.Core.Utils
{
	public static class HashCodeHelper
	{
		public static string GetHashString(this byte[] data)
		{
			MemoryStream stream = new MemoryStream();
			stream.Write(data, 0, data.Length);

			stream.Seek(0, SeekOrigin.Begin);

			using (var md5Instance = MD5.Create())
			{
				var hashResult = md5Instance.ComputeHash(stream);
				return BitConverter.ToString(hashResult).Replace("-", "").ToLowerInvariant();
			}
		}
	}
}
