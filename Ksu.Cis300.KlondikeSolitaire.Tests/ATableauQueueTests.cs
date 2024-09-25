/* ATableauQueueTests.cs
 * Author: Rod Howell
 */
using System.Reflection;

namespace Grosbin.Games.KlondikeSolitaire.Tests
{
    /// <summary>
    /// Unit tests for the TableauQueue class.
    /// </summary>
    public class ATableauQueueTests
    {
        /// <summary>
        /// Tests that Count is defined as a read-only property.
        /// </summary>
        [Test]
        [Timeout(1000), Category("A: Property")]
        public void TestCountIsProperty()
        {
            Type type = typeof(TableauQueue);
            PropertyInfo? info = type.GetProperty("Count");
            Assert.That(info, Is.Not.Null, "Count is not defined as a property.");
            Assert.That(info.CanWrite, Is.False, "Count must not contain a set accessor.");
        }

        /// <summary>
        /// Tests that an empty queue has the correct Count.
        /// </summary>
        [Test]
        [Timeout(1000), Category("B: Empty Queue")]
        public void TestEmptyCount()
        {
            TableauQueue q = new();
            Assert.That(q, Has.Count.EqualTo(0));
        }

        /// <summary>
        /// Tests peeking/dequeueing an empty queue.
        /// </summary>
        [Test]
        [Timeout(1000), Category("B: Empty Queue")]
        public void TestEmptyAccess()
        {
            TableauQueue q = new();
            Assert.Multiple(() =>
            {
                Assert.Throws<InvalidOperationException>(() => q.PeekBack(),
                    "PeekBack should throw an InvalidOperationException.");
                Assert.Throws<InvalidOperationException>(() => q.PeekFront(),
                    "PeekFront should throw an InvalidOperationException.");
                Assert.Throws<InvalidOperationException>(() => q.Dequeue(),
                    "Dequeue should throw an InvalidOperationException.");
            });
        }

        /// <summary>
        /// Tests that ToArray returns an empty array when the queue is empty.
        /// </summary>
        [Test]
        [Timeout(1000), Category("B: Empty Queue")]
        public void TestEmptyToArray()
        {
            TableauQueue q = new();
            Card[] a = q.ToArray();
            Assert.That(a, Has.Length.EqualTo(0));
        }

        /// <summary>
        /// Tests an Enqueue followed by a Dequeue.
        /// </summary>
        [Test]
        [Timeout(1000), Category("C: Single Enqueue")]
        public void TestSingleEnqueueDequeue()
        {
            TableauQueue q = new();
            Card c = new(2, Suit.Diamonds);
            Card[] a = { c };

            // Enqueue card
            q.Enqueue(c);
            Assert.Multiple(() =>
            {
                Assert.That(q, Has.Count.EqualTo(1),
                    "Count after Enqueue should be 1.");
                Assert.That(q.PeekBack(), Is.EqualTo(c),
                    "PeekBack should return the 2 of Diamonds.");
                Assert.That(q, Has.Count.EqualTo(1),
                    "Count after PeekBack should remain 1.");
                Assert.That(q.PeekFront(), Is.EqualTo(c),
                    "PeekFront should return the 2 of Diamonds.");
                Assert.That(q, Has.Count.EqualTo(1),
                    "Count after PeekFront should remain 1.");
                Assert.That(q.ToArray(), Is.EqualTo(a),
                    "ToArray should return an array containing only the 2 of Diamonds.");
            });

            // Dequeue card
            Assert.Multiple(() =>
            {
                Assert.That(q.Dequeue(), Is.EqualTo(c),
                    "Dequeue should return the 2 of Diamonds.");
                Assert.That(q, Has.Count.EqualTo(0),
                    "Count after Dequeue should be 0.");
            });
        }

        /// <summary>
        /// Enqueues the cards in the given array onto the given queue.
        /// </summary>
        /// <param name="a">The cards to enqueue.</param>
        /// <param name="q">The queue.</param>
        private static void EnqueueCards(Card[] a, TableauQueue q)
        {
            foreach (Card c in a)
            {
                q.Enqueue(c);
            }
        }

