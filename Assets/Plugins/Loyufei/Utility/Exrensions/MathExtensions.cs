using System;
using System.Collections;
using System.Collections.Generic;

namespace Loyufei
{
    public static class MathExtensions
    {
        public static int Pow(this int self, int pow) 
        {
            return (int)Math.Pow(self, pow);
        }

        public static float Pow(this float self, float pow)
        {
            return (float)Math.Pow(self, pow);
        }

        public static double Pow(this double self, double pow)
        {
            return Math.Pow(self, pow);
        }

        public static int Clamp(this int self, int min, int max) 
        {
            return Math.Clamp(self, min, max);
        }

        public static float Clamp(this float self, float min, float max)
        {
            return Math.Clamp(self, min, max);
        }

        public static double Clamp(this double self, double min, double max)
        {
            return Math.Clamp(self, min, max);
        }

        public static bool IsClamp(this int self, int min, int max) 
        {
            return self == self.Clamp(min, max);
        }

        public static bool IsClamp(this float self, float min, float max)
        {
            return self == self.Clamp(min, max);
        }

        public static bool IsClamp(this double self, double min, double max)
        {
            return self == self.Clamp(min, max);
        }
    }
}