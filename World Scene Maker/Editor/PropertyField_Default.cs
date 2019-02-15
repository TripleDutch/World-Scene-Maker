﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DS_PropertyEditor
{
    class PropertyField_Default<ValueType> : PropertyField<ValueType>, IPropertyField<ValueType> where ValueType : IConvertible
    {
        //Constants
        private const double SCROLLWHEEL_WIDTH = 10;

        //Variables
        private TextBox tbxValue = new TextBox();

        //Constructor
        public PropertyField_Default(string pName, ValueType pValue, double pWidth) : base(pName, pValue, pWidth)
        {
            //valuebox
            tbxValue.Width = pWidth - (STACKPANEL_LEFT_MARGIN * 2) - NAMEBOX_WIDTH - SCROLLWHEEL_WIDTH;
            tbxValue.KeyUp += EnterStopEdit;
            tbxValue.LostFocus += LoseFocus;
            tbxValue.GotKeyboardFocus += GetKeyboardFocus;
            this.Children.Add(tbxValue);
        }
        
        //Properties

        //Functions
        public override void UpdateGraphics()
        {
            base.UpdateGraphics();
            if (Value != null)
            {
                tbxValue.Text = Value.ToString();
            }
        }

        //Methods
        private void LoseFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                ValueType pValue;
                if (typeof(ValueType).IsEnum)
                {
                    pValue = (ValueType)Enum.Parse(typeof(ValueType), tbxValue.Text);
                }
                else
                {
                    pValue = (ValueType)Convert.ChangeType(tbxValue.Text, typeof(ValueType));
                }

                Value = pValue;
            }
            catch
            {
                tbxValue.SelectionStart = tbxValue.Text.Length;
                tbxValue.SelectionLength = 0;
            }
            UpdateGraphics();
        }
        private void EnterStopEdit(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LoseFocus(sender, null);
                GetKeyboardFocus(sender, null);
            }
        }
        private void GetKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            tbxValue.Select(tbxValue.Text.Length, 0);
        }

        //Events
    }
}
