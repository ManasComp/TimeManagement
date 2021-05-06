﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 using TimeManagement.Helpers.CalculationHelpClasses;
 using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeManagement.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Calculation : ContentPage
    {
        public Calculation()
        {
            InitializeComponent();
        }

        private Operation operation;

        private List<string> previousText = new List<string>();

        private bool validity = false;

        private void Button_Clicked(object sender, EventArgs e)
        {
            {
                string objStr = ((Button)sender).Text.ToString();

                if (objStr == (string)Equal_Button.Text)
                {
                    operation = new Operation(Display_Label.Text);
                    Display_Label.Text = operation.Result.ToString();
                    BindingContext = operation;
                    validity = true;
                }

                else if ((objStr == (string)CE_Button.Text))
                {
                    if (previousText.Count() > 0)
                    {
                        Display_Label.Text = previousText[previousText.Count() - 1];
                        previousText.RemoveAt(previousText.Count() - 1);
                        if (validity == true)
                        {
                            operation.Save = "";
                            validity = false;
                        }
                    }
                }

                else if ((objStr == (string)C_Button.Text))
                {
                    Display_Label.Text = "0";
                    previousText = new List<string>();
                    if (validity == true)
                    {
                        operation.Save = "";
                        validity = false;
                    }
                }

                else
                {
                    previousText.Add(Display_Label.Text);
                    if (Display_Label.Text == "0")
                    {
                        Display_Label.Text = "";
                    }
                    Display_Label.Text += objStr;
                }
            }
        }
    }
}