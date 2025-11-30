using System.Numerics;
using Raylib_cs;
using TheRealEngine.Nodes;
namespace KrysRendering.Ops;

public abstract record KrysOp : LeafNode {
    protected abstract void RunOp(double d);

    public override void Update(double delta) {
        RunOp(delta);
    }

    public override void Tick(double delta) { }
}

public record DrawTextOp(string text, int posX, int posY, int fontSize, string color) : KrysOp {
    public string Text = text;
    public int PosX = posX;
    public int PosY = posY;
    public int FontSize = fontSize;
    public string Color = color;
    protected override void RunOp(double d) => Raylib.DrawText(Text, PosX, PosY, FontSize, KrysUtils.ParseHex(Color));
}

// --- Drawing Setup & State ---

public record ClearBackgroundOp(string color) : KrysOp {
    public string Color = color;
    protected override void RunOp(double d) => Raylib.ClearBackground(KrysUtils.ParseHex(Color));
}

public record BeginDrawingOp : KrysOp {
    protected override void RunOp(double d) => Raylib.BeginDrawing();
}

public record EndDrawingOp : KrysOp {
    protected override void RunOp(double d) => Raylib.EndDrawing();
}

// --- 2D Mode ---

public record BeginMode2DOp(Camera2D camera) : KrysOp {
    public Camera2D Camera = camera;
    protected override void RunOp(double d) => Raylib.BeginMode2D(Camera);
}

public record EndMode2DOp : KrysOp {
    protected override void RunOp(double d) => Raylib.EndMode2D();
}

// --- 3D Mode ---

public record BeginMode3DOp(Camera3D camera) : KrysOp {
    public Camera3D Camera = camera;
    protected override void RunOp(double d) => Raylib.BeginMode3D(Camera);
}

public record EndMode3DOp : KrysOp {
    protected override void RunOp(double d) => Raylib.EndMode3D();
}

// --- Texture Mode ---

public record BeginTextureModeOp(RenderTexture2D target) : KrysOp {
    public RenderTexture2D Target = target;
    protected override void RunOp(double d) => Raylib.BeginTextureMode(Target);
}

public record EndTextureModeOp : KrysOp {
    protected override void RunOp(double d) => Raylib.EndTextureMode();
}

// --- Shader Mode ---

public record BeginShaderModeOp(Shader shader) : KrysOp {
    public Shader Shader = shader;
    protected override void RunOp(double d) => Raylib.BeginShaderMode(Shader);
}

public record EndShaderModeOp : KrysOp {
    protected override void RunOp(double d) => Raylib.EndShaderMode();
}

// --- Blend Mode ---

public record BeginBlendModeOp(BlendMode mode) : KrysOp {
    public BlendMode Mode = mode;
    protected override void RunOp(double d) => Raylib.BeginBlendMode(Mode);
}

public record EndBlendModeOp : KrysOp {
    protected override void RunOp(double d) => Raylib.EndBlendMode();
}

// --- Scissor Mode ---

public record BeginScissorModeOp(int x, int y, int width, int height) : KrysOp {
    public int X = x;
    public int Y = y;
    public int Width = width;
    public int Height = height;
    protected override void RunOp(double d) => Raylib.BeginScissorMode(X, Y, Width, Height);
}

public record EndScissorModeOp : KrysOp {
    protected override void RunOp(double d) => Raylib.EndScissorMode();
}

// --- VR Stereo Mode ---

public record BeginVrStereoModeOp(VrStereoConfig config) : KrysOp {
    public VrStereoConfig Config = config;
    protected override void RunOp(double d) => Raylib.BeginVrStereoMode(Config);
}

public record EndVrStereoModeOp : KrysOp {
    protected override void RunOp(double d) => Raylib.EndVrStereoMode();
}

// --- Pixel Drawing ---

public record DrawPixelOp(int posX, int posY, string color) : KrysOp {
    public int PosX = posX;
    public int PosY = posY;
    public string Color = color;
    protected override void RunOp(double d) => Raylib.DrawPixel(PosX, PosY, KrysUtils.ParseHex(Color));
}

public record DrawPixelVOp(Vector2 position, string color) : KrysOp {
    public Vector2 Position = position;
    public string Color = color;
    protected override void RunOp(double d) => Raylib.DrawPixelV(Position, KrysUtils.ParseHex(Color));
}

// --- Line Drawing ---

public record DrawLineOp(int startPosX, int startPosY, int endPosX, int endPosY, string color) : KrysOp {
    public int StartPosX = startPosX;
    public int StartPosY = startPosY;
    public int EndPosX = endPosX;
    public int EndPosY = endPosY;
    public string Color = color;
    protected override void RunOp(double d) => Raylib.DrawLine(StartPosX, StartPosY, EndPosX, EndPosY, KrysUtils.ParseHex(Color));
}

