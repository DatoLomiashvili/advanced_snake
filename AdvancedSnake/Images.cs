using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Avalonia.Media.Imaging;

namespace AdvancedSnake
{
    public static class Images
    {
        private static readonly string projectDirectory = Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.FullName;

        private static string assetsDirectory = Path.Combine(projectDirectory, "Assets");
        
        static Images()
        {
        }
        public static readonly Bitmap Empty = new Bitmap(Path.Combine(assetsDirectory, "Empty.png"));
        public static readonly Bitmap Body = new Bitmap(Path.Combine(assetsDirectory, "Body.png"));
        public static readonly Bitmap Head = new Bitmap(Path.Combine(assetsDirectory, "Head.png"));
        public static readonly Bitmap Food = new Bitmap(Path.Combine(assetsDirectory, "Food.png"));
        public static readonly Bitmap DeadBody = new Bitmap(Path.Combine(assetsDirectory, "DeadBody.png"));
        public static readonly Bitmap DeadHead = new Bitmap(Path.Combine(assetsDirectory, "DeadHead.png"));
    }
}