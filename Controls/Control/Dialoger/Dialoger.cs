namespace Controls.Control;


internal class Dialoger : ContentControl
{
    private BaseAdorner _container;

    private static Action<bool>? _action;

    public Dialoger()
    {
        CommandBindings.Add(new CommandBinding(ControlCommands.Close, (s, e) => { Close(); _action?.Invoke(false); }));

        CommandBindings.Add(new CommandBinding(ControlCommands.Confirm, (s, e) => { Close(); _action?.Invoke(true); }));
    }


    /// <summary>
    ///  
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public static void Show(object content, Window? win = null)
    {
        _action = null;

        var dialog = new Dialoger { Content = content };

        win ??= API_Window.ActiveWindow ?? (API_Window.MainWindow.IsVisible ? API_Window.MainWindow : API_Window.VisualWindow);

        if (win != null)
        {
            var decorator = Util_Visual.FindChild<AdornerDecorator>(win);

            if (decorator != null)
            {
                if (decorator.Child != null)
                {
                    decorator.Child.IsEnabled = false;
                    decorator.Child.Opacity = 0.6;
                }

                var layer = decorator.AdornerLayer;

                if (layer != null)
                {
                    var container = new BaseAdorner(layer) { Child = dialog };
                    dialog._container = container;

                    layer.Add(container);

                    dialog.Focus();
                }
            }
        }
    }


    public static void Show(object content, Action<bool> action)
    {
        Show(content);

        _action = action;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="element"></param>
    private void Close()
    {
        if (_container != null)
        {
            var decorator = Util_Visual.FindParent<AdornerDecorator>(_container);

            if (decorator != null)
            {
                if (decorator.Child != null)
                {
                    decorator.Child.IsEnabled = true;
                    decorator.Child.Opacity = 1;
                }
                var layer = decorator.AdornerLayer;
                layer?.Remove(_container);

                _container = null;
            }
        }

    }

}
