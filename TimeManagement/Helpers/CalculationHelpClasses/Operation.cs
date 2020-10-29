﻿using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
 using TimeManagement.ViewModels;

 namespace DM_Service
{
    class Operation : BaseViewModel
    {
        private Tasks tasks = new Tasks();

        public double Result
        {
            get
            {
                return Calculation();
            }
        }

        private string input;
        public Operation(string text)
        {
            input = text.Trim().ToLower();
        }

        private readonly string[] separators = { "+", "-", "*", "/" };
        private readonly string[] separatorsPreference = { "*", "/" };

        private string _save = "";
        public string Save
        {
            get => _save;
            set=> SetValue (ref _save, value);
        }

        private List<double> numbers;
        private List<string> operators;
        private List<string> numbers_s;

        private List<string> operators_def;
        private List<string> numbers_s_def;

        private void Extract()
        {
            Trace.Write(numbers[0]);
            for (int i = 0; i < operators.Count(); i++)
            {
                Trace.Write(operators[i] + numbers[i + 1]);
            }
            Trace.WriteLine("");
        }

        public string SavingString()
        {
            Save += (numbers[0]).ToString();
            Trace.Write(numbers[0]);
            for (int i = 0; i < operators.Count(); i++)
            {
                Trace.Write(operators[i] + numbers[i + 1]);
                Save += String.Format(operators[i] + numbers[i + 1]);
            }
            Trace.WriteLine("");
            return Save;
        }

        private double Calculation()
        {
            Trace.WriteLine("Calculation has started");
            double result = 0;
            ParsingNumbers();
            SearchingOperators();
            Trace.WriteLine("Input: " + input);
            OperatorsQuee();
            if (numbers.Count() != operators.Count() + 1)
            {
                throw new ArgumentException("wrong input");
            }
            SavingString();
            Preference();
            if (operators.Count() > 0)
            {
                for (int i = 0; i <= operators.Count(); i++)
                {
                    i = 0;
                    Calculation(i);
                }
            }
            result = numbers[0];
            Trace.WriteLine("Calculation has ended");
            Trace.WriteLine("**************");
            return result;
        }

        private void OperatorsExtract()
        {
            for (int a = 0; a < operators.Count(); a++)
            {
                Trace.Write(operators[a] + " ");
            }
            Trace.WriteLine("");
            if (!(numbers_s == numbers_s_def && operators == operators_def))
            {
                Save = "EDIT: ";
            }
        }

        private void OperatorsQuee()
        {
            Trace.WriteLine("OperatorsQuee has started");
            while (input.StartsWith("+") || input.StartsWith("-") || input.StartsWith("*") || input.StartsWith("/"))
            {
                if (input.StartsWith("-") && numbers.Count() > 0)
                {
                    numbers[0] = -1 * numbers[0];
                }
                operators.RemoveAt(0);
                input = input.Remove(0, 1);
                numbers_s.RemoveAt(0);
                OperatorsExtract();
            }

            while (input.EndsWith("+") || input.EndsWith("-") || input.EndsWith("*") || input.EndsWith("/"))
            {
                operators.RemoveAt(operators.Count() - 1);
                input = input.Remove(input.Count() - 1, 1);
                numbers_s.RemoveAt(numbers_s.Count() - 1);
                OperatorsExtract();
            }

            for (int i = 0; i < operators.Count(); i++)
            {
                if (numbers_s[i] == "")
                {
                    if (operators[i] == "+" || operators[i] == "-")
                    {
                        if ((operators[i] == "-") && (numbers.Count() > 0))
                        {
                            numbers[i] = numbers[i] * -1;
                        }
                    }
                    operators.RemoveAt(i);
                    numbers_s.RemoveAt(i);
                    OperatorsExtract();

                    if (!(i >= operators.Count()))
                    {
                        i = 0;
                    }
                }
            }
            Trace.WriteLine("OperatorsQuee has ended");
        }

        private void ParsingNumbers()
        {
            Trace.WriteLine("ParsingNumbers has started");
            numbers_s = new List<string>(input.Split(separators, StringSplitOptions.None));
            numbers_s_def = new List<string>(numbers_s);
            numbers = new List<double>();
            foreach (string cislo_s in numbers_s)
            {
                double number;
                if (!double.TryParse(cislo_s, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.GetCultureInfo("cs"), out number))
                {
                    if (cislo_s != "")
                    {
                        throw new ArgumentException("invalid input");
                    }
                }
                else
                {
                    numbers.Add(number);
                }
            }
            Trace.WriteLine("ParsingNumbers has ended");
        }

        private void SearchingOperators()
        {
            Trace.WriteLine("SearchingOperators has started");
            operators = new List<string>();

            for (int i = 0; i < input.Count(); i++)
            {
                if (separators.Contains((input[i]).ToString()))
                {
                    operators.Add(input[i].ToString());
                }
            }
            operators_def = new List<string>(operators);
            Trace.WriteLine("SearchingOperators has ended");
        }

        private void Preference()
        {
            Trace.WriteLine("Preference has started");
            for (int i = 0; i < operators.Count(); i++)
            {
                if (separatorsPreference.Contains(operators[i]))
                {
                    Calculation(i);
                    i = 0;
                }
            }
            Trace.WriteLine("Preference has ended");
        }

        private void Calculation(int i)
        {
            double intermediateResult = tasks.DecisionMaking((double)numbers[i], (double)numbers[i + 1], operators[i]);
            numbers[i] = intermediateResult;
            if (operators.Count > 1)
            {
                numbers.RemoveAt(i + 1);
                operators.RemoveAt(i);
                Extract();
            }
            else
            {
                operators.RemoveAt(i);
                Trace.WriteLine(numbers[i]);
            }
        }
    }
}
