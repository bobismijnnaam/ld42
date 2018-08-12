using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceGameController : Activatable {

    public Player player;
    public SpaceBar spaceBar;
    public TextMesh spaceCountLabel;
    public GameObject noSpaceBarText;
    public GameObject shopReticules;
    public GameObject textMeshPrefab;
    public GameObject spaceGameObject;
    public Camera screenCamera;
    public SpareDeliverer spareDeliverer;
    public TextMesh deliveryTomorrowText;

    public float approachSpeedPercentage = 0.20f;

    private int spaceCount;
    private float shopReticuleYTarget = 1.13f;
    private int shopItemIndex = 0;
    private Dictionary<int, ShopItem> shopItemIndices;
    private Dictionary<ShopItem, ShopItemInfo> shopItemDatas;
    private double lastAutoSpaceTick;

    enum State {
        PLAYING,
        NOT_PLAYING
    }

    private State state = State.NOT_PLAYING;

    enum ShopItem {
        AUTO_SPACE,
        SWITCH_POWER,
        ORDER_SPARES,
        DURABILITY
    }

    class ShopItemInfo {
        public string txt;
        public int cost;
        public int growthFactor;
        public int level = 0;
        public GameObject labelTextMesh;
        public GameObject priceTextMesh;
    }

	// Use this for initialization
	void Start () {
        shopItemIndices = new Dictionary<int, ShopItem>();
        shopItemIndices[0] = ShopItem.AUTO_SPACE;
        shopItemIndices[1] = ShopItem.SWITCH_POWER;
        shopItemIndices[2] = ShopItem.ORDER_SPARES;
        shopItemIndices[3] = ShopItem.DURABILITY;

        shopItemDatas = new Dictionary<ShopItem, ShopItemInfo>();
        {
            var sh = new ShopItemInfo();
            sh.txt = "AutoSpace";
            sh.cost = 10;
            sh.growthFactor = 5;
            shopItemDatas[ShopItem.AUTO_SPACE] = sh;
        }

        {
            var sh = new ShopItemInfo();
            sh.txt = "Switch Power";
            sh.cost = 10;
            sh.growthFactor = 80;
            shopItemDatas[ShopItem.SWITCH_POWER] = sh;
        }

        {
            var sh = new ShopItemInfo();
            sh.txt = "Order spares online";
            sh.cost = 100;
            sh.growthFactor = 10;
            shopItemDatas[ShopItem.ORDER_SPARES] = sh;
        }

        {
            var sh = new ShopItemInfo();
            sh.txt = "Durability";
            sh.cost = 10;
            sh.growthFactor = 100;
            shopItemDatas[ShopItem.DURABILITY] = sh;
        }

        instantiateShopLabels();

        lastAutoSpaceTick = Time.time;
	}

    void instantiateShopLabels() {
        float ySpacing = 0.53f;
        Vector3 currentPos = new Vector3(-4.5f, 1.49f, -0.31f);
        Vector3 currentPricePos = new Vector3(1.77f, 1.49f, -0.31f);

        for (int i = 0; i < 4; ++i) {
            var sh = shopItemDatas[shopItemIndices[i]];

            var textMesh = Instantiate(textMeshPrefab, spaceGameObject.transform);
            textMesh.transform.localPosition = currentPos - Vector3.zero;
            textMesh.GetComponent<TextMesh>().text = sh.txt;

            var priceTextMesh = Instantiate(textMeshPrefab, spaceGameObject.transform);
            priceTextMesh.transform.localPosition = currentPricePos - Vector3.zero;
            priceTextMesh.GetComponent<TextMesh>().text = sh.cost + "";

            sh.priceTextMesh = priceTextMesh;
            sh.labelTextMesh = textMesh;

            currentPos.y -= ySpacing;
            currentPricePos.y -= ySpacing;
        }
    }
	
    void spaceBarPressed() {
        if (spaceBar.isAvailable()) {
            spaceBar.doSink();
            spaceCount += getCurrentSpacePower();
            updateSpaceCountLabel();
        }
    }

    int getCurrentSpacePower() {
        var shi = shopItemDatas[ShopItem.SWITCH_POWER];
        var spacePowers = new int [] {1, 2, 5, 20, 100};
        return spacePowers[shi.level];
    }

    void updateSpaceCountLabel() {
        spaceCountLabel.text = spaceCount + "";
    }

    int getCurrentMaxSinks() {
        var shi = shopItemDatas[ShopItem.DURABILITY];
        var maxSinks = new int [] {100, 200, 500, 1000, 2000};
        return maxSinks[shi.level];
    }

	// Update is called once per frame
	void Update () {
        if (state == State.PLAYING) {
            // Game logic
            if (Input.GetKeyDown("space")) {
                spaceBarPressed();
            }

            if (spaceBar.isBroken() && Input.GetKeyDown("r")) {
                if (player.getNumSpacebars() > 0) {
                    spaceBar.repair();
                    player.takeASpaceBar();
                } else {
                    noSpaceBarText.GetComponent<BlinkComponent>().blinkOnce(1f);
                }
            }

            if (Input.GetKeyDown("up")) {
                shopReticuleYTarget += 0.53f;
                shopItemIndex -= 1;
            }
            if (Input.GetKeyDown("down")) {
                shopReticuleYTarget -= 0.53f;
                shopItemIndex += 1;
            }

            shopItemIndex = Mathf.Clamp(shopItemIndex, 0, 3);

            if (Input.GetKeyDown("return")) {
                var sh = getCurrentShopItemInfo();
                if (sh.cost <= spaceCount) {
                    buyShopItem(sh);

                    if (getCurrentShopItem() == ShopItem.AUTO_SPACE) {
                        lastAutoSpaceTick = Time.time;
                    }
                    if (getCurrentShopItem() == ShopItem.DURABILITY) {
                        spaceBar.setMaxSinks(getCurrentMaxSinks());
                    }
                    if (getCurrentShopItem() == ShopItem.ORDER_SPARES) {
                        spareDeliverer.addOrder();
                        deliveryTomorrowText.gameObject.SetActive(true);
                    }
                }
            }
            
            if (Input.GetKeyDown("n")) {
                spaceCount += 100;
                updateSpaceCountLabel();
            }
        }	

        updateShopReticule();

        updateAutoSpace();
	}

    void updateAllShopItems() {
        for (int i = 0; i < 4; ++i) {
            var sh = shopItemDatas[shopItemIndices[shopItemIndex]];
            updateShopItem(sh);
        }
    }

    void updateShopItem(ShopItemInfo shi) {
        var labelTM = shi.labelTextMesh.GetComponent<TextMesh>();
        var priceTM = shi.priceTextMesh.GetComponent<TextMesh>();

        priceTM.text = shi.cost + "";

        if (spaceCount >= shi.cost) {
            labelTM.color = new Color(0.7f, 0.7f, 0.7f);
        } else {
            labelTM.color = new Color(1, 1, 1);
        }
    }

    void buyShopItem(ShopItemInfo shi) {
        shi.level++;
        spaceCount -= shi.cost;
        shi.cost *= shi.growthFactor;

        updateSpaceCountLabel();
        updateAllShopItems();
    }

    ShopItem getCurrentShopItem() {
        return shopItemIndices[shopItemIndex];
    }

    ShopItemInfo getCurrentShopItemInfo() {
        return shopItemDatas[getCurrentShopItem()];
    }

    void updateShopReticule() {
        shopReticuleYTarget = Mathf.Min(1.13f, Mathf.Max(shopReticuleYTarget, -0.4f));

        var currentPosition = shopReticules.transform.position;
        currentPosition.y = currentPosition.y + (shopReticuleYTarget - currentPosition.y) * approachSpeedPercentage * Time.deltaTime;
        shopReticules.transform.position = currentPosition;
    }

    void updateAutoSpace() {
        var shi = shopItemDatas[ShopItem.AUTO_SPACE];
        var spacesPerSecArr = new int [] {1, 10, 100, 1000};
        if (shi.level > 0) {
            var spacesPerSec = spacesPerSecArr[shi.level - 1];
            var timePerTick = 1 / (double) spacesPerSec;
            var timeDiff = Time.time - lastAutoSpaceTick;
            if (timeDiff >= timePerTick) {
                var leftover = timeDiff % timePerTick;
                int numSpacesToAdd = (int) System.Math.Ceiling(((timeDiff - leftover) / timePerTick));
                spaceCount += numSpacesToAdd;
                updateSpaceCountLabel();
                lastAutoSpaceTick = Time.time - leftover;
            }
        }
    }

    public override string getDescription() {
        if (state == State.PLAYING) {
            return "to leave";
        } else {
            return "to play";
        }
    }

    public override void activate() {
        if (state == State.PLAYING) {
            player.setViewWalkEnabled(true);
            screenCamera.enabled = false;
            state = State.NOT_PLAYING;
        } else if (state == State.NOT_PLAYING) {
            player.setViewWalkEnabled(false);
            screenCamera.enabled = true;
            state = State.PLAYING;
        }
    }
}
