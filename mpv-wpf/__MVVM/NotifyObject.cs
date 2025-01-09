namespace mpv_wpf.__MVVM;

public abstract class NotifyObject : INotifyPropertyChanged
{
    #region PropertyChanged


    public event PropertyChangedEventHandler? PropertyChanged;

    protected PropertyChangedEventHandler? PropertyChangedHandler => PropertyChanged;

    public event PropertyChangingEventHandler? PropertyChanging;

    protected PropertyChangingEventHandler? PropertyChangingHandler => PropertyChanging;


    public virtual void OnPropertyChanging([CallerMemberName] string? propertyName = null)
    {
        PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
    }

    public virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool Set<T>(string propertyName, ref T field, T newValue)
    {
        if (EqualityComparer<T>.Default.Equals(field, newValue))
        {
            return false;
        }

        OnPropertyChanging(propertyName);
        field = newValue;

        OnPropertyChanged(propertyName);

        return true;
    }

    protected bool SetProperty<T>(ref T field, T newValue, string? propertyName = null)
    {
        return Set(propertyName, ref field, newValue);
    }

    #endregion

}
