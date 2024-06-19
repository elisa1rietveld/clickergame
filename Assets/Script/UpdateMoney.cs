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
    public ParticleSystem accessoryParticleSystem; // Add a reference to the particle system

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

        // Ensure the particle system is stopped at the start
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

    public void AddMoney()
    {
        money += clickValue; // Add current click value to money
        moneyText.text = "Money: " + money;
    }

    public void Buy(int num)
    {
        if (money >= currentPrices[num])
        {
            money -= currentPrices[num];
            if (accessories != null)
            {
                GameObject choice = accessories.transform.GetChild(num).gameObject;
                GameObject upgrade = Instantiate(upgrades[num], new Vector2(0f,0f), quaternion.identity);
                float Offset = choice.transform.childCount * 10f;
                upgrade.transform.SetParent(choice.transform, true);
                upgrade.transform.SetAsFirstSibling();
                upgrade.transform.localPosition = new Vector2(0f, Offset);
                upgrade.transform.localScale = new Vector3(1f, 1f, 1f);

                // Play the particle system
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

            // Adjust click value based on the toy bought
            if (num == 0) // First toy
            {
                clickValue += 1;
                currentPrices[0] *= 2; // Double the price of the first toy
                if (priceText1 != null)
                {
                    priceText1.text = "Toy cost: " + currentPrices[0];
                }
            }
            else if (num == 1) // Second toy
            {
                clickValue *= 2;
                currentPrices[1] *= 3; // Triple the price of the second toy
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
}
