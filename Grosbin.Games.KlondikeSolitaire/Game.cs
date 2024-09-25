/* Game.cs
 * Author: Grosbin Orellana Luna
 */
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Grosbin.Games.KlondikeSolitaire
{
    /// <summary>
    /// The game controller.
    /// </summary>
    public class Game
    {
        /// <summary>
        /// The number of cards initially in the stock.
        /// </summary>
        private const int _initialStockCount = 24;

        /// <summary>
        /// The number of cards initially face-down on the tableau.
        /// </summary>
        private const int _initialHiddenCardCount = 21;

        /// <summary>
        /// The number of ranks
        /// </summary>
        private static readonly int _ranks = Card.MaxRank - Card.MinRank + 1;

        /// <summary>
        /// The number of suits.
        /// </summary>
        private static readonly int _suits = Card.MaxSuit - Card.MinSuit + 1;

        /// <summary>
        /// The number of cards in a deck.
        /// </summary>
        private static readonly int _cardsInDeck = _ranks * _suits;

        /// <summary>
        /// The random number generator.
        /// </summary>
        private readonly Random _randomNumbers;

        /// <summary>
        /// The selected discard pile 
        /// </summary>
        private DiscardPile? _discardPileSelected;

        /// <summary>
        /// The selected tableau column
        /// </summary>
        private TableauColumn? _tableauColumnSelected;

        /// <summary>
        /// Number of face down tableau cards
        /// </summary>
        private int _numFaceDownTC = _initialHiddenCardCount;

        /// <summary>
        /// Number of cards in stock and discard piles together
        /// </summary>
        private int _numStockAndDiscard = _initialStockCount;
        
        /// <summary>
        /// Gets a new card deck.
        /// </summary>
        /// <returns>The new card deck.</returns>
        private static Card[] GetNewDeck()
        {
            Card[] cards = new Card[_cardsInDeck];
            for (int i = 0; i < cards.Length; i++)
            {
                cards[i] = new Card(i % _ranks + 1, (Suit)(i / _ranks));
            }
            return cards;
        }

        /// <summary>
        /// Shuffles a new deck and pushes the cards onto the given stack.
        /// </summary>
        /// <param name="shuffled">The stack on which to push the cards.</param>
        private void ShuffleNewDeck(Stack<Card> shuffled)
        {
            Card[] deck = GetNewDeck();
            for (int i = deck.Length - 1; i >= 0; i--)
            {
                // Get a random nonnegative integer less than or equal to i.
                int j = _randomNumbers.Next(i + 1);

                shuffled.Push(deck[j]);
                deck[j] = deck[i];
            }
        }

        /// <summary>
        /// Constructs a new game from the given controls and seed.
        /// </summary>
        /// <param name="stock">The stock.</param>
        /// <param name="tableau">The tableau columns.</param>
        /// <param name="seed">The random number seed. If -1, no seed is used.</param>
        public Game(CardPile stock, TableauColumn[] tableau, int seed)
        {
            if(seed == -(1))
            {
                _randomNumbers = new Random();
            }
            else
            {
                _randomNumbers = new Random(seed);
            }

            ShuffleNewDeck(stock.Pile);
            DealCards(stock.Pile, tableau);
        }

        /// <summary>
        /// Draws the next three cards from the stock, or returns the discard pile to the stock
        /// if the stock is empty.
        /// </summary>
        /// <param name="stock">The stock.</param>
        /// <param name="discard">The discard pile.</param>
        public void DrawCardsFromStock(CardPile stock, DiscardPile discard)
        {
            RemoveSelection();
            ///If stock is empty, send cards from discard back to stock
            if (stock.Pile.Count() == 0)
            {
                while (discard.Pile.Count > 0)
                {
                    stock.Pile.Push(discard.Pile.Pop());
                }
            }
            ///Draw 3 cards or the Stock Pile count. Whichever the minimum between the two is
            else
            {
                int num = Math.Min(3, stock.Pile.Count());
                for (int i = 0; i < num; i++)
                {
                    discard.Pile.Push(stock.Pile.Pop());
                }
            }
        }

        /// <summary>
        /// Selects the top discarded card, or removes the selection if there already is one.
        /// </summary>
        /// <param name="discard">The discard pile.</param>
        public void SelectDiscard(DiscardPile discard)
        {
            // If discard is Not selected & _tableauColumnSelected is null, Set selected property to true
            if (!discard.IsSelected && _tableauColumnSelected == null)
            {
                discard.IsSelected = true;
                _discardPileSelected = discard;
            }
            else
            {
                //Remove Selections
                RemoveSelection();
            }
        }

        /// <summary>
        /// Selects the given number of cards from the given tableau column or tries to move
        /// any currently-selected cards to the given tableau column.
        /// </summary>
        /// <param name="col">The column to select or to move cards to.</param>
        /// <param name="n">The number of cards to select.</param>
        /// <returns>Whether the play wins the game.</returns>
        public bool SelectTableauCards(TableauColumn col, int n)
        {
            // If discard pile isnt null & given number of cards is <=1
            // Transfer cards from discard pile to the columns face up pile
            if (_discardPileSelected != null)
            {
                if (n <= 1)
                {
                    DiscardPileToTQ(_discardPileSelected.Pile, col.FaceUpPile);
                }
                RemoveSelection();
            }
            //Moves cards from one tableau column to another
            else if (_tableauColumnSelected != null)
            {
                if (n <= 1)
                {
                    MoveSelectedCards(_tableauColumnSelected, col.FaceUpPile);
                }
                RemoveSelection();
            }
            //selects the whole column
            else if (n > 0)
            {
               
               _tableauColumnSelected = col;
                col.NumberSelected = n;
            }
            
            return GameWon();
        }

        /// <summary>
        /// Moves the selected card to the given foundation pile, if possible
        /// </summary>
        /// <param name="dest">The foundation pile.</param>
        /// <returns>Whether the move wins the game.</returns>
        public bool SelectFoundationPile(Stack<Card> dest)
        {
            // Move card from discard to foundation, If discard pile is not null
            if (_discardPileSelected != null)
            {
                DiscardToFoundation(_discardPileSelected.Pile, dest);
                RemoveSelection();
            }
            //Move card from tableau Column to foundation pile if Tableau col is not null
            else if(_tableauColumnSelected != null)
            {
                TableauToFoundation(_tableauColumnSelected, dest);
                RemoveSelection();
            }
            return GameWon();
        }

        /// <summary>
        /// Transfer card from Stack to TableauQueue
        /// </summary>
        /// <param name="tqReceiving">TableauQueue receiving the card</param>
        /// <param name="stack">Stack from which the card is being pulled out of</param>
        private void StackTableTransfer(TableauQueue tqReceiving, Stack<Card> stack)
        {
            tqReceiving.Enqueue(stack.Pop()); 
        }


        /// <summary>
        /// Transfer a card from a TableauQueue to a Stack/Pile
        /// </summary>
        /// <param name="stackReceiving"> The stack which the card is being pushed onto</param>
        /// <param name="tq"> TableauQueue which the card is coming from </param>
        private void StackTableTransfer(Stack<Card> stackReceiving, TableauQueue tq) 
        {
            stackReceiving.Push(tq.Dequeue());
        }

        /// <summary>
        /// Transfer a certain amaount of cards from one stack to another
        /// </summary>
        /// <param name="stackRecieving">Stack which is getting the cards</param>
        /// <param name="stackGiving">Stack which is giving the cards</param>
        /// <param name="numToMove">Number of cards being moved</param>
        private void StackToStack(Stack<Card> stackRecieving, Stack<Card> stackGiving, int numToMove)
        {
            for(int i = 0; i<numToMove; i++)
            {
                stackRecieving.Push(stackGiving.Pop());
            }
        }

        /// <summary>
        /// Method to move cards from one tableque to another
        /// </summary>
        /// <param name="tqReceiving">Table Queue receiving the card</param>
        /// <param name="tqGiving">Tableau Queue giving the card</param>
        /// <param name="num">number of cards to move</param>
        private void TableQToTableQ(TableauQueue tqReceiving, TableauQueue tqGiving, int num)
        {
            for (int i = 0; i < num; i++)
            {
                tqReceiving.Enqueue(tqGiving.Dequeue());
            }
        }

        /// <summary>
        /// Remove selectrion from a card pile or TableauColumn
        /// </summary>
        private void RemoveSelection()
        {
            //Deselect if not null
            if(_discardPileSelected != null)
            {
                _discardPileSelected.IsSelected = false;
                _discardPileSelected = null;
            }
            if(_tableauColumnSelected != null)
            {
                _tableauColumnSelected.NumberSelected = 0;
                _tableauColumnSelected = null;
            }
            
        }

        /// <summary>
        /// Returns a private bool indicating if a given card can
        /// be added onto a tableauQueue
        /// </summary>
        /// <param name="cardToMove">Card which will be moved</param>
        /// <param name="tableauQueue">Tableau onto which card is attempted to be placed</param>
        /// <returns>true if card can be added to TableauQueue. False otherwise </returns>
        private bool CanGoOnTableau(Card cardToMove, TableauQueue tableauQueue)
        {
            // returns true if its a king going onto an empty tableau queue
            if(tableauQueue.Count == 0)
            {
                return cardToMove.Rank == Card.MaxRank;
            }
            else
            {
                //returns true if its on less rank and are different colors
                Card topCard = tableauQueue.PeekBack();
                return(topCard.Rank-1 == cardToMove.Rank) && (topCard.IsRed != cardToMove.IsRed);
            }
        }

        /// <summary>
        /// Method to validate whether a given card can be added onto a foundation pile
        /// </summary>
        /// <param name="cardToMove">Card in question</param>
        /// <param name="foundationPile">Foudation pile which card will be moved to</param>
        /// <returns>true if card can be added to foundation pile. False otherwise</returns>
        private bool CanGoOnFoundation(Card cardToMove, Stack<Card> foundationPile)
        {
            if(foundationPile.Count == 0)
            {
                //Return true if its an ace going onto an empty foundation pile
                return cardToMove.Rank == Card.MinRank;
            }
            else
            {
                // return true if the card going onto pil is one rank greater and the same suit
                return (cardToMove.Rank == foundationPile.Peek().Rank + 1) && (cardToMove.Suit == foundationPile.Peek().Suit);
            }
        }

        /// <summary>
        /// Method to indicate whether a game has been won or not
        /// </summary>
        /// <returns>returns true if game has been won; false otherwise</returns>
        private bool GameWon()
        {
            // return true if the number in stock and discard is<= 1 and number of hidden cards is 0
            return ((_numStockAndDiscard <= 1) && (_numFaceDownTC == 0));
        }

        /// <summary>
        /// Method to flip a hidden card in a Tableau Column
        /// </summary>
        /// <param name="tc">Tableau Column in which the card will be flipped</param>
        private void TableauColumnFlip(TableauColumn tc)
        {
            //Flip a cad if there are no face up cards and at least on face down card
            if(tc.FaceDownPile.Count > 0 && tc.FaceUpPile.Count == 0)
            {
                tc.FaceUpPile.Enqueue(tc.FaceDownPile.Pop());
                _numFaceDownTC--;
            }
        }

        /// <summary>
        /// Method to deal cards
        /// </summary>
        /// <param name="stock"> Stack giving the deck of cards</param>
        /// <param name="tcArray"> Tableau Column array giving the tableau columns to 
        ///                        which the cards will be dealt</param>
        private void DealCards(Stack<Card> stock, TableauColumn[] tcArray)
        {
            for (int i = 0; i < tcArray.Length; i++)
            {
                //Add a face up card to every column from stock
                tcArray[i].FaceUpPile.Enqueue(stock.Pop());

                for (int j = i + 1; j < tcArray.Length; j++)
                {
                    //adds face down cards from stock
                    tcArray[j].FaceDownPile.Push(stock.Pop());
                }
            }
        }


        /// <summary>
        /// Method to mave cards from the discard pile to a tableau queue
        /// </summary>
        /// <param name="discardPile">the discard pile to grab the card from</param>
        /// <param name="tq">the TableauQueue onto which the card is going on to</param>
        private void DiscardPileToTQ(Stack<Card> discardPile, TableauQueue tq) 
        {
            Card cardToMove = discardPile.Peek();
            //If the move is legal, then move card from discard to table queue
            if (CanGoOnTableau(cardToMove, tq))
            {
                tq.Enqueue(discardPile.Pop());
                _numStockAndDiscard--;
            }
        }

        /// <summary>
        /// Moves a card from discard pile to Foundation pile
        /// </summary>
        /// <param name="discardPile">discard pile giving the card</param>
        /// <param name="foundationPile">foundation pile receiving the card</param>
        private void DiscardToFoundation(Stack<Card> discardPile, Stack<Card> foundationPile)
        {
            Card cardToMove = discardPile.Peek();
            //If the move is legal, then move card from discard to foundation pile
            if (CanGoOnFoundation(cardToMove, foundationPile)){
                foundationPile.Push(discardPile.Pop());
                _numStockAndDiscard--;
            }
        }

        /// <summary>
        /// Method to move selected cards from one Tableau column to another
        /// </summary>
        /// <param name="columnGiving">Column giving the selected cards</param>
        /// <param name="queueReceiving">Queue receiving the selected cards from the column</param>
        private void MoveSelectedCards(TableauColumn columnGiving, TableauQueue queueReceiving)
        {
            int numSelected = columnGiving.NumberSelected;
            TableQToTableQ(columnGiving.FaceUpPile, columnGiving.FaceUpPile, columnGiving.FaceUpPile.Count - numSelected);
            Card frontCard = columnGiving.FaceUpPile.PeekFront();
            //If move is legal Move cards from one Tableau Queue to another and flip a hidden card if needed
            if (CanGoOnTableau(frontCard, queueReceiving))
            {
                TableQToTableQ(queueReceiving, columnGiving.FaceUpPile, numSelected);
                TableauColumnFlip(columnGiving);            
            }
            else
            {
                TableQToTableQ(columnGiving.FaceUpPile, columnGiving.FaceUpPile, numSelected);
            }
        }

        /// <summary>
        /// Method to move given card from a tableau column to a foundation pile
        /// </summary>
        /// <param name="tcGiving">tableau column giving the card</param>
        /// <param name="foundationPile">foundation pile receiving the card</param>
        private void TableauToFoundation(TableauColumn tcGiving, Stack<Card> foundationPile)
        {
            Card backCard = tcGiving.FaceUpPile.PeekBack();
            //If the move is legal, then move card from Tableau Column to foundation pile
            if (CanGoOnFoundation(backCard, foundationPile))
            {
                TableQToTableQ(tcGiving.FaceUpPile, tcGiving.FaceUpPile, tcGiving.FaceUpPile.Count - 1);

                foundationPile.Push(tcGiving.FaceUpPile.Dequeue());

                TableauColumnFlip(tcGiving);
            }
            
        }

    }
}
