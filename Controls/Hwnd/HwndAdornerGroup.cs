using Point = System.Windows.Point;

namespace Controls.Hwnd;

/// <summary>
/// An internal class for managing the connection of a group of HwndAdorner's to their owner window.
/// The HwndAdorner searches up the visual tree for an IHwndAdornerManager containing an instance of this group,
/// if an IHwndAdornerManager is not found it creates a group containing only itself
/// </summary>
internal class HwndAdornerGroup : HwndSourceConnector
{
    // This class manages its base class resources (HwndSourceConnector) on its own.
    // i.e. when appropriately used, it dos not need to be disposed.

    private readonly HashSet<HwndAdorner> _adornersInGroup = new();

    private HwndSource? _ownerSource;
    private readonly UIElement? _ancestor;

    private const uint SET_ONLY_ZORDER = InteropValues.SWP_NOMOVE | InteropValues.SWP_NOSIZE | InteropValues.SWP_NOACTIVATE;

    internal HwndAdornerGroup(UIElement commonAncestor)
        : base(commonAncestor)
    {
        _ancestor = commonAncestor;
        _ancestor.LayoutUpdated += CommonAncestor_LayoutUpdated;
    }



    internal bool Owned => _ownerSource is not null;
    private bool HasAdorners => _adornersInGroup.Count > 0;


    private void CommonAncestor_LayoutUpdated(object? sender, EventArgs e)
    {
        SetPosition();
    }

    internal bool AddAdorner(HwndAdorner adorner)
    {
        if (!Activated) Activate();

        _adornersInGroup.Add(adorner);

        if (_ownerSource is not null)
        {
            SetOwnership(adorner);
            ActivateInGroupLimits(adorner);
            adorner.InvalidateAppearance();

            var root = (UIElement)_ownerSource.RootVisual;
            adorner.UpdateOwnerPosition(GetRectFromRoot(root));

        }

        return true;
    }

    private static Rect GetRectFromRoot(UIElement root)
        => new(root.PointToScreen(new Point()), root.PointToScreen(new Point(root.RenderSize.Width, root.RenderSize.Height)));

    internal bool RemoveAdorner(HwndAdorner adorner)
    {
        var res = _adornersInGroup.Remove(adorner);

        if (Owned)
        {
            RemoveOwnership(adorner);
            adorner.InvalidateAppearance();
        }

        if (!HasAdorners) Deactivate();

        return res;
    }

    protected override void OnSourceConnected(HwndSource connectedSource)
    {
        if (Owned) DisconnectFromOwner();

        _ownerSource = connectedSource;
        _ownerSource.AddHook(OwnerHook);

        if (HasAdorners)
        {
            SetOwnership();
            SetZOrder();
            SetPosition();
            InvalidateAppearance();
        }
    }

    protected override void OnSourceDisconnected(HwndSource disconnectedSource)
    {
        DisconnectFromOwner();
    }

    private void DisconnectFromOwner()
    {
        if (_ownerSource is null) return;

        _ownerSource.RemoveHook(OwnerHook);
        _ownerSource = null;

        _ancestor.LayoutUpdated -= CommonAncestor_LayoutUpdated;


        RemoveOwnership();
        InvalidateAppearance();
    }

    private void SetOwnership()
    {
        foreach (var adorner in _adornersInGroup)
        {
            SetOwnership(adorner);
        }
    }

    private void InvalidateAppearance()
    {
        foreach (var adorner in _adornersInGroup)
        {
            adorner.InvalidateAppearance();
        }
    }

    private void SetOwnership(HwndAdorner adorner)
        => NativeAPI.SetWindowLong(adorner.Handle, InteropValues.GWL_HWNDPARENT, _ownerSource?.Handle ?? throw new InvalidOperationException());

    private void RemoveOwnership()
    {
        foreach (var adorner in _adornersInGroup)
        {
            RemoveOwnership(adorner);
        }
    }

    private static void RemoveOwnership(HwndAdorner adorner)
        => NativeAPI.SetWindowLong(adorner.Handle, InteropValues.GWL_HWNDPARENT, IntPtr.Zero);

    private void SetPosition()
    {
        if (_ownerSource?.RootVisual is not UIElement root) return;

        var rect = GetRectFromRoot(root);

        foreach (var adorner in _adornersInGroup)
        {
            adorner.UpdateOwnerPosition(rect);
        }
    }

    private void SetZOrder()
    {
        if (_ownerSource is null) return;

        // getting the hwnd above the owner (in win32, the prev hwnd is the one visually above)
        var hwndAbove = NativeAPI.GetWindow(_ownerSource.Handle, InteropValues.GW_HWNDPREV);

        if (hwndAbove == IntPtr.Zero && HasAdorners)
        // owner is the Top most window
        {
            // randomly selecting an owned hwnd
            var owned = _adornersInGroup.First().Handle;
            // setting owner after (visually under) it 
            NativeAPI.SetWindowPos(_ownerSource.Handle, owned, 0, 0, 0, 0, SET_ONLY_ZORDER);

            // now this is the 'above' hwnd
            hwndAbove = owned;
        }

        // inserting all adorners between the owner and the hwnd initially above it
        // currently not preserving any previous z-order state between the adorners (unsupported for now)
        foreach (var adorner in _adornersInGroup)
        {
            var handle = adorner.Handle;
            NativeAPI.SetWindowPos(handle, hwndAbove, 0, 0, 0, 0, SET_ONLY_ZORDER);
            hwndAbove = handle;
        }
    }

    internal void ActivateInGroupLimits(HwndAdorner adorner)
    {
        if (_ownerSource is null) return;

        var current = _ownerSource.Handle;

        // getting the hwnd above the owner (in win32, the prev hwnd is the one visually above)
        var prev = NativeAPI.GetWindow(current, InteropValues.GW_HWNDPREV);

        // searching up for the first non-sibling hwnd
        while (_adornersInGroup.Any(o => o.Handle == prev))
        {
            current = prev;
            prev = NativeAPI.GetWindow(current, InteropValues.GW_HWNDPREV);
        }

        if (prev == IntPtr.Zero)
        // the owner or one of the siblings is the Top-most window
        {
            // setting the Top-most under the activated adorner
            NativeAPI.SetWindowPos(current, adorner.Handle, 0, 0, 0, 0, SET_ONLY_ZORDER);
        }
        else
        {
            // setting the activated adorner under the first non-sibling hwnd
            NativeAPI.SetWindowPos(adorner.Handle, prev, 0, 0, 0, 0, SET_ONLY_ZORDER);
        }
    }


    private IntPtr OwnerHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {


        if (msg == InteropValues.WM_WINDOWPOSCHANGED)
        {
            SetPosition();
        }

        return IntPtr.Zero;
    }
}
