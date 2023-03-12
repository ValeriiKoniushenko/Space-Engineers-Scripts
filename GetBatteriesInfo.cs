public void Main(string args)
{
	IMyTextPanel LCD = GridTerminalSystem.GetBlockWithName("HydrogenLCD") as IMyTextPanel;

	List<IMyBatteryBlock> Batteries = new List<IMyBatteryBlock>();
	if (Batteries == null)
		return;

	GridTerminalSystem.GetBlocksOfType<IMyBatteryBlock>(Batteries);

	LCD.WriteText("======= | Batteries | ========\n\r");
	double TotalCurrentStoredPower = 0;
	for (int i = 0; i < Batteries.Count; ++i)
	{
		int Ratio = (int)(Batteries[i].CurrentStoredPower / Batteries[i].MaxStoredPower * 100);
		LCD.WriteText("Battery "+ "'" + Batteries[i].CustomName + "'" + " #" + i + ": " + (int)(Batteries[i].CurrentStoredPower) + "MW (" + Ratio + "%)\n\r", true);
		TotalCurrentStoredPower += Batteries[i].CurrentStoredPower;
	}

	LCD.WriteText("Total: " + (int)(TotalCurrentStoredPower) + "MW \n\r", true);
}