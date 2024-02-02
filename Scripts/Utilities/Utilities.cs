using System;
using System.IO;

using TalesFromTheTable.SystemServices;

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

        public static bool IsPng(string filePath)
        {
            try
            {
                // Read the first 8 bytes of the file to identify the file format
                byte[] header = new byte[8];
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    fs.Read(header, 0, 8);
                }

                // Check if the file has a PNG header
                return header[0] == 0x89
                    && header[1] == 0x50
                    && header[2] == 0x4E
                    && header[3] == 0x47
                    && header[4] == 0x0D
                    && header[5] == 0x0A
                    && header[6] == 0x1A
                    && header[7] == 0x0A;
            }
            catch (Exception)
            {
                // An exception occurred, indicating the file is not accessible or not a valid file
                return false;
            }
        }



    }
}
