using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Media;

namespace TechnoSystem.Models;

public partial class Tarif
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int ServiceId { get; set; }

    public int MonthCount { get; set; }

    public DateOnly Date { get; set; }

    public int Price { get; set; }

    public int UserLimit { get; set; }

    public int FreeLicense { get; set; }

    public string? Image { get; set; }
    public string FullImagePath => !string.IsNullOrEmpty(Image) ? $"Images/{Image}" : "Images/icon.png";

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();

    public virtual Service Service { get; set; } = null!;

    public FontWeight TitleFontWeight
    {
        get
        {
            var today = new DateTime(2024, 7, 6);
            var daysLeft = Date.DayNumber - DateOnly.FromDateTime(today).DayNumber;
            return daysLeft is > 0 and < 7 ? FontWeights.Bold : FontWeights.Normal;
        }
    }

    public Brush backGroundColor
    {
        get
        {
            if (UserLimit > 0 && FreeLicense < (int)(UserLimit * 0.1))
            {
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFB6C1"));
            }
            return new SolidColorBrush(Colors.White);
        }
    }
}
