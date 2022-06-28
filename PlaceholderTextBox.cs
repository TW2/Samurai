using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Samurai
{
    public class PlaceholderTextBox : TextBox
    {
        private string _placeholder;

        private bool _hasText = false;
        private bool _entered = false;

        public PlaceholderTextBox()
        {
            _placeholder = string.Empty;

            MouseEnter += PlaceholderTextBox_MouseEnter;
            MouseLeave += PlaceholderTextBox_MouseLeave;
            Click += PlaceholderTextBox_Click;
            KeyDown += PlaceholderTextBox_KeyDown;
            KeyUp += PlaceholderTextBox_KeyUp;
        }

        private void PlaceholderTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (Text.Contains(_placeholder)) Text = Text.Replace(_placeholder, "");
            UserAction();
        }

        private void PlaceholderTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (Text.Contains(_placeholder)) Text = Text.Replace(_placeholder, "");
            UserAction();
        }

        private void PlaceholderTextBox_Click(object sender, EventArgs e)
        {
            _hasText = Text == _placeholder ? false : true;

            if (_entered == true && Text == _placeholder)
            {
                Text = string.Empty;
                _hasText = Text == _placeholder ? false : true;
            }

            ApplyChange();
        }

        private void PlaceholderTextBox_MouseLeave(object sender, EventArgs e)
        {
            _entered = false;
            UserAction();
        }

        private void PlaceholderTextBox_MouseEnter(object sender, EventArgs e)
        {
            _entered = true;
            UserAction();
        }

        private void ApplyChange()
        {
            if (_hasText == false)
            {
                // On a un paramètre fictif (placeholder)
                ForeColor = Color.Salmon;
            }
            else
            {
                // On n'a aucun paramètre fictif (placeholder)
                ForeColor = Color.LimeGreen;
            }
        }

        private void InitPlaceholder()
        {
            // On a un paramètre fictif (placeholder)
            ForeColor = Color.Salmon;
            Text = _placeholder;
        }

        private void UserAction()
        {
            _hasText = Text == _placeholder ? false : true;

            if (Text == string.Empty)
            {
                Text = _placeholder;
                _hasText = Text == _placeholder ? false : true;
            }

            ApplyChange();
        }

        public string Placeholder   // property
        {
            get { return _placeholder; }   // get method
            set { _placeholder = value; InitPlaceholder(); }  // set method
        }
    }
}
