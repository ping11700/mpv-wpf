namespace Controls.Utils;

public static class Util_Visual
{
    /// <summary>
    /// 获取父控件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static T? FindParent<T>(DependencyObject obj, string? name = null) where T : FrameworkElement
    {
        try
        {
            DependencyObject parent = VisualTreeHelper.GetParent(obj);
            while (parent != null)
            {
                if (parent is T t && (t.Name == name || string.IsNullOrEmpty(name)))
                {
                    return t;
                }
                parent = VisualTreeHelper.GetParent(parent);
            }
        }
        finally
        {

        }

        return null;
    }

    /// <summary>
    /// 获取子控件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static T? FindChild<T>(DependencyObject obj, string? name = null) where T : FrameworkElement
    {
        if (obj == null) return default;

        int childCount = VisualTreeHelper.GetChildrenCount(obj);
        for (int i = 0; i <= childCount - 1; i++)
        {
            DependencyObject child = VisualTreeHelper.GetChild(obj, i);
            if (child is T t && (t.Name == name || string.IsNullOrEmpty(name)))
            {
                return t;
            }
            else
            {
                T? grandChild = FindChild<T>(child, name);

                if (grandChild != null)
                    return grandChild;
            }
        }
        return null;
    }



    /// <summary>
    /// 获取子控件列表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static IEnumerable<T> FindChilds<T>(DependencyObject obj) where T : FrameworkElement
    {
        if (obj != null)
        {
            int childCount = VisualTreeHelper.GetChildrenCount(obj);

            for (int i = 0; i < childCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child is T t)
                {
                    yield return t;
                }
                if (child != null)
                {
                    foreach (T childOfChild in FindChilds<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }







    public static bool IsVerySmall(double value) => Math.Abs(value) < 1E-06;




    internal static Rect ElementToRoot(Rect rectElement, Visual element, PresentationSource presentationSource)
    {
        GeneralTransform transformElementToRoot = element.TransformToAncestor(presentationSource.RootVisual);
        Rect rectRoot = transformElementToRoot.TransformBounds(rectElement);

        return rectRoot;
    }

    internal static Rect RootToClient(Rect rectRoot, PresentationSource presentationSource)
    {
        CompositionTarget target = presentationSource.CompositionTarget;
        Matrix matrixRootTransform = GetVisualTransform(target.RootVisual);
        Rect rectRootUntransformed = Rect.Transform(rectRoot, matrixRootTransform);
        Matrix matrixDPI = target.TransformToDevice;
        Rect rectClient = Rect.Transform(rectRootUntransformed, matrixDPI);

        return rectClient;
    }

    private static Matrix GetVisualTransform(Visual v)
    {
        if (v != null)
        {
            Matrix m = Matrix.Identity;

            Transform transform = VisualTreeHelper.GetTransform(v);
            if (transform != null)
            {
                Matrix cm = transform.Value;
                m = Matrix.Multiply(m, cm);
            }

            Vector offset = VisualTreeHelper.GetOffset(v);
            m.Translate(offset.X, offset.Y);

            return m;
        }

        return Matrix.Identity;
    }


    /// <summary>
    ///     显示元素
    /// </summary>
    /// <param name="element"></param>
    public static void Show(this UIElement element) => element.Visibility = Visibility.Visible;

    /// <summary>
    ///     显示元素
    /// </summary>
    /// <param name="element"></param>
    /// <param name="show"></param>
    public static void Show(this UIElement element, bool show) => element.Visibility = show ? Visibility.Visible : Visibility.Collapsed;

    /// <summary>
    ///     不显示元素，且不保留空间
    /// </summary>
    /// <param name="element"></param>
    public static void Collapse(this UIElement element) => element.Visibility = Visibility.Collapsed;


    public static bool IsInDesignMode => (bool)DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty, typeof(FrameworkElement)).Metadata.DefaultValue;

}