using System;
using System.Collections.Generic;

using Newtonsoft.Json;

using Raytracer.Math;

namespace Raytracer.Objects.GLB
{
    public class GLBFileData
    {
        public UInt32 Magic { get; set; }
        public UInt32 Version { get; set; }
        public int Length { get; set; }
        public List<GLBFileChunk> FileChunks { get; set; }
        public List<GLBBufferView> BufferViews { get; set; }
        public List<GLBAccessor> Accessors { get; set; }
        public List<GLBImage> Images { get; set; }
        public List<GLBSampler> Samplers { get; set; }
        public List<GLBTexture> Textures { get; set; }
        public List<GLBMaterial> Materials { get; set; }
        public List<GLBMesh> Meshes { get; set; }

        public GLBFileData()
        {
            FileChunks = new List<GLBFileChunk>();
            BufferViews = new List<GLBBufferView>();
            Accessors = new List<GLBAccessor>();
            Images = new List<GLBImage>();
            Samplers = new List<GLBSampler>();
            Textures = new List<GLBTexture>();
            Materials = new List<GLBMaterial>();
            Meshes = new List<GLBMesh>();
        }

        public bool VerifyMagic()
        {
            return Magic == 0x46546C67;
        }

        /*
        public byte[] GetBuffer(int bufferIndex)
        {
            return FileChunks[bufferIndex + 1].Data;
        }
        */

        public void ParseJSON(string JSONstring)
        {
            var json = JsonConvert.DeserializeObject<dynamic>(JSONstring);

            foreach (var bufferViewJSON in json.bufferViews)
            {
                GLBBufferView bufferView = new GLBBufferView();

                bufferView.Buffer = FileChunks[(int)bufferViewJSON.buffer + 1].Data;
                bufferView.ByteOffset = bufferViewJSON.byteOffset;
                bufferView.ByteLength = bufferViewJSON.byteLength;
                bufferView.Target = bufferViewJSON.target;

                BufferViews.Add(bufferView);
            }

            foreach (var accessorJSON in json.accessors)
            {
                GLBAccessor accessor = new GLBAccessor();

                accessor.BufferView = BufferViews[(int)accessorJSON.bufferView];
                accessor.ComponentType = accessorJSON.componentType;
                accessor.Count = accessorJSON.count;
                accessor.Type = accessorJSON.type;

                Accessors.Add(accessor);
            }

            foreach (var imageJSON in json.images)
            {
                GLBImage image = new GLBImage();

                image.BufferView = BufferViews[(int)imageJSON.bufferView];
                image.MimeType = imageJSON.mimeType;

                Images.Add(image);
            }

            foreach (var samplerJSON in json.samplers)
            {
                GLBSampler sampler = new GLBSampler();

                sampler.MinFilter = samplerJSON.minFilter;
                sampler.MagFilter = samplerJSON.magFilter;
                sampler.WrapS = samplerJSON.wrapS;
                sampler.WrapT = samplerJSON.wrapT;

                Samplers.Add(sampler);
            }

            foreach (var textureJSON in json.textures)
            {
                GLBTexture texture = new GLBTexture();

                texture.Source = Images[(int)textureJSON.source];
                texture.Sampler = Samplers[(int)textureJSON.sampler];

                Textures.Add(texture);
            }

            foreach (var materialJSON in json.materials)
            {
                GLBMaterial material = new GLBMaterial();

                material.Name = materialJSON.name;

                Materials.Add(material);
            }

            foreach (var meshJSON in json.meshes)
            {
                GLBMesh mesh = new GLBMesh();

                mesh.Name = meshJSON.name;

                foreach (var primitiveJSON in meshJSON.primitives)
                {
                    GLBPrimitive primitive = new GLBPrimitive();

                    GLBAttributes attributes = new GLBAttributes();

                    attributes.Position = Accessors[(int)primitiveJSON.attributes.POSITION];
                    attributes.Normal = Accessors[(int)primitiveJSON.attributes.NORMAL];

                    primitive.Attributes = attributes;
                    primitive.Indices = Accessors[(int)primitiveJSON.indices];
                    primitive.Material = Materials[(int)primitiveJSON.material];

                    mesh.Primitives.Add(primitive);
                }

                Meshes.Add(mesh);
            }
        }
    }
}