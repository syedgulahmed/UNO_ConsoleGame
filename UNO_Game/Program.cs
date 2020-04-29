using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Reflection.PortableExecutable;

namespace UNO_Game
{
    class Program
    {
        static int totalPlayers = -1;
        private static bool skipCardPlayed;
        private static Player currentPlayer;
        private static FileHandler fileHandler;

        //Instance of PlayerList that will contains all the players
        //Info like direction of play, which player's turn it is
        static PlayerList playerList;

        //Card Decks
        private static Card[] orderedDeck;
        private static Card[] shuffledDeck;
        private static Stack drawPile;
        private static Stack discardPile;

        static void Main()
        {
            //Generating UNO Cards
            collectCards();

            int selectedOption;
            fileHandler = new FileHandler(orderedDeck);
            
            Console.WriteLine("1. To Start New Game");
            Console.WriteLine("2. To load saved Game");

            do
            {
                Console.Write("Enter your option: ");
                selectedOption = Int32.Parse(Console.ReadLine());
            } while (selectedOption < 1 || selectedOption > 2);


            //Getting Player's Information
            if (selectedOption == 1)
            {
                initializePlayers();
            }
            else
            {
                playerList = fileHandler.getPlayerList();
                drawPile = fileHandler.getDraw();
                discardPile = fileHandler.getDiscard();
                skipCardPlayed = fileHandler.getCardSkiped();
            }


            //Playing Game
            do
            {

                if(selectedOption == 1)
                {
                    //Shuffling Cards
                    shuffleCards();

                    //Distributing cards among players and puting in cards in draw pile and discard pile
                    dealCards();
                }
                else
                {
                    selectedOption = 1;
                }

                //play a round
                playRound();

            } while (!playerList.gameEnds());
            
    
        }

        
        //This method will play a single round of each game
        private static void playRound()
        {
            Card topDiscard;
            skipCardPlayed = false;

            while (!playerList.roundEnded()) {

                Console.Clear();

                Console.Write("Card on discard Deck: ");
                topDiscard = discardPile.top();
                topDiscard.showCard();

                currentPlayer = playerList.getNextPlayer();


                //Will perform the required action according to Top card of Discard Pile
                performRequiredAction(topDiscard);
                
                if (skipCardPlayed == false)
                {
                    //Displaying cards player's have
                    currentPlayer.displayCards();
                    //Allowing user to select card
                    selectCard(topDiscard);
                }
                else
                {
                    skipCardPlayed = false;
                }

                fileHandler.SaveGame(drawPile, discardPile, playerList, skipCardPlayed);
            }

        }

        //This method will allow user to select the card
        private static void selectCard(Card discardTop)
        {

            bool inputAccepted = false;
            int userInput;
            Console.Write("\nEnter your card number or '0' to pass the turn: ");
            
            do{
                userInput = Int32.Parse(Console.ReadLine());

                if (userInput == 0)
                {
                    Card drawanCard = drawPile.pop();

                    if (!isCardAcceptable(drawanCard, discardPile.top()))
                    {
                        currentPlayer.addCard(drawanCard);
                    }

                    inputAccepted = true;
                }
                else if (userInput < 1 || userInput > currentPlayer.totalCards())
                {
                    Console.Write("Invalid input, Re-Enter Card number: ");
                }
                else if (!isCardAcceptable(currentPlayer.getCardInfo(userInput - 1), discardTop))
                {
                    Console.Write("This card can't be played: ");
                }
                else if (isCardAcceptable(currentPlayer.getCardInfo(userInput - 1), discardTop)){
                    inputAccepted = true;
                    currentPlayer.removeCard(userInput - 1);
                }
                } while (!inputAccepted);

        }

        //This method will see if selected card is acceptable
        private static bool isCardAcceptable(Card card, Card discardTop)
        {

            if(card.getCardType() == "Wild Card")
            {
                int userInput;
                Console.WriteLine("1: Red, 2:Yellow, 3:Green, 4:Blue,");
                Console.Write("Enter color number to select for wild card: ");
                String[] colors = {"Red", "Yellow", "Green", "Blue"};

                do
                {
                    userInput = Int32.Parse(Console.ReadLine());

                    if(userInput < 1 || userInput > 4)
                    {
                        Console.Write("Invalid input, Retry: ");
                    }

                } while (userInput < 1 || userInput > 4);

                card.color = colors[userInput - 1];
                discardPile.push(card);

                if(card.getCardIdentity() == "Take 4")
                {
                    skipCardPlayed = true;
                }

                return true;

            }else{

                if (card.getColor() == discardTop.getColor())
                {

                    if(card.getCardIdentity() == "Skip")
                    {
                        skipCardPlayed = true;
                    }

                    discardPile.push(card);
                    return true;
                } else if (card.getCardIdentity() == discardTop.getCardIdentity()) {

                    if (card.getCardIdentity() == "Skip")
                    {
                        skipCardPlayed = true;
                    }

                    discardPile.push(card);
                    return true;
                }

            }

            return false;

        } 