        /// <summary>
        /// Tests enqueuing, then dequeueing, a sequence of three cards.
        /// </summary>
        [Test]
        [Timeout(1000), Category("D: Multiple Elements")]
        public void TestMultipleEnqueueDequeue()
        {
            TableauQueue q = new();
            Card[] a = { new(1, Suit.Spades), new(13, Suit.Hearts), new(11, Suit.Clubs) };

            // Enqueue 3 cards.
            EnqueueCards(a, q);
            Assert.Multiple(() =>
            {
                Assert.That(q, Has.Count.EqualTo(3),
                    "The Count should be 3 after all Enqueues.");
                Assert.That(q.PeekFront(), Is.EqualTo(a[0]),
                    "PeekFront should return the Ace of Spades after all Enqueues.");
                Assert.That(q.PeekBack(), Is.EqualTo(a[2]),
                    "PeekBack should return the Jack of Clubs after all Enqueues.");
                Assert.That(q.ToArray(), Is.EqualTo(a),
                    "ToArray returns the wrong array.");
            });

            // Dequeue first card.
            Assert.Multiple(() =>
            {
                Assert.That(q.Dequeue(), Is.EqualTo(a[0]),
                    "The first Dequeue should return the Ace of Spades.");
                Assert.That(q, Has.Count.EqualTo(2),
                    "The Count should be 2 after the first Dequeue.");
                Assert.That(q.PeekFront(), Is.EqualTo(a[1]),
                    "PeekFront should return the King of Hearts after the first Dequeue.");
                Assert.That(q.PeekBack(), Is.EqualTo(a[2]),
                    "PeekBack should return the Jack of Clubs after the first Dequeue.");
            });

            // Dequeue second card
            Assert.Multiple(() =>
            {
                Assert.That(q.Dequeue(), Is.EqualTo(a[1]),
                    "The second Dequeue should return the King of Hearts.");
                Assert.That(q, Has.Count.EqualTo(1),
                    "The Count should be 1 after the second Dequeue.");
                Assert.That(q.PeekBack(), Is.EqualTo(a[2]),
                    "PeekBack should return the Jack of Clubs after the second Dequeue.");
            });

            // Dequeue third card
            Assert.Multiple(() =>
            {
                Assert.That(q.Dequeue(), Is.EqualTo(a[2]),
                    "The third Dequeue should return the Jack of Clubs.");
                Assert.That(q, Has.Count.EqualTo(0),
                    "The Count should be 0 after the third Dequeue.");
                Assert.Throws<InvalidOperationException>(() => q.PeekFront(),
                    "PeekFront should throw an InvalidOperationException after the third Dequeue.");
                Assert.Throws<InvalidOperationException>(() => q.PeekBack(),
                    "PeekBack should throw an InvalidOperationException after the third Dequeue.");
                Assert.Throws<InvalidOperationException>(() => q.Dequeue(),
                    "The fourth Dequeue should throw an InvalidOperationException.");
            });
        }

        /// <summary>
        /// Tests Clear.
        /// </summary>
        [Test]
        [Timeout(1000), Category("D: Multiple Elements")]
        public void TestClear()
        {
            TableauQueue q = new();
            Card[] a = { new(5, Suit.Diamonds), new(12, Suit.Hearts), new(10, Suit.Spades) };

            // Enqueue 3 cards, then Clear
            EnqueueCards(a, q);
            q.Clear();
            Assert.Multiple(() =>
            {
                Assert.That(q, Has.Count.EqualTo(0),
                    "The Count should be 0 after the Clear.");
                Assert.That(q.ToArray(), Has.Length.EqualTo(0),
                    "ToArray should return an empty array after the Clear.");
                Assert.Throws<InvalidOperationException>(() => q.PeekFront(),
                    "PeekFront should throw an InvalidOperationException after the Clear.");
                Assert.Throws<InvalidOperationException>(() => q.PeekBack(),
                    "PeekBack should throw an InvalidOperationException after the Clear.");
                Assert.Throws<InvalidOperationException>(() => q.Dequeue(),
                    "Dequeue should throw an InvalidOperationException after the Clear.");
            });

            // Enqueue the 3 cards again
            EnqueueCards(a, q);
            Assert.Multiple(() =>
            {
                Assert.That(q, Has.Count.EqualTo(3),
                    "The Count should be 3 after new cards are enqueued.");
                Assert.That(q.ToArray(), Is.EqualTo(a),
                    "ToArray returns the wrong array after the last 3 Enqueues.");
            });
        }
    }
}