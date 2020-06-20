using System;
using System.Text;

namespace Raytracer.Objects.GLB
{
    public class GLBFileChunk
    {
        public enum Types
        {
            JSON = 0x4E4F534A,
            BIN = 0x004E4942
        }

        public int Length { get; set; }
        public UInt32 Type { get; set; }
        public byte[] Data { get; set; }

        public bool IsJSON
        {
            get
            {
                return Type == (UInt32)Types.JSON;
            }
        }

        public bool IsBIN
        {
            get
            {
                return Type == (UInt32)Types.BIN;
            }
        }

        public string JSON
        {
            get
            {
                return Encoding.ASCII.GetString(Data, 0, Data.Length);
            }
        }
    }
}