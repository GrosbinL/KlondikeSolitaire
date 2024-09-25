/* Card.cs
 * Author: Grosbin Orellana Luna
 */
using System.ComponentModel;
using System.Reflection;

namespace Grosbin.Games.KlondikeSolitaire
{
    /// <summary>
    /// An immutable representation of a single card.
    /// </summary>
    public class Card
    {
        /// <summary>
        /// The minimum rank of a card.
        /// </summary>
        public static readonly int MinRank = 1;

        /// <summary>
        /// The maximum rank of a card;
        /// </summary>
        public static readonly int MaxRank = 13;

        /// <summary>
        /// The minimum suit, as an int.
        /// </summary>
        public static readonly int MinSuit = 0;

        /// <summary>
        /// The maximum suit, as an int.
        /// </summary>
        public static readonly int MaxSuit = 3;

        /// <summary>
        /// Gets the rank of the card.
        /// </summary>
        public int Rank { get; }

        /// <summary>
        /// Gets the suit of the card.
        /// </summary>
        public Suit Suit { get; }

        /// <summary>
        /// Indicates whether this card is red.
        /// </summary>
        public bool IsRed { get; }

        /// <summary>
        /// The image of this card.
        /// </summary>
        public Image Picture { get; }

        /// <summary>
        /// Constructs a new card representing the given rank and suit.
        /// </summary>
        /// <param name="rank">The rank of the card.</param>
        /// <param name="suit">The suit of the card.</param>
        public Card(int rank, Suit suit)
        {
            if (rank < MinRank || rank > MaxRank || (int)suit < MinSuit || (int)suit > MaxSuit)
            {
                throw new ArgumentException();
            }
            Rank = rank;
            Suit = suit;
            if (suit == Suit.Diamonds || suit == Suit.Hearts)
            {
                IsRed = true;
            }

            System.Resources.ResourceManager rm = Images.ResourceManager;

            // All names formed by concatenating a file prefix with a rank are stored as resources
            // within Images; hence, GetObject will not return null.
            Picture = (Image)rm.GetObject(suit.ToString() + rank)!;
        }

        /// <summary>
        /// Returns a string representation of the card.
        /// </summary>
        /// <returns>A string representation of the card.</returns>
        public override string ToString()
        {
            switch(Rank)
            {
                case 1:
                    return "1 (Ace) of " + Suit;
                case 11:
                    return "11 (Jack) of " + Suit;
                case 12:
                    return "12 (Queen) of " + Suit;
                case 13:
                    return "13 (King) of " + Suit;
                default:
            return Rank + " of " + Suit;
            }
        }

        /// <summary>
        /// Compares the given cards for equality (same rank and suit).
        /// </summary>
        /// <param name="x">The first card.</param>
        /// <param name="y">The second card.</param>
        /// <returns>Whether the given cards are equal.</returns>
        public static bool operator ==(Card? x, Card? y)
        {
            if (Equals(x, null))
            {
                return Equals(y, null);
            }
            else if (Equals(y, null))
            {
                return false;
            }
            return x.Rank == y.Rank && x.Suit == y.Suit;
        }

        /// <summary>
        /// Compares the given cards for inequality (different rank or suit).
        /// </summary>
        /// <param name="x">The first card.</param>
        /// <param name="y">The second card.</param>
        /// <returns>Whether the given cards are different.</returns>
        public static bool operator !=(Card? x, Card? y)
        {
            return !(x == y);
        }

        /// <summary>
        /// Compares the given object for equality with this Card; i.e., whether the given object
        /// is a Card with the same rank and suit.
        /// </summary>
        /// <param name="obj">The object to compare with this Card.</param>
        /// <returns>Whether the given object is the same as this Card.</returns>
        public override bool Equals(object? obj)
        {
            return this == obj as Card;
        }

        /// <summary>
        /// Gets the hash code for this card.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            return 37 * (int)Suit + Rank;
        }
    }
}
