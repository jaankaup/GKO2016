using System;
using System.Windows.Data;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mylly
{
    /// <summary>
    /// Janne Kauppinen 2017.
    /// Copyrights: None.
    /// 
    /// Multivalueconverteri, joka nyt olettaa saavansa 4 double arvoa. Ensimmäinen on jonkun frameworkelementin leveys ja sitten vastaava korkeus. Seuraavat kaksi arvoa on jonkun toisen 
    /// frameworkelementin leveys ja korkeus. Tämän jälkeen converteri palauttaa TranslateTransformin, joka siis kertoo sen miten tulee siirtyä, jotta ensimmäisen objecti on keskitetty 
    /// jälimmäisen objectin keskelle. Toiseen suuntaan ei ole mitään toteutusta.
    /// </summary>
    public class TranslateConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {

            if (values.Length != 4) throw new Exception("TranslateConverter:Convert: Converteriin ei annettu neljää objectia.");

            // Luotetaan siihen, että annetut valuet todellakin ovat doubleja.
            double sourceWidth = (double)values[0];
            double sourceHeight = (double)values[1];
            double targetWidth = (double)values[2];
            double targetHeight = (double)values[3];

            // Huimaa lineaarialgebraa. Selitys HT.
            var X = (-1) * sourceWidth / 2.0 + targetWidth / 2.0;
            var Y = (-1) * sourceHeight / 2.0 + targetHeight / 2.0;
            return new TranslateTransform(X, Y);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
