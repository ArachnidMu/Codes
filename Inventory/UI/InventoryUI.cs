using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MFarm.Inventory
{
    public class InventoryUI : MonoBehaviour
    {
        public ItemTooltip itemTooltip;
        
        [Header("拖拽图片")]
        public Image dragItem;

        [Header("玩家背包UI")]
        [SerializeField] private GameObject bagUI;
        private bool bagOpened;

        [SerializeField] private SlotUI[] playerSlots;

        private void OnEnable() 
        {
            EventHandler.UpdateInventoryUI += OnUpdateInventoryUI;
            EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        }

        private void OnDisable() 
        {
            EventHandler.UpdateInventoryUI -= OnUpdateInventoryUI;
            EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
        }

        private void Start() 
        {
            //给每一个格子序号
            for (int i = 0; i < playerSlots.Length;i++)
            {
                playerSlots[i].slotIndex = i;
            }

            bagOpened = bagUI.activeInHierarchy;
        }
        private void Update() 
        {
            if(Input.GetKeyDown(KeyCode.B))
            {
                openBagUI();
            }
        }

        private void OnBeforeSceneUnloadEvent()
        {
            UpdateSlotHighlight(-1);
        }

        private void OnUpdateInventoryUI(InventoryLocation location, List<InventoryItem> list)
        {
            switch(location)
            {
                case InventoryLocation.Player:
                    for (int i = 0; i < playerSlots.Length; i++)
                    {
                        if(list[i].itemAmount>0)
                        {
                            var item = InventoryManager.Instance.GetItemDetails(list[i].itemID);
                            playerSlots[i].UpdateSlot(item, list[i].itemAmount);
                        }
                        else
                        {
                            playerSlots[i].UpdateEmptySlot();
                        }
                    }
                    break;
            }
        }
    
        /// <summary>
        /// 打开关闭背包UI，Button调用该事件
        /// </summary>
        public void openBagUI()
        {
            bagOpened = !bagOpened;

            bagUI.SetActive(bagOpened);
        }

        public void UpdateSlotHighlight(int index)
        {
            foreach(var slot in playerSlots)
            {
                if(slot.isSelected && slot.slotIndex == index)
                {
                    slot.slotHighlight.gameObject.SetActive(true);
                }
                else
                {
                    slot.isSelected = false;
                    slot.slotHighlight.gameObject.SetActive(false);
                }
            }
        }
    }
}
