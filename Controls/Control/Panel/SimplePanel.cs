namespace Controls.Control;

public class SimplePanel : System.Windows.Controls.Panel
{
    protected override System.Windows.Size MeasureOverride(System.Windows.Size constraint)
    {
        var maxSize = new System.Windows.Size();

        foreach (UIElement child in InternalChildren)
        {
            if (child != null)
            {
                child.Measure(constraint);
                maxSize.Width = Math.Max(maxSize.Width, child.DesiredSize.Width);
                maxSize.Height = Math.Max(maxSize.Height, child.DesiredSize.Height);
            }
        }

        return maxSize;
    }

    protected override System.Windows.Size ArrangeOverride(System.Windows.Size arrangeSize)
    {
        foreach (UIElement child in InternalChildren)
        {
            child?.Arrange(new Rect(arrangeSize));
        }

        return arrangeSize;
    }
}