public record DrawLineVOp(Vector2 startPos, Vector2 endPos, string color) : KrysOp {
    public Vector2 StartPos = startPos;
    public Vector2 EndPos = endPos;
    public string Color = color;
    protected override void RunOp(double d) => Raylib.DrawLineV(StartPos, EndPos, KrysUtils.ParseHex(Color));
}

public record DrawLineExOp(Vector2 startPos, Vector2 endPos, float thick, string color) : KrysOp {
    public Vector2 StartPos = startPos;
    public Vector2 EndPos = endPos;
    public float Thick = thick;
    public string Color = color;
    protected override void RunOp(double d) => Raylib.DrawLineEx(StartPos, EndPos, Thick, KrysUtils.ParseHex(Color));
}

public record DrawLineBezierOp(Vector2 startPos, Vector2 endPos, float thick, string color) : KrysOp {
    public Vector2 StartPos = startPos;
    public Vector2 EndPos = endPos;
    public float Thick = thick;
    public string Color = color;
    protected override void RunOp(double d) => Raylib.DrawLineBezier(StartPos, EndPos, Thick, KrysUtils.ParseHex(Color));
}

public record DrawLineBezierQuadOp(Vector2 startPos, Vector2 endPos, Vector2 controlPos, float thick, string color) : KrysOp {
    public Vector2 StartPos = startPos;
    public Vector2 EndPos = endPos;
    public Vector2 ControlPos = controlPos;
    public float Thick = thick;
    public string Color = color;
    protected override void RunOp(double d) => Raylib.DrawLineBezierQuad(StartPos, EndPos, ControlPos, Thick, KrysUtils.ParseHex(Color));
}

public record DrawLineBezierCubicOp(Vector2 startPos, Vector2 endPos, Vector2 startControlPos, Vector2 endControlPos, float thick, string color) : KrysOp {
    public Vector2 StartPos = startPos;
    public Vector2 EndPos = endPos;
    public Vector2 StartControlPos = startControlPos;
    public Vector2 EndControlPos = endControlPos;
    public float Thick = thick;
    public string Color = color;
    protected override void RunOp(double d) => Raylib.DrawLineBezierCubic(StartPos, EndPos, StartControlPos, EndControlPos, Thick, KrysUtils.ParseHex(Color));
}

// --- Circle Drawing ---

public record DrawCircleOp(int centerX, int centerY, float radius, string color) : KrysOp {
    public int CenterX = centerX;
    public int CenterY = centerY;
    public float Radius = radius;
    public string Color = color;
    protected override void RunOp(double d) => Raylib.DrawCircle(CenterX, CenterY, Radius, KrysUtils.ParseHex(Color));
}

public record DrawCircleSectorOp(Vector2 center, float radius, float startAngle, float endAngle, int segments, string color) : KrysOp {
    public Vector2 Center = center;
    public float Radius = radius;
    public float StartAngle = startAngle;
    public float EndAngle = endAngle;
    public int Segments = segments;
    public string Color = color;
    protected override void RunOp(double d) => Raylib.DrawCircleSector(Center, Radius, StartAngle, EndAngle, Segments, KrysUtils.ParseHex(Color));
}

public record DrawCircleSectorLinesOp(Vector2 center, float radius, float startAngle, float endAngle, int segments, string color) : KrysOp {
    public Vector2 Center = center;
    public float Radius = radius;
    public float StartAngle = startAngle;
    public float EndAngle = endAngle;
    public int Segments = segments;
    public string Color = color;
    protected override void RunOp(double d) => Raylib.DrawCircleSectorLines(Center, Radius, StartAngle, EndAngle, Segments, KrysUtils.ParseHex(Color));
}

public record DrawCircleGradientOp(int centerX, int centerY, float radius, string inner, string outer) : KrysOp {
    public int CenterX = centerX;
    public int CenterY = centerY;
    public float Radius = radius;
    public string Inner = inner;
    public string Outer = outer;
    protected override void RunOp(double d) => Raylib.DrawCircleGradient(CenterX, CenterY, Radius, KrysUtils.ParseHex(Inner), KrysUtils.ParseHex(Outer));
}

public record DrawCircleVOp(Vector2 center, float radius, string color) : KrysOp {
    public Vector2 Center = center;
    public float Radius = radius;
    public string Color = color;
    protected override void RunOp(double d) => Raylib.DrawCircleV(Center, Radius, KrysUtils.ParseHex(Color));
}

public record DrawCircleLinesOp(int centerX, int centerY, float radius, string color) : KrysOp {
    public int CenterX = centerX;
    public int CenterY = centerY;
    public float Radius = radius;
    public string Color = color;
    protected override void RunOp(double d) => Raylib.DrawCircleLines(CenterX, CenterY, Radius, KrysUtils.ParseHex(Color));
}

