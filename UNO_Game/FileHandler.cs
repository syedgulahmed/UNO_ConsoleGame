using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace UNO_Game
{
    class FileHandler
    {

        Stack drawPile;
        Stack discardPile;
        PlayerList playerList;
        Player[] players;

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

        private void loadBetaInfo()
        {

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

        private void loadDiscardPile()
        {
            
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