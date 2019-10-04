using ExileCore;
using ExileCore.Shared.AtlasHelper;
using ImGuiNET;
using SharpDX;
using ImGuiVector2 = System.Numerics.Vector2;
using ImGuiVector4 = System.Numerics.Vector4;

namespace CheatSheets
{
    partial class CheatSheetsCore
    {
        public Vector2 IconButton(AtlasTexture icon, Vector2 position)
        {
            Graphics.DrawImage(icon, new RectangleF(position.X,
                                                    position.Y,
                                                    icon.TextureUV.Width,
                                                    icon.TextureUV.Height));
            var size = new ImGuiVector2(icon.TextureUV.Width + 2, icon.TextureUV.Height + 2);
            bool refBool = true;
            ImGui.SetNextWindowPos(new ImGuiVector2(position.X - 1, position.Y - 1), ImGuiCond.Appearing);
            ImGui.SetNextWindowSize(size, ImGuiCond.Appearing);
            ImGui.Begin(icon.ToString(), ref refBool, ImGuiWindowFlags.NoBackground |
                                                    ImGuiWindowFlags.NoTitleBar |
                                                    ImGuiWindowFlags.NoScrollbar |
                                                    ImGuiWindowFlags.NoResize |
                                                    ImGuiWindowFlags.NoMove);
            ImGui.PushID(idPop);
            // ImGuiVector4(r, g, b, a);
            ImGui.PushStyleColor(ImGuiCol.Button, new ImGuiVector4(0, 0, 0, 0));
            ImGui.PushStyleColor(ImGuiCol.ButtonHovered, new ImGuiVector4(0, 0, 0, 0.3f));
            ImGui.PushStyleColor(ImGuiCol.ButtonActive, new ImGuiVector4(0, 0, 0, 0.6f));
            // ImGui.PushStyleVar(ImGuiStyleVar.FrameRounding, 3.0f);
            // ImGui.PushStyleVar(ImGuiStyleVar.FramePadding, 2.0f);
            ImGui.Button(icon.ToString() + idPop.ToString(), size);
            ImGui.End();

            return new Vector2(size.X, size.Y);
        }
    }
}
