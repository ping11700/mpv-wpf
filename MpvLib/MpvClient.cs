﻿namespace MpvLib;

public class MpvClient : IDisposable
{
    public event Action<string[]>? ClientMessageEvent;            // client-message      MPV_EVENT_CLIENT_MESSAGE
    public event Action<string>? ErrorEvent;                           // shutdown            MPV_EVENT_SHUTDOWN
    public event Action<mpv_log_level, string>? LogMessageEvent;  // log-message         MPV_EVENT_LOG_MESSAGE
    public event Action<mpv_error, mpv_end_file_reason>? EndFileEvent;       // end-file            MPV_EVENT_END_FILE
    public event Action? ShutdownEvent;                           // shutdown            MPV_EVENT_SHUTDOWN
    public event Action? GetPropertyReplyEvent;                   // get-property-reply  MPV_EVENT_GET_PROPERTY_REPLY
    public event Action? SetPropertyReplyEvent;                   // set-property-reply  MPV_EVENT_SET_PROPERTY_REPLY
    public event Action? CommandReplyEvent;                       // command-reply       MPV_EVENT_COMMAND_REPLY
    public event Action? StartFileEvent;                          // start-file          MPV_EVENT_START_FILE
    public event Action? FileLoadedEvent;                         // file-loaded         MPV_EVENT_FILE_LOADED
    public event Action? VideoReconfigEvent;                      // video-reconfig      MPV_EVENT_VIDEO_RECONFIG
    public event Action? AudioReconfigEvent;                      // audio-reconfig      MPV_EVENT_AUDIO_RECONFIG
    public event Action? SeekEvent;                               // seek                MPV_EVENT_SEEK
    public event Action? PlaybackRestartEvent;                    // playback-restart    MPV_EVENT_PLAYBACK_RESTART

    public Dictionary<string, List<Action>> PropChangeActions { get; set; } = [];
    public Dictionary<string, List<Action<int>>> IntPropChangeActions { get; set; } = [];
    public Dictionary<string, List<Action<bool>>> BoolPropChangeActions { get; set; } = [];
    public Dictionary<string, List<Action<double>>> DoublePropChangeActions { get; set; } = [];
    public Dictionary<string, List<Action<string>>> StringPropChangeActions { get; set; } = [];

    public nint Handle { get; set; }

    public void EventLoop()
    {
        while (true)
        {
            IntPtr ptr = mpv_wait_event(Handle, -1);
            mpv_event evt = Marshal.PtrToStructure<mpv_event>(ptr)!;

            try
            {
                switch (evt.event_id)
                {
                    case mpv_event_id.MPV_EVENT_SHUTDOWN:
                        OnShutdown();
                        return;
                    case mpv_event_id.MPV_EVENT_LOG_MESSAGE:
                        {
                            var data = Marshal.PtrToStructure<mpv_event_log_message>(evt.data)!;
                            OnLogMessage(data);
                        }
                        break;
                    case mpv_event_id.MPV_EVENT_CLIENT_MESSAGE:
                        {
                            var data = Marshal.PtrToStructure<mpv_event_client_message>(evt.data)!;
                            OnClientMessage(data);
                        }
                        break;
                    case mpv_event_id.MPV_EVENT_VIDEO_RECONFIG:
                        OnVideoReconfig();
                        break;
                    case mpv_event_id.MPV_EVENT_END_FILE:
                        {
                            var data = Marshal.PtrToStructure<mpv_event_end_file>(evt.data)!;
                            OnEndFile(data);
                        }
                        break;
                    case mpv_event_id.MPV_EVENT_FILE_LOADED:  // triggered after MPV_EVENT_START_FILE
                        OnFileLoaded();
                        break;
                    case mpv_event_id.MPV_EVENT_PROPERTY_CHANGE:
                        {
                            var data = Marshal.PtrToStructure<mpv_event_property>(evt.data)!;
                            OnPropertyChange(data);
                        }
                        break;
                    case mpv_event_id.MPV_EVENT_GET_PROPERTY_REPLY:
                        OnGetPropertyReply();
                        break;
                    case mpv_event_id.MPV_EVENT_SET_PROPERTY_REPLY:
                        OnSetPropertyReply();
                        break;
                    case mpv_event_id.MPV_EVENT_COMMAND_REPLY:
                        OnCommandReply();
                        break;
                    case mpv_event_id.MPV_EVENT_START_FILE:  // triggered before MPV_EVENT_FILE_LOADED
                        OnStartFile();
                        break;
                    case mpv_event_id.MPV_EVENT_AUDIO_RECONFIG:
                        OnAudioReconfig();
                        break;
                    case mpv_event_id.MPV_EVENT_SEEK:
                        OnSeek();
                        break;
                    case mpv_event_id.MPV_EVENT_PLAYBACK_RESTART:
                        OnPlaybackRestart();
                        break;
                }
            }
            catch (Exception ex)
            {
                HandleError(default, $"EventLoop : {ex.Message}");
                //Terminal.WriteError(ex);
            }
        }
    }

