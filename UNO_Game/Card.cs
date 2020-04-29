using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.Text;

namespace UNO_Game
{
    abstract class Card
    {
        protected int cardID;
        public String color;
        protected int points;

        public Card(int cardID, String color)
        {
            this.cardID = cardID;
            this.color = color;
        }

        public int getID()
        {
            return cardID;
        }

        public String getColor()
        {
            return this.color;
        }

        public int getPoints()
        {
            return this.points;
        }

        public abstract String getCardIdentity();

        public abstract void showCard();

        public abstract String getCardType();



    }

    class NumberCard : Card
    {
        int number;

        public NumberCard(int cardID, String color, int number):base(cardID, color)
        {
            base.points = number;
            this.number = number;

        }

        public override String getCardType()
        {
            return "Numbered Card";
        }

        public override String getCardIdentity()
        {
            return number.ToString();
        }

        public override void showCard()
        {
            Console.Write(this.color);
            Console.Write(" " + this.number);
            Console.WriteLine("");
        }
    }

    class ActionCard : Card
    {
        String actionType;

        public ActionCard(int cardID, String color, String actionType) : base(cardID, color)
        {
            base.points = 20;
            this.actionType = actionType;
        }

        public override String getCardType()
        {
            return "Action Card";
        }

        public override String getCardIdentity()
        {
            return actionType;
        }

        public override void showCard()
        {
            Console.Write(this.color);
            Console.Write(" " + this.actionType);
            Console.WriteLine("");
        }

    }

    class WildCard : Card
    {
        String wildType;

        public WildCard(int cardID, String wildType) : base(cardID, "UnColored")
        {
            base.points = 50;
            this.wildType = wildType;
        }

        public override String getCardType()
        {
            return "Wild Card";
        }

        public override String getCardIdentity()
        {
            return wildType;
        }

        public override void showCard()
        {
            if(this.color == "UnColored")
            {
                Console.WriteLine(this.wildType);
            }
            else
            {
                Console.WriteLine(this.color + " " + this.wildType);
            }
                
        }

    }

}