        Dictionary<string, int> GetAllItemsInTheShip()
        {
            var Blocks = new List<IMyTerminalBlock>();
            GridTerminalSystem.GetBlocksOfType<IMyTerminalBlock>(Blocks);

            var ItemsInTheShip = new Dictionary<string, int>();

            foreach (var Block in Blocks)
            {
                var Inventory = Block.GetInventory();
                if (Inventory != null)
                {
                    var Items = new List<MyInventoryItem>();
                    Inventory.GetItems(Items);

                    foreach (var Item in Items)
                    {
                        if (ItemsInTheShip.ContainsKey(Item.Type.SubtypeId))
                        {
                            ItemsInTheShip[Item.Type.SubtypeId] += Item.Amount.ToIntSafe();
                        }
                        else
                        {
                            ItemsInTheShip.Add(Item.Type.SubtypeId, Item.Amount.ToIntSafe());
                        }

                    }
                }
            }

            return ItemsInTheShip;
        }

        bool ContainSomeFilter(string Key, List<string> Filter )
        {
            foreach(var I in Filter)
            {
                if (Key.Contains(I) || I == "All")
                {
                    return true;
                }
            }
            return false;
        }

        public void Main(string args)
		{
            string[] arguments = args.Split(' ');

            if (arguments.Length < 3)
            {
                Echo("You have to pass a name of LCD panel and a caption to get data");
                return;
            }

            IMyTextPanel LCD = GridTerminalSystem.GetBlockWithName(arguments[0]) as IMyTextPanel;
            if (LCD == null)
            {
                Echo("Can't find such LCD panel");
                return;
            }

            LCD.WriteText(arguments[1] + "\n\r");

            var ItemsInTheShip = GetAllItemsInTheShip();

            var Filter = new List<string>();
            for (int i = 2; i < arguments.Length; ++i)
            {
                Filter.Add(arguments[i]);
            }
            
            foreach (var Item in ItemsInTheShip)
            {
                if (ContainSomeFilter(Item.Key, Filter))
                {
                    LCD.WriteText(Item.Key + " " + ((float)(Item.Value)).ToString("0.00") + "\n\r", true);
                }
            }
		}