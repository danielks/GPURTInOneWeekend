using ComputeSharp;
using System.Numerics;
using TerraFX.Interop.Windows;

namespace GPURTInOneWeekend
{
    [AutoConstructor]
    [EmbeddedBytecode(DispatchAxis.XY)]
    internal readonly partial struct RayTracerComputeShader : IComputeShader
    {
        private readonly ReadWriteBuffer<uint> buffer;
        public readonly float time;
        public readonly int renderWidth;
        public void Execute()
        {
            //boilerplate code
            var fragCoord = new Vector2(ThreadIds.X, ThreadIds.Y);
            
            
            

            






            float colX = Hlsl.Abs(Hlsl.Cos(time));
            float colY = Hlsl.Abs(Hlsl.Sin(time));
            float colZ = Hlsl.Abs(Hlsl.Cos(time));

            Vector3 col = new Vector3(colX, colY, colZ);

            col.X = 1;
            col.Y = 0;
            col.Z = 0;


            int idx = (ThreadIds.Y * renderWidth + ThreadIds.X);
            buffer[idx] = (uint)(col.X * 255) << 24;
            buffer[idx] = buffer[idx] | ((uint)(col.Y * 255) << 16);
            buffer[idx] = buffer[idx] | ((uint)(col.Z * 255) << 8);
            buffer[idx] = buffer[idx] | ((uint)255);




        }
    }
}