using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace WpfApp8
{
    static class Ext
    {
        /// <summary>
        /// Получить корневой визуальный элемент.
        /// </summary>
        /// <param name="visual">Контрол в окне.</param>
        /// <returns>Корневой визуальный элемент или <c>null</c>.</returns>
        public static UIElement FindRoot(this DependencyObject visual)
        {
            var parentWindow = Window.GetWindow(visual);
            var rootElement = parentWindow?.Content as UIElement;
            if (rootElement == null)
            {
                if (Application.Current != null && Application.Current.MainWindow != null)
                {
                    rootElement = Application.Current.MainWindow.Content as UIElement;
                }
                if (rootElement == null)
                {
                    rootElement = visual.GetVisualAncestor<Page>(null) ?? visual.GetVisualAncestor<UserControl>(null) as UIElement;
                }
            }

            return rootElement;
        }

        public static T GetVisualAncestor<T>(this DependencyObject d, Func<T, bool> filter) where T : class
        {
            for (DependencyObject parent = VisualTreeHelper.GetParent(d.FindVisualTreeRoot()); parent != null; parent = VisualTreeHelper.GetParent(parent))
            {
                T obj = parent as T;
                if ((object)obj != null && (filter == null || filter(obj)))
                    return obj;
            }
            return default(T);
        }

        internal static DependencyObject FindVisualTreeRoot(this DependencyObject d)
        {
            DependencyObject current = d;
            DependencyObject dependencyObject = d;
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