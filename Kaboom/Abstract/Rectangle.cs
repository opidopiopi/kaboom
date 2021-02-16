﻿using Kaboom.Abstract.Exceptions;
using System;

namespace Kaboom.Abstract
{
    public class Rectangle
    {
        public int X { get; }
        public int Y { get; }
        public int Width { get; }
        public int Height { get; }

        public Rectangle(int x, int y, int width, int height)
        {
            if (width <= 0 || height <= 0)
            {
                throw new ValueIsNegativeOrZero(
                    string.Format(
                        "One or both of the following values is invalid: width: {0} height: {1}. Only non zero positive values are allowed!",
                        width,
                        height
                    ));
            }

            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public bool IsOtherRectangleInside(Rectangle otherRectangle)
        {
            return  (X <= otherRectangle.X && otherRectangle.X <= X + Width) &&
                    (Y <= otherRectangle.Y && otherRectangle.Y <= Y + Height);
        }

        public override String ToString()
        {
            return $"Rectangle(X: {X}, Y: {Y}, Width: {Width}, Height: {Height})";
        }
    }
}