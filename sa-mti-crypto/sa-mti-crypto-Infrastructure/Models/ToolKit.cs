using System;
using System.IO;
using System.Security.Cryptography;

namespace sa_mti_crypto.Infrastructure.Models
{
    public static class ToolKit
    {
        public static byte[] ReadFile(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("Archivo no encontrado", path);

            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[fs.Length];
            _ = fs.Read(buffer, 0, buffer.Length);
            return buffer;
        }

        public static void SaveFile(byte[] data, string outputPath)
        {
            if (data == null || data.Length == 0)
                throw new ArgumentNullException(nameof(data));

            string? directory = Path.GetDirectoryName(outputPath);
            if (string.IsNullOrEmpty(directory))
                throw new ArgumentException("Ruta inválida", nameof(outputPath));

            Directory.CreateDirectory(directory);
            File.WriteAllBytes(outputPath, data);
        }

        [System.Security.SecurityCritical]
        public static void WipeData(byte[]? sensitiveData)
        {
            if (sensitiveData != null && sensitiveData.Length > 0)
                CryptographicOperations.ZeroMemory(sensitiveData.AsSpan());
        }

        public static string GenerateOutputPath(string inputFileName, bool encrypt, string baseDirectory)
        {
            string fileName = Path.GetFileNameWithoutExtension(inputFileName);
            string ext = encrypt ? ".enc" : ".dec";
            return Path.Combine(baseDirectory, $"{fileName}{ext}");
        }
    }
}