namespace Controls.Control;
public class BaseAdorner : Adorner
{
    private UIElement? _child;

    public BaseAdorner(UIElement adornedElement) : base(adornedElement)
    {
    }

    public UIElement? Child
    {
        get => _child;
        set
        {
            if (value == null)
            {
                RemoveVisualChild(_child);
                // ReSharper disable once ExpressionIsAlwaysNull
                _child = value;
                return;
            }
            AddVisualChild(value);
            _child = value;
        }
    }

    protected override int VisualChildrenCount => _child != null ? 1 : 0;

    protected override System.Windows.Size ArrangeOverride(System.Windows.Size finalSize)
    {
        _child?.Arrange(new Rect(finalSize));
        return finalSize;
    }

    protected override System.Windows.Media.Visual GetVisualChild(int index)
    {
        if (index == 0 && _child != null) return _child;
        return base.GetVisualChild(index);
    }
}