/* DiscardPile.cs
 * Author: Grosbin Orellana Luna
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Grosbin.Games.KlondikeSolitaire
{
    /// <summary>
    /// A user control displaying the discard pile.
    /// </summary>
    public partial class DiscardPile : UserControl
    {
        /// <summary>
        /// The horizontal offset for the top three cards.
        /// </summary>
        private const int _cardOffset = CardPainter.CardWidth / 4;

        /// <summary>
        /// The maximum number of cards that can be displayed below the top card.
        /// </summary>
        private const int _maxPartiallyCovered = 2;

        /// <summary>
        /// The padding around the card images.
        /// </summary>
        private const int _padding = 1;

        /// <summary>
        /// Gets the cards in the discard pile.
        /// </summary>
        public Stack<Card> Pile { get; } = new();

        /// <summary>
        /// Indicates whether the top card of this pile is selected.
        /// </summary>
        private bool _isSelected;

        /// <summary>
        /// Gets or sets whether the top card of this pile is selected.
        /// If the discard pile is empty, attempting to set this value
        /// to true will throw an InvalidOperationException.
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                if (Pile.Count == 0 && value)
                {
                    throw new InvalidOperationException();
                }
                _isSelected = value;
            }
        }

        /// <summary>
        /// Constructs the control.
        /// </summary>
        public DiscardPile()
        {
            InitializeComponent();
            Width = CardPainter.CardWidth + _maxPartiallyCovered * _cardOffset + _padding;
            Height = CardPainter.CardHeight + _padding;
        }

        /// <summary>
        /// Gets the horizontal offset of the top card. This value will be negative if the
        /// pile is empty.
        /// </summary>
        /// <returns>The horizontal offset of the top card.</returns>
        private int TopCardOffset()
        {
            return Math.Min(Pile.Count - 1, _maxPartiallyCovered) * _cardOffset;
        }

        /// <summary>
        /// Draws the control.
        /// </summary>
        /// <param name="e">Data about the drawing context.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            // This method redefines the OnPaint method defined within the UserControl class,
            // which is the super-type (or parent) of this class. The following line ensures
            // that everything done by the overridden method is done here.
            base.OnPaint(e);

            Graphics g = e.Graphics;
            int x = 0;
            Card[] a = Pile.ToArray();
            for (int i = Math.Min(a.Length - 1, _maxPartiallyCovered); i >= 0; i--)
            {
                CardPainter.DrawCard(a[i], g, x, 0);
                x += _cardOffset;
            }
            if (_isSelected)
            {
                x = TopCardOffset();
                g.DrawRectangle(CardPainter.HighlightPen, x, 0, CardPainter.CardWidth, CardPainter.CardHeight);
            }
        }

        /// <summary>
        /// Determines whether the given x-coordinate is on the top card in the pile, relative
        /// to the left edge of the control.
        /// </summary>
        /// <param name="x">The x-coordinate.</param>
        /// <returns>Whether x-coordinate is on the top card.</returns>
        public bool IsOnTopCard(int x)
        {
            int off = TopCardOffset();
            return Pile.Count > 0 && x >= off && x <= off + CardPainter.CardWidth;
        }
    }
}
