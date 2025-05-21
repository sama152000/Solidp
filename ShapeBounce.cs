using System;

public interface IShape
{
    double Area();
}

public interface IVolumetricShape
{
    double Volume();
}

public class Rectangle : IShape
{
    public double Height { get; set; }
    public double Width { get; set; }

    public Rectangle(double height, double width)
    {
        Height = height;
        Width = width;
    }

    public double Area()
    {
        return Height * Width;
    }
}

public class Square : IShape
{
    public double Side { get; set; }

    public Square(double side)
    {
        Side = side;
    }

    public double Area()
    {
        return Side * Side;
    }
}

public class Triangle : IShape
{
    public double Base { get; set; }
    public double Height { get; set; }

    public Triangle(double @base, double height)
    {
        Base = @base;
        Height = height;
    }

    public double Area()
    {
        return 0.5 * Base * Height;
    }
}

public class Circle : IShape
{
    public double Radius { get; set; }

    public Circle(double radius)
    {
        Radius = radius;
    }

    public double Area()
    {
        return Math.PI * Radius * Radius;
    }
}

public class Cube : IShape, IVolumetricShape
{
    public double Side { get; set; }

    public Cube(double side)
    {
        Side = side;
    }

    public double Area()
    {
        return 6 * Side * Side; 
    }

    public double Volume()
    {
        return Side * Side * Side;
    }
}

public class AreaCalculator
{
    public double TotalArea(IShape[] shapes)
    {
        double area = 0;
        foreach (var shape in shapes)
        {
            area += shape.Area(); 
        }
        return area;
    }
}

public class VolumeCalculator
{
    public double TotalVolume(IVolumetricShape[] shapes)
    {
        double volume = 0;
        foreach (var shape in shapes)
        {
            volume += shape.Volume();
        }
        return volume;
    }
}