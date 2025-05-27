using Size = System.Windows.Size;
namespace Controls.Control;

public class FrameworkElementAdorner : Adorner
{
    //
    // The framework element that is the adorner. 
    //
    private readonly FrameworkElement _child;

    //
    // Placement of the child.
    //
    private readonly AdornerPlacement _horizontalAdornerPlacement = AdornerPlacement.Inside;
    private readonly AdornerPlacement _verticalAdornerPlacement = AdornerPlacement.Inside;

    //
    // Offset of the child.
    //
    private readonly double _offsetX = 0.0;
    private readonly double _offsetY = 0.0;

    //
    // Position of the _child (when not set to NaN).
    //

    public FrameworkElementAdorner(FrameworkElement adornerChildElement, FrameworkElement adornedElement)
        : base(adornedElement)
    {
        this._child = adornerChildElement;

        base.AddLogicalChild(adornerChildElement);
        base.AddVisualChild(adornerChildElement);
    }

    public FrameworkElementAdorner(FrameworkElement adornerChildElement, FrameworkElement adornedElement,
                                   AdornerPlacement horizontalAdornerPlacement, AdornerPlacement verticalAdornerPlacement,
                                   double offsetX, double offsetY)
        : base(adornedElement)
    {
        this._child = adornerChildElement;
        this._horizontalAdornerPlacement = horizontalAdornerPlacement;
        this._verticalAdornerPlacement = verticalAdornerPlacement;
        this._offsetX = offsetX;
        this._offsetY = offsetY;

        adornedElement.SizeChanged += new SizeChangedEventHandler(AdornedElement_SizeChanged);

        base.AddLogicalChild(adornerChildElement);
        base.AddVisualChild(adornerChildElement);
    }

    /// <summary>
    /// Event raised when the adorned control's size has changed.
    /// </summary>
    private void AdornedElement_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        InvalidateMeasure();
    }

    //
    // Position of the _child (when not set to NaN).
    //
    public double PositionX { get => positionX; set { positionX = value; } }
    private double positionX = Double.NaN;

    // Position of the _child (when not set to NaN).
    //
    public double PositionY { get => positionY; set { positionY = value; } }
    private double positionY = Double.NaN;

    protected override Size MeasureOverride(Size constraint)
    {
        this._child.Measure(constraint);
        return this._child.DesiredSize;
    }

    /// <summary>
    /// Determine the X coordinate of the _child.
    /// </summary>
    private double DetermineX()
    {
        switch (_child.HorizontalAlignment)
        {
            case HorizontalAlignment.Left:
                {
                    if (_horizontalAdornerPlacement == AdornerPlacement.Outside)
                    {
                        return -_child.DesiredSize.Width + _offsetX;
                    }
                    else
                    {
                        return _offsetX;
                    }
                }
            case HorizontalAlignment.Right:
                {
                    if (_horizontalAdornerPlacement == AdornerPlacement.Outside)
                    {
                        double adornedWidth = AdornedElement.ActualWidth;
                        return adornedWidth + _offsetX;
                    }
                    else
                    {
                        double adornerWidth = this._child.DesiredSize.Width;
                        double adornedWidth = AdornedElement.ActualWidth;
                        double x = adornedWidth - adornerWidth;
                        return x + _offsetX;
                    }
                }
            case HorizontalAlignment.Center:
                {
                    double adornerWidth = this._child.DesiredSize.Width;
                    double adornedWidth = AdornedElement.ActualWidth;
                    double x = (adornedWidth / 2) - (adornerWidth / 2);
                    return x + _offsetX;
                }
            case HorizontalAlignment.Stretch:
                {
                    return 0.0;
                }
        }

        return 0.0;
    }

    /// <summary>
    /// Determine the Y coordinate of the _child.
    /// </summary>
    private double DetermineY()
    {
        switch (_child.VerticalAlignment)
        {
            case VerticalAlignment.Top:
                {
                    if (_verticalAdornerPlacement == AdornerPlacement.Outside)
                    {
                        return -_child.DesiredSize.Height + _offsetY;
                    }
                    else
                    {
                        return _offsetY;
                    }
                }
            case VerticalAlignment.Bottom:
                {
                    if (_verticalAdornerPlacement == AdornerPlacement.Outside)
                    {
                        double adornedHeight = AdornedElement.ActualHeight;
                        return adornedHeight + _offsetY;
                    }
                    else
                    {
                        double adornerHeight = this._child.DesiredSize.Height;
                        double adornedHeight = AdornedElement.ActualHeight;
                        double x = adornedHeight - adornerHeight;
                        return x + _offsetY;
                    }
                }
            case VerticalAlignment.Center:
                {
                    double adornerHeight = this._child.DesiredSize.Height;
                    double adornedHeight = AdornedElement.ActualHeight;
                    double x = (adornedHeight / 2) - (adornerHeight / 2);
                    return x + _offsetY;
                }
            case VerticalAlignment.Stretch:
                {
                    return 0.0;
                }
        }

        return 0.0;
    }

    /// <summary>
    /// Determine the width of the _child.
    /// </summary>
    private double DetermineWidth()
    {
        if (!Double.IsNaN(PositionX))
        {
            return this._child.DesiredSize.Width;
        }

        return _child.HorizontalAlignment switch
        {
            HorizontalAlignment.Left => this._child.DesiredSize.Width,
            HorizontalAlignment.Right => this._child.DesiredSize.Width,
            HorizontalAlignment.Center => this._child.DesiredSize.Width,
            HorizontalAlignment.Stretch => this.AdornedElement.ActualWidth,
            _ => 0.0,
        };
    }
    /// <summary>
    /// Determine the height of the _child.
    /// </summary>
    private double DetermineHeight()
    {
        if (!Double.IsNaN(PositionY))
        {
            return this._child.DesiredSize.Height;
        }

        return _child.VerticalAlignment switch
        {
            VerticalAlignment.Top => this._child.DesiredSize.Height,
            VerticalAlignment.Bottom => this._child.DesiredSize.Height,
            VerticalAlignment.Center => this._child.DesiredSize.Height,
            VerticalAlignment.Stretch => this.AdornedElement.ActualHeight,
            _ => 0.0,
        };
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        double x = PositionX;
        if (Double.IsNaN(x))
        {
            x = DetermineX();
        }
        double y = PositionY;
        if (Double.IsNaN(y))
        {
            y = DetermineY();
        }
        double adornerWidth = DetermineWidth();
        double adornerHeight = DetermineHeight();
        this._child.Arrange(new Rect(x, y, adornerWidth, adornerHeight));
        return finalSize;
    }

    protected override Int32 VisualChildrenCount => 1;


    protected override Visual GetVisualChild(Int32 index) => this._child;

    protected override IEnumerator LogicalChildren => (IEnumerator)new ArrayList() { this._child }.GetEnumerator();

    /// <summary>
    /// Disconnect the _child element from the visual tree so that it may be reused later.
    /// </summary>
    public void DisconnectChild()
    {
        base.RemoveLogicalChild(_child);
        base.RemoveVisualChild(_child);
    }

    /// <summary>
    /// Override AdornedElement from base class for less type-checking.
    /// </summary>
    public new FrameworkElement AdornedElement => (FrameworkElement)base.AdornedElement;
}


public enum AdornerPlacement
{
    Inside,
    Outside
}