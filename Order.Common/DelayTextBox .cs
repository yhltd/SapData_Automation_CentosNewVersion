﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace Order.Common
{
    public partial class DelayTextBox : TextBox
    {
        #region private globals

        private System.Timers.Timer DelayTimer; // used for the delay
        private bool TimerElapsed = false; // if true OnTextChanged is fired.
        private bool KeysPressed = false; // makes event fire immediately if it wasn't a keypress


        #endregion

        #region object model

        // Delay property
        private int delayTime = 3000;

        public int DelayTime
        {
            get { return delayTime; }
            set { delayTime = value; }
        }

        #endregion

        #region ctor

        public DelayTextBox()
        {
          //  InitializeComponent();

            // Initialize Timer
            DelayTimer = new System.Timers.Timer(delayTime);
            DelayTimer.Elapsed += new ElapsedEventHandler(DelayTimer_Elapsed);
        }

        #endregion

        #region event handlers

        void DelayTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // stop timer.
            DelayTimer.Enabled = false;

            // set timer elapsed to true, so the OnTextChange knows to fire
            TimerElapsed = true;

            // use invoke to get back on the UI thread.
            this.Invoke(new DelayOverHandler(DelayOver), null);
        }

        #endregion

        #region overrides

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (!DelayTimer.Enabled)
                DelayTimer.Enabled = true;
            else
            {
                DelayTimer.Enabled = false;
                DelayTimer.Enabled = true;
            }

            KeysPressed = true;
            base.OnKeyPress(e);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            // if the timer elapsed or text was changed by something besides a keystroke
            // fire base.OnTextChanged
            if (TimerElapsed || !KeysPressed)
            {
                TimerElapsed = false;
                KeysPressed = false;
                base.OnTextChanged(e);
            }
        }

        #endregion

        #region delegates

        public delegate void DelayOverHandler();

        #endregion

        #region private helpers

        private void DelayOver()
        {
            OnTextChanged(new EventArgs());
        }

        #endregion

    }
}
