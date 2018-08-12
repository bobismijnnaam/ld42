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
    public GameObject shopSlots;

    public float approachSpeedPercentage = 0.20f;
    public float shopSlotSpeedPercentage = 5f;

    private int spaceCount;
    private float shopReticuleYTarget = 1.13f;
    private int shopItemIndex = 0;
    private int shopItemSlotIndex = 0;
    private Dictionary<int, ShopItem> shopItemIndices;
    private Dictionary<ShopItem, ShopItemInfo> shopItemDatas;
    private double lastAutoSpaceTick;
    private float shopSlotXTarget = -4.67f;

    enum State {
        PLAYING,
        NOT_PLAYING
    }

    private State state = State.NOT_PLAYING;

    enum ShopItem {
        AUTO_SPACE,
        SWITCH_POWER,
        ORDER_SPARES,
        DURABILITY,
        STOCHASTIC_BOOST,
        SHRINE
    }

    class ShopItemInfo {
        public string txt;
        public int cost;
        public float growthFactor;
        public int level = 0;
        public int maxLevel;
        public bool isUnlocked = false;
        public GameObject labelTextMesh;
        public GameObject priceTextMesh;
    }

	// Use this for initialization
	void Start () {
        shopItemIndices = new Dictionary<int, ShopItem>();
        shopItemIndices[0] = ShopItem.AUTO_SPACE;
        shopItemIndices[1] = ShopItem.SWITCH_POWER;
        shopItemIndices[2] = ShopItem.DURABILITY;
        shopItemIndices[3] = ShopItem.ORDER_SPARES;

        shopItemIndices[4] = ShopItem.STOCHASTIC_BOOST;
        shopItemIndices[5] = ShopItem.SHRINE;

        shopItemDatas = new Dictionary<ShopItem, ShopItemInfo>();
        {
            var sh = new ShopItemInfo();
            sh.txt = "AutoSpace";
            sh.cost = 10;
            sh.growthFactor = 8;
            sh.maxLevel = 4;
            shopItemDatas[ShopItem.AUTO_SPACE] = sh;
        }

        {
            var sh = new ShopItemInfo();
            sh.txt = "Switch Power";
            sh.cost = 10;
            sh.growthFactor = 80;
            sh.maxLevel = 4;
            shopItemDatas[ShopItem.SWITCH_POWER] = sh;
        }

        {
            var sh = new ShopItemInfo();
            sh.txt = "Order spares online";
            sh.cost = 1000;
            sh.growthFactor = 1.01f;
            sh.maxLevel = 999999;
            shopItemDatas[ShopItem.ORDER_SPARES] = sh;
        }

        {
            var sh = new ShopItemInfo();
            sh.txt = "Durability";
            sh.cost = 500;
            sh.growthFactor = 2;
            sh.maxLevel = 4;
            shopItemDatas[ShopItem.DURABILITY] = sh;
        }

        {
            var sh = new ShopItemInfo();
            sh.txt = "Stochastic Boost";
            sh.cost = 1000;
            sh.growthFactor = 5f;
            sh.maxLevel = 3;
            shopItemDatas[ShopItem.STOCHASTIC_BOOST] = sh;
        }

        {
            var sh = new ShopItemInfo();
            sh.txt = "Space Buddha Shrine";
            sh.cost = 10000;
            sh.growthFactor = 1;
            sh.maxLevel = 1;
            shopItemDatas[ShopItem.SHRINE] = sh;
        }

        instantiateShopLabels();

        lastAutoSpaceTick = Time.time;
	}

    private const float SLOT_X_SPACING = 11;
    private const float Y_SPACING = 0.53f;

    void instantiateShopLabels() {
        var currentPos = new Vector3(0, 0, 0);
        var currentPricePos = new Vector3(6.39f, 0, 0);

        for (int slotIndex = 0; slotIndex < shopItemIndices.Count; slotIndex += 4) {
            currentPos.y = 0;
            currentPricePos.y = 0;

            for (int i = slotIndex; i < Mathf.Min(slotIndex + 4, shopItemIndices.Count); ++i) {
                var sh = shopItemDatas[shopItemIndices[i]];

                var textMesh = Instantiate(textMeshPrefab, shopSlots.transform);
                textMesh.transform.localPosition = currentPos;
                textMesh.GetComponent<TextMesh>().text = sh.txt;

                var priceTextMesh = Instantiate(textMeshPrefab, shopSlots.transform);
                priceTextMesh.transform.localPosition = currentPricePos;
                priceTextMesh.GetComponent<TextMesh>().text = sh.cost + "";

                sh.priceTextMesh = priceTextMesh;
                sh.labelTextMesh = textMesh;

                currentPos.y -= Y_SPACING;
                currentPricePos.y -= Y_SPACING;
            }

            currentPos.x += SLOT_X_SPACING;
            currentPricePos.x += SLOT_X_SPACING;
        }

        updateAllShopItems();
    }
	
    void spaceBarPressed() {
        if (spaceBar.isAvailable()) {
            spaceBar.doSink();
            spaceCount += getCurrentSpacePower();

            if (hasStochasticBoost()) {
                spaceCount += getStochasticBoost();
            }

            updateSpaceCountLabel();
            updateAllShopItems();
        }
    }

    bool hasStochasticBoost() {
        var shi = shopItemDatas[ShopItem.STOCHASTIC_BOOST];
        return shi.isUnlocked;
    }

    int getStochasticBoost() {
        var shi = shopItemDatas[ShopItem.STOCHASTIC_BOOST];
        if (Random.value < (shi.level / 100)) {
            return Random.Range(1000, 5000);
        }
        return 0;
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
        var maxSinks = new int [] {200, 250, 300, 400, 600};
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
                //shopReticuleYTarget += Y_SPACING;
                //shopItemIndex -= 1;

                moveReticule(-1);
            } else if (Input.GetKeyDown("down")) {
                //shopReticuleYTarget -= Y_SPACING;
                //shopItemIndex += 1;

                moveReticule(1);
            } else if (Input.GetKeyDown("right")) {
                moveSlots(1);
            } else if (Input.GetKeyDown("left")) {
                moveSlots(-1);
            } else if (Input.GetKeyDown("return")) {
                var sh = getCurrentShopItemInfo();
                if (sh.cost <= spaceCount && sh.level < sh.maxLevel) {
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

            shopItemIndex = Mathf.Clamp(shopItemIndex, 0, 3);
            
            if (Input.GetKeyDown("n")) {
                spaceCount += 123456789;
                updateSpaceCountLabel();
            }
        }	

        updateShopReticule();

        updateAutoSpace();

        updateSlotsAnimation();
	}

    void moveReticule(int dir) {
        shopReticuleYTarget -= Y_SPACING * dir;
        shopItemIndex += dir;
        
        clampShopReticule();
    }

    void setReticuleRowTarget(int row) {
        shopItemIndex = row;
        shopReticuleYTarget = row * Y_SPACING;
    }

    void moveSlots(int dir) {
        if (dir == -1) {
            if (shopItemSlotIndex <= 0) {
                return;
            }
        } else if (dir == 1) {
            if (shopItemSlotIndex >= (Mathf.CeilToInt(shopItemIndices.Count / 4f) - 1)) {
                return;
            }
        } else {
            Debug.Log("Unsupported value of dir passed: " + dir);
        }

        shopItemSlotIndex += dir;
        shopSlotXTarget -= dir * SLOT_X_SPACING;

        clampShopReticule();
    }

    void clampShopReticule() {
        if (shopItemSlotIndex * 4 + shopItemIndex >= shopItemIndices.Count) {
            setReticuleRowTarget(shopItemIndices.Count - shopItemSlotIndex * 4 - 1);
        }
    }

    void updateSlotsAnimation() {
        Vector3 pos = shopSlots.transform.localPosition;
        pos.x = pos.x + (shopSlotXTarget - pos.x) * shopSlotSpeedPercentage * Time.deltaTime;
        shopSlots.transform.localPosition = pos;
    }

    void updateAllShopItems() {
        for (int i = 0; i < shopItemIndices.Count; ++i) {
            var sh = shopItemDatas[shopItemIndices[i]];
            updateShopItem(sh);
        }
    }

    void updateShopItem(ShopItemInfo shi) {
        var labelTM = shi.labelTextMesh.GetComponent<TextMesh>();
        var priceTM = shi.priceTextMesh.GetComponent<TextMesh>();

        if (shi.isUnlocked) {
            labelTM.text = shi.txt;
        } else {
            labelTM.text = "???";
        }

        priceTM.text = shi.cost + "";

        if (spaceCount < shi.cost || shi.level >= shi.maxLevel) {
            labelTM.color = new Color(0.3f, 0.3f, 0.3f);
            priceTM.color = new Color(0.3f, 0.3f, 0.3f);
        } else {
            labelTM.color = new Color(1, 1, 1);
            priceTM.color = new Color(1, 1, 1);
        }
    }

    void buyShopItem(ShopItemInfo shi) {
        shi.level++;
        spaceCount -= shi.cost;
        shi.cost = (int) (shi.cost * shi.growthFactor);
        shi.isUnlocked = true;

        updateSpaceCountLabel();
        updateAllShopItems();
    }

    ShopItem getCurrentShopItem() {
        return shopItemIndices[shopItemSlotIndex * 4 + shopItemIndex];
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
        if (shi.level > 0) {
            var spacesPerSecArr = new int [] {1, 3, 8, 20};
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
