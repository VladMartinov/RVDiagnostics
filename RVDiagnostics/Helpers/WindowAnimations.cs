using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace RVDiagnostics.Helpers
{
    public static class WindowAnimations
    {
        public static void FadeIn(Window window, double duration = 0.6)
        {
            var anim = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(duration));
            window.BeginAnimation(Window.OpacityProperty, anim);
        }

        public static void FadeOutAndClose(Window window, Action? after = null, double duration = 0.6)
        {
            var anim = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(duration));
            anim.Completed += (s, e) =>
            {
                after?.Invoke();
                window.Close();
            };
            window.BeginAnimation(Window.OpacityProperty, anim);
        }
    }
}
