using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Mathematics;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class UpdateMoney : MonoBehaviour
{
    public TMP_Text moneyText;
    private int money;
    public GameObject accessories;
    private int[] basePrices;
    private int[] currentPrices;
    private bool[] wearing;
    public Button[] buttons;
    public GameObject[] upgrades;
    private int clickValue;
    public TMP_Text priceText1;
    public TMP_Text priceText2;
    public ParticleSystem accessoryParticleSystem; 
    // Start is called before the first frame update
    void Start()
    {
        basePrices = new int[2];
        basePrices[0] = 25;
        basePrices[1] = 50;

        currentPrices = new int[2];
        currentPrices[0] = basePrices[0];
        currentPrices[1] = basePrices[1];

        wearing = new bool[2];
        clickValue = 1; // Initial click value is 1

        UpdateButtonInteractivity();

        priceText1.text = "Toy cost: " + basePrices[0];
        priceText2.text = "Toy cost: " + basePrices[1];

        //ensure the particle system is stopped at the start
        if (accessoryParticleSystem != null)
        {
            accessoryParticleSystem.Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateButtonInteractivity();
        UpdatePriceText();
    }


    //if you have enough money, you can click on the button
    private void UpdateButtonInteractivity()
    {
        for (int i = 0; i < currentPrices.Length; i++)
        {
            if (money >= currentPrices[i])
            {
                buttons[i].interactable = true;
            }
            else
            {
                buttons[i].interactable = false;
            }
        }
    }

    //add money by clicking meow
    public void AddMoney()
    {
        money += clickValue; //add current click value to money
        moneyText.text = "Money: " + money;
    }


    //cost gets subtracted
    // we add an toy based on the number of the object in the array
    //we add the toy via AddUPgrade
    //if statement that add the right upgrade
    public void Buy(int num)
    {
        if (money >= currentPrices[num])
        {
            money -= currentPrices[num];
            GameObject choice = accessories.transform.GetChild(num).gameObject;
            if (accessories != null)
            {
                AddUpgrade(choice, num);

                //play the particle system
                if (accessoryParticleSystem != null)
                {
                    PlayParticleSystem();
                }
                else
                {
                    Debug.LogError("Accessory Particle System is not assigned");
                }
            }
            else
            {
                Debug.LogError("Accessories is null");
            }

            //adjust click value based on the toy bought
            if (num == 0) // First toy
            {
                clickValue += 1;
                currentPrices[0] *= 2; //double the price of the first toy
                if (priceText1 != null)
                {
                    priceText1.text = "Toy cost: " + currentPrices[0];
                }
            }
            else if (num == 1) //second toy
            {
                //first upgrade starts factory
                if (choice.transform.childCount == 1)
                {
                    StartCoroutine(ClickFactory());
                }
                currentPrices[1] *= 3; //triple the price of the second toy
                if (priceText2 != null)
                {
                    priceText2.text = "Toy cost: " + currentPrices[1];
                }
            }

            moneyText.text = "Money: " + money;
            UpdateButtonInteractivity();
        }
    }

    private void UpdatePriceText()
    {
        priceText1.text = "Toy cost: " + currentPrices[0];
        priceText2.text = "Toy cost: " + currentPrices[1];
    }

    private void PlayParticleSystem()
    {
        accessoryParticleSystem.Play();
    }

    void AddUpgrade(GameObject choice, int num)
    {
        //spawns prefab and sets it slightly above it's sibling
        GameObject upgrade = Instantiate(upgrades[num], new Vector2(0f, 0f), quaternion.identity);
        float Offset = choice.transform.childCount * 10f;
        upgrade.transform.SetParent(choice.transform, true);
        upgrade.transform.SetAsFirstSibling();
        upgrade.transform.localPosition = new Vector2(0f, Offset);
        upgrade.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    //once started, keeps looping and adding score after one second.
    //amount is based on amount of siblings of the sprite.
    private IEnumerator ClickFactory()
    {
        while (true)
        {
            int count = accessories.transform.GetChild(1).childCount;
            yield return new WaitForSeconds(1);
            money += count;
            moneyText.text = "Money: " + money;
        }
    }
}