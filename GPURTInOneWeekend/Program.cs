using GPURTInOneWeekend;
using ComputeSharp;
using Raylib_cs;
using System.Diagnostics;
using TerraFX.Interop.Windows;
using System.Numerics;
using TerraFX.Interop.DirectX;



float iTime = 0f;
Stopwatch timer = Stopwatch.StartNew();
long frameCount = 0;

Raylib.InitWindow(500, 500, "CPU Shader Toy");

int renderWidth = Raylib.GetRenderWidth();
int renderHeight = Raylib.GetRenderHeight();


var gpu = ComputeSharp.GraphicsDevice.GetDefault();
var gpuBuffer = gpu.AllocateReadWriteBuffer<uint>(renderWidth * renderHeight);


Stopwatch sw = new Stopwatch();
double frameTime = sw.Elapsed.TotalMilliseconds;

while (!Raylib.WindowShouldClose())
{
    sw.Restart();

    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.White);

    iTime = (float)timer.Elapsed.TotalSeconds;
    var shader = new RayTracerComputeShader(gpuBuffer, iTime, renderWidth);

    gpu.For(renderWidth, renderHeight, shader);
    var buffer = new uint[renderWidth * renderHeight];
    gpuBuffer.CopyTo(buffer);

    for (int i = 0; i < renderWidth; i++)
    {
        for (int j = 0; j < renderHeight; j++)
        {
            var c = buffer[j * renderWidth + i];

            var color = new Raylib_cs.Color(
                (byte)(c >> 24),
                (byte)(c >> 16),
                (byte)(c >> 8),
                (byte)255

                );

            Raylib.DrawPixel(i, j, color);
        }
    }

    /*
    Image i2 = new Image
    {
        Format = PixelFormat.UncompressedR8G8B8A8,
        Width = renderWidth,
        Height = renderHeight,
        Mipmaps = 1
    };
    Raylib_cs.Texture2D t2 = Raylib.LoadTextureFromImage(i2);

    
    unsafe
    {
        fixed (byte* bPtr = &buffer[0])
        {
            Raylib.UpdateTexture(t2, bPtr);
            Raylib.DrawTexture(t2, 0, 0, Color.White);
        }
    }*/

    Raylib.EndDrawing();

    sw.Stop();
    frameTime = sw.Elapsed.TotalMilliseconds;

    if (frameCount % 50 == 0) //a cada 50 frames mostra o ultimo frametime
    {
        Raylib.SetWindowTitle(string.Format("{0}ms. FPS: {1}", frameTime, 1000.0f / frameTime));
    }

    frameCount++;
}

Raylib.CloseWindow();
