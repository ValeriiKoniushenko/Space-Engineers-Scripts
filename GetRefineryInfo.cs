string GetOreStringFromInventory(MyInventoryItem Item)
{
	string CountOfOre = ((int)Item.Amount).ToString();
	string OreName = Item.Type.SubtypeId.ToString();
	return OreName + ": " + CountOfOre + "kg\n\r";
}

public void Main(string args)
{
	IMyTextPanel LCD = GridTerminalSystem.GetBlockWithName("OreStatusLCDPanel") as IMyTextPanel;

	List<IMyRefinery> Refineries = new List<IMyRefinery>();
	GridTerminalSystem.GetBlocksOfType<IMyRefinery>(Refineries);

	IMyInventory Inventory = Refineries[int.Parse(args)].GetInventory();

	List<MyInventoryItem> Items = new List<MyInventoryItem>();
	Inventory.GetItems(Items);

	LCD.WriteText("=== | Refinery | ===\n\r");
	for (int i = 0; i < Items.Count; ++i)
	{
		LCD.WriteText(GetOreStringFromInventory(Items[i]), true);
	}
}