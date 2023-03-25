class MyCanvas
{
	private const int LCDWidth = 35;
	private const int LCDHeight = 17;
	private int HCurret = 1;
	private List<IMyTextPanel> LCDs = new List<IMyTextPanel>();

	public void AddLCD(IMyTextPanel LCD)
	{
		LCDs.Add(LCD);
	}

	public void WriteLine(string Text)
	{
		int Index = HCurret / LCDHeight;
		if (Index < LCDs.Count)
		{
			LCDs[Index].WriteText(Text + "\n\r", true);
			++HCurret;
		}
	}
	public void Write(string Text)
	{
		int Index = HCurret / LCDHeight;
		if (Index < LCDs.Count)
		{
			LCDs[Index].WriteText(Text, true);
		}
	}

	public void Clear()
	{
		foreach (var LCD in LCDs)
		{
			LCD.WriteText("");
		}
		HCurret = 1;
	}

	public void MakeProgressBar(double Load)
	{
		Write("[ ");
		double Width = LCDWidth * 2.43;
		for(double i = 1.0; i <= Width; ++i)
		{
			Write(i / Width >= Load ? "'" : "|");
		}
		Write(" ]");
		WriteLine("");
	}
}

public void Main(string args)
{
	string[] arguments = args.Trim().Split(' ');
	var Canvas = new MyCanvas();

	if (arguments.Length < 1)
	{
		Echo("You have to pass a name almost one LCD panel, or more to get some result");
		return;
	}

	for (int i = 0; i < arguments.Length; ++i)
	{
		var LCD = GridTerminalSystem.GetBlockWithName(arguments[i]) as IMyTextPanel;
		if (LCD == null)
		{
			Echo("Can't find a LCD panel with name: " + arguments[i]);
			return;
		}
		Canvas.AddLCD(LCD);
	}

	Canvas.Clear();

	List<IMyGasTank> Tanks = new List<IMyGasTank>();
	if (Tanks == null)
		return;

	GridTerminalSystem.GetBlocksOfType<IMyGasTank>(Tanks);

	foreach (var Tank in Tanks)
	{
		Canvas.WriteLine(Tank.CustomName + ": " + (int)(Tank.FilledRatio * 100.0) + "%");
		Canvas.MakeProgressBar(Tank.FilledRatio);
	}
}