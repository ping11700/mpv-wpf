namespace Controls.AttachedProperties;

/// <summary>
///  附加属性 Item
/// </summary>
public class Attach_Item
{
    /// <summary>
    /// Item Padding
    /// </summary>
    public static readonly DependencyProperty ItemPaddingProperty = DependencyProperty.RegisterAttached(
            "ItemPadding", typeof(Thickness), typeof(Attach_Item), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.Inherits));
    public static void SetItemPadding(DependencyObject element, Thickness value) => element.SetValue(ItemPaddingProperty, value);
    public static Thickness GetItemPadding(DependencyObject element) => (Thickness)element.GetValue(ItemPaddingProperty);




    /// <summary>
    ///  是否有Item
    /// </summary>
    public static readonly DependencyPropertyKey HasItemPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
           "HasItem", typeof(bool), typeof(Attach_Item), new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.Inherits));

    public static readonly DependencyProperty HasItemProperty = HasItemPropertyKey.DependencyProperty;

    public static void SetHasItem(DependencyObject element, bool value) => element.SetValue(HasItemProperty, ValueBoxes.BooleanBox(value));
    public static bool GetHasItem(DependencyObject element) => (bool)element.GetValue(HasItemProperty);


    /// <summary>
    /// Item Background
    /// </summary>
    public static readonly DependencyProperty ItemBackgroundProperty = DependencyProperty.RegisterAttached(
            "ItemBackground", typeof(System.Windows.Media.Brush), typeof(Attach_Item), new FrameworkPropertyMetadata(System.Windows.Media.Brushes.Transparent, FrameworkPropertyMetadataOptions.Inherits));
    public static void SetItemBackground(DependencyObject element, System.Windows.Media.Brush value) => element.SetValue(ItemBackgroundProperty, value);
    public static System.Windows.Media.Brush GetItemBackground(DependencyObject element) => (System.Windows.Media.Brush)element.GetValue(ItemBackgroundProperty);


    /// <summary>
    ///  ItemTemplate
    /// </summary>
    public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.RegisterAttached(
        "ItemTemplate", typeof(DataTemplate), typeof(Attach_Item), new PropertyMetadata(default));
    public static void SetItemTemplate(DependencyObject element, DataTemplate value) => element.SetValue(ItemTemplateProperty, value);
    public static DataTemplate GetItemTemplate(DependencyObject element) => (DataTemplate)element.GetValue(ItemTemplateProperty);


    /// <summary>
    ///   Item  最小内容高度
    /// </summary>
    public static readonly DependencyProperty MinHeightProperty = DependencyProperty.RegisterAttached(
        "MinHeight", typeof(double), typeof(Attach_Item), new PropertyMetadata(ValueBoxes.Double30Box));
    public static void SetMinHeight(DependencyObject element, double value) => element.SetValue(MinHeightProperty, value);
    public static double GetMinHeight(DependencyObject element) => (double)element.GetValue(MinHeightProperty);


    /// <summary>
    ///   Item  最小内容宽度
    /// </summary>
    public static readonly DependencyProperty MinWidthProperty = DependencyProperty.RegisterAttached(
        "MinWidth", typeof(double), typeof(Attach_Item), new PropertyMetadata(ValueBoxes.Double100Box));
    public static void SetMinWidth(DependencyObject element, double value) => element.SetValue(MinWidthProperty, value);
    public static double GetMinWidth(DependencyObject element) => (double)element.GetValue(MinWidthProperty);



    /// <summary>
    ///   Item  VerticalAlignment
    /// </summary>
    public static readonly DependencyProperty VerticalAlignmentProperty = DependencyProperty.RegisterAttached(
        "VerticalAlignment", typeof(VerticalAlignment), typeof(Attach_Item), new PropertyMetadata(VerticalAlignment.Stretch));
    public static void SetVerticalAlignment(DependencyObject element, VerticalAlignment value) => element.SetValue(VerticalAlignmentProperty, value);
    public static VerticalAlignment GetVerticalAlignment(DependencyObject element) => (VerticalAlignment)element.GetValue(VerticalAlignmentProperty);



    /// <summary>
    ///   Item  HorizontalAlignment
    /// </summary>
    public static readonly DependencyProperty HorizontalAlignmentProperty = DependencyProperty.RegisterAttached(
        "HorizontalAlignment", typeof(HorizontalAlignment), typeof(Attach_Item), new PropertyMetadata(HorizontalAlignment.Stretch));
    public static void SetHorizontalAlignment(DependencyObject element, HorizontalAlignment value) => element.SetValue(HorizontalAlignmentProperty, value);
    public static HorizontalAlignment GetHorizontalAlignment(DependencyObject element) => (HorizontalAlignment)element.GetValue(HorizontalAlignmentProperty);

}