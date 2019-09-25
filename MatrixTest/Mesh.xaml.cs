using System;
using System.Collections.Generic;
using System.Numerics;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Composition.Scenes;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;

namespace MatrixTest
{
    public sealed partial class Mesh : Page
    {
        public Mesh()
        {
            InitializeComponent();
        }

        private void Spawner_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CreateVisuals();
        }

        private void CreateVisuals()
        {
            var compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;

            var mesh = SceneMesh.Create(compositor);

        }
    }

    public class CustomMesh
    {
        public SpriteVisual[,] Sprites { get; }
        public ContainerVisual Container { get; }

        public CustomMesh(int x, int y, float gridStep, Compositor compositor)
        {
            Sprites = new SpriteVisual[x,y];
            Container = compositor.CreateContainerVisual();
            Container.Size = new Vector2(x * gridStep, y * gridStep);
            Container.AnchorPoint = new Vector2(0.5f);

            var offset = compositor.CreateVector3KeyFrameAnimation();
            offset.Target = "Offset";
            offset.InsertExpressionKeyFrame(1, "This.FinalValue");
            offset.Duration = TimeSpan.FromMilliseconds(300);

            var implicitAnimations = compositor.CreateImplicitAnimationCollection();
            implicitAnimations["Offset"] = offset;

            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    var sprite = compositor.CreateSpriteVisual();
                    sprite.Size = Vector2.One * gridStep;
                    sprite.Offset = new Vector3(j * gridStep, i * gridStep, 0);
                    sprite.Brush = compositor.CreateColorBrush(Color.FromArgb(255, (byte)(255 / x * j), (byte)(255 / y * i), (byte)(255 / x * i)));
                    sprite.ImplicitAnimations = implicitAnimations;

                    Sprites[j, i] = sprite;
                    Container.Children.InsertAtBottom(sprite);
                }
            }
        }
    }

}