        //This method will perform the required action according to card present on discard top
        private static void performRequiredAction(Card discardTop)
        {

            if(discardTop.getCardType() == "Action Card"){
                if(discardTop.getCardIdentity() == "Skip"){
                    if(skipCardPlayed == true)
                    {
                        Console.WriteLine(currentPlayer.getName() + " your turn skipped.");
                        Console.WriteLine("Press Enter to continue");
                        Console.ReadLine();
                    }
                }
                else if(discardTop.getCardIdentity() == "Draw Two"){
                    Console.WriteLine(currentPlayer.getName() + " 2 new cards added.");
                    currentPlayer.addCard(drawPile.pop());
                    currentPlayer.addCard(drawPile.pop());
                }
                else if (discardTop.getCardIdentity() == "Reverse"){
                    Console.WriteLine(currentPlayer.getName() + " Direction Changed.");
                    playerList.changeDirection();
                    currentPlayer = playerList.getNextPlayer();
                    currentPlayer = playerList.getNextPlayer();
                }
            }else if(discardTop.getCardType() == "Wild Card"){
                if(discardTop.getCardIdentity() == "Take 4"){
                    if (skipCardPlayed == true)
                    {
                        Console.WriteLine(currentPlayer.getName() + " 4 new cards added.");
                        Console.WriteLine(currentPlayer.getName() + " your turn skipped.");
                        Console.WriteLine("Press Enter to continue");
                        Console.ReadLine();
                    }
                }
            }

        }

        //This method will distribute the cards among users
        private static void dealCards() {

            Player[] players = playerList.getPlayersList();
            int cardCount = 0;

            drawPile = new Stack();
            discardPile = new Stack();

            foreach(Player player in players)
            {
                for(int i = 0; i < 7; i++)
                {
                    player.addCard(shuffledDeck[cardCount++]);
                }
            }

            while(cardCount < 108)
            {
                drawPile.push(shuffledDeck[cardCount++]);
            }

            discardPile.push(drawPile.pop());
        
        }

        //this method will shuffle the cards
        private static void shuffleCards() {

            shuffledDeck = new Card[108];
            ArrayList auxiliaryArray = new ArrayList();

            for(int i = 0; i < 108; i++)
            {
                auxiliaryArray.Add(orderedDeck[i]);
            }

            for(int i = 0; i < 108; i++)
            {
                int generatedIndex = RandomNumber(0, 108 - i);
                shuffledDeck[i] = (Card) auxiliaryArray[generatedIndex];
                auxiliaryArray.RemoveAt(generatedIndex);

            }

            MakeSureFirstCardMustBeNumberCard();

        }

        //This method will help in shuffling the card
        public static int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        //This method will ask about players
        private static void initializePlayers()
        {
            do
            {
                if (totalPlayers != -1)
                {
                    Console.WriteLine("You have entered invalid number. ");
                    Console.WriteLine("Number of players must be between 2 and 4.");
                }

                Console.WriteLine("Please enter number of players: ");
                totalPlayers = Int32.Parse(Console.ReadLine());

            } while (totalPlayers < 2 || totalPlayers > 4);

            playerList = new PlayerList(totalPlayers);

            for(int i = 0; i < totalPlayers; i++)
            {
                playerList.addPlayer();
            }

        }

        //This method will create cards
        private static void collectCards()
        {
            orderedDeck = new Card[108];
            int cardCount = 0;
            String[] colors = { "Red", "Yellow", "Grean", "Blue" };
            String[] actionCards = { "Skip", "Draw Two", "Reverse" };
            String[] wildCard = { "Wild Card", "Take 4" };

            foreach(String color in colors)
            {
                for(int i = 0; i < 10; i++)
                {
                    if(i == 0)
                    {
                        Card newCard = new NumberCard(cardCount, color, i);
                        orderedDeck[cardCount++] = newCard;
                    }
                    else{

                        Card newCard = new NumberCard(cardCount, color, i);
                        orderedDeck[cardCount++] = newCard;

                        newCard = new NumberCard(cardCount, color, i);
                        orderedDeck[cardCount++] = newCard;

                    }
                }

                for(int i = 0; i < 3; i++)
                {
                    Card newCard = new ActionCard(cardCount, color, actionCards[i]);
                    orderedDeck[cardCount++] = newCard;

                    newCard = new ActionCard(cardCount, color, actionCards[i]);
                    orderedDeck[cardCount++] = newCard;
                }
            }

            for(int i = 0; i < 4; i++)
            {

                Card newCard = new WildCard(cardCount, wildCard[0]);
                orderedDeck[cardCount++] = newCard;

                newCard = new WildCard(cardCount, wildCard[1]);
                orderedDeck[cardCount++] = newCard;

            }

        }

        //This method will make sure Action card and wild card don't appear firt
        private static void MakeSureFirstCardMustBeNumberCard()
        {
            Card temp;
            int current = 0;

            if(shuffledDeck[107].getCardType() == "Action Card" || 
                shuffledDeck[107].getCardType() == "Wild Card")
            {

                for(int i = 0; i < 108; i++)
                {
                    if(shuffledDeck[i].getCardType() == "Number Card")
                    {
                        current = i;
                        break;
                    }
                }

                temp = shuffledDeck[107];
                shuffledDeck[107] = shuffledDeck[current];
                shuffledDeck[current] = temp;
            }


        }

    }
}