using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardView : MonoBehaviour, IPointerClickHandler{
    public delegate void ClickDelegate(GameObject go);
    public ClickDelegate onClickFunc;

    private bool canClick = true;
    public bool CanClick
    {
        get { return canClick; }
        set { canClick = value; }
    }

    private int id;
    public int Id
    {
        get { return id; }
    }

    private Sprite frontImg;
    private Sprite backImg;

    private Image showImg;
    public Button cardBtn;

    // Use this for initialization
    void Awake () {
        showImg = GetComponent<Image>();
        cardBtn = GetComponent<Button>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetView(int Id, Sprite FrontImg, Sprite BackImg)
    {
        this.id = Id;
        this.frontImg = FrontImg;
        this.backImg = BackImg;

        SetRecover();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!canClick) return;
        Debug.Log("OnPointerClick");
        if (onClickFunc != null) onClickFunc(gameObject);
    }

    public void SetFront()
    {
        showImg.sprite = frontImg;
        cardBtn.interactable = false;
        canClick = false;
    }

    public void SetRecover()
    {
        showImg.sprite = backImg;
        cardBtn.interactable = true;
        canClick = true;
    }
}
