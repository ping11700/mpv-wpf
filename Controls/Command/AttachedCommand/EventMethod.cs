namespace Controls.Command;


public class EventMethod : AttachObject
{

    public ICommand DoCommand
    {
        get => (ICommand)GetValue(DoCommandProperty);
        set => SetValue(DoCommandProperty, value);
    }
    public static readonly DependencyProperty DoCommandProperty = DependencyProperty.Register(
        "DoCommand", typeof(ICommand), typeof(EventMethod), new PropertyMetadata(null));


    public object CommandParameter
    {
        get { return (object)GetValue(CommandParameterProperty); }
        set { SetValue(CommandParameterProperty, value); }
    }

    // Using a DependencyProperty as the backing store for CommandParameter.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CommandParameterProperty =
        DependencyProperty.Register("CommandParameter", typeof(object), typeof(AttachObject), new PropertyMetadata(null));


    protected override void OnAttach(object args)
    {
        if (args != null && DoCommand != null)
        {
            // 做一个本地变量，解决问题
            // 1.直接使用属性，则每次拿属性都是重新获取了一份，按理说是不符合实际情况的，
            //       如 DoCommand.Execute(parameter); 语句 调用后影响到 CommandParameter 并且导致 CommandParameter 为空时 ，则会继续走 DoCommand.Execute(args)  这显然不符合预期
            // 2.每次重新获取，性能上应该会差一点点

            var parameter = CommandParameter;
            try
            {
                if (parameter != null)
                    if (DoCommand.CanExecute(parameter))
                        DoCommand.Execute(parameter);
                if (parameter == null)
                    if (DoCommand.CanExecute(args))
                        DoCommand.Execute(args);
            }
            catch { }
        }
    }


}