public record DrawCircleLinesVOp(Vector2 center, float radius, string color) : KrysOp {
    public Vector2 Center = center;
    public float Radius = radius;
    public string Color = color;
    protected override void RunOp(double d) => Raylib.DrawCircleLinesV(Center, Radius, KrysUtils.ParseHex(Color));
}

// --- Ellipse & Ring Drawing ---

public record DrawEllipseOp(int centerX, int centerY, float radiusH, float radiusV, string color) : KrysOp {
    public int CenterX = centerX;
    public int CenterY = centerY;
    public float RadiusH = radiusH;
    public float RadiusV = radiusV;
    public string Color = color;
    protected override void RunOp(double d) => Raylib.DrawEllipse(CenterX, CenterY, RadiusH, RadiusV, KrysUtils.ParseHex(Color));
}

public record DrawEllipseLinesOp(int centerX, int centerY, float radiusH, float radiusV, string color) : KrysOp {
    public int CenterX = centerX;
    public int CenterY = centerY;
    public float RadiusH = radiusH;
    public float RadiusV = radiusV;
    public string Color = color;
    protected override void RunOp(double d) => Raylib.DrawEllipseLines(CenterX, CenterY, RadiusH, RadiusV, KrysUtils.ParseHex(Color));
}

public record DrawRingOp(Vector2 center, float innerRadius, float outerRadius, float startAngle, float endAngle, int segments, string color) : KrysOp {
    public Vector2 Center = center;
    public float InnerRadius = innerRadius;
    public float OuterRadius = outerRadius;
    public float StartAngle = startAngle;
    public float EndAngle = endAngle;
    public int Segments = segments;
    public string Color = color;
    protected override void RunOp(double d) => Raylib.DrawRing(Center, InnerRadius, OuterRadius, StartAngle, EndAngle, Segments, KrysUtils.ParseHex(Color));
}

public record DrawRingLinesOp(Vector2 center, float innerRadius, float outerRadius, float startAngle, float endAngle, int segments, string color) : KrysOp {
    public Vector2 Center = center;
    public float InnerRadius = innerRadius;
    public float OuterRadius = outerRadius;
    public float StartAngle = startAngle;
    public float EndAngle = endAngle;
    public int Segments = segments;
    public string Color = color;
    protected override void RunOp(double d) => Raylib.DrawRingLines(Center, InnerRadius, OuterRadius, StartAngle, EndAngle, Segments, KrysUtils.ParseHex(Color));
}

// --- Rectangle Drawing ---

public record DrawRectangleOp(int posX, int posY, int width, int height, string color) : KrysOp {
    public int PosX = posX;
    public int PosY = posY;
    public int Width = width;
    public int Height = height;
    public string Color = color;
    protected override void RunOp(double d) => Raylib.DrawRectangle(PosX, PosY, Width, Height, KrysUtils.ParseHex(Color));
}

public record DrawRectangleVOp(Vector2 position, Vector2 size, string color) : KrysOp {
    public Vector2 Position = position;
    public Vector2 Size = size;
    public string Color = color;
    protected override void RunOp(double d) => Raylib.DrawRectangleV(Position, Size, KrysUtils.ParseHex(Color));
}

public record DrawRectangleRecOp(Rectangle rec, string color) : KrysOp {
    public Rectangle Rec = rec;
    public string Color = color;
    protected override void RunOp(double d) => Raylib.DrawRectangleRec(Rec, KrysUtils.ParseHex(Color));
}

public record DrawRectangleProOp(Rectangle rec, Vector2 origin, float rotation, string color) : KrysOp {
    public Rectangle Rec = rec;
    public Vector2 Origin = origin;
    public float Rotation = rotation;
    public string Color = color;
    protected override void RunOp(double d) => Raylib.DrawRectanglePro(Rec, Origin, Rotation, KrysUtils.ParseHex(Color));
}

public record DrawRectangleGradientVOp(int posX, int posY, int width, int height, string top, string bottom) : KrysOp {
    public int PosX = posX;
    public int PosY = posY;
    public int Width = width;
    public int Height = height;
    public string Top = top;
    public string Bottom = bottom;
    protected override void RunOp(double d) => Raylib.DrawRectangleGradientV(PosX, PosY, Width, Height, KrysUtils.ParseHex(Top), KrysUtils.ParseHex(Bottom));
}

public record DrawRectangleGradientHOp(int posX, int posY, int width, int height, string left, string right) : KrysOp {
    public int PosX = posX;
    public int PosY = posY;
    public int Width = width;
    public int Height = height;
    public string Left = left;
    public string Right = right;
    protected override void RunOp(double d) => Raylib.DrawRectangleGradientH(PosX, PosY, Width, Height, KrysUtils.ParseHex(Left), KrysUtils.ParseHex(Right));
}

