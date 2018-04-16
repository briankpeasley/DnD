using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;

namespace MapReveal
{
    public static class Fill
    {
        public static void FillInkCanvas(InkCanvas ink, Color color)
        {
            ink.Strokes.Clear();
            StylusPointCollection collection = new StylusPointCollection();
            DrawingAttributes atts = new DrawingAttributes();
            atts.Color = color;
            
            for (double i = 0; i < ink.ActualHeight; i += ink.DefaultDrawingAttributes.Height)
            {
                if (i % 2 == 0)
                {
                    collection.Add(new StylusPoint(0, i));
                    collection.Add(new StylusPoint(ink.ActualWidth, i));
                }
                else
                {
                    collection.Add(new StylusPoint(ink.ActualWidth, i));
                    collection.Add(new StylusPoint(0, i));
                }
            }
            
            ink.Strokes.Add(new Stroke(collection, atts));
        }
    }
}
