using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Debug = UnityEngine.Debug;

public static class CallLogger
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    [Conditional("UNITY_EDITOR")]
    [Conditional("DEVELOPMENT_BUILD")]
#if UNITY_2021_2_OR_NEWER
    [HideInCallstack]
#endif
    public static void LogHere(string contextMessage = null, int extraSkipFrames = 0)
    {
        var trace = new StackTrace(1 + extraSkipFrames, true);
        var caller = trace.FrameCount > 0 ? trace.GetFrame(0) : null;
        var callersCaller = trace.FrameCount > 1 ? trace.GetFrame(1) : null;
        var sb = new StringBuilder();
        sb.Append("[Call] ");
        if (!string.IsNullOrEmpty(contextMessage)) sb.Append(contextMessage).Append(" ");
        sb.Append("at ").Append(FormatFrame(caller));
        sb.Append(" | callers caller: ").Append(FormatFrame(callersCaller));
        Debug.Log(sb.ToString());
    }

    static string FormatFrame(StackFrame frame)
    {
        if (frame == null) return "<unknown>";
        var m = frame.GetMethod();
        var t = m?.DeclaringType;
        var typeName = t != null ? t.FullName : "<global>";
        var file = frame.GetFileName();
        var line = frame.GetFileLineNumber();
        var loc = !string.IsNullOrEmpty(file) && line > 0 ? $" ({System.IO.Path.GetFileName(file)}:{line})" : "";
        return typeName + "." + m?.Name + loc;
    }
}