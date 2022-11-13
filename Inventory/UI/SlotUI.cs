using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace MFarm.Inventory{
    public class SlotUI : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("组件获取")]
        [SerializeField] private Image slotImage;
        [SerializeField] private TextMeshProUGUI amountText;
        public Image slotHighlight;
        [SerializeField] private Button button;

        [Header("格子类型")]
        public SlotType slotType;
        public bool isSelected;
        public int slotIndex;

        //物品信息
        public ItemDetails itemDetails;
        public int itemAmount;

        private InventoryUI inventoryUI => GetComponentInParent<InventoryUI>();

        private void Start() 
        {
            isSelected = false;

            if(itemDetails.itemID == 0)
            {
                UpdateEmptySlot();
            }
        }

    /// <summary>
    /// 更新格子UI和数量
    /// </summary>
    /// <param name="item">ItemDetails</param>
    /// <param name="amount">持有数量</param>
        public void UpdateSlot(ItemDetails item, int amount)
        {
            itemDetails = item;
            slotImage.sprite = item.itemIcon;
            itemAmount = amount;
            amountText.text = amount.ToString();
            slotImage.enabled = true;
            button.interactable = true;
        }

        /// <summary>
        /// 将slot更新为空
        /// </summary>
        public void UpdateEmptySlot()
        {
            if(isSelected)
            {
                isSelected = false;
            }

            slotImage.enabled = false;
            amountText.text = string.Empty; //or = ""
            button.interactable = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(itemAmount == 0) return;
            isSelected = !isSelected;

            inventoryUI.UpdateSlotHighlight(slotIndex);

            if(slotType == SlotType.Bag)
            {
                //通知物品被选中的状态和信息
                EventHandler.CallItemSelectedEvent(itemDetails, isSelected);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if(itemAmount != 0)
            {
                inventoryUI.dragItem.enabled = true;
                inventoryUI.dragItem.sprite = slotImage.sprite;
                inventoryUI.dragItem.SetNativeSize();
                
                isSelected = true;
                inventoryUI.UpdateSlotHighlight(slotIndex);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            inventoryUI.dragItem.transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            inventoryUI.dragItem.enabled = false;
            //Debug.Log(eventData.pointerCurrentRaycast.gameObject);

            if(eventData.pointerCurrentRaycast.gameObject != null)
            {
                if(eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>() == null)
                    return;

                var targetSlot = eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>();
                int targetIndex = targetSlot.slotIndex;
                
                //在Player自身背包范围内交换
                if(slotType == SlotType.Bag && targetSlot.slotType == SlotType.Bag)
                {
                    InventoryManager.Instance.SwapItem(slotIndex, targetIndex);
                }

                //刷新选择高亮，没有格子被选中
                inventoryUI.UpdateSlotHighlight(-1);
            }
            // else //测试扔在地上
            // {
            //     if (itemDetails.canDropped)
            //     {
            //         var pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));

            //         EventHandler.CallInstantiateItemInScene(itemDetails.itemID, pos);
            //     }
            // }
        }
    }
}