    protected virtual void OnClientMessage(mpv_event_client_message data) => ClientMessageEvent?.Invoke(ConvertFromUtf8Strings(data.args, data.num_args));

    protected virtual void OnLogMessage(mpv_event_log_message data)
    {
        if (LogMessageEvent != null)
        {
            string msg = $"[{ConvertFromUtf8(data.prefix)}] {ConvertFromUtf8(data.text)}";
            LogMessageEvent.Invoke(data.log_level, msg);
        }
    }

    protected virtual void OnPropertyChange(mpv_event_property data)
    {
        if (data.format == mpv_format.MPV_FORMAT_FLAG)
        {
            lock (BoolPropChangeActions)
                foreach (var pair in BoolPropChangeActions)
                    if (pair.Key == data.name)
                    {
                        bool value = Marshal.PtrToStructure<int>(data.data) == 1;

                        foreach (var action in pair.Value)
                            action.Invoke(value);
                    }
        }
        else if (data.format == mpv_format.MPV_FORMAT_STRING)
        {
            lock (StringPropChangeActions)
                foreach (var pair in StringPropChangeActions)
                    if (pair.Key == data.name)
                    {
                        string value = ConvertFromUtf8(Marshal.PtrToStructure<IntPtr>(data.data));

                        foreach (var action in pair.Value)
                            action.Invoke(value);
                    }
        }
        else if (data.format == mpv_format.MPV_FORMAT_INT64)
        {
            lock (IntPropChangeActions)
                foreach (var pair in IntPropChangeActions)
                    if (pair.Key == data.name)
                    {
                        int value = Marshal.PtrToStructure<int>(data.data);

                        foreach (var action in pair.Value)
                            action.Invoke(value);
                    }
        }
        else if (data.format == mpv_format.MPV_FORMAT_NONE)
        {
            lock (PropChangeActions)
                foreach (var pair in PropChangeActions)
                    if (pair.Key == data.name)
                        foreach (var action in pair.Value)
                            action.Invoke();
        }
        else if (data.format == mpv_format.MPV_FORMAT_DOUBLE)
        {
            lock (DoublePropChangeActions)
                foreach (var pair in DoublePropChangeActions)
                    if (pair.Key == data.name)
                    {
                        double value = Marshal.PtrToStructure<double>(data.data);

                        foreach (var action in pair.Value)
                            action.Invoke(value);
                    }
        }
    }
    protected virtual void OnStartFile() => StartFileEvent?.Invoke();
    protected virtual void OnEndFile(mpv_event_end_file data) => EndFileEvent?.Invoke((mpv_error)data.error, (mpv_end_file_reason)data.reason);
    protected virtual void OnFileLoaded() => FileLoadedEvent?.Invoke();
    protected virtual void OnShutdown() => ShutdownEvent?.Invoke();
    protected virtual void OnGetPropertyReply() => GetPropertyReplyEvent?.Invoke();
    protected virtual void OnSetPropertyReply() => SetPropertyReplyEvent?.Invoke();
    protected virtual void OnCommandReply() => CommandReplyEvent?.Invoke();
    protected virtual void OnVideoReconfig() => VideoReconfigEvent?.Invoke();
    protected virtual void OnAudioReconfig() => AudioReconfigEvent?.Invoke();
    protected virtual void OnSeek() => SeekEvent?.Invoke();
    protected virtual void OnPlaybackRestart() => PlaybackRestartEvent?.Invoke();

