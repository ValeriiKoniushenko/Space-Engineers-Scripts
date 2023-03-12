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

	Dictionary<string, int> Items = GetAllItemsFromAllCargos();


	int From = 0;
	int To = Items.Count;
	int i = 0;

	if (arguments.Length != 0 && int.Parse(arguments[1]) != 0)
		To = int.Parse(arguments[1]);

	IMyTextPanel LCD = GridTerminalSystem.GetBlockWithName("ContainerStatus") as IMyTextPanel;			
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