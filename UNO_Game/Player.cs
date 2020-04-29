using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace UNO_Game
{

    //This class will contain all the information of player
    //like having cards, points, name etc
    class Player
    {
        private String playerName;
        private ArrayList cardsList;
        private int points;

        public Player(String playerName)
        {
            this.playerName = playerName;
            cardsList = new ArrayList();
            points = 0;
        }

        public String getName()
        {
            return playerName;
        }

        public void addCard(Card card)
        {

            this.cardsList.Add(card);

        }

        public Card getCard(int id)
        {
            if(id >= 0 && id < this.cardsList.Count)
            {
                Card card = (Card)this.cardsList[id];
                this.cardsList.RemoveAt(id);
                return card;
            }
            else
            {
                return null;
            }
        }

        public Card getCardInfo(int id)
        {

            return (Card)this.cardsList[id];

        }

        public void removeCard(int id)
        {
            this.cardsList.RemoveAt(id);
        }

        public int totalCards()
        {
            return this.cardsList.Count;
        }

        public ArrayList getCards()
        {
            return cardsList;
        }

        public void addPoints(int points)
        {
            this.points = this.points + points;
        }

        public int getPoinsts()
        {
            return this.points;
        }

        public void displayCards()
        {
            int cardCount = 1;
            Console.WriteLine("\n" + this.playerName + ", cards you have got: ");
            foreach (Card card in this.cardsList)
            {
                Console.Write(cardCount++ + "#: ");
                card.showCard();
            }
            Console.Write(0 + "#: To Pass the turn/Get Card from Draw Pile");
        }

    }
}