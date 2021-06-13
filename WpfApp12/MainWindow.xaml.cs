using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Media3D;
using System.Windows.Threading;

namespace WpfApp12
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            piramida(0, 13, 0, 10, Colors.SpringGreen, mainViewport);
            r_piramida(0, 3, 0, 10, Colors.SpringGreen, mainViewport);
            draw_pir();
        }

        DispatcherTimer movement = new DispatcherTimer();
        public void draw_pir()
        {

            movement.Interval = new TimeSpan(0, 0, 0, 0, 50);
            movement.IsEnabled = true;
            movement.Tick += move;
        }

        double starting_i = Math.PI / 2;
        public void move(object sender, EventArgs e)
        {
            mainViewport.Children.Clear();
            Model3DGroup myModel3DGroup = new Model3DGroup();
            ModelVisual3D myModelVisual3D = new ModelVisual3D();
            DirectionalLight myDirectionalLight = new DirectionalLight();
            myDirectionalLight.Color = Colors.White;
            myDirectionalLight.Direction = new Vector3D(-2, -3, -1);
            myModel3DGroup.Children.Add(myDirectionalLight);
            myModelVisual3D.Content = myModel3DGroup;
            mainViewport.Children.Add(myModelVisual3D);
            piramida(0, 13, 0, 10, Colors.SpringGreen, mainViewport);
            r_piramida(0, 3, 0, 10, Colors.SpringGreen, mainViewport);
            for (double i = starting_i; i < 5 * starting_i; i += Math.PI / 4)
            {
                double h1 = -20 * Math.Sin(i), h2 = -50 * Math.Cos(i);
                piramida(h1, 8, h2, 5, Colors.MintCream, mainViewport);
                r_piramida(h1, 3, h2, 5, Colors.MintCream, mainViewport);
            }

            starting_i += Math.PI / 100;
        }

        public static void drawTriangle(Point3D p0, Point3D p1, Point3D p2, Color color, Viewport3D viewport)
        {
            // функция рисования тругольника
            MeshGeometry3D mesh = new MeshGeometry3D();

            // добавляем координаты вершин треугольника
            mesh.Positions.Add(p0);
            mesh.Positions.Add(p1);
            mesh.Positions.Add(p2);

            // настраиваем правило буравчика
            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(2);

            // закрашиваем треугольник и делаем его материал матовым - DiffuseMaterial
            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = color;
            Material material = new DiffuseMaterial(brush);
            // при желании можно сделать материал блестящим - SpecularMaterial(кисть, степень искристости(double)) или светящимся - EmissiveMaterial

            GeometryModel3D geometry = new GeometryModel3D(mesh, material);
            ModelUIElement3D model = new ModelUIElement3D();
            model.Model = geometry;

            viewport.Children.Add(model);
        }

        // создание вершин треугольника и его отрисовка
        private void Tri(double x1, double y1, double z1,
                         double x2, double y2, double z2,
                         double x3, double y3, double z3, Color color, Viewport3D viewport)
        {
            Point3D[] p = new Point3D[3];
            p[0] = new Point3D(x1, y1, z1);
            p[1] = new Point3D(x2, y2, z2);
            p[2] = new Point3D(x3, y3, z3);
            drawTriangle(p[0], p[1], p[2], color, viewport);
        }



        // отрисовка квадрата как двух треугольников
        private void Quad(double x1, double y1, double z1,
                          double x2, double y2, double z2,
                          double x3, double y3, double z3,
                          double x4, double y4, double z4, Color color, Viewport3D viewport)
        {

            Tri(x1, y1, z1, x2, y2, z2, x4, y4, z4, color, viewport);
            Tri(x2, y2, z2, x3, y3, z3, x4, y4, z4, color, viewport);

        }

        public double x = 11;
        public double y = 10;
        public double z = 9;

        public void redro()
        {
            cam.Position = new Point3D(x, y, z);
        }
        private void cube(double xx, double yx, double zx, double a, Color color1, Color color2, Color color3, Viewport3D viewport)
        {
            double x1 = xx;
            double y1 = yx;
            double z1 = zx;

            double x2 = xx + a;
            double y2 = yx;
            double z2 = zx;

            double x3 = xx + a;
            double y3 = yx + a;
            double z3 = zx;

            double x4 = xx;
            double y4 = yx + a;
            double z4 = zx;

            double x5 = x1;
            double y5 = y1;
            double z5 = z1 + a;

            double x6 = x2;
            double y6 = y2;
            double z6 = z2 + a;

            double x7 = x3;
            double y7 = y3;
            double z7 = z3 + a;

            double x8 = x4;
            double y8 = y4;
            double z8 = z4 + a;

            Quad(x5, y5, z5, x8, y8, z8, x4, y4, z4, x1, y1, z1, color1, viewport);
            Quad(x6, y6, z6, x5, y5, z5, x1, y1, z1, x2, y2, z2, color2, viewport);
            Quad(x3, y3, z3, x2, y2, z2, x1, y1, z1, x4, y4, z4, color1, viewport);
            Quad(x7, y7, z7, x6, y6, z6, x2, y2, z2, x3, y3, z3, color2, viewport);
            Quad(x8, y8, z8, x7, y7, z7, x3, y3, z3, x4, y4, z4, color3, viewport);
            Quad(x5, y5, z5, x6, y6, z6, x7, y7, z7, x8, y8, z8, color3, viewport);
            /*Quad(x1, y1 + r, z1, x1, y1, z1, x1 + r, y1, z1, x1 + r, y1 + r, z1, color, viewport);
            Quad(x1, y1, z1, x1, y1 + r, z1, x1, y1 + r, z1 + r, x1, y1, z1 + r, color, viewport);
            Quad(x1, y1 + r, z1, x1, y1 + r, z1 + r, x1 + r, y1 + r, z1 + r, x1 + r, y1 + r, z1, color, viewport);
            Quad(x1, y1, z1, x1, y1, z1 + r, x1 + r, y1, z1 + r, x1 + r, y1, z1, color, viewport);
            Quad(x1, y1 + r, z1 + r, x1, y1, z1 + r, x1 + r, y1, z1 + r, x1 + r, y1 + r, z1 + r, color, viewport);
            Quad(x1 + r, y1 + r, z1, x1 + r, y1 + r, z1 + r, x1 + r, y1, z1 + r, x1 + r, y1, z1, color, viewport);*/
        }

        private void piramida(double x, double y, double z, double a, Color color, Viewport3D viewport)
        {
            double x1 = x;
            double y1 = y;
            double z1 = z;

            double x2 = x + a;
            double y2 = y;
            double z2 = z;

            double x3 = x + a / 2;
            double y3 = y + a;
            double z3 = z + a / 2;

            double x4 = x + a / 2;
            double y4 = y + a;
            double z4 = z + a / 2;

            double x5 = x1;
            double y5 = y1;
            double z5 = z1 + a;

            double x6 = x2;
            double y6 = y2;
            double z6 = z2 + a;

            double x7 = x3;
            double y7 = y3;
            double z7 = z3;

            double x8 = x4;
            double y8 = y4;
            double z8 = z4;

            Quad(x5, y5, z5, x8, y8, z8, x4, y4, z4, x1, y1, z1, color, viewport);
            Quad(x6, y6, z6, x5, y5, z5, x1, y1, z1, x2, y2, z2, color, viewport);
            Quad(x3, y3, z3, x2, y2, z2, x1, y1, z1, x4, y4, z4, color, viewport);
            Quad(x7, y7, z7, x6, y6, z6, x2, y2, z2, x3, y3, z3, color, viewport);
            Quad(x8, y8, z8, x7, y7, z7, x3, y3, z3, x4, y4, z4, color, viewport);
            Quad(x5, y5, z5, x6, y6, z6, x7, y7, z7, x8, y8, z8, color, viewport);



        }

        private void r_piramida(double x, double y, double z, double a, Color color, Viewport3D viewport)
        {
            double x1 = x + a / 2;
            double y1 = y;
            double z1 = z + a / 2;

            double x2 = x + a / 2;
            double y2 = y;
            double z2 = z + a / 2;

            double x3 = x + a;
            double y3 = y + a;
            double z3 = z;

            double x4 = x;
            double y4 = y + a;
            double z4 = z;

            double x5 = x1;
            double y5 = y1;
            double z5 = z1;

            double x6 = x2;
            double y6 = y2;
            double z6 = z2;

            double x7 = x3;
            double y7 = y3;
            double z7 = z3 + a;

            double x8 = x4;
            double y8 = y4;
            double z8 = z4 + a;

            Quad(x5, y5, z5, x8, y8, z8, x4, y4, z4, x1, y1, z1, color, viewport);
            Quad(x6, y6, z6, x5, y5, z5, x1, y1, z1, x2, y2, z2, color, viewport);
            Quad(x3, y3, z3, x2, y2, z2, x1, y1, z1, x4, y4, z4, color, viewport);
            Quad(x7, y7, z7, x6, y6, z6, x2, y2, z2, x3, y3, z3, color, viewport);
            Quad(x8, y8, z8, x7, y7, z7, x3, y3, z3, x4, y4, z4, color, viewport);
            Quad(x5, y5, z5, x6, y6, z6, x7, y7, z7, x8, y8, z8, color, viewport);



        }
        private void sx_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            x = -sx.Value;
            redro();
        }

        private void sx1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            z = -sx1.Value;
            redro();
        }

        private void sy_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            y = -sy.Value;
            redro();
        }
    }
}
