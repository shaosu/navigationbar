using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using System.Windows.Input;

namespace NavigationBar;

[TemplatePart(Name = "PART_Circle", Type = typeof(Grid))]
public class MagicBar : ListBox
{
    /// <summary>
    /// Defines the <see cref="Background"/> property.
    /// </summary>
    public static readonly StyledProperty<IBrush?> FillProperty
        = StyledProperty<MagicBar>.Register<MagicBar, IBrush?>(nameof(Fill), new SolidColorBrush(Avalonia.Media.Colors.CadetBlue));

    /// <summary>
    /// Gets or sets the brush used to draw the control's Fill Color.
    /// </summary>
    public IBrush? Fill
    {
        get => GetValue(FillProperty);
        set => SetValue(FillProperty, value);
    }


    /// <summary>
    /// Defines the <see cref="FontFamily"/> property.
    /// </summary>
    public static readonly StyledProperty<FontFamily> IconFontFamilyProperty
        = StyledProperty<MagicBar>.Register<MagicBar, FontFamily>(nameof(IconFontFamily));
    /// <summary>
    /// IconFontFamily
    /// </summary>
    public FontFamily IconFontFamily
    {
        get => GetValue(IconFontFamilyProperty);
        set => SetValue(IconFontFamilyProperty, value);
    }

    private Grid Circle;
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        Circle = e.NameScope.Find<Grid>("PART_Circle") as Grid;
        Circle.Transitions = new Transitions();
        Circle.Transitions.Add(new DoubleTransition()
        {
            Easing = new Avalonia.Animation.Easings.CubicEaseInOut(),
            Property = Canvas.LeftProperty,
            Duration = new System.TimeSpan(0, 0, 0, 0, 500)
        });
        this.SelectionChanged += MyListBox_SelectionChanged;
        this.SelectionMode = SelectionMode.Toggle;
    }

    private void MyListBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        var sel = (Avalonia.Controls.ListBoxItem)this.SelectedItem;
        double dw = sel.Bounds.Width;
        var Left = this.SelectedIndex * dw;
        Circle.SetValue(Canvas.LeftProperty, Left);

        var add = (MagicBarItem)e.AddedItems[0];
        add.UseAnm(true);
        if (e.RemovedItems.Count > 0)
        {
            var rem = (MagicBarItem)e.RemovedItems[0];
            rem.UseAnm(false);
        }
    }
}

[TemplatePart(Name = "PART_Name", Type = typeof(TextBlock))]
[TemplatePart(Name = "PART_Icon", Type = typeof(TextBlock))]
public class MagicBarItem : ListBoxItem
{
    private ThicknessTransition ThicknessAnm;
    private BrushTransition BrushAnm;
    private TextBlock Name;
    private TextBlock Icon;

    /// <summary>
    /// Defines the <see cref="Command"/> property.
    /// </summary>
    public static readonly StyledProperty<ICommand?> CommandProperty =
        AvaloniaProperty.Register<MagicBar, ICommand?>(nameof(Command), enableDataValidation: true);

    /// <summary>
    /// Gets or sets an <see cref="ICommand"/> to be invoked when the button is clicked.
    /// </summary>
    public ICommand? Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    /// <summary>
    /// Defines the <see cref="CommandParameter"/> property.
    /// </summary>
    public static readonly StyledProperty<object?> CommandParameterProperty =
        AvaloniaProperty.Register<MagicBar, object?>(nameof(CommandParameter));

    /// <summary>
    /// Gets or sets a parameter to be passed to the <see cref="Command"/>.
    /// </summary>
    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }


    public MagicBarItem()
    {
        ThicknessAnm = new ThicknessTransition();
        ThicknessAnm.Easing = new Avalonia.Animation.Easings.CubicEaseInOut();
        ThicknessAnm.Property = TextBlock.MarginProperty;
        ThicknessAnm.Duration = new System.TimeSpan(0, 0, 0, 0, 500);

        BrushAnm = new BrushTransition();
        BrushAnm.Easing = new Avalonia.Animation.Easings.CubicEaseInOut();
        BrushAnm.Property = TextBlock.ForegroundProperty;
        BrushAnm.Duration = new System.TimeSpan(0, 0, 0, 0, 500);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        Name = e.NameScope.Find<TextBlock>("PART_Name") as TextBlock;
        Icon = e.NameScope.Find<TextBlock>("PART_Icon") as TextBlock;

        Name.Transitions = new Transitions();
        Name.Transitions.Add(ThicknessAnm);
        Name.Transitions.Add(BrushAnm);

        Icon.Transitions = new Transitions();
        Icon.Transitions.Add(ThicknessAnm);
        Icon.Transitions.Add(BrushAnm);
    }

    public void UseAnm(bool select)
    {
        Name.Classes.RemoveAt(1);
        Icon.Classes.RemoveAt(1);

        if (select)
        {
            Name.Classes.Add("NameSelect");
            Icon.Classes.Add("IconSelect");
            Command?.Execute(CommandParameter);
        }
        else
        {
            Name.Classes.Add("NameUnSelect");
            Icon.Classes.Add("IconUnSelect");
        }
    }
}