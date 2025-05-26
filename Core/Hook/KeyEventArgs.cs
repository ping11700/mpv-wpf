namespace Core.Hook;

public class KeyEventArgs(Keys key) : EventArgs
{
    public Keys Key { get; private set; } = key;

    public bool Handled { get; private set; }
}

public class KeyPressEventArgs(char keyChar) : EventArgs
{
    /// <summary>
    /// Gets or sets the character corresponding to the key pressed.
    /// </summary>
    /// <returns>
    /// The ASCII character that is composed. For example, if the user presses SHIFT + K, 
    /// this property returns an uppercase K.
    /// </returns>
    public char KeyChar { get; private set; } = keyChar;

    public bool Handled { get; private set; }
}



public class MouseEventArgs(MouseButton button, int clicks, int x, int y, int delta) : EventArgs
{
    public MouseButton Button { get; private set; } = button;

    public int Clicks { get; private set; } = clicks;

    public int PosX { get; private set; } = x;

    public int PosY { get; private set; } = y;

    public int Delta { get; private set; } = delta;
}