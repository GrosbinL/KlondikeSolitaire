/* TableauQueue.cs
 * Author: Grosbin Orellana Luna
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grosbin.Games.KlondikeSolitaire
{
    /// <summary>
    /// A queue of Cards that supports peeking at both ends.
    /// </summary>
    public class TableauQueue
    {
        /// <summary>
        /// The rank of the initial card stored as the back of the queue.
        /// This card won't be used.
        /// </summary>
        private const int _defaultRank = 1;

        /// <summary>
        /// The queue.
        /// </summary>
        private readonly Queue<Card> _queue = new();

        /// <summary>
        /// The card at the back of the queue.
        /// If the queue is empty, this value is unused.
        /// </summary>
        private Card _backCard = new(_defaultRank, Suit.Spades);

        /// <summary>
        /// Gets the number of cards in the queue.
        /// </summary>
        public int Count => _queue.Count;

        /// <summary>
        /// Adds the given card to the back of the queue.
        /// </summary>
        /// <param name="c">The card to enqueue.</param>
        public void Enqueue(Card c)
        {
            _queue.Enqueue(c);
            _backCard = c;
        }

        /// <summary>
        /// Returns the card at the front of the queue.
        /// If the queue is empty, throws an InvalidOperationException.
        /// </summary>
        /// <returns>The card at the front of the queue.</returns>
        public Card PeekFront()
        {
            if (_queue.Count == 0)
            {
                throw new InvalidOperationException();
            }
            else
            {
                return _queue.Peek();
            }
        }

        /// <summary>
        /// Returns the card at the back of the queue.
        /// If the queue is empty, throws an InvalidOperationException.
        /// </summary>
        /// <returns>The card at the back of the queue.</returns>
        public Card PeekBack()
        {
            if (_queue.Count == 0)
            {
                throw new InvalidOperationException();
            }
            else
            {
                return _backCard;
            }
            
        }

        /// <summary>
        /// Removes the card from the front of the queue.
        /// If the queue is empty, throws an InvalidOperationException.
        /// </summary>
        /// <returns>The card removed.</returns>
        public Card Dequeue()
        {

            if (_queue.Count == 0)
            {
                throw new InvalidOperationException();
            }
            else
            {
                return _queue.Dequeue();
            }         
        }

        /// <summary>
        /// Returns the elements of the queue, from front to back, in an array.
        /// </summary>
        /// <returns>The elements of the queue, from front to back.</returns>
        public Card[] ToArray()
        {
            Card[] cardArray = _queue.ToArray();
            return cardArray;
        }

        /// <summary>
        /// Clears the queue.
        /// </summary>
        public void Clear()
        {
            _queue.Clear();
        }
    }
}
