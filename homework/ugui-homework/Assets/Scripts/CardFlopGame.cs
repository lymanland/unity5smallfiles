using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardFlopGame : MonoBehaviour
{
    private readonly float TimeSec4MatchFail = 1.5f;//翻牌没配对要暂停1.5秒
    private readonly int TimeSec4WaitStart = 5;
    private int waitStartSec;
    public Text msgText;
    public Button startBtn;

    public List<Sprite> showImgs;//inspector拖动方式
    public Sprite backImg;//inspector拖动方式

    public Transform cardViewContainer;
    private int maxPairsCount;
    private List<CardView> cardViewList;
    private List<CardView> choosedCards;

    private int matchedPairsCount = 0;
    private bool canClick = false;


    private string[] picNames = new string[] { "1", "2", "3", "4", "5", "6", "7", "8"};
    // Use this for initialization
    void Start()
    {

        showImgs = new List<Sprite>();
        for (int ri = 0; ri < picNames.Length; ri++)
        {
            string resourcePath = picNames[ri];
            Sprite loadedAsset = Resources.Load<Sprite>(resourcePath);//动态加载方式
            showImgs.Add(loadedAsset);
        }


        cardViewList  = new List<CardView>();
        choosedCards = new List<CardView>();

        maxPairsCount = cardViewContainer.childCount / 2;

        for (int j = 0; j < cardViewContainer.childCount; j++)
        {
            GameObject cardObj = cardViewContainer.GetChild(j).gameObject;
            CardView cardView = cardObj.GetComponent<CardView>();
            AddCardViewClickListener(cardObj);

            cardViewList.Add(cardView);
        }

        StartGame();
        startBtn.onClick.AddListener(() => StartGame());
    }

    void AddCardViewClickListener(GameObject cardObj)
    {
        CardView cardView = cardObj.GetComponent<CardView>();

        /***方式一
        //cardView.cardBtn.onClick.AddListener(() => OnCardViewClick(cardView));
        cardView.cardBtn.onClick.AddListener(delegate ()
        {
            OnCardViewClick(cardView);
        });
        **/

        //方式二
        //EventTriggerListener.Get(cardView.gameObject).onClick = OnCardViewClickEventTrigger;
        
        //方式三
        cardView.onClickFunc = OnCardViewClickEventTrigger;
    }

    void OnCardViewClickEventTrigger(GameObject cardObj)
    {
        CardView cardView = cardObj.GetComponent<CardView>();
        OnCardViewClick(cardView);
    }

    void StartGame()
    {
        waitStartSec = TimeSec4WaitStart;

        StopAllCoroutines();
        ResetGame();

        //StartBtn.gameObject.SetActive(false);
        StartCoroutine(StartCountDown());
    }

    IEnumerator StartCountDown()
    {
        msgText.text = waitStartSec.ToString() + "秒后开始游戏";
        //有5秒钟展示答案
        yield return new WaitForSeconds(1f);

        waitStartSec--;
        if (waitStartSec <= 0)
        {
            msgText.text = "开始游戏!";
            canClick = true;
            startBtn.gameObject.SetActive(true);

            SetCardRecoverAll();
        }
        else
        {
            StartCoroutine(StartCountDown());
        }
    }

    void SetCardRecoverAll()
    {
        for (int i = 0; i < cardViewList.Count; i++)
        {
            CardView cardView = cardViewList[i];
            cardView.SetRecover();
        }
    }

    void ResetGame()
    {
        choosedCards.Clear();
        matchedPairsCount = 0;
        canClick = false;
        //CardViewList.Sort(new SortRandom());

        //int[] arrInt = new int[PairsCount];
        List<int> arrCardIds  = new List<int>();
        for (int i = 0; i < maxPairsCount; i++)
        {
            //arrInt[i] = i;
            arrCardIds.Add(i);
            arrCardIds.Add(i);
        }
        //arrInt = arrInt.OrderBy(c => Guid.NewGuid()).ToArray<int>();

        int index = 0;
        for (int i = 0; i < cardViewList.Count; i++)
        {
            int ran = Random.Range(0, arrCardIds.Count);
            int id = arrCardIds[ran];
            arrCardIds.RemoveAt(ran);

            Sprite showImg = showImgs[id];
            CardView cardView = cardViewList[index];
            cardView.SetView(id, showImg, backImg);
            //cardView.SetRecover();
            cardView.SetFront();
            index++;
        }
    }


    private void OnCardViewClick(CardView card)
    {
        Debug.Log("canClick:"+ canClick);
        if (!canClick)
        {
            return;
        }

        msgText.text = "";
        card.SetFront();
        choosedCards.Add(card);

        if (choosedCards.Count == 2)
        {
            canClick = false;
            StartCoroutine(CheckChoosedCards());
        }
    }

    IEnumerator CheckChoosedCards()
    {
        CardView card1 = choosedCards[0];
        CardView card2 = choosedCards[1];

        if (card1.Id == card2.Id)
        {
            msgText.text = "匹配成功";
            matchedPairsCount++;
            if (matchedPairsCount == maxPairsCount)
            {
                msgText.text = "游戏胜利";
            }
        }
        else
        {
            msgText.text = "匹配错误✖";
            yield return new WaitForSeconds(TimeSec4MatchFail);
            card1.SetRecover();
            card2.SetRecover();
        }


        choosedCards.Clear(); 
        canClick = true;
    }
}

class SortRandom : IComparer
{
    public int Compare(object x, object y)
    {
        return 0;
    }
}
