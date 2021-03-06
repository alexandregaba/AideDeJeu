﻿
using AideDeJeu.ViewModels;
using AideDeJeuLib;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AideDeJeu.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ItemDetailPage : ContentPage
	{
        public MainViewModel Main
        {
            get
            {
                return DependencyService.Get<MainViewModel>();
            }
        }

        ItemDetailViewModel viewModel;

        public ItemDetailPage(ItemDetailViewModel itemVM)
        {
            InitializeComponent();

            BindingContext = this.viewModel = itemVM;


            //mdMarkdown.NavigateToLink = async (s) => await viewModel.Main.Navigator.NavigateToLinkAsync(s);
        }

        public ItemDetailPage()
        {
            InitializeComponent();

            BindingContext = this.viewModel = new ItemDetailViewModel(new Item()
            {
                Name = "Bibliothèque",
                Id = "index.md",
                Markdown = AideDeJeu.Tools.Helpers.GetResourceString($"AideDeJeu.Data.index.md"),
            }
            ) { Title = "Bibliothèque" };
            //var item = new Item
            //{
            //    Name = "",
            //    AltName = "",
            //    //Description = "This is an item description."
            //};

            //viewModel = new ItemDetailViewModel(item);
            //BindingContext = viewModel;
        }

        void PaintHeaderBar(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            SKPath path = new SKPath
            {
                FillType = SKPathFillType.EvenOdd
            };
            path.AddRect(new SKRect(0, 0, info.Width, 8));

            SKPaint paint = new SKPaint()
            {
                Style = SKPaintStyle.Fill,
                Color = new SKColor(0xFFE69A28)
            };

            canvas.DrawPath(path, paint);

            paint.Style = SKPaintStyle.Stroke;
            paint.StrokeWidth = 2;
            paint.Color = SKColors.Black;

            canvas.DrawPath(path, paint);
            paint.Dispose();
            path.Dispose();
        }

        void PaintRedBar(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            SKPath path = new SKPath
            {
                FillType = SKPathFillType.EvenOdd
            };
            path.AddPoly(new SKPoint[] { new SKPoint(0,0), new SKPoint(info.Width, 8), new SKPoint(0, 8) });

            SKPaint paint = new SKPaint()
            {
                Style = SKPaintStyle.Fill,
                Color = new SKColor(0xFF922610)
            };

            canvas.DrawPath(path, paint);
            paint.Dispose();
            path.Dispose();

            //paint.Style = SKPaintStyle.Stroke;
            //paint.StrokeWidth = 2;
            //paint.Color = SKColors.Black;

            //canvas.DrawPath(path, paint);
        }
    }
}