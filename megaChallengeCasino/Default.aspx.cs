using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace megaChallengeCasino
{
    public partial class Default : System.Web.UI.Page
    {
        Random random = new Random();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string[] slots = new string[] { spinSlot(), spinSlot(), spinSlot() };
                displaySlot(slots);
                ViewState.Add("PlayersMoney", 100);
                displayNewBalance();
            }
        }

        protected void leverButton_Click(object sender, EventArgs e)
        {
            int bet = pullUserBet();
            int winnings = pullLever(bet);
            calculateNewBalance(bet, winnings);
        }

        public int pullLever(int bet)
        {
            string[] slots = new string[] { spinSlot(), spinSlot(), spinSlot() };
            displaySlot(slots);
            int slotValue = determineSlotValues(slots);
            int winnings = calculateWinnings(bet, slotValue);
            return winnings;
        }

        private string spinSlot()
        {
            string[] images = new string[] { "Strawberry", "Bar", "Lemon", "Bell", "Clover", "Cherry", "Diamond", "Orange", "Seven", "HorseShoe", "Plum", "Watermelon" };
            return images[random.Next(11)];
        }
        
        private void displaySlot(string[] slots)
        {
            slotOneImage.ImageUrl = "/Images/" + slots[0] + ".png";
            slotTwoImage.ImageUrl = "/Images/" + slots[1] + ".png";
            slotThreeImage.ImageUrl = "/Images/" + slots[2] + ".png";
        }

        private int determineSlotValues(string[] slots)
        {
            int cherryValue = determineCherry(slots);
            int barValue = determineBarValue(slots);
            int jackpot = determineJackpot(slots);

            int slotValue = 0;

            if (jackpot == 3)
            {
                slotValue = 100;
            }
            else if (barValue < 1)
            {
                slotValue = slotValue * 0;
            }
            else if (cherryValue > 1)
            {
                slotValue = cherryValue;
            }

            return slotValue;
        }

        private int determineCherry(string[] slots)
        {
            int cherryValue = 1;
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] == "Cherry")
                {
                   cherryValue += 1;
                }
            }
            return cherryValue;
        }

        private int determineBarValue(string[] slots)
        {
            int barValue = 1;
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] == "Bar")
                {
                    barValue = barValue * 0;
                }
            }
            return barValue;
        }

        private int determineJackpot(string[] slots)
        {
            int jackpot = 0;
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] == "Seven")
                {
                    jackpot += 1;
                }
            }
            return jackpot;
        }   

        private int pullUserBet()
        {
            int.TryParse(betTextBox.Text, out int bet);
            return bet;
        }

        private int calculateWinnings(int bet, int slotValue)
        {
            int winnings = bet * slotValue;
            displayResultsMessage(bet, winnings);
            return winnings;
        }

        private void displayResultsMessage(int bet, int winnings)
        {
            if (winnings > 0)
            {
                resultLabel.Text = String.Format("You bet ${0:N2} and won ${1:N2}!", bet, winnings);
            }
            else
            {
                resultLabel.Text = String.Format("Sorry, you lost ${0:N2}. Better luck next time.", bet);
            }
            
        }

        private void calculateNewBalance(int bet, int winnings)
        {
            int balance = int.Parse(ViewState["PlayersMoney"].ToString()) - bet + winnings;
            ViewState["PlayersMoney"] = balance;
            displayNewBalance();
        }

        private void displayNewBalance()
        {
            moneyLabel.Text = String.Format("Player's Money: {0:C}", ViewState["PlayersMoney"]);
        }    
    }
}