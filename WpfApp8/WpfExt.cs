using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace GridSplitterSizesDemo
{
    internal static class WpfExt
    {
        internal static UIElement FindRoot(this DependencyObject visual)
        {
            var parentWindow = Window.GetWindow(visual);
            var rootElement = parentWindow?.Content as UIElement;
            if (rootElement == null)
            {
                if (Application.Current != null && Application.Current.MainWindow != null)
                    rootElement = Application.Current.MainWindow.Content as UIElement;

                if (rootElement == null)
                    rootElement = visual.GetVisualAncestor<Page>(null) ??
                                  visual.GetVisualAncestor<UserControl>(null) as UIElement;
            }

            return rootElement;
        }

        internal static T GetVisualAncestor<T>(this DependencyObject d, Func<T, bool> filter) where T : class
        {
            for (var parent = VisualTreeHelper.GetParent(d.FindVisualTreeRoot());
                parent != null;
                parent = VisualTreeHelper.GetParent(parent))
                if (parent is T obj && (filter == null || filter(obj)))
                    return obj;

            return default;
        }

        internal static T GetVisualAncestor<T>(this DependencyObject d) where T : class
        {
            return GetVisualAncestor<T>(d, null);
        }

        internal static DependencyObject FindVisualTreeRoot(this DependencyObject d)
        {
            var current = d;
            var dependencyObject = d;
            for (; current != null; current = LogicalTreeHelper.GetParent(current))
            {
                dependencyObject = current;
                if (current is Visual || current is Visual3D)
                    break;
            }

            return dependencyObject;
        }
    }
}