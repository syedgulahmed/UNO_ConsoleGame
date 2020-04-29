using System;
using System.Collections.Generic;
using System.Text;

namespace UNO_Game
{
    class Stack
    {

        Card[] cards;
        int size;
        
        public Stack()
        {
            cards = new Card[108];
            size = 0;
        }

        public Card[] getDeck()
        {
            return cards;
        }

        public void setDeck(Card[] cards)
        {
            this.cards = cards;
        }

        public void push(Card card)
        {
            cards[size++] = card;
        }

        public Card pop()
        {
            return cards[--size];
        }

        public Card top()
        {
            return cards[size - 1];
        }

        public bool isEmpty()
        {
            if (size == 0)
                return true;

            return false;
        }

        public bool isFull()
        {
            if (size == 108)
                return true;

            return false;
        }

        public int getSize()
        {
            return size;
        }

    }
}