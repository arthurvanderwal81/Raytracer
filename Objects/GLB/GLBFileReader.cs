using System;
using System.IO;
using System.Collections.Generic;

namespace Raytracer.Objects.GLB
{
    public class GLBFileReader
    {
        private static GLBFileChunk ReadChunk(BinaryReader binaryReader)
        {
            GLBFileChunk glbFileChunk = new GLBFileChunk();

            glbFileChunk.Length = binaryReader.ReadInt32();
            glbFileChunk.Type = (UInt32)binaryReader.ReadInt32();
            glbFileChunk.Data = binaryReader.ReadBytes(glbFileChunk.Length);

            return glbFileChunk;
        }

        public static GLBFileData ReadFile(string filename)
        {
            GLBFileData glbFileData = new GLBFileData();

            FileStream fileStream = new FileStream(filename, FileMode.Open);
            BinaryReader binaryReader = new BinaryReader(fileStream);

            glbFileData.Magic = (UInt32)binaryReader.ReadInt32();
            glbFileData.Version = (UInt32)binaryReader.ReadInt32();
            glbFileData.Length = binaryReader.ReadInt32();

            if (!glbFileData.VerifyMagic())
            {
                throw new Exception("Not a GLBT file");
            }

            if (glbFileData.Length != fileStream.Length)
            {
                throw new Exception("File lengths don't match");
            }

            while (binaryReader.BaseStream.Position != binaryReader.BaseStream.Length)
            {
                glbFileData.FileChunks.Add(ReadChunk(binaryReader));
            }

            foreach (GLBFileChunk fileChunk in glbFileData.FileChunks)
            {
                if (fileChunk.IsJSON)
                {
                    glbFileData.ParseJSON(fileChunk.JSON);
                }
            }

            return glbFileData;
        }
    }
}