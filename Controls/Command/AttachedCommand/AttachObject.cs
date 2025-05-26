namespace Controls.Command;


public abstract class AttachObject : Animatable
{

    public string AttachEventName
    {
        get => (string)GetValue(AttachEventNameProperty);
        set => SetValue(AttachEventNameProperty, value);
    }


    public static readonly DependencyProperty AttachEventNameProperty =
        DependencyProperty.Register("AttachEventName", typeof(string), typeof(AttachObject), new PropertyMetadata(string.Empty));


    protected Delegate? RunDelegate;

    protected override Freezable? CreateInstanceCore() => (Freezable)Activator.CreateInstance(this.GetType());

    public DependencyObject? AttachObjectTarget { get; set; }

    protected virtual void OnAttach(object args)
    {
    }

    public virtual void DoAcion(object sender, EventArgs e)
    {
        OnAttach(e);
    }


    public virtual void RunAction()
    {
        if (AttachObjectTarget != null && !string.IsNullOrWhiteSpace(AttachEventName))
        {
            var GetSource = AttachObjectTarget.GetType();
            var RunMothod = GetSource.GetEvent(AttachEventName);
            var ClassMethod = this.GetType().GetMethod(nameof(DoAcion), BindingFlags.Public | BindingFlags.Instance);
            RunDelegate = Delegate.CreateDelegate(RunMothod.EventHandlerType, this, ClassMethod);
            RunMothod.AddEventHandler(AttachObjectTarget, RunDelegate);
        }

    }
    public virtual void OnDeach()
    {
        if (RunDelegate != null)
        {
            var GetSource = AttachObjectTarget?.GetType();
            var mothod = GetSource?.GetEvent(AttachEventName);
            mothod?.RemoveEventHandler(AttachObjectTarget, RunDelegate);
            RunDelegate = null;
            AttachObjectTarget = null;
        }
    }




}