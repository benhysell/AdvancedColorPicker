/*
 * This code is licensed under the terms of the MIT license
 *
 * Copyright (C) 2012 Yiannis Bourkelis
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy 
 * of this software and associated documentation files (the "Software"), to deal 
 * in the Software without restriction, including without limitation the rights 
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
 * copies of the Software, and to permit persons to whom the Software is furnished
 * to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all 
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
 * PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION 
 * OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE 
 * SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System;
using MonoTouch.UIKit;
using System.Drawing;

namespace AdvancedColorPicker
{
	public class ColorPickerViewController : UIViewController
	{
		public event Action ColorPicked;

		SizeF satBrightIndicatorSize = new SizeF(28,28);
		HuePickerView huePickerView = new HuePickerView();
		SaturationBrightnessPickerView saturationBrightnessPickerView = new SaturationBrightnessPickerView();
		//SelectedColorPreviewView selectedColorPreviewView = new SelectedColorPreviewView();
		HueIndicatorView huewIndicatorView = new HueIndicatorView();
		SatBrightIndicatorView satBrightIndicatorView = new SatBrightIndicatorView();
		RectangleF frame;

		public ColorPickerViewController (RectangleF frame)
		{
			saturationBrightnessPickerView.Hue = .5984375f;
			saturationBrightnessPickerView.Saturation = .5f;
			saturationBrightnessPickerView.Brightness = .7482993f;
			huePickerView.Hue = saturationBrightnessPickerView.Hue;
			this.frame = frame;
			//selectedColorPreviewView.BackgroundColor = UIColor.FromHSB(saturationBrightnessPickerView.hue,saturationBrightnessPickerView.saturation,saturationBrightnessPickerView.brightness);
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			float selectedColorViewHeight = 60;

			float viewSpace = 1;

//			selectedColorPreviewView.Frame = new RectangleF(0,0,this.View.Bounds.Width,selectedColorViewHeight);
//			selectedColorPreviewView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
//			selectedColorPreviewView.Layer.ShadowOpacity = 0.6f;
//			selectedColorPreviewView.Layer.ShadowOffset = new SizeF(0,7);
//			selectedColorPreviewView.Layer.ShadowColor = UIColor.Black.CGColor;


			//to megalo view epilogis apoxrwsis tou epilegmenou xrwmats
			saturationBrightnessPickerView.Frame = new RectangleF(0,selectedColorViewHeight, frame.Width, frame.Bottom - selectedColorViewHeight);
			saturationBrightnessPickerView.ColorPicked += HandleColorPicked;
			saturationBrightnessPickerView.AutosizesSubviews = true;

			//to mikro view me ola ta xrwmata
			//huePickerView.Frame = new RectangleF(0, this.View.Bounds.Bottom - selectedColorViewHeight, this.View.Bounds.Width, selectedColorViewHeight);
			huePickerView.Frame = new RectangleF(0, 0, frame.Width, selectedColorViewHeight);
			huePickerView.HueChanged += HandleHueChanged;

			huewIndicatorView.huePickerViewRef = huePickerView;
			float pos = huePickerView.Frame.Width * huePickerView.Hue;
			huewIndicatorView.Frame = new RectangleF(pos - 10,huePickerView.Bounds.Y - 2,20,huePickerView.Bounds.Height + 2);
			huewIndicatorView.UserInteractionEnabled = false;
			huePickerView.AddSubview(huewIndicatorView);

			satBrightIndicatorView.satBrightPickerViewRef = saturationBrightnessPickerView;
			PointF pos2 = new PointF(saturationBrightnessPickerView.Saturation * saturationBrightnessPickerView.Frame.Size.Width, 
			                         saturationBrightnessPickerView.Frame.Size.Height - (saturationBrightnessPickerView.Brightness * saturationBrightnessPickerView.Frame.Size.Height));
			satBrightIndicatorView.Frame = new RectangleF(pos2.X - satBrightIndicatorSize.Width/2,pos2.Y-satBrightIndicatorSize.Height/2,satBrightIndicatorSize.Width,satBrightIndicatorSize.Height);
			satBrightIndicatorView.UserInteractionEnabled = false;
			saturationBrightnessPickerView.AddSubview(satBrightIndicatorView);


			this.View.AddSubviews(saturationBrightnessPickerView, huePickerView);
		}

		public override void ViewDidLayoutSubviews ()
		{
			base.ViewDidLayoutSubviews ();
			PositionIndicators();
		}

		void PositionIndicators()
		{
			PositionHueIndicatorView();
			PositionSatBrightIndicatorView();
		}

		void PositionSatBrightIndicatorView ()
		{
			UIView.Animate(0.3f,0f,UIViewAnimationOptions.AllowUserInteraction, delegate() {
				PointF pos = new PointF(saturationBrightnessPickerView.Saturation * saturationBrightnessPickerView.Frame.Size.Width, 
				                        saturationBrightnessPickerView.Frame.Size.Height - (saturationBrightnessPickerView.Brightness * saturationBrightnessPickerView.Frame.Size.Height));
				satBrightIndicatorView.Frame = new RectangleF(pos.X - satBrightIndicatorSize.Width/2,pos.Y-satBrightIndicatorSize.Height/2,satBrightIndicatorSize.Width,satBrightIndicatorSize.Height);
			}, delegate() {
			});
		}

		void PositionHueIndicatorView ()
		{
			UIView.Animate(0.3f,0f,UIViewAnimationOptions.AllowUserInteraction, delegate() {
				float pos = huePickerView.Frame.Width * huePickerView.Hue;
				huewIndicatorView.Frame = new RectangleF(pos - 10,huePickerView.Bounds.Y - 2,20,huePickerView.Bounds.Height + 2);
			}, delegate() {
				huewIndicatorView.Hidden = false;
		});
		}

		void HandleColorPicked ()
		{
			PositionSatBrightIndicatorView ();
			//selectedColorPreviewView.BackgroundColor = UIColor.FromHSB (saturationBrightnessPickerView.hue, saturationBrightnessPickerView.saturation, saturationBrightnessPickerView.brightness);

			if (ColorPicked != null) {
				ColorPicked();
			}
		}

		void HandleHueChanged ()
		{
			PositionHueIndicatorView();
			saturationBrightnessPickerView.Hue = huePickerView.Hue;
			saturationBrightnessPickerView.SetNeedsDisplay();
			HandleColorPicked();
		}

		public UIColor SelectedColor {
			get {
				return UIColor.FromHSB(saturationBrightnessPickerView.Hue,saturationBrightnessPickerView.Saturation,saturationBrightnessPickerView.Brightness);
			}
			set {
				float hue = 0,brightness = 0,saturation = 0,alpha = 0;
				value.GetHSBA(out hue,out saturation,out brightness,out alpha);
				huePickerView.Hue = hue;
				saturationBrightnessPickerView.Hue = hue;
				saturationBrightnessPickerView.Brightness = brightness;
				saturationBrightnessPickerView.Saturation = saturation;
				//selectedColorPreviewView.BackgroundColor = value;

				PositionIndicators();

				saturationBrightnessPickerView.SetNeedsDisplay();
				huePickerView.SetNeedsDisplay();
			}
		}

		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations ()
		{
			return UIInterfaceOrientationMask.All;
		}

		public override void DidRotate (UIInterfaceOrientation fromInterfaceOrientation)
		{
			base.DidRotate (fromInterfaceOrientation);
			if (UIDevice.CurrentDevice.SystemVersion.StartsWith("4.")){
				PositionIndicators();
			}
		}

		public override bool ShouldAutorotate ()
		{
			return true;
		} 
	}
}

