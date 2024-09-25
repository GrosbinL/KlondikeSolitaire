/* BGameTests.cs
 * Author: Rod Howell
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grosbin.Games.KlondikeSolitaire.Tests
{
    /// <summary>
    /// Unit tests for the Game class.
    /// </summary>
    public class BGameTests
    {
        /// <summary>
        /// The initial cards in the stock when a seed of 0 is used.
        /// </summary>
        private static readonly Card[] _initialStock = new Card[]
        {
            new Card(1, Suit.Clubs),
            new Card(4, Suit.Hearts),
            new Card(10, Suit.Hearts),
            new Card(8, Suit.Spades),
            new Card(11, Suit.Clubs),
            new Card(11, Suit.Diamonds),
            new Card(9, Suit.Hearts),
            new Card(6, Suit.Hearts),
            new Card(2, Suit.Clubs),
            new Card(13, Suit.Spades),
            new Card(6, Suit.Diamonds),
            new Card(13, Suit.Diamonds),
            new Card(6, Suit.Spades),
            new Card(13, Suit.Clubs),
            new Card(12, Suit.Clubs),
            new Card(5, Suit.Spades),
            new Card(7, Suit.Diamonds),
            new Card(12, Suit.Spades),
            new Card(1, Suit.Hearts),
            new Card(10, Suit.Clubs),
            new Card(2, Suit.Hearts),
            new Card(13, Suit.Hearts),
            new Card(3, Suit.Spades),
            new Card(12, Suit.Hearts),
        };

        /// <summary>
        /// The initial face-down cards on the tableau when a seed of 0 is used.
        /// </summary>
        private static readonly Card[][] _faceDownCards = new Card[][]
        {
            new Card[]
            {
            },
            new Card[]
            {
                new Card(8, Suit.Hearts),
            },
            new Card[]
            {
                new Card(3, Suit.Clubs),
                new Card(4, Suit.Clubs),
            },
            new Card[]
            {
                new Card(10, Suit.Spades),
                new Card(10, Suit.Diamonds),
                new Card(3, Suit.Diamonds),
            },
            new Card[]
            {
                new Card(6, Suit.Clubs),
                new Card(3, Suit.Hearts),
                new Card(8, Suit.Clubs),
                new Card(1, Suit.Spades),
            },
            new Card[]
            {
                new Card(1, Suit.Diamonds),
                new Card(9, Suit.Spades),
                new Card(7, Suit.Spades),
                new Card(7, Suit.Clubs),
                new Card(9, Suit.Clubs),
            },
            new Card[]
            {
                new Card(2, Suit.Diamonds),
                new Card(5, Suit.Diamonds),
                new Card(5, Suit.Clubs),
                new Card(4, Suit.Spades),
                new Card(4, Suit.Diamonds),
                new Card(5, Suit.Hearts),
            },
        };

        /// <summary>
        /// The face-up cards on the tableau when a seed of 0 is used.
        /// </summary>
        private static readonly Card[] _faceUpCards = new Card[]
        {
            new Card(8, Suit.Diamonds),
            new Card(7, Suit.Hearts),
            new Card(9, Suit.Diamonds),
            new Card(11, Suit.Spades),
            new Card(11, Suit.Hearts),
            new Card(12, Suit.Diamonds),
            new Card(2, Suit.Spades),
        };

        /// <summary>
        /// Initializes the elements of the given array to new TableauColumns.
        /// </summary>
        /// <param name="tableau">The array of columns.</param>
        private static void InitializeTableau(TableauColumn[] tableau)
        {
            for (int i = 0; i < tableau.Length; i++)
            {
                tableau[i] = new TableauColumn();
            }
        }

        /// <summary>
        /// Initializes the elements of the given array to new CardPiles.
        /// </summary>
        /// <param name="foundation">The array of foundation piles.</param>
        private static void InitializeFoundation(CardPile[] foundation)
        {
            for (int i = 0; i < foundation.Length; i++)
            {
                foundation[i] = new CardPile();
            }
        }

        /// <summary>
        /// Tests that the given stock and tableau columns have the number of cards that the
        /// initial deal should have.
        /// </summary>
        /// <param name="stock">The stock.</param>
        /// <param name="tableau">The tableau columns.</param>
        private static void VerifyLayout(CardPile stock, TableauColumn[] tableau)
        {
            Assert.Multiple(() =>
            {
                Assert.That(stock.Pile, Has.Count.EqualTo(24),
                    "The stock should have 24 cards.");
                for (int i = 0; i < tableau.Length; i++)
                {
                    Assert.That(tableau[i].FaceDownPile, Has.Count.EqualTo(i),
                        $"Tableau column {i} should have {i} face down cards.");
                    Assert.That(tableau[i].FaceUpPile, Has.Count.EqualTo(1),
                        $"Tableau column {i} should have 1 face up card.");
                }
            });
        }

        /// <summary>
        /// Tests that constructing two games with no seed correctly constructs two different games.
        /// </summary>
        [Test]
        [Timeout(2000), Category("A: Constructor")]
        public void TestNoSeed()
        {
            CardPile stock1 = new();
            CardPile stock2 = new();
            TableauColumn[] tableau1 = new TableauColumn[7];
            InitializeTableau(tableau1);
            TableauColumn[] tableau2 = new TableauColumn[7];
            InitializeTableau(tableau2);
            Game game1 = new(stock1, tableau1, -1);
            VerifyLayout(stock1, tableau1);
            Game game2 = new(stock2, tableau2, -1);
            Assert.That(stock1.Pile, Is.Not.EqualTo(stock2.Pile),
                "The two games should not have the same sequence of cards in the stock.");
        }

        /// <summary>
        /// Tests that the correct game is dealt when a seed of 0 is used.
        /// </summary>
        [Test]
        [Timeout(5000), Category("A: Constructor")]
        public void TestSeed()
        {
            CardPile stock = new();
            TableauColumn[] tableau = new TableauColumn[7];
            InitializeTableau(tableau);
            Game g = new(stock, tableau, 0);
            Card[] stockCards = stock.Pile.ToArray();
            Card[][] faceDownCards = new Card[7][];
            for (int i = 0; i < faceDownCards.Length; i++)
            {
                faceDownCards[i] = tableau[i].FaceDownPile.ToArray();
            }
            Card[] faceUpCards = new Card[7];
            for (int i = 0; i < faceUpCards.Length; i++)
            {
                faceUpCards[i] = tableau[i].FaceUpPile.PeekBack();
            }
            Assert.Multiple(() =>
            {
                // In failure messages, array indices start at the top of the stacks.
                Assert.That(stockCards, Is.EqualTo(_initialStock),
                    "The stock is not correct.");

                for (int i = 0; i < faceDownCards.Length; i++)
                {
                    // In failure messages, array indices start at the top of the stacks.
                    Assert.That(faceDownCards[i], Is.EqualTo(_faceDownCards[i]),
                        $"The face down cards on column {i} are not correct.");
                }

                // In failure messages, array indices indicate the tableau column.
                Assert.That(faceUpCards, Is.EqualTo(_faceUpCards),
                    "The face up cards are not correct.");
            });
        }

        /// <summary>
        /// Tests drawing cards using a game with a seed of 0. All cards are drawn, then another "draw"
        /// is performed to return the cards to the stock.
        /// </summary>
        [Test]
        [Timeout(5000), Category("B: Simple Actions")]
        public void TestDrawCards()
        {
            CardPile stock = new();
            TableauColumn[] tableau = new TableauColumn[7];
            InitializeTableau(tableau);
            Game g = new(stock, tableau, 0);
            DiscardPile dis = new();

            // First draw
            g.DrawCardsFromStock(stock, dis);
            Assert.Multiple(() =>
            {
                Assert.That(stock.Pile, Has.Count.EqualTo(21),
                    "After the first draw, the stock should have 21 cards remaining.");
                Assert.That(dis.Pile.ToArray(),
                    Is.EqualTo(new Card[] { new Card(10, Suit.Hearts), new Card(4, Suit.Hearts), new Card(1, Suit.Clubs) }),
                    "The discard pile is incorrect after the first draw.");
            });

            // Draw remainder of stock
            while (stock.Pile.Count > 0)
            {
                g.DrawCardsFromStock(stock, dis);
            }
            Assert.That(dis.Pile.Peek(), Is.EqualTo(new Card(12, Suit.Hearts)),
                "When the stock is empty, the top card on the discard pile should be the Queen of Hearts.");

            // Flip discard pile back to stock.
            g.DrawCardsFromStock(stock, dis);
            Assert.Multiple(() =>
            {
                Assert.That(dis.Pile, Has.Count.EqualTo(0),
                    "After the draw from the empty stock, the discard pile should be empty.");
                Assert.That(stock.Pile, Is.EqualTo(_initialStock),
                    "After the draw from the empty stock, the stock should contain its initial contents.");
            });
        }

        /// <summary>
        /// Tests selections that don't result in moving cards. The tests are on the initial game 
        /// generated with seed 0.
        /// </summary>
        [Test]
        [Timeout(1000), Category("B: Simple Actions")]
        public void TestSelections()
        {
            CardPile stock = new();
            TableauColumn[] tableau = new TableauColumn[7];
            InitializeTableau(tableau);
            Game g = new(stock, tableau, 0);
            CardPile[] foundation = new CardPile[4];
            InitializeFoundation(foundation);
            Card[][] faceUpCards = new Card[7][];

            // Place each face-up column in an array within the above array of arrays.
            for (int i = 0; i < faceUpCards.Length; i++)
            {
                faceUpCards[i] = new Card[] { _faceUpCards[i] };
            }

            // First selection - 8 of Diamonds
            Assert.Multiple(() =>
            {
                Assert.That(g.SelectTableauCards(tableau[0], 1), Is.False,
                    "The first selection should not win the game.");
                Assert.That(tableau[0].NumberSelected, Is.EqualTo(1),
                    "After the first selection, 1 card should be selected on tableau column 0.");
                Assert.That(tableau[0].FaceUpPile.ToArray(), Is.EqualTo(faceUpCards[0]),
                    "The first selection should leave the face-up cards on tableau column 0 unchaged.");
                Assert.That(tableau[0].FaceDownPile.ToArray, Is.EqualTo(_faceDownCards[0]),
                    "The first selction should leave the face-down cards on tableau column 0 unchanged.");
            });

            // Second selection 9 of Diamonds
            Assert.Multiple(() =>
            {
                Assert.That(g.SelectTableauCards(tableau[2], 1), Is.False,
                    "The second selection should not win the game.");
                Assert.That(tableau[0].NumberSelected, Is.EqualTo(0),
                    "Because the second selection is an illegal move, the number selected on tableau column 0 should be set to 0.");
                Assert.That(tableau[2].NumberSelected, Is.EqualTo(0),
                    "Because a selection already existed, no cards should be selected on tableau column 2.");
                Assert.That(tableau[0].FaceUpPile.ToArray(), Is.EqualTo(faceUpCards[0]),
                    "The second selection should leave the face-up cards on tableau column 0 unchanged.");
                Assert.That(tableau[0].FaceDownPile.ToArray(), Is.EqualTo(_faceDownCards[0]),
                    "The second selection should leave the face-down cards on tableau column 0 unchanged.");
                Assert.That(tableau[2].FaceUpPile.ToArray(), Is.EqualTo(faceUpCards[2]),
                    "The second selection should leave the face-up cards on tableau column 2 unchanged.");
                Assert.That(tableau[2].FaceDownPile.ToArray(), Is.EqualTo(_faceDownCards[2]),
                    "The second selection should leave the face-down cards on tableau column 2 unchanged.");
            });

            // Third selection - Jack of Spades
            Assert.Multiple(() =>
            {
                Assert.That(g.SelectTableauCards(tableau[3], 1), Is.False,
                    "The third selection should not win the game.");
                Assert.That(tableau[3].NumberSelected, Is.EqualTo(1),
                    "The third selection should cause 1 card to be selected on tableau column 3.");
                Assert.That(tableau[3].FaceUpPile.ToArray(), Is.EqualTo(faceUpCards[3]),
                    "The third selection should leave the face-up cards on tableau column 3 unchanged.");
                Assert.That(tableau[3].FaceDownPile.ToArray(), Is.EqualTo(_faceDownCards[3]),
                    "The third selection should leave the face-down cards on tableau column 3 unchanged.");
            });

            // Fourth selection - third foundation pile
            Assert.Multiple(() =>
            {
                Assert.That(g.SelectFoundationPile(foundation[2].Pile), Is.False,
                    "The fourth selection should not win the game.");
                Assert.That(foundation[2].Pile, Has.Count.EqualTo(0),
                    "The fourth selection should not move a card to the foundation.");
                Assert.That(tableau[3].NumberSelected, Is.EqualTo(0),
                    "The fourth selection should set the number selected on tableau column 3 to 0.");
                Assert.That(tableau[3].FaceUpPile.ToArray(), Is.EqualTo(faceUpCards[3]),
                    "The fourth selection should leave the face-up cards on tableau column 3 unchanged.");
                Assert.That(tableau[3].FaceDownPile.ToArray(), Is.EqualTo(_faceDownCards[3]),
                    "The fourth selection should not leave the face-down cards on tableau column 3 unchanged.");
            });

            // Fifth selection - 2 of Spades
            g.SelectTableauCards(tableau[6], 1);

            // Sixth selection - 7 of Hearts
            g.SelectTableauCards(tableau[1], 1);
            Assert.Multiple(() =>
            {
                Assert.That(tableau[6].NumberSelected, Is.EqualTo(0),
                    "The sixth selection should deselect the selected card.");
                Assert.That(tableau[6].FaceUpPile.ToArray(), Is.EqualTo(faceUpCards[6]),
                    "The sixth selection should leave the face-up cards on tableau column 6 unchanged.");
                Assert.That(tableau[6].FaceDownPile.ToArray(), Is.EqualTo(_faceDownCards[6]),
                    "The sixth selection should leave the face-down cards on tableau column 6 unchanged.");
                Assert.That(tableau[1].NumberSelected, Is.EqualTo(0),
                    "The sixth selection should not select any cards on tableau column 1.");
                Assert.That(tableau[1].FaceUpPile.ToArray(), Is.EqualTo(faceUpCards[1]),
                    "The sixth selection should leave the face-up cards on tableau column 1 unchaged.");
                Assert.That(tableau[1].FaceDownPile.ToArray(), Is.EqualTo(_faceDownCards[1]),
                    "The sixth selection should leave the face-down cards on tableau column 1 unchanged.");
            });
        }

        /// <summary>
        /// Tests simple actions involving the discard pile with a seed of 0. None of these actions other than drawing cards move any cards.
        /// </summary>
        [Test]
        [Timeout(5000), Category("C: Simple Actions with Discard Pile")]
        public void TestSelectionsWithDiscard()
        {
            CardPile stock = new();
            TableauColumn[] tableau = new TableauColumn[7];
            InitializeTableau(tableau);
            Game g = new(stock, tableau, 0);
            CardPile[] foundation = new CardPile[4];
            InitializeFoundation(foundation);
            DiscardPile dis = new();
            Card[][] faceUpCards = new Card[7][];

            // Place each face-up column in an array within the above array of arrays.
            for (int i = 0; i < faceUpCards.Length; i++)
            {
                faceUpCards[i] = new Card[] { _faceUpCards[i] };
            }

            // Draw cards
            g.DrawCardsFromStock(stock, dis);
            Card[] discardedCards = dis.Pile.ToArray();

            // Try to move Queen of Diamonds to discard pile
            g.SelectTableauCards(tableau[5], 1);
            g.SelectDiscard(dis);
            Assert.Multiple(() =>
            {
                Assert.That(tableau[5].FaceUpPile.ToArray(), Is.EqualTo(faceUpCards[5]),
                    "The first attempted move should leave the face-up cards on tableau column 5 unchanged.");
                Assert.That(tableau[5].FaceDownPile.ToArray(), Is.EqualTo(_faceDownCards[5]),
                    "The first apptempted move should leave the face-down cards on tableau column 5 unchanged.");
                Assert.That(dis.Pile.ToArray(), Is.EqualTo(discardedCards),
                    "The first attempted move should leave the discard pile unchanged.");
                Assert.That(tableau[5].NumberSelected, Is.EqualTo(0),
                    "The first attempted move should cause the Queen of Diamonds to be deselected.");
                Assert.That(dis.IsSelected, Is.False,
                    "The first attempted move should not select the discard pile.");
            });

            // Try to move 10 of Hearts onto Jack of Hearts
            g.SelectDiscard(dis);
            Assert.Multiple(() =>
            {
                Assert.That(dis.IsSelected, Is.True,
                    "Selecting the discard pile should make IsSelected true.");
                Assert.That(dis.Pile.ToArray(), Is.EqualTo(discardedCards),
                    "Selecting the discard pile shouldn't change its contents.");
                Assert.That(g.SelectTableauCards(tableau[4], 1), Is.False,
                    "The second attempted move should not end the game.");
                Assert.That(dis.Pile.ToArray(), Is.EqualTo(discardedCards),
                    "Because the second move is illegal, the contents of the discard pile shouldn't change.");
                Assert.That(dis.IsSelected, Is.False,
                    "The second attempted move should deselect the discard pile.");
                Assert.That(tableau[4].FaceUpPile.ToArray(), Is.EqualTo(faceUpCards[4]),
                    "The second attempted move should leave the face-up cards on tableau 4 unchanged.");
                Assert.That(tableau[4].FaceDownPile.ToArray(), Is.EqualTo(_faceDownCards[4]),
                    "The second attempted move should leave the face-down cards on tableau column 4 unchanged.");
            });

            // Try to move 10 of Hearts onto foundation pile 0
            g.SelectDiscard(dis);
            Assert.Multiple(() =>
            {
                Assert.That(g.SelectFoundationPile(foundation[0].Pile), Is.False,
                    "The third attempted move should not win the game.");
                Assert.That(foundation[0].Pile, Has.Count.EqualTo(0),
                    "The third attempted move should leave the foundation unchanged.");
                Assert.That(dis.Pile.ToArray(), Is.EqualTo(discardedCards),
                    "Because the third move is illegal, the contents of the discard pile shouldn't change.");
                Assert.That(dis.IsSelected, Is.False,
                    "The third attempted move should deselect the discard pile.");
            });

            // Select 10 of Hearts, then select stock
            g.SelectDiscard(dis);
            g.DrawCardsFromStock(stock, dis);
            Assert.Multiple(() =>
            {
                Assert.That(dis.IsSelected, Is.False,
                    "The second draw should deselect the discard pile.");
                Assert.That(dis.Pile, Has.Count.EqualTo(6),
                    "The second draw should draw the next three cards.");
            });

            // Select 7 of Hearts, then select stock
            g.SelectTableauCards(tableau[1], 1);
            g.DrawCardsFromStock(stock, dis);
            Assert.Multiple(() =>
            {
                Assert.That(tableau[1].NumberSelected, Is.EqualTo(0),
                    "The third draw should deselect the 7 of Hearts.");
                Assert.That(tableau[1].FaceUpPile.ToArray(), Is.EqualTo(faceUpCards[1]),
                    "The third draw should leave the face-up cards on tableau column 1 unchanged.");
                Assert.That(tableau[1].FaceDownPile.ToArray(), Is.EqualTo(_faceDownCards[1]),
                    "The third draw should leave the face-down cards on tableau column 1 unchanged.");
                Assert.That(dis.Pile, Has.Count.EqualTo(9),
                    "The third draw should draw the next three cards.");
            });
        }

        /// <summary>
        /// Plays a long sequence from the given initial board components, testing various actions.
        /// The given board components are assumed to describe the initial game position generated
        /// with a seed of 1.
        /// </summary>
        /// <param name="stock">The stock.</param>
        /// <param name="tableau">The tableau columns.</param>
        /// <param name="g">The game.</param>
        /// <param name="foundation">The foundation piles.</param>
        /// <param name="dis">The discard pile.</param>
        private void PlayGame(CardPile stock, TableauColumn[] tableau, Game g, CardPile[] foundation,
            DiscardPile dis)
        {
            // Place the tableau cards into arrays for checking later.
            Card[][] faceUpCards = new Card[7][];
            Card[][] faceDownCards = new Card[7][];
            for (int i = 0; i < faceUpCards.Length; i++)
            {
                faceUpCards[i] = tableau[i].FaceUpPile.ToArray();
                faceDownCards[i] = tableau[i].FaceDownPile.ToArray();
            }

            // Ace of Clubs onto foundation 1, flips 6 of Diamonds
            g.SelectTableauCards(tableau[3], 1);
            Assert.Multiple(() =>
            {
                Assert.That(g.SelectFoundationPile(foundation[1].Pile), Is.False,
                    "The first move doesn't win the game.");
                Assert.That(foundation[1].Pile.ToArray(), Is.EqualTo(faceUpCards[3]),
                    "Foundation pile 1 should contain the Ace of Clubs.");
                Assert.That(tableau[3].NumberSelected, Is.EqualTo(0),
                    "The first move should deselect tableau column 3.");
                Assert.That(tableau[3].FaceUpPile.ToArray(), Is.EqualTo(new Card[] { faceDownCards[3][0] }),
                    "The first move should flip the 6 of Diamonds.");
                Card[] newPile = new Card[faceDownCards[3].Length - 1];
                Array.Copy(faceDownCards[3], 1, newPile, 0, newPile.Length);
                Assert.That(tableau[3].FaceDownPile.ToArray(), Is.EqualTo(newPile),
                    "The first move should remove the top card from the face down-pile on tableau column 3.");
            });

            // 10 of Spades to Jack of Hearts, flips Ace of Diamonds
            g.SelectTableauCards(tableau[4], 1);
            Assert.Multiple(() =>
            {
                Assert.That(g.SelectTableauCards(tableau[2], 1), Is.False,
                    "The second move doesn't win the game.");
                Assert.That(tableau[4].NumberSelected, Is.EqualTo(0),
                    "The second move should deselect tableau column 4.");
                Assert.That(tableau[4].FaceUpPile.ToArray(), Is.EqualTo(new Card[] { faceDownCards[4][0] }),
                    "The second move should flip the Ace of Diamonds.");
                Card[] newPile = new Card[faceDownCards[4].Length - 1];
                Array.Copy(faceDownCards[4], 1, newPile, 0, newPile.Length);
                Assert.That(tableau[4].FaceDownPile.ToArray(), Is.EqualTo(newPile),
                    "The second move should remove the top card from the face-down pile on tableau column 4.");
                Assert.That(tableau[2].NumberSelected, Is.EqualTo(0),
                    "The second move shouldn't select cards on tableau column 2.");
                Assert.That(tableau[2].FaceUpPile.ToArray, Is.EqualTo(new Card[] { faceUpCards[2][0], faceUpCards[4][0] }),
                    "The second move should place the 10 of Spades on top of the Jack of Hearts.");
            });

            // Ace of Diamonds onto foundation 0, flips 7 of Spades
            g.SelectTableauCards(tableau[4], 1);
            g.SelectFoundationPile(foundation[0].Pile);

            // Draw cards
            g.DrawCardsFromStock(stock, dis);

            // 9 of Clubs to 10 of Diamonds, flips Queen of Spades
            g.SelectTableauCards(tableau[1], 1);
            g.SelectTableauCards(tableau[6], 1);

            // Jack of Hearts - 10 of Spades to Queen of Spades, flips 3 of Hearts
            g.SelectTableauCards(tableau[2], 2);
            Assert.Multiple(() =>
            {
                Assert.That(tableau[2].NumberSelected, Is.EqualTo(2),
                    "Selecting the Jack of Hearts and Queen of Spades should make NumberSelected 2.");
                Assert.That(tableau[2].FaceDownPile, Has.Count.EqualTo(2),
                    "Selecting the Jack of Hearts and Queen of Spades should not change the face-down cards on tableau column 2.");
                Assert.That(tableau[2].FaceUpPile, Has.Count.EqualTo(2),
                    "Selecting the Jack of Hearts and Queen of Spades shouldn't change the face-up cards on tableau column 2.");
            });
            g.SelectTableauCards(tableau[1], 1);
            Assert.Multiple(() =>
            {
                Assert.That(tableau[2].NumberSelected, Is.EqualTo(0),
                    "The fifth move should deselect tableau column 2.");
                Assert.That(tableau[2].FaceUpPile.ToArray(), Is.EqualTo(new Card[] { faceDownCards[2][0] }),
                    "The fifth move should flip the 3 of Hearts.");
                Assert.That(tableau[2].FaceDownPile.ToArray(), Is.EqualTo(new Card[] { faceDownCards[2][1] }),
                    "The fifth move should remove the top card from the face-down cards on tableau column 2.");
                Assert.That(tableau[1].NumberSelected, Is.EqualTo(0),
                    "The fifth move shouldn't select tableau column 1.");
                Assert.That(tableau[1].FaceUpPile.ToArray(),
                    Is.EqualTo(new Card[] { new Card(12, Suit.Spades), new Card(11, Suit.Hearts), new Card(10, Suit.Spades) }),
                    "The fifth move should place the Jack of Hearts and 10 of Spades on the Queen of Spades.");
                Assert.That(tableau[1].FaceDownPile, Has.Count.EqualTo(0),
                    "The fifth move should remove the only card from the face-down cards on tableau column 1.");
            });

            // 6 of Diamonds to 7 of Spades, flips 9 of Hearts
            g.SelectTableauCards(tableau[3], 1);
            g.SelectTableauCards(tableau[4], 1);

            // Try 9 of Hearts to Jack of Hearts - 10 of Spades
            g.SelectTableauCards(tableau[3], 1);
            g.SelectTableauCards(tableau[1], 2);
            Assert.Multiple(() =>
            {
                Assert.That(tableau[3].NumberSelected, Is.EqualTo(0),
                    "Trying to select two cards as the target should deselect tableau column 3.");
                Assert.That(tableau[3].FaceUpPile.ToArray(), Is.EqualTo(new Card[] { new Card(9, Suit.Hearts) }),
                    "Trying to select two cards as the target should leave the face-up cards on tableau column 3 unchanged.");
                Assert.That(tableau[1].FaceUpPile, Has.Count.EqualTo(3),
                    "Trying to select two cards as the target should leave the face-up cards on tableau column 1 unchanged.");
            });

            // 9 of Hearts to 10 of Spades, flips 2 of Diamonds
            g.SelectTableauCards(tableau[3], 1);
            g.SelectTableauCards(tableau[1], 1);

            // Try 2 of Diamonds to Ace of Clubs
            g.SelectTableauCards(tableau[3], 1);
            g.SelectFoundationPile(foundation[1].Pile);
            Assert.Multiple(() =>
            {
                Assert.That(tableau[3].NumberSelected, Is.EqualTo(0),
                    "Attempting to move the 2 of Diamonds onto the Ace of Clubs should deslect tableau column 3.");
                Assert.That(tableau[3].FaceUpPile.ToArray(), Is.EqualTo(new Card[] { faceDownCards[3][2] }),
                    "Attempting to move the 2 of Diamonds onto the Ace of Clubs should leave the face-up cards on tableau column 1 unchanged.");
                Assert.That(foundation[1].Pile.ToArray(), Is.EqualTo(new Card[] { new Card(1, Suit.Clubs) }),
                    "Attempting to move the 2 of Diamonds onto the Ace of Clubs should leave foundation 1 unchanged.");
            });

            // 2 of Diamonds on Ace of Diamonds
            g.SelectTableauCards(tableau[3], 1);
            g.SelectFoundationPile(foundation[0].Pile);
            Assert.Multiple(() =>
            {
                Assert.That(tableau[3].NumberSelected, Is.EqualTo(0),
                    "The eighth move should deselect tableau column 3.");
                Assert.That(tableau[3].FaceUpPile, Has.Count.EqualTo(0),
                    "The eighth move should empty the face-up pile on tableau column 3.");
                Assert.That(tableau[3].FaceDownPile, Has.Count.EqualTo(0),
                    "The eighth move should leave the face-down pile on tableau column 3 empty.");
                Assert.That(foundation[0].Pile.ToArray(), Is.EqualTo(new Card[] { new Card(2, Suit.Diamonds), new Card(1, Suit.Diamonds) }),
                    "The eighth move should place the 2 of Diamonds onto foundation 0.");
            });

            // Try 3 of Clubs on Ace of Clubs
            g.SelectDiscard(dis);
            g.SelectFoundationPile(foundation[1].Pile);
            Assert.Multiple(() =>
            {
                Assert.That(dis.IsSelected, Is.False,
                    "Attempting to move the 3 of Clubs onto the Ace of Clubs should deselect the discard pile.");
                Assert.That(foundation[1].Pile.ToArray, Is.EqualTo(new Card[] { new Card(1, Suit.Clubs) }),
                    "Attempting to move the 3 of Clubs onto the Ace of Clubs should leave foundation 1 unchanged.");
            });

            // Try Queen of Spades - 9 of Hearts onto empty column
            g.SelectTableauCards(tableau[1], 4);
            g.SelectTableauCards(tableau[3], 0);
            Assert.Multiple(() =>
            {
                Assert.That(tableau[1].NumberSelected, Is.EqualTo(0),
                    "Trying to move the Queen of Spades to the empty column should deselect tableau column 1.");
                Assert.That(tableau[1].FaceUpPile, Has.Count.EqualTo(4),
                    "Trying to move the Queen of Spades to the empty column should leave the face-up cards on tableau column 1 unchanged.");
                Assert.That(tableau[3].FaceUpPile, Has.Count.EqualTo(0),
                    "Trying to move the Queen of Spades to the empty column should leave the face-up cards on tableau column 3 unchanged.");
            });

            // Try select empty column
            g.SelectTableauCards(tableau[3], 0);
            Assert.Multiple(() =>
            {
                Assert.That(tableau[3].NumberSelected, Is.EqualTo(0),
                    "Trying to select an empty column shouldn't select anything.");
                Assert.That(tableau[3].FaceUpPile, Has.Count.EqualTo(0),
                    "Trying to select an empty column shouldn't change the column's face-up cards.");
            });

            // Try 10 of Spades - 9 of Hearts to empty column
            Card[] pile = tableau[1].FaceUpPile.ToArray();
            g.SelectTableauCards(tableau[1], 2);
            g.SelectTableauCards(tableau[3], 0);
            Assert.Multiple(() =>
            {
                Assert.That(tableau[1].NumberSelected, Is.EqualTo(0),
                    "Trying to move the 10 of Spades to the empty column should deslect tableau column 1.");
                Assert.That(tableau[1].FaceUpPile.ToArray(), Is.EqualTo(pile),
                    "Trying to move the 10 of Spades to the empty column should leave the face-up cards on tableau column 1 unchaged.");
                Assert.That(tableau[3].FaceUpPile, Has.Count.EqualTo(0),
                    "Trying to move the 10 of Spades to the empty pile should leave tableau column 3 empty.");
            });

            // Try 10 of Spades - 9 of Hearts to 9 of Clubs
            g.SelectTableauCards(tableau[1], 2);
            g.SelectTableauCards(tableau[6], 1);
            Assert.Multiple(() =>
            {
                Assert.That(tableau[1].NumberSelected, Is.EqualTo(0),
                    "Trying to move the 10 of Spades to the 9 of Clubs should deslect tableau column 1.");
                Assert.That(tableau[1].FaceUpPile.ToArray(), Is.EqualTo(pile),
                    "Trying to move the 10 of Spades to the 9 of Clubs should leave the face-up cards on tableau column 1 unchaged.");
                Assert.That(tableau[6].FaceUpPile, Has.Count.EqualTo(2),
                    "Trying to move the 10 of Spades to the 9 of Clubs should leave tableau column 6 unchaged.");
            });

            // 8 of Spades on 9 of Hearts, flips 7 of Diamonds
            g.SelectTableauCards(tableau[5], 1);
            g.SelectTableauCards(tableau[1], 1);

            // 7 of Diamonds on 8 of Spades, flips 3 of Diamonds
            g.SelectTableauCards(tableau[5], 1);
            g.SelectTableauCards(tableau[1], 1);

            // 3 of Diamonds on 2 of Diamonds, flips 7 of Hearts
            g.SelectTableauCards(tableau[5], 1);
            g.SelectFoundationPile(foundation[0].Pile);

            // Draw 3 times
            g.DrawCardsFromStock(stock, dis);
            g.DrawCardsFromStock(stock, dis);
            g.DrawCardsFromStock(stock, dis);

            // King of Spades on empty column
            int numDis = dis.Pile.Count;
            g.SelectDiscard(dis);
            g.SelectTableauCards(tableau[3], 0);
            Assert.Multiple(() =>
            {
                Assert.That(dis.IsSelected, Is.False,
                    "The twelfth move should deselect the discard pile.");
                Assert.That(dis.Pile, Has.Count.EqualTo(numDis - 1),
                    "The twelfth move should remove one card from the discard pile.");
                Assert.That(tableau[3].FaceUpPile.ToArray(), Is.EqualTo(new Card[] { new Card(13, Suit.Spades) }),
                    "The twelfth move should move the King of Spades to the face-up cards on tableau column 3.");
            });

            // 6 of Spades on 7 of Diamonds
            g.SelectTableauCards(tableau[0], 1);
            g.SelectTableauCards(tableau[1], 1);

            // King of Hearts on emtpy column
            g.SelectDiscard(dis);
            g.SelectTableauCards(tableau[0], 0);

            // Queen of Spades - 6 of Spades on King of Hearts
            g.SelectTableauCards(tableau[1], 7);
            g.SelectTableauCards(tableau[0], 1);

            // King of Diamonds on empty column
            g.SelectDiscard(dis);
            g.SelectTableauCards(tableau[1], 0);

            // Queen of Diamonds on King of Spades
            g.SelectDiscard(dis);
            g.SelectTableauCards(tableau[3], 1);

            // 2 of Spades on 3 of Hearts
            g.SelectDiscard(dis);
            g.SelectTableauCards(tableau[2], 1);

            // Draw twice
            g.DrawCardsFromStock(stock, dis);
            g.DrawCardsFromStock(stock, dis);

            // 4 of Diamonds on 3 of Diamonds
            g.SelectDiscard(dis);
            g.SelectFoundationPile(foundation[0].Pile);

            // Draw twice, flip stock, draw twice
            g.DrawCardsFromStock(stock, dis);
            g.DrawCardsFromStock(stock, dis);
            g.DrawCardsFromStock(stock, dis);
            g.DrawCardsFromStock(stock, dis);
            g.DrawCardsFromStock(stock, dis);

            // Jack of Spades on Queen of Diamonds
            g.SelectDiscard(dis);
            g.SelectTableauCards(tableau[3], 1);

            // 10 of Diamonds - 9 of Clubs on Jack of Spades, flips 4 of Hearts
            g.SelectTableauCards(tableau[6], 2);
            g.SelectTableauCards(tableau[3], 1);

            // 8 of Hearts on 9 of Clubs
            g.SelectDiscard(dis);
            g.SelectTableauCards(tableau[3], 1);

            // 7 of Spades - 6 of Diamonds on 8 of Hearts, flips 10 of Hearts
            g.SelectTableauCards(tableau[4], 2);
            g.SelectTableauCards(tableau[3], 1);

            // 5 of Hearts on 6 of Spades
            g.SelectDiscard(dis);
            g.SelectTableauCards(tableau[0], 1);

            // 6 of Spades - 5 of Hearts on 7 of Hearts
            g.SelectTableauCards(tableau[0], 2);
            g.SelectTableauCards(tableau[5], 1);
            Assert.Multiple(() =>
            {
                Assert.That(tableau[0].NumberSelected, Is.EqualTo(0),
                    "The 25th move deselects tableau column 0.");
                Assert.That(tableau[0].FaceUpPile.ToArray(),
                    Is.EqualTo(new Card[]
                    {
                        new Card(13, Suit.Hearts),
                        new Card(12, Suit.Spades),
                        new Card(11, Suit.Hearts),
                        new Card(10, Suit.Spades),
                        new Card(9, Suit.Hearts),
                        new Card(8, Suit.Spades),
                        new Card(7, Suit.Diamonds)
                    }),
                    "The 25th move leaves the first 7 cards on tableau column 0.");
                Assert.That(tableau[5].NumberSelected, Is.EqualTo(0),
                    "The 25th move deselects tableau column 5.");
                Assert.That(tableau[5].FaceUpPile.ToArray(),
                    Is.EqualTo(new Card[] { new Card(7, Suit.Hearts), new Card(6, Suit.Spades), new Card(5, Suit.Hearts) }),
                    "The 25th move places 2 cards on tableau column 5.");
            });

            // 3 of Clubs on 4 of Hearts
            g.SelectDiscard(dis);
            g.SelectTableauCards(tableau[6], 1);

            // 5 of Spades on 6 of Diamonds
            g.SelectDiscard(dis);
            g.SelectTableauCards(tableau[3], 1);

            // Queen of Clubs on King of Diamonds
            g.SelectDiscard(dis);
            g.SelectTableauCards(tableau[1], 1);

            // 4 of Hearts - 3 of Clubs on 5 of Spades, flips 8 of Clubs
            g.SelectTableauCards(tableau[6], 2);
            g.SelectTableauCards(tableau[3], 1);

            // 7 of Hearts - 5 of Hearts on 8 of Clubs, flips 4 of Clubs
            g.SelectTableauCards(tableau[5], 3);
            g.SelectTableauCards(tableau[6], 1);

            // 4 of Clubs on 5 of Hearts, flips 10 of Clubs
            g.SelectTableauCards(tableau[5], 1);
            g.SelectTableauCards(tableau[6], 1);

            // 3 of Hearts - 2 of Spades on 4 of Clubs, flips 3 of spades
            g.SelectTableauCards(tableau[2], 2);
            g.SelectTableauCards(tableau[6], 1);

            // Draw
            g.DrawCardsFromStock(stock, dis);

            // 2 of Clubs on Ace of Clubs
            g.SelectDiscard(dis);
            g.SelectFoundationPile(foundation[1].Pile);

            // Draw 3 times
            g.DrawCardsFromStock(stock, dis);
            g.DrawCardsFromStock(stock, dis);
            g.DrawCardsFromStock(stock, dis);

            // 3 of Clubs on 2 of Clubs
            g.SelectTableauCards(tableau[3], 1);
            g.SelectFoundationPile(foundation[1].Pile);

            // 3 of Spades on 4 of Hearts
            g.SelectTableauCards(tableau[2], 1);
            g.SelectTableauCards(tableau[3], 1);

            // King of Clubs on emtpy column
            g.SelectDiscard(dis);
            g.SelectTableauCards(tableau[2], 0);

            // 6 of Clubs on 7 of Diamonds
            g.SelectDiscard(dis);
            g.SelectTableauCards(tableau[0], 1);

            // Jack of Diamonds on Queen of Clubs
            g.SelectDiscard(dis);
            g.SelectTableauCards(tableau[1], 1);

            // Queen of Hearts on King of Clubs
            g.SelectDiscard(dis);
            g.SelectTableauCards(tableau[2], 1);

            // Flip stock, draw
            g.DrawCardsFromStock(stock, dis);
            g.DrawCardsFromStock(stock, dis);

            // 2 of Hearts on 3 of Spades
            g.SelectDiscard(dis);
            g.SelectTableauCards(tableau[3], 1);

            // Jack of Clubs on Queen of Hearts
            g.SelectDiscard(dis);
            g.SelectTableauCards(tableau[2], 1);

            // 10 of Hearts on Jack of Clubs, flips 9 of Spades
            g.SelectTableauCards(tableau[4], 1);
            g.SelectTableauCards(tableau[2], 1);

            // 9 of Diamonds on 10 of Clubs
            g.SelectDiscard(dis);
            g.SelectTableauCards(tableau[5], 1);

            // Draw
            g.DrawCardsFromStock(stock, dis);

            // 8 of Clubs - 2 of Spades on 9 of Diamonds, flips Ace of Spades
            g.SelectTableauCards(tableau[6], 7);
            g.SelectTableauCards(tableau[5], 1);

            // Ace of Spades to foundation 3, flips 5 of Diamonds
            g.SelectTableauCards(tableau[6], 1);
            g.SelectFoundationPile(foundation[3].Pile);

            // 2 of Spades on Ace of Spades
            g.SelectTableauCards(tableau[5], 1);
            g.SelectFoundationPile(foundation[3].Pile);

            // 5 of Diamonds on 6 of Clubs, flips 7 of Clubs
            g.SelectTableauCards(tableau[6], 1);
            g.SelectTableauCards(tableau[0], 1);

            // 8 of Diamonds on 9 of Spades
            g.SelectDiscard(dis);
            g.SelectTableauCards(tableau[4], 1);
        }

        /// <summary>
        /// Tests a complete winning game with a seed of 1. This game is won by first moving all cards
        /// from the stock, then flipping all face-down tableau cards.
        /// </summary>
        [Test]
        [Timeout(5000), Category("D: Complex Actions")]
        public void TestWinByEmptyingStockFirst()
        {
            CardPile stock = new();
            TableauColumn[] tableau = new TableauColumn[7];
            InitializeTableau(tableau);
            Game g = new(stock, tableau, 1);
            CardPile[] foundation = new CardPile[4];
            InitializeFoundation(foundation);
            DiscardPile dis = new();

            // Play most of the game, testing various actions.
            PlayGame(stock, tableau, g, foundation, dis);

            // 4 of Spades on 5 of Diamonds
            g.SelectDiscard(dis);
            g.SelectTableauCards(tableau[0], 1);

            // Draw
            g.DrawCardsFromStock(stock, dis);

            // 6 of Hearts on 7 of Clubs
            g.SelectDiscard(dis);
            Assert.That(g.SelectTableauCards(tableau[6], 1), Is.False,
                "Moving the next-to-last card from the stock shouldn't win.");

            // 5 of Clubs on 6 of Hearts
            g.SelectDiscard(dis);
            Assert.That(g.SelectTableauCards(tableau[6], 1), Is.False,
                "Moving the last card from the stock shouldn't win.");

            // 7 of Clubs - 5 of Clubs on 8 of Diamonds
            g.SelectTableauCards(tableau[6], 3);
            Assert.That(g.SelectTableauCards(tableau[4], 1), Is.True,
                "Flipping the last face-down tableau card should win.");
        }

        /// <summary>
        /// Tests a complete winning game with seed 1. This game is won by first flipping all
        /// face-down tableau cards, then bringing the stock down to a single card.
        /// </summary>
        [Test]
        [Timeout(5000), Category("D: Complex Actions")]
        public void TestWinByFlippingTableauCardsFirst()
        {
            CardPile stock = new();
            TableauColumn[] tableau = new TableauColumn[7];
            InitializeTableau(tableau);
            Game g = new(stock, tableau, 1);
            CardPile[] foundation = new CardPile[4];
            InitializeFoundation(foundation);
            DiscardPile dis = new();

            // Play most of the game, testing various actions.
            PlayGame(stock, tableau, g, foundation, dis);

            // 7 of Clubs on 8 of Diamonds, flips Ace of Hearts
            g.SelectTableauCards(tableau[6], 1);
            Assert.That(g.SelectTableauCards(tableau[4], 1), Is.False,
                "Flipping the last face-down card shouldn't win.");

            // 4 of Spades on 5 of Diamonds
            g.SelectDiscard(dis);
            g.SelectTableauCards(tableau[0], 1);

            // Ace of Hearts on foundation 2
            g.SelectTableauCards(tableau[6], 1);
            g.SelectFoundationPile(foundation[2].Pile);

            // 2 of Hearts on Ace of Hearts
            g.SelectTableauCards(tableau[3], 1);
            g.SelectFoundationPile(foundation[2].Pile);

            // 3 of Hearts on 2 of Hearts
            g.SelectTableauCards(tableau[5], 1);
            g.SelectFoundationPile(foundation[2].Pile);

            // 4 of Clubs on 3 of Clubs
            g.SelectTableauCards(tableau[5], 1);
            Assert.That(g.SelectFoundationPile(foundation[1].Pile), Is.False,
                "The next-to-last move doesn't win.");

            // 5 of Clubs on 4 of Clubs
            g.SelectDiscard(dis);
            Assert.That(g.SelectFoundationPile(foundation[1].Pile), Is.True,
                "The last move wins.");
        }
    }
}
