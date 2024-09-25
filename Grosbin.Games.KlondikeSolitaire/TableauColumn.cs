/* TableauColumn.cs
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
    /// A control representing a column on the tableau
    /// </summary>
    public partial class TableauColumn : UserControl
    {
        /// <summary>
        /// The vertical offset of each face down card.
        /// </summary>
        private const int _faceDownOffset = 2;

        /// <summary>
        /// The vertical offset of each face up card.
        /// </summary>
        private const int _faceUpOffset = CardPainter.CardHeight / 5;

        /// <summary>
        /// The total vertical offset of the face up card pile.
        /// </summary>
        private const int _faceDownHeight = 6 * _faceDownOffset;

        /// <summary>
        /// The maximum number of covered face-up cards in a column.
        /// </summary>
        private const int _maxCovered = 12;

        /// <summary>
        /// The padding around the card images.
        /// </summary>
        private const int _padding = 1;

        /// <summary>
        /// Gets the face down pile.
        /// </summary>
        public Stack<Card> FaceDownPile { get; } = new();

        /// <summary>
        /// Gets the face up pile.
        /// </summary>
        public TableauQueue FaceUpPile { get; } = new();

        /// <summary>
        /// The number of cards selected.
        /// </summary>
        private int _numberSelected;

        /// <summary>
        /// Gets or sets the number of cards selected.
        /// If a negative value or a value greater than the number of face-up
        /// cards is assigned, an ArgumentOutOfRangeException is thrown.
        /// </summary>
        public int NumberSelected
        {
            get
            {
                return _numberSelected;
            }
            set
            {
                if (value < 0 || value > FaceUpPile.Count)
                {
                    throw new ArgumentOutOfRangeException();
                }
                _numberSelected = value;
            }
        }

        /// <summary>
        /// Constructs the control.
        /// </summary>
        public TableauColumn()
        {
            InitializeComponent();
            Width = CardPainter.CardWidth + _padding;
            Height = _faceDownHeight + _maxCovered * _faceUpOffset + CardPainter.CardHeight + _padding;
        }

        /// <summary>
        /// Finds the number of face-up cards on top of the given y-coordinate, provided this
        /// coordinate is on a face-up card; otherwise, returns 0.
        /// </summary>
        /// <param name="y">The y-coordinate.</param>
        /// <returns>The number of face-up cards on top of y, or 0 if y is not on a face-up
        /// card.</returns>
        public int NumberAbove(int y)
        {
            if (FaceUpPile.Count == 0 || y < _faceDownHeight ||
                y > _faceDownHeight + (FaceUpPile.Count - 1) * _faceUpOffset + CardPainter.CardHeight)
            {
                return 0;
            }
            else if (y >= _faceDownHeight + FaceUpPile.Count * _faceUpOffset)
            {
                return 1;
            }
            return FaceUpPile.Count - (y - _faceDownHeight) / _faceUpOffset;
        }

        /// <summary>
        /// Draws the control.
        /// </summary>
        /// <param name="e">Data concerning the graphics environment.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            // This method redefines the OnPaint method defined within the UserControl class,
            // which is the super-type (or parent) of this class. The following line ensures
            // that everything done by the overridden method is done here.
            base.OnPaint(e);

            Graphics g = e.Graphics;
            CardPainter.DrawBox(g, 0, 0);
            int y = 0;
            for (int i = 0; i < FaceDownPile.Count; i++)
            {
                CardPainter.DrawBack(g, 0, y);
                y += _faceDownOffset;
            }
            y = _faceDownHeight;
            Card[] a = FaceUpPile.ToArray();
            foreach (Card c in a)
            {
                CardPainter.DrawCard(c, g, 0, y);
                y += _faceUpOffset;
            }
            if (_numberSelected > 0)
            {
                int boxHeight = CardPainter.CardHeight + (_numberSelected - 1) * _faceUpOffset;
                int boxStart = _faceDownHeight + (FaceUpPile.Count - _numberSelected) * _faceUpOffset;
                g.DrawRectangle(CardPainter.HighlightPen, 0, boxStart, CardPainter.CardWidth, boxHeight);
            }
        }
    }
}
