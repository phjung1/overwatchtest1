//필요한 네임스페이스
using System;
using System.Runtime.InteropServices; // DllImport를 위한 네임스페이스



[Flags()]
public enum MouseEventFlag : int
{
    Absolute = 0x8000,
    LeftDown = 0x0002,
    LeftUp = 0x0004,
    MiddleDown = 0x0020,
    MiddleUp = 0x0040,
    Move = 0x0001,
    RightDown = 0x0008,
    RightUp = 0x0010,
    Wheel = 0x0800,
    XDown = 0x0080,
    XUp = 0x0100,
    HWheel = 0x1000,
}



public enum MouseButton
{
    Left,
    Right,
    Middle,
    X,
}


public class Size
{
    public double Width { set; get; }
    public double Height { set; get; }

    public Size() { }
    public Size(double Width, double Height)
    { this.Width = Width; this.Height = Height; }
}



public struct Point
{
    public double X { set; get; }
    public double Y { set; get; }

    public Point(double X, double Y)
     : this()
    { this.X = X; this.Y = Y; }
}


public class MyMouse
{
    // API함수를 C#에서 사용할 수 있도록 합니다.
    #region DllImport
    [DllImport("User32.dll", CharSet = CharSet.Auto)]
 private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, IntPtr dwExtraInfo);
    #endregion

    // 절대적인 좌표로 마우스를 움직일때, 약간 특수한 계산이 필요합니다.
    #region Constant
    const int ABSOLUTE_SIZE = 65535;
    #endregion

    // 절대적인 좌표로 마우스를 움질일때, 화면 크기를 꼭 알아야합니다.
    #region Property
    public Size DisplaySize { set; get; }
    #endregion

    // 마우스를 제어하는 함수들입니다.
    #region Moving
    public void Move(Point Point)
    {
        MouseEventFlag Flag = MouseEventFlag.Move;

        mouse_event((int)Flag, (int)Point.X, (int)Point.Y, 0, IntPtr.Zero);
    }

    public void MoveAt(Point Point)
    {
        MouseEventFlag Flag = MouseEventFlag.Move | MouseEventFlag.Absolute;

        int X = (int)(ABSOLUTE_SIZE / 1024 * Point.X);
        int Y = (int)(ABSOLUTE_SIZE / 768 * Point.Y);

        mouse_event((int)Flag, X, Y, 0, IntPtr.Zero);
    }

    public void MoveAbsolute(Point Point)
    {
        MouseEventFlag Flag = MouseEventFlag.Move | MouseEventFlag.Absolute;

        mouse_event((int)Flag, (int)Point.X, (int)Point.Y, 0, IntPtr.Zero);
    }
    #endregion

    #region Input
    public void Down(MouseButton Button)
    {
        MouseEventFlag Flag = 0;

        switch (Button)
        {
            case MouseButton.Left: Flag = MouseEventFlag.LeftDown; break;
            case MouseButton.Right: Flag = MouseEventFlag.RightDown; break;
            case MouseButton.Middle: Flag = MouseEventFlag.MiddleDown; break;
            case MouseButton.X: Flag = MouseEventFlag.XDown; break;
        }

        mouse_event((int)Flag, 0, 0, 0, IntPtr.Zero);
    }

    public void Up(MouseButton Button)
    {
        MouseEventFlag Flag = 0;

        switch (Button)
        {
            case MouseButton.Left: Flag = MouseEventFlag.LeftUp; break;
            case MouseButton.Right: Flag = MouseEventFlag.RightUp; break;
            case MouseButton.Middle: Flag = MouseEventFlag.MiddleUp; break;
            case MouseButton.X: Flag = MouseEventFlag.XUp; break;
        }

        mouse_event((int)Flag, 0, 0, 0, IntPtr.Zero);
    }
    #endregion
}