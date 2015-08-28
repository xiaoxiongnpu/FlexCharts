﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FlexCharts.Helpers.DependencyHelpers;
using FlexCharts.MaterialDesign;
using FlexCharts.MaterialDesign.Descriptors;

namespace FlexCharts.Controls.Primatives
{
	public interface IDotContract
	{
		double DotRadius { get; set; }
		AbstractMaterialDescriptor DotFill { get; set; }
		AbstractMaterialDescriptor DotStroke { get; set; }
		double DotStrokeThickness { get; set; }
	}
	public abstract class DotPrimative : FlexPrimative, IDotContract
	{
		public static readonly DependencyProperty DotRadiusProperty =
			DP.Attach<double>(typeof (DotPrimative), new FrameworkPropertyMetadata(5.0), IsValidDotRadius);

		public static readonly DependencyProperty DotFillProperty = 
			DP.Attach<AbstractMaterialDescriptor>(typeof(DotPrimative), new FrameworkPropertyMetadata(MaterialPalette.Descriptors.P300Descriptor));

		public static readonly DependencyProperty DotStrokeProperty = 
			DP.Attach<AbstractMaterialDescriptor>(typeof(DotPrimative), new FrameworkPropertyMetadata(MaterialPalette.Descriptors.P900Descriptor));

		public static readonly DependencyProperty DotStrokeThicknessProperty = 
			DP.Attach<double>(typeof(DotPrimative), new FrameworkPropertyMetadata(2.0));


		public static double GetDotRadius(DependencyObject i) => i.Get<double>(DotRadiusProperty);
		public static void SetDotRadius(DependencyObject i, double v) => i.Set(DotRadiusProperty, v);
		public double DotRadius
		{
			get { return (double) GetValue(DotRadiusProperty); }
			set { SetValue(DotRadiusProperty, value); }
		}
		
		public static AbstractMaterialDescriptor GetDotFill(DependencyObject i) => i.Get<AbstractMaterialDescriptor>(DotFillProperty);
		public static void SetDotFill(DependencyObject i, AbstractMaterialDescriptor v) => i.Set(DotFillProperty, v);
		public AbstractMaterialDescriptor DotFill
		{
			get { return (AbstractMaterialDescriptor) GetValue(DotFillProperty); }
			set { SetValue(DotFillProperty, value); }
		}

		public static AbstractMaterialDescriptor GetDotStroke(DependencyObject i) => i.Get<AbstractMaterialDescriptor>(DotStrokeProperty);
		public static void SetDotStroke(DependencyObject i, AbstractMaterialDescriptor v) => i.Set(DotStrokeProperty, v);
		public AbstractMaterialDescriptor DotStroke
		{
			get { return (AbstractMaterialDescriptor) GetValue(DotStrokeProperty); }
			set { SetValue(DotStrokeProperty, value); }
		}

		public static double GetDotStrokeThickness(DependencyObject i) => i.Get<double>(DotStrokeThicknessProperty);
		public static void SetDotStrokeThickness(DependencyObject i, double v) => i.Set(DotStrokeThicknessProperty, v);
		public double DotStrokeThickness
		{
			get { return (double) GetValue(DotStrokeThicknessProperty); }
			set { SetValue(DotStrokeThicknessProperty, value); }
		}


		public static bool IsValidDotRadius(double i)
		{
			return i >= 0;
		}

	}
}