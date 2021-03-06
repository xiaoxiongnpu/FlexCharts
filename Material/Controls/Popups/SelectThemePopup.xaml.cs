﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using FlexCharts.Extensions;
using FlexCharts.MaterialDesign;
using FlexCharts.Require;
using Material.Static;

namespace Material.Controls.Popups
{
	/// <summary>
	/// Interaction logic for SelectThemePopup.xaml
	/// </summary>
	public partial class SelectThemePopup
	{
		public event ThemeSelectedHandler ThemeSelected;
		public delegate void ThemeSelectedHandler(AccentedMaterialSet e);
		public void RaiseThemeSelectedEvent(AccentedMaterialSet e)
		{
			ThemeSelected?.Invoke(e);
		}

		private readonly ReadOnlyCollection<AccentedMaterialSet> themeSource = Palette.RecommendedThemeSets;
		protected UniformGrid PART_bubbles;
		protected Grid ROOT_explosion;
		private AccentedMaterialSet lastSelectedTheme;

		static SelectThemePopup()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(SelectThemePopup), new FrameworkPropertyMetadata(typeof(SelectThemePopup)));
			 
		}
		public SelectThemePopup()
		{
			InitializeComponent();
			Loaded += OnLoaded;
		}

		private void OnLoaded(object s, RoutedEventArgs e)
		{
			ROOT_explosion = getRootLogicalOverlay();
		}


		public SelectThemePopup(ThemeSelectedHandler callback) : this()
		{
			ThemeSelected += callback;
		}
		#region Overrided Methods
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			PART_bubbles = GetTemplateChild<UniformGrid>(nameof(PART_bubbles));
			var dt = new DispatcherTimer {Interval = TimeSpan.FromMilliseconds(300)};
			dt.Tick += (Sender, Args) =>
			{
				dt.Stop();
				loadBubbles();
			};
			dt.Start();
		}

		private void loadBubbles()
		{
			PART_bubbles.Children.Clear();
			var animationSkew = 0;
			foreach (var theme in themeSource)
			{
				var scaleTransfrom = new ScaleTransform(0, 0, .5, .5);
				var bubbles = new Ellipse
				{
					Width = 50,
					Height = 50,
					Tag = theme,
					Fill = theme.P500,
					Effect = MaterialPalette.Shadows.ShadowDelta2,
					RenderTransformOrigin = new Point(.5, .5),
					RenderTransform = scaleTransfrom
				};
				bubbles.MouseUp += bubbleClicked;
				PART_bubbles.Children.Add(bubbles);
				scaleTransfrom.animate(ScaleTransform.ScaleXProperty, 400, 1, animationSkew, new BackEase
				{ EasingMode = EasingMode.EaseOut, Amplitude = .6 });
				scaleTransfrom.animate(ScaleTransform.ScaleYProperty, 400, 1, animationSkew, new BackEase
				{ EasingMode = EasingMode.EaseOut, Amplitude = .6 });
				animationSkew += 10;
				//new SineEase {EasingMode = EasingMode.EaseIn});
				//scaleTransfrom.BeginAnimation(ScaleTransform.ScaleXProperty, new DoubleAnimation())
			}

		}

		private void closePopup(object s, RoutedEventArgs e)
		{
			animateRemoveBubbles();
			
		}

		private void animateRemoveBubbles()
		{
			var animationSkew = 0;
			foreach (var bubble in PART_bubbles.Children)
			{
				bubble.RequireType<Ellipse>().RenderTransform.animate(ScaleTransform.ScaleXProperty, 400, 0, animationSkew, new BackEase
					{  EasingMode = EasingMode.EaseIn, Amplitude = .6});
				bubble.RequireType<Ellipse>().RenderTransform.animate(ScaleTransform.ScaleYProperty, 400, 0, animationSkew, new BackEase
					{ EasingMode = EasingMode.EaseIn, Amplitude = .6});
				animationSkew += 10;
				//new SineEase {EasingMode = EasingMode.EaseIn});
				//scaleTransfrom.BeginAnimation(ScaleTransform.ScaleXProperty, new DoubleAnimation())
			}
			var dt = new DispatcherTimer {Interval = TimeSpan.FromMilliseconds(400 + animationSkew)};
			dt.Tick += (Sender, Args) =>
			{
				dt.Stop();
				RaiseEvent(new RoutedEventArgs(PopupRequestCloseEvent));
			};
			dt.Start();

		}

		private void bubbleClicked(object s, MouseButtonEventArgs e)
		{
			var shellSize = ROOT_explosion.RenderSize.Largest() * 2;
			
			var bubble = s.RequireType<Ellipse>();
			lastSelectedTheme = bubble.Tag.RequireType<AccentedMaterialSet>();

			var cursor = Mouse.GetPosition(ROOT_explosion);
			var explosionBubble = new Ellipse()
			{
				Width = 1,
				Height = 1,
				Fill = bubble.Fill,
				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Top,
				//RenderTransformOrigin = new Point(.5, .5),
				Margin = new Thickness(cursor.X, cursor.Y, 0, 0),
				RenderTransform = new ScaleTransform(1, 1, .5, .5)
			};
			ROOT_explosion.Children.Add(explosionBubble);

			//bubble.RenderTransform.animate(ScaleTransform.ScaleXProperty, 400, 1, 0, new BackEase
			//		{ EasingMode = EasingMode.EaseInOut, Amplitude = .3}, by:1.2);
			//bubble.RenderTransform.animate(ScaleTransform.ScaleYProperty, 400, 1, 0, new BackEase
			//		{ EasingMode = EasingMode.EaseInOut, Amplitude = .3}, by:1.2);

			explosionBubble.RenderTransform.animate(ScaleTransform.ScaleXProperty, 400, shellSize, 0, new BackEase
					{ EasingMode = EasingMode.EaseInOut, Amplitude = .3});
			explosionBubble.RenderTransform.animate(ScaleTransform.ScaleYProperty, 400, shellSize , 0, new BackEase
					{ EasingMode = EasingMode.EaseInOut, Amplitude = .3}, OnExplode);
			explosionBubble.animate(OpacityProperty, 200, 0, 500, new CubicEase
					{ EasingMode = EasingMode.EaseIn}, OnExplodeComplete);
		}

		private void OnExplode(object s, EventArgs a)
		{
			RaiseThemeSelectedEvent(lastSelectedTheme);
		}

		private Grid getRootLogicalOverlay()
		{
			DependencyObject recursiveParent = this;
			while (!(recursiveParent is MaterialShell))
			{
				recursiveParent = LogicalTreeHelper.GetParent(recursiveParent);
			}
			return ((MaterialShell) recursiveParent).GetShellOverlayRoot;
		}
		private void OnExplodeComplete(object s, EventArgs e)
		{
			ROOT_explosion.Children.Clear();
			//PART_explosion.Children.Clear();
		}

		#endregion
	}
}

		