public record DrawRectangleGradientExOp(Rectangle rec, string topLeft, string bottomLeft, string topRight, string bottomRight) : KrysOp {
    public Rectangle Rec = rec;
    public string TopLeft = topLeft;
    public string BottomLeft = bottomLeft;
    public string TopRight = topRight;
    public string BottomRight = bottomRight;
    protected override void RunOp(double d) => Raylib.DrawRectangleGradientEx(Rec, KrysUtils.ParseHex(TopLeft), KrysUtils.ParseHex(BottomLeft), KrysUtils.ParseHex(TopRight), KrysUtils.ParseHex(BottomRight));
}

public record DrawRectangleLinesOp(int posX, int posY, int width, int height, string color) : KrysOp {
    public int PosX = posX;
    public int PosY = posY;
    public int Width = width;
    public int Height = height;
    public string Color = color;
    protected override void RunOp(double d) => Raylib.DrawRectangleLines(PosX, PosY, Width, Height, KrysUtils.ParseHex(Color));
}

public record DrawRectangleLinesExOp(Rectangle rec, float lineThick, string color) : KrysOp {
    public Rectangle Rec = rec;
    public float LineThick = lineThick;
    public string Color = color;
    protected override void RunOp(double d) => Raylib.DrawRectangleLinesEx(Rec, LineThick, KrysUtils.ParseHex(Color));
}

public record DrawRectangleRoundedOp(Rectangle rec, float roundness, int segments, string color) : KrysOp {
    public Rectangle Rec = rec;
    public float Roundness = roundness;
    public int Segments = segments;
    public string Color = color;
    protected override void RunOp(double d) => Raylib.DrawRectangleRounded(Rec, Roundness, Segments, KrysUtils.ParseHex(Color));
}

public record DrawRectangleRoundedLinesOp(Rectangle rec, float roundness, int segments, string color) : KrysOp {
    public Rectangle Rec = rec;
    public float Roundness = roundness;
    public int Segments = segments;
    public string Color = color;
    protected override void RunOp(double d) => Raylib.DrawRectangleRoundedLines(Rec, Roundness, Segments, KrysUtils.ParseHex(Color));
}

public record DrawRectangleRoundedLinesExOp(Rectangle rec, float roundness, int segments, float lineThick, string color) : KrysOp {
    public Rectangle Rec = rec;
    public float Roundness = roundness;
    public int Segments = segments;
    public float LineThick = lineThick;
    public string Color = color;
    protected override void RunOp(double d) => Raylib.DrawRectangleRoundedLinesEx(Rec, Roundness, Segments, LineThick, KrysUtils.ParseHex(Color));
}

// --- Triangle Drawing ---

public record DrawTriangleOp(Vector2 v1, Vector2 v2, Vector2 v3, string color) : KrysOp {
    public Vector2 V1 = v1;
    public Vector2 V2 = v2;
    public Vector2 V3 = v3;
    public string Color = color;
    protected override void RunOp(double d) => Raylib.DrawTriangle(V1, V2, V3, KrysUtils.ParseHex(Color));
}

public record DrawTriangleLinesOp(Vector2 v1, Vector2 v2, Vector2 v3, string color) : KrysOp {
    public Vector2 V1 = v1;
    public Vector2 V2 = v2;
    public Vector2 V3 = v3;
    public string Color = color;
    protected override void RunOp(double d) => Raylib.DrawTriangleLines(V1, V2, V3, KrysUtils.ParseHex(Color));
}

// --- Polygon Drawing ---

public record DrawPolyOp(Vector2 center, int sides, float radius, float rotation, string color) : KrysOp {
    public Vector2 Center = center;
    public int Sides = sides;
    public float Radius = radius;
    public float Rotation = rotation;
    public string Color = color;
    protected override void RunOp(double d) => Raylib.DrawPoly(Center, Sides, Radius, Rotation, KrysUtils.ParseHex(Color));
}

public record DrawPolyLinesOp(Vector2 center, int sides, float radius, float rotation, string color) : KrysOp {
    public Vector2 Center = center;
    public int Sides = sides;
    public float Radius = radius;
    public float Rotation = rotation;
    public string Color = color;
    protected override void RunOp(double d) => Raylib.DrawPolyLines(Center, Sides, Radius, Rotation, KrysUtils.ParseHex(Color));
}

public record DrawPolyLinesExOp(Vector2 center, int sides, float radius, float rotation, float lineThick, string color) : KrysOp {
    public Vector2 Center = center;
    public int Sides = sides;
    public float Radius = radius;
    public float Rotation = rotation;
    public float LineThick = lineThick;
    public string Color = color;
    protected override void RunOp(double d) => Raylib.DrawPolyLinesEx(Center, Sides, Radius, Rotation, LineThick, KrysUtils.ParseHex(Color));
}
