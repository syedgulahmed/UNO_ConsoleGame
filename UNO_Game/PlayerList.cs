using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace UNO_Game
{
    //This class will contain all the players and decide
    //who to play turn, direction and scoring
    class PlayerList
    {
        
        Player[] players;
        int totalPlayers;
        int currentSize;
        int currentPlayer;
        bool clockWise = true;

        public void getSavedData(Player[] players, int total, int currentPlayer, bool clockWise)
        {
            this.players = players;
            this.totalPlayers = total;
            this.currentSize = total;
            this.currentPlayer = currentPlayer;
            this.clockWise = clockWise;
        }

        public Player[] getPlayersList()
        {
            return players;
        }

        public bool roundEnded()
        {

            for(int i = 0; i < players.Length; i++)
            {
                if(players[i].totalCards() == 0)
                {
                    players[i].addPoints(this.calculatePoints());
                    return true;
                }
            }
            return false;
        }

        public bool gameEnds()
        {

            foreach(Player player in players)
            {
                if(player.getPoinsts() >= 500)
                {
                    Console.Clear();
                    Console.WriteLine(player.getName() + " won the game!");
                    return true;
                }
            }

            return false;

        }

        private int calculatePoints()
        {
            int points = 0;
            ArrayList cardArray;
            for (int i = 0; i < players.Length; i++)
            {
                cardArray = players[i].getCards();
                
                foreach(Card card in cardArray)
                {
                    points += card.getPoints();
                }

            }

            return points;
        }

        public int getCurrentPlayer()
        {
            return this.currentPlayer;
        }

        public String getIsDirectionClockWise()
        {
            return this.clockWise.ToString();
        }

        public int getTotalPlayers()
        {
            return this.totalPlayers;
        }

        public PlayerList(int totalPlayers) {

            this.totalPlayers = totalPlayers;
            this.players = new Player[totalPlayers];
            this.currentPlayer = -1;
            this.currentSize = 0;

        }

        public void addPlayer()
        {
            Console.WriteLine("Enter name of player #" + (currentSize + 1) + ": ");
            String name = Console.ReadLine();
            Player newPlayer = new Player(name);
            players[currentSize++] = newPlayer;
        }

        public Player getNextPlayer() {

            if (clockWise)
            {
                if(currentPlayer == (this.totalPlayers - 1))
                {
                    currentPlayer = -1;
                }
                
                return players[++currentPlayer];

            }
            else
            {

                if(currentPlayer <= 0)
                {
                    currentPlayer = totalPlayers;
                }

                return players[--currentPlayer];
            }
            
        }

        public void changeDirection()
        {

            clockWise = !clockWise;

        }

        public bool isEnd()
        {

            if(currentPlayer == (totalPlayers - 1))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

    }
}