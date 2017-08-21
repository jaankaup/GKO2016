using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace Mylly
{
    /// <summary>
    /// Janne Kauppinen 2017.
    /// Copyrights: None.
    /// Oma canvas luokka, jossa on lisätty skaalautuvuuteen/positiointiin liittyä propertyjä.
    /// </summary>
    class MyCanvas : Canvas
    {
        public MyCanvas()
        {
            HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            SizeChanged += (s, e) => { HalfWidth = ActualWidth / 2.0; HalfHeight = ActualHeight / 2.0; };

            // Lisätään tapahtuma: Päivitetään skaalauskertoimen mukaisesti skaalattu leveys ja korkeus.
            // Lisäksi päivitetään translaatio arvot, joita käytetään objectin piirtamiseen keskelle canvasta.
            SizeChanged += (s, e) => { ScaleWidth = ActualWidth * ScaleFactor;
                                       ScaleHeight = ActualHeight * ScaleFactor;
                                       TranslateX = (-1) * ScaleFactor * ScaleWidth;
                                       TranslateY = (-1) * ScaleFactor * ScaleHeight;
                /*
                TranslateX = (-1) * (0.5) * ScaleWidth;
                TranslateY = (-1) * (0.5) * ScaleHeight;
                */
            };
        }

        public double HalfWidth
        {
            get { return (double)GetValue(HalfWidthProperty); }
            set { SetValue(HalfWidthProperty, value); }
        }

        public double HalfHeight
        {
            get { return (double)GetValue(HalfHeightProperty); }
            set { SetValue(HalfHeightProperty, value); }
        }

        public static readonly DependencyProperty HalfWidthProperty =
            DependencyProperty.Register("HalfWidth", typeof(double), typeof(MyCanvas), new PropertyMetadata(0.0));

        public static readonly DependencyProperty HalfHeightProperty =
            DependencyProperty.Register("HalfHeight", typeof(double), typeof(MyCanvas), new PropertyMetadata(0.0));


        /// <summary>
        /// Dependency property, joka pitää sisällään skaalauskertoimen. Skaalauskerrointa voi käyttää 
        /// esim. objectien piirtämiseen, animaatioon jne.
        /// </summary>
        public double ScaleFactor
        {
            get { return (double)GetValue(ScaleFactorProperty); }
            set { SetValue(ScaleFactorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ScaleFactor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScaleFactorProperty =
            DependencyProperty.Register("ScaleFactor", typeof(double), typeof(MyCanvas), new PropertyMetadata(0.5));

        /// <summary>
        /// Dependency property skaalatulle leveydelle.
        /// </summary>
        public double ScaleWidth
        {
            get { return (double)GetValue(ScaleWidthProperty); }
            set { SetValue(ScaleWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ScaleWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScaleWidthProperty =
            DependencyProperty.Register("ScaleWidth", typeof(double), typeof(MyCanvas), new PropertyMetadata(0.0));


        /// <summary>
        /// Dependency property skaalatulle korkeudelle.
        /// </summary>
        public double ScaleHeight
        {
            get { return (double)GetValue(ScaleHeightProperty); }
            set { SetValue(ScaleHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ScaleHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScaleHeightProperty =
            DependencyProperty.Register("ScaleHeight", typeof(double), typeof(MyCanvas), new PropertyMetadata(0.0));

        
        /// <summary>
        /// Dependency property, jota voidaan käyttää translatetransform x-muunnoksena, jotta objecti saadaan piirrettyä 
        /// keskelle canvasta.
        /// </summary>
        public double TranslateX
        {
            get { return (double)GetValue(TranslateXProperty); }
            set { SetValue(TranslateXProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TranslateX.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TranslateXProperty =
            DependencyProperty.Register(
                "TranslateX",
                typeof(double),
                typeof(MyCanvas),
                new FrameworkPropertyMetadata(0.0));

        /// <summary>
        /// Dependency property, jota voidaan käyttää translatetransform y-muunnoksena, jotta objekti saadaan piirrettyä 
        /// keskelle canvasta.
        /// </summary>
        public double TranslateY
        {
            get { return (double)GetValue(TranslateYProperty); }
            set { SetValue(TranslateYProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TranslateY.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TranslateYProperty =
            DependencyProperty.Register("TranslateY", typeof(double), typeof(MyCanvas), new PropertyMetadata(0.0));
    }
}
