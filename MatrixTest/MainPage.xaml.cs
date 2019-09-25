using System;
using System.Numerics;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

namespace MatrixTest
{
    public sealed partial class MainPage : Page
    {
        private ContainerVisual container;

        public MainPage()
        {
            InitializeComponent();

            //ItemsControl.ItemsSource = new int[1000];
        }

        private void CreateVisuals()
        {
            var compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;

            container = compositor.CreateContainerVisual();
            container.Offset = new Vector3((float)Window.Current.Bounds.Width / 2, (float)Window.Current.Bounds.Height / 2, 0);

            container2 = compositor.CreateContainerVisual();
            container2.Offset = new Vector3(-128*5 + 32, -128 * 5 + 32, 0);

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    for (int k = 0; k < 10; k++)
                    {
                        var sprite = compositor.CreateSpriteVisual();
                        sprite.Size = new Vector2(64);
                        sprite.Offset = new Vector3(k * 128, j * 128, i * 128);
                        sprite.Brush = compositor.CreateColorBrush(Color.FromArgb(255, (byte)(k*25), (byte)(j * 25), (byte)(i * 25)));
                        sprite.BackfaceVisibility = CompositionBackfaceVisibility.Visible;
                        
                        container2.Children.InsertAtBottom(sprite);
                    }
                }
            }

            mesh = new CustomMesh(32, 32, 32, compositor);

            //container.Children.InsertAtBottom(mesh.Container);
            container.Children.InsertAtBottom(container2);

            ElementCompositionPreview.SetElementChildVisual(Spawner, container);
        }

        private void CalcPerspective()
        {
            try
            {
                container.TransformMatrix = Matrix4x4.CreateLookAt(new Vector3(x, y, z), new Vector3(x1, y1, 0), new Vector3(0, 1, 0)) 
                    * Matrix4x4.CreatePerspectiveFieldOfView(0.001f, 1, 1, 2);
                
            }
            catch
            {

            }
        }

        float x, y, z = 0, d1 = 1, d2 = 2, x1, y1;
        private ContainerVisual container2;
        private CustomMesh mesh;

        private void Slider_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            x = (float)Math.Pow(e.NewValue, 2);
            CalcPerspective();
        }

        private void Slider_ValueChanged_1(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            y = (float)Math.Pow(e.NewValue, 2);
            CalcPerspective();
        }

        private void Spawner_ManipulationDelta(object sender, Windows.UI.Xaml.Input.ManipulationDeltaRoutedEventArgs e)
        {
            x1 += (float)e.Delta.Translation.X;
            y1 += (float)e.Delta.Translation.Y;
            CalcPerspective();
        }

        private void Slider_ValueChanged_2(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            z = -(float)Math.Pow(e.NewValue, 2);
            CalcPerspective();

        }

        private void Slider_ValueChanged_3(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            for (int i = 0; i < mesh.Sprites.GetLength(1); i++)
            {
                for (int j = 0; j < mesh.Sprites.GetLength(0); j++)
                {
                    mesh.Sprites[j, i].Offset = new Vector3(mesh.Sprites[j, i].Offset.X, mesh.Sprites[j, i].Offset.Y, i * j * (float)e.NewValue);
                }
            }
        }

        private void Slider_ValueChanged_4(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            d2 = (float)Math.Pow(e.NewValue, 4) ;
            CalcPerspective();

        }

        private void Page_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            CreateVisuals();
        }
    }
}
