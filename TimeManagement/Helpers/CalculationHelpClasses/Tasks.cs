﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DM_Service
{
    class Tasks
    {
        //if we add functionality, we have to add the separators
        public double DecisionMaking(double a, double b, string separator)
        {
            double intermediateResult = 0;

            if (separator == "+")
            {
                intermediateResult = Addition(a, b);
            }
            else if (separator == "-")
            {
                intermediateResult = Subtraction(a, b);
            }
            else if (separator == "*")
            {
                intermediateResult = Multiplication(a, b);
            }
            else if (separator == "/")
            {
                intermediateResult = Division(a, b);
            }
            else
            {
                throw new ArgumentException("unexpected error");
            }
            return intermediateResult;
        }

        private double Addition(double a, double b)
        {
            double intermediateResult = a + b;
            return intermediateResult;
        }

        private double Subtraction(double a, double b)
        {
            double intermediateResult = a - b;
            return intermediateResult;
        }

        private double Multiplication(double a, double b)
        {
            double intermediateResult = a * b;
            return intermediateResult;
        }

        private double Division(double a, double b)
        {
            double intermediateResult = 0;
            if (b == 0)
            {
                throw new ArgumentException("division by zero");
            }
            else
            {
                intermediateResult = a / b;
            }
            return intermediateResult;
        }
    }
}
