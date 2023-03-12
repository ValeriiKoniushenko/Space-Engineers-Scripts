public void Main(string args)
{
	IMyTextPanel LCD = GridTerminalSystem.GetBlockWithName("HydrogenLCD") as IMyTextPanel;

	List<IMyGasTank> Tanks = new List<IMyGasTank>();
	if (Tanks == null)
		return;

	GridTerminalSystem.GetBlocksOfType<IMyGasTank>(Tanks);

	LCD.WriteText("======= | Gas Tanks | ========\n\r");
	double Ratios = 0;
	for (int i = 0; i < Tanks.Count; ++i)
	{
		LCD.WriteText("Tank "+ "'" + Tanks[i].CustomName + "'" + " #" + i + ": " + (int)(Tanks[i].FilledRatio * 100.0) + "% \n\r", true);
		Ratios += Tanks[i].FilledRatio;
	}

	LCD.WriteText("Total: " + (int)(Ratios / Tanks.Count * 100.0) + "% \n\r", true);
}