    public void Command(string command)
    {
        mpv_error err = mpv_command_string(Handle, command);

        if (err < 0)
            HandleError(err, "error executing command: " + command);
    }

    public void CommandV(params string[] args)
    {
        int count = args.Length + 1;
        IntPtr[] pointers = new IntPtr[count];
        IntPtr rootPtr = Marshal.AllocHGlobal(IntPtr.Size * count);

        for (int index = 0; index < args.Length; index++)
        {
            var bytes = GetUtf8Bytes(args[index]);
            IntPtr ptr = Marshal.AllocHGlobal(bytes.Length);
            Marshal.Copy(bytes, 0, ptr, bytes.Length);
            pointers[index] = ptr;
        }

        Marshal.Copy(pointers, 0, rootPtr, count);
        mpv_error err = mpv_command(Handle, rootPtr);

        foreach (IntPtr ptr in pointers)
            Marshal.FreeHGlobal(ptr);

        Marshal.FreeHGlobal(rootPtr);

        if (err < 0)
            HandleError(err, "error executing command: " + string.Join("\n", args));
    }


    public bool GetPropertyBool(string name)
    {
        mpv_error err = mpv_get_property(Handle, GetUtf8Bytes(name),
            mpv_format.MPV_FORMAT_FLAG, out IntPtr lpBuffer);

        if (err < 0)
            HandleError(err, "error getting property: " + name);

        return lpBuffer.ToInt32() != 0;
    }

    public void SetPropertyBool(string name, bool value)
    {
        long val = value ? 1 : 0;
        mpv_error err = mpv_set_property(Handle, GetUtf8Bytes(name), mpv_format.MPV_FORMAT_FLAG, ref val);

        if (err < 0)
            HandleError(err, $"error setting property: {name} = {value}");
    }

    public int GetPropertyInt(string name)
    {
        mpv_error err = mpv_get_property(Handle, GetUtf8Bytes(name),
            mpv_format.MPV_FORMAT_INT64, out IntPtr lpBuffer);

        if (err < 0)
            HandleError(err, "error getting property: " + name);

        return lpBuffer.ToInt32();
    }

    public void SetPropertyInt(string name, int value)
    {
        long val = value;
        mpv_error err = mpv_set_property(Handle, GetUtf8Bytes(name), mpv_format.MPV_FORMAT_INT64, ref val);

        if (err < 0)
            HandleError(err, $"error setting property: {name} = {value}");
    }

    public void SetPropertyLong(string name, long value)
    {
        mpv_error err = mpv_set_property(Handle, GetUtf8Bytes(name), mpv_format.MPV_FORMAT_INT64, ref value);

        if (err < 0)
            HandleError(err, $"error setting property: {name} = {value}");
    }

    public long GetPropertyLong(string name)
    {
        mpv_error err = mpv_get_property(Handle, GetUtf8Bytes(name),
            mpv_format.MPV_FORMAT_INT64, out IntPtr lpBuffer);

        if (err < 0)
            HandleError(err, "error getting property: " + name);

        return lpBuffer.ToInt64();
    }

    public double GetPropertyDouble(string name, bool handleError = true)
    {
        mpv_error err = mpv_get_property(Handle, GetUtf8Bytes(name),
            mpv_format.MPV_FORMAT_DOUBLE, out double value);

        if (err < 0 && handleError)
            HandleError(err, "error getting property: " + name);

        return value;
    }

    public void SetPropertyDouble(string name, double value)
    {
        double val = value;
        mpv_error err = mpv_set_property(Handle, GetUtf8Bytes(name), mpv_format.MPV_FORMAT_DOUBLE, ref val);

        if (err < 0)
            HandleError(err, $"error setting property: {name} = {value}");
    }

    public string GetPropertyString(string name)
    {
        mpv_error err = mpv_get_property(Handle, GetUtf8Bytes(name), mpv_format.MPV_FORMAT_STRING, out IntPtr lpBuffer);

        if (err == 0)
        {
            string ret = ConvertFromUtf8(lpBuffer);
            mpv_free(lpBuffer);
            return ret;
        }

        if (err < 0)
            HandleError(err, "error getting property: " + name);

        return "";
    }

