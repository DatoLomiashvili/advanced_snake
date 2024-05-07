using System;
using System.IO;
using Avalonia.Media.Imaging;

namespace AdvancedSnake
{
    public static class Images
    {
        private static readonly string ProjectDirectory = Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.FullName;
        private static readonly string AssetsDirectory = Path.Combine(ProjectDirectory, "Assets");
        
        public static readonly Bitmap Empty = new Bitmap(Path.Combine(AssetsDirectory, "Empty.png"));
        public static readonly Bitmap Body = new Bitmap(Path.Combine(AssetsDirectory, "Body.png"));
        public static readonly Bitmap Head = new Bitmap(Path.Combine(AssetsDirectory, "Head.png"));
        public static readonly Bitmap Food = new Bitmap(Path.Combine(AssetsDirectory, "Food.png"));
        public static readonly Bitmap DeadBody = new Bitmap(Path.Combine(AssetsDirectory, "DeadBody.png"));
        public static readonly Bitmap DeadHead = new Bitmap(Path.Combine(AssetsDirectory, "DeadHead.png"));
    }
}