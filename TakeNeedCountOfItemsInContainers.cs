private int AssemblerIndex = 0;

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

Dictionary<string, int> GetNeededCountOfItems(Dictionary<string, int> Items, Dictionary<string, int> WantedItems)
{
	Dictionary<string, int> NeededCountOfItems = new Dictionary<string, int>();

	foreach (var WantedItem in WantedItems)
	{
		int NeededCount = Math.Max(Items.ContainsKey(WantedItem.Key) ? Items[WantedItem.Key] : 0, 0) - WantedItem.Value;
		if (NeededCount > 0)
			NeededCount = 0;
		NeededCount = Math.Abs(NeededCount);

		NeededCountOfItems.Add(WantedItem.Key, NeededCount);
	}

	return NeededCountOfItems;
}

void SubtructNeededCountOfItemsFromAssembler(Dictionary<string, int> Items)
{
	List<IMyAssembler> Assemblers = new List<IMyAssembler>();
	GridTerminalSystem.GetBlocksOfType<IMyAssembler>(Assemblers);

	if (Assemblers.Count == 0)
	{
		return;
	}

	List<MyProductionItem> ProductionItems = new List<MyProductionItem>();
	Assemblers[AssemblerIndex].GetQueue(ProductionItems);

	foreach (var ItemInQueque in ProductionItems)
	{
		if (Items.ContainsKey(ItemInQueque.BlueprintId.SubtypeName))
		{
			Items[ItemInQueque.BlueprintId.SubtypeName] -= ItemInQueque.Amount.ToIntSafe();
		}
	}
}

public void Main(string args)
{
	Dictionary<string, int> Items = GetAllItemsFromAllCargos();
	Dictionary<string, int> WantedItems = new Dictionary<string, int>();
	WantedItems.Add("SteelPlate", 100);
	Dictionary<string, int> NeededCountOfItems = GetNeededCountOfItems(Items, WantedItems);

	SubtructNeededCountOfItemsFromAssembler(NeededCountOfItems);

	List<IMyAssembler> Assemblers = new List<IMyAssembler>();
	GridTerminalSystem.GetBlocksOfType<IMyAssembler>(Assemblers);

	if (Assemblers.Count == 0)
	{
		Echo("Assemblers not found");
		return;
	}

	foreach (var NeededItem in NeededCountOfItems)
	{
		MyDefinitionId Item = MyDefinitionId.Parse("MyObjectBuilder_BlueprintDefinition/" + NeededItem.Key);
		Assemblers[AssemblerIndex].AddQueueItem(Item, (double)NeededItem.Value);
	}
}