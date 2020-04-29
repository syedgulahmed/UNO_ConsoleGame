using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace UNO_Game
{
    //This Class will handle the filing
    class FileHandler
    {

        Stack drawPile;
        Stack discardPile;
        PlayerList playerList;
        Player[] players;
        Card[] orderedCards;

        bool cardSkiped;

        public bool getCardSkiped()
        {
            return cardSkiped;
        }

        public FileHandler(Card[] orderedCard)
        {
            this.orderedCards = orderedCard;
        }

        public void SaveGame(Stack drawPile, Stack discardPile, PlayerList playersList, bool skipCardPlayed)
        {
            this.drawPile = drawPile;
            this.discardPile = discardPile;
            this.playerList = playersList;

            storeBeteInfo(skipCardPlayed);

            players = playerList.getPlayersList();

            for(int i = 0; i < playerList.getTotalPlayers(); i++)
            {
                storePlayerInfo(i);
            }

            storeDiscardPile();

            storeDrawPile();
        }

        private void storeBeteInfo(bool skipCardPlayed)
        {

            FileInfo file = new FileInfo("Beta.text");
            StreamWriter writer = file.CreateText();

            writer.WriteLine(playerList.getTotalPlayers());
            writer.WriteLine(playerList.getCurrentPlayer());
            writer.WriteLine(playerList.getIsDirectionClockWise());
            writer.WriteLine(skipCardPlayed.ToString());

            writer.Close();

        }

        private void storeDiscardPile()
        {
            FileInfo file = new FileInfo("discard.text");
            StreamWriter writer = file.CreateText();

            Card[] dack = discardPile.getDeck();
            for (int i = 0; i < discardPile.getSize(); i++)
            {
                writer.WriteLine(dack[i].getID());
            }

            writer.Close();
        }

        private void storeDrawPile()
        {
            FileInfo file = new FileInfo("draw.text");
            StreamWriter writer = file.CreateText();

            Card[] dack = drawPile.getDeck();
            for (int i = 0; i < drawPile.getSize(); i++)
            {
                writer.WriteLine(dack[i].getID());
            }

            writer.Close();
        }

        private void storePlayerInfo(int num)
        {

            FileInfo file = new FileInfo(num + ".text");
            StreamWriter writer = file.CreateText();

            writer.WriteLine(players[num].getName());
            writer.WriteLine(players[num].getPoinsts());
            ArrayList cards = players[num].getCards();
            writer.WriteLine(cards.Count);

            for (int j = 0; j < cards.Count; j++)
            {
                Card card = (Card)cards[j];
                writer.WriteLine(card.getID());
            }

            writer.Close();

        }

        //Reading Data
        ArrayList beteInfo;
        ArrayList DiscardPile;
        ArrayList DrawPile;
        ArrayList[] playersInfo;
        
        private void readData()
        {

            FileInfo file = new FileInfo("Beta.text");
            StreamReader reader = file.OpenText();

            beteInfo = new ArrayList();

            beteInfo.Add(reader.ReadLine()); //Number of players
            beteInfo.Add(reader.ReadLine()); //Current Player
            beteInfo.Add(reader.ReadLine()); //Direction
            beteInfo.Add(reader.ReadLine()); //Is skip card used

            reader.Close();

            //Reading Discard Deck
            file = new FileInfo("discard.text");
            reader = file.OpenText();

            DiscardPile = new ArrayList();
            String line;
            while((line = reader.ReadLine()) != null)
            {
                DiscardPile.Add(line);
            }
            reader.Close();

            //Reading Draw Deck
            file = new FileInfo("draw.text");
            reader = file.OpenText();

            DrawPile = new ArrayList();
            while ((line = reader.ReadLine()) != null)
            {
                DrawPile.Add(line);
            }
            reader.Close();

            //Reading Players Info
            int totalPlayers = Int32.Parse((String)beteInfo[0]);
            playersInfo = new ArrayList[totalPlayers];
            for (int i = 0; i < totalPlayers; i++)
            {
                file = new FileInfo(i + ".text");
                reader = file.OpenText();

                playersInfo[i] = new ArrayList();
                while ((line = reader.ReadLine()) != null)
                {
                    playersInfo[i].Add(line);
                }
                reader.Close();
            }
        }

        public Stack getDiscard()
        {

            Card[] card = new Card[108];

            for(int i = 0; i < DiscardPile.Count; i++)
            {

                card[i] = orderedCards[Int32.Parse((String)DiscardPile[i])];

            }

            Stack stack = new Stack(DiscardPile.Count);
            stack.setDeck(card);

            return stack;

        }

        public Stack getDraw()
        {

            Card[] card = new Card[108];

            for (int i = 0; i < DrawPile.Count; i++)
            {

                card[i] = orderedCards[Int32.Parse((String)DrawPile[i])];

            }

            Stack stack = new Stack(DrawPile.Count);
            stack.setDeck(card);

            return stack;

        }

        public PlayerList getPlayerList()
        {

            readData();

            PlayerList thePlayerList = new PlayerList(playersInfo.Length);

            int currentPlayer = Int32.Parse((String)beteInfo[1]);
            bool clockWise;
            if ((String)beteInfo[2] == "False")
            {
                clockWise = false;
            }
            else
            {
                clockWise = true;
            }

            if ((String)beteInfo[3] == "False")
            {
                cardSkiped = false;
            }
            else
            {
                cardSkiped = true;
            }



            Player[] players = new Player[playersInfo.Length];

            for(int i = 0; i < playersInfo.Length; i++)
            {
                ArrayList player = playersInfo[i];
                String name = (String)player[0];
                players[i] = new Player(name);


                int points = Int32.Parse((String)player[1]);
                players[i].addPoints(points);

                int totalCards = Int32.Parse((String)player[2]);
                for (int j = 0; j < totalCards; j++)
                {
                    int cardCountNumber = Int32.Parse((String)player[j+3]);

                    Console.WriteLine(players[i].getName());

                    Card card = orderedCards[cardCountNumber];
                    
                    players[i].addCard(card);
                }

            }

            thePlayerList.getSavedData(players, playersInfo.Length, currentPlayer, clockWise);

            return thePlayerList;
        }

        public Object LoadGame()
        {
            var data = new
            {
                drawPile = this.drawPile,
                discardPile = this.discardPile,
                players = this.playerList,
            };

            return data;
        }

    }
}