    public void SetPropertyString(string name, string value)
    {
        byte[] bytes = GetUtf8Bytes(value);
        mpv_error err = mpv_set_property(Handle, GetUtf8Bytes(name), mpv_format.MPV_FORMAT_STRING, ref bytes);

        if (err < 0)
            HandleError(err, $"error setting property: {name} = {value}");
    }

    public string GetPropertyOsdString(string name)
    {
        mpv_error err = mpv_get_property(Handle, GetUtf8Bytes(name),
            mpv_format.MPV_FORMAT_OSD_STRING, out IntPtr lpBuffer);

        if (err == 0)
        {
            string ret = ConvertFromUtf8(lpBuffer);
            mpv_free(lpBuffer);
            return ret;
        }

        if (err < 0)
            HandleError(err, "error getting property: " + name);

        return "";
    }

    public void ObservePropertyInt(string name, Action<int> action)
    {
        lock (IntPropChangeActions)
        {
            if (!IntPropChangeActions.ContainsKey(name))
            {
                mpv_error err = mpv_observe_property(Handle, 0, name, mpv_format.MPV_FORMAT_INT64);

                if (err < 0)
                    HandleError(err, "error observing property: " + name);
                else
                    IntPropChangeActions[name] = new List<Action<int>>();
            }

            if (IntPropChangeActions.ContainsKey(name))
                IntPropChangeActions[name].Add(action);
        }
    }

    public void ObservePropertyDouble(string name, Action<double> action)
    {
        lock (DoublePropChangeActions)
        {
            if (!DoublePropChangeActions.ContainsKey(name))
            {
                mpv_error err = mpv_observe_property(Handle, 0, name, mpv_format.MPV_FORMAT_DOUBLE);

                if (err < 0)
                    HandleError(err, "error observing property: " + name);
                else
                    DoublePropChangeActions[name] = new List<Action<double>>();
            }

            if (DoublePropChangeActions.TryGetValue(name, out List<Action<double>>? value))
                value.Add(action);
        }
    }

    public void ObservePropertyBool(string name, Action<bool> action)
    {
        lock (BoolPropChangeActions)
        {
            if (!BoolPropChangeActions.ContainsKey(name))
            {
                mpv_error err = mpv_observe_property(Handle, 0, name, mpv_format.MPV_FORMAT_FLAG);

                if (err < 0)
                    HandleError(err, "error observing property: " + name);
                else
                    BoolPropChangeActions[name] = [];
            }

            if (BoolPropChangeActions.TryGetValue(name, out List<Action<bool>>? value))
                value.Add(action);
        }
    }

    public void ObservePropertyString(string name, Action<string> action)
    {
        lock (StringPropChangeActions)
        {
            if (!StringPropChangeActions.ContainsKey(name))
            {
                mpv_error err = mpv_observe_property(Handle, 0, name, mpv_format.MPV_FORMAT_STRING);

                if (err < 0)
                    HandleError(err, "error observing property: " + name);
                else
                    StringPropChangeActions[name] = [];
            }

            if (StringPropChangeActions.ContainsKey(name))
                StringPropChangeActions[name].Add(action);
        }
    }

    public void ObserveProperty(string name, Action action)
    {
        lock (PropChangeActions)
        {
            if (!PropChangeActions.ContainsKey(name))
            {
                mpv_error err = mpv_observe_property(Handle, 0, name, mpv_format.MPV_FORMAT_NONE);

                if (err < 0)
                    HandleError(err, "error observing property: " + name);
                else
                    PropChangeActions[name] = [];
            }

            if (PropChangeActions.TryGetValue(name, out List<Action>? value))
                value.Add(action);
        }
    }

    private void HandleError(mpv_error err, string msg)
    {
        ErrorEvent?.Invoke($"Error: {GetError(err)}  Msg:{msg}");

        //Terminal.WriteError(msg);
        //Terminal.WriteError(GetError(err));
    }


    public virtual void Dispose() { }
}