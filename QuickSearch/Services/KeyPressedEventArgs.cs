using System.Windows.Forms;
using System.Windows.Input;
using System;

namespace QuickSearch.Services
{
    /// <summary>
    /// Event Args for the event that is fired after the hot key has been pressed.
    /// </summary>
    public sealed class KeyPressedEventArgs : EventArgs
    {
        internal KeyPressedEventArgs(ModifierKeys modifier, Keys key)
        {
            this.modifier = modifier;
            this.key = key;
        }

        #region Properties

        public ModifierKeys Modifier
        {
            get => modifier;
        }

        public Keys Key
        {
            get => key;
        }

        #endregion

        private readonly Keys key;
        private readonly ModifierKeys modifier;
    }
}
