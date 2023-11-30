using System;
using System.IO;

namespace TalesFromTheTable.Scripts.Utilities
{
    public static class Utilities
    {
        public static bool IsJpeg(string filePath)
        {
            try
            {
                // Read the first 2 bytes of the file to identify the file format
                byte[] header = new byte[2];
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    fs.Read(header, 0, 2);
                }

                // Check if the file has a JPEG header
                return header[0] == 0xFF && header[1] == 0xD8;
            }
            catch (Exception)
            {
                // An exception occurred, indicating the file is not accessible or not a valid file
                return false;
            }
        }
    }
}
