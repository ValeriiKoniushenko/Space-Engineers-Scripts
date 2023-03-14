Dictionary<string, int> GetAllItemsFromAllCargos()
{
	List<IMyCargoContainer> Containers = new List<IMyCargoContainer>();
	GridTerminalSystem.GetBlocksOfType<IMyCargoContainer>(Containers);

	Dictionary<string, int> ItemsInAllContainers = new Dictionary<string, int>();

	foreach (IMyCargoContainer Container in Containers)
	{
		IMyInventory Inventory = Container.GetInventory();

		List<MyInventoryItem> Items = new List<MyInventoryItem>();
		Inventory.GetItems(Items);

		for (int i = 0; i < Items.Count; ++i)
		{
			if (ItemsInAllContainers.ContainsKey(Items[i].Type.SubtypeId))
			{
				ItemsInAllContainers[Items[i].Type.SubtypeId] += (int)Items[i].Amount;
			}
			else
			{
				ItemsInAllContainers.Add(Items[i].Type.SubtypeId, (int)Items[i].Amount);
			}
		}
	}

	return ItemsInAllContainers;
}

public void Main(string args)
{
	string[] arguments = args.Split(' ');

	if (arguments.Length != 3)
	{
		Echo("You forget to put an argument list: <From> <To> <LCD-Panel-Name>\n\rWhere:\n\rFrom - from which number of a material show it\n\rTo - before which material show it\n\rLCD-Panel-Name - where to put a message result");
		return;
	}

	Dictionary<string, int> Items = GetAllItemsFromAllCargos();

	int From = int.Parse(arguments[0]);
	int To = Items.Count;
	int i = 0;
	if (arguments.Length != 0 && int.Parse(arguments[1]) != 0)
		To = int.Parse(arguments[1]);

	IMyTextPanel LCD = GridTerminalSystem.GetBlockWithName(arguments[2]) as IMyTextPanel;
	LCD.WriteText("=== | Materials | ===\n\r");
	foreach (var Item in Items)
	{
		if (i >= From && i < To)
		{
			LCD.WriteText(Item.Key + ": " + Item.Value + "\n\r", true);
		}

		++i;
	}
}