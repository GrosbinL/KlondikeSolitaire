/* UserInterface.cs
 * Author: Grosbin Orellana Luna
 */
namespace Grosbin.Games.KlondikeSolitaire
{
    /// <summary>
    /// A GUI for a Klondike Solitaire game.
    /// </summary>
    public partial class UserInterface : Form
    {
        /// <summary>
        /// The number of foundation piles.
        /// </summary>
        private const int _numberOfFoundationPiles = 4;

        /// <summary>
        /// The number of tableau columns.
        /// </summary>
        private const int _numberOfTableauColumns = 7;

        /// <summary>
        /// The stock.
        /// </summary>
        private readonly CardPile _stock = new();

        /// <summary>
        /// The discard pile.
        /// </summary>
        private readonly DiscardPile _discardPile = new();

        /// <summary>
        /// The foundation piles.
        /// </summary>
        private readonly CardPile[] _foundation = new CardPile[_numberOfFoundationPiles];

        /// <summary>
        /// The tableau columns.
        /// </summary>
        private readonly TableauColumn[] _tableauColumns = new TableauColumn[_numberOfTableauColumns];

        /// <summary>
        /// The game controller.
        /// </summary>
        private Game? _game;

        /// <summary>
        /// The dialog for setting the seed.
        /// </summary>
        private readonly SeedDialog _seedDialog = new();

        /// <summary>
        /// Constructs the GUI.
        /// </summary>
        public UserInterface()
        {
            InitializeComponent();

            // Add controls and event handlers.

            // uxStockFoundationPanel is a FlowLayoutPanel set to fill from left to right.
            // It will contain the stock, discard pile, and the foundation.
            _stock.IsFaceUp = false;
            uxStockFoundationPanel.Controls.Add(_stock);
            _stock.Click += new EventHandler(StockClick);
            uxStockFoundationPanel.Controls.Add(_discardPile);
            _discardPile.MouseClick += new MouseEventHandler(DiscardPileMouseClick);
            for (int i = 0; i < _foundation.Length; i++)
            {
                _foundation[i] = new CardPile();
                _foundation[i].IsFaceUp = true;
                uxStockFoundationPanel.Controls.Add(_foundation[i]);
                _foundation[i].MouseClick += new MouseEventHandler(FoundationMouseClick);
            }

            // uxTableauPanel is another FlowLayoutPanel beneath uxStockFoundationPanel.
            // It will contain the tableau.
            for (int i = 0; i < _tableauColumns.Length; i++)
            {
                _tableauColumns[i] = new TableauColumn();
                uxTableauPanel.Controls.Add(_tableauColumns[i]);
                _tableauColumns[i].MouseClick += new MouseEventHandler(TableauColumnMouseClick);
            }

            // Correct spacing between the discard pile and the foundation piles so that the
            // right edges of the last foundation pile and the last tableau column align.
            int diff = uxStockFoundationPanel.Width - uxTableauPanel.Width;
            Padding margin = _discardPile.Margin;
            margin.Right -= diff;
            _discardPile.Margin = margin;
        }

        /// <summary>
        /// Clears the board.
        /// </summary>
        private void ClearBoard()
        {
            _stock.Pile.Clear();
            _discardPile.IsSelected = false;
            _discardPile.Pile.Clear();
            for (int i = 0; i < _foundation.Length; i++)
            {
                _foundation[i].Pile.Clear();
            }
            for (int i = 0; i < _tableauColumns.Length; i++)
            {
                _tableauColumns[i].NumberSelected = 0;
                _tableauColumns[i].FaceDownPile.Clear();
                _tableauColumns[i].FaceUpPile.Clear();
            }
        }

        /// <summary>
        /// Handles a Click event on the "New Game" button.
        /// </summary>
        /// <param name="sender">The object signaling the event.</param>
        /// <param name="e">Information about the event.</param>
        private void NewClick(object sender, EventArgs e)
        {
            ClearBoard();
            _game = new Game(_stock, _tableauColumns, Convert.ToInt32(uxSeed.Text));
            uxBoard.Enabled = true;
            Refresh();
        }

        /// <summary>
        /// Handles a Click event on the stock.
        /// </summary>
        /// <param name="sender">The object signaling the event.</param>
        /// <param name="e">Information about the event.</param>
        private void StockClick(object? sender, EventArgs e)
        {
            if (_game != null)
            {
                _game.DrawCardsFromStock(_stock, _discardPile);
                Refresh();
            }
        }

        /// <summary>
        /// Handles a MouseClick event on the discard pile.
        /// </summary>
        /// <param name="sender">The object signaling the event.</param>
        /// <param name="e">Data regarding the mouse click.</param>
        private void DiscardPileMouseClick(object? sender, MouseEventArgs e)
        {
            // We only handle clicks that are on the top card. e.X gives the horizontal distance
            // from the edge of the discard pile to the click location.
            if (_game != null && _discardPile.IsOnTopCard(e.X))
            {
                _game.SelectDiscard(_discardPile);
                Refresh();
            }
        }

        /// <summary>
        /// Ends the game.
        /// </summary>
        private void EndGame()
        {
            Refresh();
            MessageBox.Show("You win!");
            uxBoard.Enabled = false;
        }

        /// <summary>
        /// Handles a MouseClick event on a tableau column.
        /// </summary>
        /// <param name="sender">The TableauColumn that was clicked.</param>
        /// <param name="e">Data regarding the mouse click.</param>
        private void TableauColumnMouseClick(object? sender, MouseEventArgs e)
        {
            if (_game != null)
            {
                // The object signaling the event can't be null.
                TableauColumn col = (TableauColumn)sender!;

                // We only handle clicks that are on a card or an empty column.
                // e.Y gives the vertical distance from the top of the control to the click location.
                int n = col.NumberAbove(e.Y);
                if (n > 0 || (n == 0 && col.FaceUpPile.Count == 0))
                {
                    if (_game.SelectTableauCards(col, n))
                    {
                        EndGame();
                    }
                    Refresh();
                }
            }
        }

        /// <summary>
        /// Handles a MouseClick event on a foundation pile.
        /// </summary>
        /// <param name="sender">The CardPile that was clicked.</param>
        /// <param name="e">Information about the event.</param>
        private void FoundationMouseClick(object? sender, MouseEventArgs e)
        {
            // The object signaling the event can't be null.
            if (_game != null && _game.SelectFoundationPile(((CardPile)sender!).Pile))
            {
                EndGame();
            }
            Refresh();
        }

        /// <summary>
        /// Handles a Click event on the "Seed:" button.
        /// </summary>
        /// <param name="sender">The object signaling the event.</param>
        /// <param name="e">Information about the event.</param>
        private void GetSeedClick(object sender, EventArgs e)
        {
            _seedDialog.Seed = Convert.ToInt32(uxSeed.Text);
            if (_seedDialog.ShowDialog() == DialogResult.OK)
            {
                uxSeed.Text = _seedDialog.Seed.ToString();
            }
        }
    }
}
