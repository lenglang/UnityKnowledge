using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Item : MonoBehaviour {
	public int itemType;
	private Sprite uisprite;
	public LPoint pos;
	public ItemManager itemManager;
	public int hasItem;
    private Image _image;
    void Awake()
    {
        uisprite = Resources.Load<Sprite>("ico0") as Sprite;
        _image = this.GetComponent<Image>();
    }

	/**设置item类型*/
	public void SetItemType(int type)
	{
        itemType = type;
        uisprite = Resources.Load<Sprite>("ico" + type) as Sprite;
        _image.sprite = uisprite;
	}

	public void ClickItem()
	{
		itemManager.ClickItem (this);
	}
	public void xiaohui()
	{
		Destroy (this.gameObject);
	}
}
