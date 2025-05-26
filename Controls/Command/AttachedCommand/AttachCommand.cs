namespace Controls.Command;


public class AttachCommand
{

    public static FreezableCollection<AttachObject> GetAttachTarget(DependencyObject obj)
    {
        if (obj.GetValue(AttachTargetProperty) is not FreezableCollection<AttachObject> list)
        {
            list = new FreezableCollection<AttachObject>();
            obj.SetValue(AttachTargetProperty, list);
        }
        return list;
    }

    private static readonly DependencyProperty AttachTargetProperty = DependencyProperty.RegisterAttached(
        "DarkAttachTarget", typeof(FreezableCollection<AttachObject>), typeof(AttachCommand), new PropertyMetadata(null, new PropertyChangedCallback(OnValueChanged)));

    private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (e.OldValue != null)
        {
            var list = e.OldValue as FreezableCollection<AttachObject>;

            if (list?.Count > 0)
            {
                foreach (var item in list)
                {
                    item.OnDeach();
                }
            }

        }

        if (e.NewValue != null)
        {
            var list = e.NewValue as FreezableCollection<AttachObject>;

            if (list?.Count > 0)
            {
                foreach (var item in list)
                {
                    if (!string.IsNullOrWhiteSpace(item.AttachEventName))
                    {
                        if (item.AttachObjectTarget == null)
                        {
                            item.AttachObjectTarget = d;
                            item.RunAction();
                        }
                    }
                }
            }
        }

    }
}