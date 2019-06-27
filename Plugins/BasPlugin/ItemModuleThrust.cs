﻿using BS;

namespace BasPluginExample
{
    // This create an item module that can be referenced in the item JSON
    public class ItemModuleThrust : ItemModule
    {
        public float minForce = 500;
        public float maxForce = 1500;

        public override void OnItemLoaded(Item item)
        {
            base.OnItemLoaded(item);
            item.gameObject.AddComponent<ItemThrust>();
        }
    }
}
